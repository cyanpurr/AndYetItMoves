// customTextureAngle.cs.dso
if (!(isObject(BeCustomTextureAngle)))
{
	%template = new BehaviorTemplate(Name : BeCustomTextureAngle);
	%template.friendlyName = "CustomTextureAngle";
	%template.behaviorType = "Visual";
	%template.description = "neighter use original texture angle, nor mask angle, but a custom or one from an other object";
	%template.addBehaviorField(useObject, "use this objects parameters or specify the fields below (overriden by pickFromAutoRotaionMask)", object, null, t2dSceneObject);
	%template.addBehaviorField(pickFromAutoRotaionMask, "picks a mask with useMaskRotationForTexture = true below it self(s linkpoint)", bool, "1");
	%template.addBehaviorField(useLinkPointForPicking, "use a linkpoints position for picking (number of linkpoint; 0 = no linkpoint)", int, "0");
}
function BeCustomTextureAngle::onBehaviorAdd(%this)
{
	%owner = Owner;
	%this.maskBehavior = %owner.addDependentBehavior("BeMask");
	maskBehavior.useMaskRotationForTexture = "1";
	subscribeToEvent(%this, "onLevelLoadFinished5");
	return;
}
function BeCustomTextureAngle::onLevelLoadFinished5(%this)
{
	%owner = Owner;
	if (!(isObject(useObject)) && pickFromAutoRotaionMask)
	{
		if (useLinkPointForPicking)
		{
			%atPosition = %owner.getLinkPoint(useLinkPointForPicking);
		}
		else
		{
			%atPosition = %owner.getPosition();
		}
		%possibleObjects = scenegraph.pickPoint(%atPosition, $MASK_ALL_LIST, %owner.getLayer(), "0", %owner);
		%i = 0;
		while (%i < getWordCount(%possibleObjects))
		{
			%obj = getWord(%possibleObjects, %i);
			%objMaskBehavior = %obj.getBehavior("BeMask");
			if (!(isObject(%objMaskBehavior)))
			{
			}
			else
			{
				if (!(useMaskRotationForTexture))
				{
					break;
				}
				%this.useObject = %obj;
				%i = getWordCount(%possibleObjects);
			}
			%i = %i + 1.0;
		}
	}
	if (isObject(useObject))
	{
		%objMaskBehavior = useObject.getBehavior("BeMask");
		if (!(isObject(%objMaskBehavior)))
		{
			debugWarn("no mask behavior at given object in BeCustomTextureAngle!!");
			return;
		}
		maskBehavior.setTexture(textureObject);
	}
	else
	{
		debugWarn("found no object for behavior BeCustomTextureAngle to clone parameters from!");
	}
	return;
}
