// game.cs.dso
function startGame(%level)
{
	$curLevelStartTime = getRealTime();
	echo("startGame at" SPC $curLevelStartTime SPC "seconds");
	if (!($enableDebugMap) && getPublisherName() $= "Greenhouse" && !($demoVersion) && !($thisSessionModifiedOK) && getWordIndex($LEVELLIST, %level) > 4.0)
	{
		if (isExternalModified())
		{
			echo("!Greenhouse modified!");
			showAuthQuitDialog($lbl_GHmodified, "1");
			return "0";
			break;
		}
		$thisSessionModifiedOK = 1;
	}
	if ($WII)
	{
		wiiHomeMenuAllowed("1");
	}
	setupDemoVersion();
	Canvas.setContent(mainScreenGui);
	$firstTickOccured = 0;
	$firstKeyPressed = 0;
	$pref::T2D::warnSceneOccupancy = 0;
	clearGroups();
	initStatObjects();
	if (%level > 0.0)
	{
		globals.currentLevelNumber = %level;
	}
	%t = getRealTime();
	getLevelScriptObject(%level).unloadLevel();
	debugEcho("*** unloadLevel() took" SPC getRealTime() - %t);
	resetAllAudioChannelVolumes();
	fadeAllMusicAudioChannels("1");
	setDontTick("1");
	$ACTUAL_MENU.enterMenu(menu_loading);
	resetLoadingString();
	setNextLoadingString();
	if (isEventPending($respawnScheduleId))
	{
		cancel($respawnScheduleId);
	}
	echo("startGame finished");
	schedule(duration32ms * 1000.0, "0", "echo", "0.032 second schedule test successful!");
	schedule("1000", "0", "echo", "1 second schedule test successful!");
	echo("started scheduletest" SPC "in" SPC duration32ms);
	if ($WII)
	{
		schedule(duration32ms * 1000.0, "0", "startGame2", %level);
	}
	else
	{
		startGame2(%level);
	}
	return;
}
function startGame2(%level)
{
	%t = getRealTime();
	echo("startGame2 started at" SPC %t SPC "seconds");
	if (!($WII))
	{
		setQuittable("0");
	}
	%scenegraph = getLevelScriptObject(%level).loadLevel();
	echo("loaded level:" SPC %level);
	debugEcho("*** loadLevel() took" SPC getRealTime() - %t);
	%scenegraph.setScenePause("0");
	triggerEvent("collectObjectsInBehavior");
	if (%scenegraph.getName() != "daScenegraph")
	{
		debugWarn("*****ATTENTIONATTENTIONATTENTIONATTENTIONATTENTION*****");
		debugWarn("call the scenegraph daScenegraph");
		debugWarn("*****ATTENTIONATTENTIONATTENTIONATTENTIONATTENTION*****");
	}
	daSceneGraph.setMainSceneWindow(sceneWindow2d);
	achievements.setup();
	subscribeToEvents(Statistics, "onLevelLoadFinished40 onLevelLoadFinished");
	subscribeToEvents(playmodeManager, "onLevelLoadFinished");
	SpeedRunMode.checkDisplay();
	if ($enableDebugMap)
	{
		$splashScreenShown = 1;
	}
	if (isMenu || isVideo)
	{
		initCamera("1");
		menu_main.previousMenu = "";
		if ($splashScreenShown)
		{
			initCheats();
			if ($settings::Profile::allProfiles $= "" || $futureProfileId != "" && $showCreateProfilePopUp)
			{
				if ($WII && !($saveOnWii))
				{
					loadDefaultUserSettings();
					$settings::Profile::allProfiles = "0";
					$settings::Profile::currentProfile = "0";
					break;
				}
				echo("showing create profile");
				chb_createProfile_showHints.setVisible("0");
				menu_createProfile.editProfile = "0";
				if ($SnowdriftlandSpecial || $WII)
				{
					menu_switchProfiles.display();
				}
				else
				{
					menu_createProfile.display();
				}
				return;
			}
			setDontTick("0");
			menu_loading.Hide();
			if (isMenu)
			{
				menu_main.display();
				triggerEvent("onMenuLoadFinished");
			}
		}
	}
	else
	{
		echo("initing all" SPC %this);
		initPlayer();
		initPlayerParts();
		$GRABBERINPUT = isGrabberActive();
		initCamera();
		if ($GRABBERINPUT)
		{
			initGrabberInput();
		}
		initCheats();
		initLayers();
		initSpawnPointTextures();
		initGlobalSmoother();
		if (!($WII))
		{
			Replay::init();
		}
	}
	currentLevelObject.playingId = $settings::Profile::currentProfile;
	echo("*** stuff before ollf took" SPC getRealTime() - $curLevelStartTime);
	$curOllfIndex = 0;
	callNextFrame("0", "nextOnLevelLoadFinished", "1", "1");
	echo("startGame2 finished");
	return;
}
function nextOnLevelLoadFinished()
{
	%t = getRealTime();
	echo("nextOnLevelLoadFinished started at" SPC %t SPC "seconds");
	%ollf = "onLevelLoadFinished" @ getWord($OLLF_NUMBERS, $curOllfIndex);
	triggerEvent(%ollf);
	debugEcho("***" SPC %ollf SPC "took" SPC getRealTime() - %t);
	$curOllfIndex = $curOllfIndex + 1.0;
	%punktis = getWaitingString($NUM_OLLFS + 3.0, "0");
	setNextLoadingString();
	if ($curOllfIndex < $NUM_OLLFS)
	{
		callNextFrame("0", "nextOnLevelLoadFinished", "1", "1");
	}
	else
	{
		callNextFrame("0", "finishStartGame", "1", "1");
	}
	echo("nextOnLevelLoadFinished finished");
	return;
}
function finishStartGame()
{
	%t = getRealTime();
	echo("finishStartGame started at" SPC %t SPC "seconds");
	setDontTick("0");
	triggerEvent("onLevelLoadFinished");
	menu_loading.Hide();
	if (!(getShaderSupport()))
	{
		$settings::Performance::Shaders = 0;
	}
	setVerticalSync($settings::Performance::VerticalSync);
	setUseShader($settings::Performance::Shaders);
	daSceneGraph.setDebugOff("0");
	daSceneGraph.setDebugOff("1");
	daSceneGraph.setDebugOff("2");
	daSceneGraph.setDebugOff("3");
	daSceneGraph.setDebugOff("4");
	daSceneGraph.setDebugOff("5");
	if ($enableDebugMap && 0)
	{
		schedule(15.0 * 1000.0, "0", "savePeriodFrameCount", "15");
		schedule(1.0 * 1000.0, "0", "savePeriodFrameCount", "1");
	}
	if (playerImmortal)
	{
		player.lethalSpeed = "550";
	}
	if (getPublisherName() $= "Greenhouse")
	{
		schedule(getRandom("5000", "25000"), "0", "checkAuthInGame");
	}
	$tickStats.tickCounter = "0";
	if (!($WII))
	{
		setQuittable("1");
	}
	$firstStartupDone = 1;
	echo("finishStartGame finished");
	return;
}
function daSceneGraph::onUpdateSceneTick(%this)
{
	$bcCounter = 0;
	$triggerCounter = 0;
	$tickStats.tickCounter = tickCounter + 1.0;
	$tickStats.thisTime = daSceneGraph.getSceneTime();
	setLastTickTime(thisTime);
	if (!($firstTickOccured))
	{
		return;
	}
	if (!(modulo(tickCounter, ticksUntilUpdate32ms)))
	{
		triggerEvent("onUpdateTick32ms10");
		triggerEvent("onUpdateTick32ms");
		$tickStats.lastTime32ms = thisTime;
	}
	if (!(modulo(tickCounter, ticksUntilUpdate)))
	{
		$tickStats.allTicks = allTicks + 1.0;
		triggerEvent("onUpdateTick1");
		triggerEvent("onUpdateTick10");
		triggerEvent("onUpdateTick15");
		triggerEvent("onUpdateTick20");
		triggerEvent("onUpdateTick30");
		if ($zoomIn)
		{
			sceneWindow2d.setCurrentCameraZoom(camera.getRealZoom() - duration * camera.getRealZoom());
		}
		if ($zoomOut)
		{
			sceneWindow2d.setCurrentCameraZoom(camera.getRealZoom() + duration * camera.getRealZoom());
		}
		$tickStats.lastTime = thisTime;
	}
	dbgRestart();
	return;
}
function getTickCount()
{
	return allTicks;
	return allTicks;
}
function camera::onUpdate(%this)
{
	camera.tickCounter = tickCounter + 1.0;
	if (tickCounter == 1.0)
	{
		Canvas.popDialog(menu_oneLineWarning);
		%t = getRealTime();
		triggerEvent("onUpdateFirstTick10");
		debugEcho("*** onUpdateFirstTick10 took" SPC getRealTime() - %t);
		%t = getRealTime();
		triggerEvent("onUpdateFirstTick20");
		debugEcho("*** onUpdateFirstTick20 took" SPC getRealTime() - %t);
		%t = getRealTime();
		triggerEvent("onUpdateFirstTick30");
		debugEcho("*** onUpdateFirstTick30 took" SPC getRealTime() - %t);
		%t = getRealTime();
		triggerEvent("onUpdateFirstTick40");
		debugEcho("*** onUpdateFirstTick40 took" SPC getRealTime() - %t);
	}
	if (tickCounter == 3.0)
	{
		camera.disableUpdateCallback();
		%t = getRealTime();
		triggerEvent("onUpdateSecondTick");
		debugEcho("*** onUpdateSecondTick took" SPC getRealTime() - %t);
		debugWarn("loading level took" SPC getRealTime() - $curLevelStartTime SPC "ms");
		$levelLoadedFinished = 1;
		$firstTickOccured = 1;
	}
	return;
}
function daSceneGraph::onUpdateScene(%this)
{
	$frameStats.thisTime = daSceneGraph.getSceneTime();
	setLastFrameTime(thisTime);
	$frameStats.duration = thisTime - lastTime;
	$frameStats.invDuration = 1.0 / duration;
	triggerEvent("onUpdateFrame10");
	triggerEvent("onUpdateFrame20");
	triggerEvent("onUpdateFrame30");
	triggerEvent("onUpdateFrame");
	triggerEvent("onUpdateFreeRotation");
	$frameStats.lastTime = thisTime;
	if (getFrameCount() < 20.0)
	{
		$frameStats.first20Time = thisTime;
	}
	return;
}
function savePeriodFrameCount(%period)
{
	$frameStats.lastFrameCount[%period] = thisFrameCount[%period];
	$frameStats.thisFrameCount[%period] = getFrameCount();
	schedule(%period * 1000.0, "0", "savePeriodFrameCount", %period);
	return;
}
function onWindowFocusChange(%isFocused)
{
	echo("onWindowFocusChange:" SPC %isFocused);
	return;
	if ($WII)
	{
		return;
	}
	if (%isFocused)
	{
		if (getOS() $= "mac" && $settings::Video::Fullscreen)
		{
			setNewResolution(getX($settings::Video::Full), getY($settings::Video::Full), "32", "1");
		}
		$MenuAudioChannel.setVolume($settings::Audio::Music);
	}
	else
	{
		if (getOS() $= "mac" && isFullScreen())
		{
			setNewResolution(getX($settings::Video::Window), getY($settings::Video::Window), "32", "0");
		}
		$MenuAudioChannel.setVolume("0");
	}
	Canvas.showCursor();
	if (!($firstTickOccured))
	{
		return;
	}
	if (!(%isFocused))
	{
		%pauseIt = 1;
		%i = 0;
		while (%i < Canvas.getCount())
		{
			%name = Canvas.getObject(%i).getName();
			if (strpos(%name, "menu") > -1.0)
			{
				%pauseIt = 0;
				break;
			}
			%i = %i + 1.0;
		}
		if (%pauseIt)
		{
			menu_loading.back();
		}
	}
	return;
}
function endGame()
{
	$gameIsQuitting = 1;
	levelShutdown();
	%actionmapsToDestroy = "moveMap resistentMoveMap splashScreenMap analogJoystickMap debugMap debugJumpMap keyAssignmentMap buttonAssignmentMap";
	%i = 0;
	while (%i < getWordCount(%actionmapsToDestroy))
	{
		%actionMap = getWord(%actionmapsToDestroy, %i);
		if (isObject(%actionMap))
		{
			%actionMap.pop();
			%actionMap.delete();
		}
		%i = %i + 1.0;
	}
	OpenALShutdownDriver();
	debugEcho("we shut down! time," SPC getRealTime());
	return;
}
function clearGroups()
{
	%i = 0;
	while (%i < getWordCount($MAINGROUPLIST))
	{
		getWord($MAINGROUPLIST, %i).clear();
		%i = %i + 1.0;
	}
	return getWordCount($MAINGROUPLIST);
}
function initStatObjects()
{
	if (!(isObject($tickStats)))
	{
		$tickStats = new ScriptObject(Name : "");
	}
	$tickStats.lastTime = "0";
	$tickStats.thisTime = "0";
	$tickStats.duration = "0";
	$tickStats.duration32ms = "0.032";
	$tickStats.invDuration32ms = 1.0 / duration32ms;
	$tickStats.ticksUntilUpdate32ms = duration32ms * 1000.0 / getTickMs();
	$tickStats.duration = "0.128";
	$tickStats.invDuration = 1.0 / duration;
	$tickStats.realInvDuration = 1.0 / getTickMs();
	$tickStats.ticksUntilUpdate = duration * 1000.0 / getTickMs();
	setScriptTickDuration(duration);
	$tickStats.allTicks = "0";
	if (!(isObject($frameStats)))
	{
		$frameStats = new ScriptObject(Name : frameStats);
	}
	$frameStats.lastTime = "0";
	$frameStats.thisTime = "0";
	$frameStats.duration = "0";
	return;
}
function setupDemoVersion()
{
	if ($demoVersion)
	{
		$LEVELLIST = $DEMOLEVELLIST;
		$levelDetailList = "";
		%i = 1;
		while (%i < getWordCount($DEMOLEVELLIST))
		{
			$levelDetailList = $levelDetailList SPC getLastToken(getWord($DEMOLEVELLIST, %i), "_");
			%i = %i + 1.0;
		}
		$levelDetailList = trim($levelDetailList);
		echo("its a demo with levels:" SPC $LEVELLIST);
	}
	else
	{
		if ($previewVersion)
		{
			$LEVELLIST = $PREVIEWLEVELLIST;
			break;
		}
		if ($WII)
		{
			$LEVELLIST = $WIILEVELLIST;
			break;
		}
		$LEVELLIST = $FULLLEVELLIST;
	}
	return;
}
function quitGame()
{
	debugEcho("quiting game" SPC %this);
	saveSettings(-1.0);
	saveSettings($settings::Profile::currentProfile);
	return;
}
