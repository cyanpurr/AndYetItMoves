// main.cs.dso
exec("./stuff.cs");
exec("./distributorName.cs");
$flagAllocs = 0;
$dumpAllocs = 0;
$takeSnapshot = 0;
$dumpSnapshot = 0;
$memDumpCounter = 0;
$executeMinimum = $enableDebugMap & 0;
if ($demoVersion)
{
	$waitForOnlineUser = 0;
}
else
{
	$waitForOnlineUser = 5000;
}
if (!($WII))
{
	exec("~/data/lang/english.cs");
	$DSO_EXTENSION = "";
	$DataFolder = "data";
}
else
{
	$DSO_EXTENSION = "";
	$DataFolder = "wiiData";
}
exec("~/gameScripts/utilities/preExecStuff.cs" @ $DSO_EXTENSION);
exec("./gameScripts/gui/alterableMsg.cs" @ $DSO_EXTENSION);
function initializeProject()
{
	if (!($enableDebugMap))
	{
		GlobalActionMap.unbind("keyboard", $Game::ConsoleBind);
		if (getOS() != "linux")
		{
			GlobalActionMap.unbind("keyboard", $Game::FullscreenBind);
		}
	}
	GlobalActionMap.unbind("keyboard", $Game::ScreenshotBind);
	if ($WII)
	{
		exec("~/../common/gameScripts/language.cs");
		exec("~/gui/hud.gui" @ $DSO_EXTENSION);
		new WiiInputManager(Name : wiiInput);
		wiiInput.init();
		wiiMoteDisableSpeaker();
	}
	if ($WII && getOS() $= "wii")
	{
		loadWristStrap();
	}
	else
	{
		onWristStrapDone();
	}
	return;
}
function onWristStrapDone()
{
	if ($WII)
	{
		wiiInput.setInput("menu");
		if (getOS() != "wii")
		{
			$saveOnWii = 1;
			postFlashInit();
		}
		else
		{
			debugEcho("initing wii flash");
			wiiFlashInit();
		}
	}
	else
	{
		postFlashInit();
	}
	return;
}
function postFlashInit()
{
	if ($WII)
	{
		debugEcho("are we saving on wii?" SPC $saveOnWii);
		exec("~/gameScripts/guiProfilesWii.cs" @ $DSO_EXTENSION);
	}
	else
	{
		exec("~/gameScripts/guiProfiles.cs" @ $DSO_EXTENSION);
	}
	exec("~/gui/oneLineWarning.gui" @ $DSO_EXTENSION);
	exec("~/gui/mainScreen.gui" @ $DSO_EXTENSION);
	if (getPublisherName() != "")
	{
		exec("~/gameScripts/externalUser.cs" @ $DSO_EXTENSION);
		if (!(checkExternalClient()))
		{
			return "0";
		}
	}
	if ($demoVersion)
	{
	}
	else
	{
	}
	%suffix = "";
	if ($currentVersionAppend $= "")
	{
	}
	else
	{
	}
	%append = " " @ $currentVersionAppend;
	echo("");
	echo(['"And Yet It Moves v"', '$currentVersion', '%suffix', '%append'] NL "Distributor is" SPC $distributorName NL "time:" SPC getRealTime());
	applyGlobalOptions();
	execAll();
	$STARTUP_DESKTOP_RESOLUTION = getDesktopResolution();
	loadMenuScrips();
	initLevelScriptObjects();
	initMenu();
	exec("~/gui/wiiInstructions.gui" @ $DSO_EXTENSION);
	exec("~/gui/timeDisplay.gui" @ $DSO_EXTENSION);
	exec("~/gui/survivalDisplay.gui" @ $DSO_EXTENSION);
	exec("~/gui/rotationDisplay.gui" @ $DSO_EXTENSION);
	exec("~/gui/rallyDisplay.gui" @ $DSO_EXTENSION);
	if ($WII)
	{
		exec("~/gui/failedDisplayWii.gui" @ $DSO_EXTENSION);
	}
	else
	{
		exec("~/gui/failedDisplay.gui" @ $DSO_EXTENSION);
	}
	createGroups();
	initAchievements();
	initSettings();
	mainInits();
	videoInit();
	echo("video now inited!");
	audioInit();
	exec("./gameScripts/game.cs" @ $DSO_EXTENSION);
	startupMenu();
	return;
}
function startupMenu()
{
	startGame(expandFilename($Game::DefaultScene));
	return;
}
function mainInits()
{
	initKeyboard();
	if (isSixensePossible())
	{
		new sixenseManager(Name : sixenseManager);
		sixenseManager.init();
	}
	else
	{
		$settings::Controls::SixenseEnabled = 0;
	}
	if ($WII)
	{
		%index = getWordIndex($ALL_WII_INPUT_MODES, $settings::Wii::WiiInputMode);
		wiiInput.setSensitivity(getWord($settings::Controls::Sensitivity, %index));
		wiiInput.setInverted(getWord($settings::Controls::Inverted, %index));
	}
	initGlobals();
	Statistics::init();
	Webclient::createInstance();
	playmodeManager::init();
	return;
}
function videoInit()
{
	if ($WII)
	{
		%res = getDesktopResolution();
		%fs = 1;
	}
	else
	{
		if ($settings::Video::Fullscreen)
		{
			%res = $settings::Video::Full;
		}
		else
		{
			%res = $settings::Video::Window;
		}
		%fs = $settings::Video::Fullscreen;
	}
	setNewResolution(getX(%res), getY(%res), "32", %fs);
	return;
}
function audioInit()
{
	$FXAudioChannel.setVolume($settings::Audio::Effects);
	$MenuAudioChannel.setVolume($settings::Audio::Music);
	$MusicAudioChannel["1"].setVolume($settings::Audio::Music);
	$MusicAudioChannel["2"].setVolume($settings::Audio::Music);
	$MusicAudioChannel["3"].setVolume($settings::Audio::Music);
	if ($WII)
	{
		wiiMoteDisableSpeaker("0");
	}
	return;
}
function execAll()
{
	exec("./gameScripts/settings.cs" @ $DSO_EXTENSION);
	exec("./gameScripts/input.cs" @ $DSO_EXTENSION);
	exec("./gameScripts/grabInput.cs" @ $DSO_EXTENSION);
	exec("./gameScripts/utilities/geometric.cs" @ $DSO_EXTENSION);
	exec("./gameScripts/utilities/mathematic.cs" @ $DSO_EXTENSION);
	exec("./gameScripts/utilities/misc.cs" @ $DSO_EXTENSION);
	exec("./gameScripts/utilities/objectManipulation.cs" @ $DSO_EXTENSION);
	exec("./gameScripts/utilities/audio.cs" @ $DSO_EXTENSION);
	exec("./gameScripts/utilities/debugAndTesting.cs" @ $DSO_EXTENSION);
	exec("./gameScripts/eventSystem.cs" @ $DSO_EXTENSION);
	exec("./gameScripts/safeSchedule.cs" @ $DSO_EXTENSION);
	exec("./gameScripts/callNextFrame.cs" @ $DSO_EXTENSION);
	if (!($executeMinimum))
	{
		exec("./gameScripts/player.cs" @ $DSO_EXTENSION);
		exec("./gameScripts/playerMovement.cs" @ $DSO_EXTENSION);
		exec("./gameScripts/playerDeath.cs" @ $DSO_EXTENSION);
		exec("./gameScripts/playerStateMachine.cs" @ $DSO_EXTENSION);
		exec("./gameScripts/respawn.cs" @ $DSO_EXTENSION);
		exec("./gameScripts/smoother.cs" @ $DSO_EXTENSION);
		exec("./gameScripts/smootherGroup.cs" @ $DSO_EXTENSION);
		exec("./gameScripts/smootherGlobal.cs" @ $DSO_EXTENSION);
		exec("./gameScripts/shaker.cs" @ $DSO_EXTENSION);
		exec("./gameScripts/camera.cs" @ $DSO_EXTENSION);
		exec("./gameScripts/layer.cs" @ $DSO_EXTENSION);
	}
	exec("./gameScripts/globals.cs" @ $DSO_EXTENSION);
	exec("./gameScripts/levelLoading.cs" @ $DSO_EXTENSION);
	exec("./gameScripts/statistics.cs" @ $DSO_EXTENSION);
	if (!($executeMinimum))
	{
		exec("./gameScripts/replay.cs" @ $DSO_EXTENSION);
		exec("./gameScripts/playmodeManager.cs" @ $DSO_EXTENSION);
		exec("./gameScripts/speedRunMode.cs" @ $DSO_EXTENSION);
		exec("./gameScripts/limitedRotationMode.cs" @ $DSO_EXTENSION);
		exec("./gameScripts/survivalMode.cs" @ $DSO_EXTENSION);
		exec("./gameScripts/rallyMode.cs" @ $DSO_EXTENSION);
		exec("./gameScripts/cheats.cs" @ $DSO_EXTENSION);
	}
	if (!($executeMinimum))
	{
		exec("./gameScripts/audio/audioChannels.cs" @ $DSO_EXTENSION);
		exec("./gameScripts/audio/audioDatablocks.cs" @ $DSO_EXTENSION);
		exec("./gameScripts/audio/soundQueue.cs" @ $DSO_EXTENSION);
		exec("./gameScripts/audio/gameEventSounds.cs" @ $DSO_EXTENSION);
		exec("./gameScripts/audio/markovScriptObjects.cs" @ $DSO_EXTENSION);
		exec("./gameScripts/audio/beatGenerator.cs" @ $DSO_EXTENSION);
		exec("./gameScripts/audio/menuSound.cs" @ $DSO_EXTENSION);
		exec("./gameScripts/jungle/flame.cs" @ $DSO_EXTENSION);
		exec("./gameScripts/jungle/spark.cs" @ $DSO_EXTENSION);
		exec("./gameScripts/jungle/flameControler.cs" @ $DSO_EXTENSION);
		exec("./gameScripts/jungle/bees.cs" @ $DSO_EXTENSION);
		exec("./gameScripts/trip/alterEgo.cs" @ $DSO_EXTENSION);
		exec("./gameScripts/trip/selfDiscovery.cs" @ $DSO_EXTENSION);
		exec("./gameScripts/trip/vanishingGrid.cs" @ $DSO_EXTENSION);
		exec("./gameScripts/finalLevel/finalLevelStuff.cs" @ $DSO_EXTENSION);
		exec("./gameScripts/InvisibleBehaviors/controlPlayerSounds.cs" @ $DSO_EXTENSION);
		exec("./gameScripts/InvisibleBehaviors/fragOnCollision.cs" @ $DSO_EXTENSION);
		exec("./gameScripts/InvisibleBehaviors/keepOrientation.cs" @ $DSO_EXTENSION);
	}
	exec("./gameScripts/gui/subMenu.cs" @ $DSO_EXTENSION);
	exec("./gameScripts/gui/levelDetail.cs" @ $DSO_EXTENSION);
	exec("./gameScripts/gui/menuStatistics.cs" @ $DSO_EXTENSION);
	exec("./gameScripts/gui/switchProfiles.cs" @ $DSO_EXTENSION);
	exec("./gameScripts/gui/list.cs" @ $DSO_EXTENSION);
	exec("./gameScripts/gui/menuAction.cs" @ $DSO_EXTENSION);
	exec("./gameScripts/gui/menu.cs" @ $DSO_EXTENSION);
	exec("./gameScripts/gui/menuTab.cs" @ $DSO_EXTENSION);
	exec("./gameScripts/gui/fileIO.cs" @ $DSO_EXTENSION);
	exec("./gameScripts/gui/achievements.cs" @ $DSO_EXTENSION);
	exec("./gameScripts/gui/options.cs" @ $DSO_EXTENSION);
	if (!($executeMinimum))
	{
		exec("./gameScripts/web/webclient.cs" @ $DSO_EXTENSION);
	}
	return;
}
function createGroups()
{
	%i = 0;
	while (%i < getWordCount($MAINGROUPLIST))
	{
		new SimSet(Name : getWord($MAINGROUPLIST, %i));
		%i = %i + 1.0;
	}
	return getWordCount($MAINGROUPLIST);
}
function applyGlobalOptions()
{
	$soundEnabled = 1;
	$ambientEnabled = 1;
	$disableZoomTrigger = 0;
	$pref::T2D::warnSceneOccupancy = 1;
	$pref::T2D::dualCollisionCallbacks = 0;
	$pref::T2D::imageMapEchoErrors = 1;
	echo("T2D System Variales:" NL "    warnFileDeprecated" SPC $pref::T2D::warnFileDeprecated NL "    warnSceneOccupancy" SPC $pref::T2D::warnSceneOccupancy NL "    dualCollisionCallbacks" SPC $pref::T2D::dualCollisionCallbacks NL "    imageMapDumpTextures" SPC $pref::T2D::imageMapDumpTextures NL "    imageMapEchoErrors" SPC $pref::T2D::imageMapEchoErrors NL "    imageMapFixedMaxTextureSize" SPC $pref::T2D::imageMapFixedMaxTextureSize NL "    imageMapFixedMaxTextureError" SPC $pref::T2D::imageMapFixedMaxTextureError NL "    imageMapShowPacking" SPC $pref::T2D::imageMapShowPacking);
	return;
}
function shutdownProject()
{
	endGame();
	return;
}
function setupKeybinds()
{
	return;
}
