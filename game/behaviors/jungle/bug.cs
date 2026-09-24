// bug.cs.dso
if (!(isObject(BeBug)))
{
	%template = new BehaviorTemplate(Name : BeBug);
	%template.friendlyName = "Bug";
	%template.behaviorType = "LevelJungle";
	%template.description = "a bug that slowly crawls around - turns around if he hits an object";
	%template.addBehaviorField(maxVelocity, "maximum speed it will move", float, "40");
	%template.addBehaviorField(accelFactor, "how fast the object should accelerate", float, "35");
	%template.addBehaviorField(turnAtEdges, "if the bug should turn around at edges or fall down", bool, "1");
	%template.addBehaviorField(toggleSwitch, "the switch we should beadded to so he can switch us off", object, null, t2dSceneObject);
}
function BeBug::onBehaviorAdd(%this)
{
	%owner = Owner;
	subscribeToEvents(%this, "onLevelLoadFinished1");
	return;
}
function BeBug::onBehaviorRemove(%this)
{
	if (spawning)
	{
		spawner.schedule("500", "spawn");
	}
	return;
}
function BeBug::onLevelLoadFinished1(%this)
{
	%owner = Owner;
	%owner.addDependentBehaviors("BePaceBackAndForth BeKeepOrientation");
	%paceBehavior = %owner.getBehavior("BePaceBackAndForth");
	%paceBehavior.accelFactor = accelFactor;
	%paceBehavior.maxVelocity = maxVelocity;
	%paceBehavior.turnAroundRightAway = "1";
	%paceBehavior.autoPlay = "0";
	if (turnAtEdges)
	{
		%paceBehavior.CollisionGroups = CollisionGroups SPC "jungleBugStopper";
	}
	if ($WII)
	{
		%owner.setSize(t2dVectorScale(%owner.getSize(), 4.0 / 3.0));
	}
	return;
}
function BeBug::onSpawnFinished(%this, %spawner, )
{
	%owner = Owner;
	%owner.setMaxAngularVelocity("30");
	%owner.getBehavior("BeMask").link();
	%owner.setFlipX("1");
	%this.spawner = %spawner;
	%owner.getBehavior("BeMoving").init();
	triggerShape.setCollisionGroups(triggerShape.getCollisionGroups() SPC $GROUPS["jungleDionaeaSnapTrigger"]);
	%owner.setGraphGroup($GROUPS["jungleBug"]);
	%owner.setMaxAngularVelocity("30");
	%owner.addDependentBehaviors("BePlayLoopingSound");
	%sound = %owner.getBehavior("BePlayLoopingSound");
	%sound.setAudioProfile("Bug");
	%sound.setVolume("0.1");
	%sound.play();
	%owner.viewSafeDelete("10");
	%owner.getBehavior("BeKeepOrientation").useFrameUpdate = "1";
	%owner.getBehavior("BeKeepOrientation").setOrientationMode("FULL");
	%owner.getBehavior("BeKeepOrientation").finishRotation();
	return;
}
function BeBug::switchOff(%this)
{
	%owner = Owner;
	%owner.safeDelete();
	return;
}
