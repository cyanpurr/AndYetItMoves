// camera.cs.dso
function initCamera(%onlyCameraItSelf)
{
	if ($ACTUAL_SCREEN_RATIO $= "")
	{
		$ACTUAL_SCREEN_RATIO = getX(getRes()) / getY(getRes());
	}
	echo("ASR:" SPC $ACTUAL_SCREEN_RATIO);
	if (!(isObject(camera)))
	{
		new AyimCamera(Name : camera)
		{
			Layer = "0";
			size = "2 2";
			scenegraph = daSceneGraph;
			GraphGroup = $GROUPS["dontCollide"];
		}
		camera.timeToRotate = "0.5";
		camera.currentRotation = "0";
		camera.lastRotation = "0";
		camera.horizontalVector = "1 0";
		camera.verticalVector = "0 1";
		camera.maxHeight = "550";
		camera.minHeight = "90";
		camera.maxZoom = "2";
		camera.minZoom = "0.4";
		camera.defaultCameraHeight = "180";
		camera.dontShrink = "1";
	}
	globals.setCamera(camera);
	camera.escapeSwitch = "1";
	debugEcho("maxheight:" SPC maxHeight);
	sceneWindow2d.setUseWindowMouseEvents(%onlyCameraItSelf);
	if (%onlyCameraItSelf)
	{
		setCameraRatio();
		sceneWindow2d.setCameraRotation("0");
		sceneWindow2d.setCurrentCameraZoom("1");
		if (isVideo)
		{
			subscribeToEvents(camera, "onLevelLoadFinished40 onUpdateTick10");
		}
		return;
	}
	camera.setCollisionActive("1", "0");
	camera.setCollisionGroups($GROUPS["zoomTrigger"]);
	camera.setCollisionPhysics("0", "0");
	camera.currentCameraZoom = camera.getRealZoom();
	if (!(isObject(viewWindow)))
	{
		camera.viewWindow = new t2dSceneObject(Name : viewWindow)
		{
			Layer = "0";
			scenegraph = daSceneGraph;
			GraphGroup = $GROUPS["dontCollide"];
			CollisionDetectionMode = "POLYGON";
		}
	}
	viewWindow.setCollisionActive("1", "0");
	viewWindow.setCollisionPhysics("0", "0");
	viewWindow.setCollisionGroups($GROUPS["viewWindowTrigger"]);
	viewWindow.addCollisionGroups(viewWindowGroups);
	viewWindow.escapeSwitch = "1";
	sceneWindow2d.setCameraRotation("0");
	sceneWindow2d.setCurrentCameraZoom(zoomFactor);
	setCameraRatio();
	if (!($watchAsReplay))
	{
		sceneWindow2d.mount(player, "0 0", cameraMountForce, "1");
		camera.mount(player, "0 0", cameraMountForce, "0", "1", "0", "0");
		viewWindow.mount(player, "0 0", cameraMountForce, "0", "1");
	}
	camera.Smoother = Smoother::createInstance();
	camera.zoomSmoother = Smoother::createInstance();
	camera.fadeShape = new t2dShapeVector(Name : "")
	{
		scenegraph = scenegraph;
		PolyList = "-1.000 -1.000 1.000 -1.000 1.000 1.000 -1.000 1.000";
		FillMode = "1";
		Layer = "0";
	}
	%shapeSize = 1200;
	fadeShape.setSize(%shapeSize, %shapeSize);
	fadeShape.setBlendColor("0", "0", "0");
	fadeShape.setBlendAlpha("1");
	fadeShape.mount(camera, "0 0", "0", "0", "1", "0", "0");
	camera.initialFadeCompleted = "0";
	camera.enableUpdateCallback();
	camera.sceneWindow = sceneWindow2d;
	subscribeToEvents(camera, "onLevelLoadFinished onUpdateSecondTick");
	return;
}
function sceneWindow2d::onMouseDown(%this, , , )
{
	if (isObject($splashScreenerObject))
	{
		$splashScreenerObject.onMouseDown();
	}
	return;
}
function camera::onLevelLoadFinished40(%this)
{
	Canvas.popDialog(rally_display);
	camera.enableUpdateCallback();
	setControlsEnabled("0");
	playMovie();
	return;
}
function camera::onUpdateSecondTick(%this)
{
	camera.schedule("300", "startShapeFade");
	return;
}
function camera::startShapeFade(%this)
{
	debugEcho("startinf fade shapeing");
	fadeShape.setAlphaVelocity(-4.0);
	%this.schedule("250", "onShapeFadeComplete");
	return;
}
function camera::onShapeFadeComplete(%this)
{
	fadeShape.setAlphaVelocity("0");
	fadeShape.setBlendAlpha("0");
	fadeShape.safeDelete();
	camera.initialFadeCompleted = "1";
	if ($watchAsReplay)
	{
		schedule("1000", "0", "firstKeyPressed");
	}
	else
	{
		if ($WII)
		{
			subscribeToEvents(wiiInput, "onUnpauseGame");
			wiiInput.checkInput();
			break;
		}
		if ($settings::Controls::SixenseEnabled)
		{
			showSixenseInstructions();
		}
	}
	return;
}
function setCameraRatio()
{
	camera.screenRatio = $ACTUAL_SCREEN_RATIO;
	%cameraHeight = defaultCameraHeight;
	%cameraWidth = mRound(defaultCameraHeight * $ACTUAL_SCREEN_RATIO);
	%camPos = sceneWindow2d.getCurrentCameraPosition();
	if (sceneWindow2d.getIsCameraMounted())
	{
		%wasMounted = 1;
		sceneWindow2d.dismount();
	}
	sceneWindow2d.setCurrentCameraPosition(getX(%camPos), getY(%camPos), %cameraWidth, %cameraHeight);
	if (%wasMounted)
	{
		sceneWindow2d.mount(viewWindow.getMountedParent(), "0 0", viewWindow.getMountForce(), "0");
	}
	if (isObject(camera))
	{
		camera.viewWindowSize = %cameraWidth SPC %cameraHeight;
		camera.updateViewWindow();
	}
	else
	{
		debugEcho("setting curCameraArea of scenwindow2d" SPC %this);
		sceneWindow2d.setCurrentCameraArea("-120 -90 120 90");
	}
	return;
}
function setNewResolution(%width, %height, %bitDepth, %fullcreen)
{
	if ($WII && getOS() != "wii")
	{
		%width = 640;
		%height = 480;
		%fullcreen = 0;
	}
	if (%bitDepth $= "")
	{
		%bitDepth = 32;
	}
	if (%fullcreen $= "")
	{
		%fullcreen = 0;
	}
	echo("trying to set new res:" SPC %width SPC %height SPC "bD:" SPC %bitDepth SPC "fS:" SPC %fullcreen);
	if (!($WII) || getOS() != "wii")
	{
		setScreenMode(%width, %height, %bitDepth, %fullcreen);
	}
	$ACTUAL_SCREEN_RATIO = %width / %height;
	$ACTUAL_BITDEPTH = %bitDepth;
	setCameraRatio();
	setVerticalSync($settings::Performance::VerticalSync);
	return;
}
function camera::rotate(%this, %direction, %skipAnimation)
{
	if (camera.getIsRotating())
	{
		if (rotationDirection == %direction || mAbs(%direction) > 1.0)
		{
			return mAbs(%direction);
		}
		else
		{
			%rotationDone = 1.0 - Smoother.getProgressPercentage();
			%reverse = !(Smoother.getIsReverse());
			Smoother.start(%reverse, %rotationDone);
			%this.abortionParameters = %reverse TAB %rotationDone;
			triggerEvent("onRotationAbort");
		}
	}
	else
	{
		%this.rotateToAngle(currentRotation + %direction * 90.0, %skipAnimation);
	}
	camera.rotationDirection = getSign(%direction);
	return;
}
function camera::is180Rotation(%this)
{
	return mAbs(rotationDirection) > 1.0;
	return mAbs(rotationDirection) > 1.0;
}
function camera::getRotationTimeLeft(%this)
{
	if (%this.getIsRotating())
	{
		return Smoother.getTimeLeft();
	}
	else
	{
		return "0";
	}
	return "0";
}
function camera::zoomTo(%this, %zoomValue, %duration)
{
	%zoomDifference = mAbs(%this.getZoom() - %zoomValue);
	if (%duration $= "")
	{
		%duration = %zoomDifference;
	}
	zoomSmoother.init(%duration, %this.getZoom(), %zoomValue, "SMOOTH", "1");
	zoomSmoother.start();
	subscribeToEvent(%this, "onUpdateFrame20");
	%this.isZooming = "1";
	return;
}
function camera::getRealZoom(%this)
{
	return sceneWindow2d.getCurrentCameraZoom();
	return sceneWindow2d.getCurrentCameraZoom();
}
function camera::getIsPointInside(%this, %point)
{
	return viewWindow.getIsPointInObject(%point);
	return viewWindow.getIsPointInObject(%point);
}
function camera::prepareLevelTransition(%this, %end, %startDirectly)
{
	%rect = viewWindow.getCollisionPolyWorldExtent();
	%this.objectsInsideViewWindow = scenegraph.pickRect(%rect, $MASK_ALL_LIST, $MASK_ALL_LIST, "0");
	%i = 0;
	while (%i < getWordCount(objectsInsideViewWindow))
	{
		%obj = getWord(objectsInsideViewWindow, %i);
		%obj.originalBlendColor = %obj.getBlendColor();
		%obj.originalBlendAlpha = %obj.getBlendAlpha();
		if (isPlayer(%obj) || %obj.getClassName() $= "t2dParticleEmmiter" || %obj.getClassNamespace() $= "LevelSwitchMask")
		{
			%this.objectsInsideViewWindow = removeWord(objectsInsideViewWindow, %i);
			%i = %i - 1.0;
		}
		else
		{
			if (!(%end))
			{
				%obj.setBlendColor("0", "0", "0");
				%obj.setBlendAlpha("0");
			}
		}
		%i = %i + 1.0;
	}
	%this.levelTransitionEnd = %end;
	%this.levelEndFader = globalSmoother.addSmoother("levelEndFader");
	levelEndFader.init("1.5", "0", "1", "SINC", "0");
	if (%startDirectly)
	{
		%this.levelTransitionBlend(%end);
	}
	return;
}
function camera::levelTransitionBlend(%this, %end)
{
	debugEcho("blending level at end!");
	levelEndFader.start(%end);
	subscribeToEvents(%this, "onUpdateTick15");
	return;
}
function camera::onUpdateTick10(%this)
{
	if (MoviePlayerFinished())
	{
		debugEcho("video stopped playing!");
		stopFuckingMovie();
		unSubscribeFromEvents(%this, "onUpdateTick10");
		%this.schedule("1000", "startDemoEndLevel");
	}
	return;
}
function camera::startDemoEndLevel(%this)
{
	MenuAction::loadLevel("level_demoEnd");
	return;
}
function camera::onUpdateTick15(%this)
{
	%value = levelEndFader.getValue();
	%i = 0;
	while (%i < getWordCount(objectsInsideViewWindow))
	{
		%obj = getWord(objectsInsideViewWindow, %i);
		if (levelEndAlpha == levelTransitionEnd)
		{
			%obj.setBlendAlpha(originalBlendAlpha * %value);
		}
		else
		{
			%color = multVector(originalBlendColor, %value SPC %value SPC %value);
			%obj.setBlendColor(getR(%color), getG(%color), getB(%color), originalBlendAlpha);
		}
		%i = %i + 1.0;
	}
	if (levelEndFader.getIsFinished())
	{
		if (levelEndAlpha != levelTransitionEnd)
		{
			%this.levelEndAlpha = levelTransitionEnd;
			levelEndFader.start(levelTransitionEnd);
			break;
		}
		unSubscribeFromEvents(%this, "onUpdateTick15");
	}
	return;
}
function camera::shake(%this, %magnitude, %time)
{
	if (isShaking)
	{
		sceneWindow.stopCameraShake();
	}
	%this.isShaking = "1";
	%this.safeSchedule(%time * 1000.0, "stopShake");
	sceneWindow.startCameraShake(%magnitude, %time);
	return;
}
function camera::stopShake(%this)
{
	if (isShaking)
	{
		sceneWindow.stopCameraShake();
		%this.isShaking = "0";
	}
	return;
}
function camera::startFreeRotation(%this, %dir)
{
	if (isRotating)
	{
		return camera;
	}
	camera.rotationDirection = %dir;
	%this.startRotation();
	subscribeToEvents(%this, "onUpdateFreeRotation");
	return;
}
function camera::stopFreeRotation(%this, %dir)
{
	if (!(isRotating))
	{
		return camera;
	}
	unSubscribeFromEvent(%this, "onUpdateFreeRotation");
	%this.onUpdateFreeRotation();
	%this.finishRotation();
	return;
}
function camera::remount(%this, %offset, %mountForce)
{
	if (!(%offset))
	{
		%offset = "0 0";
	}
	if (!(%mountForce))
	{
		%mountForce = cameraMountForce;
	}
	if (!($watchAsReplay))
	{
		sceneWindow2d.mount(player, %offset, %mountForce, "0");
		camera.mount(player, %offset, %mountForce, "0", "0", "0", "0");
		viewWindow.mount(player, %offset, %mountForce, "0", "0", "0", "0");
	}
	return;
}
