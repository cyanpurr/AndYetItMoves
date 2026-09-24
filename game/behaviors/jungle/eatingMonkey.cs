// eatingMonkey.cs.dso
if (!(isObject(BeEatingMonkey)))
{
	%template = new BehaviorTemplate(Name : BeEatingMonkey);
	%template.friendlyName = "EatingMonkey";
	%template.behaviorType = "LevelJungle";
	%template.description = "a monkey that lusts for bananas";
	%template.addBehaviorField(hideOut, "the hideOut of the monkey where he retracts when eating", object, null, t2dSceneObject);
}
function BeEatingMonkey::onBehaviorAdd(%this)
{
	subscribeToEvents(%this, "onLevelLoadFinished5");
	Owner.addDependentBehavior("BeCollide");
	return;
}
function BeEatingMonkey::onLevelLoadFinished5(%this)
{
	%owner = Owner;
	%owner.setLayer($LAYER["main_foreground"]);
	%owner.addDependentBehavior("BeTranslate");
	if (isObject(hideOut))
	{
		%this.hideoutDist = t2dVectorSub(hideOut.getPosition(), %owner.getPosition());
	}
	else
	{
		debugWarn("no hideOut set for monkey:" SPC %owner);
	}
	%owner.setupTranslate("5 0", "2", "0", "1", "1", "1");
	%react = %owner.addDependentBehavior("BeReactOnCollision");
	%react.maxCollisions = "1";
	%react.initGroups("jungleBanana");
	%react.BehaviorList = "BeEatingMonkey";
	%owner.setGraphGroup($GROUPS["jungleMonkey"]);
	%activateBehavior = %owner.addDependentBehavior("BeActivate");
	%activateBehavior.activator = "viewWindowTrigger";
	%activateBehavior.suppressOwnerCollision = "0";
	return;
}
function BeEatingMonkey::reactOnCollision(%this, %dstObject)
{
	%owner = Owner;
	%dstObject.removeBehavior(%owner.getBehavior("BeReactOnCollision"));
	%dstObject.removeBehavior(%owner.getBehavior("BeMoving"));
	%dstObject.setCollisionGroups("");
	%bananaBehavior = %dstObject.getBehavior("BeBanana");
	%freshBanana = collisionCount == 0.0;
	Owner.safeDelete();
	cancel(viewSafeDeleteSchedule);
	%dstObject.removeBehavior(%bananaBehavior);
	%dstObject.mount(%owner, "0 0", "0", "1", "1", "1", "1");
	%owner.setImmovable("0");
	%owner.getBehavior("BeTranslate").setupTranslate(hideoutDist, "2", "0", "0", "0", "1");
	%owner.getBehavior("BeTranslate").play();
	%sound = %owner.getBehavior("BePlayLoopingSound");
	%sound.stop();
	%sound.setAudioProfile(MonkeyEating);
	%sound.volume = "0.6";
	%sound.play();
	if (%freshBanana)
	{
		achievements.setAchieved("Zookeeper");
	}
	return;
}
