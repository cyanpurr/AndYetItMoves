// grabInput.cs.dso
function initGrabberInput()
{
	new t2dStaticSprite(Name : texGrabCompass)
	{
		scenegraph = scenegraph;
		imageMap = "compassTextureImageMap";
		size = $compassSize;
		Layer = "0";
		_behavior0 = "BeMask	layer	custom	usePositive	1	textureObject	grabCompass";
		escapeSwitch = "1";
	}
	texGrabCompass.setVisible("0");
	new t2dStaticSprite(Name : grabCompass)
	{
		scenegraph = scenegraph;
		imageMap = "compassImageMap";
		size = $compassSize;
		Layer = "0";
		_behavior0 = "BeTexture";
	}
	new t2dStaticSprite(Name : grabCircles)
	{
		scenegraph = scenegraph;
		imageMap = "circlesImageMap";
		size = $circlesSize;
		Layer = "0";
		_behavior0 = "BeTexture";
	}
	grabCircles.mount(viewWindow, "0 0", "0", "1", "1", "1");
	new t2dStaticSprite(Name : grabCirclesMask)
	{
		scenegraph = scenegraph;
		imageMap = "circlesMaskImageMap";
		size = "75 75";
		Layer = "0";
		_behavior0 = "BeMask	layer	custom	usePositive	0	textureObject	grabCircles";
		escapeSwitch = "1";
	}
	grabCirclesMask.setVisible("0");
	new t2dStaticSprite(Name : grabCursorOpen)
	{
		scenegraph = scenegraph;
		imageMap = "grabHandOpenImageMap";
		size = "15 15";
		Layer = "0";
		_behavior0 = "BeDontCollide";
	}
	grabCursorOpen.setVisible("0");
	new t2dStaticSprite(Name : grabSpot)
	{
		scenegraph = scenegraph;
		imageMap = "grabSpotAbstractImageMap";
		size = "15 15";
		Layer = "0";
		_behavior0 = "BeDontCollide";
	}
	grabSpot.setVisible("0");
	new t2dStaticSprite(Name : grabberGuide)
	{
		scenegraph = scenegraph;
		imageMap = "grabberGuideImageMap";
		size = "30 30";
		Layer = "0";
		_behavior0 = "BeDontCollide";
	}
	grabberGuide.setVisible("0");
	new t2dStaticSprite(Name : warningBorder)
	{
		scenegraph = scenegraph;
		imageMap = "warningborderImageMap";
		size = "150 112.5";
		Layer = "0";
		_behavior0 = "BeDontCollide";
	}
	warningBorder.setVisible("0");
	if ($GRABBERINPUT && !($WII))
	{
		sceneWindow2d.activateGrabber();
	}
	if ($WII)
	{
		$grabberPosRotDir = 1;
	}
	return;
}
function togglePosRotDir()
{
	$grabberPosRotDir = !($grabberPosRotDir);
	wiiInput.setWiiInputParam("posRotDir", $grabberPosRotDir);
	return;
}
