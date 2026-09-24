// playmodeManager.cs.dso
function playmodeManager::init()
{
	if (!(isObject(playmodeManager)))
	{
		new ScriptObject(Name : playmodeManager);
	}
	playmodeManager.activeMode = storyMode;
	playmodeManager.failedSmoother = Smoother::createInstance();
	gameGarbageCollector.add(failedSmoother);
	SpeedRunMode::init();
	limitedRotationMode::init();
	survivalMode::init();
	RallyMode::init();
	return;
}
function playmodeManager::setMode(%this, %modeName)
{
	if (%modeName $= "Story")
	{
		%this.modeSet = "0";
	}
	else
	{
		if (%modeName $= "SpeedRun")
		{
			%this.modeSet = SpeedRunMode;
			if (isUpdateDiscoverable("speedrunmode"))
			{
				setUpdateDiscovered("speedrunmode");
			}
			break;
		}
		if (%modeName $= "Rally")
		{
			%this.modeSet = RallyMode;
			if (isUpdateDiscoverable("rallymode"))
			{
				setUpdateDiscovered("rallymode");
			}
			break;
		}
		if (%modeName $= "Survival")
		{
			%this.modeSet = survivalMode;
			if (isUpdateDiscoverable("survivalmode"))
			{
				setUpdateDiscovered("survivalmode");
			}
			break;
		}
		if (%modeName $= "LimitedRotation")
		{
			%this.modeSet = limitedRotationMode;
			if (isUpdateDiscoverable("limitedrotationmode"))
			{
				setUpdateDiscovered("limitedrotationmode");
			}
			break;
		}
		debugWarn("No such playmode:" SPC %modeName);
		return;
	}
	return;
}
function playmodeManager::getMode(%this)
{
	return modeSet;
	return modeSet;
}
function playmodeManager::startLevel(%this, %levelScriptObject)
{
	echo("starting level:" SPC %levelScriptObject SPC " in mode:" SPC modeSet.getName() SPC "st.wE" SPC wholeEnvironment);
	if (isObject(modeSet))
	{
		modeSet.levelIndex = getWordIndex($FULLLEVELLIST, %levelScriptObject.getName());
		modeSet.levelScriptObject = %levelScriptObject;
		modeSet.difficulty = max(getWord($settings::Playmodes::Stamps[modeSet], levelIndex), "0");
		modeSet.setupLevel();
	}
	if (!(active))
	{
		if (isObject($ghostToLoad))
		{
			$ghostToLoad.delete();
		}
	}
	settings.showHints = !(modeSet) && $settings::Other::ShowHints;
	if (wholeEnvironment && !(isMenu))
	{
		%levelScriptObject = "level_" @ environment;
		if ($ghostToLoad != -1.0)
		{
			$ghostToLoad.delete();
			$ghostToLoad = Replay::load(environment, getWord(environmentGhosts, environmentLevel));
		}
	}
	if (strpos(%levelScriptObject, "level_") < 0.0)
	{
		%levelScriptObject = "level_" @ %levelScriptObject;
	}
	%levelObject = getLevelScriptObject(fileName);
	if (dontMeasureTime)
	{
		%this.setMode("Story");
	}
	if ($WII)
	{
		unSubscribeFromEvents(wiiInput, "onUnpauseGame");
	}
	startGame(%levelObject);
	subscribeToEvents(playmodeManager, "onEnterLevelSwitch onLevelFadeOutEnded onLevelShutdown");
	subscribeToEvents(playmodeManager, "onSpawnPointActivate");
	%this.activeMode = modeSet;
	%this.modeToFinish = activeMode;
	return;
}
function playmodeManager::onSpawnpointActivate(%this)
{
	if (activeMode.getId() == RallyMode.getId() && !(isObject(modeToFinish)))
	{
		RallyMode.onSpawnpointActivate();
	}
	return;
}
function playmodeManager::onLevelLoadFinished(%this)
{
	if (isObject(modeSet) && !(isMenu))
	{
		modeSet.onLevelLoadFinished();
	}
	return;
}
function playmodeManager::onLevelShutdown(%this)
{
	Canvas.popDialog(failed_display);
	unSubscribeFromEvents(%this, "onUpdateFrame");
	if (isObject(activeMode))
	{
		activeMode.disableMode();
	}
	return;
}
function playmodeManager::onEnterLevelSwitch(%this)
{
	if (%this.isMedalMode(modeToFinish))
	{
		%mode = modeToFinish;
		%difficulty = difficulty;
		while (%difficulty >= 0.0)
		{
			if (%mode.beatDifficulty(%difficulty))
			{
				%difficulty = %difficulty - 1.0;
			}
			else
			{
				break;
			}
		}
		if (%difficulty < getWord($settings::Playmodes::Stamps[%mode], levelIndex))
		{
			%bitmap = "game/" @ $DataFolder @ "/images/Menu/stamps/" @ %mode @ "_" @ getWord("gold silver bronze", %difficulty + 1.0) @ ".png";
			achievements.showPopup(%bitmap, $lbl_bravo, $lbl_getStamp);
			$settings::Playmodes::Stamps[%mode] = setWord($settings::Playmodes::Stamps[%mode], levelIndex, %difficulty);
			%this.checkStampAchievement();
		}
	}
	return;
}
function playmodeManager::checkStampAchievement(%this, %mode)
{
	if (%mode $= "")
	{
		%mode = modeToFinish;
	}
	%stampCount = 0;
	if (achievements.getAchieved("5" @ %mode))
	{
		return achievements.getAchieved("5" @ %mode);
	}
	debugEcho("checking stamps of this mode" SPC %mode);
	%i = 1;
	while (%i < getWordCount($settings::Playmodes::Stamps[%mode]))
	{
		if (%i == 17.0)
		{
		}
		else
		{
			if (getWord($settings::Playmodes::Stamps[%mode], %i) < 2.0)
			{
				%stampCount = %stampCount + 1.0;
			}
			if (%stampCount >= 5.0)
			{
				achievements.setAchieved("5" @ %mode);
				break;
			}
		}
		%i = %i + 1.0;
	}
	return getWordCount($settings::Playmodes::Stamps[%mode]);
}
function playmodeManager::onLevelFadeOutEnded(%this)
{
	achievements.checkCrazy();
	return;
}
function playmodeManager::disableMode(%this)
{
	if (!(isObject(modeToFinish)))
	{
		return isObject(modeToFinish);
	}
	%this.modeToFinish = "0";
	if (isObject(activeMode))
	{
		activeMode.disableMode();
	}
	if (!($WII))
	{
		%resetKeyIndex = getWordIndex($ALL_GAME_ACTIONS, "reset");
		%resetKey = getWord($settings::Controls::KeyboardLayout, %resetKeyIndex);
		lbl_reset_key.text = $lbl_press SPC %resetKey;
	}
	failed_display.setPosition(getX(failed_display.getPosition()), -130.0);
	Canvas.pushDialog(failed_display);
	subscribeToEvents(%this, "onUpdateFrame");
	failedSmoother.init("0.5", -130.0, "0", "SMOOTH");
	failedSmoother.start();
	return;
}
function playmodeManager::onUpdateFrame(%this)
{
	failed_display.setPosition(getX(failed_display.getPosition()), failedSmoother.getValue());
	if (failedSmoother.getIsFinished())
	{
		unSubscribeFromEvents(%this, "onUpdateFrame");
	}
	return;
}
function playmodeManager::backToMenu(%this, %goToSubMenu)
{
	menu_main.goToSubMenu = %goToSubMenu;
	%this.setMode("Story");
	if (isObject(activeMode))
	{
		debugEcho("disabling this mode" SPC activeMode);
		activeMode.disableMode();
	}
	MenuAction::loadLevel("level_gameMenu");
	subscribeToEvents(menu_main, "onMenuLoadFinished");
	return;
}
function playmodeManager::isTimeMode(%this)
{
	if (isObject(activeMode))
	{
		return activeMode.getId() == SpeedRunMode.getId() && !(competeWithoutTimes) || activeMode.getId() == RallyMode.getId();
	}
	else
	{
		return "0";
	}
	return "0";
}
function playmodeManager::isSpeedRunMode(%this, %mode)
{
	if (%mode $= "")
	{
		%mode = %this.getMode();
	}
	if (!(isObject(%mode)))
	{
		return "0";
	}
	return %mode.getId() == SpeedRunMode.getId();
	return %mode.getId() == SpeedRunMode.getId();
}
function playmodeManager::isLimitedRotationMode(%this)
{
	if (!(isObject(%this.getMode())))
	{
		return "0";
	}
	return %this.getMode().getId() == limitedRotationMode.getId();
	return %this.getMode().getId() == limitedRotationMode.getId();
}
function playmodeManager::isMedalMode(%this, %mode)
{
	if (%mode $= "")
	{
		%mode = %this.getMode();
	}
	return isObject(%mode) && !(%this.isSpeedRunMode(%mode));
	return isObject(%mode) && !(%this.isSpeedRunMode(%mode));
}
function playmodeManager::unlockPlaymode(%this, %achievementIndex)
{
	if ($demoVersion)
	{
		return isObject(%mode) && !(%this.isSpeedRunMode(%mode));
	}
	%indexToSet = -1.0;
	debugEcho("unlocking playmode?" SPC %achievementIndex SPC achievementIndex SPC achievementIndex SPC achievementIndex);
	if (%achievementIndex == achievementIndex)
	{
		%indexToSet = 0;
	}
	else
	{
		if (%achievementIndex == achievementIndex)
		{
			%indexToSet = 1;
			break;
		}
		if (%achievementIndex == achievementIndex)
		{
			%indexToSet = 2;
		}
	}
	if (%indexToSet > -1.0)
	{
		$settings::Playmodes::ModesUnlocked = setWord($settings::Playmodes::ModesUnlocked, %indexToSet, "1");
		saveSettings($settings::Profile::currentProfile);
	}
	return;
}
function enterPlaymodeMenu()
{
	$ACTUAL_MENU.enterMenu(menu_playmodes);
	return;
}
function menu_playmodes::onDialogPush(%this)
{
	%buttons = "btn_mode_rally btn_mode_limitedRotation btn_mode_survival";
	if (isUpdateDiscoverable("speedrunmode"))
	{
		btn_mode_speedRun.setBitmap("game/" @ $DataFolder @ "/images/Menu/button2_new");
	}
	else
	{
		btn_mode_speedRun.setBitmap("game/" @ $DataFolder @ "/images/Menu/button2");
	}
	if (isUpdateDiscoverable("rallymode"))
	{
		btn_mode_rally.setBitmap("game/" @ $DataFolder @ "/images/Menu/button3_new");
	}
	else
	{
		btn_mode_rally.setBitmap("game/" @ $DataFolder @ "/images/Menu/button3");
	}
	if (isUpdateDiscoverable("limitedrotationmode"))
	{
		btn_mode_limitedRotation.setBitmap("game/" @ $DataFolder @ "/images/Menu/button1_new");
	}
	else
	{
		btn_mode_limitedRotation.setBitmap("game/" @ $DataFolder @ "/images/Menu/button1");
	}
	if (isUpdateDiscoverable("survivalmode"))
	{
		btn_mode_survival.setBitmap("game/" @ $DataFolder @ "/images/Menu/button2_new");
	}
	else
	{
		btn_mode_survival.setBitmap("game/" @ $DataFolder @ "/images/Menu/button2");
	}
	%i = 0;
	while (%i < getWordCount($settings::Playmodes::ModesUnlocked))
	{
		getWord(%buttons, %i).setActive(getWord($settings::Playmodes::ModesUnlocked, %i));
		%i = %i + 1.0;
	}
	if ($WII && $demoVersion)
	{
		btn_mode_speedRun.setActive("0");
	}
	lbl_mode_unlock.setVisible("0");
	%buttons = "story speedRun limitedRotation rally survival";
	if ($settings::Personal::Language $= "french")
	{
		debugEcho("adjusting these buttons" SPC %buttons);
		%i = 0;
		while (%i < getWordCount(%buttons))
		{
			%currentButton = "btn_mode_" @ getWord(%buttons, %i);
			%currentButton.Extent = "215 90";
			debugEcho("curBut" SPC %currentButton SPC "extent:" SPC Extent);
			%i = %i + 1.0;
		}
	}
	else
	{
		if (Extent != "180 90")
		{
			%i = 0;
			while (%i < getWordCount(%buttons))
			{
				%currentButton = "btn_mode_" @ getWord(%buttons, %i);
				%currentButton.Extent = "180 90";
				%i = %i + 1.0;
			}
		}
	}
	return getWordCount(%buttons);
}
function PlaymodeButton::onAdd(%this)
{
	%this.mode = getLastToken(%this.getName(), "_");
	%this.Name = mode;
	%this.text = Name;
	return;
}
function PlaymodeButton::onWake(%this)
{
	if (mode $= "rally")
	{
		%this.achievement = getField(names, achievementIndex);
	}
	else
	{
		if (mode $= "limitedRotation")
		{
			%this.achievement = getField(names, achievementIndex);
			break;
		}
		if (mode $= "survival")
		{
			%this.achievement = getField(names, achievementIndex);
		}
	}
	return;
}
function PlaymodeButton::onAction(%this)
{
	playmodeManager.setMode(mode);
	$ACTUAL_MENU.enterMenu(menu_level);
	return;
}
function PlaymodeButton::onMouseEnter(%this)
{
	if (!(%this.isActive()))
	{
		if ($demoVersion)
		{
			lbl_mode_unlock.text = $lbl_requires_full_version;
		}
		else
		{
			lbl_mode_unlock.text = ['$lbl_mode_unlock_1', 'achievement', '$lbl_mode_unlock_2'];
		}
		lbl_mode_unlock.setVisible("1");
	}
	return;
}
function PlaymodeButton::onMouseLeave(%this)
{
	lbl_mode_unlock.setVisible("0");
	return;
}
