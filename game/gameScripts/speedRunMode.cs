// speedRunMode.cs.dso
function SpeedRunMode::init()
{
	if (!(isObject(SpeedRunMode)))
	{
		new SpeedRunMode(Name : SpeedRunMode);
	}
	%this.oldTimes = -1.0;
	lbl_sp_time_diff.setVisible("0");
	return;
}
function SpeedRunMode::setupLevel(%this, )
{
	return;
}
function SpeedRunMode::setSpawnPointTimes(%this, %spawnpointTimes)
{
	%this.oldTimes = %spawnpointTimes;
	return;
}
function SpeedRunMode::checkDisplay(%this)
{
	if (!(active))
	{
		Canvas.popDialog(time_display);
	}
	return;
}
function SpeedRunMode::onLevelLoadFinished(%this)
{
	Canvas.pushDialog(time_display);
	subscribeToEvents(%this, "onUpdateTick20 onSpawnPointActivate");
	if (!(active) || competeWithoutTimes)
	{
		debugEcho("speedrunmode is not active" SPC %this);
		return;
	}
	echo("starting speedrunmode");
	%this.noOldSpawnPointTimes = "0";
	%this.spawnPoints = getAllSpawnPoints();
	if (oldTimes < 0.0)
	{
		%this.noOldSpawnPointTimes = "1";
		%this.oldTimes = "";
		%i = 0;
		while (%i < getWordCount(spawnPoints))
		{
			getWord(spawnPoints, %i).showTime("--:--:--");
			%this.oldTimes = oldTimes SPC "*";
			%i = %i + 1.0;
		}
		ltrim(oldTimes);
	}
	%this.oldSpawnPointTimes = oldTimes;
	return;
}
function SpeedRunMode::onSpawnpointActivate(%this)
{
	%spawnpointBehavior = spawnPoint.getBehavior("BeSpawnPoint");
	%sPNumber = number;
	if (%sPNumber == 1.0)
	{
		return;
	}
	%time = timeEntered + Statistics.getEnvironmentLevelOffset();
	%oldTime = getWord(oldTimes, %sPNumber - 1.0);
	debugEcho("at spP" SPC %sPNumber SPC "oT:" SPC %oldTime SPC "t:" SPC %time);
	if (active && !(competeWithoutTimes))
	{
		if (oldTimes == -1.0 || %oldTime $= "*")
		{
			%prefix = "";
			%timeDiff = "*";
		}
		else
		{
			if (%oldTime < %time)
			{
				%prefix = "+";
				%timeDiff = %time - %oldTime;
				%color = "190 10 10";
				break;
			}
			%prefix = "-";
			%timeDiff = %oldTime - %time;
			%color = "10 150 10";
		}
		%readableTime = %prefix @ Statistics.getReadableTime(%timeDiff);
		spawnPoint.getBehavior("BeSpawnPoint").showTime(%readableTime);
		lbl_sp_time_diff.setVisible("1");
		lbl_sp_time_diff.setText(%readableTime);
		bckgrnd_time.setPosition(%this.getXoffset(), -20.0);
		AyimTimeDiffTextProfile.fontColor = %color;
		%this.timeDiffSchedule = %this.schedule("5000", "hideTimeDiff");
	}
	return;
}
function SpeedRunMode::stopTime(%this)
{
	unSubscribeFromEvents(%this, "onUpdateTick20");
	if ($watchAsReplay)
	{
		%endTime = getWord(oldTimes, getWordCount(oldTimes) - 1.0);
		%levelSwitchSpawnPoint = spawnPoint.getBehavior("BeSpawnPoint");
		lbl_current_time.setText(Statistics.getReadableTime(%endTime));
		%levelSwitchSpawnPoint.showTime("+" @ Statistics.getReadableTime("0"));
	}
	else
	{
		lbl_current_time.setText(Statistics.getReadableTime(totalTime));
	}
	return;
}
function SpeedRunMode::getXoffset(%this)
{
	%backgroundX = getX(getRes()) - getX(bckgrnd_time.getExtent()) + 50.0;
	return %backgroundX;
	return %backgroundX;
}
function SpeedRunMode::hideTimeDiff(%this)
{
	lbl_sp_time_diff.setVisible("0");
	bckgrnd_time.setPosition(%this.getXoffset(), -70.0);
	return;
}
function SpeedRunMode::setupSpeedRun(%this, %level, %wholeEnvironment, %ghost, %loadGhostForSure, %playingLocal)
{
	echo("speedRunMode:setupSpeedRun" SPC "level:" SPC %level SPC "wholeEnv:" SPC %wholeEnvironment SPC "ghost:" SPC %ghost SPC "loadGhost:" SPC %loadGhostForSure SPC "local?" SPC %playingLocal);
	SpeedRunMode.active = "1";
	Statistics.playingLocal = %playingLocal;
	if (%wholeEnvironment)
	{
		Statistics.wholeEnvironment = "1";
		Statistics.environmentGhosts = -1.0;
		Statistics.environment = getLastToken(%level, "_");
		%level = %level @ "1";
	}
	%endings = "pos rot evt";
	%i = 0;
	while (%i < getWordCount(%endings))
	{
		fileDelete(getGhostFilePath("loaded", "ghost", getWord(%endings, %i)));
		%i = %i + 1.0;
	}
	if (%loadGhostForSure)
	{
		%ghostFile = -1.0;
		if (%wholeEnvironment)
		{
			%ghostLevel = environment;
			%ghostFile = "ghost0";
			if (environment $= "cave")
			{
				%ghostCount = 4;
			}
			else
			{
				%ghostCount = 6;
			}
			%zipObject = new ZipObject(Name : "");
			if (%playingLocal)
			{
				%archivePath = getGhostFilePath(environment, %ghost);
			}
			else
			{
				if (Webclient.loadGhost(%ghost))
				{
					%archivePath = getGhostFilePath("loaded", "ghost");
					break;
				}
				%archivePath = "";
			}
			%zipObject.openArchive(%archivePath);
			%i = 0;
			while (%i < %zipObject.getFileEntryCount())
			{
				%ghostName = "ghost" @ %i;
				%zipObject.extractFile(%ghostName @ ".gst", getGhostFilePath(environment, %ghostName));
				%i = %i + 1.0;
			}
			%zipObject.closeArchive();
			%zipObject.delete();
			Statistics.environmentGhosts = "ghost0";
			%i = 1;
			while (%i < %ghostCount)
			{
				Statistics.environmentGhosts = environmentGhosts SPC "ghost" SPC %i;
				%i = %i + 1.0;
			}
		}
		else
		{
			if (%playingLocal)
			{
				%ghostLevel = getLastToken(%level, "_");
				%ghostFile = %ghost;
				break;
			}
			if (Webclient.loadGhost(%ghost))
			{
				%ghostLevel = "loaded";
				%ghostFile = "ghost";
				break;
			}
			echo("WARNING: couldnt load online ghost with index" SPC %ghost);
		}
		if (%ghostFile == -1.0)
		{
			echo("WARNING: menuAction::loadLevel couldnt load ghost" SPC %ghostFile SPC "of level" SPC %ghostLevel);
			if (isObject($ghostToLoad))
			{
				$ghostToLoad.delete();
			}
			$ghostToLoad = -1.0;
		}
		else
		{
			debugEcho("MenuAction::loadLevel: local?:" SPC %playingLocal SPC "loading this ghost:" SPC %ghostLevel SPC "/" SPC %ghostFile);
			if (isObject($ghostToLoad))
			{
				$ghostToLoad.delete();
			}
			$ghostToLoad = Replay::load(%ghostLevel, %ghostFile);
		}
	}
	else
	{
		if (isObject($ghostToLoad))
		{
			$ghostToLoad.delete();
		}
		$ghostToLoad = -1.0;
	}
	return;
}
function SpeedRunMode::disableMode(%this)
{
	cancel(timeDiffSchedule);
	Canvas.popDialog(time_display);
	return;
}
