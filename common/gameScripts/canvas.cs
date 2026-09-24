// canvas.cs.dso
$canvasCreated = 0;
function initializeCanvas(%windowName)
{
	if ($canvasCreated)
	{
		error("Cannot instantiate more than one canvas!");
		return;
	}
	videoSetGammaCorrection($pref::OpenGL::gammaCorrection);
	if (!(createCanvas(%windowName)))
	{
		error("Canvas creation failed. Shutting down.");
		quit();
	}
	%goodres = $Game::Resolution;
	setScreenMode(getWord(%goodres, "0"), getWord(%goodres, "1"), getWord(%goodres, "2"), "0");
	$canvasCreated = 1;
	return;
}
function resetCanvas()
{
	if (isObject(Canvas))
	{
		Canvas.repaint();
	}
	return;
}
