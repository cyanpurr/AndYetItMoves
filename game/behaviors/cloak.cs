// cloak.cs.dso
if (!(isObject(BeCloak)))
{
	%template = new BehaviorTemplate(Name : BeCloak);
	%template.friendlyName = "Cloak";
	%template.behaviorType = "Visual";
	%template.description = "use this object to cloak and later uncloak other objects. it will decrease the side (height or width) that is larger";
	%template.addBehaviorField(timeToUncloak, "the time (s) the object needs to uncloak; delayed breaking overrides this", float, "1");
}
function BeCloak::onAddToScene(%this)
{
	%owner = Owner;
	%size = %owner.getSize();
	if (getWord(%size, "0") > getWord(%size, "1"))
	{
		%this.useWidth = "1";
	}
	%this.Smoother = Smoother::createInstance();
	subscribeToEvent(%this, "onLevelLoadFinished15");
	return;
}
function BeCloak::onBehaviorRemove(%this)
{
	Smoother.delete();
	return;
}
function BeCloak::onLevelLoadFinished15(%this)
{
	%owner = Owner;
	%scenegraph = scenegraph;
	if (useWidth)
	{
		%point1 = %owner.getWorldPoint("0", -1.0);
		%point2 = %owner.getWorldPoint("0", "1");
	}
	else
	{
		%point1 = %owner.getWorldPoint(-1.0, "0");
		%point2 = %owner.getWorldPoint("1", "0");
	}
	%objectsAround = %scenegraph.pickLine(%point1, %point2);
	%i = 0;
	while (%i < getWordCount(%objectsAround))
	{
		%object = getWord(%objectsAround, %i);
		%delayBreakBehavior = %object.getBehavior("BeDelayedBreaking");
		if (!(%delayBreakBehavior))
		{
			%delayBreakBehavior = %object.getBehavior("BeBreakingPlatform");
		}
		if (!(%delayBreakBehavior))
		{
		}
		else
		{
			%delayBreakBehavior.cloakObjects = %owner SPC cloakObjects;
		}
		%i = %i + 1.0;
	}
	return getWordCount(%objectsAround);
}
function BeCloak::uncloak(%this, %timeToUncloak)
{
	%owner = Owner;
	if (%timeToUncloak $= "")
	{
	}
	else
	{
	}
	%timeToUncloak = %timeToUncloak;
	if (useWidth)
	{
	}
	else
	{
	}
	%side = %owner.getHeight();
	Smoother.init(%timeToUncloak, %side, "0");
	Smoother.start();
	subscribeToEvents(%this, "onUpdateTick10 onRotationStart onRotationFinish");
	return;
}
function BeCloak::onUpdateTick10(%this)
{
	%owner = Owner;
	if (Smoother.getIsFinished())
	{
		unSubscribeFromEvents(%this, "onUpdateTick10 onRotationStart onRotationFinish");
		if (useWidth)
		{
			%owner.setWidth("0");
		}
		else
		{
			%owner.setHeight("0");
		}
		%owner.safeDelete();
		return;
	}
	if (useWidth)
	{
		%owner.setPivotSize(Smoother.getValue() SPC %owner.getHeight(), "-1 0");
	}
	else
	{
		%owner.setPivotSize(%owner.getWidth() SPC Smoother.getValue(), "0 -1");
	}
	return;
}
function BeCloak::onRotationStart(%this)
{
	Smoother.pause();
	unSubscribeFromEvent(%this, "onUpdateTick10");
	return;
}
function BeCloak::onRotationFinish(%this)
{
	Smoother.start();
	subscribeToEvents(%this, "onUpdateTick10");
	return;
}
