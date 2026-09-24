// menuTab.cs.dso
function MenuTab::activate(%this)
{
	if (isHorizontal)
	{
		%this.setBitmap($BTN_H_TAB_ACTIVE);
	}
	else
	{
		%this.setBitmap($BTN_V_TAB_ACTIVE);
	}
	%this.originalParent = %this.getParent();
	originalParent.remove(%this);
	originalParent.getParent().addGuiControl(%this);
	return;
}
function MenuTab::deactivate(%this)
{
	if (!(isObject(originalParent)))
	{
		return isObject(originalParent);
	}
	%this.getParent().remove(%this);
	originalParent.addGuiControl(%this);
	if (isHorizontal)
	{
		%this.setBitmap($BTN_H_TAB_NORMAL);
	}
	else
	{
		%this.setBitmap($BTN_V_TAB_NORMAL);
	}
	%this.originalParent = "";
	return;
}
function MenuTab::refreshList(%this, %page)
{
	btn_levelDetail_local.deactivate();
	btn_levelDetail_competitors.deactivate();
	btn_levelDetail_best.deactivate();
	btn_levelDetail_location.deactivate();
	btn_levelDetail_user.deactivate();
	%this.activate();
	menu_levelDetail.refreshList(%page);
	return;
}
function MenuTab::switchEnvironment(%this, %env)
{
	tablbl_level_cave.deactivate();
	tablbl_level_jungle.deactivate();
	tablbl_level_trip.deactivate();
	tablbl_level_epilog.deactivate();
	%this.activate();
	menu_level.onTabSelected(%env);
	return;
}
function MenuTab::displayOptions(%this, %page)
{
	tablbl_options_joystick.deactivate();
	if (isObject(tablbl_options_keyboard))
	{
		tablbl_options_keyboard.deactivate();
	}
	if (isObject(tablbl_options_sixense))
	{
		tablbl_options_sixense.deactivate();
	}
	tablbl_options_video.deactivate();
	%this.activate();
	menu_options.onTabSelected(%page);
	return;
}
