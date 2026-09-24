// zoomScaling.cs.dso
if (!(isObject(BeZoomScaling)))
{
	if (!(isTorquePlayer()))
	{
		%template = new BehaviorTemplate(Name : BeZoomScaling);
	}
	else
	{
		%template = new BeZoomScalingTemplate(Name : BeZoomScaling);
	}
	%template.friendlyName = "Zoom Scaling Object";
	%template.behaviorType = "GameplayMechanisms";
	%template.description = "Scales the camera dynamic larger or smaller when the player enters the area around it";
	if (!(isTorquePlayer()))
	{
		%template.addBehaviorField(targetZoomFactor, "zoomfactor of the camera, when inside of near radius", float, "0.5");
		%template.addBehaviorField(outerZoomFactor, "zoomfactor at the border of zoom scaler", float, "1");
		%template.addBehaviorField(nearRadius, "between 0 and 1; 0 -> targetFactor is reached at centerpoint, 1 -> reached at outer collidion circle", float, "0.5");
	}
}
function BeZoomScaling::onBehaviorAdd(%this)
{
	%owner = Owner;
	if (%owner.getClassName() != "t2dTrigger")
	{
		debugWarn("owner of camera scaling behavior is not a trigger!");
		%owner.removeBehavior(%this);
		return;
	}
	%owner.setCollisionCircleSuperscribed("0");
	%owner.setCollisionCircleScale("1");
	%triggerBehavior = %owner.addDependentBehavior("BeTrigger");
	subscribeToEvents(%this, "onLevelLoadFinished1 onLevelLoadFinished5 onLevelLoadFinished10 onUpdateFirstTick40");
	return;
}
function BeZoomScaling::onLevelLoadFinished1(%this)
{
	%owner = Owner;
	%owner.setCollisionSuppress("1");
	return;
}
function BeZoomScaling::onLevelLoadFinished5(%this)
{
	%owner = Owner;
	%this.farRadius = Owner.getCollisionRadius();
	%this.nearRadius = nearRadiusFactor * %farRadius;
	%this.maxZoom = targetZoomFactor;
	%this.minZoom = targetZoomFactor;
	zoomAdjusterGroup.add(%this);
	%this.Smoother = Smoother::createInstance();
	Smoother.init("1", outerZoomFactor, targetZoomFactor);
	return;
}
function BeZoomScaling::onLevelLoadFinished10(%this)
{
	%owner = Owner;
	%triggerBehavior = %owner.getBehavior("BeTrigger");
	%triggerBehavior.setTriggerCollisionGroup("zoomTrigger");
	%owner.setStayCallback("1");
	if (nearRadius <= 1.0)
	{
		%this.nearRadius = nearRadius * %owner.getCollisionRadius();
	}
	return %this;
}
function BeZoomScaling::onUpdateFirstTick40(%this)
{
	%owner = Owner;
	%owner.setCollisionSuppress("0");
	return;
}
function BeZoomScaling::onEnter(%this, %object)
{
	if ($disableZoomTrigger)
	{
		return;
	}
	if (%object != camera.getId())
	{
		return camera.getId();
	}
	subscribeToEvents(%this, "onUpdateFrame10");
	if (isDead)
	{
		return player;
	}
	%this.oldZoomFactor = outerZoomFactor;
	return;
}
function BeZoomScaling::onLeave(%this, %object)
{
	if ($disableZoomTrigger)
	{
		return;
	}
	if (%object != camera.getId())
	{
		return camera.getId();
	}
	unSubscribeFromEvents(%this, "onUpdateFrame10");
	if (isDead)
	{
		return player;
	}
	camera.setZoom(oldZoomFactor);
	return;
}
function BeZoomScaling::switchOff(%this)
{
	Owner.setCollisionSuppress("1");
	return;
}
function BeZoomScaling::switchOn(%this)
{
	Owner.setCollisionSuppress("0");
	return;
}
