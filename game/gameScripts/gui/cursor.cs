// cursor.cs.dso
if (!(isObject(noCursor)))
{
	new GuiCursor(Name : noCursor)
	{
		hotSpot = "1 1";
		renderOffset = "0 0";
		bitmapName = ['"~/"', '$DataFolder', '"/images/Menu/noCursor"'];
	}
}
if (getOS() != "wii" && getOS() != "linux")
{
	Canvas.setCursor(noCursor);
}
