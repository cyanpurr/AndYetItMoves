// quirkyMovement.cs.dso
if (!(isObject(BeQuirkyMovement)))
{
	%template = new BehaviorTemplate(Name : BeQuirkyMovement);
	%template.friendlyName = "QuirkyMovement";
	%template.behaviorType = "Positioning";
	%template.description = "will move this object on a swaying path depending on the min- and max-offset parameters";
	%template.addBehaviorField(minOffset, "this will be the minimal offset", Point2F, "-50 -80");
	%template.addBehaviorField(maxOffset, "this will be the maximal offset", Point2F, "50 -40");
	%template.addBehaviorField(MountForce, "will determine how smooth or jumpy the movement will be", float, "1.5");
	%template.addBehaviorField(timerInterval, "how often the target object will update", int, "300");
	%template.addBehaviorField(autoPlay, "if it shoudl start moving right away", bool, "0");
}
function BeQuirkyMovement::onBehaviorAdd(%this)
{
	subscribeToEvents(%this, "onLevelLoadFinished10");
	return;
}
function BeQuirkyMovement::onLevelLoadFinished10(%this)
{
	%owner = Owner;
	%this.init();
	if (autoPlay)
	{
		%this.startMoving();
		subscribeToEvents(%this, "onRotationStart onRotationFinish");
	}
	%this.updateTargetSchedule = "0";
	unSubscribeFromEvent(%this, "onLevelLoadFinished10");
	return;
}
function BeQuirkyMovement::init(%this)
{
	%owner = Owner;
	%this.targetObject = new t2dSceneObject(Name : "")
	{
		scenegraph = scenegraph;
		Position = %owner.getPosition();
		_behavior0 = "BeDontCollide";
	}
	return;
}
function BeQuirkyMovement::startMoving(%this)
{
	%owner = Owner;
	%owner.setMountForce(MountForce);
	%this.startPosition = %owner.getPosition();
	%this.updateTarget();
	return;
}
function BeQuirkyMovement::updateTarget(%this)
{
	%owner = Owner;
	%targetPos = %owner.getPosition();
	setHorizontalComponent(%targetPos, getHorizontalComponent(startPosition));
	%targetOffset = getRandom(getWord(minOffset, "0"), getWord(maxOffset, "0")) SPC getRandom(getWord(minOffset, "1"), getWord(maxOffset, "1"));
	%targetPos = t2dVectorAdd(%targetPos, absRot(%targetOffset));
	targetObject.setPosition(%targetPos);
	if (!(%owner.getMountedParent()))
	{
		%owner.mount(targetObject, "0 0", MountForce, "0", "0", "0", "0");
	}
	%this.updateTargetSchedule = %this.schedule(timerInterval, "updateTarget");
	return;
}
function BeQuirkyMovement::stopMoving(%this)
{
	%owner = Owner;
	%owner.dismount();
	%owner.setAtRest();
	if (updateTargetSchedule)
	{
		cancel(updateTargetSchedule);
		%this.updateTargetSchedule = "0";
	}
	return;
}
function BeQuirkyMovement::onRotationStart(%this)
{
	%this.stopMoving();
	return;
}
function BeQuirkyMovement::onRotationFinish(%this)
{
	%this.startMoving();
	return;
}
function BeQuirkyMovement::switchOn(%this)
{
	subscribeToEvents(%this, "onRotationFinish onRotationStart");
	%this.startMoving();
	return;
}
function BeQuirkyMovement::switchOff(%this)
{
	%this.stopMoving();
	unSubscribeFromEvents(%this, "onRotationFinish onRotationStart");
	return;
}
function BeQuirkyMovement::onBehaviorRemove(%this)
{
	targetObject.safeDelete();
	return;
}
function BeQuirkyMovement::onSpawnFinished(%this)
{
	%this.init();
	return;
}
