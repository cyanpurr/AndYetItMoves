// playerDeath.cs.dso
function player::onResetLevel(%this)
{
	%this.levelIsQuitting = "1";
	return;
}
function player::dieFragged(%this, %impactValue)
{
	if (isZombie)
	{
		return %this;
	}
	if (isPlayer(%this))
	{
		triggerEvent("onDieFragged");
	}
	debugEcho("player died with impactValue" SPC %impactValue);
	player::frag(%this);
	player::die(%this);
	if ($WII && isPlayer(%this) && !(levelIsQuitting) && wiiInput.getConnectedExtension() != "classic")
	{
		wiiMoteStartRumble();
		%intensity = 150.0 + t2dGetMax("0", t2dVectorLength(impactVelocityVector) - lethalSpeed) * 2.0;
		%this.rumbleSchedule = schedule(%intensity, "0", "wiiMoteStopRumble");
	}
	%this.setVisible("0");
	$respawnScheduleId = schedule("1500", "0", "respawn", %this);
	return;
}
function player::dieOutside(%this)
{
	if (isPlayer(%this))
	{
		triggerEvent("onDieOutside");
	}
	player::die(%this);
	return;
}
function player::dieAlterEgoOutside(%this)
{
	triggerEvent("onDieAlterEgoOutside");
	%this.die();
	%this.getBehavior("BeShrinker").startShrinking();
	%this.origMaxAngularVelocity = %this.getMaxAngularVelocity();
	%this.setMaxAngularVelocity("360");
	%this.setAngularVelocity("360");
	return;
}
function player::dieExploding(%this, %strenght)
{
	if (%strenght $= "")
	{
		%strenght = 50;
	}
	%this.lastExplodingStrenght = %strenght;
	if (isPlayer(%this))
	{
		triggerEvent("onDieExploding");
	}
	player::dieFragged(%this);
	player::splatter(%this, %strenght);
	return;
}
function player::dieSquashed(%this)
{
	%this.lastDeathEvent = "squash";
	player::dieExploding(%this, "100");
	return;
}
function player::dieSnakeBite(%this)
{
	if (isPlayer(%this))
	{
		triggerEvent("onDiePoisoned");
		%this.setLinearVelocity(t2dVectorAdd(%this.getLinearVelocity(), "-250 0"));
		setControlsEnabled("0");
		schedule("100", "0", "setControlsEnabled", "1");
	}
	%this.isPoisoned = "1";
	player::startVisualPoisoning(%this);
	return;
}
function player::dieBurning(%this)
{
	if (isBurning)
	{
		return %this;
	}
	player::createFireTexture(%this);
	%this.isBurning = "1";
	if (isPlayer(%this))
	{
		triggerEvent("onDieBurning");
		fireMask.setSize("0 0");
		fireMask.mount(%this, "0 0", "0", "1", "1", "0", "0");
		%this.setTexObjectName(fireMask);
		%this.setUsePositive("1");
		%this.setRenderTexture("1");
		%this.setTransformTexture("1");
		fireTex.mount(%this, "0 0", "0", "1", "1", "0", "0");
		%this.setTexObjectName(fireTex, "1");
		%this.setRepeatTexture("1", "1");
		%anbrennTime = 3;
		%growVel = t2dVectorScale(player.getSize(), 2.0 / %anbrennTime);
		fireMask.setSizeVelocity(%growVel);
		if (raining)
		{
			%anbrennTime = %anbrennTime / 3.0;
		}
		%this.burnSchedule = schedule(%anbrennTime * 1000.0, "0", "player::endDieBurning", %this);
	}
	else
	{
		%this.setUsePositive("1");
		%this.setTexObjectName(fireTex);
		%this.setRenderTexture("1");
		fireTex.mount(%this, "0 0", "0", "1", "1", "0", "0");
	}
	return;
}
function player::createFireTexture(%this)
{
	if (!(isObject(fireTex)))
	{
		%this.fireTex = new t2dAnimatedSprite(Name : "")
		{
			animationName = "fireclipAnimation";
			scenegraph = scenegraph;
		}
		fireTex.setSize(%this.getSizeY() * 0.699999988079071 SPC %this.getSizeY() * 0.699999988079071);
		fireTex.setVisible("0");
		fireTex.addDependentBehavior("BeTexture");
		%this.fireMask = new t2dStaticSprite(Name : "")
		{
			imageMap = "undefined_circleImageMap";
			scenegraph = scenegraph;
		}
		fireMask.setVisible("0");
	}
	return;
}
function player::endDieBurning(%this)
{
	if (!(raining))
	{
		player::dieExploding(%this, "100");
	}
	triggerEvent("onEndDieBurning");
	player::removeFire(%this);
	return;
}
function player::removeFire(%this)
{
	%this.setRenderTexture("0");
	fireMask.dismount();
	fireMask.setSizeVelocity("0 0");
	fireTex.dismount();
	%this.isBurning = "0";
	return;
}
function player::onRainStart(%this)
{
	debugEcho("player: its raining!" SPC %this);
	%this.raining = "1";
	if (isBurning)
	{
		%this.burnSchedule = schedule(getEventTimeLeft(burnSchedule) / 2.0, "0", "player::endDieBurning", %this);
	}
	return;
}
function player::die(%this)
{
	if (!(isPlayer(%this)))
	{
		return isPlayer(%this);
	}
	if (isBurning)
	{
		cancel(burnSchedule);
		player::removeFire(%this);
	}
	speedHint.setVisible("0");
	%this.setCollisionSuppress("1");
	triggerShape.setCollisionSuppress("1");
	fallOutPoly.setCollisionSuppress("1");
	fallOutPoly.dismount();
	%this.isDead = "1";
	setControlsEnabled("0");
	%this.getBehavior("BeGravitic").stopGravity();
	if (camera.getIsRotating())
	{
		unSubscribeFromEvent(%this.getBehavior("BeGravitic"), "onRotationFinish");
	}
	%this.baseVelocity = "0 0";
	%this.isFalling = "1";
	%this.setState(stateId["lethalFall"]);
	triggerEvent("onPlayerDeath");
	if (!(isInCameraSnapper))
	{
		camera.zoomTo("1.2");
	}
	return;
}
function player::reanimate(%this, %skipCameraAnimation, %skipZoomAnimation)
{
	%this.setAtRest();
	%this.isMarkedForCollisionActivation = "1";
	if (!($watchAsReplay))
	{
		%this.setVisible(1 && !($makeAllSpawnpointsInvisible));
	}
	%this.getBehavior("BeShrinker").changeLayer("0");
	%spawnpointRotation = -1.0 * spawnPoint.getRotation();
	if (playmodeManager.isLimitedRotationMode() && lastCameraRotation != "")
	{
		%spawnpointRotation = lastCameraRotation;
	}
	%direction = t2dShortestAngleDifference(camera.getCurrentRotation(), %spawnpointRotation) / 90.0;
	if (!(freeRotation))
	{
		%direction = mRound(%direction);
	}
	camera.rotate(%direction, %skipCameraAnimation);
	if (camera.getZoom() != zoomFactor)
	{
		if (%skipZoomAnimation)
		{
			camera.setZoom(zoomFactor);
			break;
		}
		camera.zoomTo(zoomFactor);
	}
	setControlsEnabled("1");
	if ($WII)
	{
		if (initialFadeCompleted)
		{
			wiiInput.checkExtension();
		}
	}
	player::defrag(%this);
	%this.isDead = "0";
	triggerEvent("onPlayerReanimate");
	$isJumpingToSpawnPoint = 0;
	return;
}
function player::frag(%this)
{
	if (isBurning)
	{
		player::createFireTexture(%this);
		%firstTexToUse = fireTex;
	}
	else
	{
		if (isPoisoned)
		{
			%firstTexToUse = playerPoisonTex;
		}
	}
	%i = 0;
	while (%i < fraggedPlayerPartGroup.getCount())
	{
		%obj = fraggedPlayerPartGroup.getObject(%i);
		if (isObject(%firstTexToUse))
		{
			%obj.getBehavior("BeMask").setTexture(%firstTexToUse, "1");
			%obj.setRepeatTexture("1");
		}
		if (isObject(%sndTexToUse))
		{
			%obj.setTexObjectName(%sndTexToUse, "1");
			%obj.setRepeatTexture("1", "1");
		}
		%obj.setUsePositive("1");
		%obj.setVisible("1");
		%obj.setCollisionSuppress("0");
		%obj.dismount();
		%obj.setLinearVelocity(impactVelocityVector);
		%i = %i + 1.0;
	}
	return fraggedPlayerPartGroup.getCount();
}
function player::defrag(%this)
{
	%i = 0;
	while (%i < fraggedPlayerPartGroup.getCount())
	{
		%obj = fraggedPlayerPartGroup.getObject(%i);
		%obj.getBehavior("BeMask").unlink();
		%obj.setVisible("0");
		%obj.setLayer($LAYER["player"]);
		%obj.setCollisionSuppress("1");
		%i = %i + 1.0;
	}
	return fraggedPlayerPartGroup.getCount();
}
function player::resetPlayerparts(%this)
{
	if (!(isObject(fraggedPlayerPartGroup)))
	{
		return isObject(fraggedPlayerPartGroup);
	}
	%i = 0;
	while (%i < fraggedPlayerPartGroup.getCount())
	{
		%obj = fraggedPlayerPartGroup.getObject(%i);
		%mountWithOffsetBehavior = %obj.getBehavior("BeMountWithOffset");
		%mountWithOffsetBehavior.remount("0", "1");
		%obj.setMountForce("0");
		%i = %i + 1.0;
	}
	return fraggedPlayerPartGroup.getCount();
}
function player::clonePlayerParts(%this, %doulbe, %excludeCollisionLayerList, %alphaBlending, %invertColor)
{
	if (%alphaBlending $= "")
	{
		%alphaBlending = 1;
	}
	%doulbe.fraggedPlayerPartGroup = new SimSet(Name : "");
	levelGarbageCollector.add(fraggedPlayerPartGroup);
	%i = 0;
	while (%i < fraggedPlayerPartGroup.getCount())
	{
		%part = fraggedPlayerPartGroup.getObject(%i);
		%partClone = new t2dStaticSprite(Name : "")
		{
			scenegraph = scenegraph;
			imageMap = imageMap;
			frame = frame;
			size = size;
			_behavior0 = "BePartOfPlayer";
		}
		%pobBehavior = %partClone.getBehavior("BePartOfPlayer");
		%pobBehavior.onLevelLoadFinished10();
		%pobBehavior.onLevelLoadFinished20();
		%partClone.setLayer($LAYER["player"] + 1.0);
		%partClone.setCollisionLayers(excludeBits(%excludeCollisionLayerList));
		%partClone.setBlendAlpha(%alphaBlending);
		%origMountBehavior = %part.getBehavior("BeMountWithOffset");
		%mountWithOffsetBehavior = %partClone.getBehavior("BeMountWithOffset");
		%mountWithOffsetBehavior.setupMountWithOffset(%doulbe, spawnMountForce, "1");
		%mountWithOffsetBehavior.init();
		%mountWithOffsetBehavior.offset = offset;
		%mountWithOffsetBehavior.remount();
		%partClone.setMountForce("0");
		if (%invertColor)
		{
			%partClone.setInvertMainTex("1");
		}
		fraggedPlayerPartGroup.add(%partClone);
		%i = %i + 1.0;
	}
	return fraggedPlayerPartGroup.getCount();
}
function player::splatter(%this, %strenght)
{
	if (%strenght == 0.0)
	{
		return fraggedPlayerPartGroup.getCount();
	}
	%variance = 0;
	%i = 0;
	while (%i < fraggedPlayerPartGroup.getCount())
	{
		%randStrenght = %strenght + floatRandom("0", %variance) - %variance / 2.0;
		%velocity = rotateVector("0" SPC %randStrenght, getRandom("0", "360"));
		%obj = fraggedPlayerPartGroup.getObject(%i);
		%obj.setLinearVelocity(%velocity);
		%i = %i + 1.0;
	}
	return fraggedPlayerPartGroup.getCount();
}
