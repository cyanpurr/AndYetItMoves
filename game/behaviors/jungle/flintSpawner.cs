// flintSpawner.cs.dso
if (!(isObject(BeFlintSpawner)))
{
	%template = new BehaviorTemplate(Name : BeFlintSpawner);
	%template.friendlyName = "FlintSpawner";
	%template.behaviorType = "LevelJungle";
	%template.description = "will trigger a spawn every 15 sec. as long as the durchgang is still closed";
	%template.addBehaviorField(Earthquake, "shoudl we do an earthquake or spawn breakable stones?", bool, "1");
	%template.addBehaviorField(barricade, "the barricade. as long as this is alive it will spawn", object, "barricade", t2dSceneObject);
}
function BeFlintSpawner::onBehaviorAdd(%this)
{
	subscribeToEvent(%this, "onLevelLoadFinished10");
	return;
}
function BeFlintSpawner::onBehaviorRemove(%this)
{
	%i = 0;
	while (%i < getWordCount(spawnerList))
	{
		getWord(spawnerList, %i).safeDelete();
		%i = %i + 1.0;
	}
	if (isObject(spawnSchedule))
	{
		spawnSchedule.cancelSchedule();
	}
	return;
}
function BeFlintSpawner::onLevelLoadFinished10(%this)
{
	%owner = Owner;
	%owner.escapeSwitch = "1";
	%spawnerList = getObjectsWithClass("flintstoneSpawner");
	%i = 0;
	while (%i < getWordCount(%spawnerList))
	{
		%obj = getWord(%spawnerList, %i);
		if (%owner.getIsPointInObject(%obj.getPosition()))
		{
			%this.spawnerList = spawnerList SPC %obj;
		}
		%i = %i + 1.0;
	}
	%this.spawnerList = ltrim(spawnerList);
	%this.spawnInterval = "5";
	return;
}
function BeFlintSpawner::switchOn(%this)
{
	%i = 0;
	while (%i < getWordCount(spawnerList))
	{
		getWord(spawnerList, %i).switchOn();
		%i = %i + 1.0;
	}
	%this.spawn();
	return;
}
function BeFlintSpawner::switchOff(%this)
{
	%i = 0;
	while (%i < getWordCount(spawnerList))
	{
		getWord(spawnerList, %i).switchOff();
		%i = %i + 1.0;
	}
	if (isObject(spawnSchedule))
	{
		spawnSchedule.cancelSchedule();
	}
	return;
}
function BeFlintSpawner::spawn(%this)
{
	%owner = Owner;
	if (!(isObject(barricade)))
	{
		Owner.safeDelete();
		return;
	}
	%i = 0;
	while (%i < getWordCount(spawnerList))
	{
		%didspawn = getWord(spawnerList, %i).getBehavior("BeSpawnObjects").spawn();
		if (%didspawn && Earthquake)
		{
			camera.shake("20", "2");
		}
		%i = %i + 1.0;
	}
	%this.spawnSchedule = %owner.safeSchedule(spawnInterval * 1000.0, "spawn");
	return;
}
function BeFlintSpawner::onSpawnFinished(%this, %spawnedObject)
{
	%owner = Owner;
	if (Earthquake)
	{
		%spawnedObject.setImmovable("0");
	}
	else
	{
		%spawnedObject.setImmovable("0");
	}
	return;
}
