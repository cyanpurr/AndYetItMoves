// smootherGroup.cs.dso
function SmootherGroup::createInstance(%smoother)
{
	if (!(isObject(%smoother)) || %smoother.getClassNamespace() != "Smoother")
	{
		debugWarn("SmootherGroup initialized with a non valid Smoother!");
		return "0";
	}
	%smootherGroup = new ScriptObject(Name : "")
	{
		class = "SmootherGroup";
	}
	%smootherGroup.Smoother = %smoother;
	%smootherGroup.objectsToSmooth = new SimSet(Name : "");
	levelGarbageCollector.add(%smootherGroup);
	levelGarbageCollector.add(objectsToSmooth);
	return %smootherGroup;
	return %smootherGroup;
}
function SmootherGroup::SMOOTH(%this)
{
	%smoothValue = Smoother.getValue();
	%i = 0;
	while (%i < getWordCount(activeObjects))
	{
		%object = getWord(activeObjects, %i);
		%object.smootherGroupCallback(%smoothValue);
		%i = %i + 1.0;
	}
	return getWordCount(activeObjects);
}
function SmootherGroup::doSmoothing(%this, %reverse, %inBetween, %alternateObjectList)
{
	if (%alternateObjectList $= "")
	{
		%this.activeObjects = convertToList(objectsToSmooth);
	}
	else
	{
		if (%alternateObjectList == 0.0)
		{
			%this.activeObjects = "";
			break;
		}
		%this.activeObjects = %alternateObjectList;
	}
	Smoother.start(%reverse, %inBetween);
	%this.intervallMode();
	return;
}
function SmootherGroup::sendFinishedCallback(%this, %direction)
{
	%i = 0;
	while (%i < getWordCount(activeObjects))
	{
		%object = getWord(activeObjects, %i);
		%object.smootherGroupFinished(%direction);
		%i = %i + 1.0;
	}
	if (isObject(callbackObject))
	{
		callbackObject.smootherGroupFinished(%direction);
	}
	return;
}
function SmootherGroup::setCallbackObject(%this, %object)
{
	%this.callbackObject = %object;
	return;
}
function SmootherGroup::intervallMode(%this)
{
	%this.SMOOTH();
	%finished = Smoother.getIsFinished();
	if (!(%finished))
	{
		%this.nextSmoothing = safeSchedule("300", %this, "intervallMode");
	}
	else
	{
		%this.sendFinishedCallback(%finished);
	}
	return;
}
function SmootherGroup::isMember(%this, %object)
{
	return objectsToSmooth.isMember(%object);
	return objectsToSmooth.isMember(%object);
}
function SmootherGroup::AddObject(%this, %object)
{
	objectsToSmooth.add(%object);
	return;
}
function SmootherGroup::removeObject(%this, %object)
{
	objectsToSmooth.remove(%object);
	return;
}
function SmootherGroup::getCount(%this)
{
	return objectsToSmooth.getCount();
	return objectsToSmooth.getCount();
}
function SmootherGroup::getObject(%this, %i)
{
	return objectsToSmooth.getObject(%i);
	return objectsToSmooth.getObject(%i);
}
function SmootherGroup::clear(%this)
{
	objectsToSmooth.clear();
	return;
}
