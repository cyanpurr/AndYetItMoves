// levelDetail.cs.dso
function tab_level_cave::onAdd(%this)
{
	%this.setVisible("0");
	return;
}
function tab_level_jungle::onAdd(%this)
{
	%this.setVisible("0");
	return;
}
function tab_level_trip::onAdd(%this)
{
	%this.setVisible("0");
	return;
}
function tab_level_epilog::onAdd(%this)
{
	%this.setVisible("0");
	return;
}
function LevelDetailButton::onAdd(%this)
{
	%levelId = getLastToken(%this.getName(), "_");
	%this.level = ['"level_"', '%levelId'];
	if (%levelId $= "cave" || %levelId $= "jungle" || %levelId $= "trip")
	{
		%this.wholeEnvironment = "1";
	}
	%extent = %this.getExtent();
	%this.playButton = new GuiBitmapButtonTextCtrl(Name : "")
	{
		canSaveDynamicFields = "1";
		class = "PlayButton";
		Profile = Profile;
		HorizSizing = "right";
		VertSizing = "bottom";
		Position = "0 0";
		Extent = %extent;
		Visible = "1";
		AlwaysUseMouseEvents = "1";
		bitmap = ['"~/"', '$DataFolder', '"/images/Menu/empty"'];
		UseMouseEvents = "1";
		level = level;
		Type = "play";
		wholeEnvironment = wholeEnvironment;
	}
	%this.addGuiControl(playButton);
	%this.levelIndex = getWordIndex($FULLLEVELLIST, level);
	if (%levelId != "credits")
	{
		%this.playmodeStamp = new GuiBitmapButtonCtrl(Name : "")
		{
			canSaveDynamicFields = "0";
			class = "Stamp";
			isContainer = "0";
			Profile = "AyimEmptyProfile";
			HorizSizing = "relative";
			VertSizing = "bottom";
			SizeMargin = "0 0";
			PositionAbsolute = "0";
			Position = "0 0";
			Extent = "56 56";
			Visible = "1";
			AlwaysUseMouseEvents = "0";
			UseMouseEvents = "1";
			hovertime = "1000";
			bitmap = ['"~/"', '$DataFolder', '"/images/Menu/stamps/survivalMode_gold.png"'];
			wrap = "0";
			ExtentsAbsolute = "1";
		}
		playmodeStamp.playButton = playButton;
	}
	return;
}
function menu_level::onWake(%this)
{
	btn_level_credits.setVisible(!(playmodeManager.isSpeedRunMode()) && !(playmodeManager.isMedalMode()));
	if ($demoVersion && !(isObject(lockedString)))
	{
		%this.lockedString = new GuiTextCtrl(Name : "")
		{
			isContainer = "0";
			Profile = "AyimMenuTextProfile";
			HorizSizing = "right";
			VertSizing = "bottom";
			SizeMargin = "0 0";
			PositionAbsolute = "0";
			Position = "150 512";
			Extent = "173 64";
			MinExtent = "8 2";
			canSave = "1";
			Visible = "0";
			AlwaysUseMouseEvents = "0";
			hovertime = "1000";
			text = $lbl_requires_full_version;
			maxLength = "1024";
		}
		%newPos = t2dVectorSub(%this.getExtent(), "580 90");
		lockedString.setPosition(getX(%newPos), getY(%newPos));
		%this.addGuiControl(lockedString);
	}
	return;
}
function LevelDetailButton::onWake(%this)
{
	if (isObject(playmodeStamp))
	{
		if (!(isAdded))
		{
			if ($WII)
			{
				%container = %this.getParent().getParent().getParent();
			}
			else
			{
				%container = %this;
			}
			%container.addGuiControl(playmodeStamp);
			playmodeStamp.isAdded = "1";
			if ($WII)
			{
				%pos = %this.getGlobalPosition();
				%posXadapt = 800.0 - getX(getRes()) / 11.88888931274414;
				%pos = t2dVectorSub(%pos, %posXadapt SPC "11");
				playmodeStamp.setPositionGlobal(getX(%pos), getY(%pos));
			}
		}
	}
	return;
}
function menu_level::onDialogPush(%this)
{
	if ($settings::Secrets::LevelsUnlocked == 0.0)
	{
		%this.back();
		return;
	}
	%tabButton = "tablbl_level_" @ $settings::Other::LastLevelTab;
	%tabButton.switchEnvironment($settings::Other::LastLevelTab);
	tablbl_level_epilog.setVisible("0");
	btn_level_cave.activateButton("0");
	btn_level_jungle.activateButton("0");
	btn_level_trip.activateButton("0");
	btn_level_cave.setVisible(playmodeManager.isSpeedRunMode());
	btn_level_jungle.setVisible(playmodeManager.isSpeedRunMode());
	btn_level_trip.setVisible(playmodeManager.isSpeedRunMode());
	if (!($demoVersion) && !($previewVersion) && playmodeManager.isSpeedRunMode())
	{
		if ($settings::Secrets::LevelsUnlocked >= 4.0)
		{
			btn_level_cave.unlocked = "1";
			btn_level_cave.activateButton("1");
		}
		if ($settings::Secrets::LevelsUnlocked >= 10.0)
		{
			btn_level_jungle.unlocked = "1";
			btn_level_jungle.activateButton("1");
		}
		if ($settings::Secrets::LevelsUnlocked >= 16.0)
		{
			btn_level_trip.unlocked = "1";
			btn_level_trip.activateButton("1");
		}
	}
	if ($settings::Secrets::LevelsUnlocked >= 16.0)
	{
		tablbl_level_epilog.setVisible("1");
	}
	%i = 1;
	while (%i < getWordCount($FULLLEVELLIST))
	{
		%button = "btn_" @ getWord($FULLLEVELLIST, %i);
		if (isObject(%button))
		{
			%button.activateButton("0");
		}
		%i = %i + 1.0;
	}
	%i = 1;
	while (%i < getWordCount($LEVELLIST))
	{
		%button = "btn_" @ getWord($LEVELLIST, %i);
		if (!(isObject(%button)))
		{
		}
		else
		{
			if (%i < $settings::Secrets::LevelsUnlocked + 1.0)
			{
				%button.unlocked = "1";
				%button.activateButton("1");
				break;
			}
			if (%i == $settings::Secrets::LevelsUnlocked + 1.0)
			{
				if (!(playmodeManager.isSpeedRunMode()) && !(playmodeManager.isMedalMode()))
				{
					%button.activateButton("1");
				}
				break;
			}
			%button.unlocked = "0";
			%button.activateButton("0");
		}
		%i = %i + 1.0;
	}
	return getWordCount($LEVELLIST);
}
function menu_level::onTabSelected(%this, %tab)
{
	if ($demoVersion && %tab $= "epilog")
	{
		%tab = "cave";
	}
	tab_level_cave.setVisible("0");
	tab_level_jungle.setVisible("0");
	tab_level_trip.setVisible("0");
	tab_level_epilog.setVisible("0");
	$settings::Other::LastLevelTab = %tab;
	%newTab = "tab_level_" @ %tab;
	%newTab.setVisible("1");
	return;
}
function LevelDetailButton::activateButton(%this, %active)
{
	if (%active)
	{
		if (originalBitmap != "")
		{
			%this.setBitmap(originalBitmap);
		}
		%this.updatePlaymodeButtons();
	}
	else
	{
		%this.setBitmap("game/" @ $DataFolder @ "/images/Menu/levels/inactive");
	}
	%this.setActive(%active);
	if (isObject(playButton))
	{
		playButton.setActive(%active);
	}
	if (isObject(playmodeStamp))
	{
		playmodeStamp.setVisible("0");
	}
	if (isObject(playmodeStamp) && %active && playmodeManager.isMedalMode())
	{
		%i = levelIndex;
		%mode = playmodeManager.getMode();
		%difficulty = getWord($settings::Playmodes::Stamps[%mode], %i);
		if (%difficulty < 2.0)
		{
			%worthName = getWord("gold silver bronze", %difficulty + 1.0);
			%bitmap = "game/" @ $DataFolder @ "/images/Menu/stamps/" @ %mode @ "_" @ %worthName @ ".png";
			playmodeStamp.setBitmap(%bitmap);
			playmodeStamp.setVisible("1");
		}
	}
	return;
}
function LevelDetailButton::updatePlaymodeButtons(%this)
{
	if (playmodeManager.isSpeedRunMode())
	{
		playButton.label = $lbl_compete;
	}
	else
	{
		playButton.label = $lbl_play;
	}
	%extent = %this.getExtent();
	playButton.setPosition("0", "0");
	playButton.setExtent(getX(%extent), getY(%extent));
	playButton.setBitmap("game/" @ $DataFolder @ "/images/Menu/levels/wholeOverlay");
	playButton.setModeButtonVisible("0");
	return;
}
function Stamp::onMouseEnter(%this)
{
	playButton.onMouseEnter();
	return;
}
function Stamp::onMouseLeave(%this)
{
	playButton.onMouseLeave();
	return;
}
function Stamp::onAction(%this)
{
	playButton.onAction();
	return;
}
function playButton::onMouseEnter(%this)
{
	%parent = %this.getParent();
	if (%parent.isActive())
	{
		playButton.setModeButtonVisible("1", "1");
	}
	else
	{
		if ($demoVersion)
		{
			lockedString.setVisible("1");
		}
	}
	return;
}
function playButton::onMouseLeave(%this)
{
	%parent = %this.getParent();
	%parent.hidePlaymodeButtons();
	lockedString.setVisible("0");
	return;
}
function LevelDetailButton::hidePlaymodeButtons(%this)
{
	playButton.setModeButtonVisible("0");
	%this.text = levelName;
	return;
}
function playButton::setModeButtonVisible(%this, %visible, )
{
	if (%visible)
	{
		%this.getParent().text = "";
		%this.text = label;
		%this.setVisible("1");
	}
	else
	{
		%this.text = "";
		%this.setVisible("0");
	}
	return;
}
function playButton::onAction(%this)
{
	if (isObject(level))
	{
		if (playmodeManager.isSpeedRunMode())
		{
			subscribeToEvents(menu_levelDetail, "OnRowSelected");
			menu_levelDetail.currentLevel = level;
			menu_levelDetail.wholeEnvironment = wholeEnvironment;
			$ACTUAL_MENU.enterMenu(menu_levelDetail);
		}
		else
		{
			Webclient.setPermanentFilter("level", levelID);
			SpeedRunMode.active = "0";
			Statistics.wholeEnvironment = "0";
			$watchAsReplay = 0;
			MenuAction::loadLevel(level);
		}
	}
	else
	{
		debugWarn("not a valid level for detail view:" SPC level);
	}
	return;
}
function LevelDetailButton::onAction(%this)
{
	if (isObject(level))
	{
		menu_levelDetail.currentLevel = level;
		menu_levelDetail.wholeEnvironment = wholeEnvironment;
		$ACTUAL_MENU.enterMenu(menu_levelDetail);
	}
	else
	{
		debugWarn("not a valid level for detail view:" SPC level);
	}
	return;
}
function menu_levelDetail::onDialogPush(%this)
{
	subscribeToEvents(menu_levelDetail, "OnRowSelected");
	lbl_levelDetail_name.text = levelName;
	%levelIndex = getWordIndex($levelDetailList, getLastToken(currentLevel, "_"));
	if (%levelIndex >= getWordCount($levelDetailList))
	{
		btn_levelDetail_nextLevel.setVisible("0");
		%this.previousLevelButton = ['"btn_level_"', <torque.FuncCall object at 0x0000021716CC1EB0>];
		btn_levelDetail_previousLevel.setVisible(unlocked);
	}
	else
	{
		if (%levelIndex == 0.0)
		{
			btn_levelDetail_previousLevel.setVisible("0");
			%this.nextLevelButton = ['"btn_level_"', <torque.FuncCall object at 0x0000021716CC2060>];
			btn_levelDetail_nextLevel.setVisible(unlocked);
			break;
		}
		%this.previousLevelButton = ['"btn_level_"', <torque.FuncCall object at 0x0000021716CC2240>];
		btn_levelDetail_previousLevel.setVisible(unlocked);
		%this.nextLevelButton = ['"btn_level_"', <torque.FuncCall object at 0x0000021716CC2210>];
		btn_levelDetail_nextLevel.setVisible(unlocked);
	}
	chb_levelDetail_filterGhost.setStateOn($settings::Other::filterGhost);
	Webclient.setPermanentFilter("ghost", $settings::Other::filterGhost);
	if ($WII)
	{
		debugEcho("this section is outdated");
	}
	else
	{
		if ($distributorName $= "BigFishGames")
		{
			btn_levelDetail_local.text = text;
			btn_levelDetail_competitors.setVisible("0");
			btn_levelDetail_best.setVisible("0");
			btn_levelDetail_location.setVisible("0");
			btn_levelDetail_user.setVisible("0");
			chb_levelDetail_filterGhost.setVisible("0");
			btn_levelDetail_timeFrame.setVisible("0");
			btn_levelDetail_previous.setVisible("0");
			btn_levelDetail_next.setVisible("0");
			break;
		}
		%onlineButtonActive = Webclient.isOnline();
		btn_levelDetail_competitors.setActive(%onlineButtonActive);
		btn_levelDetail_best.setActive(%onlineButtonActive);
		btn_levelDetail_location.setActive(%onlineButtonActive);
		btn_levelDetail_user.setActive(%onlineButtonActive);
	}
	%this.internalTimeFrames = "all month week";
	%this.timeFrameCount = getWordCount(internalTimeFrames);
	btn_levelDetail_timeFrame.text = getField($lbl_timeFrames, $settings::Statistics::TimeFrame);
	Webclient.setPermanentFilter("timeFrame", getWord(internalTimeFrames, $settings::Statistics::TimeFrame));
	btn_levelDetail_next.text = $lbl_next SPC $settings::Standards::scoresQuantity;
	btn_levelDetail_previous.text = $lbl_previous SPC $settings::Standards::scoresQuantity;
	%this.scorePageIndex = "0";
	%this.saveSettings = "0";
	Webclient.setPermanentFilter("level", levelID);
	if (mode $= "")
	{
	}
	else
	{
	}
	%mode = mode;
	%this.refreshList(%mode);
	return;
}
function menu_levelDetail::onDialogPop(%this)
{
	unSubscribeFromEvents(menu_levelDetail, "OnRowSelected");
	return;
}
function menu_levelDetail::onPostDialogPush(%this)
{
	if (mode $= "")
	{
	}
	else
	{
	}
	%mode = mode;
	if (mode $= "custom")
	{
		%this.refreshList(%mode);
	}
	else
	{
		%btn = "btn_levelDetail_" @ mode;
		%btn.refreshList(mode);
	}
	return;
}
function menu_levelDetail_lastElement::onWake(%this)
{
	if (mode $= "")
	{
	}
	else
	{
	}
	%mode = mode;
	menu_levelDetail.refreshList(%mode);
	return;
}
function chb_levelDetail_filterGhost::onClick(%this)
{
	$settings::Other::filterGhost = chb_levelDetail_filterGhost.getValue();
	Webclient.setPermanentFilter("ghost", $settings::Other::filterGhost);
	menu_levelDetail.saveSettings = "1";
	menu_levelDetail.scorePageIndex = "0";
	menu_levelDetail.refreshList();
	return;
}
function menu_levelDetail::changeLevel(%this, %next)
{
	$ACTUAL_MENU.back();
	if (%next)
	{
		nextLevelButton.onAction();
	}
	else
	{
		previousLevelButton.onAction();
	}
	return;
}
function menu_levelDetail::toggleTimeFrame(%this)
{
	$settings::Statistics::TimeFrame = $settings::Statistics::TimeFrame + 1.0 % timeFrameCount;
	btn_levelDetail_timeFrame.text = getField($lbl_timeFrames, $settings::Statistics::TimeFrame);
	Webclient.setPermanentFilter("timeFrame", getWord(internalTimeFrames, $settings::Statistics::TimeFrame));
	%this.saveSettings = "1";
	%this.refreshList();
	return;
}
function menu_levelDetail::search(%this)
{
	if (mode $= "user" && !(defaultText) && mode $= "location" && !(defaultText))
	{
		%this.deactivateAllTabs();
	}
	%this.refreshList("custom");
	return;
}
function menu_levelDetail::refreshList(%this, %mode)
{
	if (%mode $= "local")
	{
		btn_levelDetail_timeFrame.setActive("0");
		chb_levelDetail_filterGhost.setActive("0");
	}
	else
	{
		btn_levelDetail_timeFrame.setActive("1");
		chb_levelDetail_filterGhost.setActive("1");
	}
	if (%mode $= "competitors")
	{
		highscoreList.setVisible("0");
		competitorsList.setVisible("1");
		%this.list = competitorsList;
	}
	else
	{
		highscoreList.setVisible("1");
		competitorsList.setVisible("0");
		%this.list = highscoreList;
	}
	btn_levelDetail_competeWoGhost.setActive("0");
	btn_levelDetail_competeWoGhost.text = $lbl_selectARow;
	if ($WII)
	{
		btn_levelDetail_competeWoGhost.Command = "menu_levelDetail.showResetNotification(false);";
	}
	else
	{
		btn_levelDetail_compete.setActive("0");
		btn_levelDetail_compete.text = $lbl_selectARow;
		btn_levelDetail_compete.Command = "menu_levelDetail.showResetNotification(true);";
		btn_levelDetail_watch.setActive("0");
		btn_levelDetail_watch.text = $lbl_selectARow;
	}
	lbl_levelDetail_noscores.setVisible("0");
	list.clearList();
	if (%mode $= "")
	{
		%mode = mode;
	}
	else
	{
		%this.scorePageIndex = "0";
	}
	debugEcho("refreshing list. mode is" SPC %mode SPC "level" SPC currentLevel SPC "list size:" SPC list.getExtent());
	%standardRange = "0" SPC $settings::Standards::scoresQuantity;
	%this.mode = %mode;
	Statistics.selectedHighscore = "0";
	if (%mode $= "local")
	{
		%this.setSearchFields("0");
		%highscores = Statistics.getLocalHighscores(getLastToken(currentLevel, "_"));
		%this.maxPageCount = "1";
	}
	else
	{
		if (%mode $= "competitors")
		{
			%this.setSearchFields("1");
			%bestTime = Statistics.getUserBestScore(getLastToken(currentLevel, "_"));
			%highscores = %this.reformatCompetitors(Webclient.getCompetitors(%bestTime), %bestTime);
			%this.maxPageCount = "1";
			break;
		}
		if (%mode $= "best")
		{
			%this.setSearchFields("1");
			%highscores = Webclient.getScores("", "", scorePageIndex);
			%this.maxPageCount = Webclient.getPageCount();
			break;
		}
		if (%mode $= "location")
		{
			%this.setSearchFields("1", "", $settings::Personal::Location);
			%highscores = Webclient.getScores("", $settings::Personal::Location, scorePageIndex);
			%this.maxPageCount = Webclient.getPageCount();
			break;
		}
		if (%mode $= "user")
		{
			%this.setSearchFields("1", getUserName($settings::Profile::currentProfile));
			%highscores = Webclient.getScores(getUserName($settings::Profile::currentProfile), "", scorePageIndex);
			%this.maxPageCount = Webclient.getPageCount();
			break;
		}
		if (%mode $= "custom")
		{
			if (defaultText)
			{
				%user = "";
			}
			else
			{
				%user = trim(txt_levelDetail_user.getText());
				txt_levelDetail_user.setText(%user);
			}
			if (defaultText)
			{
				%location = "";
			}
			else
			{
				%location = trim(txt_levelDetail_location.getText());
				txt_levelDetail_location.setText(%location);
			}
			%highscores = Webclient.getScores(%user, %location, scorePageIndex);
			%this.maxPageCount = Webclient.getPageCount();
			chb_levelDetail_filterGhost.setActive("1");
			break;
		}
		%this.setSearchFields("0");
		%highscores = Statistics.getLocalHighscores(getLastToken(currentLevel, "_"));
	}
	list.setColorCodeBy("$lbl_levelDetail_time");
	%pageSwitchActive = maxPageCount > 1.0;
	btn_levelDetail_previous.setActive(%pageSwitchActive && scorePageIndex > 0.0);
	btn_levelDetail_next.setActive(%pageSwitchActive && scorePageIndex < maxPageCount - 1.0);
	%noScores = %mode $= "competitors";
	if (getRecordCount(%highscores) == 0.0 || getRecordCount(%highscores) <= 1.0 && $WII)
	{
		%timeIndex = 2;
	}
	else
	{
		%timeIndex = 3;
	}
	%formatTimeDiff = 0;
	if (!($WII) && %mode != "local")
	{
		%formatTimeDiff = %mode $= "competitors";
		%i = 0;
		if (%i < getRecordCount(%highscores))
		{
			%record = getRecord(%highscores, %i);
			%formatTimeDiffNow = myCompetitorPosition != -1.0;
			%readableTime = Statistics.getReadableTime(getField(%record, %timeIndex), "0", %formatTimeDiffNow);
			%record = setField(%record, "3", %readableTime);
			%highscores = setRecord(%highscores, %i, %record);
			%i = %i + 1.0;
		}
	}
	%i = 0;
	while (%i < getRecordCount(%highscores))
	{
		if (%formatTimeDiff)
		{
			%time = getField(getRecord(%highscores, %i), %timeIndex);
			if (strpos(%time, "-") >= 0.0)
			{
				%altTextProfile = "AyimListItemRightGreenProfile";
			}
			else
			{
				if (strpos(%time, "+") >= 0.0)
				{
					%altTextProfile = "AyimListItemRightRedProfile";
					break;
				}
				%altTextProfile = "";
			}
		}
		else
		{
			%altTextProfile = "";
		}
		list.addRow(getRecord(%highscores, %i), "", %timeIndex, %altTextProfile);
		%i = %i + 1.0;
	}
	if (%mode $= "competitors" && myCompetitorPosition != -1.0)
	{
		list.setRowNotSelectable(myCompetitorPosition);
		list.scrollToIndex(myCompetitorPosition);
	}
	if (%noScores)
	{
		lbl_levelDetail_noscores.setVisible("1");
		if ($WII)
		{
			btn_levelDetail_competeWoGhost.setActive("1");
			btn_levelDetail_competeWoGhost.text = $lbl_play;
			btn_levelDetail_competeWoGhost.Command = "menu_levelDetail.play();";
			break;
		}
		btn_levelDetail_competeWoGhost.setActive("0");
		btn_levelDetail_competeWoGhost.text = $btn_levelDetail_noscores;
		btn_levelDetail_watch.setActive("0");
		btn_levelDetail_watch.text = $btn_levelDetail_noscores;
		btn_levelDetail_compete.setActive("1");
		btn_levelDetail_compete.text = $lbl_play;
		btn_levelDetail_compete.Command = "menu_levelDetail.play();";
	}
	playmodeManager.competeWithoutTimes = %noScores;
	%this.selectedGhost = -1.0;
	return;
}
function menu_levelDetail::deactivateAllTabs(%this)
{
	btn_levelDetail_local.deactivate();
	btn_levelDetail_competitors.deactivate();
	btn_levelDetail_best.deactivate();
	btn_levelDetail_location.deactivate();
	btn_levelDetail_user.deactivate();
	return;
}
function menu_levelDetail::reformatCompetitors(%this, %scoreList, %bestTime)
{
	%formatedScores = "";
	%this.myCompetitorPosition = "0";
	%timeIsFaster = 1;
	%i = 0;
	while (%i < getRecordCount(%scoreList) - 1.0)
	{
		%score = getRecord(%scoreList, %i);
		%nextScore = getRecord(%scoreList, %i + 1.0);
		%actRank = getField(%score, "0");
		%nextRank = getField(%nextScore, "0");
		%ranksBetween = %nextRank - %actRank;
		if (%ranksBetween == 1.0)
		{
			%rankSpan = %actRank;
		}
		else
		{
			%rankSpan = %actRank SPC "-" SPC %nextRank - 1.0;
		}
		%player = getField(%score, "1") SPC "from" SPC getField(%score, "2");
		if (%bestTime != -1.0)
		{
			%time = getField(%score, "3") - %bestTime;
			if (%time > 0.0 && %timeIsFaster)
			{
				%timeIsFaster = 0;
				%this.myCompetitorPosition = %i;
			}
		}
		else
		{
			%time = getField(%score, "3");
		}
		%reformated = %rankSpan TAB %ranksBetween TAB %player TAB %time TAB getFields(%score, "4", getFieldCount(%score) - 1.0);
		%formatedScores = %formatedScores NL %reformated;
		%i = %i + 1.0;
	}
	if (%bestTime != -1.0)
	{
		%formatedScores = trim(%formatedScores);
		%myScoreLine = "Your score:" TAB "" TAB $settings::Personal::Name TAB %bestTime TAB "" TAB "-1";
		%formatedScores = getRecords(%formatedScores, "0", myCompetitorPosition - 1.0) NL %myScoreLine NL getRecords(%formatedScores, myCompetitorPosition, getRecordCount(%formatedScores) - 1.0);
	}
	else
	{
		%this.myCompetitorPosition = -1.0;
	}
	return trim(%formatedScores);
	return trim(%formatedScores);
}
function SearchField::onFocusChange(%this, %isFocus)
{
	if (%isFocus && defaultText)
	{
		%this.text = "";
		%this.setProfile("AyimTextEditProfile");
	}
	else
	{
		if (!(%isFocus))
		{
			if (%this.getText() $= "")
			{
				%this.setDefault();
				break;
			}
			%this.defaultText = "0";
		}
	}
	return;
}
function SearchField::setDefault(%this)
{
	if (%this.getName() $= "txt_levelDetail_user")
	{
		txt_levelDetail_user.text = $lbl_username;
		txt_levelDetail_user.defaultText = "1";
		txt_levelDetail_user.setProfile("AyimTextEditDefaultValueProfile");
	}
	else
	{
		if (%this.getName() $= "txt_levelDetail_location")
		{
			txt_levelDetail_location.text = $lbl_location;
			txt_levelDetail_location.defaultText = "1";
			txt_levelDetail_location.setProfile("AyimTextEditDefaultValueProfile");
		}
	}
	return;
}
function menu_levelDetail::setSearchFields(%this, %active, %user, %location)
{
	txt_levelDetail_user.defaultText = "0";
	txt_levelDetail_user.setProfile("AyimTextEditProfile");
	txt_levelDetail_location.defaultText = "0";
	txt_levelDetail_location.setProfile("AyimTextEditProfile");
	if (%user $= "")
	{
		%user = $lbl_username;
		txt_levelDetail_user.setDefault();
	}
	if (%location $= "")
	{
		%location = $lbl_location;
		txt_levelDetail_location.setDefault();
	}
	txt_levelDetail_user.text = %user;
	txt_levelDetail_location.text = %location;
	if (%active)
	{
		txt_levelDetail_user.setActive("1");
		txt_levelDetail_user.setVisible("1");
		txt_levelDetail_location.setActive("1");
		txt_levelDetail_location.setVisible("1");
	}
	else
	{
		txt_levelDetail_user.setActive("0");
		txt_levelDetail_user.setVisible("0");
		txt_levelDetail_location.setActive("0");
		txt_levelDetail_location.setVisible("0");
	}
	return;
}
function menu_levelDetail::OnRowSelected(%this)
{
	if (lastSelectionTime > 0.0 && lastRowSelected == list.getSelectedRow() && getRealTime() - lastSelectionTime < 400.0)
	{
		%this.showResetNotification("1");
		return;
	}
	%this.lastRowSelected = list.getSelectedRow();
	%this.lastSelectionTime = getRealTime();
	if (!(btn_levelDetail_competeWoGhost.isActive()))
	{
		if ($WII)
		{
			btn_levelDetail_competeWoGhost.text = $lbl_compete;
		}
		else
		{
			btn_levelDetail_competeWoGhost.text = $btn_levelDetail_competeWoGhost;
		}
		btn_levelDetail_competeWoGhost.setActive("1");
	}
	%this.rowData = %this.getDetailData();
	debugEcho("onRowSelected from levelDetail" SPC %this SPC "rowData:" SPC rowData);
	lbl_levelDetail_deaths.text = Statistics.getDeaths(rowData);
	lbl_levelDetail_date.text = Statistics.getDate(rowData);
	%this.selectedGhost = Statistics.getGhost(rowData);
	if (!($WII))
	{
		if (selectedGhost != -1.0)
		{
			btn_levelDetail_compete.text = $btn_levelDetail_compete;
			btn_levelDetail_compete.setActive("1");
			btn_levelDetail_watch.text = $btn_levelDetail_watch;
			btn_levelDetail_watch.setActive("1");
			break;
		}
		btn_levelDetail_compete.text = $btn_levelDetail_ghostNA;
		btn_levelDetail_compete.setActive("0");
		btn_levelDetail_watch.text = $btn_levelDetail_ghostNA;
		btn_levelDetail_watch.setActive("0");
		if (mode != "local")
		{
			%this.selectedGhost = "ghost";
		}
	}
	%this.oldSpawnPointTimes = Statistics.getSpawnpointTimes(rowData);
	return;
}
function menu_levelDetail::getDetailData(%this)
{
	if (mode $= "local")
	{
		%record = getRecord(Statistics.getSavedTimes(getLastToken(currentLevel, "_"), "1"), selectedRow);
		Statistics.oldRank = list.getSelectedRow() + 1.0;
		return %record;
	}
	else
	{
		%data = Webclient.getDetailData(selectedRow);
		Statistics.oldRank = Statistics.getRank(%data);
		return %data;
	}
	return %data;
}
function menu_levelDetail::previousPage(%this)
{
	%this.scorePageIndex = scorePageIndex - 1.0;
	%this.refreshList();
	return;
}
function menu_levelDetail::nextPage(%this)
{
	%this.scorePageIndex = scorePageIndex + 1.0;
	%this.refreshList();
	return;
}
function menu_levelDetail::onEnterPressed(%this)
{
	btn_levelDetail_compete.setFirstResponder();
	%this.search();
	return;
}
function SearchField::onEscapePressed(%this)
{
	debugEcho("escapepressed in searchfield" SPC %this);
	menu_levelDetail.onEscapePressed();
	return;
}
function menu_levelDetail::onEscapePressed(%this)
{
	%this.back();
	return;
}
function menu_levelDetail::showResetNotification(%this, %ghost)
{
	$notificationCount = $notificationCount % $notifications;
	if (!($WII) && !($settings::Controls::dontShowResetNotifications))
	{
		lbl_resetNotification_question_1.text = getRecord($lbl_notifications_text[$notificationCount], "0");
		lbl_resetNotification_question_2.text = getRecord($lbl_notifications_text[$notificationCount], "1");
		if ($notificationCount == 0.0)
		{
			lbl_resetNotification_question_2.text = text SPC """ SPC getWord($settings::Controls::KeyboardLayout, getWordCount($settings::Controls::KeyboardLayout) - 2.0) SPC """;
		}
		chb_resetNotification_dontShowAgain.setValue("0");
		btn_resetNotification_ok.text = $btn_OK;
		if (%ghost)
		{
			btn_resetNotification_ok.Command = "menu_levelDetail.compete(true);";
		}
		else
		{
			btn_resetNotification_ok.Command = "menu_levelDetail.compete(false);";
		}
		$ACTUAL_MENU.enterMenu(menu_resetNotification, "1");
		$notificationCount = $notificationCount + 1.0;
	}
	else
	{
		%this.compete(%ghost);
	}
	return;
}
function menu_levelDetail::play(%this)
{
	debugEcho("playing level" SPC currentLevel SPC "wE" SPC wholeEnvironment SPC "mLD.wE" SPC wholeEnvironment);
	if (wholeEnvironment)
	{
		debugEcho("setting up statistics for wE" SPC %this);
		Statistics.wholeEnvironment = "1";
		Statistics.environmentGhosts = -1.0;
		Statistics.environment = getLastToken(currentLevel, "_");
	}
	MenuAction::loadLevel(currentLevel);
	return;
}
function menu_levelDetail::compete(%this, %loadGhost, %watchAsReplay)
{
	$watchAsReplay = %watchAsReplay;
	if ($ACTUAL_MENU.getId() == menu_resetNotification.getId())
	{
		menu_resetNotification.back();
		if (chb_resetNotification_dontShowAgain.getValue() != $settings::Controls::dontShowResetNotifications)
		{
			$settings::Controls::dontShowResetNotifications = chb_resetNotification_dontShowAgain.getValue();
			%this.saveSettings = "1";
		}
	}
	if (!($WII) && saveSettings)
	{
		saveSettings($settings::Profile::currentProfile);
	}
	Statistics.reset();
	Statistics.oldData = rowData;
	debugEcho("setup speedrun. oldData:" SPC oldData);
	if (mode $= "local")
	{
	}
	else
	{
	}
	%ghost = list.getSelectedRow();
	SpeedRunMode.setupSpeedRun(currentLevel, wholeEnvironment, %ghost, %loadGhost, mode $= "local");
	playmodeManager.setMode("SpeedRun");
	MenuAction::loadLevel(currentLevel);
	return;
}
function menu_levelDetail::watch(%this)
{
	debugEcho("we should be watching the ghost now" SPC %this);
	%this.compete("1", "1");
	return;
}
