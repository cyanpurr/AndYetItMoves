// levelLoading.cs.dso
function initLevelScriptObjects()
{
	echo("initialising LevelScriptObjects");
	new ScriptObject(Name : level_gameMenu)
	{
		class = "GameLevel";
		fileName = "game/data/levels/gameMenu.t2d";
		environment = -1.0;
		objectGroups = "";
		environmentResource = "";
		levelResource = "menuArt";
		isMenu = "1";
	}
	new ScriptObject(Name : level_cave)
	{
		levelName = "Chapter One";
		levelID = "c1a";
		fileName = "game/data/levels/cave1.t2d";
	}
	new ScriptObject(Name : level_cave1)
	{
		class = "GameLevel";
		environment = "0";
		levelName = "Cave 1";
		levelID = "c11";
		fileName = "game/data/levels/cave1.t2d";
		objectGroups = "";
		environmentResource = "caveArt";
		levelResource = "cave1Art";
		rallyTimes = "10 17 25 35";
		rotationLimit = "5";
		totalTime = "0";
		deathCount = "0";
	}
	new ScriptObject(Name : level_cave2)
	{
		class = "GameLevel";
		environment = "0";
		levelName = "Cave 2";
		levelID = "c12";
		fileName = "game/data/levels/cave2.t2d";
		objectGroups = "";
		environmentResource = "caveArt";
		levelResource = "cave2Art";
		rallyTimes = "6 15 21 32 41 48 59";
		rotationLimit = "13";
		totalTime = "0";
		deathCount = "0";
	}
	if ($WII && $demoVersion)
	{
		level_cave2.fileName = "game/data/levels/cave2demo.t2d";
		level_cave2.levelResource = "cave2Art hintArt";
	}
	new ScriptObject(Name : level_cave3)
	{
		class = "GameLevel";
		environment = "0";
		levelName = "Cave 3";
		levelID = "c13";
		fileName = "game/data/levels/cave3.t2d";
		objectGroups = "";
		environmentResource = "caveArt";
		levelResource = "cave3Art";
		rallyTimes = "9 17 23 34 41 49 56 67 74";
		rotationLimit = "13";
		totalTime = "0";
		deathCount = "0";
	}
	new ScriptObject(Name : level_cave4)
	{
		class = "GameLevel";
		environment = "0";
		levelName = "Cave 4";
		levelID = "c14";
		fileName = "game/data/levels/cave4.t2d";
		objectGroups = "stalagtiteGroup rootsGroup";
		playerTriggerGroups = "saurianTrigger";
		playerWalkOnGroups = "growingRoot";
		environmentResource = "caveArt";
		levelResource = "cave4Art";
		rallyTimes = "20 40 50 55 63 70 90";
		rotationLimit = "24";
		totalTime = "0";
		deathCount = "0";
		endOfEnvironment = "1";
	}
	new ScriptObject(Name : level_jungle)
	{
		levelName = "Chapter Two";
		levelID = "c2a";
		fileName = "game/data/levels/jungle1.t2d";
	}
	new ScriptObject(Name : level_jungle1)
	{
		class = "GameLevel";
		environment = "1";
		levelName = "Jungle 1";
		levelID = "c21";
		fileName = "game/data/levels/jungle1.t2d";
		objectGroups = "";
		playerWalkOnGroups = "jungleMonkey";
		fallOutGroups = "";
		environmentResource = "jungleArt";
		levelResource = "jungle1Art";
		rallyTimes = "15 38 50 66 72 83 92 113 129 135";
		rotationLimit = "18";
		totalTime = "0";
		deathCount = "0";
	}
	new ScriptObject(Name : level_jungle2)
	{
		class = "GameLevel";
		environment = "1";
		levelName = "Jungle 2";
		levelID = "c22";
		fileName = "game/data/levels/jungle2.t2d";
		objectGroups = "";
		playerWalkOnGroups = "jungleSwingPlatform";
		playerTriggerGroups = "jungleDionaeaSnapTrigger";
		fallOutGroups = "";
		environmentResource = "jungleArt";
		levelResource = "jungle2Art";
		rallyTimes = "23 35 44 63 76 91 104 113 131";
		rotationLimit = "17";
		totalTime = "0";
		deathCount = "0";
	}
	if ($WII && $demoVersion)
	{
		level_jungle2.fileName = "game/data/levels/jungle2demo.t2d";
	}
	new ScriptObject(Name : level_jungle3)
	{
		class = "GameLevel";
		environment = "1";
		levelName = "Jungle 3";
		levelID = "c23";
		fileName = "game/data/levels/jungle3.t2d";
		objectGroups = "";
		playerWalkOnGroups = "jungleSwingPlatform jungleMonkey";
		fallOutGroups = "";
		environmentResource = "jungleArt";
		levelResource = "jungle3Art";
		rallyTimes = "14 28 41 51 68 79 91 106 118 132 156 169";
		rotationLimit = "17";
		totalTime = "0";
		deathCount = "0";
	}
	new ScriptObject(Name : level_jungle4)
	{
		class = "GameLevel";
		environment = "1";
		levelName = "Jungle 4";
		levelID = "c24";
		fileName = "game/data/levels/jungle4.t2d";
		objectGroups = "";
		playerWalkOnGroups = "jungleSwingPlatform";
		fallOutGroups = "";
		environmentResource = "jungleArt";
		levelResource = "jungle4Art";
		rallyTimes = "17 25 40 52 68 83 91 101 111 125 130 138 153 166 172";
		rotationLimit = "20";
		totalTime = "0";
		deathCount = "0";
	}
	new ScriptObject(Name : level_jungle5)
	{
		class = "GameLevel";
		environment = "1";
		levelName = "Jungle 5";
		levelID = "c25";
		fileName = "game/data/levels/jungle5.t2d";
		objectGroups = "";
		playerWalkOnGroups = "jungleSwingPlatform";
		fallOutGroups = "";
		environmentResource = "jungleArt";
		levelResource = "jungle5Art";
		rallyTimes = "10 17 33 46 55 69 78 88 107 118 129 143 160";
		rotationLimit = "22";
		totalTime = "0";
		deathCount = "0";
	}
	new ScriptObject(Name : level_jungle6)
	{
		class = "GameLevel";
		environment = "1";
		levelName = "Jungle 6";
		levelID = "c26";
		fileName = "game/data/levels/jungle6.t2d";
		objectGroups = "flameSmoothers";
		playerWalkOnGroups = "flintstone jungleSwingPlatform";
		playerTriggerGroups = "flame flameable";
		viewWindowGroups = "";
		fallOutGroups = "flintstone";
		environmentResource = "jungleArt";
		levelResource = "jungle6Art";
		rallyTimes = "5 18 25 46 63 84 91 107 112 123 135 153 164";
		rotationLimit = "33";
		totalTime = "0";
		deathCount = "0";
		endOfEnvironment = "1";
	}
	new ScriptObject(Name : level_trip)
	{
		levelName = "Chapter Three";
		levelID = "c3a";
		fileName = "game/data/levels/trip1.t2d";
	}
	new ScriptObject(Name : level_trip1)
	{
		class = "GameLevel";
		environment = "2";
		levelName = "Trip 1";
		levelID = "c31";
		fileName = "game/data/levels/trip1.t2d";
		objectGroups = "";
		environmentResource = "jungleArt";
		levelResource = "trip1Art";
		rallyTimes = "9 25 40 60 65 86 94 108 122 166";
		rotationLimit = "24";
		totalTime = "0";
		deathCount = "0";
	}
	new ScriptObject(Name : level_trip2)
	{
		class = "GameLevel";
		environment = "2";
		levelName = "Trip 2";
		levelID = "c32";
		fileName = "game/data/levels/trip2.t2d";
		objectGroups = "";
		playerWalkOnGroups = "0";
		environmentResource = "tripArt";
		levelResource = "trip2Art";
		rallyTimes = "12 20 31 44 59 71 90 107 125 135 147 157 167 180 197 210 237";
		rotationLimit = "39";
		totalTime = "0";
		deathCount = "0";
	}
	if ($WII && $demoVersion)
	{
		level_trip2.fileName = "game/data/levels/trip2demo.t2d";
		level_trip2.rallyTimes = "12 20 31 44 46";
		level_trip2.levelResource = "trip2Art demoArt";
	}
	new ScriptObject(Name : level_trip3)
	{
		class = "GameLevel";
		environment = "2";
		levelName = "Trip 3";
		levelID = "c33";
		fileName = "game/data/levels/trip3.t2d";
		objectGroups = "";
		playerWalkOnGroups = "0";
		environmentResource = "tripArt";
		levelResource = "trip3Art";
		rallyTimes = "8 13 21 33 50 62 62 62 83 103 128 152 167 203";
		rotationLimit = "28";
		totalTime = "0";
		deathCount = "0";
	}
	new ScriptObject(Name : level_trip4)
	{
		class = "GameLevel";
		environment = "2";
		levelName = "Trip 4";
		levelID = "c34";
		fileName = "game/data/levels/trip4.t2d";
		objectGroups = "";
		playerWalkOnGroups = "0";
		environmentResource = "tripArt";
		levelResource = "trip4Art";
		rallyTimes = "9 15 21 33 40 46 76 108 120 128 137 142 148";
		rotationLimit = "16";
		totalTime = "0";
		deathCount = "0";
	}
	new ScriptObject(Name : level_trip5)
	{
		class = "GameLevel";
		environment = "2";
		levelName = "Trip 5";
		levelID = "c35";
		fileName = "game/data/levels/trip5.t2d";
		objectGroups = "";
		playerWalkOnGroups = "0";
		environmentResource = "tripArt";
		levelResource = "trip5Art";
		rallyTimes = "11 25 37 42 52 61 73 85 100 119 141 147 167";
		rotationLimit = "27";
		totalTime = "0";
		deathCount = "0";
	}
	new ScriptObject(Name : level_trip6)
	{
		class = "GameLevel";
		environment = "2";
		levelName = "Trip 6";
		levelID = "c36";
		fileName = "game/data/levels/trip6.t2d";
		objectGroups = "";
		playerWalkOnGroups = "0";
		environmentResource = "tripArt";
		levelResource = "trip6Art";
		rallyTimes = "8 20 34 49 69 102 109 129 142 176 182 237 268 322 330 398 415";
		rotationLimit = "48";
		totalTime = "0";
		deathCount = "0";
		endOfEnvironment = "1";
	}
	new ScriptObject(Name : level_credits)
	{
		class = "GameLevel";
		environment = "3";
		levelName = "Credits";
		fileName = "game/data/levels/credits.t2d";
		objectGroups = "";
		playerWalkOnGroups = "";
		environmentResource = "bonusLevelsArt";
		levelResource = "creditsArt";
		playerImmortal = "1";
		dontMeasureTime = "1";
	}
	new ScriptObject(Name : level_demoEnd)
	{
		class = "GameLevel";
		environment = "3";
		levelName = "Credits";
		fileName = "game/data/levels/demoEnd.t2d";
		objectGroups = "";
		playerWalkOnGroups = "";
		environmentResource = "bonusLevelsArt";
		levelResource = "demoArt";
		playerImmortal = "1";
		dontMeasureTime = "1";
	}
	if ($WII && $demoVersion)
	{
		level_demoEnd.fileName = "game/data/levels/demoEndWii.t2d";
	}
	new ScriptObject(Name : level_video)
	{
		class = "GameLevel";
		environment = "3";
		levelName = "Video";
		fileName = "game/data/levels/video.t2d";
		objectGroups = "";
		playerWalkOnGroups = "";
		environmentResource = "";
		levelResource = "";
		playerImmortal = "1";
		dontMeasureTime = "1";
		isVideo = "1";
	}
	new ScriptObject(Name : level_finalLevel)
	{
		class = "GameLevel";
		environment = "3";
		levelName = "Final Level";
		levelID = "s1";
		fileName = "game/data/levels/finalLevel.t2d";
		objectGroups = "";
		playerWalkOnGroups = "jungleSwingPlatform";
		environmentResource = "bonusLevelsArt";
		levelResource = "finalLevelArt";
		rallyTimes = "9 21 26 51";
		rotationLimit = "6";
		totalTime = "0";
		deathCount = "0";
	}
	new ScriptObject(Name : level_elevator)
	{
		class = "GameLevel";
		environment = "3";
		levelName = "Elevator";
		levelID = "s2";
		fileName = "game/data/levels/elevator.t2d";
		objectGroups = "";
		playerWalkOnGroups = "elevator peggleNails";
		environmentResource = "bonusLevelsArt";
		levelResource = "elevatorArt";
		rallyTimes = "6 20 41 55 67 84 98 118 131";
		rotationLimit = "25";
		totalTime = "0";
		deathCount = "0";
	}
	new ScriptObject(Name : level_labyrinth)
	{
		class = "GameLevel";
		environment = "3";
		levelName = "Labyrinth";
		levelID = "s3";
		fileName = "game/data/levels/labyrinth.t2d";
		objectGroups = "";
		playerWalkOnGroups = "";
		environmentResource = "bonusLevelsArt";
		levelResource = "";
		rallyTimes = "7 30 39 49 71 80 100 117 176";
		rotationLimit = "30";
		totalTime = "0";
		deathCount = "0";
	}
	new ScriptObject(Name : level_chase)
	{
		class = "GameLevel";
		environment = "3";
		levelName = "The Chase";
		levelID = "s4";
		fileName = "game/data/levels/chase.t2d";
		objectGroups = "";
		playerWalkOnGroups = "";
		environmentResource = "bonusLevelsArt";
		levelResource = "chaseArt";
		rallyTimes = "12 20 30 40 48 69 83 101 112 140 180 279";
		rotationLimit = "40";
		totalTime = "0";
		deathCount = "0";
	}
	new ScriptObject(Name : level_presentation)
	{
		class = "GameLevel";
		fileName = "game/data/levels/presentation.t2d";
		objectGroups = "";
		environmentResource = "presentationArt";
		levelResource = "";
		playerImmortal = "1";
		dontMeasureTime = "1";
	}
	$caveLevelNames = levelName TAB levelName TAB levelName TAB levelName;
	$jungleLevelNames = levelName TAB levelName TAB levelName TAB levelName TAB levelName TAB levelName;
	$tripLevelNames = levelName TAB levelName TAB levelName TAB levelName TAB levelName TAB levelName;
	return;
}
function getLevelScriptObject(%levelNr)
{
	if (%levelNr > 0.0)
	{
		return getWord($LEVELLIST, %levelNr - 1.0);
	}
	else
	{
		%i = 0;
		while (%i < getWordCount($LEVELLIST))
		{
			%obj = getWord($LEVELLIST, %i);
			if (fileName $= %levelNr || %obj.getName() $= %levelNr)
			{
				globals.currentLevelNumber = %i + 1.0;
				return getWord($LEVELLIST, %i);
			}
			%i = %i + 1.0;
		}
		%i = 0;
		while (%i < getWordCount($COMPLETE_LEVELLIST))
		{
			%obj = getWord($COMPLETE_LEVELLIST, %i);
			if (fileName $= %levelNr || %obj.getName() $= %levelNr)
			{
				globals.currentLevelNumber = %i + 1.0;
				return getWord($COMPLETE_LEVELLIST, %i);
			}
			%i = %i + 1.0;
		}
	}
	return "0";
	return "0";
}
function GameLevel::unloadLevel(%this)
{
	if (isObject(currentLevelObject))
	{
		GameLevel::endLevel();
		%path = $resourceFolderName @ "/";
		%this.updateResources(%path, environmentResource, "0", environmentResource);
		%this.updateResources(%path, levelResource, "0", levelResource);
	}
	return;
}
function GameLevel::loadLevel(%this)
{
	%path = $resourceFolderName @ "/";
	%this.updateResources(%path, environmentResource, "1");
	%this.updateResources(%path, levelResource, "1");
	if ($WII)
	{
		loadCommonAudioProfiles();
	}
	globals.currentLevelObject = %this;
	globals.spawnPoints = "0";
	%this.createObjectGroups();
	%scenegraph = sceneWindow2d.loadLevel(fileName);
	%this.echoLevelBanner();
	globals.scenegraph = %scenegraph;
	return %scenegraph;
	return %scenegraph;
}
function GameLevel::updateResources(%this, %path, %ressourceList, %toLoad, %skipRessources)
{
	if (%toLoad)
	{
		%i = 0;
		while (%i < getWordCount(%ressourceList))
		{
			%currentResource = getWord(%ressourceList, %i);
			if (findWord(%skipRessources, %currentResource))
			{
				debugEcho("skip loading ressource" SPC %path SPC %currentResource);
			}
			else
			{
				if (!(isObject(ResourceFinder::getResource(%currentResource))))
				{
					debugEcho("loading resource" SPC %path SPC %currentResource);
					ResourceObject::load(%path @ %currentResource);
					break;
				}
				debugEcho("resource" SPC %path SPC %currentResource SPC "already in use, no need to load");
			}
			%i = %i + 1.0;
		}
	}
	else
	{
		%i = 0;
		while (%i < getWordCount(%ressourceList))
		{
			%currentResource = getWord(%ressourceList, %i);
			if (findWord(%skipRessources, %currentResource))
			{
				debugEcho("skip unloading ressource" SPC %path SPC %currentResource);
			}
			else
			{
				if (isObject(ResourceFinder::getResource(%currentResource)))
				{
					debugEcho("unloading resource" SPC %currentResource);
					ResourceObject::unload(%currentResource);
					break;
				}
				debugEcho("resource" SPC %path SPC %currentResource SPC "wasn't loaded, no need to unload");
			}
			%i = %i + 1.0;
		}
	}
	return getWordCount(%ressourceList);
}
function GameLevel::createObjectGroups(%this)
{
	new SimSet(Name : safeScheduleList);
	levelGarbageCollector.add(safeScheduleList);
	%i = 0;
	while (%i < getWordCount(objectGroups))
	{
		new SimSet(Name : getWord(objectGroups, %i));
		%i = %i + 1.0;
	}
	return getWordCount(objectGroups);
}
function GameLevel::deleteObjectGroups(%this)
{
	%i = 0;
	while (%i < safeScheduleList.getCount())
	{
		safeScheduleList.getObject(%i).cancelSchedule();
		%i = %i + 1.0;
	}
	safeScheduleList.delete();
	%i = 0;
	while (%i < getWordCount(objectGroups))
	{
		getWord(objectGroups, %i).delete();
		%i = %i + 1.0;
	}
	return getWordCount(objectGroups);
}
function GameLevel::endLevel(%this)
{
	if (isObject(currentLevelObject))
	{
		currentLevelObject.deleteObjectGroups();
	}
	levelShutdown();
	while (levelGarbageCollector.getCount() > 0.0)
	{
		levelGarbageCollector.getObject("0").delete();
	}
	return levelGarbageCollector.getCount();
}
function levelShutdown()
{
	$levelLoadedFinished = 0;
	setControlsEnabled("0");
	triggerEvent("onLevelShutdown");
	emptySubscriber();
	sceneWindow2d.endLevel();
	alxStopAll();
	return;
}
function GameLevel::echoLevelBanner(%this)
{
	%banner = "* LEVEL LOADED:" SPC %this.getName() SPC "*";
	%border = "";
	%i = 0;
	while (%i < strlen(%banner))
	{
		%border = %border @ "*";
		%i = %i + 1.0;
	}
	echo(%border NL %banner NL %border);
	return;
}
