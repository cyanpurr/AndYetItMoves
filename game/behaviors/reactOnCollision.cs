// reactOnCollision.cs.dso
if (!(isObject(BeReactOnCollision)))
{
	%template = new BehaviorTemplate(Name : BeReactOnCollision);
	%template.friendlyName = "ReactOnCollision";
	%template.behaviorType = "GameplayMechanisms";
	%template.description = "this object will get the callback reactOnCollision if it collides with a specified object, or with an object of specified weight and with specified speed";
	%template.addBehaviorField(reactToGroups, "to which groups (a space seperated list) it should react, if its none only weight und speed will determine callback", string, "");
	%template.addBehaviorField(maxCollisions, "after how many collisions it should end to react", integer, -1.0);
	%template.addBehaviorField(minCollisionGap, "how much time should pass minimum between two coll so they are valid (in s)", float, "0");
	%template.addBehaviorField(continuousCollisionTime, "for how mocuh time the collision should last before calling reactOnCollision", float, "0");
	%template.addBehaviorField(criticalWeight, "the weight an object needs to break through", float, "0");
	%template.addBehaviorField(criticalSpeed, "the speed the object needs to break through", float, "0");
	%template.addBehaviorField(useOwnerSpeed, "for critical speed use only the speed of the owner, rather then speed of the impact vector", bool, "0");
	%template.addBehaviorField(BehaviorList, "a list of behaviors on which the reatOnCollision function get called - only cause of RC1BUG", string, "");
}
function BeReactOnCollision::onBehaviorAdd(%this)
{
	%owner = Owner;
	%owner.setCollisionActiveSend("1");
	%this.collisionCounter = "1";
	subscribeToEvent(%this, "onLevelLoadFinished20");
	return;
}
function BeReactOnCollision::onLevelLoadFinished20(%this)
{
	Owner.initGroups(reactToGroups);
	unSubscribeFromEvent(%this, "onLevelLoadFinished20");
	return;
}
function BeReactOnCollision::initGroups(%this, %reactToGroups)
{
	%owner = Owner;
	%this.reactToGroups = %reactToGroups;
	if (reactToGroups != "")
	{
		%owner.addCollisionGroups(reactToGroups);
	}
	else
	{
		debugWarn("no groups are set in reactOnCollision of:" SPC %owner);
	}
	return;
}
function BeReactOnCollision::switchOn(%this)
{
	%this.setBehaviorCollisionCallback("1");
	return;
}
function BeReactOnCollision::switchOff(%this)
{
	%this.setBehaviorCollisionCallback("0");
	return;
}
function BeReactOnCollision::onCollision(%this, %dstObject, %srcRef, %dstRef, %time, %normal, %contacts, %points)
{
	if (!(wantsCollisionCallback))
	{
		return %this;
	}
	%owner = Owner;
	if (!(%this.checkGroups(%dstObject)))
	{
		return %this.checkGroups(%dstObject);
	}
	if (maxCollisions > -1.0 && collisionCounter > maxCollisions)
	{
		return %this;
	}
	if (thisTime - lastCollisionTime < minCollisionGap)
	{
		return $tickStats;
	}
	if (continuousCollisionTime > 0.0)
	{
		if (isObject(continuousCollisionSchedule))
		{
			continuousCollisionSchedule.cancelSchedule();
		}
		%this.continuousCollisionSchedule = %owner.safeSchedule(10.0 * getTickMs(), "resetStartTime");
		if (startTime == 0.0)
		{
			%this.startTime = thisTime;
			return;
			break;
		}
		if (thisTime - startTime < continuousCollisionTime)
		{
			return $tickStats;
		}
	}
	if (criticalSpeed > 0.0 || criticalWeight > 0.0)
	{
		while (%dstObject.getIsMounted() && %dstObject.getMountForce() == 0.0)
		{
			%dstObject = %dstObject.getMountedParent();
		}
		%weight = %dstObject.getMass();
		if (criticalSpeed > 0.0)
		{
			if (useOwnerSpeed)
			{
				%speed = t2dVectorLength(%owner.getLinearVelocity());
				break;
			}
			if (%dstObject.getImmovable())
			{
			}
			else
			{
			}
			%impactVelVec = %dstObject.getLinearVelocity();
			%speed = getImpactSpeed(%impactVelVec, %normal);
		}
		if (%speed >= criticalSpeed && %weight >= criticalWeight)
		{
			%this.callReactOnCollision(%dstObject, %srcRef, %dstRef, %time, %normal, %contacts, %points);
		}
	}
	else
	{
		%this.callReactOnCollision(%dstObject, %srcRef, %dstRef, %time, %normal, %contacts, %points);
	}
	return;
}
function BeReactOnCollision::checkGroups(%this, %object)
{
	if (reactToGroups $= "")
	{
		return "1";
	}
	else
	{
		%i = 0;
		while (%i < getWordCount(reactToGroups))
		{
			if ($GROUPS[getWord(reactToGroups, %i)] == %object.getGraphGroup())
			{
				return "1";
			}
			%i = %i + 1.0;
		}
		return "0";
	}
	return "0";
}
function BeReactOnCollision::callReactOnCollision(%this, %dstObject, %srcRef, %dstRef, %time, %normal, %contacts, %points)
{
	%i = 0;
	while (%i < getWordCount(BehaviorList))
	{
		%instance = Owner.getBehavior(getWord(BehaviorList, %i));
		if (!(%instance))
		{
			%instance = getWord(BehaviorList, %i);
		}
		%instance.ReactOnCollision(%dstObject, %srcRef, %dstRef, %time, %normal, %contacts, %points, collisionCounter);
		%i = %i + 1.0;
	}
	%this.collisionCounter = collisionCounter + 1.0;
	%this.lastCollisionTime = thisTime;
	%this.resetStartTime();
	return;
}
function BeReactOnCollision::resetStartTime(%this)
{
	%this.startTime = "0";
	return;
}
