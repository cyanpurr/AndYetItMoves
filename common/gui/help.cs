// help.cs.dso
function HelpDlg::onWake(%this)
{
	HelpFileList.entryCount = "0";
	HelpFileList.clear();
	%file = findFirstFile("*.hfl");
	while (%file != "")
	{
		HelpFileList.fileName[entryCount] = %file;
		HelpFileList.addRow(entryCount, fileBase(%file));
		HelpFileList.entryCount = entryCount + 1.0;
		%file = findNextFile("*.hfl");
	}
	HelpFileList.sortNumerical("0");
	%i = 0;
	while (%i < entryCount)
	{
		%rowId = HelpFileList.getRowId(%i);
		%text = HelpFileList.getRowTextById(%rowId);
		%text = %i + 1.0 @ ". " @ restWords(%text);
		HelpFileList.setRowById(%rowId, %text);
		%i = %i + 1.0;
	}
	HelpFileList.setSelectedRow("0");
	return;
}
function HelpFileList::onSelect(%this, %row)
{
	%fo = new FileObject(Name : "");
	%fo.openForRead(fileName[%row]);
	%text = "";
	while (!(%fo.isEOF()))
	{
		%text = %text @ %fo.readLine() @ "
";
	}
	%fo.delete();
	HelpText.setText(%text);
	return;
}
function getHelp(%helpName)
{
	Canvas.pushDialog(HelpDlg);
	if (%helpName != "")
	{
		%index = HelpFileList.findTextIndex(%helpName);
		HelpFileList.setSelectedRow(%index);
	}
	return;
}
function contextHelp()
{
	%i = 0;
	while (%i < Canvas.getCount())
	{
		if (Canvas.getObject(%i).getName() $= HelpDlg)
		{
			Canvas.popDialog(HelpDlg);
			return;
		}
		%i = %i + 1.0;
	}
	%content = Canvas.getContent();
	%helpPage = %content.getHelpPage();
	getHelp(%helpPage);
	return;
}
function GuiControl::getHelpPage(%this)
{
	return helpPage;
	return helpPage;
}
function GuiMLTextCtrl::onURL(%this, %url)
{
	gotoWebPage(%url);
	return;
}
