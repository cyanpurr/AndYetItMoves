// menuAction.cs.dso
function ButtonOK::onWake(%this)
{
	if (text $= "")
	{
		%this.text = $btn_OK;
	}
	return;
}
function ButtonCancel::onWake(%this)
{
	if (text $= "")
	{
		%this.text = $btn_cancel;
	}
	return;
}
function ButtonOK::onAction(%this)
{
	%goBack = 1;
	if (%this.getName() $= "btn_video_OK")
	{
	}
	else
	{
		if (%this.getName() $= "btn_options_OK")
		{
			menu_options.saveAudio();
			menu_options.saveInput();
			if ($WII)
			{
				if (save)
				{
					menu_options.save = "0";
					saveSettings($settings::Profile::currentProfile);
				}
				if (straightToGame)
				{
					menu_options.straightToGame = "0";
					menu_options.back();
					MenuAction::continueGame();
					return;
				}
			}
			else
			{
				%goBack = !(menu_options.saveVideo());
			}
			break;
		}
		if (%this.getName() $= "btn_custom_OK")
		{
			if (checkBoxChecked)
			{
				menu_options.performanceLevel = "custom";
				rd_performance_low.setStateOn("0");
				rd_performance_medium.setStateOn("0");
				rd_performance_high.setStateOn("0");
			}
			break;
		}
		if (%this.getName() $= "btn_cheats_OK")
		{
			confirmCheats();
			break;
		}
		debugWarn("the pressed OK button" SPC %this.getName() SPC "has no action configured!");
	}
	if (%goBack)
	{
		$ACTUAL_MENU.back();
	}
	return;
}
function ButtonCancel::onAction(%this)
{
	if (%this.getName() $= "btn_options_cancel")
	{
		$MenuAudioChannel.setVolume($settings::Audio::Music);
		if ($WII)
		{
			debugEcho("canceling Options");
			if (straightToGame)
			{
				menu_options.straightToGame = "0";
				menu_options.back();
				MenuAction::continueGame();
				return;
			}
		}
	}
	$ACTUAL_MENU.back();
	return;
}
function ButtonBack::onAction(%this)
{
	$ACTUAL_MENU.back();
	return;
}
function PopUpLanguage::onWake(%this)
{
	ltrim($availableLanguagesNative);
	%this.clear();
	%i = 0;
	while (%i < getFieldCount($availableLanguagesNative))
	{
		%this.add(getField($availableLanguagesNative, %i), %i);
		%i = %i + 1.0;
	}
	%this.sort();
	%this.setSelected(%this.findText(englishToNativeLanguage($settings::Personal::Language)));
	return;
}
function MenuAction::createProfile(%ourProfileId)
{
	if ($WII)
	{
		if (%ourProfileId $= "" || getWordIndex($settings::Profile::allProfiles, %ourProfileId) != -1.0)
		{
			%wiiProfileIds = "1 2 3";
			%i = 0;
			while (%i < getWordCount(%wiiProfileIds))
			{
				%currentId = getWord(%wiiProfileIds, %i);
				if (getWordIndex($settings::Profile::allProfiles, %currentId) == -1.0)
				{
					%ourProfileId = %currentId;
					break;
				}
				%i = %i + 1.0;
			}
		}
		%profileName = $lbl_wii_profile_name SPC %ourProfileId;
		currentLevelObject.endLevel();
		if ($settings::Profile::currentProfile != -1.0)
		{
			saveSettings($settings::Profile::currentProfile);
		}
		loadDefaultUserSettings();
		$settings::Profile::allProfiles = trim($settings::Profile::allProfiles SPC %ourProfileId);
		saveSettings(-1.0);
	}
	else
	{
		if ($SnowdriftlandSpecial)
		{
			%profileName = "Snowdriftlander";
			%location = "Snowdriftland";
			%language = "english";
			currentLevelObject.endLevel();
			loadDefaultUserSettings();
			%ourProfileId = "MIS";
			$settings::Profile::allProfiles = trim($settings::Profile::allProfiles SPC %ourProfileId);
			$settings::Personal::Name = %profileName;
			$settings::Personal::Language = %language;
			$settings::Personal::Location = %location;
			$settings::Statistics::SubmitTime = 0;
			$settings::Statistics::SubmitGhost = 0;
			echo("Snowdriftland Settings");
			break;
		}
		%profileName = validateString(txt_createProfile_name.getText());
		%location = validateString(txt_createProfile_location.getText(), "1");
		if (%profileName == -1.0 || %location == -1.0)
		{
			return;
		}
		%language = nativeLanguageToEnglish(popup_createProfile_language.getTextById(popup_createProfile_language.getSelected()));
		currentLevelObject.endLevel();
		loadDefaultUserSettings();
		if (!(isExternalUser($futureProfileId)))
		{
			nextUniqueId("profile");
			%ourProfileId = $settings::Profile::highestID;
			$settings::Profile::allProfiles = trim($settings::Profile::allProfiles SPC %ourProfileId);
			saveSettings(-1.0);
		}
		else
		{
			$settings::Other::useExternalName = %profileName $= getExternalData("Name");
			debugEcho("set useExternal name of" SPC $futureProfileId SPC "to this:" SPC $settings::Other::useExternalName);
			%ourProfileId = $futureProfileId;
		}
		$settings::Personal::Language = %language;
		$settings::Personal::Location = %location;
		$settings::Statistics::SubmitTime = rd_createProfile_submitGhost.getValue();
		$settings::Statistics::SubmitGhost = rd_createProfile_submitGhost.getValue();
	}
	$settings::Personal::Name = %profileName;
	$settings::Profile::Id = %ourProfileId;
	debugEcho("new profile:" SPC %ourProfileId SPC "got name" SPC %profileName SPC "and location:" SPC %location SPC "language" SPC %language SPC "submit" SPC $settings::Statistics::SubmitTime);
	Canvas.popDialog(menu_createProfile);
	debugEcho("created profile file for" SPC %profileName SPC "with id:" SPC $settings::Profile::Id SPC "cP" SPC %ourProfileId);
	$settings::Profile::currentProfile = %ourProfileId;
	SwitchProfiles::switchProfile(%ourProfileId, "1");
	return;
}
function MenuAction::editProfile(%this)
{
	%name = validateString(txt_createProfile_name.getText());
	%location = validateString(txt_createProfile_location.getText(), "1");
	if (rd_createProfile_submitTime.getValue() || %name == -1.0 || %location == -1.0)
	{
		return;
	}
	$settings::Personal::Name = %name;
	if (isExternalUser($settings::Personal::Id) && $settings::Other::useExternalName)
	{
		$settings::Other::useExternalName = %name $= oldName;
	}
	$settings::Personal::Location = txt_createProfile_location.getText();
	$settings::Personal::Language = nativeLanguageToEnglish(popup_createProfile_language.getTextById(popup_createProfile_language.getSelected()));
	$settings::Statistics::SubmitTime = rd_createProfile_submitGhost.getValue();
	$settings::Statistics::SubmitGhost = rd_createProfile_submitGhost.getValue();
	if (rd_createProfile_submitTime.getValue() || chb_createProfile_showHints.getValue() != $settings::Other::ShowHints)
	{
		toggleHints();
	}
	saveSettings($settings::Personal::Id);
	loadSettings($settings::Profile::currentProfile, "0");
	$ACTUAL_MENU.back();
	return;
}
function validateString(%string, %location)
{
	%unusableChars = "";
	if (%string $= "")
	{
		if (%location)
		{
			lbl_createProfile_enterLocation.Profile = "AyimHeader1AlertProfile";
			lbl_createProfile_enterLocation.setText($alert_enterLocation);
			txt_createProfile_location.setText("");
		}
		else
		{
			lbl_createProfile_enterName.Profile = "AyimHeader1AlertProfile";
			lbl_createProfile_enterName.setText($alert_enterName);
			txt_createProfile_name.setText(%string);
		}
		if ($WII)
		{
			return "Default Wii Name";
		}
		return -1.0;
	}
	else
	{
		if (strlen(%string) > 100.0)
		{
			lbl_createProfile_enterLocation.Profile = "AyimHeader1AlertProfile";
			if (%location)
			{
				lbl_createProfile_enterLocation.setText($alert_stringTooLong);
				txt_createProfile_location.setText(%string);
			}
			else
			{
				lbl_createProfile_enterName.Profile = "AyimHeader1AlertProfile";
				lbl_createProfile_enterName.setText($alert_stringTooLong);
				txt_createProfile_name.setText(%string);
			}
			return -1.0;
			break;
		}
		if (%unusableChars != "")
		{
			%string = stripChars(%string, %unusableChars);
			%i = 0;
			while (%i < strlen(%unusableChars))
			{
				%charList = %charList @ getSubStr(%unusableChars, %i, "1");
				debugEcho("foudn unuseable char" SPC getSubStr(%unusableChars, %i, "1"));
				%i = %i + 1.0;
			}
			%charList = getSubStr(%charList, "2", strlen(%charList) - 2.0);
			if (%location)
			{
				lbl_createProfile_enterLocation.Profile = "AyimHeader1AlertProfile";
				lbl_createProfile_enterLocation.setText($alert_wrongCharacters SPC %charList);
				txt_createProfile_location.setText(%string);
			}
			else
			{
				lbl_createProfile_enterName.Profile = "AyimHeader1AlertProfile";
				lbl_createProfile_enterName.setText($alert_wrongCharacters SPC %charList);
				txt_createProfile_name.setText(%string);
			}
			return -1.0;
			break;
		}
		if (%location)
		{
			lbl_createProfile_enterLocation.Profile = "AyimHeader1Profile";
			lbl_createProfile_enterLocation.setText($lbl_enterLocation);
		}
		else
		{
			lbl_createProfile_enterName.Profile = "AyimHeader1Profile";
			lbl_createProfile_enterName.setText($lbl_enterName);
		}
		return trim(%string);
	}
	return trim(%string);
}
function MenuAction::continueGame(%loadFirst)
{
	if (getIsGamePaused())
	{
		$ACTUAL_MENU.back();
	}
	else
	{
		if (%loadFirst)
		{
			%levelIndex = 1;
		}
		else
		{
			%levelIndex = $settings::Secrets::LevelsUnlocked + 1.0;
		}
		echo("starting game with" SPC getWord($LEVELLIST, %levelIndex) SPC %loadFirst);
		playmodeManager.setMode("Story");
		MenuAction::loadLevel(getWord($LEVELLIST, %levelIndex));
	}
	return;
}
function MenuAction::loadLevel(%levelScriptObject)
{
	playmodeManager.startLevel(%levelScriptObject);
	return;
}
function MenuAction::toggleFullScreen(%on)
{
	echo("toggling fullscreen" SPC %on);
	menu_options.selectedScreenMode = %on;
	menu_options.setResolutonList();
	return;
}
function MenuAction::toggleResolution(%foreward)
{
	if (%foreward)
	{
		%modify = 1;
	}
	else
	{
		%modify = -1.0;
	}
	menu_options.setResolutionIndex(selectedResIndex + %modify);
	return;
}
function MenuAction::toggleSpeedRun(%this)
{
	SpeedRunMode.active = !(active);
	if (active)
	{
		btn_levelDetail_speedRun.setBitmap($BTN_RADIO_SELECTED);
	}
	else
	{
		btn_levelDetail_speedRun.setBitmap($BTN_RADIO_NORMAL);
	}
	return;
}
function MenuAction::toggleGhost(%this)
{
	$settings::Other::IncludeGhost = !($settings::Other::IncludeGhost);
	return;
}
function MenuAction::resumeGame(%this)
{
	$ACTUAL_MENU.back();
	return;
}
function MenuAction::viewAchievements(%this)
{
	menu_achievements.profileId = $settings::Profile::currentProfile;
	$ACTUAL_MENU.enterMenu(menu_achievements);
	return;
}
function MenuAction::quit()
{
	quit();
	return;
}
function MenuAction::setWiiInput(%input)
{
	if (!($WII) || !(isObject(wiiInput)))
	{
		return isObject(wiiInput);
	}
	if (%input != $settings::Wii::WiiInputMode)
	{
		menu_options.saveInput = "1";
	}
	$settings::Wii::WiiInputMode = %input;
	%inputButtons = "btn_options_driver btn_options_grabber btn_options_keyhole btn_options_classic";
	%inputIndex = getWordIndex($ALL_WII_INPUT_MODES, %input);
	if (%inputIndex == 1.0)
	{
		inputButton.setBitmap("game/" @ $DataFolder @ "/images/Menu/input/overlay_active");
		inputButton.setBitmap("game/" @ $DataFolder @ "/images/Menu/input/overlay_inactive");
		$settings::Wii::NunchuckMode = %input;
	}
	else
	{
		if (%inputIndex == 2.0)
		{
			inputButton.setBitmap("game/" @ $DataFolder @ "/images/Menu/input/overlay_inactive");
			inputButton.setBitmap("game/" @ $DataFolder @ "/images/Menu/input/overlay_active");
			$settings::Wii::NunchuckMode = %input;
			break;
		}
		inputButton.setBitmap("game/" @ $DataFolder @ "/images/Menu/input/overlay_active");
		inputButton.setBitmap("game/" @ $DataFolder @ "/images/Menu/input/overlay_active");
	}
	menu_options.setSensitivityButton(getWord(sensitivity, %inputIndex));
	chb_options_invert.setValue(getWord(inverted, %inputIndex));
	debugEcho("set $settings::Wii::WiiInputMode to:" SPC %input SPC "(" SPC %inputIndex SPC ")");
	return;
}
