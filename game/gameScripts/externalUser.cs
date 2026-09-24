// externalUser.cs.dso
exec("~/gui/enterKey.gui");
function setExternalStat(%key, %value, %storeNow)
{
	if (getPublisherName() $= "Steam")
	{
		setExtStat(%key, %value, %storeNow);
	}
	else
	{
		if ($demoVersion)
		{
			return;
		}
		if (getExternalSetStatProcessAlive())
		{
			debugEcho("trying to set external stat" SPC %key SPC %value SPC "while thread is alive");
			schedule("300", "0", "setExternalStat", %key, %value);
			return;
		}
		debugEcho("now setting external stat" SPC %key SPC %value SPC "with thread");
		setExtStat(%key, %value);
	}
	return;
}
function setExternalAchievement(%achievementName, %saveNow)
{
	if (getPublisherName() $= "Steam")
	{
		setExtAchievement(%achievementName, %saveNow);
	}
	else
	{
		if (getExternalSetStatProcessAlive())
		{
			debugEcho("trying to set external achievement" SPC %achievementName SPC "while thread is alive");
			schedule("300", "0", "setExternalAchievement", %achievementName);
			return;
		}
		debugEcho("now setting external achievement" SPC %achievementName SPC "with thread");
		setExtAchievement(%achievementName);
	}
	return;
}
function checkExternalClient()
{
	echo("checking for" SPC getPublisherName() SPC "client");
	if (getPublisherName() $= "Steam")
	{
		if (!(isExternalLoggedIn()))
		{
			menu_oneLineWarning.Show($lbl_steamLoginWarning, "");
			schedule("500", "0", "checkExternalInit");
			return "0";
		}
		if (!(isExternalOnline()) && $waitForOnlineUser > 0.0)
		{
			menu_oneLineWarning.Show($lbl_steamWaitForServer, getWaitingString("7", "1"));
			$onlineCheckSchedId = schedule("300", "0", "checkExternalOnline");
			return "0";
		}
		return "1";
	}
	else
	{
		if (getPublisherName() $= "Greenhouse")
		{
			$inGameAuthCheck = 0;
			return checkExternalAuthorized();
		}
	}
	return "1";
	return "1";
}
function checkExternalInit()
{
	if (isExternalLoggedIn())
	{
		menu_oneLineWarning.Hide();
		initializeProject();
		return;
	}
	%nextTryTime = 500;
	if (initExternalClient())
	{
		%nextTryTime = 1000;
	}
	schedule(%nextTryTime, "0", "checkExternalInit");
	return;
}
function checkExternalOnline()
{
	if (!(isExternalOnline()) && $waitForOnlineUser > 0.0)
	{
		initExternalClient();
		menu_oneLineWarning.setLine2(getWaitingString("7"));
		$waitForOnlineUser = $waitForOnlineUser - 300.0;
		schedule("300", "0", "checkExternalOnline");
	}
	else
	{
		menu_oneLineWarning.Hide();
		$onlineCheckSchedId = schedule("10", "0", "initializeProject");
	}
	return;
}
function checkExternalAuthorized()
{
	if ($demoVersion)
	{
		return "1";
	}
	if (!(isExternalAuthorized()))
	{
		if ($inGameAuthCheck)
		{
			MenuAction::loadLevel(level_gameMenu);
		}
		if (!(checkAuthStatus()))
		{
			return "0";
		}
	}
	if (!($inGameAuthCheck))
	{
		%nitrogenResult = initNitrogen();
		echo("nigrogen init returned:" SPC %nitrogenResult);
	}
	return "1";
	return "1";
}
function checkAuthStatus()
{
	%authStatus = getExternalAuthorizedStatus();
	if (%authStatus $= "Status_OK")
	{
		return "1";
	}
	else
	{
		if (%authStatus $= "Status_NoKey")
		{
			showLicenseInputDialog();
			break;
		}
		if (%authStatus $= "Status_Mismatch_Prod")
		{
			showLicenseInputDialog();
			break;
		}
		if (%authStatus $= "Status_Mismatch_RelNum")
		{
			showLicenseInputDialog();
			break;
		}
		if (%authStatus $= "Status_InconsistentKey")
		{
			showSpecialLicenseInputDialog($lbl_GHkeyProblem);
			break;
		}
		if (%authStatus $= "Status_InconsistentDuration")
		{
			showSpecialLicenseInputDialog($lbl_GHkeyProblem);
			break;
		}
		if (%authStatus $= "Status_Mismatch_HWID")
		{
			showRetryAuthDialog();
			break;
		}
		if (%authStatus $= "Status_Mismatch_Plat")
		{
			showRetryAuthDialog();
			break;
		}
		if (%authStatus $= "Status_UnknownError")
		{
			showAuthQuitDialog($lbl_GHunknownProblem);
		}
	}
	echo("!!!!! Greenhouse AuthStatus result:" SPC %authStatus SPC "!!!!!");
	return "0";
	return "0";
}
function showSpecialLicenseInputDialog(%text)
{
	showLicenseInputDialog();
	setAuthWarningText(%text);
	return;
}
function showLicenseInputDialog()
{
	Canvas.popDialog(menu_enterKey);
	hideAuthWarningText();
	lbl_enterKey.setVisible("1");
	txt_enterKey.setVisible("1");
	btn_enterKey_enter.setActive("1");
	btn_enterKey_enter.text = $btn_enterKey_validate;
	btn_enterKey_enter.Command = "enterLicenseKey();";
	Canvas.pushDialog(menu_enterKey);
	return;
}
function showAuthQuitDialog(%text, %hideRightButton)
{
	Canvas.popDialog(menu_enterKey);
	lbl_enterKey.setVisible("0");
	txt_enterKey.setVisible("0");
	setAuthWarningText(%text);
	btn_enterKey_enter.setActive("0");
	btn_enterKey_enter.setVisible("1");
	if (%hideRightButton)
	{
		btn_enterKey_enter.setVisible("0");
	}
	Canvas.pushDialog(menu_enterKey);
	return;
}
function showRetryAuthDialog()
{
	Canvas.popDialog(menu_enterKey);
	lbl_enterKey.setVisible("0");
	txt_enterKey.setVisible("0");
	btn_enterKey_enter.setActive("1");
	setAuthWarningText($lbl_GHretryAuth);
	btn_enterKey_enter.text = $btn_enterKey_retry;
	btn_enterKey_enter.Command = "retryAuthFromKeyfile();";
	Canvas.pushDialog(menu_enterKey);
	return;
}
function retryAuthFromKeyfile()
{
	setAuthWarningText($lbl_GHwaitingForLicenseServer);
	btn_enterKey_enter.setActive("0");
	startExternalAuthProcess("");
	$sceneIsUnpausedForDynamicGui = 0;
	if (isObject(scenegraph) && scenegraph.getScenePause())
	{
		$sceneIsUnpausedForDynamicGui = 1;
		scenegraph.setScenePause("0");
	}
	schedule("300", "0", "finishAuthorization");
	return;
}
function enterLicenseKey()
{
	%license = txt_enterKey.getValue();
	setAuthWarningText($lbl_GHwaitingForLicenseServer);
	btn_enterKey_enter.setActive("0");
	if (%license $= "")
	{
		%license = "bla";
	}
	startExternalAuthProcess(%license);
	$sceneIsUnpausedForDynamicGui = 0;
	if (isObject(scenegraph) && scenegraph.getScenePause())
	{
		$sceneIsUnpausedForDynamicGui = 1;
		scenegraph.setScenePause("0");
	}
	schedule("300", "0", "finishAuthorization");
	return;
}
function finishAuthorization()
{
	if (getExternalAuthProcessAlive())
	{
		setAuthWarningText($lbl_GHwaitingForLicenseServer @ getWaitingString("10"));
		schedule("300", "0", "finishAuthorization");
		return;
	}
	if (isObject(scenegraph) && $sceneIsUnpausedForDynamicGui)
	{
		$sceneIsUnpausedForDynamicGui = 0;
		scenegraph.setScenePause("1");
	}
	btn_enterKey_enter.setActive("1");
	hideAuthWarningText();
	%result = getExternalAuthResult();
	if (isExternalAuthResultOk())
	{
		Canvas.popDialog(menu_enterKey);
		if (!($inGameAuthCheck))
		{
			initializeProject();
		}
		return;
	}
	if (%result $= "AuthResult_Connect_DrmUrlMissing")
	{
		showSpecialLicenseInputDialog($lbl_GHunableConnect);
	}
	else
	{
		if (%result $= "AuthResult_Connect_CannotResolveDrmUrl")
		{
			showSpecialLicenseInputDialog($lbl_GHunableConnect);
			break;
		}
		if (%result $= "AuthResult_Connect_Unable")
		{
			showSpecialLicenseInputDialog($lbl_GHunableConnect);
			break;
		}
		if (%result $= "AuthResult_Connect_Timeout")
		{
			showSpecialLicenseInputDialog($lbl_GHunableConnect);
			break;
		}
		if (%result $= "AuthResult_Connect_SSL_Error")
		{
			showSpecialLicenseInputDialog($lbl_GHunableConnect);
			break;
		}
		if (%result $= "AuthResult_Server_AlreadyAuthorized")
		{
			showAuthQuitDialog($lbl_GHlicenseProblemText);
			break;
		}
		if (%result $= "AuthResult_Server_InconsistentAuthCode")
		{
			showAuthQuitDialog($lbl_GHlicenseProblemText);
			break;
		}
		if (%result $= "AuthResult_Server_LicenceRevoked")
		{
			showAuthQuitDialog($lbl_GHlicenseProblemText);
			break;
		}
		if (%result $= "AuthResult_Server_OverInstallLimit")
		{
			showAuthQuitDialog($lbl_GHlicenseProblemText);
			break;
		}
		if (%result $= "AuthResult_Server_MismatchProduct")
		{
			showAuthQuitDialog($lbl_GHlicenseProblemText);
			break;
		}
		if (%result $= "AuthResult_Server_MismatchReleaseNumber")
		{
			showAuthQuitDialog($lbl_GHlicenseProblemText);
			break;
		}
		if (%result $= "AuthResult_Server_MismatchPlatform")
		{
			showAuthQuitDialog($lbl_GHlicenseProblemText);
			break;
		}
		if (%result $= "AuthResult_KeyFile_Inconsistent")
		{
			showAuthQuitDialog($lbl_GHlicenseProblemText);
			break;
		}
		if (%result $= "AuthResult_UnknownError")
		{
			showAuthQuitDialog($lbl_GHlicenseProblemText);
			break;
		}
		if (%result $= "AuthResult_Server_DatabaseError")
		{
			showSpecialLicenseInputDialog($lbl_GHserverProblemText);
			break;
		}
		if (%result $= "AuthResult_Server_Error")
		{
			showSpecialLicenseInputDialog($lbl_GHserverProblemText);
			break;
		}
		if (%result $= "AuthResult_Licence_NotWellFormed")
		{
			showSpecialLicenseInputDialog($lbl_GHnotWellFormedText);
			break;
		}
		if (%result $= "AuthResult_Server_LicenceNotFound")
		{
			showSpecialLicenseInputDialog($lbl_GHnotWellFormedText);
			break;
		}
		if (%result $= "AuthResult_KeyFile_CannotRead")
		{
			showAuthQuitDialog($lbl_GHaccessKeyFileText);
			break;
		}
		if (%result $= "AuthResult_KeyFile_CannotWrite")
		{
			showAuthQuitDialog($lbl_GHaccessKeyFileText);
			break;
		}
		if (%result $= "AuthResult_MiscCurlError")
		{
			showAuthQuitDialog($lbl_GHtotallyFuckedText);
			break;
		}
		if (%result $= "AuthResult_CurlTotallyFucked")
		{
			showAuthQuitDialog($lbl_GHtotallyFuckedText);
		}
	}
	echo("!!!!! Greenhouse Authresult:" SPC %result SPC "!!!!!");
	return;
}
function checkAuthInGame()
{
	$inGameAuthCheck = 1;
	checkExternalAuthorized();
	return;
}
function hideAuthWarningText()
{
	%i = 0;
	while (%i < 3.0)
	{
		%alertLabel = "lbl_enterKey_alert_" @ %i + 1.0;
		%alertLabel.setVisible("0");
		%i = %i + 1.0;
	}
	return;
}
function setAuthWarningText(%text)
{
	hideAuthWarningText();
	%i = 0;
	while (%i < getRecordCount(%text))
	{
		%alertLabel = "lbl_enterKey_alert_" @ %i + 1.0;
		%alertLabel.text = getRecord(%text, %i);
		%alertLabel.setVisible("1");
		%i = %i + 1.0;
	}
	return getRecordCount(%text);
}
