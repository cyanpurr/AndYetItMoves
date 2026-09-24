// delayedBreaking.cs.dso
if (!(isObject(BeDelayedBreaking)))
{
	%template = new BehaviorTemplate(Name : BeDelayedBreaking);
	%template.friendlyName = "DelayedBreaking";
	%template.behaviorType = "CommonObjects";
	%template.description = "breakes off after a specified time";
	%template.addBehaviorField(breakDelay, "the time (s) the object needs to break off", float, "1");
	%template.addBehaviorField(breakingAudioProfile, "the sound this object should play during breaking", string, "");
}
function BeDelayedBreaking::onBehaviorAdd(%this)
{
	%owner = Owner;
	if (!(breakingBehavior))
	{
		%this.breakingBehavior = %owner.addDependentBehavior("BeBreaking");
		breakingBehavior.breakoutAudioProfile = breakoutAudioProfile;
		breakingBehavior.collisionAudioProfile = collisionAudioProfile;
	}
	subscribeToEvent(%this, "onLevelLoadFinished10");
	return;
}
function BeDelayedBreaking::onBehaviorRemove(%this)
{
	return;
}
function BeDelayedBreaking::onLevelLoadFinished10(%this)
{
	%this.init();
	return;
}
function BeDelayedBreaking::init(%this)
{
	if (!(breakingBehavior))
	{
		%this.breakingBehavior = Owner.getBehavior("BeBreaking");
	}
	Owner.removeBehavior(breakingBehavior, "0");
	return;
}
function BeDelayedBreaking::quarry(%this)
{
	%owner = Owner;
	if (quarriedOut)
	{
		return %owner;
	}
	%owner.quarriedOut = "1";
	if (breakDelay > 0.0)
	{
		if (isObject(breakingAudioProfile))
		{
			playDistanceEventSound(%owner, breakingAudioProfile, "0.7");
		}
		safeSchedule(1000.0 * breakDelay, %this, "breakDelayExpired");
		%i = 0;
		while (%i < getWordCount(cloakObjects))
		{
			if (isObject(getWord(cloakObjects, %i)))
			{
				getWord(cloakObjects, %i).uncloak(breakDelay);
			}
			%i = %i + 1.0;
		}
	}
	else
	{
		%this.breakDelayExpired();
	}
	return;
}
function BeDelayedBreaking::breakDelayExpired(%this)
{
	%owner = Owner;
	%owner.addBehavior(breakingBehavior);
	breakingBehavior.quarry();
	%owner.removeBehavior(%this);
	return;
}
function BeDelayedBreaking::setCloakObjects(%this, %cloakObjects)
{
	%this.cloakObjects = %cloakObjects;
	return;
}
function BeDelayedBreaking::setBreakDelay(%this, %breakDelay)
{
	%this.breakDelay = %breakDelay;
	return;
}
