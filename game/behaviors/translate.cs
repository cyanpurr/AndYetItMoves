// translate.cs.dso
if (!(isObject(BeTranslate)))
{
	if (!(isTorquePlayer()))
	{
		%template = new BehaviorTemplate(Name : BeTranslate);
	}
	else
	{
		%template = new BeTranslateTemplate(Name : BeTranslate);
	}
	%template.friendlyName = "Translate";
	%template.behaviorType = "Positioning";
	%template.description = "translates this object by a specified vectoor back and forth";
	%template.addBehaviorField(distanceVector, "vector ('x y') the object will translate from current position (forth and back if symmetricOffset is true)", Vector, "5 0", t2dVector);
	%template.addBehaviorField(time, "the time (s) it should take from original position to target position in one direction and back", float, "2");
	%template.addBehaviorField(mode, "the smoothing function to use", enum, "SMOOTH", $SMOOTHING_FUNCTIONS);
	%template.addBehaviorField(autoPlay, "if rotation starts on levelload", bool, "1");
	%template.addBehaviorField(symmetricOffset, "see description of distanceVector", bool, "1");
	%template.addBehaviorField(loop, "repeat it or not", bool, "1");
	%template.addBehaviorField(loop4ever, "repeat it unstopable", bool, "0");
	%template.addBehaviorField(loopReset, "wheter it should reset to it's original position or move back&forth smoothly", bool, "0");
	%template.addBehaviorField(pauseOnRotation, "if loop4ever is true this does NOT work!", bool, "0");
	%template.addBehaviorField(BaseVelocityAdaptionFactor, "how much the player will be dragged along", float, "1");
	%template.addBehaviorField(playSingleSoundOnStart, "plays a singleSound on start, behavior has to be adde manually on object (!) ", bool, "0");
	%template.addBehaviorField(killAfterFirstLoop, "plays a singleSound on start, behavior has to be adde manually on object (!) ", bool, "0");
	%template.addBehaviorField(use32msTick, "update every 32ms", bool, "0");
}
function BeTranslate::onBehaviorAdd(%this)
{
	%this.Smoother = Smoother::createInstance();
	subscribeToEvents(%this, "onLevelLoadFinished5 onLevelLoadFinished30");
	return;
}
function BeTranslate::onLevelLoadFinished5(%this)
{
	if ($WII)
	{
		%this.loop4ever = "0";
		%this.pauseOnRotation = "1";
	}
	%this.init();
	return;
}
function BeTranslate::onLevelLoadFinished30(%this)
{
	%owner = Owner;
	%owner.BaseVelocityAdaptionFactor = BaseVelocityAdaptionFactor;
	%owner.setMountedCollidesNotImmovable();
	%this.pauseOnRotation = pauseOnRotation && !(loop4ever);
	return;
}
function BeTranslate::setupTranslate(%this, %distanceVector, %time, %autoplay, %symmetricOffset, %loop, %init)
{
	if (%distanceVector $= "")
	{
	}
	else
	{
	}
	%this.distanceVector = %distanceVector;
	if (%time $= "")
	{
	}
	else
	{
	}
	%this.time = %time;
	if (%autoplay $= "")
	{
	}
	else
	{
	}
	%this.autoPlay = %autoplay;
	if (%symmetricOffset $= "")
	{
	}
	else
	{
	}
	%this.symmetricOffset = %symmetricOffset;
	if (%loop $= "")
	{
	}
	else
	{
	}
	%this.loop = %loop;
	if (%init)
	{
		%this.init();
	}
	return;
}
function BeTranslate::init(%this)
{
	%owner = Owner;
	%this.initSmoother();
	%this.rotateBehavior = %owner.getBehavior("BeRotate");
	%this.isRotator = isObject(rotateBehavior);
	%this.neverPlayed = "1";
	if (autoPlay)
	{
		subscribeToEvents(%this, "onFirstKeyPressed");
	}
	return;
}
function BeTranslate::onFirstKeyPressed(%this)
{
	if (symmetricOffset)
	{
		%this.play("0.5", "1");
	}
	else
	{
		%this.play("0", "1");
	}
	return;
}
function BeTranslate::initSmoother(%this)
{
	%owner = Owner;
	%originalPosition = %owner.getPosition();
	if (symmetricOffset)
	{
		Smoother.init(time, t2dVectorSub(%originalPosition, distanceVector), t2dVectorAdd(%originalPosition, distanceVector), mode, loop, "1", loopReset);
	}
	else
	{
		Smoother.init(time, %originalPosition, t2dVectorAdd(%originalPosition, distanceVector), mode, "0", loop, loopReset);
	}
	return;
}
function BeTranslate::switchOn(%this)
{
	%this.isSwitchedOff = "0";
	if (hasStarted)
	{
		%this.resume();
		if (pauseOnRotation)
		{
			subscribeToEvents(%this, "onRotationStart onRotationFinish");
		}
		if (neverPlayed)
		{
			%this.neverPlayed = "0";
			if (playSingleSoundOnStart)
			{
				Owner.getBehavior("BePlaySingleSound").play();
			}
		}
	}
	return;
}
function BeTranslate::switchOff(%this)
{
	%this.isSwitchedOff = "1";
	%this.pause();
	if (pauseOnRotation)
	{
		unSubscribeFromEvents(%this, "onRotationStart onRotationFinish");
	}
	return;
}
function BeTranslate::play(%this, %startInBetween, %dontPlayWhenSwitchedOff)
{
	%this.hasStarted = "1";
	Smoother.start("0", %startInBetween);
	if (%dontPlayWhenSwitchedOff && isSwitchedOff)
	{
		return %this;
	}
	%this.neverPlayed = "0";
	if (playSingleSoundOnStart)
	{
		Owner.getBehavior("BePlaySingleSound").play();
	}
	if (pauseOnRotation)
	{
		subscribeToEvents(%this, "onRotationStart onRotationFinish");
	}
	if (use32msTick)
	{
		subscribeToEvent(%this, "onUpdateTick32ms");
	}
	else
	{
		subscribeToEvent(%this, "onUpdateTick15");
	}
	return;
}
function BeTranslate::kill(%this)
{
	unSubscribeFromEvents(%this, "onUpdateTick32ms onUpdateTick15");
	%this.updateVelocity("0 0");
	Owner.recursiveDelete();
	return;
}
function BeTranslate::onSpawnFinished(%this)
{
	%this.init();
	%this.onFirstKeyPressed();
	return;
}
