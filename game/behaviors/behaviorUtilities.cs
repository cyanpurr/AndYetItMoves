// behaviorUtilities.cs.dso
function BehaviorComponent::addDependentBehavior(%this, %behaviorName)
{
	if (!(findWord(%this.getRealBehaviorList(), %behaviorName)))
	{
		%instanz = %behaviorName.createInstance();
		%this.addBehavior(%instanz);
		return %instanz;
	}
	else
	{
		return %this.getBehavior(%behaviorName);
	}
	return %this.getBehavior(%behaviorName);
}
function BehaviorComponent::addDependentBehaviors(%this, %behaviorNames)
{
	%i = 0;
	while (%i < getWordCount(%behaviorNames))
	{
		%this.addDependentBehavior(getWord(%behaviorNames, %i));
		%i = %i + 1.0;
	}
	return getWordCount(%behaviorNames);
}
function BehaviorComponent::callOnBehaviors(%this, %method, %args, %behaviorList, %excludeBehaviorList)
{
	if (%behaviorList $= "")
	{
	}
	else
	{
	}
	%behaviorsToCall = %behaviorList;
	%i = 0;
	while (%i < getWordCount(%behaviorsToCall))
	{
		%behaviorName = getWord(%behaviorsToCall, %i);
		%behavior = %this.getBehavior(%behaviorName);
		if (!(%behavior))
		{
		}
		else
		{
			if (%excludeBehaviorList != "")
			{
				if (!(findWord(%excludeBehaviorList, %behaviorName)) && %behavior.isMethod(%method))
				{
					%behavior.call(%method, %args);
				}
				break;
			}
			if (%behavior.isMethod(%method))
			{
				%behavior.call(%method, %args);
			}
		}
		%i = %i + 1.0;
	}
	return getWordCount(%behaviorsToCall);
}
function copyBehaviorFields(%template, %fromInstance, %toInstance)
{
	%i = 0;
	while (%i < %template.getBehaviorFieldCount())
	{
		%FieldName = getWord(%template.getBehaviorField(%i), "0");
		eval(%toInstance @ "." @ %FieldName @ "=" @ %fromInstance @ "." @ %FieldName @ ";");
		%i = %i + 1.0;
	}
	return %template.getBehaviorFieldCount();
}
function t2dSceneObject::setOwnerCollisionCallback(%this, %on)
{
	if (%on)
	{
		%this.setCollisionCallback("1");
		%this.wantsCollisionCallback = "1";
	}
	else
	{
		%this.wantsCollisionCallback = "0";
		if (getWordCount(behaviorCollisionCallbackList) == 0.0)
		{
			%this.setCollisionCallback("0");
		}
	}
	return;
}
function BehaviorTemplate::setBehaviorCollisionCallback(%this, %on)
{
	if (%on)
	{
		%this.wantsCollisionCallback = "1";
		Owner.setCollisionCallback("1");
		if (!(isObject(behaviorCollisionCallbackList)) || isObject(behaviorCollisionCallbackList) && behaviorCollisionCallbackList.getClassName() != "SimSet")
		{
			%justCreated = 1;
			Owner.behaviorCollisionCallbackList = new SimSet(Name : "");
			levelGarbageCollector.add(behaviorCollisionCallbackList);
		}
		if (!(behaviorCollisionCallbackList.isMember(%this)))
		{
			behaviorCollisionCallbackList.add(%this);
		}
	}
	else
	{
		if (!(wantsCollisionCallback))
		{
			return %this;
		}
		%this.wantsCollisionCallback = "0";
		if (!(isObject(behaviorCollisionCallbackList)))
		{
			if (!(wantsCollisionCallback))
			{
				Owner.setCollisionCallback("0");
			}
			break;
		}
		behaviorCollisionCallbackList.remove(%this);
		if (!(wantsCollisionCallback) && behaviorCollisionCallbackList.getCount() == 0.0)
		{
			Owner.setCollisionCallback("0");
		}
	}
	return;
}
function t2dSceneObject::setOwnerCollisionReceiveCallback(%this, %on)
{
	if (%on)
	{
		%this.setCollisionReceiveCallback("1");
		%this.wantsCollisionReceiveCallback = "1";
	}
	else
	{
		%this.wantsCollisionReceiveCallback = "0";
		if (getWordCount(behaviorCollisionReceiveCallbackList) == 0.0)
		{
			%this.setCollisionReceiveCallback("0");
		}
	}
	return;
}
function BehaviorTemplate::setBehaviorCollisionReceiveCallback(%this, %on)
{
	if (%on)
	{
		%this.wantsCollisionReceiveCallback = "1";
		Owner.setCollisionReceiveCallback("1");
		if (!(isObject(behaviorCollisionReceiveCallbackList)))
		{
			Owner.behaviorCollisionReceiveCallbackList = new SimSet(Name : "");
			levelGarbageCollector.add(behaviorCollisionReceiveCallbackList);
		}
		if (!(behaviorCollisionReceiveCallbackList.isMember(%this)))
		{
			behaviorCollisionReceiveCallbackList.add(%this);
		}
	}
	else
	{
		if (!(wantsCollisionReceiveCallback))
		{
			return %this;
		}
		%this.wantsCollisionReceiveCallback = "0";
		if (!(isObject(behaviorCollisionReceiveCallbackList)))
		{
			if (!(wantsCollisionReceiveCallback))
			{
				Owner.setCollisionReceiveCallback("0");
			}
			break;
		}
		behaviorCollisionReceiveCallbackList.remove(%this);
		if (!(wantsCollisionReceiveCallback) && behaviorCollisionReceiveCallbackList.getCount() == 0.0)
		{
			Owner.setCollisionReceiveCallback("0");
		}
	}
	return;
}
function BehaviorComponent::applyBehavior(%this, %behaviorName)
{
	if (!(%this.getBehavior(%behaviorName)))
	{
		%instanz = %behaviorName.createInstance();
		%this.addBehavior(%instanz);
		return %instanz;
	}
	return "0";
	return "0";
}
