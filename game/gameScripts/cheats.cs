// cheats.cs.dso
function initCheats()
{
	menu_cheats.speedFactors = "0.7 1.0 1.5";
	menu_cheats.achievementsIndices = "18 11 17";
	confirmCheats("1");
	menu_cheats.allCheatsAreOn = menu_cheats.allCheatsAreOn();
	subscribeToEvents(menu_cheats, "onUnpauseGame onEnterLevelSwitch");
	return;
}
function showCheats()
{
	$ACTUAL_MENU.enterMenu(menu_cheats);
	return;
}
function menu_cheats::onDialogPush(%this)
{
	if (isUpdateDiscoverable("modifications"))
	{
		setUpdateDiscovered("modifications");
	}
	if (isUpdateDiscoverable("speedmodification"))
	{
		setUpdateDiscovered("speedmodification");
	}
	if (isUpdateDiscoverable("freerotationmodification"))
	{
		setUpdateDiscovered("freerotationmodification");
	}
	if (isUpdateDiscoverable("fixedcameramodification"))
	{
		setUpdateDiscovered("fixedcameramodification");
	}
	if (!($WII))
	{
		chb_cheats_freeRotation.setVisible("0");
	}
	debugEcho("unlockedCheats" SPC $settings::Cheats::CheatsUnlocked SPC "dS:" SPC getWord($settings::Cheats::CheatsUnlocked, "0") SPC "fR" SPC getWord($settings::Cheats::CheatsUnlocked, "1") SPC "fC" SPC getWord($settings::Cheats::CheatsUnlocked, "2"));
	lbl_cheats_speed.setActive(getWord($settings::Cheats::CheatsUnlocked, "0") $= "1");
	btn_cheats_halfSpeed.setActive(getWord($settings::Cheats::CheatsUnlocked, "0") $= "1");
	btn_cheats_halfSpeed.achievement = getField(names, getWord(achievementsIndices, "0"));
	btn_cheats_normalSpeed.setActive(getWord($settings::Cheats::CheatsUnlocked, "0") $= "1");
	btn_cheats_normalSpeed.achievement = getField(names, getWord(achievementsIndices, "0"));
	btn_cheats_doubleSpeed.setActive(getWord($settings::Cheats::CheatsUnlocked, "0") $= "1");
	btn_cheats_doubleSpeed.achievement = getField(names, getWord(achievementsIndices, "0"));
	chb_cheats_freeRotation.setActive(getWord($settings::Cheats::CheatsUnlocked, "1") $= "1");
	chb_cheats_freeRotation.achievement = getField(names, getWord(achievementsIndices, "1"));
	chb_cheats_fixedCamera.setActive(getWord($settings::Cheats::CheatsUnlocked, "2") $= "1");
	chb_cheats_fixedCamera.achievement = getField(names, getWord(achievementsIndices, "2"));
	%this.speedFactorIndex = $settings::Cheats::DoubleSpeed;
	if ($WII)
	{
		chb_cheats_freeRotation.setValue(!($settings::Cheats::FreeRotation));
	}
	else
	{
		chb_cheats_freeRotation.setValue($settings::Cheats::FreeRotation);
	}
	chb_cheats_fixedCamera.setValue($settings::Cheats::FixedCamera);
	if (speedFactorIndex == 0.0)
	{
		btn_cheats_halfSpeed.setStateOn("1");
	}
	else
	{
		if (speedFactorIndex == 1.0)
		{
			btn_cheats_normalSpeed.setStateOn("1");
			break;
		}
		if (speedFactorIndex == 2.0)
		{
			btn_cheats_doubleSpeed.setStateOn("1");
		}
	}
	%this.hideNotice();
	return;
}
function menu_cheats::onEnterPressed(%this)
{
	btn_cheats_ok.onAction();
	return;
}
function timeScale::setSpeed(%this, %speedFactor)
{
	menu_cheats.speedFactorIndex = %speedFactor;
	menu_cheats.hideNotice();
	return;
}
function unlockCheats(%achievementIndex)
{
	if ($demoVersion)
	{
		return;
	}
	%indexToSet = -1.0;
	debugEcho("unlocking cheats? searching" SPC %achievementIndex SPC "in" SPC achievementsIndices);
	%indexToSet = getWordIndex(achievementsIndices, %achievementIndex);
	if (%indexToSet > -1.0)
	{
		echo("unlocked cheat" SPC %indexToSet);
		$settings::Cheats::CheatsUnlocked = setWord($settings::Cheats::CheatsUnlocked, %indexToSet, "1");
	}
	return;
}
function confirmCheats(%valuesAreSet)
{
	if (!(%valuesAreSet))
	{
		$settings::Cheats::DoubleSpeed = speedFactorIndex;
		if ($WII)
		{
			$settings::Cheats::FreeRotation = !(chb_cheats_freeRotation.getValue());
		}
		else
		{
			$settings::Cheats::FreeRotation = chb_cheats_freeRotation.getValue();
		}
		$settings::Cheats::FixedCamera = chb_cheats_fixedCamera.getValue();
	}
	%speedFactor = getWord(speedFactors, $settings::Cheats::DoubleSpeed);
	if ($settings::Cheats::DoubleSpeed == 0.0)
	{
		$cheatsWereUsed = 1;
	}
	debugEcho("setting cheats!" SPC %speedFactor SPC $settings::Cheats::FreeRotation SPC $settings::Cheats::FixedCamera);
	setTimeScale(%speedFactor);
	alxRepitchLoops();
	camera.rotate = !($settings::Cheats::FixedCamera);
	camera.freeRotation = $settings::Cheats::FreeRotation;
	if (getIsGamePaused())
	{
		if (rotate)
		{
			camera.setViewWindowRotation(camera.getCurrentRotation());
		}
		else
		{
			camera.setViewWindowRotation("0");
		}
		if (!(freeRotation))
		{
			camera.snapToClosestAngle();
		}
	}
	if (!($WII) && !(%valuesAreSet))
	{
		saveSettings($settings::Profile::currentProfile);
	}
	return;
}
function CheatButton::onMouseEnter(%this)
{
	if (!(%this.isActive()))
	{
		if ($demoVersion)
		{
			menu_cheats.showNotice($lbl_requires_full_version);
			break;
		}
		menu_cheats.showNotice($lbl_mode_unlock_1 @ achievement @ $lbl_mode_unlock_2);
	}
	return;
}
function CheatButton::onMouseLeave(%this)
{
	menu_cheats.hideNotice();
	return;
}
function timeScale::onMouseEnter(%this)
{
	if (!(%this.isActive()))
	{
		if ($demoVersion)
		{
			menu_cheats.showNotice($lbl_requires_full_version);
		}
		else
		{
			menu_cheats.showNotice($lbl_mode_unlock_1 @ achievement @ $lbl_mode_unlock_2);
		}
	}
	else
	{
		if (%this.getId() == btn_cheats_halfSpeed.getId())
		{
			menu_cheats.showNotice($lbl_mode_nosaving, "1");
		}
	}
	return;
}
function timeScale::onMouseLeave(%this)
{
	menu_cheats.hideNotice();
	return;
}
function menu_cheats::showNotice(%this, %text, %isRed)
{
	lbl_cheats_unlock.text = %text;
	lbl_cheats_unlock.setVisible("1");
	if (%isRed)
	{
		lbl_cheats_unlock.setProfile("AyimMenuTextCenterRedProfile");
	}
	else
	{
		lbl_cheats_unlock.setProfile("AyimMenuTextCenterProfile");
	}
	return;
}
function menu_cheats::hideNotice(%this)
{
	if (speedFactorIndex == 0.0)
	{
		%this.showNotice($lbl_mode_nosaving, "1");
	}
	else
	{
		lbl_cheats_unlock.setVisible("0");
	}
	return;
}
function menu_cheats::allCheatsAreOn(%this)
{
	%allCheatsAreOn = $settings::Cheats::FixedCamera == 1.0;
	if ($settings::Cheats::DoubleSpeed != 1.0 && $WII)
	{
		%allCheatsAreOn = $settings::Cheats::FreeRotation == 0.0;
	}
	return %allCheatsAreOn;
	return %allCheatsAreOn;
}
function menu_cheats::onUnpauseGame(%this)
{
	if (%allCheatsAreOn && !(%this.allCheatsAreOn()))
	{
		%this.allCheatsAreOn = "0";
	}
	return;
}
function menu_cheats::onEnterLevelSwitch(%this)
{
	if (allCheatsAreOn)
	{
		achievements.setAchieved("Cheater");
	}
	return;
}
