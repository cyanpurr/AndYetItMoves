// cursor.cs.dso
$cursorControlled = 1;
function showCursor()
{
	if ($cursorControlled)
	{
		lockMouse("0");
	}
	Canvas.cursorOn();
	return;
}
function hideCursor()
{
	if ($cursorControlled)
	{
		lockMouse("1");
	}
	Canvas.cursorOff();
	return;
}
function GuiCanvas::checkCursor(%this)
{
	%count = %this.getCount();
	%i = 0;
	while (%i < %count)
	{
		%control = %this.getObject(%i);
		if (noCursor $= "" || !(noCursor))
		{
			showCursor();
			return;
		}
		%i = %i + 1.0;
	}
	hideCursor();
	return;
}
function GuiCanvas::setContent(%this, %ctrl)
{
	Parent::setContent(%this, %ctrl);
	%this.checkCursor();
	return;
}
function GuiCanvas::pushDialog(%this, %ctrl, %layer)
{
	Parent::pushDialog(%this, %ctrl, %layer);
	%this.checkCursor();
	return;
}
function GuiCanvas::popDialog(%this, %ctrl)
{
	Parent::popDialog(%this, %ctrl);
	%this.checkCursor();
	return;
}
function GuiCanvas::popLayer(%this, %layer)
{
	Parent::popLayer(%this, %layer);
	%this.checkCursor();
	return;
}
activatePackage(CanvasCursorPackage);
