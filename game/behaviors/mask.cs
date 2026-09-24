// mask.cs.dso
if (!(isObject(BeMask)))
{
	if (!(isTorquePlayer()))
	{
		%template = new BehaviorTemplate(Name : BeMask);
	}
	else
	{
		%template = new BeMaskTemplate(Name : BeMask);
	}
	%template.friendlyName = "Mask";
	%template.behaviorType = "Visual";
	%template.description = "make an object handled as a mask - set texture";
	%template.addBehaviorField(Layer, "1st, 2nd or 3rd(frontmost) foreroundlayer, mainlayer or 1st, 2nd or 3rd (all-the-way-back) backgroundlayer", enum, "main", $LAYER_ENUM);
	%template.addBehaviorField(layerModification, "modify the layernumber by this value (+/-); - is to front, + to back", int, "0");
	%template.addBehaviorField(protoObject, "Object that is used as Prototype for this", object, null, t2dSceneObject);
	%template.addBehaviorField(textureObject, "Texture that is used for masking", object, null, t2dSceneObject);
	%template.addBehaviorField(trackTextureAngle, "If relative angle between protomask and its proto texture should be kept", bool, "0");
	%template.addBehaviorField(parallaxScrollTexture, "if true the texture will be set on and mounted to the same layer(node) as the mask", bool, "1");
	%template.addBehaviorField(usePositive, "black or or white?!.. taetaeraetaeaeaeae", bool, "0");
	%template.addBehaviorField(dontCollide, "use dontCollide behavior?", bool, "1");
	%template.addBehaviorField(useTexBlending, "use blending of texture object to blend ONLY the texture before its applied to the mask? (blending of mask is also applied afterwards)", bool, "0");
	%template.addBehaviorField(noLayerBlending, "set this to true if you don't want blending for the parallax layers", bool, "0");
	%template.addBehaviorField(setParallaxSize, "wheter or not this mask shcould adjust its size according to the layer it is on", bool, "1");
	%template.addBehaviorField(useMaskRotationForTexture, "the texture shall look into direktion of mask", bool, "0");
	%template.addBehaviorField(useOnlyParallaxing, "it no mask at all! only needs parallaxing", bool, "0");
	%template.addBehaviorField(mountTexture, "if true the mask will clone the texture and mount it to itself taking the original offset into account. use wisely!", bool, "0");
}
function BeMask::onBehaviorAdd(%this)
{
	%owner = Owner;
	subscribeToEvents(%this, "collectObjectsInBehavior onLevelLoadFinished2 onLevelLoadFinished10 onLevelLoadFinished onUpdateFirstTick20");
	if (%owner.getClassName() $= "t2dShapeVector")
	{
		%owner.setLineColor("1 1 1");
		%owner.setFillMode("1");
	}
	%owner.textureObject = textureObject;
	if (Layer != "custom")
	{
		%owner.setLayer($LAYER[Layer]);
	}
	return;
}
function BeMask::onLevelLoadFinished2(%this)
{
	%owner = Owner;
	if (%owner.getClassName() $= "t2dShapeVector")
	{
		%owner.setBlendColor("1 1 1");
		%owner.setFillColor("1 1 1");
	}
	else
	{
		if ($WII && %owner.getClassName() != "t2dSceneObject" && %owner.getImageMap() $= "blackImageMap")
		{
			%owner.setImageMap("whiteImageMap1");
		}
	}
	%this.originalBlendColor = %owner.getBlendColor();
	if (isObject(protoObject))
	{
		%this.applyProtoObject();
	}
	else
	{
		if (mountTexture && !(%this.isTextureMountedToUs()))
		{
			%this.cloneTexture("1");
		}
	}
	if (findWord($PARALAXLAYER_ENUM, Layer))
	{
		%owner.initLayerPosition(Layer, setParallaxSize);
		if (!(isObject(%owner.getBehavior("BeMountWithOffset"))))
		{
			%mountWithOffsetBehavior = %owner.addDependentBehavior("BeMountWithOffset");
			%mountWithOffsetBehavior.mother = $LAYER_NODE[Layer];
		}
		if (parallaxScrollTexture && isObject(textureObject))
		{
			if (!(isObject(protoObject)) && !(%this.isTextureMountedToUs()) && !(mountTexture))
			{
				%newName = textureObject.getName() @ "_" @ Layer;
				if (!(isObject(%newName)))
				{
					%newTexture = textureObject.createCopy();
					%newTexture.setName(%newName);
				}
				%this.setTexture(%newName);
			}
			textureObject.getBehavior("BeTexture").Layer = Layer;
		}
	}
	return;
}
function BeMask::isTextureMountedToUs(%this)
{
	%owner = Owner;
	if (isObject(textureObject))
	{
		%texParent = textureObject.getMountedParent();
	}
	if (isObject(%texParent))
	{
		if (%texParent.getId() == %owner.getId())
		{
			return "1";
		}
	}
	return "0";
	return "0";
}
function BeMask::onLevelLoadFinished(%this)
{
	%owner = Owner;
	if (%owner.getIsMounted() && escapeSwitch)
	{
		%owner.escapeSwitch = "1";
	}
	return;
}
function BeMask::onUpdateFirstTick20(%this)
{
	%owner = Owner;
	if (!(noLayerBlending) && Layer != "custom")
	{
		%owner.setLayer($LAYER[Layer] + layerModification);
	}
	if (originalPosition != "")
	{
		%owner.setPosition(originalPosition);
	}
	return;
}
function BeMask::onShowParalaxLayers(%this)
{
	%owner = Owner;
	%this.enableMask("1");
	if (isObject(%owner.getBehavior("BeRotate")))
	{
		if (isObject(%owner.getBehavior("BeCounterRotate")))
		{
			%owner.getBehavior("BeCounterRotate").switchOn("1");
			break;
		}
		if (isObject(%owner.getBehavior("BeLookAtRotate")))
		{
			%owner.getBehavior("BeLookAtRotate").switchOn("1");
			break;
		}
		%owner.getBehavior("BeRotate").switchOn("1");
	}
	if (isObject(%owner.getBehavior("BeContinuouslyRotate")))
	{
		%owner.getBehavior("BeContinuouslyRotate").switchOn("1");
	}
	return;
}
function BeMask::onHideParalaxLayers(%this)
{
	%owner = Owner;
	if (isObject(%owner.getBehavior("BeRotate")))
	{
		if (isObject(%owner.getBehavior("BeCounterRotate")))
		{
			%owner.getBehavior("BeCounterRotate").switchOff("1");
			break;
		}
		if (isObject(%owner.getBehavior("BeLookAtRotate")))
		{
			%owner.getBehavior("BeLookAtRotate").switchOff("1");
			break;
		}
		%owner.getBehavior("BeRotate").switchOff("1");
	}
	if (isObject(%owner.getBehavior("BeContinuouslyRotate")))
	{
		%owner.getBehavior("BeContinuouslyRotate").switchOff("1");
	}
	%this.enableMask("0");
	return;
}
function BeMask::enableMask(%this, %enable)
{
	Owner.setEnabled(%enable);
	return;
}
function BeMask::setMaskUsePositive(%this, %usePositive)
{
	%this.usePositive = %usePositive;
	return;
}
function BeMask::setTexture(%this, %texture, %relink)
{
	%this.textureObject = %texture;
	Owner.textureObject = %texture;
	if (%relink)
	{
		%this.unlink();
		%this.link();
	}
	return;
}
function BeMask::initMaskLayer(%this)
{
	%owner = Owner;
	if (!(noLayerBlending) && Layer != "custom")
	{
		%owner.setLayer($LAYER[Layer]);
		%blendFactor = $LAYER_BLENDING[Layer];
		%this.layerBlendColor = %blendFactor SPC %blendFactor SPC %blendFactor;
		%owner.setBlendColor(%blendFactor, %blendFactor, %blendFactor, "1");
	}
	return;
}
function BeMask::unlink(%this)
{
	if (useOnlyParallaxing)
	{
		return %this;
	}
	%owner = Owner;
	%owner.setRenderTexture("0");
	if (!($WII))
	{
		%owner.setUsePositive("0");
	}
	return;
}
function BeMask::startCloningBlendColor(%this, %objectToCloneFrom)
{
	%this.objectToCloneBlendColorFrom = %objectToCloneFrom;
	subscribeToEvents(%this, "onUpdateTick10");
	return;
}
function BeMask::onUpdateTick10(%this)
{
	%owner = Owner;
	%owner.setBlendColor(objectToCloneBlendColorFrom.getBlendColor());
	if (!(isBlending))
	{
		unSubscribeFromEvents(%this, "onUpdateTick10");
	}
	return;
}
function unlinkMaskObjects()
{
	%i = 0;
	while (%i < maskObjectGroup.getCount())
	{
		maskObjectGroup.getObject(%i).unlink();
		%i = %i + 1.0;
	}
	return maskObjectGroup.getCount();
}
