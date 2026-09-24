// snake.cs.dso
if (!(isObject(BeSnake)))
{
	%template = new BehaviorTemplate(Name : BeSnake);
	%template.friendlyName = "Snake";
	%template.behaviorType = "LevelTrip";
	%template.description = "user friendly description of the behavior";
	%template.addBehaviorField(timeTillRetreat, "how long shall it take, till the snake retreats again (s)", float, "1");
}
function BeSnake::onBehaviorAdd(%this)
{
	subscribeToEvent(%this, "onLevelLoadFinished10");
	return;
}
function BeSnake::onLevelLoadFinished10(%this)
{
	%owner = Owner;
	Owner.escapeSwitch = "1";
	return;
}
function BeSnake::switchOn(%this)
{
	%this.bite();
	return;
}
function BeSnake::bite(%this)
{
	%rotateBehavior = Snake.getBehavior("BeRotate");
	%biteDelay = time;
	%rotateBehavior.pauseOnRotation = "0";
	%rotateBehavior.play();
	%translateBehavior = Snake.getBehavior("BeTranslate");
	%translateBehavior.pauseOnRotation = "0";
	%translateBehavior.switchOn();
	safeSchedule(%biteDelay * 1000.0, player, "dieSnakeBite");
	safeSchedule(timeTillRetreat * 1000.0, %this, "retreat");
	playEventSound(SnakeBite, "1");
	curPlaySoundBehavior.start();
	return;
}
function BeSnake::onCollisionReceive(%this)
{
	return;
}
function BeSnake::retreat(%this)
{
	%rotateBehavior = Snake.getBehavior("BeRotate");
	%rotateBehavior.play("1");
	%translateBehavior = Snake.getBehavior("BeTranslate");
	%translateBehavior.distanceVector = reverseVector(distanceVector);
	%translateBehavior.init();
	%translateBehavior.play();
	snakeTenseSound.getBehavior("BePlaySound").forcedFadeOut("4");
	safeSchedule("4200", snakeTenseSound, "safeDelete");
	if (isObject(schwindelMelody))
	{
		schwindelMelody.schedule("500", "enterSoundArea");
	}
	return;
}
function player::startVisualPoisoning(%this)
{
	if (isPlayer(%this))
	{
		%this.poisonTex = playerPoisonTex;
	}
	else
	{
		if (!(isObject(ghostPoisonTex)))
		{
			%this.poisonTex = new t2dStaticSprite(Name : ghostPoisonTex)
			{
				scenegraph = scenegraph;
				size = size;
				imageMap = imageMap;
				Layer = Layer;
				BlendColor = BlendColor;
				AngularVelocity = AngularVelocity;
				Visible = "0";
			}
		}
	}
	poisonTex.mount(%this, "0 0", "3", "0", "0", "0");
	poisonTex.setAngularVelocity("45");
	%this.setTexObjectName(poisonTex);
	%this.setUsePositive("1");
	%this.setRenderTexture("1");
	%this.setRepeatTexture("1");
	%this.setTransformTexture("1");
	poisonTex.setBlendAlpha("0");
	%alphaFadeTime = 60;
	poisonTex.setAlphaVelocity(0.949999988079071 / %alphaFadeTime);
	%this.startSchwindelCamera();
	schedule(%alphaFadeTime * 1000.0, "0", "player::endDieSnakeBite", %this);
	return;
}
function player::endDieSnakeBite(%this)
{
	poisonTex.setBlendAlpha("0.95");
	poisonTex.setAlphaVelocity("0");
	return;
}
function playerPoisonTex::ownerSwitchOff(%this)
{
	%this.startSwitchOff();
	if (isObject(ghostPoisonTex))
	{
		ghostPoisonTex.startSwitchOff();
	}
	return;
}
function poisonTex::startSwitchOff(%this)
{
	%alphaFadeTime = 30;
	%this.setAlphaVelocity(-0.949999988079071 / %alphaFadeTime);
	%this.schedule(%alphaFadeTime * 1000.0, "endSwitchOff");
	return;
}
function poisonTex::endSwitchOff(%this)
{
	%this.setAlphaVelocity("0");
	%this.setBlendAlpha("0");
	return;
}
function player::startSchwindelCamera(%this)
{
	%controller = playerPoisonMask;
	%controller.schwindelDuration = "30";
	%controller.schwindelStartTime = thisTime;
	%controller.rotationTargetVariance = "30";
	%controller.rotationDurationMean = "1.5";
	%controller.rotationDurationVariance = "0.01";
	%controller.lastRotationTarget = "0";
	%controller.isFinalRotation = "0";
	%controller.rotSmoother = Smoother::createInstance();
	levelGarbageCollector.add(rotSmoother);
	subscribeToEvents(%controller, "onUpdateFrame");
	%controller.startRotation();
	%controller.zoomTargetMean = "1.3";
	%controller.zoomTargetVariance = "0.4";
	%controller.zoomDurationMean = "1.2";
	%controller.zoomDurationVariance = "0.4";
	%controller.lastZoomTarget = "0";
	%controller.isFinalZoom = "0";
	%controller.zoomSmoother = Smoother::createInstance();
	levelGarbageCollector.add(zoomSmoother);
	subscribeToEvents(%controller, "onUpdateFrame30 onZoomTriggerLeave");
	%controller.startZoom();
	return;
}
function playerPoisonMask::startRotation(%this)
{
	if (isFinalRotation)
	{
		%target = 0;
	}
	else
	{
		if (rotationTargetVariance > 2.0)
		{
			%this.rotationTargetVariance = rotationTargetVariance - 1.0;
		}
		%target = floatRandom("0", rotationTargetVariance);
		if (lastRotationTarget < 0.0)
		{
		}
		else
		{
		}
		%target = %target * -1.0;
	}
	%duration = floatRandom(rotationDurationMean - rotationDurationVariance, rotationDurationMean + rotationDurationVariance);
	rotSmoother.init(%duration, lastRotationTarget, %target, "SMOOTH", "1");
	rotSmoother.start();
	return;
}
function playerPoisonMask::onUpdateFrame(%this)
{
	camera.schwindelOffset = rotSmoother.getValue();
	camera.setCameraRotation(camera.getCurrentRotation());
	if (rotSmoother.getIsFinished())
	{
		%this.lastRotationTarget = schwindelOffset;
		if (thisTime < schwindelStartTime + schwindelDuration)
		{
			%this.startRotation();
			break;
		}
		if (!(isFinalRotation))
		{
			%this.isFinalRotation = "1";
			%this.startRotation();
			break;
		}
		if (isObject(schwindelMelody))
		{
			schwindelMelody.fadeDuration = "6";
			schwindelMelody.leaveSoundArea();
		}
		camera.schwindelOffset = "0";
		unSubscribeFromEvent(%this, "onUpdateFrame");
	}
	return;
}
function playerPoisonMask::startZoom(%this)
{
	if (isFinalZoom)
	{
		%target = 0;
	}
	else
	{
		if (lastZoomTarget > zoomTargetMean)
		{
			%target = floatRandom(zoomTargetMean - zoomTargetVariance, zoomTargetMean);
			break;
		}
		%target = floatRandom(zoomTargetMean, zoomTargetMean + zoomTargetVariance);
	}
	%duration = floatRandom(zoomDurationMean - zoomDurationVariance, zoomDurationMean + zoomDurationVariance);
	zoomSmoother.init(%duration, lastZoomTarget, %target, "SMOOTH", "1");
	zoomSmoother.start();
	if (zoomTargetMean > 0.10000000149011612)
	{
		%this.zoomTargetMean = zoomTargetMean - 0.07000000029802322;
		%this.zoomTargetVariance = min(zoomTargetMean, zoomTargetVariance - 0.019999999552965164);
	}
	return;
}
function playerPoisonMask::onUpdateFrame30(%this)
{
	%this.curZoom = zoomSmoother.getValue();
	sceneWindow2d.setCurrentCameraZoom(camera.getZoom() + curZoom);
	if (zoomSmoother.getIsFinished())
	{
		%this.lastZoomTarget = curZoom;
		if (thisTime < schwindelStartTime + schwindelDuration)
		{
			%this.startZoom();
			break;
		}
		if (!(isFinalZoom))
		{
			%this.isFinalZoom = "1";
			%this.startZoom();
			break;
		}
		unSubscribeFromEvents(%this, "onUpdateFrame30 onZoomTriggerLeave");
	}
	return;
}
function playerPoisonMask::onZoomTriggerLeave(%this)
{
	sceneWindow2d.setCurrentCameraZoom(camera.getZoom() + curZoom);
	return;
}
