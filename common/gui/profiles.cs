// profiles.cs.dso
$Gui::fontCacheDirectory = expandFilename("~/data/fonts");
if (!(isObject(GuiDefaultProfile)))
{
	new GuiControlProfile(Name : GuiDefaultProfile)
	{
		tab = "0";
		canKeyFocus = "0";
		hasBitmapArray = "0";
		mouseOverSelected = "0";
		opaque = "0";
		fillColor = "211 211 211";
		fillColorHL = "244 244 244";
		fillColorNA = "244 244 244";
		border = "1";
		borderColor = "40 40 40 100";
		borderColorHL = "128 128 128";
		borderColorNA = "64 64 64";
		fontType = "Arial";
		fontSize = "14";
		fontColor = "0 0 0";
		fontColorHL = "32 100 100";
		fontColorNA = "0 0 0";
		fontColorSEL = "200 200 200";
		bitmap = "./images/window";
		bitmapBase = "";
		textOffset = "0 0";
		Modal = "1";
		justify = "left";
		autoSizeWidth = "0";
		autoSizeHeight = "0";
		returnTab = "0";
		numbersOnly = "0";
		cursorColor = "0 0 0 255";
		soundButtonDown = "";
		soundButtonOver = "";
	}
}
if (!(isObject(GuiSolidDefaultProfile)))
{
	new GuiControlProfile(Name : GuiSolidDefaultProfile)
	{
		opaque = "1";
		border = "1";
	}
}
if (!(isObject(GuiTransparentProfile)))
{
	new GuiControlProfile(Name : GuiTransparentProfile)
	{
		opaque = "0";
		border = "0";
	}
}
if (!(isObject(GuiToolTipProfile)))
{
	new GuiControlProfile(Name : GuiToolTipProfile)
	{
		fillColor = "239 237 222";
		borderColor = "138 134 122";
		fontType = "Arial";
		fontSize = "14";
		fontColor = "0 0 0";
	}
}
if (!(isObject(GuiModelessDialogProfile)))
{
	new GuiControlProfile(Name : "GuiModelessDialogProfile")
	{
		Modal = "0";
	}
}
if (!(isObject(GuiFrameSetProfile)))
{
	new GuiControlProfile(Name : GuiFrameSetProfile)
	{
		fillColor = "239 237 222";
		borderColor = "138 134 122";
		opaque = "1";
		border = "1";
	}
}
if (!(isObject(GuiWindowProfile)))
{
	new GuiControlProfile(Name : GuiWindowProfile)
	{
		opaque = "1";
		border = "1";
		fillColor = "211 211 211";
		fillColorHL = "190 255 255";
		fillColorNA = "255 255 255";
		fontColor = "0 0 0";
		fontColorHL = "200 200 200";
		text = "untitled";
		bitmap = "./images/window";
		textOffset = "5 5";
		hasBitmapArray = "1";
		justify = "center";
	}
}
if (!(isObject(GuiContentProfile)))
{
	new GuiControlProfile(Name : GuiContentProfile)
	{
		opaque = "1";
		fillColor = "255 255 255";
	}
}
if (!(isObject(GuiBlackContentProfile)))
{
	new GuiControlProfile(Name : GuiBlackContentProfile)
	{
		opaque = "1";
		fillColor = "0 0 0";
	}
}
if (!(isObject(GuiInputCtrlProfile)))
{
	new GuiControlProfile(Name : GuiInputCtrlProfile)
	{
		tab = "1";
		canKeyFocus = "1";
	}
}
if (!(isObject(GuiTextProfile)))
{
	new GuiControlProfile(Name : GuiTextProfile)
	{
		fontColor = "0 0 0";
	}
}
if (!(isObject(GuiMediumTextProfile)))
{
	new GuiControlProfile(Name : GuiMediumTextProfile)
	{
		fontType = "Akbar Plain";
		fontSize = "20";
		border = "0";
	}
}
if (!(isObject(GuiBigTextProfile)))
{
	new GuiControlProfile(Name : GuiBigTextProfile)
	{
		fontSize = "36";
	}
}
if (!(isObject(GuiMLTextProfile)))
{
	new GuiControlProfile(Name : "GuiMLTextProfile")
	{
		fontColorLink = "255 96 96";
		fontColorLinkHL = "0 0 255";
		autoSizeWidth = "1";
		autoSizeHeight = "1";
		border = "0";
	}
}
if (!(isObject(GuiTextArrayProfile)))
{
	new GuiControlProfile(Name : GuiTextArrayProfile)
	{
		fontColorHL = "32 100 100";
		fillColorHL = "200 200 200";
	}
}
if (!(isObject(GuiTextListProfile)))
{
	new GuiControlProfile(Name : GuiTextListProfile)
	{
		tab = "1";
		canKeyFocus = "1";
	}
}
if (!(isObject(GuiTextEditProfile)))
{
	new GuiControlProfile(Name : GuiTextEditProfile)
	{
		opaque = "1";
		fillColor = "255 255 255";
		fillColorHL = "128 128 128";
		border = -2.0;
		bitmap = "./images/textEdit";
		borderColor = "40 40 40 100";
		fontColor = "0 0 0";
		fontColorHL = "255 255 255";
		fontColorNA = "128 128 128";
		textOffset = "4 2";
		autoSizeWidth = "0";
		autoSizeHeight = "1";
		tab = "1";
		canKeyFocus = "1";
	}
}
if (!(isObject(GuiProgressProfile)))
{
	new GuiControlProfile(Name : "GuiProgressProfile")
	{
		opaque = "0";
		fillColor = "44 152 162 100";
		border = "1";
		borderColor = "78 88 120";
	}
}
if (!(isObject(GuiProgressTextProfile)))
{
	new GuiControlProfile(Name : "GuiProgressTextProfile")
	{
		fontColor = "0 0 0";
		justify = "center";
	}
}
if (!(isObject(GuiButtonProfile)))
{
	new GuiControlProfile(Name : GuiButtonProfile)
	{
		opaque = "1";
		border = -1.0;
		fontType = "Akbar Plain";
		fontSize = "20";
		fontColor = "0 0 0";
		fontColorHL = "32 100 100";
		fixedExtent = "1";
		justify = "center";
		canKeyFocus = "0";
		bitmap = "./images/button";
	}
}
if (!(isObject(GuiCheckBoxProfile)))
{
	new GuiControlProfile(Name : GuiCheckBoxProfile)
	{
		opaque = "0";
		fillColor = "232 232 232";
		border = "0";
		borderColor = "0 0 0";
		fontSize = "14";
		fontColor = "0 0 0";
		fontColorHL = "32 100 100";
		fixedExtent = "1";
		justify = "left";
		bitmap = "./images/checkBox";
		hasBitmapArray = "1";
	}
}
if (!(isObject(GuiRadioProfile)))
{
	new GuiControlProfile(Name : GuiRadioProfile)
	{
		fontSize = "14";
		fillColor = "232 232 232";
		fontColorHL = "32 100 100";
		fixedExtent = "1";
		bitmap = "./images/radioButton";
		hasBitmapArray = "1";
	}
}
if (!(isObject(GuiScrollProfile)))
{
	new GuiControlProfile(Name : GuiScrollProfile)
	{
		opaque = "1";
		fillColor = "255 255 255";
		border = "1";
		borderThickness = "2";
		bitmap = "./images/scrollBar";
		hasBitmapArray = "1";
	}
}
if (!(isObject(GuiTransparentScrollProfile)))
{
	new GuiControlProfile(Name : GuiTransparentScrollProfile)
	{
		opaque = "0";
		fillColor = "255 255 255";
		border = "0";
		borderThickness = "2";
		borderColor = "0 0 0";
		bitmap = "./images/scrollBar";
		hasBitmapArray = "1";
	}
}
if (!(isObject(GuiSliderProfile)))
{
	new GuiControlProfile(Name : GuiSliderProfile)
	{
		bitmap = "./images/slider";
	}
}
if (!(isObject(GuiPaneProfile)))
{
	new GuiControlProfile(Name : GuiPaneProfile)
	{
		bitmap = "./images/popupMenu";
		hasBitmapArray = "1";
	}
}
if (!(isObject(GuiPopupMenuItemBorder)))
{
	new GuiControlProfile(Name : GuiPopupMenuItemBorder)
	{
		borderColor = "51 51 53 200";
		borderColorHL = "51 51 53 200";
	}
}
if (!(isObject(GuiPopUpMenuDefault)))
{
	new GuiControlProfile(Name : GuiPopUpMenuDefault)
	{
		opaque = "1";
		mouseOverSelected = "1";
		textOffset = "3 3";
		border = "4";
		borderThickness = "2";
		fixedExtent = "1";
		bitmap = "./images/scrollBar";
		hasBitmapArray = "1";
		profileForChildren = GuiPopupMenuItemBorder;
		fillColor = "255 255 255 200";
		fontColorHL = "128 128 128";
		borderColor = "151 151 153 175";
		borderColorHL = "151 151 153 175";
	}
}
if (!(isObject(GuiPopUpMenuProfile)))
{
	new GuiControlProfile(Name : GuiPopUpMenuProfile)
	{
		textOffset = "6 3";
		bitmap = "./images/dropDown";
		hasBitmapArray = "1";
		border = -3.0;
		profileForChildren = GuiPopUpMenuDefault;
	}
}
if (!(isObject(GuiPopUpMenuEditProfile)))
{
	new GuiControlProfile(Name : GuiPopUpMenuEditProfile)
	{
		textOffset = "6 3";
		canKeyFocus = "1";
		bitmap = "./images/dropDown";
		hasBitmapArray = "1";
		border = -3.0;
		profileForChildren = GuiPopUpMenuDefault;
	}
}
if (!(isObject(GuiListBoxProfile)))
{
	new GuiControlProfile(Name : GuiListBoxProfile)
	{
		tab = "1";
		canKeyFocus = "1";
	}
}
if (!(isObject(GuiTabBookProfile)))
{
	new GuiControlProfile(Name : GuiTabBookProfile)
	{
		fillColor = "255 255 255";
		fillColorHL = "64 150 150";
		fillColorNA = "150 150 150";
		fontColor = "30 30 30";
		fontColorHL = "32 100 100";
		fontColorNA = "0 0 0";
		fontType = "Arial Bold";
		fontSize = "14";
		justify = "center";
		bitmap = "./images/tab";
		tabWidth = "64";
		TabHeight = "24";
		TabPosition = "Top";
		tabRotation = "Horizontal";
		textOffset = "0 -2";
		tab = "1";
		canKeyFocus = "1";
	}
}
if (!(isObject(GuiTabPageProfile)))
{
	new GuiControlProfile(Name : GuiTabPageProfile)
	{
		fillColor = "255 255 255";
		bitmap = "./images/tab";
		opaque = "1";
	}
}
if (!(isObject(GuiMenuBarProfile)))
{
	new GuiControlProfile(Name : GuiMenuBarProfile)
	{
		fontType = "Arial";
		fontSize = "15";
		opaque = "1";
		fillColor = "239 237 222";
		fillColorHL = "102 153 204";
		borderColor = "138 134 122";
		borderColorHL = "0 51 153";
		border = "5";
		fontColor = "0 0 0";
		fontColorHL = "255 255 255";
		fontColorNA = "128 128 128";
		fixedExtent = "1";
		justify = "center";
		canKeyFocus = "0";
		mouseOverSelected = "1";
		bitmap = "./images/menu";
		hasBitmapArray = "1";
	}
}
if (!(isObject(GuiConsoleProfile)))
{
	new GuiControlProfile(Name : GuiConsoleProfile)
	{
		if ($platform $= "macos")
		{
		}
		else
		{
		}
		fontType = "Lucida Console";
		if ($platform $= "macos")
		{
		}
		else
		{
		}
		fontSize = "12";
		fontColor = "0 0 0";
		fontColorHL = "130 130 130";
		fontColorNA = "255 0 0";
		fontColors["6"] = "50 50 50";
		fontColors["7"] = "50 50 0";
		fontColors["8"] = "0 0 50";
		fontColors["9"] = "0 50 0";
	}
}
if (!(isObject(GuiConsoleTextEditProfile)))
{
	new GuiControlProfile(Name : GuiConsoleTextEditProfile)
	{
		if ($platform $= "macos")
		{
		}
		else
		{
		}
		fontType = "Lucida Console";
		if ($platform $= "macos")
		{
		}
		else
		{
		}
		fontSize = "12";
	}
}
if (!(isObject(GuiTreeViewProfile)))
{
	new GuiControlProfile(Name : GuiTreeViewProfile)
	{
		fillColorHL = "0 60 150";
		fontSize = "14";
		fontColor = "0 0 0";
		fontColorHL = "64 150 150";
		fontColorNA = "240 240 240";
		fontColorSEL = "250 250 250";
		bitmap = "./images/treeView";
		canKeyFocus = "1";
		autoSizeHeight = "1";
	}
}
if (!(isObject(GuiText24Profile)))
{
	new GuiControlProfile(Name : GuiText24Profile)
	{
		fontSize = "24";
	}
}
if (!(isObject(GuiRSSFeedMLTextProfile)))
{
	new GuiControlProfile(Name : "GuiRSSFeedMLTextProfile")
	{
		fontColorLink = "55 55 255";
		fontColorLinkHL = "255 55 55";
	}
}
