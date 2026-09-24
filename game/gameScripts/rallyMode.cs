// rallyMode.cs.dso
function RallyMode::init()
{
	if (!(isObject(RallyMode)))
	{
		new RallyMode(Name : RallyMode);
	}
	RallyMode.achievementIndex = "24";
	return;
}
function RallyMode::setupLevel(%this)
{
	debugEcho("setup rallyMode");
	if (getWordCount(rallyTimes) <= 1.0)
	{
		debugWarn("WARNING: there are no rallyTimes set for this level:" SPC levelName SPC "cancelling rallyMode");
		%this.checkPoints = "-1";
		%this.disableMode();
		return;
	}
	%this.checkPoints = %this.getLimit(difficulty);
	lbl_rally.setText("");
	AyimTimeDiffTextProfile.fontColor = "10 150 10";
	return;
}
function RallyMode::getLimit(%this, %difficulty)
{
	%checkPoints = "0" SPC rallyTimes;
	%i = 1;
	while (%i < getWordCount(%checkPoints))
	{
		%checkpointTime = getWord(%checkPoints, %i);
		%checkpointTime = mRound(%checkpointTime * 1.0 + %difficulty * 0.25);
		%checkPoints = setWord(%checkPoints, %i, %checkpointTime);
		%i = %i + 1.0;
	}
	return %checkPoints;
	return %checkPoints;
}
function RallyMode::beatDifficulty(%this, %difficulty)
{
	%difficultyTime = %this.getLimit(%difficulty);
	%skippedSp = 0;
	%i = 1;
	while (%i < getWordCount(spawnpointTimes))
	{
		%myTime = getWord(spawnpointTimes, %i);
		if (!(%skippedSp))
		{
			%limitTime = getWord(%difficultyTime, %i);
		}
		if (%myTime $= "*")
		{
			%skippedSp = 1;
		}
		else
		{
			%skippedSp = 0;
			if (%myTime > %limitTime)
			{
				return "0";
			}
		}
		%i = %i + 1.0;
	}
	return "1";
	return "1";
}
function RallyMode::onLevelLoadFinished(%this)
{
	subscribeToEvents(%this, "onMenuReplayLevel onFirstKeyPressed onSpawnpointEnter");
	%spawnPoints = getAllSpawnPoints();
	%this.spawnPoints = %spawnPoints;
	%this.spawnPointsBackup = %spawnPoints;
	%this.lastTimeWasLeft = "0";
	Canvas.pushDialog(rally_display);
	if (getWordCount(checkPoints) != getWordCount(spawnPoints))
	{
		debugWarn("WARNING: number of rallyTimes doesn't match the number of spawnpoints!" SPC getWordCount(checkPoints) SPC "/" SPC getSpawnPointCount() SPC "/" SPC getWordCount(spawnPoints));
		%this.checkPoints = "-1";
		%this.disableMode();
		return;
	}
	%this.onUpdateTick20();
	return;
}
function RallyMode::onFirstKeyPressed(%this)
{
	if (checkPoints $= "-1")
	{
		return;
	}
	subscribeToEvents(%this, "onUpdateTick20 onSpawnPointActivate");
	%this.onSpawnpointActivate();
	return;
}
function RallyMode::onSpawnpointEnter(%this)
{
	if (activated || hasEntered)
	{
		return %this;
	}
	%enterTime = Statistics.getLevelTime();
	%thisTime = getWord(checkPoints, number - 1.0);
	if (%thisTime > -1.0 && %enterTime > %thisTime)
	{
		cancel(failSchedule);
		%this.fail();
		return;
	}
	if (!(isFalling))
	{
		return player;
	}
	%this.hasEntered = "1";
	cancel(failSchedule);
	subscribeToEvents(%this, "onPlayerLanded onPlayerDeath");
	return;
}
function RallyMode::onPlayerLanded(%this)
{
	%this.hasEntered = "0";
	unSubscribeFromEvents(%this, "onPlayerLanded onPlayerDeath");
	return;
}
function RallyMode::onPlayerDeath(%this)
{
	%this.hasEntered = "0";
	unSubscribeFromEvents(%this, "onPlayerLanded onPlayerDeath");
	%this.onSpawnpointActivate();
	return;
}
function RallyMode::onSpawnpointActivate(%this)
{
	%isLevelSwitchPoint = isLevelSwitchSpawnPoint;
	if (%isLevelSwitchPoint)
	{
		%actualNumber = getWordCount(checkPoints) - 1.0;
	}
	else
	{
		%actualNumber = number;
	}
	%actualTime = Statistics.getLevelTime();
	%nextTime = getWord(checkPoints, %actualNumber);
	%timeLeft = %nextTime - %actualTime;
	%spawnpointBehavior = spawnPoint.getBehavior("BeSpawnPoint");
	%timeWasLeft = getWord(checkPoints, %actualNumber - 1.0) - timeEntered;
	%targetTimeDiff = %timeWasLeft - lastTimeWasLeft;
	debugEcho("got spP" SPC %actualNumber - 1.0 SPC "@" SPC timeEntered SPC "time left" SPC %timeWasLeft SPC "changed" SPC %targetTimeDiff);
	%this.lastTimeWasLeft = %timeWasLeft;
	cancel(failSchedule);
	if (%timeLeft < 0.0)
	{
		%this.fail();
		return;
	}
	if (%isLevelSwitchPoint)
	{
		unSubscribeFromEvents(%this, "onUpdateTick20");
	}
	else
	{
		%this.failSchedule = %this.schedule(%timeLeft * 1000.0 + 100.0, "fail");
	}
	return;
}
function RallyMode::fail(%this)
{
	playmodeManager.disableMode();
	return;
}
function RallyMode::disableMode(%this)
{
	cancel(failSchedule);
	unSubscribeFromEvents(%this, "onMenuReplayLevel onUpdateTick20 onSpawnpointEnter onSpawnPointActivate");
	%i = 0;
	while (%i < getWordCount(spawnPointsBackup))
	{
		%spawnPoint = getWord(spawnPointsBackup, %i).getBehavior("BeSpawnPoint");
		%spawnPoint.hideTimeDisplay();
		%i = %i + 1.0;
	}
	Canvas.popDialog(rally_display);
	return;
}
