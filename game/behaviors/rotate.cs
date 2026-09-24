// rotate.cs.dso
if (!(isObject(BeRotate)))
{
	if (!(isTorquePlayer()))
	{
		%template = new BehaviorTemplate(Name : BeRotate);
	}
	else
	{
		%template = new BeRotateTemplate(Name : BeRotate);
	}
	%template.friendlyName = "Rotate";
	%template.behaviorType = "Positioning";
	%template.description = "rotates this object by a specified angle back and forth around link point";
	if (!(isTorquePlayer()))
	{
		%template.addBehaviorField(angle, "symmetricAngle on: angle (deg) the object will rotate left and rigth from current rotation; symmetricAngle off: angle to rotate to from current rotation (neg = clockwise)", float, "10");
		%template.addBehaviorField(time, "the time (s) it should take from original rotation to angle in one direction and back", float, "2");
		%template.addBehaviorField(mode, "the smoothing function to use", enum, "SMOOTH", $SMOOTHING_FUNCTIONS);
		%template.addBehaviorField(sincParams, "list of parameters for SINC mode: swingCount, swingExponent, swingFactor, reachTargetFactor(can be omitted); see Smoother::init() for details", string, "3 1 1");
		%template.addBehaviorField(autoPlay, "if rotation starts on levelload", bool, "1");
		%template.addBehaviorField(symmetricAngle, "see description of angle", bool, "1");
		%template.addBehaviorField(loop, "repeat it or not", bool, "1");
		%template.addBehaviorField(loop4ever, "repeat it unstopable", bool, "0");
		%template.addBehaviorField(use32msTick, "if updates should occur all 32ms", bool, "0");
		%template.addBehaviorField(pauseOnRotation, "...", bool, "0");
		%template.addBehaviorField(playSingleSoundOnStart, "plays singleSound on start of rotation (behavior has to be applied to object before manually (!)", bool, "0");
	}
}
function BeRotate::onBehaviorAdd(%this)
{
	%this.Smoother = Smoother::createInstance();
	subscribeToEvents(%this, "onLevelLoadFinished4 onLevelLoadFinished5 onLevelLoadFinished30");
	return;
}
function BeRotate::onLevelLoadFinished4(%this)
{
	%owner = Owner;
	if (!(%owner.getLinkCount()))
	{
		debugWarn("there is no link point set to object:" SPC %owner SPC "
 making a new one at 0 0");
		%owner.addLinkPoint("0", "0");
	}
	%this.pivot = new t2dSceneObject(Name : "")
	{
		Layer = "0";
		Position = %owner.getLinkPoint("1");
		Rotation = %owner.getRotation();
		size = "1 1";
		scenegraph = daSceneGraph;
		_behavior0 = "BeDontCollide";
	}
	return;
}
function BeRotate::onLevelLoadFinished5(%this)
{
	if ($WII)
	{
		%this.loop4ever = "0";
		%this.pauseOnRotation = "1";
	}
	%this.init();
	return;
}
function BeRotate::onLevelLoadFinished30(%this)
{
	%owner = Owner;
	%owner.setMountedCollidesNotImmovable();
	return;
}
function BeRotate::init(%this, %dontWaitForFirstKeyPress)
{
	%owner = Owner;
	if (Owner.getBehavior("BeMountWithOffset") && trackRotation)
	{
		%this.addParentAngularVelocity = "1";
	}
	%this.initMount();
	%this.initSmoother();
	%this.isRotating = "0";
	if (autoPlay)
	{
		if (%dontWaitForFirstKeyPress)
		{
			if (symmetricAngle)
			{
			}
			else
			{
			}
			%this.play("0", "0", "1");
			break;
		}
		subscribeToEvents(%this, "onFirstKeyPressed");
	}
	return;
}
function BeRotate::onFirstKeyPressed(%this)
{
	if (symmetricAngle)
	{
	}
	else
	{
	}
	%this.play("0", "0", "1");
	return;
}
function BeRotate::initMount(%this)
{
	%owner = Owner;
	%owner.setRotation("0");
	%localOffsetVector = pivot.getLocalPoint(%owner.getPosition());
	%this.pivotMountId = %owner.mount(pivot, %localOffsetVector, "0", "1", "1", "0", "0");
	return;
}
function BeRotate::initSmoother(%this)
{
	%owner = Owner;
	if (getWordCount(sincParams) == 4.0)
	{
		Smoother.setSincParams(getWord(sincParams, "0"), getWord(sincParams, "1"), getWord(sincParams, "2"), getWord(sincParams, "3"));
	}
	else
	{
		Smoother.setSincParams(getWord(sincParams, "0"), getWord(sincParams, "1"), getWord(sincParams, "2"));
	}
	if (symmetricAngle)
	{
		Smoother.init(time, %this.getPivotRotation() - angle, %this.getPivotRotation() + angle, mode, "0", loop4ever);
	}
	else
	{
		%targetAngle = %this.getPivotRotation() + angle;
		Smoother.init(time, %this.getPivotRotation(), %targetAngle, mode, "0", loop4ever);
	}
	return;
}
function BeRotate::setupRotate(%this, %angle, %time, %mode, %sincParams, %autoplay, %symmetricAngle, %loop, %init)
{
	if (%angle $= "")
	{
	}
	else
	{
	}
	%this.angle = %angle;
	if (%time $= "")
	{
	}
	else
	{
	}
	%this.time = %time;
	if (%mode $= "")
	{
	}
	else
	{
	}
	%this.mode = %mode;
	if (%sincParams $= "")
	{
	}
	else
	{
	}
	%this.sincParams = %sincParams;
	if (%autoplay $= "")
	{
	}
	else
	{
	}
	%this.autoPlay = %autoplay;
	if (%symmetricAngle $= "")
	{
	}
	else
	{
	}
	%this.symmetricAngle = %symmetricAngle;
	if (%loop $= "")
	{
	}
	else
	{
	}
	%this.loop = %loop;
	if (%init)
	{
		%this.init(autoPlay);
	}
	return;
}
function BeRotate::setRotatorPosition(%this, %posX, %posY)
{
	if (%posY $= "")
	{
		pivot.setPosition(%posX);
	}
	else
	{
		pivot.setPosition(%posX, %posY);
	}
	return;
}
function BeRotate::setRotatorSize(%this, %sizeX, %sizeY)
{
	%owner = Owner;
	if (%sizeY $= "")
	{
		%size = %sizeX;
	}
	else
	{
		%size = %sizeX SPC %sizeY;
	}
	pivot.setSize(%size);
	return;
}
function BeRotate::setRotateAngle(%this, %angle)
{
	%this.angle = %angle;
	%this.initSmoother();
	return;
}
function BeRotate::setRotateTime(%this, %time)
{
	%this.time = %time;
	return;
}
function BeRotate::setRotateAutoPlay(%this, %autoplay)
{
	%this.autoPlay = %autoplay;
	return;
}
function BeRotate::setRotateCallbackObject(%this, %object)
{
	%this.callbackObject = %object;
	return;
}
function BeRotate::play(%this, %reverse, %startInBetween, %dontPlayWhenSwitchedOff)
{
	Smoother.start(%reverse, %startInBetween);
	%this.targetReached = "0";
	%this.maximumReached = "0";
	%this.isRotating = "1";
	if (%dontPlayWhenSwitchedOff && isSwitchedOff)
	{
		return %this;
	}
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
		subscribeToEvents(%this, "onUpdateTick32ms");
	}
	else
	{
		subscribeToEvents(%this, "onUpdateTick15");
	}
	return;
}
