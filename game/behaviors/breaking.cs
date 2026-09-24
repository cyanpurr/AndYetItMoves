// breaking.cs.dso
if (!(isObject(BeBreaking)))
{
	%template = new BehaviorTemplate(Name : BeBreaking);
	%template.friendlyName = "Breaking";
	%template.behaviorType = "CommonObjects";
	%template.description = "gives this object the functionality to break out";
	%template.addBehaviorField(breakoutAudioProfile, "the audioprofile that this object should recevie for breakout", string, "");
}
function BeBreaking::onBehaviorAdd(%this)
{
	%owner = Owner;
	%owner.setImmovable("1");
	if (!(moveBehavior))
	{
		%this.moveBehavior = %owner.addDependentBehavior("BeMoving");
	}
	subscribeToEvent(%this, "onLevelLoadFinished10");
	if (breakoutAudioProfile $= "")
	{
		%this.breakoutAudioProfile = breakoutAudioProfile;
	}
	return;
}
function BeBreaking::onLevelLoadFinished10(%this)
{
	%owner = Owner;
	if (!(moveBehavior))
	{
		%this.moveBehavior = %owner.getBehavior("BeMoving");
	}
	%owner.removeBehavior(moveBehavior, "0");
	%owner.addDependentBehavior("BeCollide");
	%owner.quarriedOut = "0";
	return;
}
function BeBreaking::quarry(%this)
{
	%owner = Owner;
	if (%owner.getBehavior("BeMoving"))
	{
		return %owner.getBehavior("BeMoving");
	}
	%owner.quarriedOut = "1";
	if (isObject(breakoutAudioProfile))
	{
		playDistanceEventSound(%owner, breakoutAudioProfile, "0.7");
	}
	else
	{
		debugEcho("tried to play breakout sound but it's not an object" SPC breakoutAudioProfile);
	}
	if (%owner.getBehavior("BeCollide"))
	{
		%owner.removeBehavior(%owner.getBehavior("BeCollide"));
	}
	%owner.addBehavior(moveBehavior);
	moveBehavior.init();
	%owner.setImmovable("0");
	%owner.dismount();
	%this.checkTexture();
	if (isObject(quarryOutCallbackObject))
	{
		quarryOutCallbackObject.onQuarryOut();
	}
	return;
}
function BeBreaking::checkTexture(%this)
{
	%owner = Owner;
	%kids = %owner.getMountedChildren();
	%i = 0;
	while (%i < getWordCount(%kids))
	{
		%obj = getWord(%kids, %i);
		%maskBehavior = %obj.getBehavior("BeMask");
		if (!(%maskBehavior) || isObject(protoObject))
		{
		}
		else
		{
			%mask = %obj;
			%mask.setMountOwned("1");
			%texParent = textureObject.getMountedParent();
			if (%texParent)
			{
				if (%texParent.getId() == %owner.getId() || %texParent.getId() == %mask.getId())
				{
					break;
				}
			}
			%maskBehavior.cloneTexture();
		}
		%i = %i + 1.0;
	}
	return getWordCount(%kids);
}
