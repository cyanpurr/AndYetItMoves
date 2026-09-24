// moveDirection.cs.dso
if (!(isObject(BeMoveDirection)))
{
	%template = new BehaviorTemplate(Name : BeMoveDirection);
	%template.friendlyName = "MoveDirection";
	%template.behaviorType = "Bonus";
	%template.description = "moves in a specified direction, which is kept relative to the screen (p.e. allways up seen from user)";
	%template.addBehaviorField(baseVelocity, "velocity the object will move", Vector, "5 0", t2dVector);
	%template.addBehaviorField(autoPlay, "wheter or not the object should start rotating right away", bool, "0");
}
function BeMoveDirection::onBehaviorAdd(%this)
{
	subscribeToEvents(%this, "onLevelLoadFinished10 onLevelLoadFinished20 onLevelLoadFinished30");
	return;
}
function BeMoveDirection::onLevelLoadFinished10(%this)
{
	subscribeToEvents(%this, "onFirstKeyPressed onRotationStart onRotationFinish");
	%this.isMoving = "0";
	return;
}
function BeMoveDirection::onLevelLoadFinished20(%this)
{
	%this.allFadeOnSwitches = new SimSet(Name : "");
	levelGarbageCollector.add(allFadeOnSwitches);
	%this.collectFadeMasks(%owner);
	return;
}
function BeMoveDirection::onLevelLoadFinished30(%this)
{
	%owner = Owner;
	%owner.setMountedCollidesNotImmovable();
	if (!(isObject(%owner.getBehavior("BeCollide"))))
	{
		%owner.setImmovable("0");
	}
	return;
}
function BeMoveDirection::collectFadeMasks(%this, %mother)
{
	%owner = Owner;
	if (isObject(%mother.getBehavior("BeMask")))
	{
		%fadeOnSwitchBehavior = %mother.addDependentBehavior("BeFadeOnSwitch");
		%fadeOnSwitchBehavior.fadeTime = "0.5";
		allFadeOnSwitches.add(%fadeOnSwitchBehavior);
	}
	%allChilds = %mother.getMountedChildren();
	%i = 0;
	while (%i < getWordCount(%allChilds))
	{
		%child = getWord(%allChilds, %i);
		%this.collectFadeMasks(%child);
		%i = %i + 1.0;
	}
	return getWordCount(%allChilds);
}
function BeMoveDirection::onFirstKeyPressed(%this)
{
	if (autoPlay)
	{
		%this.setMoving("1");
	}
	else
	{
		%this.switchOff();
	}
	return;
}
function BeMoveDirection::setMoving(%this, %state)
{
	%owner = Owner;
	if (%state)
	{
		%owner.setLinearVelocity(%this.getMoveVelocity());
		subscribeToEvents(%this, "onPlayerDeath onPlayerReanimate");
	}
	else
	{
		%owner.setLinearVelocity("0 0");
		unSubscribeFromEvents(%this, "onPlayerDeath onPlayerReanimate");
	}
	%this.isMoving = %state;
	return;
}
function BeMoveDirection::getMoveVelocity(%this)
{
	return rotateVector(baseVelocity, camera.getCurrentRotation());
	return rotateVector(baseVelocity, camera.getCurrentRotation());
}
function BeMoveDirection::onPlayerDeath(%this)
{
	%owner = Owner;
	%owner.setLinearVelocity("0 0");
	return;
}
function BeMoveDirection::onPlayerReanimate(%this)
{
	%owner = Owner;
	%owner.setPosition(player.getPosition());
	return;
}
function BeMoveDirection::onRotationStart(%this)
{
	%owner = Owner;
	%this.oldWorldRotation = camera.getCurrentRotation();
	if (isMoving)
	{
		%owner.setLinearVelocity("0 0");
	}
	return;
}
function BeMoveDirection::onRotationFinish(%this)
{
	%owner = Owner;
	if (isMoving)
	{
		%owner.setLinearVelocity(%this.getMoveVelocity());
	}
	return;
}
function BeMoveDirection::switchOff(%this)
{
	%owner = Owner;
	%this.setMoving("0");
	%this.activateChilds(%owner, "0");
	%i = 0;
	while (%i < allFadeOnSwitches.getCount())
	{
		allFadeOnSwitches.getObject(%i).switchOff();
		%i = %i + 1.0;
	}
	return allFadeOnSwitches.getCount();
}
function BeMoveDirection::switchOn(%this)
{
	%owner = Owner;
	%owner.setPosition(player.getPosition());
	%this.setMoving("1");
	%this.activateChilds(%owner, "1");
	%i = 0;
	while (%i < allFadeOnSwitches.getCount())
	{
		allFadeOnSwitches.getObject(%i).switchOn();
		%i = %i + 1.0;
	}
	return allFadeOnSwitches.getCount();
}
function BeMoveDirection::activateChilds(%this, %mother, %state)
{
	%mother.setCollisionSuppress(!(%state));
	%allChilds = %mother.getMountedChildren();
	%i = 0;
	while (%i < getWordCount(%allChilds))
	{
		%child = getWord(%allChilds, %i);
		%this.activateChilds(%child, %state);
		%i = %i + 1.0;
	}
	return getWordCount(%allChilds);
}
