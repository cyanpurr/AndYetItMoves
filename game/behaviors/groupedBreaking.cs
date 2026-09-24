// groupedBreaking.cs.dso
if (!(isObject(BeGroupedBreaking)))
{
	%template = new BehaviorTemplate(Name : BeGroupedBreaking);
	%template.friendlyName = "GroupedBreaking";
	%template.behaviorType = "CommonObjects";
	%template.description = "lets this object break out as part of a group";
	%template.addBehaviorField(groupNumber, "number of the wallgroup - 0 makes a single part without group", integer, "1");
	%template.addBehaviorField(isSolid, "if this object can be quarried out or stays at its place", bool, "0");
}
function BeGroupedBreaking::onBehaviorAdd(%this)
{
	%owner = Owner;
	%owner.addDependentBehavior("BeBreaking");
	subscribeToEvents(%this, "onLevelLoadFinished10 onLevelShutdown");
	return;
}
function BeGroupedBreaking::onLevelLoadFinished10(%this)
{
	%owner = Owner;
	%this.setGroupNumber(groupNumber);
	return;
}
function BeGroupedBreaking::onLevelShutdown(%this, )
{
	if (groupNumber)
	{
		%breakingGroup = "breakingGroup_" @ groupNumber;
		if (isObject(%breakingGroup))
		{
			%breakingGroup.remove(%this);
			if (!(%breakingGroup.getCount()))
			{
				%breakingGroup.delete();
			}
		}
	}
	return;
}
function BeGroupedBreaking::quarry(%this, %suppressCollision)
{
	%owner = Owner;
	if (quarriedOut)
	{
		return %owner;
	}
	if (!(isObject(%owner.getBehavior(BeDelayedBreaking))))
	{
		%owner.quarriedOut = "1";
	}
	if (groupNumber)
	{
		%breakingGroup = "breakingGroup_" @ groupNumber;
		%breakingGroup.remove(%this);
		if (%breakingGroup.getCount())
		{
			%obj = %breakingGroup.getObject("0");
			%obj.quarry(%suppressCollision);
			%reactiveBreaking = Owner.getBehavior(BeReactiveBreaking);
			if (isObject(%reactiveBreaking))
			{
				%reactiveBreaking.setBehaviorCollisionReceiveCallback("0");
				Owner.removeBehavior(%reactiveBreaking);
			}
		}
	}
	if (!(isSolid))
	{
		if (%suppressCollision)
		{
			%owner.setCollisionSuppress("1");
		}
		%this.behaviorQuarry();
	}
	return;
}
function BeGroupedBreaking::behaviorQuarry(%this)
{
	%owner = Owner;
	%be = %owner.getBehavior("BeDelayedBreaking");
	if (!(%be))
	{
		%be = %owner.getBehavior("BeBreaking");
	}
	%be.quarry();
	return;
}
function BeGroupedBreaking::setGroupNumber(%this, %number)
{
	%this.groupNumber = %number;
	if (groupNumber)
	{
		%breakingGroup = "breakingGroup_" @ groupNumber;
		if (!(isObject(%breakingGroup)))
		{
			new SimSet(Name : %breakingGroup);
			levelGarbageCollector.add(%breakingGroup);
		}
		%breakingGroup.add(%this);
	}
	return;
}
