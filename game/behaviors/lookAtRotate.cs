// lookAtRotate.cs.dso
if (!(isObject(BeLookAtRotate)))
{
	if (!(isTorquePlayer()))
	{
		%template = new BehaviorTemplate(Name : BeLookAtRotate);
	}
	else
	{
		%template = new BeLookAtRotateTemplate(Name : BeLookAtRotate);
	}
	%template.friendlyName = "lookAt Rotate";
	%template.behaviorType = "Positioning";
	%template.description = "rotates this object so that it always faces a given point";
	%template.addBehaviorField(lookAtPoint, "coords of point to look at, if empty lookatObject is used", string, "0 0");
	%template.addBehaviorField(lookAtPlayer, "look at player", bool, "0");
	%template.addBehaviorField(lookAtObject, "object to look at", object, null, t2dSceneObject);
	%template.addBehaviorField(objectLinkpoint, "use coords of object link point number", int, "0");
	%template.addBehaviorField(scaleToDistance, "scale width to distance length", bool, "1");
	%template.addBehaviorField(autoPlay, "wheter or not to turn it on right away", bool, "0");
	%template.addBehaviorField(rotationLimit, "angle limit: object may rotate in originalRotation +/-rotationLimit", float, "0");
	%template.addBehaviorField(isDeko, "if player can walk on it or its just dekoration", bool, "1");
}
function BeLookAtRotate::onBehaviorAdd(%this)
{
	subscribeToEvents(%this, "onLevelLoadFinished1 collectObjectsInBehavior onLevelLoadFinished10 onLevelLoadFinished30");
	return;
}
function BeLookAtRotate::onLevelLoadFinished1(%this)
{
	%owner = Owner;
	%this.originalRotation = %owner.getRotation();
	%this.baseVector = t2dVectorNormalise(t2dVectorSub(%owner.getLinkPoint("1"), %owner.getPosition()));
	%this.rotateBehavior = %owner.addDependentBehavior("BeRotate");
	rotateBehavior.autoPlay = "0";
	return;
}
function BeLookAtRotate::onLevelLoadFinished10(%this)
{
	if (lookAtPlayer)
	{
		%this.lookAtObject = player;
	}
	if (autoPlay)
	{
		%this.switchOn();
	}
	return;
}
function BeLookAtRotate::onLevelLoadFinished30(%this)
{
	%owner = Owner;
	%owner.setMountedCollidesNotImmovable();
	return;
}
function BeLookAtRotate::switchOn(%this, %onShowParalaxLayers)
{
	if (hiddenDekoLayers != %onShowParalaxLayers)
	{
		return %this;
	}
	%this.hiddenDekoLayers = "0";
	%this.targetPoint = lookAtPoint;
	if (isObject(lookAtObject))
	{
		if (objectLinkpoint > 0.0)
		{
		}
		else
		{
		}
		%this.targetPoint = lookAtObject.getPosition();
	}
	if (isDeko)
	{
		subscribeToEvents(%this, "onUpdateTick30");
	}
	else
	{
		subscribeToEvents(%this, "onUpdateFrame");
	}
	return;
}
function BeLookAtRotate::switchOff(%this, %onHideParalaxLayers)
{
	%this.hiddenDekoLayers = %onHideParalaxLayers;
	Owner.setAngularVelocity("0");
	if (isDeko)
	{
		unSubscribeFromEvents(%this, "onUpdateTick30");
	}
	else
	{
		unSubscribeFromEvents(%this, "onUpdateFrame");
	}
	return;
}
