// continuouslyRotate.cs.dso
if (!(isObject(BeContinuouslyRotate)))
{
	%template = new BehaviorTemplate(Name : BeContinuouslyRotate);
	%template.friendlyName = "ContinuouslyRotate";
	%template.behaviorType = "Positioning";
	%template.description = "once started this object will rotate with the given angular velocity";
	%template.addBehaviorField(AngularVelocity, "how fast the object should rotate (angles in sec)", float, "10");
	%template.addBehaviorField(autoPlay, "wheter or not the object should start rotating right away", bool, "0");
	%template.addBehaviorField(pauseOnRotate, "pause during camera rotation freeze", bool, "0");
}
function BeContinuouslyRotate::onBehaviorAdd(%this)
{
	subscribeToEvents(%this, "onLevelLoadFinished10 onLevelLoadFinished30");
	return;
}
function BeContinuouslyRotate::onLevelLoadFinished10(%this)
{
	if ($WII)
	{
		%this.pauseOnRotate = "1";
	}
	%this.originalRotation = Owner.getRotation();
	if (autoPlay)
	{
		subscribeToEvents(%this, "onFirstKeyPressed");
	}
	return;
}
function BeContinuouslyRotate::onFirstKeyPressed(%this)
{
	%this.initRotating("1");
	return;
}
function BeContinuouslyRotate::onLevelLoadFinished30(%this)
{
	%owner = Owner;
	%owner.setMountedCollidesNotImmovable();
	return;
}
function BeContinuouslyRotate::switchOn(%this, %onShowParalaxLayers)
{
	if (hiddenDekoLayers != %onShowParalaxLayers)
	{
		return %this;
	}
	if (pauseOnRotate)
	{
		subscribeToEvents(%this, "onRotationStart onRotationFinish");
	}
	%this.switchedOff = "0";
	%this.hiddenDekoLayers = "0";
	%this.initRotating();
	return;
}
function BeContinuouslyRotate::switchOff(%this, %onHideParalaxLayers)
{
	%this.switchedOff = "1";
	if (pauseOnRotate)
	{
		unSubscribeFromEvents(%this, "onRotationStart onRotationFinish");
	}
	%this.hiddenDekoLayers = %onHideParalaxLayers;
	%this.stopRotating();
	return;
}
function BeContinuouslyRotate::initRotating(%this, %dontPlayWhenSwitchedOff)
{
	%owner = Owner;
	if (initedTime $= "")
	{
		%this.initedTime = scenegraph.getSceneTime();
		%this.initialRotation = %owner.getRotation();
	}
	if (%dontPlayWhenSwitchedOff && switchedOff)
	{
		return %this;
	}
	%this.startRotating();
	return;
}
function BeContinuouslyRotate::startRotating(%this)
{
	%owner = Owner;
	if (pauseOnRotate)
	{
		subscribeToEvents(%this, "onRotationStart onRotationFinish");
	}
	%rot360duration = 360.0 / AngularVelocity;
	%timePlaying = scenegraph.getSceneTime() - initedTime;
	%rotationToApply = normaliseAngle(%timePlaying / %rot360duration * 360.0 + initialRotation);
	%owner.setRotation(%rotationToApply);
	%owner.setAngularVelocity(AngularVelocity);
	return;
}
function BeContinuouslyRotate::pauseRotating(%this)
{
	%owner = Owner;
	%owner.setAngularVelocity("0");
	return;
}
function BeContinuouslyRotate::stopRotating(%this)
{
	%this.pauseRotating();
	return;
}
function BeContinuouslyRotate::reset(%this)
{
	%owner = Owner;
	%owner.setRotation(originalRotation);
	return;
}
function BeContinuouslyRotate::onRotationStart(%this)
{
	%this.pauseRotating();
	return;
}
function BeContinuouslyRotate::onRotationFinish(%this)
{
	Owner.setAngularVelocity(AngularVelocity);
	return;
}
