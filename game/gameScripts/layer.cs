// layer.cs.dso
function layerNode::onLevelLoadFinished(%this)
{
	scenegraph.pushToBack(%this);
	return;
}
function layerNode::onUpdateTick1(%this)
{
	%originPosition = Position;
	%dinstanceFromOrigin = t2dVectorSub(sceneWindow2d.getCurrentCameraPosition(), %originPosition);
	%curPos = %this.getPosition();
	%targetPosition = t2dVectorAdd(%originPosition, t2dVectorScale(%dinstanceFromOrigin, speedFactor));
	%this.setLinearVelocity(t2dVectorScale(t2dVectorSub(%targetPosition, %curPos), invDuration));
	return;
}
function t2dSceneObject::initLayerPosition(%this, %layerName, %setParalaxSize, %setParalaxPosition)
{
	if (%setParalaxSize $= "")
	{
		%setParalaxSize = 1;
	}
	if (%setParalaxPosition $= "")
	{
		%setParalaxPosition = 1;
	}
	%speedFactor = speedFactor;
	%scalingFactor = 1.0 - %speedFactor;
	if (%setParalaxSize)
	{
		%paralaxedSize = t2dVectorScale(%this.getSize(), %scalingFactor);
		%this.setSize(%paralaxedSize);
	}
	if (%setParalaxPosition)
	{
		%this.originalLayerPosition = %this.getPosition();
		%paralaxedPosition = t2dVectorScale(%this.getPosition(), %scalingFactor);
		%this.setPosition(%paralaxedPosition);
	}
	return;
}
function initLayers()
{
	if (!(isObject(worldOrigin)))
	{
		new t2dSceneObject(Name : worldOrigin)
		{
			Position = "0 0";
		}
	}
	worldOrigin.setSize("20 20");
	worldOrigin.Position = worldOrigin.getPosition();
	new SimSet(Name : layerNodeGroup);
	levelGarbageCollector.add(layerNodeGroup);
	$LAYER_NODE["background_paper"] = new t2dSceneObject(Name : "")
	{
		scenegraph = scenegraph;
		class = "LayerNode";
		speedFactor = $LAYER_SPEEDFACTOR["background_paper"];
	}
	layerNodeGroup.add($LAYER_NODE["background_paper"]);
	$LAYER_NODE["background_3"] = new t2dSceneObject(Name : "")
	{
		scenegraph = scenegraph;
		class = "LayerNode";
		speedFactor = $LAYER_SPEEDFACTOR["background_3"];
	}
	layerNodeGroup.add($LAYER_NODE["background_3"]);
	$LAYER_NODE["background_2"] = new t2dSceneObject(Name : "")
	{
		scenegraph = scenegraph;
		class = "LayerNode";
		speedFactor = $LAYER_SPEEDFACTOR["background_2"];
	}
	layerNodeGroup.add($LAYER_NODE["background_2"]);
	$LAYER_NODE["background_1"] = new t2dSceneObject(Name : "")
	{
		scenegraph = scenegraph;
		class = "LayerNode";
		speedFactor = $LAYER_SPEEDFACTOR["background_1"];
	}
	layerNodeGroup.add($LAYER_NODE["background_1"]);
	$LAYER_NODE["foreground_1"] = new t2dSceneObject(Name : "")
	{
		scenegraph = scenegraph;
		class = "LayerNode";
		speedFactor = $LAYER_SPEEDFACTOR["foreground_1"];
	}
	layerNodeGroup.add($LAYER_NODE["foreground_1"]);
	$LAYER_NODE["foreground_2"] = new t2dSceneObject(Name : "")
	{
		scenegraph = scenegraph;
		class = "LayerNode";
		speedFactor = $LAYER_SPEEDFACTOR["foreground_2"];
	}
	layerNodeGroup.add($LAYER_NODE["foreground_2"]);
	$LAYER_NODE["foreground_3"] = new t2dSceneObject(Name : "")
	{
		scenegraph = scenegraph;
		class = "LayerNode";
		speedFactor = $LAYER_SPEEDFACTOR["foreground_3"];
	}
	layerNodeGroup.add($LAYER_NODE["foreground_3"]);
	globals.storeLayerNodes();
	%i = 0;
	while (%i < layerNodeGroup.getCount())
	{
		%layerNode = layerNodeGroup.getObject(%i);
		%layerNode.setSize(t2dVectorScale(worldOrigin.getSize(), 1.0 - speedFactor));
		%layerNode.setPosition(worldOrigin.getPosition());
		subscribeToEvents(%layerNode, "onLevelLoadFinished onUpdateTick1");
		%i = %i + 1.0;
	}
	return layerNodeGroup.getCount();
}
