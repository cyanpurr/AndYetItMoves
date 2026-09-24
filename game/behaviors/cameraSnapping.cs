// cameraSnapping.cs.dso
if (!(isObject(BeCameraSnapping)))
{
	%template = new BehaviorTemplate(Name : BeCameraSnapping);
	%template.friendlyName = "Camera Snapping Object";
	%template.behaviorType = "GameplayMechanisms";
	%template.description = "mounts the camera softly to the objects center.";
	%template.addBehaviorField(pullStrength, "the initial mountforce to set - gets larger the nearer the center gets", float, "3");
	%template.addBehaviorField(collisionDetection, "the collision mode (CIRCLE is default)", enum, "CIRCLE", "FULL	CIRCLE	POLYGON");
}
function BeCameraSnapping::onBehaviorAdd(%this)
{
	%owner = Owner;
	if (%owner.getClassName() != "t2dTrigger")
	{
		debugWarn("owner of camera scaling behavior is not a trigger!");
		%owner.removeBehavior(%this);
		return;
	}
	%triggerBehavior = %owner.addDependentBehavior("BeTrigger");
	subscribeToEvents(%this, "onLevelLoadFinished onUpdateFirstTick40");
	return;
}
function BeCameraSnapping::onLevelLoadFinished(%this)
{
	%owner = Owner;
	%triggerBehavior = %owner.getBehavior("BeTrigger");
	%triggerBehavior.setTriggerCollisionDetection(collisionDetection);
	%triggerBehavior.setTriggerCollisionGroup("zoomTrigger");
	%owner.setCollisionSuppress("1");
	return;
}
function BeCameraSnapping::onUpdateFirstTick40(%this)
{
	%owner = Owner;
	%owner.setCollisionSuppress("0");
	return;
}
function BeCameraSnapping::onEnter(%this, )
{
	if ($disableZoomTrigger)
	{
		return;
	}
	%owner = Owner;
	player.isInCameraSnapper = isInCameraSnapper + 1.0;
	if (%owner.getLinkCount() > 0.0)
	{
	}
	else
	{
	}
	%center = "0 0";
	%this.cameraman = viewWindow.getMountedParent();
	sceneWindow2d.mount(%owner, %center, pullStrength, "0");
	viewWindow.mount(%owner, %center, pullStrength, "0", "0", "0", "0");
	return;
}
function BeCameraSnapping::onLeave(%this, )
{
	if ($disableZoomTrigger)
	{
		return;
	}
	%owner = Owner;
	player.isInCameraSnapper = isInCameraSnapper - 1.0;
	if (viewWindow.getMountedParent().getId() == %owner.getId())
	{
		sceneWindow2d.mount(cameraman, "0 0", pullStrength, "0");
		viewWindow.mount(cameraman, "0 0", pullStrength, "0", "0", "0", "0");
		%this.schedule("1000", "resetMountForce");
	}
	return;
}
function BeCameraSnapping::resetMountForce(%this)
{
	if (isPlayer(viewWindow.getMountedParent()))
	{
		sceneWindow2d.setMountForce(cameraMountForce);
		viewWindow.setMountForce(cameraMountForce);
	}
	return;
}
