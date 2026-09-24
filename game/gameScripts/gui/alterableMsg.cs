// alterableMsg.cs.dso
function menu_oneLineWarning::Show(%this, %textLine1, %textLine2)
{
	%this.setLine1(%textLine1);
	%this.setLine2(%textLine2);
	Canvas.pushDialog(%this);
	return;
}
function menu_oneLineWarning::Hide(%this)
{
	Canvas.popDialog(%this);
	return;
}
function menu_oneLineWarning::setLine1(%this, %text)
{
	lbl_oneLineWarning_text_1.text = %text;
	return;
}
function menu_oneLineWarning::setLine2(%this, %text)
{
	lbl_oneLineWarning_text_2.text = %text;
	return;
}
function menu_dialog::Show(%this, %title, %text, %btnText1, %btnText2, %checkBox, %cmdID, %btn1Command, %btn2Command, %dontUseEnter, %dontUseEscape)
{
	lbl_dialog_title.text = %title;
	%this.setText(%text);
	%this.setButton1(%btnText1, %btn1Command);
	%this.setButton2(%btnText2, %btn2Command);
	%this.setCheckbox(%checkBox);
	%this.cmdID = %cmdID;
	if (%cmdID $= "newversion")
	{
		chb_dialog_choice.setValue("0");
	}
	%this.dontUseEnter = %dontUseEnter;
	%this.dontUseEscape = %dontUseEscape;
	debugEcho("showingDialog:" SPC %title NL %text NL %btnText1 NL %btnText2 NL %checkBox NL %cmdID NL %btn1Command NL %btn2Command NL %dontUseEnter NL %dontUseEscape);
	if (isObject($ACTUAL_MENU))
	{
		$ACTUAL_MENU.enterMenu(menu_dialog, "1");
	}
	else
	{
		Canvas.pushDialog(%this);
	}
	return;
}
function menu_dialog::onDialogPop(%this)
{
	%i = 0;
	while (%i < getWordCount(tempElements))
	{
		if (isObject(getWord(tempElements, %i)))
		{
			getWord(tempElements, %i).delete();
		}
		%i = %i + 1.0;
	}
	%this.tempElements = "";
	debugEcho("poppig menu_dialog" SPC previousMenu);
	if (previousMenu.getId() == menu_main.getId())
	{
		menu_main.schedule("300", "checkUpdateNews");
	}
	return;
}
function menu_dialog::setText(%this, %text)
{
	lbl_dialog_question.text = getRecord(%text, "0");
	%textPos = lbl_dialog_question.getPosition();
	%textExtent = lbl_dialog_question.getExtent();
	%i = 1;
	while (%i < getRecordCount(%text))
	{
		%newLine = new GuiTextCtrl(Name : "")
		{
			canSaveDynamicFields = "0";
			isContainer = "0";
			Profile = "AyimMenuTextProfile";
			HorizSizing = "right";
			VertSizing = "bottom";
			Position = getX(%textPos) SPC getY(%textPos) + getY(%textExtent) * %i;
			Extent = "448 32";
			MinExtent = "8 2";
			canSave = "1";
			Visible = "1";
			hovertime = "1000";
			text = getRecord(%text, %i);
			maxLength = "1024";
		}
		ctrl_dialog_main.addGuiControl(%newLine);
		%this.tempElements = trim(tempElements SPC %newLine);
		%i = %i + 1.0;
	}
	return getRecordCount(%text);
}
function menu_dialog::setButton1(%this, %text, %command)
{
	btn_dialog_1.text = %text;
	if (%command != "")
	{
		btn_dialog_1.Command = %command;
	}
	btn_dialog_1.setVisible(%text != "");
	return;
}
function menu_dialog::setButton2(%this, %text, %command)
{
	btn_dialog_2.text = %text;
	if (%command != "")
	{
		btn_dialog_2.Command = %command;
	}
	btn_dialog_2.setVisible(%text != "");
	return;
}
function menu_dialog::setCheckbox(%this, %text)
{
	chb_dialog_choice.text = %text;
	chb_dialog_choice.setVisible(%text != "");
	return;
}
function btn_dialog_1::onAction(%this)
{
	debugEcho("btn1 onAction" SPC cmdID);
	if (cmdID $= "newversion")
	{
	}
	else
	{
		menu_dialog.back();
	}
	return;
}
function btn_dialog_2::onAction(%this)
{
	if (cmdID $= "newversion")
	{
		if (chb_dialog_choice.getValue())
		{
			$settings::Other::VersionNotified = serverVersion;
			saveSettings($settings::Profile::currentProfile);
		}
		menu_dialog.back();
	}
	else
	{
		if (cmdID $= "fullscreennotify")
		{
			if (chb_dialog_choice.getValue())
			{
				$settings::Status::FullscreenNotify = $currentVersion;
				saveSettings(-1.0);
			}
			menu_dialog.back();
			break;
		}
		menu_dialog.back();
	}
	return;
}
function menu_dialog::onEnterPressed(%this)
{
	if (dontUseEnter)
	{
		return %this;
	}
	if (Command $= "")
	{
		if (cmdID $= "")
		{
			%this.back();
		}
		else
		{
			btn_dialog_2.onAction();
		}
	}
	else
	{
		eval(Command);
	}
	return;
}
function menu_dialog::onEscapePressed(%this)
{
	if (dontUseEscape)
	{
		return %this;
	}
	if (Command $= "")
	{
		if (cmdID $= "")
		{
			%this.back();
		}
		else
		{
			btn_dialog_1.onAction();
		}
	}
	else
	{
		eval(Command);
	}
	return;
}
function menu_dialog::openWebPage(%this, %url, %quitAfterClick)
{
	debugEcho("going to this website" SPC %url SPC "quitAfter?" SPC %quitAfterClick);
	%this.back();
	gotoWebPage(%url);
	if (%quitAfterClick)
	{
		MenuAction::quit();
	}
	return;
}
