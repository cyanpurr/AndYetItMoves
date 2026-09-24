// dekoTilemap.cs.dso
if (!(isObject(BeDekoTilemap)))
{
	%template = new BehaviorTemplate(Name : BeDekoTilemap);
	%template.friendlyName = "DekoTilemap";
	%template.behaviorType = "Visual";
	%template.description = "a tile map for decoration";
	%template.addBehaviorField(dekoLayer, "the layer (like in mask)", enum, "background_paper", $PARALAXLAYER_ENUM);
	%template.addBehaviorField(initParalaxPosition, "if the layer size shall be initialazed for paralaxing", bool, "1");
	%template.addBehaviorField(useCustomBlending, "na wos?", float, -1.0);
}
function BeDekoTilemap::onBehaviorAdd(%this)
{
	%owner = Owner;
	%owner.setLayer($LAYER[dekoLayer]);
	subscribeToEvents(%this, "onLevelLoadFinished2 onLevelLoadFinished");
	return;
}
function BeDekoTilemap::onLevelLoadFinished2(%this)
{
	%owner = Owner;
	%blendFactor = $LAYER_BLENDING[dekoLayer];
	%owner.setBlendColor(%blendFactor, %blendFactor, %blendFactor, "1");
	if (initParalaxPosition)
	{
		%owner.initLayerPosition(dekoLayer, "0", "1");
	}
	%mountWithOffsetBehavior = %owner.addDependentBehavior("BeMountWithOffset");
	%mountWithOffsetBehavior.mother = $LAYER_NODE[dekoLayer];
	return;
}
function BeDekoTilemap::onLevelLoadFinished(%this)
{
	%owner = Owner;
	if (useCustomBlending >= 0.0)
	{
		%owner.setBlendColor(useCustomBlending, useCustomBlending, useCustomBlending);
	}
	return;
}
