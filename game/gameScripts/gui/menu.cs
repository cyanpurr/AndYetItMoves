// menu.cs.dso
function loadMenuScrips()
{
	exec("~/gameScripts/gui/cursor.cs" @ $DSO_EXTENSION);
	if ($enableDebugMap)
	{
		exec("~/gui/safeFrame.gui" @ $DSO_EXTENSION);
	}
	if ($WII)
	{
		if ($demoVersion)
		{
			exec("~/gui/mainMenuWiiDemo.gui" @ $DSO_EXTENSION);
		}
		else
		{
			exec("~/gui/mainMenuWii.gui" @ $DSO_EXTENSION);
		}
		exec("~/gui/startGameWii.gui" @ $DSO_EXTENSION);
		exec("~/gui/levelDetailWii.gui" @ $DSO_EXTENSION);
		exec("~/gui/statisticsWii.gui" @ $DSO_EXTENSION);
		exec("~/gui/optionsWii.gui" @ $DSO_EXTENSION);
		exec("~/gui/playmodesWii.gui" @ $DSO_EXTENSION);
		exec("~/gui/resetWii.gui" @ $DSO_EXTENSION);
		exec("~/gui/profilesWii.gui" @ $DSO_EXTENSION);
	}
	else
	{
		exec("~/gui/createProfile.gui" @ $DSO_EXTENSION);
		exec("~/gui/mainMenu.gui" @ $DSO_EXTENSION);
		exec("~/gui/startGame.gui" @ $DSO_EXTENSION);
		if (isSixensePossible())
		{
			exec("~/gui/optionsSixense.gui" @ $DSO_EXTENSION);
		}
		else
		{
			exec("~/gui/options.gui" @ $DSO_EXTENSION);
		}
		exec("~/gui/playmodes.gui" @ $DSO_EXTENSION);
		exec("~/gui/levelDetail.gui" @ $DSO_EXTENSION);
		exec("~/gui/statistics.gui" @ $DSO_EXTENSION);
		exec("~/gui/startGame.gui" @ $DSO_EXTENSION);
		exec("~/gui/switchProfiles.gui" @ $DSO_EXTENSION);
		exec("~/gui/sixenseCalibration.gui" @ $DSO_EXTENSION);
		exec("~/gui/reset.gui" @ $DSO_EXTENSION);
	}
	exec("~/gui/oneLineDialog.gui" @ $DSO_EXTENSION);
	exec("~/gui/loading.gui" @ $DSO_EXTENSION);
	exec("~/gui/achievements.gui" @ $DSO_EXTENSION);
	exec("~/gui/custom.gui" @ $DSO_EXTENSION);
	exec("~/gui/choiceDialog.gui" @ $DSO_EXTENSION);
	exec("~/gui/achievementUnlocked.gui" @ $DSO_EXTENSION);
	exec("~/gui/enterKey.gui" @ $DSO_EXTENSION);
	exec("~/gui/resetNotification.gui" @ $DSO_EXTENSION);
	exec("~/gui/cheats.gui" @ $DSO_EXTENSION);
	if ($availableLanguagesNative $= "" || $availableLanguages $= "")
	{
		$availableLanguagesNative = "English	Deutsch	FranÃ§ais	Italiano	EspaÃ±ol	æ¥æ¬èª";
		$availableLanguages = "english	german	french	italian	spanish	japanese";
	}
	$spaceReplace = "___";
	$useableCharacters = " !#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[]^_`abcdefghijklmnopqrstuvwxyz{|}~Â°Â¢Â£Â§â¢Â¶ÃÂ®Â©â¢Â´Â¨â ÃÃâÂ±â¤â¥Â¥âââÏâ«ÂªÂºÎ©Ã¦Ã¸Â¿Â¡Â¬âÆâÂ«Â»â¦Â ÃÃÃÅÅââââââÃ·âÃ¿Å¸ââ¬â¹âºï¬â¡Â·âââ°ÃÃÃÃÃÃÃÃÃÃï£¿ÃÃÃÃÄ±ËËÂ¯ËËËÂ¸ËAaCcCcCcCcâdEeEeEeGgGgHhIiIiIiJjLlLl??LlNnNn?OoÃ¥ÃºRrRrSsSsÃ¤Ã¶TtTtUuUuUuWwYyÃ¼ZzZzÃ©Ã»Ã ?Ã?âÃ²?????????--?Ã±Ã³Ã«Ã­ÃÃ¬Ã®ÃÃÃ¡Ã¯ÃÃÃ´";
	$levelDetailList = "cave1 cave2 cave3 cave4 cave jungle1 jungle2 jungle3 jungle4 jungle5 jungle6 jungle trip1 trip2 trip3 trip4 trip5 trip6 trip finalLevel elevator labyrinth chase";
	$BTN_H_TAB_NORMAL = "game/" @ $DataFolder @ "/images/Menu/tabsHoriz";
	$BTN_H_TAB_ACTIVE = "game/" @ $DataFolder @ "/images/Menu/tabsHoriz_d";
	$BTN_V_TAB_NORMAL = "game/" @ $DataFolder @ "/images/Menu/tabsVert";
	$BTN_V_TAB_ACTIVE = "game/" @ $DataFolder @ "/images/Menu/tabsVert_d";
	$BTN_RADIO_NORMAL = "game/" @ $DataFolder @ "/images/Menu/radiobutton";
	$BTN_RADIO_SELECTED = "game/" @ $DataFolder @ "/images/Menu/radiobutton_selected";
	$BTN_SMALL_NORMAL = "game/" @ $DataFolder @ "/images/Menu/button_small";
	$BTN_SMALL_ACTIVE = "game/" @ $DataFolder @ "/images/Menu/button_small_active";
	$BTN_EMPTY = "game/" @ $DataFolder @ "/images/Menu/empty";
	$notifications = 2;
	$notificationCount = 0;
	return;
}
function setSmallButtonActive(%buttonName, %value)
{
	eval("btn_" @ %buttonName @ ".setActive(" @ %value @ ");");
	eval("ibtn_" @ %buttonName @ ".setActive(" @ %value @ ");");
	eval("lbl_" @ %buttonName @ ".setActive(" @ %value @ ");");
	return;
}
function initMenu()
{
	if ($WII)
	{
		menu_options.onTabSelected("joystick");
	}
	else
	{
		menu_options.onTabSelected("video");
	}
	debugEcho("last level selected was:" SPC $settings::Other::LastLevelTab);
	return;
}
function setupMenuLists()
{
	if (!(isObject(highscoreList)))
	{
		if ($WII)
		{
			menu_levelDetail.highscoreList = listContainer::createInstance(ctrl_levelDetail_main, "$lbl_levelDetail_rank" TAB "$lbl_user" TAB "$lbl_levelDetail_time" TAB "$lbl_levelDetail_deaths", "right left right right", "0.15 0.35 0.35 0.15", "1", "26", "0", "$lbl_levelDetail_time");
			break;
		}
		menu_levelDetail.highscoreList = listContainer::createInstance(ctrl_levelDetail_main, "$lbl_levelDetail_rank" TAB "$lbl_user" TAB "$lbl_levelDetail_location" TAB "$lbl_levelDetail_time" TAB "$lbl_levelDetail_deaths" TAB "$lbl_levelDetail_ghost", "right left left right right center", "0.14 0.31 0.2 0.2 0.075 0.075", "1", "24", "0", "$lbl_levelDetail_time");
	}
	if (!(isObject(competitorsList)))
	{
		menu_levelDetail.competitorsList = listContainer::createInstance(ctrl_levelDetail_main, "$lbl_levelDetail_ranks" TAB "$lbl_levelDetail_count" TAB "$lbl_competitors" TAB "$lbl_levelDetail_time" TAB "$lbl_levelDetail_deaths" TAB "$lbl_levelDetail_ghost", "right right left right right center", "0.275 0.075 0.3 0.2 0.075 0.075", "1", "30", "0", "$lbl_levelDetail_time");
	}
	if (!($WII) && !(isObject(list)))
	{
		menu_switchProfiles.list = listContainer::createInstance(ctrl_switchProfiles_list, "$lbl_user" TAB "$lbl_levelDetail_location" TAB "$lbl_switchProfiles_totalTime" TAB "$lbl_switchProfiles_totalDeaths" TAB "$lbl_switchProfiles_progress", "left left right right right", "0.3 0.3 0.15 0.1 0.15", "1", "24", "0", "$lbl_user");
	}
	if (!(isObject(list)))
	{
		menu_achievements.list = listContainer::createInstance(ctrl_achievements_main, "Icon	Text", "center center", "100 1", "0", "100", "1", "Text");
	}
	return;
}
function localizeLists()
{
	highscoreList.addTitles();
	competitorsList.addTitles();
	if (!($WII))
	{
		list.addTitles();
	}
	list.addTitles();
	return;
}
function setGameLanguage(%lang)
{
	if ($WII && getOS() $= "wii")
	{
		if (getLanguageCode() == 0.0)
		{
			%lang = "japanese";
			break;
		}
		if (getLanguageCode() == 1.0)
		{
			%lang = "english";
			break;
		}
		if (getLanguageCode() == 2.0)
		{
			%lang = "german";
			break;
		}
		if (getLanguageCode() == 3.0)
		{
			%lang = "french";
			break;
		}
		if (getLanguageCode() == 4.0)
		{
			%lang = "spanish";
			break;
		}
		if (getLanguageCode() == 5.0)
		{
			%lang = "italian";
			break;
		}
		%lang = "english";
	}
	if (%lang != "english")
	{
		exec("~/data/lang/english.cs");
	}
	$settings::Personal::Language = %lang;
	debugEcho("setting game language to:" SPC %lang);
	exec("~/data/lang/" @ %lang @ ".cs");
	achievements.localize();
	localizeLists();
	return;
}
function GuiControl::enterMenu(%this, %subMenu, %dontHidePrevious)
{
	%subMenu.previousMenu = %this;
	%subMenu.dontPushAgain = %dontHidePrevious;
	if (!(%dontHidePrevious))
	{
		%this.Hide();
	}
	%subMenu.display();
	return;
}
function GuiControl::display(%this, %dontPushAgain)
{
	startMenuMusic();
	if ($WII)
	{
		wiiInput.setInput("menu");
		Canvas.showCursor("0");
		Canvas.setCursor("0", ayimCursor);
	}
	else
	{
		Canvas.showCursor();
		if (isSixensePossible())
		{
			sixenseManager.setInput("menu");
		}
	}
	$ACTUAL_MENU = %this;
	%container = container;
	%position = t2dVectorScale(t2dVectorSub(getRes(), %this.getExtent()), "0.5");
	%this.setPosition(getWord(%position, "0"), getWord(%position, "1"));
	if (!(isObject(backButton)) && backButton)
	{
		%this.backButton = new GuiBitmapButtonTextCtrl(Name : "")
		{
			canSaveDynamicFields = "0";
			superclass = "menuItem";
			class = "ButtonBack";
			isContainer = "0";
			Profile = "AyimButtonProfile";
			HorizSizing = "left";
			VertSizing = "top";
			Position = "600 512";
			Extent = "180 90";
			MinExtent = "8 2";
			canSave = "1";
			Visible = "1";
			hovertime = "1000";
			text = $btn_back;
			groupNum = "-1";
			buttonType = "PushButton";
			UseMouseEvents = "0";
			bitmap = ['"game/"', '$DataFolder', '"/images/Menu/button1"'];
		}
		if ($WII)
		{
			%newPos = t2dVectorSub(%this.getExtent(), "200 108");
			backButton.setPosition(getX(%newPos), getY(%newPos));
		}
		%this.addGuiControl(backButton);
	}
	if (isObject(backButton))
	{
		backButton.text = $btn_back;
	}
	if (!(%dontPushAgain))
	{
		Canvas.pushDialog(%this);
	}
	return;
}
function GuiControl::close(%this)
{
	if (isObject(previousMenu))
	{
		%this.Hide();
		previousMenu.display(dontPushAgain);
	}
	else
	{
		debugEcho("'do you want to quit' dialog should be displayed now");
	}
	return;
}
function GuiControl::onEscapePressed(%this)
{
	%this.back();
	return;
}
function GuiControl::back(%this)
{
	%this.close();
	return;
}
function GuiControl::Hide(%this)
{
	Canvas.popDialog(%this);
	$PREVIOUS_MENU = %this;
	return;
}
function GuiControl::showOverlay(%this, )
{
	return;
}
function GuiControl::onEnterUp(%this)
{
	$ACTUAL_MENU.onEnterPressed();
	return;
}
function GuiControl::onEnterPressed(%this)
{
	return;
}
function GuiControl::onOKPressed(%this)
{
	return;
}
function GuiControl::onMouseUp(%this)
{
	return;
}
function menu_loading::back(%this)
{
	if (!(getIsGamePaused()))
	{
		debugEcho("calling pausgame from menu_loading back" SPC getRealTime());
		pauseGame();
	}
	if (isObject(currentLevelObject) && environment > -1.0)
	{
		if (environment == 0.0)
		{
			tablbl_level_cave.switchEnvironment("cave");
			break;
		}
		if (environment == 1.0)
		{
			tablbl_level_jungle.switchEnvironment("jungle");
			break;
		}
		if (environment == 2.0)
		{
			tablbl_level_trip.switchEnvironment("trip");
			break;
		}
		if (environment == 3.0)
		{
			tablbl_level_epilog.switchEnvironment("epilog");
		}
	}
	if ($WII)
	{
		Canvas.showCursor("0");
		Canvas.setCursor("0", ayimCursor);
	}
	else
	{
		Canvas.showCursor();
	}
	%this.enterMenu(menu_main);
	return;
}
function menu_loading::display(%this, %dontPushAgain)
{
	$ACTUAL_MENU = %this;
	%container = container;
	%position = t2dVectorScale(t2dVectorSub(getRes(), %this.getExtent()), "0.5");
	%this.setPosition(getWord(%position, "0"), getWord(%position, "1"));
	if ($WII)
	{
		Canvas.setCursor("0", noCursor);
	}
	else
	{
		if ($settings::Video::Fullscreen)
		{
			Canvas.hideCursor();
		}
	}
	stopMenuMusic();
	if (getIsGamePaused())
	{
		debugEcho("calling pauseGame from menu_loading::diaplay" SPC getRealTime());
		pauseGame();
	}
	if (!($firstTickOccured))
	{
		Canvas.pushDialog(%this);
	}
	return;
}
function menu_loading::onEscapePressed(%this)
{
	if (initialFadeCompleted)
	{
		%this.back();
	}
	return;
}
function menu_main::onDialogPush(%this)
{
	achievements.checkAchievementStatus();
	lbl_main_version.text = $lbl_version_short SPC getSubStr($currentVersion, "0", "3") SPC "." SPC getSubStr($currentVersion, "3", strlen($currentVersion)) SPC $currentVersionAppend;
	if (isUpdateDiscoverable("rallymode") || isUpdateDiscoverable("speedrunmode") || isUpdateDiscoverable("limitedrotationmode") || isUpdateDiscoverable("survivalmode"))
	{
		btn_main_level.setBitmap("game/" @ $DataFolder @ "/images/Menu/button3_new");
	}
	else
	{
		btn_main_level.setBitmap("game/" @ $DataFolder @ "/images/Menu/button3");
	}
	if (isUpdateDiscoverable("achievementnum28") || isUpdateDiscoverable("steamSummerAchievement"))
	{
		btn_main_achievements.setBitmap("game/" @ $DataFolder @ "/images/Menu/button2_new");
	}
	else
	{
		btn_main_achievements.setBitmap("game/" @ $DataFolder @ "/images/Menu/button2");
	}
	if (isUpdateDiscoverable("modifications") || isUpdateDiscoverable("speedmodification") || isUpdateDiscoverable("freerotationmodification") || isUpdateDiscoverable("fixedcameramodification"))
	{
		btn_main_cheats.setBitmap("game/" @ $DataFolder @ "/images/Menu/button3_new");
	}
	else
	{
		btn_main_cheats.setBitmap("game/" @ $DataFolder @ "/images/Menu/button3");
	}
	if (gotoCompetition)
	{
		%this.gotoCompetition = "0";
		enterPlaymodeMenu();
		btn_mode_speedRun.onAction();
		SpeedRunMode.active = "0";
		playButton.onAction();
	}
	else
	{
		setupMenuLists();
		if (!($WII) && $distributorName != "BigFishGames")
		{
			%this.checkUpdateNews();
		}
	}
	if ($WII)
	{
		lbl_main_hello.text = getUserName($settings::Profile::currentProfile);
	}
	else
	{
		lbl_main_hello.text = $lbl_hello SPC getUserName($settings::Profile::currentProfile);
	}
	if ($WII && $demoVersion)
	{
		lbl_main_hello.text = $lbl_ayim_demo;
		btn_main_startGame.text = $lbl_play_demo;
		btn_main_startGame.Command = "MenuAction::continueGame(true);";
		btn_main_buy.text = $lbl_get_full_version;
		btn_main_level.setVisible("1");
	}
	else
	{
		if ($SnowdriftlandSpecial)
		{
			btn_main_startGame.Command = "MenuAction::continueGame(true);";
			btn_main_level.setVisible("0");
			btn_main_achievements.setVisible("0");
			btn_main_switchProfiles.setVisible("0");
			break;
		}
		if ($settings::Secrets::LevelsUnlocked >= 17.0 || $demoVersion && $settings::Secrets::LevelsUnlocked >= getWordCount($DEMOLEVELLIST) - 2.0)
		{
			btn_main_startGame.text = $lbl_chooseLevel;
			btn_main_startGame.Command = "menu_main.chooseLevel();";
			btn_main_level.setVisible("1");
			break;
		}
		if ($settings::Secrets::LevelsUnlocked == 0.0)
		{
			btn_main_startGame.text = $btn_startGame;
			btn_main_startGame.Command = "MenuAction::continueGame();";
			btn_main_level.setVisible("0");
			break;
		}
		if ($settings::Secrets::LevelsUnlocked < 17.0)
		{
			btn_main_startGame.text = $btn_resumeGame;
			btn_main_startGame.Command = "MenuAction::continueGame();";
			btn_main_level.setVisible("1");
		}
	}
	if (getIsGamePaused())
	{
		btn_main_startGame.text = $btn_resumeGame;
		btn_main_startGame.Command = "MenuAction::continueGame();";
	}
	if ($demoVersion)
	{
		if (competeAfterDemo)
		{
			enterPlaymodeMenu();
			btn_mode_speedRun.onAction();
			SpeedRunMode.active = "0";
			%firstLevelButton = "btn_" @ getWord($LEVELLIST, "1");
			playButton.onAction();
			if ($distributorName $= "BigFishGames")
			{
				btn_levelDetail_local.refreshList(local);
			}
			else
			{
				btn_levelDetail_competitors.refreshList(competitors);
			}
			%this.competeAfterDemo = "0";
		}
	}
	if (!($demoVersion) || $distributorName $= "BigFishGames")
	{
		btn_main_buy.setVisible("0");
	}
	if ($previewVersion)
	{
		btn_main_switchProfiles.setActive("0");
	}
	if ($WII && !($saveOnWii) || $demoVersion)
	{
		btn_main_switchProfiles.setActive("0");
	}
	if (!(wasDisplayedBefore))
	{
		menu_main.schedule("1000", "saveGamePlayed");
		menu_main.wasDisplayedBefore = "1";
	}
	return;
}
function menu_main::buyFullVersion(%this)
{
	if ($WII)
	{
		gotoWiiShop();
	}
	else
	{
		if ($distributorName $= "Greenhouse")
		{
			gotoWebPage("http://www.playgreenhouse.com/game/BRKRL-000001-01/");
			break;
		}
		if ($distributorName $= "Steam")
		{
			gotoWebPage("http://store.steampowered.com/app/18700/");
			break;
		}
		if ($distributorName $= "BigFishGames")
		{
			quitGame();
			break;
		}
		if ($distributorName $= "Yawma")
		{
			gotoWebPage("http://www.yawma.net/games/and-yet-it-moves");
			break;
		}
		if ($distributorName $= "Playism")
		{
			gotoWebPage("http://www.playism.jp/product/detail.php?game=26");
			break;
		}
		gotoWebPage("http://www.andyetitmoves.net");
	}
	return;
}
function menu_main::chooseLevel(%this)
{
	playmodeManager.setMode("Story");
	$ACTUAL_MENU.enterMenu(menu_level);
	return;
}
function menu_main::checkUpdateNews(%this)
{
	if (!(checkedForNewVersion))
	{
		%this.updateDialogShown = "0";
		%serverVersion = mFloatLength(Webclient.getCurrentVersion(), "3");
		debugEcho("comparing" SPC %serverVersion SPC $currentVersion);
		if (%serverVersion > $currentVersion && $settings::Other::VersionNotified < %serverVersion)
		{
			menu_dialog.serverVersion = %serverVersion;
			%text = $lbl_newversion_line1_pt1 @ mFloatLength(%serverVersion, "2") @ $lbl_newversion_line1_pt2;
			%updateURL = Webclient.getUpdateURL($demoVersion, $currentVersion, $distributorName);
			if ($distributorName $= "BigFishGames")
			{
				%text = %text NL $lbl_newversion_line2_pt1_bfg NL $currentVersion NL $lbl_newversion_line2_pt2;
				%command = "";
				%btn1_text = "";
				%btn2_text = $lbl_newversion_button2_bfg;
			}
			else
			{
				%text = %text NL $lbl_newversion_line2_pt1 NL $currentVersion NL $lbl_newversion_line2_pt2;
				%command = "menu_dialog.openWebPage("" @ %updateURL @ "", true);";
				%btn1_text = $lbl_newversion_button1;
				%btn2_text = $lbl_newversion_button2;
			}
			%text = %text NL $lbl_newversion_line3_pt1 NL $lbl_newversion_line3_pt2;
			menu_dialog.Show($lbl_newVersion_title, %text, %btn1_text, %btn2_text, $lbl_newversion_checkbox, "newVersion", %command);
			%this.updateDialogShown = "1";
		}
		%this.checkedForNewVersion = "1";
	}
	if (!(updateDialogShown))
	{
		%serverText = Webclient.getNews($settings::Other::ReceivedNews);
		%newsId = getRecord(%serverText, "0");
		if (%serverText != -1.0 && %newsId > $settings::Other::ReceivedNews)
		{
			$settings::Other::ReceivedNews = %newsId;
			%serverText = removeRecord(%serverText, "0");
			%allButtonText = getRecord(%serverText, "0");
			%buttonText = getField(%allButtonText, "0");
			%buttonUrl = getField(%allButtonText, "1");
			%buttonClose = getField(%allButtonText, "2");
			if (%buttonClose $= "")
			{
				%buttonClose = "false";
			}
			%textTabSep = getRecord(%serverText, "1");
			%title = getField(%textTabSep, "0");
			%text = getField(%textTabSep, "1");
			%i = 2;
			while (%i < getFieldCount(%textTabSep))
			{
				%text = %text NL getField(%textTabSep, %i);
				%i = %i + 1.0;
			}
			menu_dialog.Show(%title, %text, %buttonText, $lbl_news_button2, $lbl_news_checkbox, "", "menu_dialog.openWebPage("" @ %buttonUrl @ "", " @ %buttonClose @ ");");
		}
		%this.updateDialogShown = "1";
	}
	return;
}
function menu_createProfile::onDialogPush(%this)
{
	loading_image.getObject("0").setVisible("0");
	if (!($WII))
	{
		setQuittable("1");
	}
	if ($distributorName $= "BigFishGames")
	{
		lbl_createProfile_submit_1.setVisible("0");
		lbl_createProfile_submit_2.setVisible("0");
		rd_createProfile_dontSubmit.setVisible("0");
		rd_createProfile_submitTime.setVisible("0");
		rd_createProfile_submitGhost.setVisible("0");
	}
	debugEcho("init menu" SPC "name" SPC $lbl_enterName SPC "loc" SPC $lbl_enterLocation);
	lbl_createProfile_enterName.text = $lbl_enterName;
	lbl_createProfile_enterName.Profile = "AyimHeader2Profile";
	lbl_createProfile_enterLocation.text = $lbl_enterLocation;
	lbl_createProfile_enterLocation.Profile = "AyimHeader2Profile";
	lbl_createProfile_language.text = $lbl_enterLanguage;
	lbl_createProfile_language.Profile = "AyimHeader2Profile";
	txt_createProfile_location.text = $default_location;
	if ($WII)
	{
		txt_createProfile_name.text = $settings::Personal::Name;
	}
	if (isExternalUser($futureProfileId) && getUserName(-1.0) != -1.0)
	{
		txt_createProfile_name.text = getUserName(-1.0);
	}
	%this.oldName = text;
	if (editProfile)
	{
		if ($settings::Statistics::SubmitTime && $settings::Statistics::SubmitGhost)
		{
			rd_createProfile_submitGhost.setStateOn("1");
		}
		else
		{
			if ($settings::Statistics::SubmitTime)
			{
				rd_createProfile_submitTime.setStateOn("1");
				break;
			}
			rd_createProfile_dontSubmit.setStateOn("1");
		}
	}
	else
	{
		rd_createProfile_submitGhost.setStateOn("1");
	}
	if ($settings::Personal::Language $= "japanese" && text $= "ãã¬ã¤ã¤ã¼ãã¼ã¿ãä½æ")
	{
		debugEcho("stretching create button" SPC %this SPC text);
		btn_createProfile_create.setExtent("215", "90");
	}
	return;
}
function menu_createProfile::onDialogPop(%this)
{
	loading_image.getObject("0").setVisible("1");
	$futureProfileId = "";
	$showCreateProfilePopUp = 0;
	return;
}
function popup_createProfile_language::onAction()
{
	debugEcho("hello from the language popup" SPC %this SPC popup_createProfile_language.getTextById(popup_createProfile_language.getSelected()));
	return;
}
function nativeLanguageToEnglish(%languageName)
{
	%index = getFieldIndex($availableLanguagesNative, %languageName);
	return getField($availableLanguages, %index);
	return getField($availableLanguages, %index);
}
function englishToNativeLanguage(%languageName)
{
	%index = getFieldIndex($availableLanguages, %languageName);
	return getField($availableLanguagesNative, %index);
	return getField($availableLanguagesNative, %index);
}
function menu_main::onMenuLoadFinished(%this)
{
	if (isObject(goToSubMenu))
	{
		menu_main.enterMenu(goToSubMenu);
	}
	if (goToEpilog)
	{
		%this.goToEpilog = "0";
		tablbl_level_epilog.switchEnvironment("epilog");
	}
	unSubscribeFromEvents(%this, "onMenuLoadFinished");
	return;
}
function menu_main::saveGamePlayed(%this)
{
	echo("main menu loaded!");
	if ($WII)
	{
		return;
	}
	if ($settings::Status::GamePlayed)
	{
		if ($settings::Status::FullscreenNotify < $currentVersion && !($settings::Video::Fullscreen))
		{
			echo("notify fullscreen dialog shown" SPC $ACTUAL_MENU SPC "is actual menu");
			menu_dialog.Show($lbl_startRes, $lbl_startResText, "", $btn_OK, $lbl_newversion_checkbox, "fullscreennotify", "", "", "1", "1");
		}
	}
	else
	{
		$settings::Status::GamePlayed = 1;
		debugEcho("saving that the game was played at least once");
	}
	return;
}
