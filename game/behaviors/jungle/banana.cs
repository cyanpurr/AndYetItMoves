// banana.cs.dso
if (!(isObject(BeBanana)))
{
	%template = new BehaviorTemplate(Name : BeBanana);
	%template.friendlyName = "Banana";
	%template.behaviorType = "LevelJungle";
	%template.description = "user friendly description of the behavior";
	%template.addBehaviorField(collisionsToSquash, "the number of collision the banana can take until totally squashed- from 1 to 4", integer, "2");
	%template.addBehaviorField(squashSpeed, "how hard the collision has to be until the banan squashes", float, "60");
	%template.addBehaviorField(minCollisionGap, "how much time should pass minimum between two coll so they result in a squash (in s)", float, "0.6");
}
function BeBanana::onBehaviorAdd(%this)
{
	%owner = Owner;
	%owner.addDependentBehavior("BeMoving");
	subscribeToEvents(%this, "onLevelLoadFinished10");
	return;
}
function BeBanana::onLevelLoadFinished10(%this)
{
	%owner = Owner;
	%owner.setGraphGroup($GROUPS["jungleBanana"]);
	%react = %owner.addDependentBehavior("BeReactOnCollision");
	%react.maxCollisions = collisionsToSquash;
	%react.minCollisionGap = minCollisionGap;
	%react.criticalSpeed = squashSpeed;
	%react.BehaviorList = "BeBanana";
	%react.initGroups("player collide moving");
	%move = %owner.getBehavior("BeMoving");
	%move.Density = "0.01";
	%move.applyPhysicConstants();
	%owner.setImmovable("1");
	%owner.setCollisionSuppress("1");
	return;
}
function BeBanana::onSpawnFinished(%this, %spawner)
{
	%owner = Owner;
	%this.spawner = %spawner;
	%owner.setImmovable("0");
	%owner.setCollisionSuppress("0");
	%owner.setLayer($LAYER["main_foreground"]);
	%this.viewSafeDeleteSchedule = safeSchedule("2000", %owner, "viewSafeDelete", "10");
	playDistanceEventSound(%owner, BananenSpawn, "1");
	return;
}
function BeBanana::reactOnCollision(%this, %dstObject, , , , , , , %collisionCount)
{
	%owner = Owner;
	if (%owner.getName() $= "originalBanana")
	{
		debugWarn("original Banana received a collision from" SPC %dstObject SPC ". WHY? but we will not react to it!");
		return;
	}
	%this.collisionCount = %collisionCount;
	if (collisionsToSquash == %collisionCount)
	{
		%this.squash();
	}
	else
	{
		%owner.setFrame(%collisionCount);
		playDistanceEventSound(%owner, BananenSpawn, "1");
	}
	return;
}
function BeBanana::squash(%this)
{
	%owner = Owner;
	%this.effect = new t2dParticleEffect(Name : "")
	{
		scenegraph = daSceneGraph;
		effectFile = "~/data/particles/bananaBreak.eff";
		useEffectCollisions = "1";
		effectMode = "KILL";
		effectTime = "0.2";
		canSaveDynamicFields = "1";
		size = %owner.getSize();
	}
	%owner.setVisible("0");
	effect.setPosition(%owner.getPosition());
	effect.setRotation(%owner.getRotation());
	effect.playEffect();
	playDistanceEventSound(%owner, BananenSpawn, "1");
	%owner.safeDelete();
	return;
}
function BeBanana::onBehaviorRemove(%this)
{
	%owner = Owner;
	if (isObject(viewSafeDeleteSchedule))
	{
		viewSafeDeleteSchedule.cancelSchedule();
	}
	spawner.objectsAlive = objectsAlive - 1.0;
	safeSchedule("500", spawner, "spawn");
	return;
}
