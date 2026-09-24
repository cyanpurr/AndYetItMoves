// hintTrigger.cs.dso
$hint_navigate = "hint_NavigateImageMap";
$hint_hints_off = "hint_hints_offImageMap";
$hint_rotate = "hint_RotateImageMap";
$hint_compete = "hint_CompeteImageMap";
$hint_momentum_1 = "hint_Momentum_1ImageMap";
$hint_momentum_2 = "hint_Momentum_2ImageMap";
$hint_momentum_pic = "hint_Momentum_PicImageMap";
$hint_slope_death_1 = "hint_slope_death_1ImageMap";
$hint_slope_death_pic = "hint_slope_death_picImageMap";
$hint_fin = "hint_finImageMap";
$hint_sensitivity = "hint_sensitivityImageMap";
if (!(isObject(BeHintTrigger)))
{
	%template = new BehaviorTemplate(Name : BeHintTrigger);
	%template.friendlyName = "Hint Trigger";
	%template.behaviorType = "MetaGameMechanisms";
	%template.description = "Shows and hdies a hint depending on the position in this circle";
	%template.addBehaviorField(hint, "which hint text to display", enum, "navigate", "navigate	hints_off	rotate	rotate180	buttonRotate	sensitivity	compete	momentum_1	momentum_2	momentum_pic	slope_death_1	slope_death_pic	fin");
	%template.addBehaviorField(nearRadiusFactor, "between 0 and 1; 0 -> hint is fully displayedat centerpoint, 1 -> hint fully displayed at outer collidion circle", float, "0.5");
	%template.addBehaviorField(hintObject, "the object that will be faded in/out", object, null, t2dStaticSprite);
	%template.addBehaviorField(hintObjectLayer, "layer of hintObject. 1st, 2nd or 3rd(frontmost) foreroundlayer, mainlayer or 1st, 2nd or 3rd (all-the-way-back) backgroundlayer", enum, "main", $LAYER_ENUM);
	%template.addBehaviorField(wiiOnly, "only shown on wii", bool, "0");
	%template.addBehaviorField(fadeIn, "wheter or not we should fade in or out", bool, "0");
}
function BeHintTrigger::onBehaviorAdd(%this)
{
	%owner = Owner;
	if (%owner.getClassName() != "t2dTrigger")
	{
		debugWarn("owner of hint trigger behavior is not a trigger!");
		%owner.removeBehavior(%this);
		return;
	}
	%triggerBehavior = %owner.addDependentBehavior("BeTrigger");
	if (isObject(hintObject))
	{
		hintObject.setLayer($LAYER[Layer]);
	}
	hintsGroup.add(%this);
	subscribeToEvents(%this, "onLevelLoadFinished3 onLevelLoadFinished10 onLevelLoadFinished");
	return;
}
function BeHintTrigger::onLevelLoadFinished3(%this)
{
	%owner = Owner;
	if (wiiOnly && !($WII) || $WII && $settings::Wii::WiiInputMode $= "classic" && hint $= "buttonRotate")
	{
		hintObject.safeDelete();
		%owner.safeDelete();
		return;
	}
	if (%owner.getSizeX() > %owner.getSizeY())
	{
		debugWarn("hint trigger" SPC %owner SPC "not square! making it square with size:" SPC %owner.getSizeX());
		%owner.setSizeY(%owner.getSizeX());
	}
	else
	{
		if (%owner.getSizeY() > %owner.getSizeX())
		{
			debugWarn("hint trigger" SPC %owner SPC "not square! making it square with size:" SPC %owner.getSizeY());
			%owner.setSizeX(%owner.getSizeY());
		}
	}
	%owner.setCollisionCircleSuperscribed("0");
	%this.farRadius = Owner.getCollisionRadius();
	%this.nearRadius = nearRadiusFactor * farRadius;
	hintObject.addDependentBehaviors("BeKeepOrientation BeRotate");
	%keepOrientation = hintObject.getBehavior("BeKeepOrientation");
	%keepOrientation.origRotation = hintObject.getRotation();
	%keepOrientation.useFrameUpdate = "1";
	%keepOrientation.orientationMode = "FULL";
	hintObject.localFootPoint = hintObject.getLocalPoint(hintObject.getLinkPoint("1"));
	%rotateBehavior = hintObject.getBehavior("BeRotate");
	%rotateBehavior.autoPlay = "0";
	hintObject.textObject = new t2dStaticSprite(Name : "")
	{
		scenegraph = scenegraph;
		imageMap = "blackImageMap";
		frame = "0";
		canSaveDynamicFields = "1";
		size = hintObject.getSize();
		BaseVelocityAdaptionFactor = "-1";
	}
	%this.updateHintImage();
	subscribeToEvents(%this, "onUnpauseGame");
	textObject.mount(hintObject, "0 0", "0", "1", "1", "1", "1");
	%layer = hintObjectLayer;
	hintObject.setLayer($LAYER[%layer]);
	textObject.setLayer($LAYER[%layer]);
	if (findWord($PARALAXLAYER_ENUM, %layer))
	{
		hintObject.initLayerPosition(%layer, "0");
		%mountWithOffsetBehavior = hintObject.addDependentBehavior("BeMountWithOffset");
		%mountWithOffsetBehavior.mother = $LAYER_NODE[%layer];
		subscribeToEvents(%this, "onShowParalaxLayers onHideParalaxLayers");
	}
	if (fadeIn)
	{
		hintObject.setBlendAlpha("0");
		textObject.setBlendAlpha("0");
	}
	return;
}
function BeHintTrigger::onUnpauseGame(%this)
{
	%this.updateHintImage();
	return;
}
function BeHintTrigger::updateHintImage(%this)
{
	if ($WII && showingHintsForInput $= $settings::Wii::WiiInputMode)
	{
		return;
	}
	else
	{
		%this.showingHintsForInput = $settings::Wii::WiiInputMode;
	}
	if ($WII && hint $= "navigate" || hint $= "rotate" || hint $= "rotate180" || hint $= "buttonRotate")
	{
		if ($settings::Wii::WiiInputMode $= "keyhole" && hint != "rotate")
		{
			textObject.setImageMap("hint_grabber_" @ hint @ "ImageMap");
		}
		else
		{
			textObject.setImageMap("hint_" @ $settings::Wii::WiiInputMode @ "_" @ hint @ "ImageMap");
		}
	}
	else
	{
		if ($settings::Controls::SixenseEnabled && hint $= "navigate" || hint $= "rotate")
		{
			textObject.setImageMap("hint_keyhole_" @ hint @ "ImageMap");
			break;
		}
		eval("%this.hintObject.textObject.setImageMap($hint_" @ hint @ ");");
	}
	return;
}
function BeHintTrigger::onLevelLoadFinished10(%this)
{
	%owner = Owner;
	%triggerBehavior = %owner.getBehavior("BeTrigger");
	%owner.setGraphGroup($GROUPS["zoomTrigger"]);
	%owner.setCollisionCircleSuperscribed("0");
	%owner.setCollisionCircleScale("1");
	%owner.setStayCallback("1");
	%owner.setEnterCallback("0");
	if (!(showHints))
	{
		%this.switchOff();
	}
	return;
}
function BeHintTrigger::onLevelLoadFinished(%this)
{
	if (findWord($PARALAXLAYER_ENUM, hintObjectLayer) && !($settings::Performance::Layers))
	{
		%mountWithOffsetBehavior = hintObject.getBehavior("BeMountWithOffset");
		%mountWithOffsetBehavior.switchOff("1");
	}
	return;
}
function BeHintTrigger::onShowParalaxLayers(%this)
{
	%mountWithOffsetBehavior = hintObject.getBehavior("BeMountWithOffset");
	%mountWithOffsetBehavior.switchOn("1");
	return;
}
function BeHintTrigger::onHideParalaxLayers(%this)
{
	%mountWithOffsetBehavior = hintObject.getBehavior("BeMountWithOffset");
	%mountWithOffsetBehavior.switchOff("1");
	return;
}
function BeHintTrigger::onStay(%this, )
{
	if (!(fadeIn))
	{
		return %this;
	}
	%distance = t2dVectorDistance(player.getPosition(), Owner.getPosition());
	if (%distance < farRadius)
	{
		if (%distance < nearRadius)
		{
			%blendFactor = 1;
		}
		else
		{
			%blendFactor = %distance - farRadius / nearRadius - farRadius;
		}
	}
	else
	{
		%blendFactor = 0;
	}
	hintObject.setBlendAlpha(%blendFactor);
	textObject.setBlendAlpha(%blendFactor);
	return;
}
function BeHintTrigger::onLeave(%this, )
{
	if (!(fadeIn))
	{
		return %this;
	}
	if (!(isDead))
	{
		hintObject.setBlendAlpha("0");
		textObject.setBlendAlpha("0");
	}
	return;
}
function BeHintTrigger::toggle(%this)
{
	if (showHints)
	{
		%this.switchOn();
	}
	else
	{
		%this.switchOff();
	}
	return;
}
function BeHintTrigger::switchOff(%this)
{
	Owner.setCollisionSuppress("1");
	hintObject.setVisible("0");
	textObject.setVisible("0");
	return;
}
function BeHintTrigger::switchOn(%this)
{
	Owner.setCollisionSuppress("0");
	hintObject.setVisible("1");
	textObject.setVisible("1");
	return;
}
