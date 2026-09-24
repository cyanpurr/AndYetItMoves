// guiProfiles.cs.dso
if (!(isObject(AyimMenuItemProfile)))
{
	new GuiControlProfile(Name : AyimMenuItemProfile)
	{
		border = "0";
		canKeyFocus = "1";
		fontColorHL = "35 35 35";
		fontColorNA = "140 140 140";
		tab = "0";
	}
}
if (!(isObject(AyimEmptyProfile)))
{
	new GuiControlProfile(Name : AyimEmptyProfile)
	{
		opaque = "0";
		border = "0";
		tab = "0";
	}
}
if (!(isObject(AyimBackgroundProfile)))
{
	new GuiControlProfile(Name : AyimBackgroundProfile);
}
if (!(isObject(AyimButtonProfile)))
{
	new GuiControlProfile(Name : AyimButtonProfile)
	{
		fontType = "Akbar Plain";
		fontSize = "20";
		fontColor = "50 50 50";
		fontColorHL = "0 0 0";
		justify = "center";
		autoSizeWidth = "1";
		soundButtonDown = "MenuMouseOver";
		soundButtonOver = "MenuMouseOver";
	}
}
if (!(isObject(AyimButtonActiveProfile)))
{
	new GuiControlProfile(Name : AyimButtonActiveProfile)
	{
		fontColor = "255 150 0";
		fontColorHL = "255 180 0";
	}
}
if (!(isObject(AyimButtonSmallTextProfile)))
{
	new GuiControlProfile(Name : AyimButtonSmallTextProfile)
	{
		fontSize = "14";
	}
}
if (!(isObject(AyimButtonFatTextProfile)))
{
	new GuiControlProfile(Name : AyimButtonFatTextProfile)
	{
		fontSize = "24";
	}
}
if (!(isObject(AyimLevelButtonProfile)))
{
	new GuiControlProfile(Name : AyimLevelButtonProfile)
	{
		fontType = "Akbar Plain";
		fontSize = "20";
		fontColor = "205 205 205";
		fontColorHL = "255 255 255";
		fontColorNA = "50 50 50";
	}
}
if (!(isObject(AyimPlaymodeButtonProfile)))
{
	new GuiControlProfile(Name : AyimPlaymodeButtonProfile);
}
if (!(isObject(AyimLevelButtonBonusProfile)))
{
	new GuiControlProfile(Name : AyimLevelButtonBonusProfile)
	{
		fontColor = "255 20 20";
	}
}
if (!(isObject(AyimButtonHeader1Profile)))
{
	new GuiControlProfile(Name : AyimButtonHeader1Profile)
	{
		fontSize = "28";
		textOffset = "0 0";
	}
}
if (!(isObject(AyimButtonLeftProfile)))
{
	new GuiControlProfile(Name : AyimButtonLeftProfile)
	{
		justify = "left";
		textOffset = "0 0";
	}
}
if (!(isObject(AyimButtonRightProfile)))
{
	new GuiControlProfile(Name : AyimButtonRightProfile)
	{
		justify = "right";
		textOffset = "0 0";
	}
}
if (!(isObject(AyimListItemProfile)))
{
	new GuiControlProfile(Name : AyimListItemProfile)
	{
		textOffset = "0 2";
		fontType = "Arial Unicode MS";
	}
}
if (!(isObject(AyimListItemLeftProfile)))
{
	new GuiControlProfile(Name : AyimListItemLeftProfile)
	{
		justify = "left";
	}
}
if (!(isObject(AyimListItemRightProfile)))
{
	new GuiControlProfile(Name : AyimListItemRightProfile)
	{
		justify = "Right";
	}
}
if (!(isObject(AyimListItemRightGreenProfile)))
{
	new GuiControlProfile(Name : AyimListItemRightGreenProfile)
	{
		fontColor = "10 150 10";
		fontColorHL = "10 110 10";
	}
}
if (!(isObject(AyimListItemRightRedProfile)))
{
	new GuiControlProfile(Name : AyimListItemRightRedProfile)
	{
		fontColor = "190 10 10";
		fontColorHL = "150 10 10";
	}
}
if (!(isObject(AyimListItemColor1Profile)))
{
	new GuiControlProfile(Name : AyimListItemColor1Profile)
	{
		opaque = "1";
		fillColor = "205 205 205 51";
	}
}
if (!(isObject(AyimListItemColor2Profile)))
{
	new GuiControlProfile(Name : AyimListItemColor2Profile)
	{
		opaque = "1";
		fillColor = "176 176 176 80";
	}
}
if (!(isObject(AyimListItemSelectedProfile)))
{
	new GuiControlProfile(Name : AyimListItemSelectedProfile)
	{
		opaque = "1";
		fillColor = "255 255 255 255";
	}
}
if (!(isObject(AyimListItemHighlightedProfile)))
{
	new GuiControlProfile(Name : AyimListItemHighlightedProfile)
	{
		opaque = "1";
		fillColor = "120 137 179 153";
	}
}
if (!(isObject(AyimMenuTextProfile)))
{
	new GuiControlProfile(Name : AyimMenuTextProfile)
	{
		fontType = "Akbar Plain";
		fontSize = "20";
		fillColorNA = "255 255 255 100";
		autoSizeWidth = "1";
	}
}
if (!(isObject(AyimMenuTextNoAutoSizeProfile)))
{
	new GuiControlProfile(Name : AyimMenuTextNoAutoSizeProfile)
	{
		autoSizeWidth = "0";
	}
}
if (!(isObject(AyimUnicodeMenuTextProfile)))
{
	new GuiControlProfile(Name : AyimUnicodeMenuTextProfile)
	{
		fontType = "Arial Unicode MS";
	}
}
if (!(isObject(AyimToolTipProfile)))
{
	new GuiControlProfile(Name : AyimToolTipProfile)
	{
		fontType = "Arial Unicode MS";
	}
}
if (!(isObject(AyimMenuTextRedProfile)))
{
	new GuiControlProfile(Name : AyimMenuTextRedProfile)
	{
		fontColor = "190 10 10";
	}
}
if (!(isObject(AyimMenuTextGreenProfile)))
{
	new GuiControlProfile(Name : AyimMenuTextGreenProfile)
	{
		fontColor = "10 150 10";
	}
}
if (!(isObject(AyimMenuTextCenterProfile)))
{
	new GuiControlProfile(Name : AyimMenuTextCenterProfile)
	{
		autoSizeWidth = "0";
		justify = "center";
	}
}
if (!(isObject(AyimUnicodeMenuTextCenterProfile)))
{
	new GuiControlProfile(Name : AyimUnicodeMenuTextCenterProfile)
	{
		autoSizeWidth = "0";
		justify = "center";
	}
}
if (!(isObject(AyimMenuTextRightProfile)))
{
	new GuiControlProfile(Name : AyimMenuTextRightProfile)
	{
		autoSizeWidth = "0";
		justify = "right";
	}
}
if (!(isObject(AyimMenuTextRightWhiteProfile)))
{
	new GuiControlProfile(Name : AyimMenuTextRightWhiteProfile)
	{
		fontColor = "172 172 172 255";
	}
}
if (!(isObject(AyimMenuTextCenterSmallProfile)))
{
	new GuiControlProfile(Name : AyimMenuTextCenterSmallProfile)
	{
		fontSize = "14";
	}
}
if (!(isObject(AyimMenuTextCenterGreenProfile)))
{
	new GuiControlProfile(Name : AyimMenuTextCenterGreenProfile)
	{
		fontColor = "10 150 10";
	}
}
if (!(isObject(AyimMenuTextCenterRedProfile)))
{
	new GuiControlProfile(Name : AyimMenuTextCenterRedProfile)
	{
		fontColor = "190 10 10";
	}
}
if (!(isObject(AyimMenuTextCenterGreyProfile)))
{
	new GuiControlProfile(Name : AyimMenuTextCenterGreyProfile)
	{
		fontColor = "121 121 120";
	}
}
if (!(isObject(AyimAlertProfile)))
{
	new GuiControlProfile(Name : AyimAlertProfile)
	{
		fontColor = "190 10 10";
		tab = "0";
	}
}
if (!(isObject(AyimHeader1Profile)))
{
	new GuiControlProfile(Name : AyimHeader1Profile)
	{
		fontType = "Akbar Plain";
		fontSize = "28";
		justify = "left";
		autoSizeHeight = "1";
		fontColor = "172 172 172 255";
	}
}
if (!(isObject(AyimHeader1AlertProfile)))
{
	new GuiControlProfile(Name : AyimHeader1AlertProfile)
	{
		fontColor = "190 10 10";
	}
}
if (!(isObject(AyimHeader1CenterProfile)))
{
	new GuiControlProfile(Name : AyimHeader1CenterProfile)
	{
		autoSizeWidth = "0";
		justify = "center";
	}
}
if (!(isObject(AyimHeader1CenterWhiteProfile)))
{
	new GuiControlProfile(Name : AyimHeader1CenterWhiteProfile)
	{
		fontColor = "255 255 255";
	}
}
if (!(isObject(AyimHeader1CenterWhiteProfile)))
{
	new GuiControlProfile(Name : AyimHeader1CenterWhiteProfile)
	{
		fontColor = "235 235 235";
	}
}
if (!(isObject(AyimHeader1CenterRedProfile)))
{
	new GuiControlProfile(Name : AyimHeader1CenterRedProfile)
	{
		fontColor = "190 10 10";
	}
}
if (!(isObject(AyimFailedProfile)))
{
	new GuiControlProfile(Name : AyimFailedProfile)
	{
		fontColor = "190 10 10";
	}
}
if (!(isObject(AyimHeader1CenterGreenProfile)))
{
	new GuiControlProfile(Name : AyimHeader1CenterGreenProfile)
	{
		fontColor = "10 150 10";
	}
}
if (!(isObject(AyimHeader1RightProfile)))
{
	new GuiControlProfile(Name : AyimHeader1RightProfile)
	{
		justify = "right";
	}
}
if (!(isObject(AyimHeader2Profile)))
{
	new GuiControlProfile(Name : AyimHeader2Profile)
	{
		fontColor = "0 0 0 255";
	}
}
if (!(isObject(AyimHeader2NoAutoSizeProfile)))
{
	new GuiControlProfile(Name : AyimHeader2NoAutoSizeProfile)
	{
		autoSizeWidth = "0";
	}
}
if (!(isObject(AyimHeader2RightProfile)))
{
	new GuiControlProfile(Name : AyimHeader2RightProfile)
	{
		fontColor = "0 0 0 255";
	}
}
if (!(isObject(AyimHeader2CenterProfile)))
{
	new GuiControlProfile(Name : AyimHeader2CenterProfile)
	{
		fontColor = "0 0 0 255";
		autoSizeWidth = "0";
		autoSizeHeight = "0";
	}
}
if (!(isObject(AyimHelloProfile)))
{
	new GuiControlProfile(Name : AyimHelloProfile)
	{
		fontType = "Arial Unicode MS";
	}
}
if (!(isObject(AyimVersionProfile)))
{
	new GuiControlProfile(Name : AyimVersionProfile)
	{
		fontType = "Akbar Plain";
		fontSize = "16";
		justify = "right";
		fontColor = "172 172 172 255";
		autoSizeWidth = "0";
	}
}
if (!(isObject(AyimTextEditProfile)))
{
	new GuiControlProfile(Name : AyimTextEditProfile)
	{
		opaque = "1";
		fillColor = "255 255 255 100";
		fillColorHL = "255 255 255 255";
		borderColor = "40 40 40 100";
		fontType = "Arial Unicode MS";
		fontSize = "14";
		fontColorHL = "255 255 255";
		fontColorNA = "128 128 128";
		textOffset = "4 2";
		tab = "1";
		canKeyFocus = "1";
	}
}
if (!(isObject(AyimTextEditDefaultValueProfile)))
{
	new GuiControlProfile(Name : AyimTextEditDefaultValueProfile)
	{
		fontColor = "128 128 128";
	}
}
if (!(isObject(AyimPopUpMenuDefault)))
{
	new GuiControlProfile(Name : AyimPopUpMenuDefault)
	{
		fontType = "Arial Unicode MS";
	}
}
if (!(isObject(AyimPopUpMenuProfile)))
{
	new GuiControlProfile(Name : AyimPopUpMenuProfile)
	{
		fontType = "Arial Unicode MS";
		profileForChildren = AyimPopUpMenuDefault;
	}
}
if (!(isObject(AyimScrollProfile)))
{
	new GuiControlProfile(Name : AyimScrollProfile)
	{
		opaque = "0";
		fillColor = "255 255 255 0";
		border = "0";
		bitmap = ['"~/"', '$DataFolder', '"/images/Menu/scrollBar"'];
		hasBitmapArray = "1";
	}
}
if (!(isObject(AyimSliderProfile)))
{
	new GuiControlProfile(Name : AyimSliderProfile)
	{
		fontColor = "0 0 0 0";
		bitmap = ['"~/"', '$DataFolder', '"/images/Menu/slider"'];
		hasBitmapArray = "1";
	}
}
if (!(isObject(AyimRadioProfile)))
{
	new GuiControlProfile(Name : AyimRadioProfile)
	{
		fixedExtent = "1";
		bitmap = ['"~/"', '$DataFolder', '"/images/Menu/radiobutton_celled"'];
		hasBitmapArray = "1";
	}
}
if (!(isObject(AyimCheckBoxProfile)))
{
	new GuiControlProfile(Name : AyimCheckBoxProfile)
	{
		fixedExtent = "1";
		bitmap = ['"~/"', '$DataFolder', '"/images/Menu/checkbox_celled"'];
		hasBitmapArray = "1";
		autoSizeWidth = "0";
	}
}
if (!(isObject(AyimCurTimeTextProfile)))
{
	new GuiControlProfile(Name : AyimCurTimeTextProfile)
	{
		fontSize = "28";
		autoSizeWidth = "0";
	}
}
if (!(isObject(AyimTimeDiffTextProfile)))
{
	new GuiControlProfile(Name : AyimTimeDiffTextProfile)
	{
		fontColor = "10 150 10";
	}
}
if (!(isObject(AyimRotationTextProfile)))
{
	new GuiControlProfile(Name : AyimRotationTextProfile)
	{
		fontColor = "10 150 10";
		justify = "right";
	}
}
if (!(isObject(AyimListItemHeaderColorNAProfile)))
{
	new GuiControlProfile(Name : AyimListItemHeaderColorNAProfile)
	{
		fontColor = "165 165 165 255";
	}
}
if (!(isObject(AyimListItemColorNAProfile)))
{
	new GuiControlProfile(Name : AyimListItemColorNAProfile)
	{
		fontColor = "165 165 165 255";
	}
}
if (!(isObject(AyimListItemNAProfile)))
{
	new GuiControlProfile(Name : AyimListItemNAProfile)
	{
		opaque = "1";
		fillColor = "150 150 150 150";
	}
}
function changeFont(%font, %smallSize, %bigSize)
{
	debugEcho("changing font to" SPC %font SPC %smallSize SPC %bigSize);
	AyimButtonProfile.setFont(%font, %smallSize);
	AyimPlaymodeButtonProfile.setFont(%font, %smallSize);
	AyimMenuTextProfile.setFont(%font, %smallSize);
	AyimMenuTextNoAutoSizeProfile.setFont(%font, %smallSize);
	AyimMenuTextRightProfile.setFont(%font, %smallSize);
	AyimMenuTextRightWhiteProfile.setFont(%font, %smallSize);
	AyimMenuTextCenterProfile.setFont(%font, %smallSize);
	AyimRadioProfile.setFont(%font, %smallSize);
	AyimCheckBoxProfile.setFont(%font, %smallSize);
	AyimLevelButtonProfile.setFont(%font, %smallSize);
	AyimButtonSmallTextProfile.setFont(%font, "14");
	AyimMenuTextCenterSmallProfile.setFont(%font, "14");
	AyimAlertProfile.setFont(%font, %bigSize);
	AyimHeader1AlertProfile.setFont(%font, %bigSize);
	AyimHeader1Profile.setFont(%font, %bigSize);
	AyimHeader2Profile.setFont(%font, %bigSize);
	AyimHeader1CenterProfile.setFont(%font, %bigSize);
	AyimFailedProfile.setFont(%font, %bigSize);
	AyimHeader2CenterProfile.setFont(%font, %bigSize);
	AyimHeader2Profile.setFont(%font, %bigSize);
	AyimHeader2NoAutoSizeProfile.setFont(%font, %bigSize);
	return;
}
