// input.cs.dso
$KEYBOARD_ALL = "a b c d e f g h i j k l m n o p q r s t u v w x y z 1 2 3 4 5 6 7 8 9 0 numpad0 numpad1 numpad2 numpad3 numpad4 numpad5 numpad6 numpad7 numpad8 numpad9 numpadmult numpadadd numpadsep numpadminus numpaddecimal numpaddivide numpadenter backspace tab enter shift ctrl alt pause space pagedown pageup end home left up right down print insert delete tilde minus equals lbracket rbracket backslash semicolon apostrophe comma period slash lessthan f1 f2 f3 f4 f5 f6 f7 f8 f9 f10 f11 f12 f13 f14 f15 f16 f17 f18 f19 f20 f21 f22 f23 f24";
$CONTROLER_ALL = "button0 button1 button2 button3 button4 button5 button6 button7 button8 button9 button10 button11 button12 button13 button14 button15 button16 button17 button18 button19 button20 button21 button22 button23 button24 button25 button26 button27 button28 button29 button30 button31 upov dpov lpov rpov";
$ANALOG_ALL = "xaxis yaxis zaxis rxaxis ryaxis rzaxis slider";
$ANALOG_TRESHOLD = 0.30000001192092896;
$ALL_GAME_ACTIONS = "left right jump cw ccw rot180 reset pause";
$ACTION_NUMBER_MAPPING["noAction"] = 0;
$ACTION_NUMBER_MAPPING["left"] = 1;
$ACTION_NUMBER_MAPPING["right"] = 2;
$ACTION_NUMBER_MAPPING["jump"] = 3;
$ACTION_NUMBER_MAPPING["cw"] = 4;
$ACTION_NUMBER_MAPPING["ccw"] = 5;
$ACTION_NUMBER_MAPPING["rot180"] = 6;
$ACTION_NUMBER_MAPPING["reset"] = 7;
$ACTION_NUMBER_MAPPING["pause"] = 8;
function initKeyboard()
{
	$enableDirectInput = "1";
	activateDirectInput();
	enableJoystick();
	InitJoystickHandler();
	SwitchJoysticks("1");
	if (!(isObject(moveMap)))
	{
		new ActionMap(Name : moveMap);
	}
	configureInput("keyboard", $settings::Controls::KeyboardLayout, "1");
	configureInput("joystick", $settings::Controls::JoystickLayout);
	new ActionMap(Name : analogJoystickMap);
	analogJoystickMap.bind("joystick", "xaxis", "analogX");
	analogJoystickMap.bind("joystick", "yaxis", "analogY");
	analogJoystickMap.bind("joystick", "zaxis", "analogZ");
	analogJoystickMap.bind("joystick", "rxaxis", "analogRX");
	analogJoystickMap.bind("joystick", "ryaxis", "analogRY");
	analogJoystickMap.bind("joystick", "rzaxis", "analogRZ");
	analogJoystickMap.bind("joystick", "slider", "analogSldr");
	if (!(isObject(splashScreenMap)))
	{
		new ActionMap(Name : splashScreenMap);
	}
	if (!($WII))
	{
		splashScreenMap.bindCmd("keyboard", "anykey", "BeSplashScreener::keyPressed();", "");
		splashScreenMap.bindCmd("keyboard", "space", "BeSplashScreener::keyPressed();", "");
		splashScreenMap.bindCmd("keyboard", "escape", "BeSplashScreener::keyPressed();", "");
		splashScreenMap.bindCmd("joystick", "anykey", "BeSplashScreener::keyPressed();", "");
	}
	else
	{
		splashScreenMap.bindCmd("wiimote0", "anykey", "BeSplashScreener::keyPressed();", "");
		splashScreenMap.bindCmd("classic", "anykey", "BeSplashScreener::keyPressed();", "");
	}
	if ($enableDebugMap)
	{
		new ActionMap(Name : debugMap);
		debugMap.bindCmd("keyboard", "p", "toggleProfiler();", "");
		debugMap.bindCmd("keyboard", "k", "toggleVictoryMusic();", "");
		debugMap.bindCmd("keyboard", "l", "toggleAmbience();", "");
		debugMap.bindCmd("keyboard", "v", "verbose();", "verbose();");
		debugMap.bindCmd("keyboard", "c", "sixenseManager.startCalibration();", "");
		debugMap.bindCmd("keyboard", "strg s", "doScreenShot();", "1");
		debugMap.bind("keyboard", "x", "xDown");
		debugMap.bind("keyboard", "n", "nDown");
		new ActionMap(Name : debugJumpMap);
		debugJumpMap.bindCmd("keyboard", "up", "upDownKeys(1);", "");
		debugJumpMap.bindCmd("keyboard", "down", "upDownKeys(-1);", "");
		debugMap.bindCmd("keyboard", "1", "numberPressed(0);", "");
		debugMap.bindCmd("keyboard", "2", "numberPressed(1);", "");
		debugMap.bindCmd("keyboard", "3", "numberPressed(2);", "");
		debugMap.bindCmd("keyboard", "4", "numberPressed(3);", "");
		debugMap.bindCmd("keyboard", "5", "numberPressed(4);", "");
		debugMap.bindCmd("keyboard", "6", "numberPressed(5);", "");
		debugMap.bindCmd("keyboard", "7", "numberPressed(6);", "");
		debugMap.bindCmd("keyboard", "8", "numberPressed(7);", "");
		debugMap.bindCmd("keyboard", "9", "numberPressed(8);", "");
		debugMap.bind("keyboard", "PageUp", "zoomIn");
		debugMap.bind("keyboard", "PageDown", "zoomOut");
		debugMap.bind("keyboard", "Home", "resetZoom");
		debugMap.bind("keyboard", "space", "pauseGame");
		debugMap.bindCmd("keyboard", "alt k", "cls();", "");
		debugMap.bindCmd("keyboard", "g", "toggleGrabber();", "");
		debugMap.bindCmd("keyboard", "r", "simulateStartRotation();", "simulateFinishRotation();");
		debugMap.push();
	}
	else
	{
		GlobalActionMap.unbind("keyboard", $Game::ConsoleBind);
		GlobalActionMap.unbind("keyboard", $Game::ScreenshotBind);
		if (getOS() != "linux")
		{
			GlobalActionMap.unbind("keyboard", $Game::FullscreenBind);
		}
	}
	GlobalActionMap.bindCmd("keyboard", "escape", "escapeKey();", "");
	new ActionMap(Name : keyAssignmentMap);
	%i = 0;
	while (%i < getWordCount($KEYBOARD_ALL))
	{
		%key = getWord($KEYBOARD_ALL, %i);
		keyAssignmentMap.bindCmd("keyboard", %key, "assignInput(keyboard, " @ %key @ ");", "");
		%i = %i + 1.0;
	}
	new ActionMap(Name : buttonAssignmentMap);
	%i = 0;
	while (%i < getWordCount($CONTROLER_ALL))
	{
		%button = getWord($CONTROLER_ALL, %i);
		buttonAssignmentMap.bindCmd("joystick", %button, "assignInput(joystick, " @ %button @ ");", "");
		%i = %i + 1.0;
	}
	return getWordCount($CONTROLER_ALL);
}
function listenToDevice(%device, %action)
{
	cancelInputAssignment();
	$NEXT_ACTION_TO_ASSIGN = %action;
	if (%device $= "keyboard")
	{
		keyAssignmentMap.push();
	}
	else
	{
		if (%device $= "joystick")
		{
			$CONFIGURING_JOYSTICK = 1;
			buttonAssignmentMap.push();
			analogJoystickMap.push();
			break;
		}
		warn("cannot listen to unknown device:" SPC %device);
	}
	return;
}
function assignInput(%device, %input)
{
	if (%device $= "keyboard")
	{
		keyAssignmentMap.pop();
		$MODIFIED_KEYBOARD_LAYOUT = modifyMapping($MODIFIED_KEYBOARD_LAYOUT, $NEXT_ACTION_TO_ASSIGN, %input);
	}
	else
	{
		if (%device $= "joystick")
		{
			buttonAssignmentMap.pop();
			analogJoystickMap.pop();
			$CONFIGURING_JOYSTICK = 0;
			$MODIFIED_JOYSTICK_LAYOUT = modifyMapping($MODIFIED_JOYSTICK_LAYOUT, $NEXT_ACTION_TO_ASSIGN, %input);
			break;
		}
		warn("cannot assign input to unknown device:" SPC %device);
	}
	$NEXT_ACTION_TO_ASSIGN = "";
	menu_options.displayInput();
	return;
}
function cancelInputAssignment()
{
	if ($NEXT_ACTION_TO_ASSIGN != "")
	{
		keyAssignmentMap.pop();
		buttonAssignmentMap.pop();
		analogJoystickMap.pop();
		$CONFIGURING_JOYSTICK = 0;
		$NEXT_ACTION_TO_ASSIGN = "";
	}
	return;
}
function configureInput(%device, %inputList, %cleanMoveMap)
{
	if (%cleanMoveMap)
	{
		globalPauseKey("0");
		if (isObject(moveMap))
		{
			moveMap.delete();
		}
		if (isObject(resistentMoveMap))
		{
			resistentMoveMap.delete();
		}
		%i = 0;
		while (%i < getWordCount($ANALOG_ALL))
		{
			%j = 0;
			while (%j < 2.0)
			{
				%pseudoButton = getWord($ANALOG_ALL, %i) @ %j;
				$ANALOG_ACTION[%pseudoButton] = $ACTION_NUMBER_MAPPING["noAction"];
				%j = %j + 1.0;
			}
			%i = %i + 1.0;
		}
	}
	%i = 0;
	while (%i < getWordCount($ALL_GAME_ACTIONS))
	{
		%action = getWord($ALL_GAME_ACTIONS, %i);
		%input = getWord(%inputList, %i);
		if (%input $= "")
		{
			%input = "-";
		}
		if (%device $= "joystick" && findWord($ANALOG_ALL, stripChars(%input, "01")))
		{
			assignAnalog(%action, %input);
		}
		else
		{
			assignKey(%device, %action, %input);
		}
		%i = %i + 1.0;
	}
	if (%device $= "keyboard")
	{
		$settings::Controls::KeyboardLayout = %inputList;
	}
	else
	{
		if (%device $= "joystick")
		{
			$settings::Controls::JoystickLayout = %inputList;
		}
	}
	if (!(%cleanMoveMap))
	{
		globalPauseKey("1");
	}
	return;
}
function assignKey(%device, %action, %key)
{
	if (%key $= "-" || %key $= "")
	{
		return;
	}
	if (!(isObject(moveMap)))
	{
		new ActionMap(Name : moveMap);
	}
	if (!(isObject(resistentMoveMap)))
	{
		new ActionMap(Name : resistentMoveMap);
	}
	if (!(isObject(resetMap)))
	{
		new ActionMap(Name : resetMap);
	}
	if (%action $= "left")
	{
		moveMap.bind(%device, %key, "moveLeft");
	}
	else
	{
		if (%action $= "right")
		{
			moveMap.bind(%device, %key, "moveRight");
			break;
		}
		if (%action $= "jump")
		{
			moveMap.bind(%device, %key, "jump");
			break;
		}
		if (%action $= "cw")
		{
			moveMap.bindCmd(%device, %key, "rotateKeyDown(1);", "rotateKeyUp(1);");
			break;
		}
		if (%action $= "ccw")
		{
			moveMap.bindCmd(%device, %key, "rotateKeyDown(-1);", "rotateKeyUp(-1);");
			break;
		}
		if (%action $= "rot180")
		{
			moveMap.bindCmd(%device, %key, "rotate180();", "");
			break;
		}
		if (%action $= "reset")
		{
			resistentMoveMap.bindCmd(%device, %key, "resetLevel();", "");
			resetMap.bindCmd(%device, %key, "resetLevel();", "");
			break;
		}
		if (%action $= "pause")
		{
			resistentMoveMap.bindCmd(%device, %key, "escapeKey();", "");
			break;
		}
		return;
	}
	debugEcho("bound key" SPC %key SPC "to" SPC %action);
	return;
}
function assignAnalog(%action, %pseudoButton)
{
	if (%pseudoButton $= "-")
	{
		return;
	}
	$ANALOG_ACTION[%pseudoButton] = $ACTION_NUMBER_MAPPING[%action];
	return;
}
function modifyMapping(%mappingList, %action, %input)
{
	%actionIndex = getWordIndex($ALL_GAME_ACTIONS, %action);
	%oldMapping = getWord(%mappingList, %actionIndex);
	%dupicateIndex = getWordIndex(%mappingList, %input);
	if (%dupicateIndex != -1.0 && %dupicateIndex != %actionIndex)
	{
		%mappingList = setWord(%mappingList, %dupicateIndex, %oldMapping);
	}
	%mappingList = setWord(%mappingList, %actionIndex, %input);
	return %mappingList;
	return %mappingList;
}
function analogX(%value)
{
	analogInput("xaxis", %value);
	return;
}
function analogY(%value)
{
	analogInput("yaxis", %value);
	return;
}
function analogZ(%value)
{
	analogInput("zaxis", %value);
	return;
}
function analogRX(%value)
{
	analogInput("rxaxis", %value);
	return;
}
function analogRY(%value)
{
	analogInput("ryaxis", %value);
	return;
}
function analogRZ(%value)
{
	analogInput("rzaxis", %value);
	return;
}
function analogSldr(%value)
{
	analogInput("slider", %value);
	return;
}
function analogInput(%source, %value)
{
	if (%value >= $ANALOG_TRESHOLD)
	{
		if ($analogLeftZeroPosition[%source] == 0.0)
		{
			analogAction(%source @ "1", "1");
			$analogLeftZeroPosition[%source] = 1;
		}
		else
		{
			if ($analogLeftZeroPosition[%source] < 0.0)
			{
				analogAction(%source @ "0", "0");
				$analogLeftZeroPosition[%source] = 0;
			}
		}
	}
	else
	{
		if (%value <= -1.0 * $ANALOG_TRESHOLD)
		{
			if ($analogLeftZeroPosition[%source] == 0.0)
			{
				analogAction(%source @ "0", "1");
				$analogLeftZeroPosition[%source] = -1.0;
			}
			else
			{
				if ($analogLeftZeroPosition[%source] > 0.0)
				{
					analogAction(%source @ "1", "0");
					$analogLeftZeroPosition[%source] = 0;
				}
			}
			break;
		}
		if ($analogLeftZeroPosition[%source] == 1.0)
		{
			analogAction(%source @ "1", "0");
		}
		else
		{
			if ($analogLeftZeroPosition[%source] == -1.0)
			{
				analogAction(%source @ "0", "0");
			}
		}
		$analogLeftZeroPosition[%source] = 0;
	}
	return ['$analogLeftZeroPosition', '%source'];
}
function analogAction(%pseudoButton, %pushNotRelease)
{
	%actionToTrigger = $ANALOG_ACTION[%pseudoButton];
	if (%pushNotRelease)
	{
		if ($CONFIGURING_JOYSTICK)
		{
			assignInput("joystick", %pseudoButton);
			return;
		}
		if (%actionToTrigger == 1.0)
		{
			moveLeft("1");
		}
		else
		{
			if (%actionToTrigger == 2.0)
			{
				moveRight("1");
				break;
			}
			if (%actionToTrigger == 3.0)
			{
				jump("1");
				break;
			}
			if (%actionToTrigger == 4.0)
			{
				rotateKeyDown("1");
				break;
			}
			if (%actionToTrigger == 5.0)
			{
				rotateKeyDown(-1.0);
				break;
			}
			if (%actionToTrigger == 6.0)
			{
				rotate180();
				break;
			}
			if (%actionToTrigger == 7.0)
			{
				resetLevel();
				break;
			}
			if (%actionToTrigger == 8.0)
			{
				escapeKey();
			}
		}
	}
	else
	{
		if ($CONFIGURING_JOYSTICK)
		{
			return;
		}
		if (%actionToTrigger <= 2.0)
		{
			if (%actionToTrigger == 1.0)
			{
				moveLeft("0");
				break;
			}
			if (%actionToTrigger == 2.0)
			{
				moveRight("0");
			}
		}
	}
	return;
}
function rotateKeyDown(%key)
{
	if (!(firstKeyPressed()))
	{
		return firstKeyPressed();
	}
	if (freeRotation)
	{
		camera.startFreeRotation(%key);
	}
	else
	{
		camera.rotate(%key);
	}
	return;
}
function rotateKeyUp(%key)
{
	if (freeRotation)
	{
		camera.stopFreeRotation(%key);
	}
	return;
}
function rotate180()
{
	if (freeRotation)
	{
		return camera;
	}
	if (!(firstKeyPressed()))
	{
		return firstKeyPressed();
	}
	%direction = player.getFlipX() * 2.0 - 1.0;
	camera.rotate(%direction * 2.0);
	return;
}
function upDownKeys(%up)
{
	if ($nDown || $xDown)
	{
		nextPrevious(%up);
	}
	return;
}
function numberPressed(%num)
{
	if ($xDown)
	{
		$cheatsWereUsed = 1;
		setCustomSpawnPoint(%num + 1.0);
	}
	else
	{
		if ($nDown)
		{
			startGame(%num + 1.0);
			break;
		}
		if (%num < 8.0)
		{
			if (!(daSceneGraph.getDebugOn(%num)))
			{
				daSceneGraph.setDebugOn(%num);
			}
			else
			{
				daSceneGraph.setDebugOff(%num);
			}
			break;
		}
		if (%num == 8.0)
		{
			Canvas.toggleShowMemoryStats();
		}
	}
	return;
}
function nextPrevious(%next)
{
	if ($xDown)
	{
		%setSpawnPoint = number + %next;
		%setSpawnPoint = negModulo(%setSpawnPoint - 1.0, spawnPoints) + 1.0;
		$cheatsWereUsed = 1;
		debugEcho("jumping directly to spawnpoint:" SPC %setSpawnPoint SPC "of total:" SPC spawnPoints);
		setCustomSpawnPoint(%setSpawnPoint);
	}
	else
	{
		if ($nDown)
		{
			%levelToLoad = currentLevelNumber + %next;
			%levelToLoad = negModulo(%levelToLoad - 1.0, getWordCount($LEVELLIST)) + 1.0;
			startGame(%levelToLoad);
		}
	}
	return;
}
function zoomIn(%key)
{
	$zoomIn = %key;
	return;
}
function zoomOut(%key)
{
	$zoomOut = %key;
	return;
}
function resetZoom(%key)
{
	sceneWindow2d.setCurrentCameraZoom(currentCameraZoom);
	return;
}
function xDown(%key)
{
	$xDown = %key;
	if (%key)
	{
		debugJumpMap.push();
	}
	else
	{
		debugJumpMap.pop();
	}
	return;
}
function nDown(%key)
{
	$nDown = %key;
	if (%key)
	{
		debugJumpMap.push();
	}
	else
	{
		debugJumpMap.pop();
	}
	return;
}
function toggleHints()
{
	$settings::Other::ShowHints = !($settings::Other::ShowHints);
	settings.showHints = $settings::Other::ShowHints;
	%i = 0;
	while (%i < hintsGroup.getCount())
	{
		hintsGroup.getObject(%i).toggle();
		%i = %i + 1.0;
	}
	return hintsGroup.getCount();
}
function setControlsEnabled(%enable)
{
	globals.controlsEnabled = %enable;
	if (%enable && !($watchAsReplay))
	{
		globals.creditsControl = "0";
		if (isObject(moveMap))
		{
			moveMap.push();
		}
		if (isObject(resistentMoveMap))
		{
			resistentMoveMap.push();
		}
		if (isObject(analogJoystickMap))
		{
			analogJoystickMap.push();
		}
		if ($xDown)
		{
			xDown("1");
		}
		if ($nDown)
		{
			nDown("1");
		}
		if ($WII)
		{
			debugEcho("activating wiiInput:" SPC $settings::Wii::WiiInputMode);
			wiiInput.setInput($settings::Wii::WiiInputMode);
		}
		else
		{
			if (isSixensePossible())
			{
				sixenseManager.setInput("keyhole");
			}
		}
	}
	else
	{
		moveMap.pop();
		analogJoystickMap.pop();
		moveLeft("0");
		moveRight("0");
		if ($settings::Cheats::FreeRotation && camera.getIsRotating())
		{
			camera.stopFreeRotation();
		}
		if ($WII)
		{
			wiiInput.setInput("basic");
		}
	}
	return;
}
function setCustomSpawnPoint(%number, %skipZoomAnim)
{
	if ($isJumpingToSpawnPoint)
	{
		return;
	}
	%spawnPoint = getSpawnPoint(%number);
	if (!(isObject(%spawnPoint)))
	{
		return isObject(%spawnPoint);
	}
	$isJumpingToSpawnPoint = 1;
	%spawnPoint.setAsCurrent();
	%position = %spawnPoint.getPosition();
	player.setPosition(%position);
	if (!($watchAsReplay))
	{
		sceneWindow2d.dismount();
		sceneWindow2d.setCurrentCameraPosition(%position);
		sceneWindow2d.mount(player, "0 0", cameraMountForce, "0");
	}
	viewWindow.setPosition(%position);
	camera.setPosition(%position);
	player.reanimate("1", %skipZoomAnim);
	return;
}
function escapeKey()
{
	if (isVideo)
	{
		return currentLevelObject;
	}
	if (!($splashScreenShown))
	{
		BeSplashScreener::keyPressed();
		return;
	}
	if (Canvas.getObject(Canvas.getCount() - 1.0).getId() == menu_enterKey.getId())
	{
		return Canvas.getObject(Canvas.getCount() - 1.0).getId();
	}
	debugEcho("esc pressed" SPC $ACTUAL_MENU SPC "prev" SPC previousMenu SPC "global prev" SPC $PREVIOUS_MENU);
	if (!(isObject($ACTUAL_MENU)))
	{
		$dontplaymenusound = 1;
		menu_loading.back();
		$dontplaymenusound = 0;
	}
	else
	{
		$ACTUAL_MENU.onEscapePressed();
	}
	return;
}
function pauseGame()
{
	%paused = !(getIsGamePaused());
	daSceneGraph.setScenePause(%paused);
	if (!(isDead) || %paused)
	{
		setControlsEnabled(!(%paused));
	}
	debugEcho("pauseGame:" SPC %paused);
	globalPauseKey(%paused);
	if (%paused)
	{
		if (isObject(resistentMoveMap))
		{
			resistentMoveMap.pop();
		}
		triggerEvent("onPauseGame");
	}
	else
	{
		triggerEvent("onUnpauseGame");
	}
	return;
}
function globalPauseKey(%enable)
{
	%pauseKeyIndex = getWordIndex($ALL_GAME_ACTIONS, "pause");
	%pauseKey = getWord($settings::Controls::KeyboardLayout, %pauseKeyIndex);
	%pauseJoy = getWord($settings::Controls::JoystickLayout, %pauseKeyIndex);
	if (%enable)
	{
		if (%pauseKey != "-" && %pauseKey != "")
		{
			GlobalActionMap.bindCmd("keyboard", %pauseKey, "escapeKey();", "");
		}
		if (%pauseJoy != "-" && %pauseJoy != "")
		{
			GlobalActionMap.bindCmd("joystick", %pauseJoy, "escapeKey();", "");
		}
	}
	else
	{
		if (%pauseKey != "-" && %pauseKey != "")
		{
			GlobalActionMap.unbind("keyboard", %pauseKey);
		}
		if (%pauseJoy != "-" && %pauseJoy != "")
		{
			GlobalActionMap.unbind("joystick", %pauseJoy);
		}
	}
	return;
}
function resetLevel()
{
	if (isVideo)
	{
		return currentLevelObject;
	}
	if (!($levelLoadedFinished))
	{
		debugEcho("we don't do anything if the level hasn't loaded yet" SPC %this);
		return;
	}
	if ($WII || $settings::Controls::SixenseEnabled && !($reallyReset))
	{
		$reallyReset = 1;
		if (!(getIsGamePaused()))
		{
			debugEcho("calling pauseGame form resetLevel()" SPC getRealTime());
			pauseGame();
		}
		resetMap.push();
		$ACTUAL_MENU.enterMenu(menu_reset);
		if ($WII)
		{
			if (wiiInput.getConnectedExtension() $= "classic")
			{
				wiiInput.setInput("resetClassic");
			}
			else
			{
				wiiInput.setInput("reset");
			}
		}
		else
		{
			if ($settings::Controls::SixenseEnabled)
			{
				sixenseManager.setInput("reset");
			}
		}
		Canvas.setCursor("0", noCursor);
		return;
	}
	else
	{
		if ($WII || $settings::Controls::SixenseEnabled)
		{
			$ACTUAL_MENU.back();
		}
		Statistics.reset("1");
		triggerEvent("onResetLevel");
	}
	return;
}
function menu_reset::onDialogPop(%this)
{
	resetMap.pop();
	$reallyReset = 0;
	return;
}
function getIsGamePaused()
{
	if (!(isObject(daSceneGraph)))
	{
		return "0";
	}
	return daSceneGraph.getScenePause();
	return daSceneGraph.getScenePause();
}
function firstKeyPressed()
{
	if ($firstKeyPressed)
	{
		return "1";
	}
	if (!($levelLoadedFinished) || !(initialFadeCompleted))
	{
		return "0";
	}
	if (getIsGamePaused())
	{
		return "0";
	}
	debugEcho("first key pressed" SPC daSceneGraph.getSceneTime() SPC getFrameCount() SPC tickCounter);
	$firstKeyPressed = 1;
	if ($settings::Cheats::DoubleSpeed == 0.0)
	{
		$cheatsWereUsed = 1;
	}
	triggerEvent("onFirstKeyPressed");
	return "1";
	return "1";
}
function onWiimoteConnected()
{
	if (!(initialFadeCompleted))
	{
		wiiInput.disconnectedNotification = "0";
		return;
	}
	if (disconnectedNotification)
	{
		menu_dialog.back();
	}
	wiiInput.disconnectedNotification = "0";
	return;
}
function onWiiExtensionChange(, %type)
{
	debugEcho("onWiiExtensionChange: $A_M:" SPC $ACTUAL_MENU SPC "$P_M:" SPC $PREVIOUS_MENU SPC %type);
	if (%type != 0.0 && %type != 1.0 && %type != 2.0)
	{
		return;
	}
	if ($ACTUAL_MENU.getId() == menu_options.getId())
	{
		menu_options.showWiiInputModes();
		return;
	}
	if ($ACTUAL_MENU.getId() == menu_instructions.getId())
	{
		if ($PREVIOUS_MENU.getId() == menu_options.getId() || cameFromMenu)
		{
			menu_instructions.back();
			menu_options.showWiiInputModes();
			return;
			break;
		}
		echo("we are showing the instructions. don't do anything" SPC $PREVIOUS_MENU);
		menu_instructions.back();
		return;
	}
	if ($ACTUAL_MENU.getId() == menu_reset.getId())
	{
		echo("we are showing the reset menu. don't do anything" SPC $PREVIOUS_MENU);
		menu_reset.back();
		return;
	}
	if (currentLevelObject $= "" || !(initialFadeCompleted))
	{
		return camera;
	}
	if (!(isMenu) && !(getIsGamePaused()))
	{
		wiiInput.checkExtension();
	}
	return;
}
function onWiimoteDisconnected()
{
	if ($stressTestEnabled)
	{
		return;
	}
	wiiInput.disconnectedNotification = "1";
	if (!(initialFadeCompleted))
	{
		debugEcho("initialFade not completed" SPC %this);
		return;
	}
	if (!(isMenu) && !(getIsGamePaused()))
	{
		escapeKey();
	}
	menu_dialog.Show($lbl_wiimoteDisconnected_title, $lbl_wiimoteDisconnected_line, "", "", "", "", "", "", "1", "1");
	return;
}
function wiiInput::goToInputMenu()
{
	if (!(getIsGamePaused()))
	{
		escapeKey();
		menu_options.straightToGame = "1";
		menu_main.enterMenu(menu_options);
		tablbl_options_joystick.displayOptions("joystick");
	}
	return;
}
function wiiInput::getReadableExtensionName(%name)
{
	if (%name $= "wiimote")
	{
		return $lbl_wiimote;
	}
	else
	{
		if (%name $= "nunchuck")
		{
			return $lbl_nunchuk;
			break;
		}
		if (%name $= "classic")
		{
			return $lbl_classicControllers;
		}
	}
	return $lbl_classicControllers;
}
function wiiInput::checkExtension()
{
	debugEcho("CONNECTED:" SPC wiiInput.getConnectedExtension() SPC "NEEDED:" SPC wiiInput.getExtensionNeeded());
	if (wiiInput.getConnectedExtension() != wiiInput.getExtensionNeeded())
	{
		wiiInput.goToInputMenu();
		return "0";
	}
	return "1";
	return "1";
}
function showWiiInstructions(%inputMode)
{
	if (!($WII))
	{
		return "1";
	}
	if (%inputMode $= "")
	{
		%fromMenu = 0;
		%inputMode = $settings::Wii::WiiInputMode;
	}
	else
	{
		%fromMenu = 1;
	}
	%index = getWordIndex($ALL_WII_INPUT_MODES, %inputMode);
	if (!(%fromMenu) && getWord($settings::Wii::ShowedInstructions, %index) == 1.0)
	{
		return getWord($settings::Wii::ShowedInstructions, %index);
	}
	if ($ACTUAL_MENU.getId() == menu_instructions.getId())
	{
		debugEcho("tried to show instructions again although we are already showing it");
		return;
	}
	menu_instructions.showedPages = "0";
	menu_instructions.showedInstructionIndex = %index;
	menu_instructions.inputMode = %inputMode;
	menu_instructions.cameFromMenu = %fromMenu;
	if (!(%fromMenu) && !(getIsGamePaused()))
	{
		debugEcho("calling pauseGame from showWiiInstructions" SPC getRealTime());
		pauseGame();
	}
	if (%inputMode $= "keyhole")
	{
		%useInput = "grabber";
	}
	else
	{
		%useInput = %inputMode;
	}
	pic_instructions.setBitmap("game/" @ $DataFolder @ "/images/hints/" @ $settings::Personal::Language @ "/" @ %useInput @ "_instructions.png");
	debugEcho("language for instructions are:" SPC $settings::Personal::Language);
	$ACTUAL_MENU.enterMenu(menu_instructions, "1");
	if (!(%fromMenu))
	{
		if (!(getIsGamePaused()))
		{
			debugEcho("calling pauseGame from showWiiInstructions !%fromMenu" SPC getRealTime());
			pauseGame();
		}
		stopMenuMusic();
		Canvas.setCursor("0", noCursor);
	}
	return;
}
function menu_instructions::onOKPressed(%this)
{
	%this.showedPages = showedPages + 1.0;
	if ($WII)
	{
		%InstructionCount = "2 3 2 1";
		debugEcho("ok pressed:" SPC showedPages SPC "ïnstruction count" SPC getWord(%InstructionCount, showedInstructionIndex) SPC "index:" SPC showedInstructionIndex);
	}
	else
	{
		if ($settings::Controls::SixenseEnabled)
		{
			%InstructionCount = "2";
			break;
		}
		return;
	}
	if (showedPages < getWord(%InstructionCount, showedInstructionIndex))
	{
		pic_instructions.setBitmap("game/" @ $DataFolder @ "/images/hints/" @ $settings::Personal::Language @ "/" @ inputMode @ "_instructions_" @ showedPages + 1.0 @ ".png");
	}
	else
	{
		if (!(cameFromMenu))
		{
			if ($WII)
			{
				$settings::Wii::ShowedInstructions = setWord($settings::Wii::ShowedInstructions, showedInstructionIndex, "1");
			}
			if ($settings::Controls::SixenseEnabled)
			{
				$settings::Controls::ShowedInstructions = "1";
			}
			saveSettings($settings::Profile::currentProfile);
		}
		menu_instructions.back();
	}
	return;
}
function wiiInput::onUnpauseGame(%this)
{
	debugEcho("wiiInput onUnpauseGame...checking input" SPC %this);
	%this.checkInput();
	return;
}
function wiiInput::checkInput(%this)
{
	if (disconnectedNotification)
	{
		onWiimoteDisconnected();
		return;
	}
	if (isDead)
	{
		wiiInput.setInput($settings::Wii::WiiInputMode);
	}
	if (wiiInput.checkExtension())
	{
		showWiiInstructions();
		if (isDead)
		{
			wiiInput.setInput("basic");
		}
	}
	return;
}
function simulateStartRotation()
{
	camera.startRotation();
	return;
}
function simulateFinishRotation()
{
	camera.snapToClosestAngle();
	return;
}
function verbose()
{
	sixenseManager.setParam("verbose", "0");
	return;
}
function setupSixenseCalibration(%beforeSplashScreens)
{
	menu_sixenseCalibration.beforeSplashScreens = %beforeSplashScreens;
	if (isObject($ACTUAL_MENU))
	{
		$ACTUAL_MENU.enterMenu(menu_sixenseCalibration, "1");
	}
	else
	{
		Canvas.pushDialog(menu_sixenseCalibration);
	}
	return;
}
function skipSixenseCalibration()
{
	if (isCalibrating)
	{
		menu_sixenseCalibration.back();
		Canvas.popDialog(menu_sixenseCalibration);
	}
	return;
}
function menu_sixenseCalibration::onDialogPush(%this)
{
	sixenseManager.setInput("basic");
	sixenseManager.setEnabled("1");
	%this.isCalibrating = "1";
	setSixenseState("0", "0");
	setSixenseState("1", "0");
	if (sixenseManager.isControllerActive("0"))
	{
		setSixenseState("0", "1");
	}
	if (sixenseManager.isControllerActive("1"))
	{
		setSixenseState("1", "1");
	}
	sixenseManager.startCalibration();
	return;
}
function onSixenseConnected(%controllerNumber)
{
	echo("controller" SPC %controllerNumber SPC "connected");
	if (isCalibrating)
	{
		setSixenseState(%controllerNumber, "1");
	}
	return;
}
function onSixenseDisconnected(%controllerNumber)
{
	echo("controller" SPC %controllerNumber SPC "disconnected");
	if (isCalibrating)
	{
		setSixenseState(%controllerNumber, "0");
	}
	return;
}
function onSixenseCalibrated(%controllerNumber)
{
	echo("controller" SPC %controllerNumber SPC "calibrated");
	if (isCalibrating)
	{
		setSixenseState(%controllerNumber, "2");
	}
	return;
}
function onSixenseCalibrationEnded(%controllerNumber)
{
	echo("controller calibration ended!");
	sixenseManager.setInput("menu");
	if (!($splashScreenShown))
	{
		schedule("500", "0", "endSixenseCalibration");
	}
	else
	{
		endSixenseCalibration();
	}
	return;
}
function setSixenseState(%controller, %state)
{
	if (%controller == 0.0)
	{
		txt_sixenseCalibration_1_1_1.text = $lbl_sixense_activate_1;
		txt_sixenseCalibration_1_1_2.text = $lbl_sixense_activate_2;
		txt_sixenseCalibration_1_2_1.text = $lbl_sixense_calibrate_1;
		txt_sixenseCalibration_1_2_2.text = $lbl_sixense_calibrate_2;
		if (%state == 0.0)
		{
			pic_sixenseCalibration_1.setBitmap("game/data/images/Menu/sixense_controller_1_1.png");
			pic_sixenseCalibration_check_1_1.setVisible("0");
			txt_sixenseCalibration_1_1_1.setProfile("AyimMenuTextCenterProfile");
			txt_sixenseCalibration_1_1_2.setProfile("AyimMenuTextCenterProfile");
			pic_sixenseCalibration_check_1_2.setVisible("0");
			txt_sixenseCalibration_1_2_1.setProfile("AyimMenuTextCenterGreyProfile");
			txt_sixenseCalibration_1_2_2.setProfile("AyimMenuTextCenterGreyProfile");
		}
		else
		{
			if (%state == 1.0)
			{
				pic_sixenseCalibration_1.setBitmap("game/data/images/Menu/sixense_controller_1_2.png");
				pic_sixenseCalibration_check_1_1.setVisible("1");
				txt_sixenseCalibration_1_1_1.setProfile("AyimMenuTextCenterGreyProfile");
				txt_sixenseCalibration_1_1_2.setProfile("AyimMenuTextCenterGreyProfile");
				pic_sixenseCalibration_check_1_2.setVisible("0");
				txt_sixenseCalibration_1_2_1.setProfile("AyimMenuTextCenterProfile");
				txt_sixenseCalibration_1_2_2.setProfile("AyimMenuTextCenterProfile");
				break;
			}
			if (%state == 2.0)
			{
				pic_sixenseCalibration_1.setBitmap("game/data/images/Menu/sixense_controller_1_3.png");
				pic_sixenseCalibration_check_1_1.setVisible("1");
				txt_sixenseCalibration_1_1_1.setProfile("AyimMenuTextCenterGreyProfile");
				txt_sixenseCalibration_1_1_2.setProfile("AyimMenuTextCenterGreyProfile");
				pic_sixenseCalibration_check_1_2.setVisible("1");
				txt_sixenseCalibration_1_2_1.setProfile("AyimMenuTextCenterGreyProfile");
				txt_sixenseCalibration_1_2_2.setProfile("AyimMenuTextCenterGreyProfile");
			}
		}
	}
	else
	{
		txt_sixenseCalibration_2_1_1.text = $lbl_sixense_activate_1;
		txt_sixenseCalibration_2_1_2.text = $lbl_sixense_activate_2;
		txt_sixenseCalibration_2_2_1.text = $lbl_sixense_calibrate_1;
		txt_sixenseCalibration_2_2_2.text = $lbl_sixense_calibrate_2;
		if (%state == 0.0)
		{
			pic_sixenseCalibration_2.setBitmap("game/data/images/Menu/sixense_controller_2_1.png");
			pic_sixenseCalibration_check_2_1.setVisible("0");
			txt_sixenseCalibration_2_1_1.setProfile("AyimMenuTextCenterProfile");
			txt_sixenseCalibration_2_1_2.setProfile("AyimMenuTextCenterProfile");
			pic_sixenseCalibration_check_2_2.setVisible("0");
			txt_sixenseCalibration_2_2_1.setProfile("AyimMenuTextCenterGreyProfile");
			txt_sixenseCalibration_2_2_2.setProfile("AyimMenuTextCenterGreyProfile");
			break;
		}
		if (%state == 1.0)
		{
			pic_sixenseCalibration_2.setBitmap("game/data/images/Menu/sixense_controller_2_2.png");
			pic_sixenseCalibration_check_2_1.setVisible("1");
			txt_sixenseCalibration_2_1_1.setProfile("AyimMenuTextCenterGreyProfile");
			txt_sixenseCalibration_2_1_2.setProfile("AyimMenuTextCenterGreyProfile");
			pic_sixenseCalibration_check_2_2.setVisible("0");
			txt_sixenseCalibration_2_2_1.setProfile("AyimMenuTextCenterProfile");
			txt_sixenseCalibration_2_2_2.setProfile("AyimMenuTextCenterProfile");
			break;
		}
		if (%state == 2.0)
		{
			pic_sixenseCalibration_2.setBitmap("game/data/images/Menu/sixense_controller_2_3.png");
			pic_sixenseCalibration_check_2_1.setVisible("1");
			txt_sixenseCalibration_2_1_1.setProfile("AyimMenuTextCenterGreyProfile");
			txt_sixenseCalibration_2_1_2.setProfile("AyimMenuTextCenterGreyProfile");
			pic_sixenseCalibration_check_2_2.setVisible("1");
			txt_sixenseCalibration_2_2_1.setProfile("AyimMenuTextCenterGreyProfile");
			txt_sixenseCalibration_2_2_2.setProfile("AyimMenuTextCenterGreyProfile");
		}
	}
	return;
}
function endSixenseCalibration()
{
	if (isCalibrating)
	{
		menu_sixenseCalibration.back();
		Canvas.popDialog(menu_sixenseCalibration);
	}
	return;
}
function menu_sixenseCalibration::onDialogPop(%this)
{
	menu_sixenseCalibration.isCalibrating = "0";
	sixenseManager.setInput("menu");
	if (beforeSplashScreens)
	{
		%this.beforeSplashScreens = "0";
		$splashScreenObject.startSplashes("1");
	}
	return;
}
function showSixenseInstructions(%fromMenu)
{
	if (!($settings::Controls::SixenseEnabled))
	{
		return;
	}
	if (!(%fromMenu) && $settings::Controls::ShowedInstructions)
	{
		return;
	}
	if ($ACTUAL_MENU.getId() == menu_instructions.getId())
	{
		debugEcho("tried to show instructions again although we are already showing it");
		return;
	}
	%inputMode = "keyhole";
	menu_instructions.showedPages = "0";
	menu_instructions.showedInstructionIndex = %index;
	menu_instructions.inputMode = %inputMode;
	menu_instructions.cameFromMenu = %fromMenu;
	if (!(%fromMenu) && !(getIsGamePaused()))
	{
		debugEcho("calling pauseGame from showSixenseInstructions" SPC getRealTime());
		pauseGame();
	}
	pic_instructions.setBitmap("game/" @ $DataFolder @ "/images/hints/" @ $settings::Personal::Language @ "/keyhole_instructions.png");
	debugEcho("language for instructions are:" SPC $settings::Personal::Language);
	$ACTUAL_MENU.enterMenu(menu_instructions, "1");
	if (!(%fromMenu))
	{
		if (!(getIsGamePaused()))
		{
			debugEcho("calling pauseGame from showSixenseInstructions !%fromMenu" SPC getRealTime());
			pauseGame();
		}
		stopMenuMusic();
		Canvas.setCursor("0", noCursor);
	}
	return;
}
