// statistics.cs.dso
function statsTest()
{
	%lines = "Version 1.10" NL "userID 1	time 38.064	deaths 0	spawnpointTimes * 8.27192 16.784 24.8164 38.064	ghostID 2" NL "userID 1	time 48.064	deaths 0	spawnpointTimes * 8.27192 16.784 34.8164 48.064	ghostID 3";
	writeFile(Statistics.getStatsFilePath("cave4"), %lines);
	%saveData = Statistics.getSavedTimes("cave4");
	echo("got this from getSavedTimes:" SPC %saveData SPC "end");
	return;
}
function Statistics::init()
{
	if (!(isObject(Statistics)))
	{
		new Statistics(Name : Statistics);
	}
	globals.setStatistics(Statistics);
	%i = 0;
	while (%i < getWordCount($LEVELLIST))
	{
		%levelName = getWord($LEVELLIST, %i);
		Statistics.levelBestTimes[%levelName] = "";
		%i = %i + 1.0;
	}
	return getWordCount($LEVELLIST);
}
function Statistics::onLevelLoadFinished40(%this)
{
	%curSpawnpointTimes = getField(Statistics.getSpawnpointTimes(oldData), environmentLevel);
	if (%curSpawnpointTimes $= "")
	{
		%curSpawnpointTimes = -1.0;
	}
	SpeedRunMode.setSpawnPointTimes(%curSpawnpointTimes);
	return;
}
function Statistics::onLevelLoadFinished(%this)
{
	if (!(wholeEnvironment))
	{
		%this.spawnpointTimes = "";
		%i = 0;
		while (%i < getSpawnPointCount())
		{
			%this.spawnpointTimes = spawnpointTimes SPC "*";
			%i = %i + 1.0;
		}
		%this.spawnpointTimes = ltrim(spawnpointTimes);
	}
	else
	{
		if (environmentLevel == 0.0)
		{
			%this.spawnpointTimes = "";
		}
		%i = 0;
		while (%i < getSpawnPointCount())
		{
			%spawnpointTimes = %spawnpointTimes SPC "*";
			%i = %i + 1.0;
		}
		%this.spawnpointTimes = setField(spawnpointTimes, environmentLevel, ltrim(%spawnpointTimes));
	}
	subscribeToEvents(%this, "onFirstKeyPressed onLevelShutdown onSpawnPointActivate onPlayerDeath onEnterLevelSwitch onLevelFadeOutEnded");
	%this.levelStarted = "0";
	%this.endedAtTime = "0";
	return;
}
function Statistics::onFirstKeyPressed(%this)
{
	%this.startedWithOffest = scenegraph.getSceneTime();
	%this.levelStarted = "1";
	%this.playerDeathCount = "0";
	return;
}
function Statistics::onPlayerDeath(%this)
{
	%this.playerDeathCount = playerDeathCount + 1.0;
	return %this;
}
function Statistics::getDeathCount(%this)
{
	return playerDeathCount;
	return playerDeathCount;
}
function Statistics::onEnterLevelSwitch(%this)
{
	%this.endedAtTime = scenegraph.getSceneTime();
	currentLevelObject.totalTime = %this.getLevelTime();
	currentLevelObject.deathCount = playerDeathCount;
	ghostRecorder.stopRecording("1");
	if (wholeEnvironment)
	{
		if (oldData == -1.0)
		{
			%this.environmentTimeOffset = "0";
		}
		else
		{
			%this.environmentTimeOffset = totalTime - getWord(%this.getLevelTimes(oldData), environmentLevel) + environmentTimeOffset;
		}
		%this.totalTime = totalTime + totalTime;
		%this.deathCount = deathCount + deathCount;
		if (jumpedSpawnPointInEvironment)
		{
			$cheatsWereUsed = 1;
		}
		else
		{
			if ($cheatsWereUsed)
			{
				Statistics.jumpedSpawnPointInEvironment = "1";
			}
		}
	}
	else
	{
		%this.totalTime = totalTime;
		%this.deathCount = deathCount;
	}
	SpeedRunMode.stopTime();
	return;
}
function Statistics::onLevelFadeOutEnded(%this)
{
	ghostRecorder.stopRecording();
	ghostRecorder.saveGhost("0");
	%this.ghostsToSubmit = trim(ghostsToSubmit SPC $settings::Ghost::highestID);
	if ($cheatsWereUsed)
	{
		echo("you cheated! no data saved!");
		return;
	}
	if ($settings::Secrets::LevelsUnlocked < currentLevelNumber - 1.0)
	{
		setSecretData("LevelsUnlocked", currentLevelNumber - 1.0);
	}
	setSecretData("TotalPlayTime", $settings::Secrets::TotalPlayTime + totalTime);
	setSecretData("TotalDeathCount", $settings::Secrets::TotalDeathCount + deathCount);
	%this.saveBestTime();
	subscribeToEvents(%this, "onLevelCompleted");
	if (wholeEnvironment && endOfEnvironment)
	{
		%levelName = getLastToken(currentLevelObject.getName(), "_");
		%levelName = getSubStr(%levelName, "0", strpos(%levelName, environmentLevel + 1.0));
		%this.saveBestTime(%levelName);
		triggerEvent("onEnvironmentCompleted");
	}
	unSubscribeFromEvents(%this, "onLevelShutdown");
	subscribeToEvents(%this, "onSave");
	return;
}
function Statistics::onLevelCompleted(%this)
{
	if (!(active) && !(wholeEnvironment))
	{
		Webclient.setPermanentFilter("level", levelID);
		%this.submitOnline();
		Statistics.reset();
	}
	return;
}
function Statistics::onSave(%this)
{
	if (!($WII))
	{
		saveSettings(-1.0);
	}
	saveSettings($settings::Profile::currentProfile);
	return;
}
function Statistics::onLevelShutdown(%this)
{
	%this.endedAtTime = scenegraph.getSceneTime();
	currentLevelObject.totalTime = %this.getLevelTime();
	currentLevelObject.deathCount = playerDeathCount;
	%saveAtTheEnd = !(isMenu);
	debugEcho("statistics onlSd" SPC %this SPC currentLevelObject SPC totalTime);
	setSecretData("TotalPlayTime", $settings::Secrets::TotalPlayTime + totalTime);
	setSecretData("TotalDeathCount", $settings::Secrets::TotalDeathCount + deathCount, %saveAtTheEnd);
	return;
}
function Statistics::reset(%this, %levelReplayed)
{
	%this.deleteGhosts(ghostsToSubmit SPC "environmentGhost", "toSubmit");
	%this.ghostsToSubmit = "";
	%this.totalTime = "0";
	%this.deathCount = "0";
	%this.environmentLevel = "0";
	%this.environmentTimeOffset = "0";
	%this.jumpedSpawnPointInEvironment = "0";
	if (!($WII) && !(%levelReplayed))
	{
		Statistics.environment = "";
		%this.wholeEnvironment = "0";
	}
	return;
}
function Statistics::submitOnline(%this)
{
	if ($WII || $distributorName $= "BigFishGames" || $currentVersion < Webclient.getCurrentVersion())
	{
		return Webclient.getCurrentVersion();
	}
	if (totalTime < 0.10000000149011612)
	{
		return %this;
	}
	if (isExternalUser(-1.0) && getPublisherName() $= "Greenhouse" && !($cheatsWereUsed) && !($enableDebugMap))
	{
		Webclient.submitToGreenhouse();
	}
	if ($settings::Statistics::SubmitTime && Webclient.isOnline() && !($cheatsWereUsed))
	{
		if (wholeEnvironment)
		{
			%zipFile = new ZipObject(Name : "");
			%saveToDir = getGhostFilePath("toSubmit");
			assertDirectory(%saveToDir);
			%zipFilePath = getGhostFilePath("toSubmit", "environmentGhost");
			%zipFile.openArchive(%zipFilePath, "write");
			%i = 0;
			while (%i < getWordCount(ghostsToSubmit))
			{
				%zipFile.addFile(getGhostFilePath("toSubmit", getWord(ghostsToSubmit, %i)), "ghost" @ %i @ ".gst");
				%i = %i + 1.0;
			}
			%zipFile.closeArchive();
			%zipFile.delete();
			Statistics.ghostFileToSubmit = %zipFilePath;
		}
		else
		{
			Statistics.ghostFileToSubmit = getGhostFilePath("toSubmit", ghostsToSubmit);
		}
		Webclient.submit($settings::Statistics::SubmitGhost);
	}
	return;
}
function Statistics::setUserBestScore(%this, %levelName, %time)
{
	debugEcho("comparing" SPC %time SPC "to" SPC $settings::Secrets::BestScores SPC "in level" SPC %levelName);
	%allLevels = "cave1 cave2 cave3 cave4 cave jungle1 jungle2 jungle3 jungle4 jungle5 jungle6 jungle trip1 trip2 trip3 trip4 trip5 trip6 trip finalLevel";
	%levelIndex = getWordIndex(%allLevels, %levelName);
	%bestSavedTime = getWord($settings::Secrets::BestScores, %levelIndex);
	if (%levelIndex >= 0.0 && %time < %bestSavedTime || %bestSavedTime $= "")
	{
		$settings::Secrets::BestScores = setWord($settings::Secrets::BestScores, %levelIndex, %time);
	}
	return;
}
function Statistics::getUserBestScore(%this, %levelName)
{
	%allLevels = "cave1 cave2 cave3 cave4 cave jungle1 jungle2 jungle3 jungle4 jungle5 jungle6 jungle trip1 trip2 trip3 trip4 trip5 trip6 trip finalLevel";
	%levelIndex = getWordIndex(%allLevels, %levelName);
	%bestTime = getWord($settings::Secrets::BestScores, %levelIndex);
	if (%bestTime $= "")
	{
		return -1.0;
	}
	else
	{
		return %bestTime;
	}
	return %bestTime;
}
function Statistics::saveBestTime(%this, %environmentName)
{
	%currentTime = totalTime;
	if (%currentTime < 0.10000000149011612)
	{
		return;
	}
	if (%environmentName != "")
	{
		%currentTime = totalTime;
	}
	if (%environmentName $= "")
	{
		%levelName = strchr(currentLevelObject.getName(), "_");
		if (strlen(%levelName) > 0.0)
		{
			%levelName = getSubStr(%levelName, "1", strlen(%levelName) - 1.0);
		}
	}
	else
	{
		%levelName = %environmentName;
	}
	%this.setUserBestScore(%levelName, %currentTime);
	%bestTimes = %this.getSavedTimes(%environmentName, "1");
	if (%bestTimes == -1.0)
	{
		%bestTimes = "";
	}
	%limit = getRecordCount(%bestTimes);
	%this.rank = $lbl_notRanked;
	%newBestTimes = "";
	%i = 0;
	while (%i < %limit)
	{
		if (getRecordCount(%newBestTimes) > 0.0)
		{
			%newBestTimes = setRecord(%newBestTimes, %i, getRecord(%bestTimes, %i - 1.0));
		}
		else
		{
			if (%currentTime < %this.getTime(getRecord(%bestTimes, %i)))
			{
				%newBestTimes = setRecord(%bestTimes, %i, %this.getBestTimeData(%environmentName));
				%this.rank = %i + 1.0;
				%limit = %limit + 1.0;
			}
		}
		%i = %i + 1.0;
	}
	if (getRecordCount(%newBestTimes) == 0.0 && getRecordCount(%bestTimes) < 10.0)
	{
		%bestTimes = trim(%bestTimes NL %this.getBestTimeData(%environmentName));
		%this.rank = getRecordCount(%bestTimes);
	}
	%bestTimes = trim(%bestTimes);
	if (getRecordCount(%newBestTimes) > 0.0)
	{
	}
	else
	{
	}
	%bestTimes = %bestTimes;
	if (getRecordCount(%bestTimes) > 10.0)
	{
		%limit = 10;
	}
	else
	{
		%limit = getRecordCount(%bestTimes);
	}
	%i = 0;
	while (%i < %limit)
	{
		%lines = %lines NL getRecord(%bestTimes, %i);
		%i = %i + 1.0;
	}
	%lines = ltrim(%lines);
	if (!($WII))
	{
		if (getRecordCount(%bestTimes) > %limit)
		{
			%this.deleteGhosts(%this.getGhost(getRecord(%bestTimes, %limit)), %environmentName);
		}
	}
	%this.writeBestTimes(%levelName, %lines);
	if (rank != $lbl_notRanked)
	{
		debugEcho("highscore! rank" SPC rank SPC "level" SPC %environmentName SPC "time" SPC %currentTime SPC "sp times" SPC spawnpointTimes);
	}
	else
	{
		debugEcho("no highscore :( rank" SPC rank SPC "level" SPC %environmentName SPC "time" SPC %currentTime SPC "sp times" SPC spawnpointTimes);
	}
	return;
}
function Statistics::writeBestTimes(%this, %levelName, %lines)
{
	Statistics.levelBestTimes[%levelName] = %lines;
	%lines = "Version" SPC $currentVersion NL %lines;
	%saveFilePath = %this.getStatsFilePath(%levelName);
	writeFile(%saveFilePath, %lines, "0", "1");
	return;
}
function Statistics::getBestTimeData(%this, %levelName)
{
	if (%levelName $= "cave" || %levelName $= "jungle" || %levelName $= "trip")
	{
		ghostRecorder.saveEnvironmentGhosts(ghostsToSubmit, %levelName);
		%ghostID = $settings::Ghost::highestID;
		%spawnpointTimes = strreplace(spawnpointTimes, $tab, "#");
		%time = totalTime;
		debugEcho("saving for whole environemnt gIDs:" SPC %ghostID);
	}
	else
	{
		ghostRecorder.saveGhost("1");
		%ghostID = $settings::Ghost::highestID;
		%spawnpointTimes = ltrim(getField(spawnpointTimes, environmentLevel));
		%time = totalTime;
	}
	return "userID" SPC $settings::Profile::currentProfile TAB "time" SPC %time TAB "deaths" SPC deathCount TAB "spawnpointTimes" SPC %spawnpointTimes TAB "ghostID" SPC %ghostID;
	return "userID" SPC $settings::Profile::currentProfile TAB "time" SPC %time TAB "deaths" SPC deathCount TAB "spawnpointTimes" SPC %spawnpointTimes TAB "ghostID" SPC %ghostID;
}
function Statistics::getSavedTimes(%this, %levelName, %bestOfTen)
{
	%savedData = "";
	if (%levelName $= "")
	{
		%levelName = strchr(currentLevelObject.getName(), "_");
		if (strlen(%levelName) > 0.0)
		{
			%levelName = getSubStr(%levelName, "1", strlen(%levelName) - 1.0);
		}
	}
	if (levelBestTimes[%levelName] $= "")
	{
		debugEcho("reading saved data:");
		%i = 0;
		%statsFile = %this.getStatsFile("0", %levelName);
		while (%statsFile && !(%statsFile.isEOF()))
		{
			%nextLine = %statsFile.readLine();
			if (%nextLine $= "" || %nextLine $= "
" || %nextLine $= "" || %nextLine $= "
" || getSubStr(%nextLine, "0", "7") $= "Version")
			{
			}
			else
			{
				if (%savedData $= "")
				{
					%savedData = %nextLine;
				}
				else
				{
					%savedData = %savedData NL %nextLine;
				}
				%i = %i + 1.0;
			}
		}
		%statsFile.close();
		%versionLine = getRecord(%savedData, "0");
		if (firstWord(%versionLine) $= "Version")
		{
		}
		else
		{
		}
		%version = -1.0;
		if (%version >= 0.3100000023841858)
		{
			%savedData = removeRecord(%savedData, "0");
		}
		if (!($WII))
		{
			if (%version < 1.0299999713897705)
			{
				debugEcho("version in statsfile is smaller then 1.03" SPC %version);
				%i = 0;
				while (%i < getWordCount($settings::Profile::allProfiles))
				{
					%profileBestTime[getWord($settings::Profile::allProfiles, %i)] = "";
					%i = %i + 1.0;
				}
				%i = 0;
				while (%i < getRecordCount(%savedData))
				{
					%currentRecord = getRecord(%savedData, %i);
					%currentId = %this.getUserID(%currentRecord);
					%currentTime = %this.getTime(%currentRecord, "0");
					if (%profileBestTime[%currentId] $= "" || %currentTime < %profileBestTime[%currentId])
					{
						%profileBestTime[%currentId] = %currentTime;
					}
					%i = %i + 1.0;
				}
				%originalProfileId = $settings::Profile::currentProfile;
				saveSettings(%originalProfileId);
				if (%levelName $= "")
				{
					%levelName = strchr(currentLevelObject.getName(), "_");
					if (strlen(%levelName) > 0.0)
					{
						%levelName = getSubStr(%levelName, "1", strlen(%levelName) - 1.0);
					}
				}
				%i = 0;
				while (%i < getWordCount($settings::Profile::allProfiles))
				{
					%profileId = getWord($settings::Profile::allProfiles, %i);
					if (%profileBestTime[%profileId] $= "")
					{
					}
					else
					{
						loadSettings(%profileId);
						%this.setUserBestScore(%levelName, %profileBestTime[%profileId]);
						saveSettings(%profileId);
					}
					%i = %i + 1.0;
				}
				loadSettings(%originalProfileId);
			}
		}
		Statistics.levelBestTimes[%levelName] = %savedData;
	}
	else
	{
		%savedData = levelBestTimes[%levelName];
	}
	if (%bestOfTen && getRecordCount(%savedData) > 10.0)
	{
		%i = 0;
		while (%i < 10.0)
		{
			%newSaveData = %newSaveData NL getRecord(%savedData, %i);
			%i = %i + 1.0;
		}
		%saveData = ltrim(%newSaveData);
	}
	return %savedData;
	return %savedData;
}
function Statistics::getTime(%this, %record, %readable)
{
	%i = 0;
	while (%i < getFieldCount(%record))
	{
		if (firstWord(getField(%record, %i)) $= "time")
		{
			if (%readable)
			{
			}
			else
			{
			}
			return restWords(getField(%record, %i));
		}
		%i = %i + 1.0;
	}
	return -1.0;
	return -1.0;
}
function Statistics::getUserID(%this, %record)
{
	%i = 0;
	while (%i < getFieldCount(%record))
	{
		if (firstWord(getField(%record, %i)) $= "userId")
		{
			return restWords(getField(%record, %i));
		}
		%i = %i + 1.0;
	}
	return -1.0;
	return -1.0;
}
function Statistics::getUser(%this, %record)
{
	%i = 0;
	while (%i < getFieldCount(%record))
	{
		if (firstWord(getField(%record, %i)) $= "userId")
		{
			return getUserName(restWords(getField(%record, %i)));
		}
		%i = %i + 1.0;
	}
	%i = 0;
	while (%i < getFieldCount(%record))
	{
		if (firstWord(getField(%record, %i)) $= "userName")
		{
			return restWords(getField(%record, %i));
		}
		%i = %i + 1.0;
	}
	return -1.0;
	return -1.0;
}
function Statistics::getDeaths(%this, %record)
{
	%i = 0;
	while (%i < getFieldCount(%record))
	{
		if (firstWord(getField(%record, %i)) $= "deaths")
		{
			return restWords(getField(%record, %i));
		}
		%i = %i + 1.0;
	}
	return -1.0;
	return -1.0;
}
function Statistics::getDate(%this, %record)
{
	%i = 0;
	while (%i < getFieldCount(%record))
	{
		if (firstWord(getField(%record, %i)) $= "date")
		{
			return restWords(getField(%record, %i));
		}
		%i = %i + 1.0;
	}
	return -1.0;
	return -1.0;
}
function Statistics::getRank(%this, %record)
{
	%i = 0;
	while (%i < getFieldCount(%record))
	{
		if (firstWord(getField(%record, %i)) $= "rank")
		{
			return restWords(getField(%record, %i));
		}
		%i = %i + 1.0;
	}
	return -1.0;
	return -1.0;
}
function Statistics::getLocation(%this, %record)
{
	%i = 0;
	while (%i < getFieldCount(%record))
	{
		if (firstWord(getField(%record, %i)) $= "userid")
		{
			return getProfileData(restWords(getField(%record, %i)), "Personal", "Location");
		}
		%i = %i + 1.0;
	}
	return -1.0;
	return -1.0;
}
function Statistics::getGhost(%this, %record)
{
	%i = 0;
	while (%i < getFieldCount(%record))
	{
		if (firstWord(getField(%record, %i)) $= "ghostID")
		{
			return restWords(getField(%record, %i));
		}
		%i = %i + 1.0;
	}
	return -1.0;
	return -1.0;
}
function Statistics::getSpawnpointTimes(%this, %record)
{
	%spawnpointTimes = "";
	%i = 0;
	while (%i < getFieldCount(%record))
	{
		if (firstWord(getField(%record, %i)) $= "spawnpointTimes")
		{
			%spawnpointTimes = restWords(getField(%record, %i));
		}
		%i = %i + 1.0;
	}
	if (%spawnpointTimes $= "")
	{
		return -1.0;
	}
	%spawnpointTimes = strreplace(%spawnpointTimes, "#", $tab);
	return %spawnpointTimes;
	return %spawnpointTimes;
}
function Statistics::getLevelTimes(%this, %record)
{
	return %this.getLevelTimesOfSpanwpointTimes(%this.getSpawnpointTimes(%record));
	return %this.getLevelTimesOfSpanwpointTimes(%this.getSpawnpointTimes(%record));
}
function Statistics::getLevelTimesOfSpanwpointTimes(%this, %spawnpointTimes)
{
	%leveltimes = "";
	%i = 0;
	while (%i < getFieldCount(%spawnpointTimes))
	{
		%leveltimes = %leveltimes SPC getWord(getField(%spawnpointTimes, %i), getWordCount(getField(%spawnpointTimes, %i)) - 1.0);
		%i = %i + 1.0;
	}
	return trim(%leveltimes);
	return trim(%leveltimes);
}
function Statistics::getStatsFilePath(%this, %levelName, %onlyLevelPath)
{
	if (%levelName $= "")
	{
		%levelName = strchr(currentLevelObject.getName(), "_");
		if (strlen(%levelName) > 0.0)
		{
			%levelName = getSubStr(%levelName, "1", strlen(%levelName) - 1.0);
		}
	}
	if ($WII)
	{
		%basePath = "";
	}
	else
	{
		%basePath = "stats/";
	}
	if (%onlyLevelPath)
	{
		return ['%levelName', '".aym"'];
	}
	else
	{
		return ['%basePath', '%levelName', '".aym"'];
	}
	return ['%basePath', '%levelName', '".aym"'];
}
function Statistics::getStatsFile(%this, %writeMode, %levelName)
{
	%statsFilePath = %this.getStatsFilePath(%levelName);
	if (%writeMode)
	{
		debugWarn("WARNING: statistics getStatsFile is used for opening a file in write mode. To write only fileIOs writeFile(lines) should be used! FIXME!");
		%statsFile = new FileObject(Name : "");
		%statsFile.openForWrite(%statsFilePath);
	}
	else
	{
		%statsFile = validateFile(%statsFilePath, "1");
	}
	return %statsFile;
	return %statsFile;
}
function Statistics::setSpawnPointTime(%this, %number, %time)
{
	%spawnpointTimes = getField(spawnpointTimes, environmentLevel);
	%spawnpointTimes = setWord(%spawnpointTimes, %number, %time);
	%this.spawnpointTimes = setField(spawnpointTimes, environmentLevel, %spawnpointTimes);
	return;
}
function Statistics::getLocalHighscores(%this, %levelName)
{
	%bestTimes = %this.getSavedTimes(%levelName, "1");
	if (%bestTimes == -1.0)
	{
		return -1.0;
	}
	%localHighscores = "";
	%i = 0;
	while (%i < getRecordCount(%bestTimes))
	{
		%record = getRecord(%bestTimes, %i);
		if ($WII)
		{
			%localHighscores = %localHighscores NL %i + 1.0 TAB %this.getUser(%record) TAB %this.getTime(%record, "1") TAB %this.getDeaths(%record);
		}
		else
		{
			%localHighscores = %localHighscores NL %i + 1.0 TAB %this.getUser(%record) TAB %this.getLocation(%record) TAB %this.getTime(%record, "1") TAB %this.getDeaths(%record) TAB %this.getGhost(%record);
		}
		%i = %i + 1.0;
	}
	%localHighscores = removeRecord(%localHighscores, "0");
	return %localHighscores;
	return %localHighscores;
}
function Statistics::deleteGhosts(%this, %ghostsToDelete, %path)
{
	if (%path $= "")
	{
		%path = getLastToken(currentLevelObject.getName(), "_");
	}
	%i = 0;
	while (%i < getWordCount(%ghostsToDelete))
	{
		%ghost = getWord(%ghostsToDelete, %i);
		fileDelete(getGhostFilePath(%path, %ghost));
		%i = %i + 1.0;
	}
	return getWordCount(%ghostsToDelete);
}
function Statistics::onSpawnpointActivate(%this)
{
	%spawnpointBehavior = spawnPoint.getBehavior("BeSpawnPoint");
	%sPNumber = number;
	if (%sPNumber == 1.0)
	{
		return;
	}
	Statistics.setSpawnPointTime(%sPNumber - 1.0, timeEntered);
	return;
}
