// levelManagement.cs.dso
$lastLoadedScene = "";
function getLastLoadedScene()
{
	return $lastLoadedScene;
	return $lastLoadedScene;
}
$useNewSceneGraph = 0;
function t2dSceneWindow::loadLevel(%sceneWindow, %levelFile)
{
	%sceneWindow.endLevel();
	$useNewSceneGraph = 1;
	%scenegraph = %sceneWindow.addToLevel(%levelFile);
	if (!(isObject(%scenegraph)))
	{
		return "0";
	}
	%sceneWindow.setSceneGraph(%scenegraph);
	%cameraPosition = %sceneWindow.getCurrentCameraPosition();
	%cameraSize = t2dVectorSub(getWords(%sceneWindow.getCurrentCameraArea(), "2", "3"), getWords(%sceneWindow.getCurrentCameraArea(), "0", "1"));
	if (cameraPosition != "")
	{
		%cameraPosition = cameraPosition;
	}
	if (cameraSize != "")
	{
		%cameraSize = cameraSize;
	}
	%sceneWindow.setCurrentCameraPosition(%cameraPosition, %cameraSize);
	$lastLoadedScene = %scenegraph;
	return %scenegraph;
	return %scenegraph;
}
function t2dSceneWindow::addToLevel(%sceneWindow, %levelFile)
{
	%scenegraph = %sceneWindow.getSceneGraph();
	if (!(isObject(%scenegraph)))
	{
		%scenegraph = new t2dSceneGraph(Name : "");
		%sceneWindow.setSceneGraph(%scenegraph);
	}
	%newScenegraph = %scenegraph.addToLevel(%levelFile);
	$lastLoadedScene = %newScenegraph;
	return %newScenegraph;
	return %newScenegraph;
}
function t2dSceneGraph::addToLevel(%scenegraph, %levelFile)
{
	%useNewSceneGraph = $useNewSceneGraph;
	$useNewSceneGraph = 0;
	%scenegraph = %scenegraph.getId();
	if (!(isFile(%levelFile)) && !(isFile(%levelFile @ ".dso")))
	{
		error("Error loading level " @ %levelFile @ ". Invalid file.");
		return "0";
	}
	%t = getRealTime();
	exec(%levelFile);
	debugEcho("*** exec level took" SPC getRealTime() - %t);
	if (!(isObject(%levelContent)))
	{
		error("Invalid level file specified: " @ %levelFile);
		return "0";
	}
	%newScenegraph = %scenegraph;
	%object = %levelContent;
	$LevelManagement::newObjects = "";
	if (%object.getClassName() $= "t2dSceneObjectGroup")
	{
		%newScenegraph.addToScene(%object);
		%i = 0;
		while (%i < %object.getCount())
		{
			%obj = %object.getObject(%i);
			if (%obj.getClassName() $= "t2dParticleEffect")
			{
				%newScenegraph.addToScene(%obj);
				%oldPosition = %obj.getPosition();
				%oldSize = %obj.getSize();
				%obj.loadEffect(effectFile);
				%obj.setPosition(%oldPosition);
				%obj.setSize(%oldSize);
				%obj.playEffect();
			}
			else
			{
				if (%obj.getClassName() $= "t2dTileLayer")
				{
					%oldPosition = %obj.getPosition();
					%oldSize = %obj.getSize();
					%tileMap = %newScenegraph.getGlobalTileMap();
					if (isObject(%tileMap))
					{
						%tileMap.addTileLayer(%obj);
						%obj.loadTileLayer(LayerFile);
						%obj.setPosition(%oldPosition);
						%obj.setSize(%oldSize);
						break;
					}
					error("Unable to find scene graph's global tile map.");
				}
			}
			%i = %i + 1.0;
		}
		$LevelManagement::newObjects = %object;
	}
	else
	{
		if (%object.getClassName() $= "t2dSceneObjectSet")
		{
			%i = 0;
			while (%i < %object.getCount())
			{
				%obj = %object.getObject(%i);
				%newScenegraph.addToScene(%obj);
				if (%obj.getClassName() $= "t2dParticleEffect")
				{
					%oldPosition = %obj.getPosition();
					%oldSize = %obj.getSize();
					%obj.loadEffect(effectFile);
					%obj.setPosition(%oldPosition);
					%obj.setSize(%oldSize);
					%obj.playEffect();
				}
				else
				{
					if (%obj.getClassName() $= "t2dTileLayer")
					{
						%oldPosition = %obj.getPosition();
						%oldSize = %obj.getSize();
						%tileMap = %newScenegraph.getGlobalTileMap();
						if (isObject(%tileMap))
						{
							%tileMap.addTileLayer(%obj);
							%obj.loadTileLayer(LayerFile);
							%obj.setPosition(%oldPosition);
							%obj.setSize(%oldSize);
							break;
						}
						error("Unable to find scene graph's global tile map.");
					}
				}
				%i = %i + 1.0;
			}
			$LevelManagement::newObjects = %object;
			break;
		}
		if (%object.isMemberOfClass("t2dSceneObject"))
		{
			if (%object.getClassName() $= "t2dParticleEffect")
			{
				%newScenegraph.addToScene(%object);
				%oldPosition = %object.getPosition();
				%oldSize = %object.getSize();
				%object.loadEffect(effectFile);
				%object.setPosition(%oldPosition);
				%object.setSize(%oldSize);
				%object.playEffect();
			}
			else
			{
				if (%object.getClassName() $= "t2dTileLayer")
				{
					%oldPosition = %object.getPosition();
					%oldSize = %object.getSize();
					%tileMap = %newScenegraph.getGlobalTileMap();
					if (isObject(%tileMap))
					{
						%tileMap.addTileLayer(%object);
						%object.loadTileLayer(LayerFile);
						%object.setPosition(%oldPosition);
						%object.setSize(%oldSize);
					}
					else
					{
						error("Unable to find scene graph's global tile map.");
					}
					break;
				}
				%newScenegraph.addToScene(%object);
			}
			$LevelManagement::newObjects = %object;
			break;
		}
		if (%object.getClassName() $= "t2dSceneGraph")
		{
			%fromSceneGraph = 0;
			%toSceneGraph = 0;
			%newScenegraph = %levelContent;
			%scenegraph.delete();
			break;
		}
		error("Error loading level " @ %levelFile @ ". " @ %object.getClassName() @ " is not a valid level object type.");
		return "0";
	}
	%newScenegraph.performPostInit();
	$lastLoadedScene = %newScenegraph;
	return %newScenegraph;
	return %newScenegraph;
}
function t2dSceneWindow::endLevel(%sceneWindow)
{
	%scenegraph = %sceneWindow.getSceneGraph();
	if (!(isObject(%scenegraph)))
	{
		return isObject(%scenegraph);
	}
	%scenegraph.endLevel();
	if (isObject(%scenegraph))
	{
		if (isObject(%scenegraph.getGlobalTileMap()))
		{
			%scenegraph.getGlobalTileMap().delete();
		}
		%scenegraph.delete();
	}
	$lastLoadedScene = "";
	return;
}
function t2dSceneGraph::endLevel(%scenegraph)
{
	%globalTileMap = %scenegraph.getGlobalTileMap();
	if (isObject(%globalTileMap))
	{
		%scenegraph.removeFromScene(%globalTileMap);
	}
	%scenegraph.clearScene("1");
	if (isObject(%globalTileMap))
	{
		%scenegraph.addToScene(%globalTileMap);
	}
	$lastLoadedScene = "";
	return;
}
