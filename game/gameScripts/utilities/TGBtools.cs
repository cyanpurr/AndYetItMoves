// TGBtools.cs.dso
function adjustParts(%maskLayer, %maskTexture, %protoObject)
{
	%group = getSelectedItems();
	%i = 0;
	while (%i < %group.getCount())
	{
		%obj = %group.getObject(%i);
		%maskBehavior = %obj.getBehavior("BeMask");
		if (!(isObject(%maskBehavior)))
		{
		}
		else
		{
			if ($LAYER[%maskLayer] != "")
			{
				%maskBehavior.Layer = %maskLayer;
				Owner.setLayer($LAYER[%maskLayer]);
			}
			if (isObject(%maskTexture))
			{
				%maskBehavior.setTexture(%maskTexture);
			}
			else
			{
				if (%maskTexture $= "none")
				{
					%maskBehavior.textureObject = "None";
					Owner.textureObject = "";
				}
			}
			if (isObject(%protoObject) || %protoObject $= "none")
			{
				%maskBehavior.protoObject = %protoObject;
			}
		}
		%i = %i + 1.0;
	}
	return %group.getCount();
}
function replaceTex(%oldTex, %newTex)
{
	%group = getSelectedItems();
	%i = 0;
	while (%i < %group.getCount())
	{
		%obj = %group.getObject(%i);
		%maskBehavior = %obj.getBehavior("BeMask");
		if (!(isObject(%maskBehavior)) || !(isObject(textureObject)))
		{
		}
		else
		{
			if (%oldTex.getId() == textureObject.getId() && isObject(%newTex))
			{
				%maskBehavior.setTexture(%newTex);
			}
		}
		%i = %i + 1.0;
	}
	return %group.getCount();
}
function replaceProto(%oldProto, %newProto)
{
	%group = getSelectedItems();
	%i = 0;
	while (%i < %group.getCount())
	{
		%obj = %group.getObject(%i);
		%maskBehavior = %obj.getBehavior("BeMask");
		if (!(isObject(%maskBehavior)))
		{
		}
		else
		{
			if (%oldProto.getId() == protoObject.getId() && isObject(%newProto))
			{
				%maskBehavior.protoObject = %newProto;
			}
		}
		%i = %i + 1.0;
	}
	return %group.getCount();
}
function setObjectParam(%type, %param1, %param2)
{
	if (%type $= "texture")
	{
		adjustParts(%param1, %param2);
		return;
	}
	if (%type $= "proto")
	{
		adjustParts(%param1, "", %param2);
		return;
	}
	%typeTable["size"] = 1;
	%typeTable["addBehavior"] = 2;
	%typeTable["mountCondition"] = 3;
	%typeTable["className"] = 4;
	%typeTable["mountRotationTracking"] = 5;
	%typeTable["layer"] = 6;
	%typeTable["autoRotate"] = 7;
	%typeTable["protoObject"] = 8;
	%typeTable["useMaskRotation"] = 9;
	%typeTable["removeBehavior"] = 10;
	%typeTable["setImmovable"] = 11;
	%typeTable["autoBurn"] = 12;
	%typeTable["layerModification"] = 13;
	%typeTable["flameableAutoBurn"] = 14;
	%typeTable["flameableCreateFlames"] = 15;
	%typeTable["maskLayer"] = 16;
	%typeTable["hasObjectCollision"] = 17;
	%typeTable["visible"] = 18;
	%typeTable["pulsator"] = 19;
	%typeTable["layersToClone"] = 20;
	%typeTable["mountMother"] = 21;
	%typeTable["imageMap"] = 22;
	%typeTable["frame"] = 23;
	%typeTable["pulsator"] = 24;
	%typeTable["layerModification"] = 25;
	%typeTable["dynamicField"] = 26;
	%typeTable["useParallaxTexture"] = 27;
	%typeTable["trackTextureAngle"] = 28;
	%typeTable["deleteEmptyImages"] = 29;
	%typeTable["addToGroup"] = 30;
	%typeTable["pushAllToBack"] = 31;
	%typeTable["pulsatingObjectWhichSide"] = 32;
	%typeTable["addLinkPoint"] = 33;
	%typeTable["pulsatorUseLinkPoint"] = 34;
	%typeTable["copyTexToFadeTexBehavior"] = 35;
	%typeTable["setFadeMask"] = 36;
	%typeTable["useMaskOnly4Parallaxing"] = 37;
	%typeTable["setFadeTex"] = 38;
	%typeTable["setOnlyDeko"] = 39;
	%typeTable["setOnRotation"] = 40;
	%typeTable["setLoop4ever"] = 41;
	%typeTable["setMountTexture"] = 42;
	%typeTable["setUsePositive"] = 43;
	%typeTable["customTextureAngleUseLinkPoint"] = 44;
	%typeTable["setNoLayerBlending"] = 45;
	%typeTable["mustBeVisible"] = 46;
	%typeTable["texBlending"] = 47;
	%typeTable["setRotation"] = 48;
	%typeTable["randomRotation"] = 49;
	%group = getSelectedItems();
	%objectsToDelete = "";
	%i = 0;
	while (%i < %group.getCount())
	{
		%obj = %group.getObject(%i);
		%mountBehavior = %obj.getBehavior("BeMountWithOffset");
		%maskBehavior = %obj.getBehavior("BeMask");
		%flameableBehavior = %obj.getBehavior("BeFlameable");
		%pulsateBehavior = %obj.getBehavior("BePulsatingObject");
		%textureFadeBehavior = %obj.getBehavior("BeTextureFade");
		%fadeOnRotationBehavior = %obj.getBehavior("BeFadeOnRotation");
		%rotateBehavior = %obj.getBehavior("BeRotate");
		%translateBehavior = %obj.getBehavior("BeTranslate");
		%customTextureAngleBehavior = %obj.getBehavior("BeCustomTextureAngle");
		if (%typeTable[%type] == 1.0)
		{
			%obj.setSize(%param1);
		}
		else
		{
			if (%typeTable[%type] == 2.0)
			{
				%obj.addDependentBehavior(%param1);
				break;
			}
			if (%typeTable[%type] == 3.0)
			{
				if (isObject(%mountBehavior))
				{
					%mountBehavior.searchParentCondition = %param1;
				}
				break;
			}
			if (%typeTable[%type] == 4.0)
			{
				%obj.setClassNamespace(%param1);
				break;
			}
			if (%typeTable[%type] == 5.0)
			{
				if (isObject(%mountBehavior))
				{
					%mountBehavior.trackRotation = %param1;
				}
				break;
			}
			if (%typeTable[%type] == 6.0)
			{
				%obj.setLayer(%param1);
				break;
			}
			if (%typeTable[%type] == 7.0)
			{
				if (isObject(%maskBehavior))
				{
					%maskBehavior.useMaskRotationForTexture = %param1;
				}
				break;
			}
			if (%typeTable[%type] == 8.0)
			{
				if (isObject(%maskBehavior))
				{
					%maskBehavior.protoObject = %param1;
				}
				break;
			}
			if (%typeTable[%type] == 9.0)
			{
				if (isObject(%maskBehavior))
				{
					%maskBehavior.useMaskRotationForTexture = %param1;
				}
				break;
			}
			if (%typeTable[%type] == 10.0)
			{
				%behavior = %obj.getBehavior(%param1);
				if (isObject(%behavior))
				{
					%obj.removeBehavior(%behavior);
				}
				break;
			}
			if (%typeTable[%type] == 11.0)
			{
				%collide = %obj.getBehavior(BeCollide);
				if (isObject(%collide))
				{
					%collide.Immovable = %param1;
				}
				%rippedEdge = %obj.getBehavior(BeRippedEdge);
				if (isObject(%rippedEdge))
				{
					%rippedEdge.Immovable = %param1;
				}
				break;
			}
			if (%typeTable[%type] == 12.0)
			{
				%flameable = %obj.getBehavior(BeFlameable);
				if (isObject(%flameable))
				{
					%flameable.autoBurn = %param1;
					debugEcho("setting autoBurn of" SPC %obj SPC "to" SPC %param1);
				}
				break;
			}
			if (%typeTable[%type] == 13.0)
			{
				if (isObject(%maskBehavior))
				{
					%maskBehavior.layerModification = %param1;
					debugEcho("setting layerModification of" SPC %obj SPC "to" SPC %param1);
				}
				break;
			}
			if (%typeTable[%type] == 14.0)
			{
				if (isObject(%flameableBehavior))
				{
					%flameableBehavior.autoBurn = %param1;
				}
				break;
			}
			if (%typeTable[%type] == 15.0)
			{
				if (isObject(%flameableBehavior))
				{
					%flameableBehavior.createFlames = %param1;
				}
				break;
			}
			if (%typeTable[%type] == 16.0)
			{
				if (isObject(%maskBehavior))
				{
					%maskBehavior.Layer = %param1;
				}
				break;
			}
			if (%typeTable[%type] == 17.0)
			{
				%rippedEdge = %obj.getBehavior(BeRippedEdge);
				if (isObject(%rippedEdge))
				{
					%rippedEdge.hasObjectCollision = %param1;
					debugEcho("setting hasObjectCollision of" SPC %obj SPC "to" SPC %param1);
				}
				break;
			}
			if (%typeTable[%type] == 18.0)
			{
				if (isObject(%obj))
				{
					%obj.setVisible(%param1);
					debugEcho("setting visibility of" SPC %obj SPC "to" SPC %param1);
				}
				break;
			}
			if (%typeTable[%type] == 19.0)
			{
				%pulsatingObject = %obj.getBehavior(BePulsatingObject);
				if (isObject(%pulsatingObject))
				{
					%pulsatingObject.Pulsator = %param1;
					debugEcho("setting pulsator of" SPC %obj SPC "to" SPC %param1);
				}
				break;
			}
			if (%typeTable[%type] == 20.0)
			{
				%appearOnBeat = %obj.getBehavior(BeAppearOnBeat);
				if (isObject(%appearOnBeat))
				{
					%appearOnBeat.layersToClone = %param1;
					debugEcho("setting layersToClone of" SPC %obj SPC "to" SPC layersToClone);
				}
				break;
			}
			if (%typeTable[%type] == 21.0)
			{
				if (isObject(%mountBehavior))
				{
					%mountBehavior.mother = %param1;
				}
				break;
			}
			if (%typeTable[%type] == 22.0)
			{
				if (%obj.getClassName() $= "t2dStaticSprite")
				{
					%obj.setImageMap(%param1);
					debugEcho("setting imageMap of" SPC %obj SPC "to" SPC %obj.getImageMap());
				}
				break;
			}
			if (%typeTable[%type] == 23.0)
			{
				if (%obj.getClassName() $= "t2dStaticSprite")
				{
					%obj.setFrame(%param1);
					debugEcho("setting imageMap of" SPC %obj SPC "to" SPC %obj.getFrame());
				}
				break;
			}
			if (%typeTable[%type] == 24.0)
			{
				if (isObject(%pulsateBehavior))
				{
					%pulsateBehavior.Pulsator = %param1;
				}
				break;
			}
			if (%typeTable[%type] == 25.0)
			{
				if (isObject(%maskBehavior))
				{
					%maskBehavior.layerModification = %param1;
				}
				break;
			}
			if (%typeTable[%type] == 26.0)
			{
				%string = %obj @ "." @ %param1 @ "=" @ %param2 @ ";";
				eval(%string);
				break;
			}
			if (%typeTable[%type] == 27.0)
			{
				if (isObject(%maskBehavior))
				{
					%maskBehavior.parallaxScrollTexture = %param1;
				}
				break;
			}
			if (%typeTable[%type] == 28.0)
			{
				if (isObject(%maskBehavior))
				{
					%maskBehavior.trackTextureAngle = %param1;
				}
				break;
			}
			if (%typeTable[%type] == 29.0)
			{
				if (%obj.getClassName() $= "t2dStaticSprite" && %obj.getImageMap() $= "")
				{
					%objectsToDelete = %objectsToDelete SPC %obj;
				}
				break;
			}
			if (%typeTable[%type] == 30.0)
			{
				%param1.add(%obj);
				break;
			}
			if (%typeTable[%type] == 31.0)
			{
				daSceneGraph.setLayerDrawOrder(%obj, "BACK");
				break;
			}
			if (%typeTable[%type] == 32.0)
			{
				if (isObject(%pulsateBehavior))
				{
					%pulsateBehavior.whichSide = %param1;
					debugEcho("setting whichSide of" SPC %obj SPC "to" SPC %param1);
				}
				break;
			}
			if (%typeTable[%type] == 33.0)
			{
				%obj.addLinkPoint(%param1);
				break;
			}
			if (%typeTable[%type] == 34.0)
			{
				if (isObject(%pulsateBehavior))
				{
					%pulsateBehavior.useLinkPoint = %param1;
					debugEcho("setting useLinkPoint of" SPC %obj SPC "to" SPC %param1);
				}
				break;
			}
			if (%typeTable[%type] == 35.0)
			{
				if (isObject(%maskBehavior) && isObject(%textureFadeBehavior) && !(isObject(fadeTexture)))
				{
					%textureFadeBehavior.fadeTexture = textureObject;
				}
				break;
			}
			if (%typeTable[%type] == 36.0)
			{
				if (isObject(%textureFadeBehavior))
				{
					%textureFadeBehavior.fadeMask = %param1;
				}
				break;
			}
			if (%typeTable[%type] == 37.0)
			{
				if (isObject(%maskBehavior))
				{
					%maskBehavior.useOnlyParallaxing = %param1;
				}
				break;
			}
			if (%typeTable[%type] == 38.0)
			{
				if (isObject(%textureFadeBehavior))
				{
					%textureFadeBehavior.fadeTexture = %param1;
				}
				break;
			}
			if (%typeTable[%type] == 39.0)
			{
				if (isObject(%fadeOnRotationBehavior))
				{
					%fadeOnRotationBehavior.onlyDeko = %param1;
				}
				else
				{
					debugEcho("this object" SPC %obj SPC "has no fade on rotation behavior");
				}
				break;
			}
			if (%typeTable[%type] == 40.0)
			{
				if (isObject(%fadeOnRotationBehavior))
				{
					%fadeOnRotationBehavior.onRotation = %param1;
				}
				else
				{
					debugEcho("this object" SPC %obj SPC "has no fade on rotation behavior");
				}
				break;
			}
			if (%typeTable[%type] == 41.0)
			{
				if (isObject(%rotateBehavior))
				{
					%rotateBehavior.loop4ever = %param1;
				}
				if (isObject(%translateBehavior))
				{
					%translateBehavior.loop4ever = %param1;
				}
				break;
			}
			if (%typeTable[%type] == 42.0)
			{
				if (isObject(%maskBehavior))
				{
					%maskBehavior.mountTexture = %param1;
				}
				else
				{
					debugEcho("this object" SPC %obj SPC "has no mask behavior");
				}
				break;
			}
			if (%typeTable[%type] == 43.0)
			{
				if (isObject(%maskBehavior))
				{
					%maskBehavior.usePositive = %param1;
				}
				else
				{
					debugEcho("this object" SPC %obj SPC "has no mask behavior");
				}
				break;
			}
			if (%typeTable[%type] == 44.0)
			{
				if (isObject(%customTextureAngleBehavior))
				{
					%customTextureAngleBehavior.useLinkPointForPicking = %param1;
				}
				break;
			}
			if (%typeTable[%type] == 45.0)
			{
				if (isObject(%maskBehavior))
				{
					%maskBehavior.noLayerBlending = %param1;
				}
				else
				{
					debugEcho("this object" SPC %obj SPC "has no mask behavior");
				}
				break;
			}
			if (%typeTable[%type] == 46.0)
			{
				if (isObject(%flameableBehavior))
				{
					%flameableBehavior.mustBeVisible = %param1;
				}
				break;
			}
			if (%typeTable[%type] == 47.0)
			{
				if (isObject(%maskBehavior))
				{
					%maskBehavior.useTexBlending = %param1;
				}
				else
				{
					debugEcho("this object" SPC %obj SPC "has no mask behavior");
				}
				break;
			}
			if (%typeTable[%type] == 48.0)
			{
				%obj.setRotation(%param1);
				break;
			}
			if (%typeTable[%type] == 49.0)
			{
				%obj.setRotation(getRandom("0", "360"));
				break;
			}
			debugEcho("couldnt find your command:" SPC %type SPC "typo?");
		}
		%i = %i + 1.0;
	}
	%objectsToDelete = trim(%objectsToDelete);
	%i = 0;
	while (%i < getWordCount(%objectsToDelete))
	{
		%obj = getWord(%objectsToDelete, %i);
		%obj.delete();
		%i = %i + 1.0;
	}
	return getWordCount(%objectsToDelete);
}
function makeRotatingDekoLeaves(%onMainLayer)
{
	setObjectParam(addBehavior, bemountwithoffset);
	setObjectParam(mountrotationtracking, "1");
	if (%onMainLayer)
	{
		%mountCondition = "%parent.getBehavior(BeCollide)";
	}
	else
	{
		%mountCondition = "%parent.getBehavior(BeMask) && %parent.getLayer() == %owner.getLayer()";
	}
	setObjectParam(mountcondition, %mountCondition);
	setObjectParam(mountcondition, "%parent.getBehavior(BeCollide)");
	return;
}
function onlyShowLayer(%layerNumber)
{
	ToolManager.getLastWindow().setRenderMasks(addBitToMask("0", %layerNumber));
	return;
}
function onlyShowLayers(%layerNumberList)
{
	ToolManager.getLastWindow().setRenderMasks(bits(%layerNumberList));
	return;
}
function showLayerType(%typeList)
{
	%layerList = "";
	%i = 0;
	while (%i < getWordCount(%typeList))
	{
		%type = getWord(%typeList, %i);
		if (%type $= "FRONT")
		{
			%layerList = %layerList SPC $LAYER["foreground_3"] SPC $LAYER["foreground_2"] SPC $LAYER["foreground_1"];
		}
		else
		{
			if (%type $= "MAIN")
			{
				%layerList = %layerList SPC $LAYER["main_background"] SPC $LAYER["main"] SPC $LAYER["main_foreground"];
				break;
			}
			if (%type $= "NODEKO")
			{
				%layerList = %layerList SPC $LAYER["main_background"] SPC $LAYER["main"] SPC $LAYER["main_foreground"] SPC $LAYER["background_paper"] SPC $LAYER["mainBehindPlayer"] SPC $LAYER["collide"] SPC $LAYER["rippedEdge"];
				break;
			}
			if (%type $= "BACK")
			{
				%layerList = %layerList SPC $LAYER["background_1"] SPC $LAYER["background_2"] SPC $LAYER["background_3"];
				break;
			}
			if (%type $= "BUILD_MAIN")
			{
				%layerList = %layerList SPC $LAYER["collide"] SPC $LAYER["rippedEdge"] SPC $LAYER["main"] SPC $LAYER["main_background"] SPC $LAYER["main_foreground"];
				break;
			}
			if (%type $= "BUILD_VISUAL")
			{
				%layerList = %layerList SPC $LAYER["background_1"] SPC $LAYER["background_2"] SPC $LAYER["background_3"] SPC $LAYER["rippedEdge"] SPC $LAYER["main"] SPC $LAYER["main_background"] SPC $LAYER["main_foreground"] SPC $LAYER["foreground_3"] SPC $LAYER["foreground_2"] SPC $LAYER["foreground_1"];
			}
		}
		%i = %i + 1.0;
	}
	onlyShowLayers(trim(%layerList));
	return;
}
function alsoShowLayer(%layer)
{
	ToolManager.getLastWindow().setRenderMasks(addBitToMask(ToolManager.getLastWindow().getRenderLayerMask(), %layer));
	return;
}
function storeLayers()
{
	$storedLayerMask = ToolManager.getLastWindow().getRenderLayerMask();
	return;
}
function restoreLayers()
{
	ToolManager.getLastWindow().setRenderMasks($storedLayerMask);
	return;
}
function showAllLayers(%storeView)
{
	if (%storeView)
	{
		storeLayers();
	}
	%bitMask = 0;
	%i = 0;
	while (%i <= 31.0)
	{
		%bitMask = addBitToMask(%bitMask, %i);
		%i = %i + 1.0;
	}
	ToolManager.getLastWindow().setRenderMasks(%bitMask);
	return;
}
function countSelection()
{
	debugEcho("currently selected" SPC getSelectedItems().getCount() SPC "objetcs");
	return;
}
function getSelectedItems()
{
	ToolManager.getLastWindow().setFirstResponder();
	return ToolManager.getAcquiredObjects();
	return ToolManager.getAcquiredObjects();
}
function reloadTGBtools()
{
	exec("./TGBtools.cs");
	return;
}
function popFonts()
{
	populateFontCacheString(akbar, "20", "!#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[]^_`abcdefghijklmnopqrstuvwxyz{|}~Â°Â¢Â£Â§â¢Â¶ÃÂ®Â©â¢Â´Â¨â ÃÃâÂ±â¤â¥Â¥âââÏâ«ÂªÂºÎ©Ã¦Ã¸Â¿Â¡Â¬âÆâÂ«Â»â¦Â ÃÃÃÅÅââââââÃ·âÃ¿Å¸ââ¬â¹âºï¬â¡Â·âââ°ÃÃÃÃÃÃÃÃÃÃï£¿ÃÃÃÃÄ±ËËÂ¯ËËËÂ¸ËAaCcCcCcCcâdEeEeEeGgGgHhIiIiIiJjLlLl??LlNnNn?OoÃ¥ÃºRrRrSsSsÃ¤Ã¶TtTtUuUuUuWwYyÃ¼ZzZzÃ©Ã»Ã ?Ã?âÃ²?????????--?Ã±Ã³Ã«Ã­ÃÃ¬Ã®ÃÃÃ¡Ã¯ÃÃÃ´");
	return;
}
