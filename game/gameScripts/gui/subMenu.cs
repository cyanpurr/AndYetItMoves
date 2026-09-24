// subMenu.cs.dso
function SubMenu::onAdd(%this)
{
	%this.scriptName = getLastToken(%this.getName(), "_");
	%this.openSubMenu = ['"menu_"', 'scriptName'];
	return;
}
function SubMenu::onAction(%this)
{
	if (isObject(openSubMenu))
	{
		debugEcho("entering this submenu:" SPC openSubMenu);
		$ACTUAL_MENU.enterMenu(openSubMenu);
	}
	else
	{
		debugWarn("menu item missing:" SPC openSubMenu);
	}
	return;
}
