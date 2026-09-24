// customizeTextureName.cs.dso
if (!(isObject(BeCustomizeTextureName)))
{
	%template = new BehaviorTemplate(Name : BeCustomizeTextureName);
	%template.friendlyName = "CustomizeTextureName";
	%template.behaviorType = "Visual";
	%template.description = "user friendly description of the behavior";
	%template.addBehaviorField(customTexture, "tell the mask to use a different texture object", string, "");
}
function BeCustomizeTextureName::onBehaviorAdd(%this)
{
	subscribeToEvent(%this, "onLevelLoadFinished15");
	return;
}
function BeCustomizeTextureName::onLevelLoadFinished15(%this)
{
	%owner = Owner;
	%maskBehavior = %owner.getBehavior("BeMask");
	if (!(isObject(customTexture)) || !(isObject(%maskBehavior)) || !(isObject(customTexture.getBehavior("BeTexture"))))
	{
		debugWarn("BeCustomizeTextureName with invalid Texture:" SPC customTexture);
	}
	else
	{
		%maskBehavior.setTexture(customTexture, "1");
	}
	return;
}
