// texture.cs.dso
if (!(isObject(BeTexture)))
{
	%template = new BehaviorTemplate(Name : BeTexture);
	%template.friendlyName = "Texture";
	%template.behaviorType = "Visual";
	%template.description = "use for textures that are used for masking";
	%template.addBehaviorField(trackMaskFlip, "should the texture flip like its mask", bool, "1");
	%template.addBehaviorField(Layer, "set the layer for parallax scrolling. the mask has the power to overwrite it if mask.parallaxScrollTexture is true", enum, "last", $LAYER_ENUM);
	%template.addBehaviorField(texRotationOffset, "the offset to the texes 0Â°-rotation (0Â° points to right)", int, "0");
}
function BeTexture::onBehaviorAdd(%this)
{
	%owner = Owner;
	%owner.addDependentBehavior("BeDontCollide");
	subscribeToEvents(%this, "onLevelLoadFinished3 onLevelLoadFinished");
	return;
}
function BeTexture::onAddToScene(%this, )
{
	%owner = Owner;
	%owner.trackMaskFlip = trackMaskFlip;
	return;
}
function BeTexture::onLevelLoadFinished3(%this)
{
	%owner = Owner;
	%owner.setLayer($LAYER[Layer]);
	%owner.setVisible("0");
	if (findWord($PARALAXLAYER_ENUM, Layer))
	{
		%owner.initLayerPosition(Layer, "1", !(%owner.getIsMounted()));
		if (!(isObject(%owner.getBehavior("BeMountWithOffset"))) && !(%owner.getIsMounted()))
		{
			%mountWithOffsetBehavior = %owner.addDependentBehavior("BeMountWithOffset");
			%mountWithOffsetBehavior.mother = $LAYER_NODE[Layer];
		}
	}
	return;
}
function BeTexture::onLevelLoadFinished(%this)
{
	%owner = Owner;
	%owner.setVisible("0");
	%owner.setBlendAlpha("1");
	return;
}
function BeTexture::getRotationOffset(%this)
{
	return texRotationOffset;
	return texRotationOffset;
}
