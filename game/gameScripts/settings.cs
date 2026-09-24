// settings.cs.dso
function initSettings()
{
	if ($WII)
	{
		setWiiMoteEnabled("1", "0");
		setWiiMoteEnabled("2", "0");
		setWiiMoteEnabled("3", "0");
	}
	$default_location = "Nirvana";
	if ($distributorName $= "Zoo" || $distributorName $= "Playism")
	{
		$default_language = "japanese";
	}
	else
	{
		$default_language = "english";
	}
	$fallback_language = "english";
	debugEcho("init settings:" SPC "fallbacklang" SPC $fallback_language SPC "defaultlang" SPC $default_language SPC "defaultloc" SPC $default_location);
	loadSettings(-1.0, "0", "1");
	setGameLanguage($fallback_language);
	if ($fallback_language != $default_language)
	{
		setGameLanguage($default_language);
	}
	loadDefaultUserSettings();
	if ($settings::Profile::currentProfile == -1.0)
	{
		echo("no profile set -> create one");
		newExternalSetting();
		$showCreateProfilePopUp = 1;
	}
	else
	{
		if (getPublisherName() != "")
		{
			if (isExternalUser($settings::Profile::currentProfile) && !(isCurrentUser(getExternalData("Id"))))
			{
				debugEcho("changing curProfile from" SPC $settings::Profile::currentProfile SPC "to" SPC getExternalData("Id"));
				$settings::Profile::currentProfile = getExternalData("Id");
				$showCreateProfilePopUp = 1;
			}
			debugEcho("checking externalIds" SPC $settings::Profile::externalIDs SPC "aP:" SPC $settings::Profile::allProfiles);
			%i = 0;
			while (%i < getWordCount($settings::Profile::externalIDs))
			{
				if (getExternalData("Id") $= getWord($settings::Profile::externalIDs, %i))
				{
					$settings::Profile::allProfiles = trim(getExternalData("Id") SPC $settings::Profile::allProfiles);
					debugEcho("found file for id" SPC getExternalData("Id") SPC $settings::Profile::allProfiles);
					%foundProfile = 1;
					break;
				}
				%i = %i + 1.0;
			}
			if (!(%foundProfile))
			{
				newExternalSetting();
			}
			loadSettings($settings::Profile::currentProfile, "0", "1");
			break;
		}
		loadSettings($settings::Profile::currentProfile, "0", "1");
	}
	return;
}
function newExternalSetting()
{
	loadDefaultUserSettings();
	if ($WII || getPublisherName() $= "")
	{
		return "0";
	}
	debugEcho("creating new external Data" SPC getExternalData("Id"));
	$settings::Personal::Id = getExternalData("Id");
	$settings::Other::useExternalName = 1;
	$settings::Personal::Name = getExternalData("Name");
	saveSettings($settings::Personal::Id);
	$settings::Profile::allProfiles = getExternalData("Id") SPC $settings::Profile::allProfiles;
	$settings::Profile::allProfiles = trim($settings::Profile::allProfiles);
	$settings::Profile::externalIDs = $settings::Profile::externalIDs SPC getExternalData("Id");
	$settings::Profile::externalIDs = trim($settings::Profile::externalIDs);
	saveSettings(-1.0);
	$futureProfileId = $settings::Personal::Id;
	return "1";
	return "1";
}
function nextUniqueId(%target, %level)
{
	%idMapping["profile"] = 1;
	%idMapping["ghost"] = 2;
	if (%idMapping[%target] == 1.0)
	{
		while (1)
		{
			$settings::Profile::highestID = $settings::Profile::highestID + 1.0;
			%profilePath = getSettingFilePath($settings::Profile::highestID, "0");
			%secretPath = getSettingFilePath($settings::Profile::highestID, "1");
			if (!(isFile(%profilePath)) && !(isFile(%secretPath)))
			{
				break;
			}
		}
	}
	else
	{
		if (%idMapping[%target] == 2.0)
		{
			while (1)
			{
				$settings::Ghost::highestID = $settings::Ghost::highestID + 1.0;
				%ghostPath = getGhostFilePath(%level, $settings::Ghost::highestID);
				if (!(isFile(%ghostPath)))
				{
					break;
				}
			}
			break;
		}
		debugWarn("invalid target in function 'nextUniqueId':" SPC %target);
	}
	return;
}
function loadSettings(%profileId, %onlyReadValues, %forceFileRead)
{
	if (%profileId == -1.0)
	{
		$settings::Profile::currentProfile = -1.0;
		$settings::Profile::externalUser = 0;
		$settings::Profile::externalIDs = "";
		$settings::Profile::highestID = 0;
		$settings::Profile::allProfiles = "";
		$settings::Ghost::highestID = 0;
		$settings::Standards::scoresQuantity = 50;
		$settings::Status::GamePlayed = 0;
		$settings::Status::FullscreenNotify = 0.0;
	}
	else
	{
		loadDefaultUserSettings();
	}
	if ($WII && !($demoVersion))
	{
		%validSettings = readValues(%profileId, "0", %forceFileRead);
	}
	if (%profileId > -1.0)
	{
		$settings::Performance::Layers = 1;
		if (!($settings::Status::GamePlayed) && $settings::Video::Fullscreen)
		{
			$settings::Status::GamePlayed = 1;
			$settings::Video::Fullscreen = 0;
		}
		if (!($WII))
		{
			$settings::Cheats::DoubleSpeed = 1;
			$settings::Cheats::FixedCamera = 0;
		}
	}
	if (%validSettings > -1.0)
	{
		%errorReadingValues = !(%validSettings);
		if (!($WII) && %profileId != -1.0)
		{
			%validSecrets = readValues(%profileId, "1", %forceFileRead);
			if (%validSecrets == -1.0)
			{
				return;
			}
			%errorReadingValues = %errorReadingValues | !(%validSecrets);
			if (!(%onlyReadValues) && isExternalUser(%profileId) && isExternalOnline())
			{
				if ($settings::Other::useExternalName)
				{
					$settings::Personal::Name = getExternalData("Name");
				}
				%externalTotalPlayTime = getExternalData("TotalPlayTime");
				if ($settings::Secrets::TotalPlayTime > %externalTotalPlayTime)
				{
				}
				else
				{
				}
				$settings::Secrets::TotalPlayTime = %externalTotalPlayTime;
				%externalTotalDeathCount = getExternalData("TotalDeathCount");
				if ($settings::Secrets::TotalDeathCount > %externalTotalDeathCount)
				{
				}
				else
				{
				}
				$settings::Secrets::TotalDeathCount = %externalTotalDeathCount;
				%externalLevelsUnlocked = getExternalData("LevelsUnlocked");
				if ($settings::Secrets::LevelsUnlocked > %externalLevelsUnlocked)
				{
				}
				else
				{
				}
				$settings::Secrets::LevelsUnlocked = %externalLevelsUnlocked;
				%externalAchievements = achievements.getExternalList();
				%i = 0;
				while (%i < getWordCount(%externalAchievements))
				{
					%achievedIndex = getWord(%externalAchievements, %i);
					if (!(findWord($settings::Secrets::Achieved, %achievedIndex)))
					{
						$settings::Secrets::Achieved = trim($settings::Secrets::Achieved SPC %achievedIndex);
					}
					%i = %i + 1.0;
				}
				saveSettings(%profileId);
				setAllExternalData();
			}
			if (!(%onlyReadValues))
			{
				menu_options.getAvailableResolutions();
				if (!(findField(resolutionsFullscreen, $settings::Video::Full)))
				{
					%defRes = getWords(getDesktopResolution(), "0", "1");
					debugEcho(['"set unavailable fullscreen resolution ("', '$settings::Video::Full', '") to default:"'] SPC %defRes);
					$settings::Video::Full = %defRes;
				}
				if (!(findField(resolutionsWindowed, $settings::Video::Window)))
				{
					%defRes = "1024 768";
					debugEcho(['"set unavailable windowed resolution ("', '$settings::Video::Window', '") to default:"'] SPC %defRes);
					$settings::Video::Window = %defRes;
				}
				configureInput("keyboard", $settings::Controls::KeyboardLayout, "1");
				configureInput("joystick", $settings::Controls::JoystickLayout);
			}
		}
		if (!(%onlyReadValues) && %profileId != -1.0)
		{
			setGameLanguage($settings::Personal::Language);
		}
		if (%errorReadingValues)
		{
			if (%profileId != -1.0)
			{
				$settings::Personal::Id = %profileId;
			}
			echo("WARNING: loadSettings: something went wrong with reading the values of this profile:" SPC %profileId SPC "overwriting with defaults: Other::ID is" SPC $settings::Personal::Id);
			saveSettings(%profileId);
		}
	}
	if ($unlockSpecials || $previewVersion)
	{
		$settings::Cheats::CheatsUnlocked = "1 1 1";
		$settings::Playmodes::ModesUnlocked = "1 1 1";
		$settings::Secrets::LevelsUnlocked = 22;
	}
	return;
}
function loadDefaultUserSettings()
{
	$lastStartedVersion = "";
	$settings::Personal::Language = $default_language;
	$settings::Personal::Location = $default_location;
	$settings::Personal::Id = 0;
	$settings::Personal::Name = "Default Username";
	$settings::Other::useExternalName = 0;
	$settings::Other::IncludeGhost = 1;
	$settings::Other::lastLevel = 1;
	$settings::Other::filterGhost = 0;
	$settings::Other::ShowHints = 1;
	$settings::Other::VersionNotified = 0.0;
	$settings::Other::ReceivedNews = 0;
	$settings::Other::LastLevelTab = "cave";
	$settings::Other::uniqueUserScore = 1;
	$settings::Other::UpdateDiscovered = "";
	$settings::Other::LastVersion = -1.0;
	$settings::Secrets::TotalPlayTime = 0;
	$settings::Secrets::TotalDeathCount = 0;
	if ($WII && $demoVersion)
	{
		$settings::Secrets::LevelsUnlocked = 13;
	}
	else
	{
		$settings::Secrets::LevelsUnlocked = 0;
	}
	$settings::Secrets::BonusLevelsFinished = "";
	$settings::Secrets::Achieved = "";
	$settings::Secrets::BestScores = "";
	$settings::Cheats::DoubleSpeed = 1;
	if ($WII)
	{
		$settings::Cheats::FreeRotation = 1;
	}
	else
	{
		$settings::Cheats::FreeRotation = 0;
	}
	$settings::Cheats::FixedCamera = 0;
	$settings::Cheats::CheatsUnlocked = "0 0 0";
	if ($WII && $demoVersion)
	{
		$settings::Playmodes::ModesUnlocked = "1 0 0";
	}
	else
	{
		$settings::Playmodes::ModesUnlocked = "0 0 0";
	}
	$settings::Playmodes::StampsRallyMode = "-1 2 2 2 2 2 2 2 2 2 2 2 2 2 2 2 2 -1 2 2 2 2";
	$settings::Playmodes::StampsLimitedRotationMode = "-1 2 2 2 2 2 2 2 2 2 2 2 2 2 2 2 2 -1 2 2 2 2";
	$settings::Playmodes::StampsSurvivalMode = "-1 2 2 2 2 2 2 2 2 2 2 2 2 2 2 2 2 -1 2 2 2 2";
	$settings::Statistics::SubmitTime = 1;
	$settings::Statistics::SubmitGhost = 1;
	$settings::Statistics::TimeFrame = 0;
	$settings::Video::Full = getWords(getDesktopResolution(), "0", "1");
	$settings::Video::Window = "1024 768";
	if ($settings::Video::Fullscreen $= "")
	{
		$settings::Video::Fullscreen = 0;
	}
	$settings::Audio::Music = 1;
	$settings::Audio::Effects = 1;
	$settings::Performance::Level = "Low";
	$settings::Performance::Shaders = 0;
	$settings::Performance::Layers = 1;
	$settings::Performance::VerticalSync = 1;
	$pref::Video::disableVerticalSync = 0;
	$settings::Controls::KeyboardLayout = "a d w right left up backspace -";
	$settings::Controls::JoystickLayout = "- - - - - - - -";
	$settings::Controls::dontShowResetNotifications = 0;
	if (getOS() $= "wii")
	{
		if (getLanguageCode() == 1.0)
		{
			%lang = "english";
		}
		else
		{
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
		$settings::Personal::Language = %lang;
		$settings::Controls::Sensitivity = "1 1 1 1";
		$settings::Controls::Inverted = "1 1 1 1";
		$settings::Wii::WiiInputMode = "driver";
		$settings::Wii::NunchuckMode = "keyhole";
		$settings::Wii::ShowedInstructions = "0 0 0 0";
	}
	else
	{
		$settings::Controls::SixenseEnabled = isSixensePossible();
		$settings::Controls::Sensitivity = "1";
		$settings::Controls::Inverted = "0";
		$settings::Controls::ShowedInstructions = "0";
	}
	if ($unlockSpecials || $previewVersion)
	{
		$settings::Cheats::CheatsUnlocked = "1 1 1";
		$settings::Playmodes::ModesUnlocked = "1 1 1";
		$settings::Secrets::LevelsUnlocked = 21;
	}
	return;
}
function readValues(%profileId, %secret, %forceFileRead)
{
	if (!($WII))
	{
		%forceFileRead = 1;
	}
	if (%profileId == -1.0)
	{
		%dataIndex = 0;
	}
	else
	{
		%dataIndex = %profileId;
	}
	if (%forceFileRead || settingsData[%dataIndex] $= "")
	{
		%settingFile = getSettingFile(%profileId, "0", %secret);
		if (isObject(%settingFile))
		{
			if (checkVersionCompatibility(%settingFile.readLine(), %secret))
			{
				while (!(%settingFile.isEOF()))
				{
					%line = %settingFile.readLine();
					if (%settingsData $= "")
					{
						%settingsData = %line;
					}
					else
					{
						%settingsData = %settingsData NL %line;
					}
				}
				%settingFile.close();
				if (!(%forceFileRead))
				{
					globals.settingsData[%dataIndex] = %settingsData;
				}
			}
			else
			{
				%settingFile.close();
				return "0";
			}
		}
		else
		{
			%settingFile.close();
			return -1.0;
		}
	}
	else
	{
		%settingsData = settingsData[%dataIndex];
	}
	%j = 0;
	while (%j < getRecordCount(%settingsData))
	{
		%line = getRecord(%settingsData, %j);
		%category = firstWord(%line);
		%pairs = restWords(%line);
		%i = 0;
		while (%i < getFieldCount(%pairs))
		{
			%key = firstWord(getField(%pairs, %i));
			%value = restWords(getField(%pairs, %i));
			if ($WII && %category $= "Personal" && %key $= "Language")
			{
			}
			else
			{
				if (getWordCount(%value) == 0.0)
				{
					eval("$settings::" @ %category @ "::" @ %key @ "="";");
					break;
				}
				if (getWordCount(%value) > 1.0 || %key $= "Name" || %key $= "Location")
				{
					eval("$settings::" @ %category @ "::" @ %key @ "= "" @ %value @ "";");
					break;
				}
				eval("$settings::" @ %category @ "::" @ %key @ "= " @ %value @ ";");
			}
			%i = %i + 1.0;
		}
		%j = %j + 1.0;
	}
	if (!(%secret))
	{
		if ($lastStartedVersion != "" && $lastStartedVersion > $settings::Other::LastVersion)
		{
			$settings::Other::LastVersion = $lastStartedVersion;
		}
		if ($settings::Other::LastVersion == -1.0)
		{
			setUpdateDiscovered("achievementnum28");
			setUpdateDiscovered("steamSummerAchievement");
			setUpdateDiscovered("modifications");
		}
	}
	return "1";
	return "1";
}
function checkVersionCompatibility(%version, %secret)
{
	if (!(firstWord(%version)) $= "Version")
	{
		$lastStartedVersion = 0;
		return "0";
	}
	%lastVersion = restWords(%version);
	if (%lastVersion != $currentVersion && !(%secret))
	{
		$lastStartedVersion = %lastVersion;
	}
	if (%lastVersion == $currentVersion || %lastVersion >= 0.3499999940395355)
	{
		return "1";
	}
	else
	{
		echo("WARNING: different version of settings file:" SPC %version SPC "currentVersion is" SPC $currentVersion SPC "reverting settings to default! TODO: make something more intelligent here");
		return "0";
	}
	return "0";
}
function isUpdateDiscoverable(%name)
{
	if ($WII)
	{
		return "0";
	}
	else
	{
		%isDiscoverable = findWord($settings::Other::UpdateDiscovered, %name) == 0.0;
		if (%isDiscoverable)
		{
			if (%name $= "speedrunmode")
			{
				%isDiscoverable = $settings::Secrets::LevelsUnlocked > 0.0;
				break;
			}
			if (%name $= "rallymode")
			{
				%isDiscoverable = getWord($settings::Playmodes::ModesUnlocked, "0");
				break;
			}
			if (%name $= "limitedrotationmode")
			{
				%isDiscoverable = getWord($settings::Playmodes::ModesUnlocked, "1");
				break;
			}
			if (%name $= "survivalmode")
			{
				%isDiscoverable = getWord($settings::Playmodes::ModesUnlocked, "2");
				break;
			}
			if (%name $= "steamSummerAchievement")
			{
				if ($distributorName != "Steam")
				{
					%isDiscoverable = 0;
				}
				break;
			}
			if (%name $= "speedmodification")
			{
				%isDiscoverable = getWord($settings::Cheats::CheatsUnlocked, "0");
				break;
			}
			if (%name $= "freerotationmodification")
			{
				return "0";
				%isDiscoverable = getWord($settings::Cheats::CheatsUnlocked, "1");
				break;
			}
			if (%name $= "fixedcameramodification")
			{
				%isDiscoverable = getWord($settings::Cheats::CheatsUnlocked, "2");
			}
		}
		return %isDiscoverable;
	}
	return %isDiscoverable;
}
function setUpdateDiscovered(%name)
{
	if ($distributorName != "Steam" && %name $= "steamSummerAchievement")
	{
		return;
	}
	if (isUpdateDiscoverable(%name))
	{
		$settings::Other::UpdateDiscovered = trim($settings::Other::UpdateDiscovered SPC %name);
	}
	return;
}
function saveSettings(%profile, %clearFile)
{
	echo("saving settings of profile," SPC %profile SPC "," SPC daSceneGraph.getSceneTime());
	if (%profile == -1.0)
	{
		%tempAllProfiles = $settings::Profile::allProfiles;
		$settings::Profile::allProfiles = "";
		%i = 0;
		while (%i < getWordCount(%tempAllProfiles))
		{
			if (!(isExternalUser(getWord(%tempAllProfiles, %i))))
			{
				$settings::Profile::allProfiles = trim($settings::Profile::allProfiles SPC getWord(%tempAllProfiles, %i));
			}
			%i = %i + 1.0;
		}
		$settings::Profile::allProfiles = trim($settings::Profile::allProfiles);
		%lines = "Version" SPC $currentVersion;
		%lines = %lines NL "Profile" SPC "currentProfile" SPC $settings::Profile::currentProfile TAB "allProfiles" SPC $settings::Profile::allProfiles TAB "highestID" SPC $settings::Profile::highestID TAB "externalUser" SPC $settings::Profile::externalUser TAB "externalIDs" SPC $settings::Profile::externalIDs;
		%lines = %lines NL "Ghost" SPC "highestID" SPC $settings::Ghost::highestID;
		%lines = %lines NL "Standards" SPC "scoresQuantity" SPC $settings::Standards::scoresQuantity;
		if (!($WII))
		{
			%lines = %lines NL "Status" SPC "GamePlayed" SPC $settings::Status::GamePlayed TAB "FullscreenNotify" SPC $settings::Status::FullscreenNotify;
		}
		writeFile(getSettingFilePath(%profile), %lines, "0", %clearFile);
		$settings::Profile::allProfiles = %tempAllProfiles;
	}
	else
	{
		%lines = "Version" SPC $currentVersion;
		if ($WII)
		{
			%lines = %lines NL "Personal" SPC "Id" SPC %profile TAB "Name" SPC $settings::Personal::Name TAB "Location" SPC $settings::Personal::Location;
		}
		else
		{
			%lines = %lines NL "Personal" SPC "Id" SPC %profile TAB "Name" SPC $settings::Personal::Name TAB "Language" SPC $settings::Personal::Language TAB "Location" SPC $settings::Personal::Location;
		}
		%lines = %lines NL "Other" SPC "useExternalName" SPC $settings::Other::useExternalName TAB "IncludeGhost" SPC $settings::Other::IncludeGhost TAB "FilterGhost" SPC $settings::Other::filterGhost TAB "showHints" SPC $settings::Other::ShowHints TAB "VersionNotified" SPC $settings::Other::VersionNotified TAB "LastLevelTab" SPC $settings::Other::LastLevelTab TAB "uniqueUserScore" SPC $settings::Other::uniqueUserScore TAB "ReceivedNews" SPC $settings::Other::ReceivedNews TAB "UpdateDiscovered" SPC $settings::Other::UpdateDiscovered TAB "LastVersion" SPC $settings::Other::LastVersion;
		%lines = %lines NL "Statistics" SPC "SubmitTime" SPC $settings::Statistics::SubmitTime TAB "SubmitGhost" SPC $settings::Statistics::SubmitGhost TAB "TimeFrame" SPC $settings::Statistics::TimeFrame;
		%lines = %lines NL "Video" SPC "Full" SPC $settings::Video::Full TAB "Window" SPC $settings::Video::Window TAB "Fullscreen" SPC $settings::Video::Fullscreen;
		%lines = %lines NL "Audio" SPC "Music" SPC $settings::Audio::Music TAB "Effects" SPC $settings::Audio::Effects;
		%lines = %lines NL "Performance" SPC "VerticalSync" SPC $settings::Performance::VerticalSync TAB "Shaders" SPC $settings::Performance::Shaders TAB "Level" SPC $settings::Performance::Level;
		%lines = %lines NL "Controls" SPC "KeyboardLayout" SPC $settings::Controls::KeyboardLayout TAB "JoystickLayout" SPC $settings::Controls::JoystickLayout TAB "dontShowResetNotifications" SPC $settings::Controls::dontShowResetNotifications TAB "SixenseEnabled" SPC $settings::Controls::SixenseEnabled TAB "ShowedInstructions" SPC $settings::Controls::ShowedInstructions TAB "Sensitivity" SPC $settings::Controls::Sensitivity TAB "Inverted" SPC $settings::Controls::Inverted;
		if ($WII)
		{
			%lines = %lines NL "Wii" SPC "WiiInputMode" SPC $settings::Wii::WiiInputMode TAB "NunchuckMode" SPC $settings::Wii::NunchuckMode TAB "ShowedInstructions" SPC $settings::Wii::ShowedInstructions;
			%lines = %lines NL "Playmodes" SPC "ModesUnlocked" SPC $settings::Playmodes::ModesUnlocked TAB "StampsRallyMode" SPC $settings::Playmodes::StampsRallyMode TAB "StampsLimitedRotationMode" SPC $settings::Playmodes::StampsLimitedRotationMode TAB "StampsSurvivalMode" SPC $settings::Playmodes::StampsSurvivalMode;
			%lines = %lines NL "Cheats" SPC "DoubleSpeed" SPC $settings::Cheats::DoubleSpeed TAB "FreeRotation" SPC $settings::Cheats::FreeRotation TAB "FixedCamera" SPC $settings::Cheats::FixedCamera TAB "CheatsUnlocked" SPC $settings::Cheats::CheatsUnlocked;
		}
		else
		{
			writeFile(getSettingFilePath(%profile), %lines, "0");
			%lines = "Version" SPC $currentVersion;
			%lines = %lines NL "Playmodes" SPC "ModesUnlocked" SPC $settings::Playmodes::ModesUnlocked TAB "StampsRallyMode" SPC $settings::Playmodes::StampsRallyMode TAB "StampsLimitedRotationMode" SPC $settings::Playmodes::StampsLimitedRotationMode TAB "StampsSurvivalMode" SPC $settings::Playmodes::StampsSurvivalMode;
			%lines = %lines NL "Cheats" SPC "DoubleSpeed" SPC $settings::Cheats::DoubleSpeed TAB "FreeRotation" SPC $settings::Cheats::FreeRotation TAB "FixedCamera" SPC $settings::Cheats::FixedCamera TAB "CheatsUnlocked" SPC $settings::Cheats::CheatsUnlocked;
		}
		%lines = %lines NL "Secrets" SPC "TotalPlayTime" SPC $settings::Secrets::TotalPlayTime TAB "TotalDeathCount" SPC $settings::Secrets::TotalDeathCount TAB "Achieved" SPC $settings::Secrets::Achieved TAB "LevelsUnlocked" SPC $settings::Secrets::LevelsUnlocked TAB "BestScores" SPC $settings::Secrets::BestScores TAB "BonusLevelsFinished" SPC $settings::Secrets::BonusLevelsFinished;
		if (!($WII))
		{
			writeFile(getSettingFilePath(%profile, "1"), %lines, "1");
		}
		else
		{
			writeFile(getSettingFilePath(%profile), %lines, "0", %clearFile);
		}
		if (%profile == -1.0)
		{
			%dataIndex = 0;
		}
		else
		{
			%dataIndex = %profile;
		}
		globals.settingsData[%dataIndex] = removeRecord(%lines, "0");
	}
	return;
}
function getSettingFile(%profileId, %writeMode, %secret)
{
	%settingFilePath = getSettingFilePath(%profileId, %secret);
	if (%writeMode)
	{
		%settingFile = new FileObject(Name : "");
		%settingFile.openForWrite(%settingFilePath);
	}
	else
	{
		%settingFile = validateFile(%settingFilePath, "1");
		if (!($WII) && %secret)
		{
			%file = new FileObject(Name : "");
			%file.openForRead(%settingFilePath);
			%lines = "";
			while (!(%file.isEOF()))
			{
				%lines = %lines NL %file.readLine();
			}
			%lines = trim(%lines);
			%lineCnt = getRecordCount(%lines);
			%content = getRecords(%lines, "1", %lineCnt - 2.0);
			%content = trim(%content);
			%hashFromFile = getRecord(%lines, %lineCnt - 1.0);
			%realHash = getSecret(%content);
			if (%hashFromFile != %realHash)
			{
				copyFile(%settingFilePath, %settingFilePath @ ".corrupted");
				return -1.0;
			}
		}
	}
	return %settingFile;
	return %settingFile;
}
function getSettingFilePath(%profileId, %secret)
{
	if ($WII)
	{
		%prePath = "";
	}
	else
	{
		%prePath = "settings/";
	}
	if (%profileId == -1.0)
	{
		%settingFilePath = %prePath @ "general.opt";
	}
	else
	{
		if (!($WII) && %secret)
		{
			%settingFilePath = %prePath @ "secret" @ %profileId @ ".aym";
			break;
		}
		%settingFilePath = %prePath @ "profile" @ %profileId @ ".aym";
	}
	return %settingFilePath;
	return %settingFilePath;
}
function getGhostFilePath(%level, %ghostID, %fileEnding)
{
	if (%ghostID $= "")
	{
		return ['"ghosts/"', '%level', '"/"'];
	}
	else
	{
		if (%fileEnding $= "")
		{
			%fileEnding = "gst";
		}
		return ['"ghosts/"', '%level', '"/"', '%ghostID', '"."', '%fileEnding'];
	}
	return ['"ghosts/"', '%level', '"/"', '%ghostID', '"."', '%fileEnding'];
}
function getUserName(%profileId)
{
	if (%profileId == -1.0)
	{
		%profileId = $settings::Profile::currentProfile;
	}
	if ($WII)
	{
		return $lbl_wii_profile_name SPC %profileId;
	}
	if (getPublisherName() != "" && isExternalUser(%profileId) && isCurrentUser(%profileId) && getProfileData(%profileId, "Personal", "useExternalName"))
	{
		return getExternalData("Name");
	}
	%name = getProfileData(%profileId, "Personal", "Name");
	if (%name $= "")
	{
		return -1.0;
	}
	return %name;
	return %name;
}
function getProfileData(%profileId, %category, %key)
{
	if (%profileId == -1.0)
	{
		%profileId = $settings::Profile::currentProfile;
	}
	if (isCurrentUser(%profileId))
	{
		eval("%value = $settings::" @ %category @ "::" @ %key @ ";");
		return %value;
	}
	%settingsData = "";
	if ($WII)
	{
		if (%profileId == -1.0)
		{
			%dataIndex = 0;
		}
		else
		{
			%dataIndex = %profileId;
		}
		if (settingsData[%dataIndex] $= "")
		{
			%settingsFile = getSettingFile(%profileId);
		}
		else
		{
			%settingsData = settingsData[%dataIndex];
		}
	}
	else
	{
		if (%category $= "Secrets")
		{
			if (isExternalUser(%profileId) && isCurrentUser(%profileId) && isOnline())
			{
				return getExternalData(%key);
			}
			%settingsFile = getSettingFile(%profileId, "0", "1");
			break;
		}
		%settingsFile = getSettingFile(%profileId);
	}
	if (%settingsData $= "")
	{
		if (!(%settingsFile))
		{
			debugEcho("no settings file found for profile" SPC %profileId);
			return -1.0;
		}
		while (!(%settingsFile.isEOF()))
		{
			if (%settingsData $= "")
			{
				%settingsData = %settingsFile.readLine();
			}
			else
			{
				%settingsData = %settingsData NL %settingsFile.readLine();
			}
		}
	}
	%i = 0;
	while (%i < getRecordCount(%settingsData))
	{
		%line = getRecord(%settingsData, %i);
		%curCategory = firstWord(%line);
		%pairs = restWords(%line);
		if (%category $= %curCategory)
		{
			if (%key $= "")
			{
				return %pairs;
			}
			%j = 0;
			while (%j < getFieldCount(%pairs))
			{
				%curKey = firstWord(getField(%pairs, %j));
				if (%key $= %curKey)
				{
					return restWords(getField(%pairs, %j));
				}
				%j = %j + 1.0;
			}
		}
		%i = %i + 1.0;
	}
	return -1.0;
	return -1.0;
}
function isExternalUser(%profileId)
{
	if (%profileId == -1.0)
	{
		%profileId = $settings::Profile::currentProfile;
	}
	return strpos(%profileId, "External") > -1.0;
	return strpos(%profileId, "External") > -1.0;
}
function setSecretData(%key, %value, %saveNow, %profileId)
{
	if (%profileId $= "")
	{
		%profileId = $settings::Profile::currentProfile;
	}
	%value = trim(%value);
	if (isExternalUser(%profileId))
	{
		setExternalData(%key, %value, %saveNow);
	}
	eval(['"$settings::Secrets::"', '%key'] SPC "="" SPC %value SPC "";");
	if (%saveNow)
	{
		debugEcho("saving secret data" SPC %profileId);
		saveSettings(%profileId);
	}
	return;
}
function isCurrentUser(%profileId)
{
	return $settings::Profile::currentProfile $= %profileId;
	return $settings::Profile::currentProfile $= %profileId;
}
function getExternalData(%key)
{
	if (%key $= "Achieved")
	{
		return achievements.getExternalList();
	}
	else
	{
		if (%key $= "Id")
		{
			return ['"External"', <torque.FuncCall object at 0x0000021716DCDD90>];
			break;
		}
		if (%key $= "Name")
		{
			return getExternalInfo(%key);
			break;
		}
		if (%key $= "TotalPlaytime")
		{
			return getExternalStat(%key);
			break;
		}
		if (%key $= "TotalDeathCount")
		{
			return getExternalStat(%key);
			break;
		}
		if (%key $= "LevelsUnlocked")
		{
			return getExternalStat(%key);
		}
	}
	return getExternalStat(%key);
}
function setAllExternalData()
{
	%i = 0;
	while (%i < getWordCount($settings::Secrets::Achieved))
	{
		%index = getWord($settings::Secrets::Achieved, %i);
		achievements.setAchieved(getField(internalNames, %index), "0");
		%i = %i + 1.0;
	}
	setExternalData("TotalPlaytime", $settings::Secrets::TotalPlayTime, "0");
	setExternalData("TotalDeathCount", $settings::Secrets::TotalDeathCount, "0");
	setExternalData("LevelsUnlocked", $settings::Secrets::LevelsUnlocked, "1");
	return;
}
function setExternalData(%key, %value, %storeNow)
{
	if (%key $= "Achieved")
	{
		return;
	}
	setExternalStat(%key, %value, %storeNow);
	return;
}
function isEmptyProfile(%profileId)
{
	if (getWordIndex($settings::Profile::allProfiles, %profileId) == -1.0)
	{
		return "1";
	}
	else
	{
		return "0";
	}
	return "0";
}
