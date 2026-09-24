// spawnStones.cs.dso
$firstSpawn = 1;
if (!(isObject(BeSpawnStones)))
{
	if (!(isTorquePlayer()))
	{
		%template = new BehaviorTemplate(Name : BeSpawnStones);
	}
	else
	{
		%template = new BeSpawnStonesTemplate(Name : BeSpawnStones);
	}
	%template.friendlyName = "spawnStones";
	%template.behaviorType = "LevelPrototype";
	%template.description = "user friendly description of the behavior";
	if (!(isTorquePlayer()))
	{
		%template.addBehaviorField(spawnInterval, "how often the object should spawnStones stones (in s)", float, "2");
		%template.addBehaviorField(spawnIntervalVariance, "how much the intervall should randomly differ (in s; [spawnInterval - spawnIntervalVariance; spawnInterval + spawnIntervalVariance])", float, "2");
		%template.addBehaviorField(spawnPositionVariance, "how much the position will vary", float, "40");
		%template.addBehaviorField(spawnAroundPlayer, "if we should also spawn stones in front or behind the player (at the same intervall, variance as the one on top of player).", bool, "1");
		%template.addBehaviorField(stoneList, "a list of stones that will be cloned, if empty it will get all objects with class Stone", string, "");
		%template.addBehaviorField(maxAliveObjects, "how many objects should be alive at most", integer, "6");
		%template.addBehaviorField(autoStart, "if the object should start spawnStonesing stones right away", bool, "0");
	}
}
function BeSpawnStones::onBehaviorAdd(%this)
{
	if (spawnIntervalVariance > spawnInterval)
	{
		%this.spawnIntervalVariance = spawnInterval;
		debugWarn("spawnIntervalVariance too big; it could lead to a negative spawnStonesIntervall.");
		debugWarn("setting spawnIntervalVariance to next possible value:" SPC spawnIntervalVariance);
	}
	subscribeToEvent(%this, "onLevelLoadFinished10");
	return;
}
function BeSpawnStones::onLevelLoadFinished10(%this)
{
	%this.stoneList = getObjectsWithClass("Stone");
	if (autoStart)
	{
		%this.switchOn();
	}
	return;
}
function BeSpawnStones::switchOn(%this)
{
	if (spawning)
	{
		return %this;
	}
	if (isRotating)
	{
		Owner.safeSchedule("300", "switchOn");
		return;
	}
	if (spawnAroundPlayer)
	{
	}
	else
	{
	}
	%spawnPositions = 1;
	%i = 0;
	while (%i < %spawnPositions)
	{
		%this.spawnPositions[%i] = new spawnPosition(Name : "")
		{
			scenegraph = daSceneGraph;
			size = spawnPositionVariance SPC spawnPositionVariance;
			stoneList = stoneList;
			spawnInterval = spawnInterval;
			spawnIntervalVariance = spawnIntervalVariance;
			spawner = %this;
			_behavior0 = "BeDontCollide";
		}
		%newPos = "0 0";
		if (%i == 0.0)
		{
			%newPos = setHorizontalComponent(%newPos, getHorizontalComponent(player.getPosition()));
			if (camera.getCurrentRotation() % 180 == 0.0)
			{
				spawnPositions["0"].setWidth("30");
			}
			else
			{
				spawnPositions["0"].setHeight("30");
			}
		}
		else
		{
			if (%i % 2 == 0.0)
			{
				%newPos = setHorizontalComponent(%newPos, getHorizontalComponent(player.getPosition()) - 100.0);
				break;
			}
			%newPos = setHorizontalComponent(%newPos, getHorizontalComponent(player.getPosition()) + 70.0);
		}
		%newPos = setVerticalComponent(%newPos, getVerticalComponent(viewWindow.getWorldPoint("0 -1")) - spawnPositionVariance / 2.0);
		spawnPositions[%i].setPosition(%newPos);
		%mountBehavior = spawnPositions[%i].addDependentBehavior("BeMountWithOffset");
		%mountBehavior.mother = viewWindow;
		%mountBehavior.MountOwned = "1";
		%mountBehavior.trackRotation = "1";
		%mountBehavior.init();
		%mountBehavior.mountWithOffset();
		%i = %i + 1.0;
	}
	%this.spawning = "1";
	%this.scheduleNextSpawn();
	return;
}
function BeSpawnStones::switchOff(%this)
{
	%this.spawning = "0";
	if (isObject(spawnPositions["0"]))
	{
		if (spawnAroundPlayer)
		{
		}
		else
		{
		}
		%spawnPositions = 1;
		%i = 0;
		while (%i < %spawnPositions)
		{
			if (isObject(spawnSchedule))
			{
				spawnSchedule.cancelSchedule();
			}
			spawnPositions[%i].safeDelete();
			%i = %i + 1.0;
		}
	}
	return;
}
function BeSpawnStones::scheduleNextSpawn(%this)
{
	if (spawnAroundPlayer)
	{
	}
	else
	{
	}
	%spawnPositions = 1;
	%i = 0;
	while (%i < %spawnPositions)
	{
		if (spawnIntervalVariance)
		{
		}
		else
		{
		}
		%nextSpawn = spawnInterval;
		spawnPositions[%i].spawnSchedule = safeSchedule(%nextSpawn * 1000.0, spawnPositions[%i], "spawnStone");
		%i = %i + 1.0;
	}
	return;
}
function RockMass::onLevelLoaded(%this, )
{
	%this.setGraphGroup($GROUPS["rockMass"]);
	return;
}
