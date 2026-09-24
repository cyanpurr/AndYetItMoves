// options.cs.dso
function tab_options_joystick::onAdd(%this)
{
	%this.setVisible("0");
	return;
}
function tab_options_keyboard::onAdd(%this)
{
	%this.setVisible("0");
	return;
}
function tab_options_video::onAdd(%this)
{
	%this.setVisible("0");
	return;
}
function menu_options::onDialogPush(%this)
{
	sld_audio_music.setValue($settings::Audio::Music);
	sld_audio_effects.setValue($settings::Audio::Effects);
	debugEcho("showing menu_options tabsel:" SPC optionsTabSelected);
	if (optionsTabSelected $= "")
	{
		menu_main.optionsTabSelected = "joystick";
	}
	%tabButton = "tablbl_options_" @ optionsTabSelected;
	%tabButton.displayOptions(optionsTabSelected);
	if ($WII)
	{
		if (straightToGame)
		{
			btn_options_cancel.setVisible("0");
		}
		else
		{
			btn_options_cancel.setVisible("1");
		}
		%index = getWordIndex($ALL_WII_INPUT_MODES, $settings::Wii::WiiInputMode);
		menu_options.sensitivity = $settings::Controls::Sensitivity;
		menu_options.inverted = $settings::Controls::Inverted;
		debugEcho("loaded wii otopins sens:" SPC sensitivity SPC "inv:" SPC inverted);
		menu_options.setSensitivityButton(getWord(sensitivity, %index));
		chb_options_invert.setValue(getWord(inverted, %index));
		menu_options.save = "0";
		wiiInput.setInput("menu");
		Canvas.showCursor("0");
		Canvas.setCursor("0", ayimCursor);
		return;
	}
	if (isSixensePossible())
	{
		%index = 0;
		menu_options.sensitivity = $settings::Controls::Sensitivity;
		menu_options.inverted = $settings::Controls::Inverted;
		debugEcho("loaded sixense otopins sens:" SPC sensitivity SPC "inv:" SPC inverted);
		menu_options.setSensitivityButton(getWord(sensitivity, %index));
		chb_options_invert.setValue(getWord(inverted, %index));
		%this.refreshSixense();
	}
	if (setDevice $= "")
	{
		%this.setDevice = "keyboard";
	}
	$MODIFIED_KEYBOARD_LAYOUT = $settings::Controls::KeyboardLayout;
	$MODIFIED_JOYSTICK_LAYOUT = $settings::Controls::JoystickLayout;
	%this.displayInput();
	%this.selectedScreenMode = $settings::Video::Fullscreen;
	if ($settings::Video::Fullscreen)
	{
	}
	else
	{
	}
	%this.selectedResolution = $settings::Video::Window;
	%this.getAvailableResolutions();
	MenuAction::toggleFullScreen(selectedScreenMode);
	%this.setResolutionIndex(getFieldIndex(actualResolutionList, selectedResolution));
	%this.performanceLevel = $settings::Performance::Level;
	if (performanceLevel $= "custom")
	{
		%this.shaders = $settings::Performance::Shaders;
		%this.verticalSync = $settings::Performance::VerticalSync;
		%this.layers = $settings::Performance::Layers;
	}
	else
	{
		eval("rd_performance_" @ performanceLevel @ ".setStateOn(true);");
	}
	return;
}
function menu_options::back(%this)
{
	if (!(straightToGame))
	{
		GuiControl::back(%this);
	}
	return;
}
function menu_options::onDialogPop(%this)
{
	cancelInputAssignment();
	return;
}
function menu_options::onEnterPressed(%this)
{
	btn_options_ok.onAction();
	return;
}
function menu_options::onTabSelected(%this, %tab)
{
	%oldTab = "tab_options_" @ optionsTabSelected;
	%oldTab.setVisible("0");
	menu_main.optionsTabSelected = %tab;
	%newTab = "tab_options_" @ %tab;
	%newTab.setVisible("1");
	if (%tab $= "joystick")
	{
		%this.displayInput("joystick");
		if ($WII)
		{
			menu_options.showWiiInputModes();
		}
		else
		{
			RebindJoysticks();
		}
	}
	else
	{
		if (%tab $= "keyboard")
		{
			%this.displayInput("keyboard");
			break;
		}
		if (%tab $= "sixense")
		{
			%this.refreshSixense();
		}
	}
	return;
}
function menu_options::showWiiInputModes(%this)
{
	debugEcho("showing wiiInputModes" SPC %this);
	inputButton.setActive("0");
	inputButton.setText("");
	inputButton.setActive("0");
	inputButton.setText("");
	inputButton.setActive("0");
	inputButton.setText("");
	inputButton.setActive("0");
	inputButton.setText("");
	inputText.setVisible("0");
	inputText.setVisible("0");
	inputText.setVisible("0");
	inputText.setVisible("0");
	inputText.setText($lbl_wii_disconnect_extension_title);
	inputText.setText($lbl_nunchuk SPC $lbl_wii_connect_extension_title);
	inputText.setText($lbl_nunchuk SPC $lbl_wii_connect_extension_title);
	inputText.setText($lbl_classicController SPC $lbl_wii_connect_extension_title);
	if (wiiInput.getConnectedExtension() $= "wiimote")
	{
		inputButton.setActive("1");
		inputButton.text = $lbl_options_controller_driver;
		MenuAction::setWiiInput("driver");
		inputText.setVisible("1");
		inputText.setVisible("1");
		inputText.setVisible("1");
	}
	else
	{
		if (wiiInput.getConnectedExtension() $= "nunchuck")
		{
			inputButton.setActive("1");
			inputButton.text = $lbl_options_controller_keyhole;
			inputButton.setActive("1");
			inputButton.text = $lbl_options_controller_pointer;
			MenuAction::setWiiInput($settings::Wii::NunchuckMode);
			inputText.setVisible("1");
			inputText.setVisible("1");
			break;
		}
		if (wiiInput.getConnectedExtension() $= "classic")
		{
			inputButton.setActive("1");
			inputButton.text = $lbl_options_controller_classic;
			MenuAction::setWiiInput("classic");
			inputText.setVisible("1");
			inputText.setVisible("1");
			inputText.setVisible("1");
			break;
		}
		inputText.setVisible("1");
		inputText.setVisible("1");
		inputText.setVisible("1");
		inputText.setVisible("1");
	}
	return;
}
function menu_options::saveInput(%this)
{
	if (!($WII))
	{
		configureInput("keyboard", $MODIFIED_KEYBOARD_LAYOUT, "1");
		configureInput("joystick", $MODIFIED_JOYSTICK_LAYOUT);
		$settings::Controls::Sensitivity = sensitivity;
		$settings::Controls::Inverted = inverted;
		%index = 0;
		sixenseManager.setSensitivity(getWord($settings::Controls::Sensitivity, %index));
		sixenseManager.setInverted(getWord($settings::Controls::Inverted, %index));
		saveSettings($settings::Profile::currentProfile);
	}
	if ($WII)
	{
		%index = getWordIndex($ALL_WII_INPUT_MODES, $settings::Wii::WiiInputMode);
		if (sensitivity != $settings::Controls::Sensitivity)
		{
			$settings::Controls::Sensitivity = sensitivity;
			menu_options.save = "1";
			debugEcho("saving sensitivity! called this on wiiInput" SPC getWord($settings::Controls::Sensitivity, %index));
		}
		if (inverted != $settings::Controls::Inverted)
		{
			$settings::Controls::Inverted = inverted;
			menu_options.save = "1";
			debugEcho("saving inverted! called this on wiiInput" SPC getWord($settings::Controls::Inverted, %index));
		}
		wiiInput.setSensitivity(getWord($settings::Controls::Sensitivity, %index));
		wiiInput.setInverted(getWord($settings::Controls::Inverted, %index));
		debugEcho("new sensitivity is" SPC $settings::Controls::Sensitivity SPC "inv" SPC $settings::Controls::Inverted);
	}
	return;
}
function menu_options::saveVideo(%this)
{
	if ($WII)
	{
		debugEcho("WARNING:menu_options::saveVideo: we are on wii thus should never be here. WHY?" SPC %this);
		return;
	}
	if ($settings::Video::Fullscreen)
	{
	}
	else
	{
	}
	%savedResolution = $settings::Video::Window;
	%this.oldResListIndex = getFieldIndex(actualResolutionList, %savedResolution);
	%save = 0;
	if ($settings::Video::Fullscreen != selectedScreenMode || selectedResolution != %savedResolution)
	{
		%resolutionChanged = 1;
		echo("setting new res in saveVideo to" SPC selectedResolution SPC "sM:" SPC selectedScreenMode);
		setNewResolution(getX(selectedResolution), getY(selectedResolution), "32", selectedScreenMode);
		if (selectedScreenMode)
		{
			menu_dialog.Show($lbl_resChanged, $lbl_resChangedQuestion, $btn_resChangedCancel, $btn_resChangedOk, "", "", "menu_options.cancelResolution();", "menu_options.saveResolution();", "1");
			%this.resolutionScheduleID = %this.schedule("15000", "cancelResolution");
		}
		else
		{
			menu_options.saveResolution();
		}
		Canvas.showCursor();
	}
	if (performanceLevel $= "custom" || performanceLevel != $settings::Performance::Level)
	{
		$settings::Performance::Level = performanceLevel;
		%save = 1;
		if (performanceLevel $= "low")
		{
			%settings = getField($performanceLevels, "0");
		}
		else
		{
			if (performanceLevel $= "medium")
			{
				%settings = getField($performanceLevels, "1");
				break;
			}
			if (performanceLevel $= "high")
			{
				%settings = getField($performanceLevels, "2");
				break;
			}
			if (performanceLevel $= "custom")
			{
				echo("custom" SPC shaders SPC verticalSync SPC layers);
				%settings = shaders SPC verticalSync SPC layers;
			}
		}
		if ($settings::Performance::Shaders != getWord(%settings, "0"))
		{
			$settings::Performance::Shaders = getWord(%settings, "0");
			setUseShader(getWord(%settings, "0"));
		}
		if ($settings::Performance::VerticalSync != getWord(%settings, "1"))
		{
			$settings::Performance::VerticalSync = getWord(%settings, "1");
			setVerticalSync(getWord(%settings, "1"));
		}
		if (%save)
		{
			saveSettings($settings::Profile::currentProfile);
		}
	}
	return %resolutionChanged;
	return %resolutionChanged;
}
function menu_options::cancelResolution(%this, %autoFullscreen)
{
	cancel(resolutionScheduleID);
	if (%autoFullscreen)
	{
		$settings::Video::Fullscreen = 1;
	}
	if ($settings::Video::Fullscreen)
	{
	}
	else
	{
	}
	%savedResolution = $settings::Video::Window;
	echo("resetting resolution to" SPC %savedResolution SPC "sM:" SPC $settings::Video::Fullscreen);
	setNewResolution(getX(%savedResolution), getY(%savedResolution), "32", $settings::Video::Fullscreen);
	%this.selectedScreenMode = $settings::Video::Fullscreen;
	%this.setResolutionIndex(oldResListIndex);
	if (%autoFullscreen)
	{
		Canvas.popDialog(menu_dialog);
		$splashScreenObject.startSplashes("1");
	}
	else
	{
		$ACTUAL_MENU.back();
	}
	return;
}
function menu_options::saveResolution(%this, %autoFullscreen)
{
	cancel(resolutionScheduleID);
	if (%autoFullscreen)
	{
		Canvas.popDialog(menu_dialog);
		$splashScreenObject.startSplashes("1");
		return;
	}
	$settings::Video::Fullscreen = selectedScreenMode;
	if (isFullScreen())
	{
		$settings::Video::Full = selectedResolution;
	}
	else
	{
		$settings::Video::Window = selectedResolution;
	}
	debugEcho("saved res:" SPC $settings::Video::Window SPC "full?" SPC $settings::Video::Fullscreen SPC "fullRes:" SPC $settings::Video::Full);
	if (!($WII))
	{
		saveSettings($settings::Profile::currentProfile);
	}
	$ACTUAL_MENU.back();
	menu_options.back();
	return;
}
function menu_options::saveAudio(%this)
{
	%save = 0;
	if ($settings::Audio::Effects != sld_audio_effects.getValue())
	{
		$settings::Audio::Effects = sld_audio_effects.getValue();
		%save = 1;
	}
	if ($settings::Audio::Music != sld_audio_music.getValue())
	{
		$settings::Audio::Music = sld_audio_music.getValue();
		%save = 1;
	}
	if (%save)
	{
		if ($WII)
		{
			menu_options.save = "1";
			break;
		}
		saveSettings($settings::Profile::currentProfile);
	}
	return;
}
function menu_options::setResolutonList(%this)
{
	if (selectedScreenMode)
	{
		%this.actualResolutionList = resolutionsFullscreen;
	}
	else
	{
		%this.actualResolutionList = resolutionsWindowed;
	}
	%this.selectedResIndex = getFieldIndex(actualResolutionList, selectedResolution);
	if (selectedResIndex == -1.0)
	{
		if (selectedScreenMode)
		{
			%defaultRes = $settings::Video::Full;
		}
		else
		{
			%defaultRes = $settings::Video::Window;
		}
		%this.selectedResIndex = getFieldIndex(actualResolutionList, %defaultRes);
	}
	%this.setResolutionIndex(selectedResIndex);
	return;
}
function menu_options::setResolutionIndex(%this, %index)
{
	%this.selectedResIndex = negModulo(%index, getFieldCount(actualResolutionList));
	%res = getField(actualResolutionList, selectedResIndex);
	%this.selectedResolution = %res;
	if (selectedScreenMode)
	{
	}
	else
	{
	}
	%lbl_res_prefix = $lbl_video_window;
	lbl_video_resolution.text = %lbl_res_prefix SPC $lbl_video_resolution SPC selectedResIndex + 1.0 SPC "/" SPC getFieldCount(actualResolutionList);
	lbl_video_actualResolution.text = getX(%res) SPC "x" SPC getY(%res);
	if (selectedScreenMode)
	{
		btn_video_fullscreenOn.setStateOn("1");
	}
	else
	{
		btn_video_fullscreenOff.setStateOn("1");
	}
	return;
}
function menu_options::getAvailableResolutions(%this)
{
	%allResolutions = getResolutionList("OpenGL");
	%currentResolution = $STARTUP_DESKTOP_RESOLUTION;
	%currentWidth = getX(%currentResolution);
	%currentHeight = getY(%currentResolution);
	%currentBitDepth = getZ(%currentResolution);
	%currentRatio = %currentWidth / %currentHeight;
	%resNr = getFieldCount(%allResolutions);
	%this.resolutionsFullscreen = "";
	%this.resolutionsWindowed = "";
	if ($WII)
	{
		%minHeight = 480;
	}
	else
	{
		%minHeight = 600;
	}
	%r = 0;
	while (%r < %resNr)
	{
		%actualDeviceRes = getField(%allResolutions, %r);
		if (getZ(%actualDeviceRes != %currentBitDepth))
		{
		}
		else
		{
			%actRes = getWords(%actualDeviceRes, "0", "1");
			if (%resolutionChecked[%actRes])
			{
				while ()
				{
					while ()
					{
					}
					%resolutionChecked[%actRes] = 1;
				}
				%width = getX(%actRes);
				%height = getY(%actRes);
				%ratio = %width / %height;
				if (%ratio > 16.0 / 9.0)
				{
					%ratio = 16.0 / 9.0;
				}
				if (mAbs(%currentRatio - %ratio) < 0.009999999776482582 && %height >= %minHeight)
				{
					%this.resolutionsFullscreen = ['resolutionsFullscreen', '%actRes'] TAB "";
				}
				if (%width <= %currentWidth && %height <= %currentHeight && %height >= %minHeight && mAbs(%ratio - 4.0 / 3.0) < 0.009999999776482582 || mAbs(%ratio - 16.0 / 10.0) < 0.009999999776482582 || mAbs(%ratio - 16.0 / 9.0) < 0.009999999776482582)
				{
					%this.resolutionsWindowed = ['resolutionsWindowed', '%actRes'] TAB "";
				}
			}
		}
		%r = %r + 1.0;
	}
	%this.resolutionsFullscreen = trim(resolutionsFullscreen);
	%this.resolutionsWindowed = trim(resolutionsWindowed);
	if ($enableDebugMap)
	{
		%this.resolutionsWindowed = resolutionsWindowed TAB "640 480" TAB "854 480";
	}
	debugEcho("filtered windowed resolutions:  " SPC resolutionsWindowed);
	debugEcho("filtered fullscreen resolutions:" SPC resolutionsFullscreen);
	return;
}
function menu_options::displayInput(%this, %device)
{
	if (%device $= "")
	{
		%device = setDevice;
	}
	if (isObject($CONTROLS_ASSIGNBUTTON_ACTIVE))
	{
		$CONTROLS_ASSIGNBUTTON_ACTIVE.setBitmap($BTN_SMALL_NORMAL);
		$CONTROLS_ASSIGNBUTTON_ACTIVE = "";
	}
	if (%device $= "keyboard")
	{
		%inputList = $MODIFIED_KEYBOARD_LAYOUT;
	}
	else
	{
		if (%device $= "joystick")
		{
			%inputList = $MODIFIED_JOYSTICK_LAYOUT;
			break;
		}
		return;
	}
	%this.setDevice = %device;
	%i = 0;
	while (%i < getWordCount($ALL_GAME_ACTIONS))
	{
		%action = getWord($ALL_GAME_ACTIONS, %i);
		%input = getWord(%inputList, %i);
		if (%input $= "")
		{
			%input = "-";
		}
		%menuButton = "btn_options_" @ %device @ "_" @ %action;
		if ($settings::Personal::Language $= "japanese")
		{
			if (%input $= "left")
			{
				%input = "â";
			}
			if (%input $= "right")
			{
				%input = "â";
			}
			if (%input $= "up")
			{
				%input = "â";
			}
			if (%input $= "down")
			{
				%input = "â";
			}
		}
		%menuButton.text = %input;
		if (isObject(%menuButton))
		{
			if (strlen(%input) <= 5.0)
			{
				%menuButton.setProfile("AyimButtonProfile");
				break;
			}
			%menuButton.setProfile("AyimButtonSmallTextProfile");
		}
		%i = %i + 1.0;
	}
	return getWordCount($ALL_GAME_ACTIONS);
}
function PerformanceRadioButton::onAction(%this)
{
	%type = getLastToken(%this.getName(), "_");
	debugEcho(%type SPC "has been selected" SPC %this);
	menu_options.performanceLevel = %type;
	return;
}
function menu_options::showCustom(%this)
{
	$ACTUAL_MENU.enterMenu(menu_custom, "1");
	if (performanceLevel $= "low")
	{
		%settings = getField($performanceLevels, "0");
	}
	else
	{
		if (performanceLevel $= "medium")
		{
			%settings = getField($performanceLevels, "1");
			break;
		}
		if (performanceLevel $= "high")
		{
			%settings = getField($performanceLevels, "2");
			break;
		}
		if (performanceLevel $= "custom")
		{
			%settings = shaders SPC verticalSync SPC layers;
		}
	}
	if (performanceLevel != "custom")
	{
		%this.shaders = getWord(%settings, "0");
		%this.verticalSync = getWord(%settings, "1");
		%this.layers = getWord(%settings, "2");
	}
	menu_custom.checkBoxChecked = "0";
	if (getShaderSupport())
	{
		chb_custom_shaders.setStateOn(getWord(%settings, "0"));
	}
	else
	{
		chb_custom_shaders.setStateOn("0");
		chb_custom_shaders.setActive("0");
	}
	chb_custom_vSync.setStateOn(getWord(%settings, "1"));
	chb_custom_decoLayers.setStateOn(getWord(%settings, "2"));
	return;
}
function CustomPerformanceButton::onAction(%this)
{
	%function = getLastToken(%this.getName(), "_");
	menu_custom.checkBoxChecked = "1";
	if (%function $= "shaders")
	{
		menu_options.shaders = !(shaders);
	}
	else
	{
		if (%function $= "vSync")
		{
			menu_options.verticalSync = !(verticalSync);
			break;
		}
		if (%function $= "decoLayers")
		{
			menu_options.layers = !(layers);
		}
	}
	return;
}
function menu_custom::onEnterPressed(%this)
{
	btn_custom_ok.onAction();
	return;
}
function AudioSlider::onMouseDragged(%this)
{
	if (getLastToken(%this.getName(), "_") $= "effects")
	{
		$FXAudioChannel.setVolume(%this.getValue());
	}
	else
	{
		if (getLastToken(%this.getName(), "_") $= "music")
		{
			$MenuAudioChannel.setVolume(%this.getValue());
			$MusicAudioChannel["1"].setVolume(%this.getValue());
			$MusicAudioChannel["2"].setVolume(%this.getValue());
			$MusicAudioChannel["3"].setVolume(%this.getValue());
		}
	}
	return;
}
function ControlsButton::onAction(%this)
{
	%button = %this;
	%type = getLastToken(%this.getName(), "_");
	if ($CONTROLS_ASSIGNBUTTON_ACTIVE $= %button)
	{
		cancelInputAssignment();
		menu_options.displayInput();
		return;
	}
	menu_options.displayInput();
	$CONTROLS_ASSIGNBUTTON_ACTIVE = %button;
	%button.setBitmap($BTN_SMALL_ACTIVE);
	listenToDevice(setDevice, %type);
	return;
}
function InputSelectButton::onAdd(%this)
{
	%inputMode = getLastToken(%this.getName(), "_");
	%this.inputButton = new GuiBitmapButtonTextCtrl(Name : "")
	{
		canSaveDynamicFields = "1";
		class = "";
		Profile = "AyimInputButtonProfile";
		HorizSizing = "center";
		VertSizing = "center";
		Position = "0 0";
		Extent = "172 116";
		Visible = "1";
		AlwaysUseMouseEvents = "1";
		Command = ['"MenuAction::setWiiInput("', '%inputMode', '");"'];
		bitmap = ['"~/"', '$DataFolder', '"/images/Menu/input/overlay_active"'];
		UseMouseEvents = "1";
	}
	%this.inputText = new GuiMLTextCtrl(Name : "")
	{
		canSaveDynamicFields = "0";
		isContainer = "0";
		Profile = "AyimMenuTextCenterProfile";
		HorizSizing = "center";
		VertSizing = "bottom";
		SizeMargin = "0 0";
		PositionAbsolute = "0";
		Position = "25 35";
		Extent = "105 40";
		MinExtent = "8 2";
		canSave = "1";
		Visible = "1";
		hovertime = "1000";
		lineSpacing = "10";
		allowColorChars = "0";
		maxChars = "-1";
		text = "play on NDEV to select this!";
	}
	%this.infoButton = new GuiBitmapButtonCtrl(Name : "")
	{
		canSaveDynamicFields = "1";
		class = "";
		Profile = "AyimPlaymodeButtonProfile";
		HorizSizing = "right";
		VertSizing = "bottom";
		Position = "123 0";
		Extent = "49 49";
		Visible = "1";
		AlwaysUseMouseEvents = "1";
		Command = ['"showWiiInstructions("', '%inputMode', '");"'];
		bitmap = ['"~/"', '$DataFolder', '"/images/Menu/input/info"'];
		UseMouseEvents = "1";
	}
	%this.Command = "";
	%this.addGuiControl(inputButton);
	%this.addGuiControl(inputText);
	inputButton.addGuiControl(infoButton);
	return;
}
function SensitivityRadioButton::onAction(%this)
{
	%type = getLastToken(%this.getName(), "_");
	debugEcho("SENSITIVITY:" SPC %type SPC "has been selected" SPC %this);
	if ($WII)
	{
		%index = getWordIndex($ALL_WII_INPUT_MODES, $settings::Wii::WiiInputMode);
	}
	else
	{
		%index = 0;
	}
	if (%type $= "low")
	{
		menu_options.sensitivity = setWord(sensitivity, %index, "0");
	}
	else
	{
		if (%type $= "medium")
		{
			menu_options.sensitivity = setWord(sensitivity, %index, "1");
			break;
		}
		if (%type $= "high")
		{
			menu_options.sensitivity = setWord(sensitivity, %index, "2");
		}
	}
	return;
}
function menu_options::setSensitivityButton(%this, %sensitivity)
{
	if (%sensitivity == 0.0)
	{
		rd_sensitivity_low.setStateOn("1");
	}
	else
	{
		if (%sensitivity == 1.0)
		{
			rd_sensitivity_medium.setStateOn("1");
			break;
		}
		if (%sensitivity == 2.0)
		{
			rd_sensitivity_high.setStateOn("1");
		}
	}
	return;
}
function chb_options_invert::onAction(%this, )
{
	debugEcho("INVERTED:" SPC chb_options_invert.getValue() SPC "has been selected. current input is" SPC $settings::Wii::WiiInputMode);
	if ($WII)
	{
		%index = getWordIndex($ALL_WII_INPUT_MODES, $settings::Wii::WiiInputMode);
	}
	else
	{
		%index = 0;
	}
	menu_options.inverted = setWord(inverted, %index, chb_options_invert.getValue());
	return;
}
function menu_options::refreshSixense(%this)
{
	sixenseManager.setEnabled($settings::Controls::SixenseEnabled);
	if ($settings::Controls::SixenseEnabled)
	{
		tab_options_sixense.setAllChildrenActive("1");
		btn_sixense_enable.text = $lbl_sixense_disable;
	}
	else
	{
		tab_options_sixense.setAllChildrenActive("0");
		btn_sixense_enable.text = $lbl_sixense_enable;
		btn_sixense_enable.setActive("1");
	}
	lbl_sixense_nr.text = sixenseManager.getActiveControllers();
	return;
}
function btn_sixense_enable::onAction(%this)
{
	$settings::Controls::SixenseEnabled = !($settings::Controls::SixenseEnabled);
	menu_options.refreshSixense();
	return;
}
