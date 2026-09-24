// spawnBananas.cs.dso
if (!(isObject(BeSpawnBananas)))
{
	%template = new BehaviorTemplate(Name : BeSpawnBananas);
	%template.friendlyName = "SpawnBananas";
	%template.behaviorType = "LevelJungle";
	%template.description = "user friendly description of the behavior";
}
function BeSpawnBananas::onBehaviorAdd(%this)
{
	subscribeToEvents(%this, "onLevelLoadFinished5");
	return;
}
function BeSpawnBananas::onLevelLoadFinished5(%this)
{
	%owner = Owner;
	%spawn = %owner.addDependentBehavior("BeSpawnObjects");
	%spawn.maxAliveObjects = "1";
	%spawn.spawnInterval = -1.0;
	%activateBehavior = %owner.addDependentBehavior("BeActivate");
	%activateBehavior.activator = "viewWindowTrigger";
	%spawn.objectList = getObjectsWithBehavior("BeBanana");
	%i = 0;
	while (%i < getWordCount(objectList))
	{
		%obj = getWord(objectList, %i);
		%obj.setCollisionSuppress("1");
		%obj.setName("originalBanana");
		%i = %i + 1.0;
	}
	return getWordCount(objectList);
}
