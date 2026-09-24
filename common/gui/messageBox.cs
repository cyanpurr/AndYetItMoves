// messageBox.cs.dso
function messageCallback(%dlg, %callback)
{
	Canvas.popDialog(%dlg);
	eval(%callback);
	return;
}
function MBSetText(%text, %frame, %msg)
{
	%ext = %text.getExtent();
	%text.setText("<just:center>" @ %msg);
	%text.forceReflow();
	%newExtent = %text.getExtent();
	%deltaY = getWord(%newExtent, "1") - getWord(%ext, "1");
	%windowPos = %frame.getPosition();
	%windowExt = %frame.getExtent();
	%frame.resize(getWord(%windowPos, "0"), getWord(%windowPos, "1") - %deltaY / 2.0, getWord(%windowExt, "0"), getWord(%windowExt, "1") + %deltaY);
	return;
}
function MessageBoxOK(%title, %message, %callback)
{
	MBOKFrame.setText(%title);
	Canvas.pushDialog(MessageBoxOKDlg);
	MBSetText(MBOKText, MBOKFrame, %message);
	MessageBoxOKDlg.callback = %callback;
	return;
}
function MessageBoxOKDlg::onSleep(%this)
{
	%this.callback = "";
	return;
}
function MessageBoxOKCancel(%title, %message, %callback, %cancelCallback)
{
	MBOKCancelFrame.setText(%title);
	Canvas.pushDialog(MessageBoxOKCancelDlg);
	MBSetText(MBOKCancelText, MBOKCancelFrame, %message);
	MessageBoxOKCancelDlg.callback = %callback;
	MessageBoxOKCancelDlg.CancelCallback = %cancelCallback;
	return;
}
function MessageBoxOKCancelDlg::onSleep(%this)
{
	%this.callback = "";
	return;
}
function messageBoxOkCancelDetails(%title, %message, %details, %callback, %cancelCallback)
{
	if (%details $= "")
	{
		MBOKCancelDetailsButton.setVisible("0");
	}
	MBOKCancelDetailsScroll.setVisible("0");
	MBOKCancelDetailsFrame.setText(%title);
	Canvas.pushDialog(MessageBoxOKCancelDetailsDlg);
	MBSetText(MBOKCancelDetailsText, MBOKCancelDetailsFrame, %message);
	MBOKCancelDetailsInfoText.setText(%details);
	%textExtent = MBOKCancelDetailsText.getExtent();
	%textExtentY = getWord(%textExtent, "1");
	%textPos = MBOKCancelDetailsText.getPosition();
	%textPosY = getWord(%textPos, "1");
	%extentY = %textPosY + %textExtentY + 65.0;
	MBOKCancelDetailsInfoText.setExtent("285", "128");
	MBOKCancelDetailsFrame.setExtent("300", %extentY);
	MessageBoxOKCancelDetailsDlg.callback = %callback;
	MessageBoxOKCancelDetailsDlg.CancelCallback = %cancelCallback;
	MBOKCancelDetailsFrame.defaultExtent = MBOKCancelDetailsFrame.getExtent();
	return;
}
function MBOKCancelDetailsToggleInfoFrame()
{
	if (!(MBOKCancelDetailsScroll.isVisible()))
	{
		MBOKCancelDetailsScroll.setVisible("1");
		%textExtent = MBOKCancelDetailsText.getExtent();
		%textExtentY = getWord(%textExtent, "1");
		%textPos = MBOKCancelDetailsText.getPosition();
		%textPosY = getWord(%textPos, "1");
		%posY = %textPosY + %textExtentY + 10.0;
		%posX = getWord(MBOKCancelDetailsScroll.getPosition(), "0");
		MBOKCancelDetailsScroll.setPosition(%posX, %posY);
		MBOKCancelDetailsFrame.MinExtent = "300" SPC %textExtentY + 260.0;
		MBOKCancelDetailsFrame.setExtent("300", %textExtentY + 260.0);
	}
	else
	{
		%extent = defaultExtent;
		%width = getWord(%extent, "0");
		%height = getWord(%extent, "1");
		MBOKCancelDetailsFrame.MinExtent = %width SPC %height;
		MBOKCancelDetailsFrame.setExtent(%width, %height);
		MBOKCancelDetailsScroll.setVisible("0");
	}
	return;
}
function MessageBoxOKCancelDetailsDlg::onSleep(%this)
{
	%this.callback = "";
	return;
}
function MessageBoxYesNo(%title, %message, %yesCallback, %noCallback)
{
	MBYesNoFrame.setText(%title);
	Canvas.pushDialog(MessageBoxYesNoDlg);
	MBSetText(MBYesNoText, MBYesNoFrame, %message);
	MessageBoxYesNoDlg.yesCallBack = %yesCallback;
	MessageBoxYesNoDlg.noCallback = %noCallback;
	return;
}
function MessageBoxYesNoDlg::onSleep(%this)
{
	%this.yesCallBack = "";
	%this.noCallback = "";
	return;
}
function MessageBoxYesNoCancel(%title, %message, %yesCallback, %noCallback, )
{
	MBYesNoCancelFrame.setText(%title);
	Canvas.pushDialog(MessageBoxYesNoCancelDlg);
	MBSetText(MBYesNoCancelText, MBYesNoCancelFrame, %message);
	MessageBoxYesNoCancelDlg.yesCallBack = %yesCallback;
	MessageBoxYesNoCancelDlg.noCallback = %noCallback;
	MessageBoxYesNoCancelDlg.CancelCallback = %cancelCallback;
	return;
}
function MessageBoxYesNoCancelDlg::onSleep(%this)
{
	%this.yesCallBack = "";
	%this.noCallback = "";
	%this.CancelCallback = "";
	return;
}
function MessagePopup(%title, %message, %delay)
{
	MessagePopFrame.setText(%title);
	Canvas.pushDialog(MessagePopupDlg);
	MBSetText(MessagePopText, MessagePopFrame, %message);
	if (%delay != "")
	{
		schedule(%delay, "0", CloseMessagePopup);
	}
	return;
}
function CloseMessagePopup()
{
	Canvas.popDialog(MessagePopupDlg);
	return;
}
