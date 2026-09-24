// switchProfiles.cs.dso
function menu_switchProfiles::onDialogPush(%this)
{
	if ($SnowdriftlandSpecial || $WII && $demoVersion)
	{
		MenuAction::createProfile("1");
		return;
	}
	SwitchProfiles::refresh();
	list.setRowSelected(getWordIndex($settings::Profile::allProfiles, $settings::Profile::currentProfile));
	%this.OnRowSelected();
	subscribeToEvents(menu_switchProfiles, "OnRowSelected");
	return;
}
function menu_switchProfiles::onDialogPop(%this)
{
	if ($WII && save)
	{
		menu_switchProfiles.save = "0";
		saveSettings(-1.0, "1");
	}
	unSubscribeFromEvents(menu_switchProfiles, "OnRowSelected");
	return;
}
function menu_switchProfiles::onEnterPressed(%this)
{
	if (!(%WII))
	{
		return;
	}
	debugEcho("enter in menu_switchProfiles" SPC SwitchProfiles::getListData());
	if (SwitchProfiles::getListData() > -1.0)
	{
		SwitchProfiles::switchProfile();
	}
	return;
}
function menu_switchProfiles::onEscapePressed(%this)
{
	if (getWordCount($settings::Profile::allProfiles) > 0.0)
	{
		%this.back();
	}
	return;
}
function menu_switchProfiles::OnRowSelected(%this)
{
	if (lastSelectionTime > 0.0 && lastRowSelected == list.getSelectedRow() && getRealTime() - lastSelectionTime < 400.0)
	{
		debugEcho("double click on" SPC list.getSelectedRow() SPC "timeDiff" SPC getRealTime() - lastSelectionTime);
		SwitchProfiles::switchProfile();
		return;
	}
	%this.lastRowSelected = list.getSelectedRow();
	%this.lastSelectionTime = getRealTime();
	if (SwitchProfiles::getSelectedId() == $settings::Profile::currentProfile)
	{
		btn_switchProfiles_switch.setActive("0");
		btn_switchProfiles_edit.setActive("1");
		btn_switchProfiles_edit.text = $lbl_switchProfiles_edit;
	}
	else
	{
		btn_switchProfiles_switch.setActive("1");
		btn_switchProfiles_edit.setActive("0");
		if ($settings::Personal::Language $= "english")
		{
			btn_switchProfiles_edit.text = $lbl_switchProfiles_switchToEdit;
		}
	}
	return;
}
function SwitchProfiles::getListData(%id)
{
	loadSettings(%id, "1");
	%totalTime = $settings::Secrets::TotalPlayTime;
	%totalDeaths = $settings::Secrets::TotalDeathCount;
	if (%id == $settings::Profile::currentProfile)
	{
		%totalTime = %totalTime + Statistics.getLevelTime();
		%totalDeaths = %totalDeaths + playerDeathCount;
		debugEcho("this is currnet:" SPC %id SPC "adding" SPC Statistics.getLevelTime() SPC "and" SPC playerDeathCount);
	}
	%achieved = getWordCount($settings::Secrets::Achieved) / getFieldCount(internalNames);
	%levelsUnlocked = t2dGetMin($settings::Secrets::LevelsUnlocked, "17") + getWordCount($settings::Secrets::BonusLevelsFinished) / 21.0;
	%medalsUnlocked = 0;
	%modes = "RallyMode LimitedRotationMode SurvivalMode";
	%i = 0;
	while (%i < getWordCount(%modes))
	{
		%mode = getWord(%modes, %i);
		%j = 1;
		while (%j < getWordCount($settings::Playmodes::Stamps[%mode]))
		{
			if (%j == 17.0)
			{
			}
			else
			{
				%stampValue = 2.0 - getWord($settings::Playmodes::Stamps[%mode], %j);
				%medalsUnlocked = %medalsUnlocked + %stampValue;
			}
			%j = %j + 1.0;
		}
		%i = %i + 1.0;
	}
	%medalsPercent = %medalsUnlocked / getWordCount(%modes) * getWordCount($settings::Playmodes::StampsRallyMode) - 2.0 * 3.0;
	%progress = mFloor(%levelsUnlocked * 0.6600000262260437 + %achieved * 0.17000000178813934 + %medalsPercent * 0.17000000178813934 * 100.0 + 0.0010000000474974513);
	debugEcho("wii: achieved" SPC %achieved SPC "lvlunlocked" SPC %levelsUnlocked SPC "medals" SPC %medalsPercent SPC "=" SPC %progress);
	%stats = Statistics.getReadableTime(%totalTime, "1") TAB %totalDeaths TAB %progress TAB "%";
	if ($WII)
	{
		%profileData = $settings::Personal::Name TAB %stats;
	}
	else
	{
		%profileData = $settings::Personal::Name TAB $settings::Personal::Location TAB %stats;
	}
	return %profileData;
	return %profileData;
}
function SwitchProfiles::Edit()
{
	%profileId = SwitchProfiles::getSelectedId();
	if (%profileId == -1.0)
	{
		return;
	}
	debugEcho("this profile has been selected for edit" SPC %profileId SPC "name" SPC getUserName(%profileId) SPC "cP" SPC $settings::Profile::currentProfile);
	if (%profileId != $settings::Profile::currentProfile)
	{
		return;
	}
	chb_createProfile_showHints.setVisible("1");
	chb_createProfile_showHints.setValue($settings::Other::ShowHints);
	menu_createProfile.editProfile = "1";
	$ACTUAL_MENU.enterMenu(menu_createProfile);
	txt_createProfile_name.text = getUserName(%profileId);
	menu_createProfile.oldName = text;
	txt_createProfile_location.text = $settings::Personal::Location;
	popup_createProfile_language.setSelected(popup_createProfile_language.findText(englishToNativeLanguage($settings::Personal::Language)));
	debugEcho("read this native language out fo the settings:" SPC englishToNativeLanguage($settings::Personal::Language));
	btn_createProfile_qt.text = $btn_back;
	btn_createProfile_qt.Command = "SwitchProfiles::cancelEdit();";
	btn_createProfile_create.text = $btn_save;
	btn_createProfile_create.Command = "MenuAction::editProfile();";
	return;
}
function SwitchProfiles::cancelEdit()
{
	$ACTUAL_MENU.back();
	return;
}
function SwitchProfiles::deleteProfile(%profileId)
{
	if (!($WII) && getWordCount($settings::Profile::allProfiles) == 1.0)
	{
		lbl_switchProfiles_alert.text = $lbl_oneActiveProfile;
		return -1.0;
	}
	if (%profileId $= "")
	{
		%profileId = SwitchProfiles::getSelectedId();
	}
	%selectedId = %profileId;
	$idToDelete = %selectedId;
	if (isExternalUser(%selectedId))
	{
		lbl_switchProfiles_alert.text = $lbl_cantDeleteExternalProfile;
		return -1.0;
	}
	debugEcho("deleting this profile" SPC %selectedId);
	if (%selectedId == -1.0)
	{
		return;
	}
	if ($settings::Personal::Language $= "japanese")
	{
		%title = $lbl_deleteProfile;
		%question = $lbl_deleteProfileQuestion1 SPC $lbl_deleteProfileQuestion2 SPC getUserName(%selectedId) SPC ".";
	}
	else
	{
		%title = $lbl_deleteProfile SPC getUserName(%selectedId);
		%question = $lbl_deleteProfileQuestion1 SPC %selectedId SPC $lbl_deleteProfileQuestion2 SPC getUserName(%selectedId) SPC ".";
	}
	menu_dialog.Show(%title, %question NL $lbl_deleteProfileQuestion3, $btn_deleteProfileCancel, $btn_deleteProfileOk, "", "", "", "SwitchProfiles::reallyDelete();");
	return;
}
function SwitchProfiles::reallyDelete()
{
	if ($idToDelete $= "")
	{
		%selectedId = SwitchProfiles::getSelectedId();
	}
	else
	{
		%selectedId = $idToDelete;
	}
	$idToDelete = "";
	%oldAllProfile = $settings::Profile::allProfiles;
	%i = 0;
	while (%i < getWordCount($settings::Profile::allProfiles))
	{
		if (getWord($settings::Profile::allProfiles, %i) == %selectedId)
		{
			$settings::Profile::allProfiles = removeWord($settings::Profile::allProfiles, %i);
		}
		%i = %i + 1.0;
	}
	if (%oldAllProfile $= $settings::Profile::allProfiles)
	{
		debugEcho("WARNING:SwitchProfiles::reallyDelete tried to delete a profile that was not in our general settings. CAN'T BE. returning" SPC %this);
		$ACTUAL_MENU.back();
		return;
	}
	if (!($WII))
	{
		fileDelete(getSettingFilePath(%selectedId, "0"));
		fileDelete(getSettingFilePath(%selectedId, "1"));
	}
	else
	{
		writeFile(getSettingFilePath(%selectedId), "empty", "0", "1");
	}
	if ($WII)
	{
		%statsPath = "";
	}
	else
	{
		%statsPath = "stats/";
	}
	%i = 0;
	while (%i < getWordCount($LEVELLIST))
	{
		%levelName = getLastToken(getWord($LEVELLIST, %i), "_");
		if (%levelName $= "")
		{
		}
		else
		{
			%bestTimes = Statistics.getSavedTimes(%levelName);
			if (%bestTimes $= "")
			{
				break;
			}
			%cleanedBestTimes = "";
			%j = 0;
			while (%j < getRecordCount(%bestTimes))
			{
				%line = getRecord(%bestTimes, %j);
				%key = firstWord(%line);
				if (%key $= "userID")
				{
					%value = getWord(%line, "1");
					if (%value == %selectedId)
					{
						if (!($WII))
						{
							%k = 0;
							while (%k < getFieldCount(%line))
							{
								%key = firstWord(getField(%line, %k));
								if (%key $= "ghostID")
								{
									Statistics.deleteGhosts(restWords(getField(%line, %k)), %levelName);
								}
								%k = %k + 1.0;
							}
						}
					}
				}
				else
				{
					if (%cleanedBestTimes $= "")
					{
						%cleanedBestTimes = %line;
						break;
					}
					%cleanedBestTimes = %cleanedBestTimes NL %line;
				}
				%j = %j + 1.0;
			}
			Statistics.writeBestTimes(%levelName, %cleanedBestTimes);
		}
		%i = %i + 1.0;
	}
	$ACTUAL_MENU.back();
	if (isCurrentUser(%selectedId))
	{
		debugEcho("deleting currentprofile...switching to first profile in list" SPC getWord($settings::Profile::allProfiles, "0"));
		$settings::Profile::currentProfile = getWord($settings::Profile::allProfiles, "0");
		loadSettings($settings::Profile::currentProfile, "0");
		playmodeManager.backToMenu();
	}
	if ($WII)
	{
		menu_switchProfiles.save = "1";
	}
	else
	{
		saveSettings(-1.0);
	}
	SwitchProfiles::refresh();
	return;
}
function SwitchProfiles::onSwitchProfileButton(%profileId)
{
	%selectedId = %profileId;
	if (%profileId $= "")
	{
		%selectedId = SwitchProfiles::getSelectedId();
	}
	if (%selectedId == -1.0)
	{
		return;
	}
	if (%selectedId == $settings::Profile::currentProfile)
	{
		debugEcho("we did not switch profile becuase its the same profile that was selected" SPC $settings::Profile::currentProfile SPC %profileId);
		if ($WII)
		{
			menu_switchProfiles.back();
		}
		return;
	}
	saveSettings($settings::Profile::currentProfile);
	SwitchProfiles::switchProfile(%profileId);
	return;
}
function SwitchProfiles::switchProfile(%profileId, %forceSwitch)
{
	%selectedId = %profileId;
	if (%profileId $= "")
	{
		%selectedId = SwitchProfiles::getSelectedId();
	}
	if (%selectedId == -1.0)
	{
		return;
	}
	globalPauseKey("0");
	if (%selectedId == $settings::Profile::currentProfile && !(%forceSwitch))
	{
		debugEcho("we did not switch profile becuase its the same profile that was selected" SPC $settings::Profile::currentProfile);
		return;
	}
	if ($levelLoadedFinished)
	{
		currentLevelObject.endLevel();
	}
	if ($settings::Profile::currentProfile != -1.0)
	{
		saveSettings($settings::Profile::currentProfile);
	}
	$settings::Profile::currentProfile = %selectedId;
	loadSettings($settings::Profile::currentProfile, "0");
	if ($WII)
	{
		menu_switchProfiles.save = "1";
	}
	else
	{
		saveSettings(-1.0);
	}
	SpeedRunMode.active = "0";
	initMenu();
	setupMenuLists();
	videoInit();
	audioInit();
	playmodeManager.backToMenu();
	globalPauseKey("1");
	if ($WII)
	{
		Canvas.showCursor("0");
		Canvas.setCursor("0", ayimCursor);
	}
	else
	{
		Canvas.showCursor();
	}
	return;
}
function SwitchProfiles::viewAchievements()
{
	%selectedId = SwitchProfiles::getSelectedId();
	if (%selectedId == -1.0)
	{
		return;
	}
	menu_achievements.profileId = %selectedId;
	$ACTUAL_MENU.enterMenu(menu_achievements);
	return;
}
function SwitchProfiles::newProfile()
{
	if ($WII)
	{
		%profileLimit = 3;
	}
	else
	{
		%profileLimit = 20;
	}
	if (getWordCount($settings::Profile::allProfiles) >= %profileLimit)
	{
		menu_dialog.Show($lbl_tooManyProfilesTitle, $lbl_tooManyProfilesText, $btn_OK);
		return;
	}
	txt_createProfile_name.text = "";
	btn_createProfile_qt.text = $btn_back;
	btn_createProfile_qt.Command = "$ACTUAL_MENU.back();";
	btn_createProfile_create.text = $btn_create;
	btn_createProfile_create.setExent("180", "90");
	btn_createProfile_create.Command = "MenuAction::createProfile();";
	chb_createProfile_showHints.setVisible("0");
	menu_createProfile.editProfile = "0";
	$ACTUAL_MENU.enterMenu(menu_createProfile);
	return;
}
function SwitchProfiles::getSelectedId()
{
	%selection = listContainer.getSelectedItems();
	if (%selection == -1.0)
	{
		lbl_switchProfiles_alert.text = "No profile has been selected. Please choose a row in the list.";
		return -1.0;
	}
	lbl_switchProfiles_alert.text = "";
	return getWord($settings::Profile::allProfiles, getField(%selection, "0"));
	return getWord($settings::Profile::allProfiles, getField(%selection, "0"));
}
function SwitchProfiles::refresh()
{
	if ($WII)
	{
		%i = 1;
		while (%i <= 3.0)
		{
			"btn_profile" @ %i.setText($lbl_switchProfiles_newProfile);
			['"btn_profile"', '%i'].Profile = "AyimButtonProfile";
			['"btn_profile"', '%i'].Command = ['"MenuAction::createProfile("', '%i', '");"'];
			"btn_clear_profile" @ %i.setActive("0");
			['"btn_clear_profile"', '%i'].text = $lbl_clearProfile;
			"lbl_playtime_profile" @ %i.setText($lbl_switchProfiles_totalTime SPC "-");
			"lbl_deathcount_profile" @ %i.setText($lbl_switchProfiles_totalDeaths SPC "-");
			"lbl_progress_profile" @ %i.setText($lbl_switchProfiles_progress SPC "-");
			%i = %i + 1.0;
		}
		backButton.setVisible("0");
		%i = 0;
		while (%i < getWordCount($settings::Profile::allProfiles))
		{
			%profileNR = getWord($settings::Profile::allProfiles, %i);
			%profileData = SwitchProfiles::getListData(%profileNR);
			%name = $lbl_wii_profile_name SPC %profileNR;
			"btn_profile" @ %profileNR.setText(%name);
			if (%profileNR == $settings::Profile::currentProfile)
			{
				['"btn_profile"', '%profileNR'].Profile = "AyimButtonActiveProfile";
			}
			%command = "SwitchProfiles::onSwitchProfileButton(" @ %profileNR @ ");";
			['"btn_profile"', '%profileNR'].Command = %command;
			"btn_clear_profile" @ %profileNR.setActive("1");
			"lbl_playtime_profile" @ %profileNR.setText($lbl_switchProfiles_totalTime SPC getField(%profileData, "1"));
			"lbl_deathcount_profile" @ %profileNR.setText($lbl_switchProfiles_totalDeaths SPC getField(%profileData, "2"));
			"lbl_progress_profile" @ %profileNR.setText($lbl_switchProfiles_progress SPC getField(%profileData, "3"));
			backButton.setVisible("1");
			%i = %i + 1.0;
		}
	}
	else
	{
		lbl_switchProfiles_alert.text = "";
		lbl_switchProfiles_hello.text = $lbl_hello SPC getUserName($settings::Profile::currentProfile);
		listContainer.clearList();
		%i = 0;
		while (%i < getWordCount($settings::Profile::allProfiles))
		{
			%listData = SwitchProfiles::getListData(getWord($settings::Profile::allProfiles, %i));
			listContainer.addRow(%listData);
			%i = %i + 1.0;
		}
	}
	loadSettings($settings::Profile::currentProfile, "1");
	return;
}
