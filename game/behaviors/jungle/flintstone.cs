// flintstone.cs.dso
if (!(isObject(BeFlintstone)))
{
	%template = new BehaviorTemplate(Name : BeFlintstone);
	%template.friendlyName = "Flintstone";
	%template.behaviorType = "LevelJungle";
	%template.description = "a stone, that creates sparks, when it falls on a pyrite";
}
function BeFlintstone::onBehaviorAdd(%this)
{
	%owner = Owner;
	%movingBehavior = %owner.addDependentBehavior("BeMoving");
	%movingBehavior.Density = "0.02";
	%movingBehavior.Friction = "0.25";
	%movingBehavior.Restitution = "0";
	%movingBehavior.influencedByPlayer = "1";
	%owner.cantSquashPlayer = "1";
	%this.doSparks = "2";
	%this.madeSparks = "0";
	subscribeToEvent(%this, "onLevelLoadFinished12");
	return;
}
function BeFlintstone::onBehaviorRemove(%this)
{
	if (isObject(viewSafeDeleteSchedule))
	{
		viewSafeDeleteSchedule.cancelSchedule();
	}
	if (isObject(spawner))
	{
		spawner.objectsAlive = objectsAlive - 1.0;
	}
	return spawner;
}
function BeFlintstone::onLevelLoadFinished12(%this)
{
	%owner = Owner;
	%owner.setGraphGroup($GROUPS["flintstone"]);
	%owner.addCollisionGroups("flintstone");
	%this.madeSpark = "0";
	unSubscribeFromEvent(%this, "onLevelLoadFinished12");
	%this.originalPosition = %owner.getPosition();
	return;
}
function BeFlintstone::madeSpark(%this)
{
	%owner = Owner;
	%this.activate("0");
	%this.madeSparks = madeSparks + 1.0;
	if (madeSparks < doSparks)
	{
		%scaleFactor = 1.0 - madeSparks / doSparks * 1.600000023841858;
		%owner.setSize(t2dVectorScale("16 16", %scaleFactor));
		%mountedKids = %owner.getMountedChildren();
		%i = 0;
		while (%i < getWordCount(%mountedKids))
		{
			%mountedChild = getWord(%mountedKids, %i);
			%mountedChild.setSize(t2dVectorScale("16 16", %scaleFactor));
			%i = %i + 1.0;
		}
		safeSchedule("300", %this, "activate", "true");
	}
	else
	{
		%owner.safeDelete();
	}
	return;
}
function BeFlintstone::activate(%this, %active)
{
	%this.madeSpark = !(%active);
	return;
}
function BeFlintstone::isActive(%this)
{
	return !(madeSpark);
	return !(madeSpark);
}
function BeFlintstone::onSpawnFinished(%this, %spawner)
{
	%owner = Owner;
	%owner.setImmovable("0");
	%this.viewSafeDeleteSchedule = safeSchedule("2000", %owner, "viewSafeDelete", "10");
	%this.spawner = %spawner;
	return;
}
