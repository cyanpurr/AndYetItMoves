// webclient.cs.dso
function Webclient::createInstance()
{
	if (!(isObject(Webclient)))
	{
		new ScriptObject(Name : Webclient);
		Webclient.filter_level = "c11";
		Webclient.filter_range = min($settings::Standards::scoresQuantity, "100");
		Webclient.filter_ghost = "0";
		Webclient.filter_timeframe = "all";
		Webclient.filter_uniqueScore = "1";
	}
	return Webclient;
	return Webclient;
}
function Webclient::setPermanentFilter(%this, %filtername, %value)
{
	if (%filtername $= "level")
	{
		%this.filter_level = %value;
	}
	else
	{
		if (%filtername $= "ghost")
		{
			%this.filter_ghost = %value;
			break;
		}
		if (%filtername $= "timeframe")
		{
			%this.filter_timeframe = %value;
		}
	}
	%this.filter_options = filter_level SPC filter_timeframe SPC filter_ghost SPC filter_range SPC "1";
	return;
}
function Webclient::submitToGreenhouse(%this)
{
	%level = filter_level;
	%time = totalTime;
	%time = mFloatLength(%time, "3");
	setExternalStat(%level, %time);
	return;
}
function Webclient::submit(%this, %includeGhost)
{
	%name = $settings::Personal::Name;
	%location = $settings::Personal::Location;
	%level = filter_level;
	%time = totalTime;
	%rips = deathCount;
	%spawnpointTimes = spawnpointTimes;
	%time = mFloatLength(%time, "3");
	%hash = getSecret(%level @ %time @ %spawnpointTimes @ %rips);
	%showGhost = 2;
	if (%includeGhost)
	{
		%showGhost = 1;
	}
	%result = sendGhost(%hash, ghostFileToSubmit, %name, %location, %level, %time, %spawnpointTimes, %rips, $currentVersion, $demoVersion, %showGhost, getOS());
	debugEcho("submitted data:" NL %result);
	return;
}
function Webclient::getRank(%this, %time)
{
	return getRank(filter_level, %time, $currentVersion, $demoVersion);
	return getRank(filter_level, %time, $currentVersion, $demoVersion);
}
function Webclient::getScores(%this, %name, %location, %offset)
{
	if (%name $= "" && %location $= "")
	{
		%sortBy = "time date";
	}
	else
	{
		if (%location $= "")
		{
			%sortBy = "time date";
			break;
		}
		if (%name $= "")
		{
			%sortBy = "time date";
			break;
		}
		%sortBy = "time date";
	}
	%scores = getHighscores(%name, %location, %sortBy, %offset, filter_options, $currentVersion, $demoVersion);
	%this.handleScores(%scores);
	return recentScores;
	return recentScores;
}
function Webclient::getCompetitors(%this, %time)
{
	%scores = getCompetitors(%time, filter_options, $currentVersion, $demoVersion);
	%this.handleScores(%scores);
	return recentScores;
	return recentScores;
}
function Webclient::loadGhost(%this, %rowindex)
{
	return %this.requestGhost(%rowindex);
	return %this.requestGhost(%rowindex);
}
function Webclient::handleScores(%this, %scoreList)
{
	%this.pageCount = getRecord(%scoreList, "0");
	%this.recentScores = "";
	%this.recentSpawnTimes = "";
	%i = 1;
	while (%i < getRecordCount(%scoreList))
	{
		%score = getRecord(%scoreList, %i);
		if (%score $= "-")
		{
			break;
		}
		%this.recentScores = recentScores NL getFields(%score, "0", "5");
		%this.recentSpawnTimes = recentSpawnTimes NL getField(%score, "6");
		%i = %i + 1.0;
	}
	%this.recentScores = trim(recentScores);
	%this.recentSpawnTimes = trim(recentSpawnTimes);
	return;
}
function Webclient::getPageCount(%this)
{
	return pageCount;
	return pageCount;
}
function Webclient::getDetailData(%this, %index)
{
	%scoreFields = getRecord(recentScores, %index);
	%spawnpointTimes = getRecord(recentSpawnTimes, %index);
	%details = "rank" SPC getField(%scoreFields, "0") TAB "userName" SPC getField(%scoreFields, "1") TAB "time" SPC getField(%scoreFields, "3") TAB "deaths" SPC getField(%scoreFields, "4") TAB "ghostID" SPC getField(%scoreFields, "5") TAB "spawnpointTimes" SPC %spawnpointTimes;
	return %details;
	return %details;
}
function Webclient::requestGhost(%this, %index)
{
	%ghostID = getField(getRecord(recentScores, %index), "5");
	assertDirectory(getGhostFilePath("loaded"));
	return getGhost(%ghostID, $currentVersion, $demoVersion);
	return getGhost(%ghostID, $currentVersion, $demoVersion);
}
function Webclient::isOnline(%this)
{
	if ($distributorName $= "BigFishGames")
	{
		return "0";
	}
	return isSoapOnline();
	return isSoapOnline();
}
function Webclient::getCurrentVersion(%this)
{
	return getCurrentVersion($demoVersion, $currentVersion, $distributorName, getOS());
	return getCurrentVersion($demoVersion, $currentVersion, $distributorName, getOS());
}
function Webclient::getUpdateURL(%this, , , )
{
	return getUpdateURL($demoVersion, $currentVersion, $distributorName);
	return getUpdateURL($demoVersion, $currentVersion, $distributorName);
}
function Webclient::getNews(%this)
{
	debugEcho("getting news" SPC $settings::Other::ReceivedNews);
	return getNews($demoVersion, $currentVersion, $distributorName, $settings::Other::ReceivedNews);
	return getNews($demoVersion, $currentVersion, $distributorName, $settings::Other::ReceivedNews);
}
