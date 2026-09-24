// counterRotate.cs.dso
if (!(isObject(BeCounterRotate)))
{
	if (!(isTorquePlayer()))
	{
		%template = new BehaviorTemplate(Name : BeCounterRotate);
	}
	else
	{
		%template = new BeCounterRotateTemplate(Name : BeCounterRotate);
	}
	%template.friendlyName = "Counter Rotate";
	%template.behaviorType = "LevelTrip";
	%template.description = "objects that rotate in the opposite direction than the world";
	%template.addBehaviorField(counterDegree, "description of field", int, "90");
	%template.addBehaviorField(orientationMode, "FULL - while rotating; STEP - on finish rotation; NO - never", enum, "FULL", "FULL	STEP	NO");
}
function BeCounterRotate::onBehaviorAdd(%this)
{
	subscribeToEvents(%this, "onLevelLoadFinished0 onLevelLoadFinished1 onLevelLoadFinished10");
	%this.modes["FULL"] = "1";
	%this.modes["STEP"] = "2";
	%this.modes["NO"] = "3";
	%this.mode = modes[orientationMode];
	return;
}
function BeCounterRotate::onLevelLoadFinished0(%this)
{
	%owner = Owner;
	if (!(%owner.getLinkCount()))
	{
		%owner.addLinkPoint("0 0");
	}
	%this.footPoint = %owner.getLocalPoint(%owner.getLinkPoint("1"));
	return;
}
function BeCounterRotate::onLevelLoadFinished1(%this)
{
	%owner = Owner;
	%this.counterRotation = %owner.getRotation();
	if (isObject(%owner.getBehavior("BeContinuouslyRotate")))
	{
		return isObject(%owner.getBehavior("BeContinuouslyRotate"));
	}
	%rotateBehavior = %owner.addDependentBehavior("BeRotate");
	%rotateBehavior.autoPlay = "0";
	%rotateBehavior.symmetricAngle = "0";
	%rotateBehavior.loop = "0";
	return;
}
function BeCounterRotate::onLevelLoadFinished10(%this)
{
	%owner = Owner;
	%this.setOrientationMode(orientationMode);
	%this.rotationFactor = counterDegree / 90.0;
	return;
}
function BeCounterRotate::setOrientationMode(%this, %mode)
{
	%this.mode = modes[%mode];
	if (mode $= modes["FULL"])
	{
		subscribeToEvents(%this, "onRotationStart onRotationUpdate");
	}
	else
	{
		if (mode $= modes["STEP"])
		{
			subscribeToEvents(%this, "onRotationStart onRotationFinish");
			unSubscribeFromEvent(%this, "onRotationUpdate");
			break;
		}
		if (mode $= modes["NO"])
		{
			unSubscribeFromEvents(%this, "onRotationStart onRotationFinish onRotationUpdate");
		}
	}
	return;
}
function BeCounterRotate::switchOn(%this, %onShowParalaxLayers)
{
	%owner = Owner;
	if (hiddenDekoLayers != %onShowParalaxLayers)
	{
		return %this;
	}
	%this.hiddenDekoLayers = "0";
	%this.setOrientationMode(orientationMode);
	return;
}
function BeCounterRotate::switchOff(%this, %onHideParalaxLayers)
{
	%this.hiddenDekoLayers = %onHideParalaxLayers;
	%this.setOrientationMode("STEP");
	return;
}
