// cursors.cs.dso
if ($platform $= "macos")
{
	new GuiCursor(Name : defaultCursor)
	{
		hotSpot = "4 4";
		renderOffset = "0 0";
		bitmapName = "~/gui/images/macCursor";
	}
	new GuiCursor(Name : ayimCursor)
	{
		hotSpot = "10 3";
		renderOffset = "0 0";
		bitmapName = "~/gui/images/ayimCursor";
	}
}
else
{
	new GuiCursor(Name : defaultCursor)
	{
		hotSpot = "1 1";
		renderOffset = "0 0";
		bitmapName = "~/gui/images/defaultCursor";
	}
	new GuiCursor(Name : ayimCursor)
	{
		hotSpot = "1 1";
		renderOffset = "0 0";
		bitmapName = "~/gui/images//ayimCursor";
	}
}
