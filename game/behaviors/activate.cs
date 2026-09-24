// activate.cs.dso
if (!(isObject(BeActivate)))
{
	%template = new BehaviorTemplate(Name : BeActivate);
	%template.friendlyName = "activate";
	%template.behaviorType = "GameplayMechanisms";
	%template.description = "the circle (size determined by factor) around the owner will be a trigger reacting to activator - onEnter(activator) call %owner.switchOn(); onLeave(activator) call %owner.switchOff()";
	%template.addBehaviorField(activator, "which Group should be able to activate the owner", enum, "playerTrigger", $GROUPS_ENUM);
	%template.addBehaviorField(onlyActivateOnce, "calls switchOn only on first enter", bool, "0");
	%template.addBehaviorField(onlyActivateOnLanded, "only activates if the player lands safely after touching the trigger", bool, "0");
	%template.addBehaviorField(suppressOwnerCollision, "wheter or not to surpress all collisions of the owner", bool, "1");
	%template.addBehaviorField(factor, "determines size of trigger around owner: 1 = same size as owner, -1 = same size as owner, but means that whole owner has to be inside viewwindow (only use if really neccessary -> expansive); must be <> 0", float, "1");
	%template.addBehaviorField(useBoundingCircle, "use the surrounding circle of the owner", bool, "1");
	%template.addBehaviorField(useInnerCircle, "only if useCircleCollision: use inner or outer circle", bool, "0");
	%template.addBehaviorField(BehaviorList, "a list of behaviors on which the switchOn/Off function get called - leave empty to call all behaviors", string, "");
	%template.addBehaviorField(delaySwitchOff, "special method of handling onEnter/onLeave for Switches", bool, "0");
}
function BeActivate::onBehaviorAdd(%this)
{
	subscribeToEvent(%this, "onLevelLoadFinished10");
	return;
}
function BeActivate::onBehaviorRemove(%this)
{
	if (isObject(trigger))
	{
		trigger.dismount();
		trigger.safeDelete();
	}
	return;
}
function BeActivate::onLevelLoadFinished10(%this)
{
	%this.init();
	return;
}
function BeActivate::init(%this)
{
	%owner = Owner;
	%owner.inPlayerView = "0";
	%this.trigger = new t2dTrigger(Name : "")
	{
		class = "ActivateTrigger";
		scenegraph = daSceneGraph;
		_behavior0 = "BeTrigger";
		Owner = %owner;
	}
	trigger.onLeaveBufferList = "";
	trigger.onLeaveBufferSet = new SimSet(Name : "");
	levelGarbageCollector.add(onLeaveBufferSet);
	trigger.onEnterBufferList = "";
	trigger.onEnterBufferSet = new SimSet(Name : "");
	levelGarbageCollector.add(onEnterBufferSet);
	trigger.setGraphGroup($GROUPS[activator]);
	trigger.setSize(t2dVectorScale(%owner.getSize(), mAbs(factor)));
	trigger.setCollisionDetection("POLYGON");
	if (useBoundingCircle)
	{
		trigger.setCollisionDetection("CIRCLE");
		if (useInnerCircle)
		{
			trigger.setCollisionCircleSuperscribed("0");
		}
	}
	trigger.mount(Owner, "0 0", "0", "1", "1", "1", "0");
	if (factor < 0.0)
	{
		trigger.setEnterCallback("0");
		trigger.setStayCallback("1");
		trigger.setLeaveCallback("0");
		trigger.setCollisionDetection("CIRCLE");
	}
	Owner.setCollisionSuppress(suppressOwnerCollision);
	return;
}
function BeActivate::setFactor(%this, %factor)
{
	%this.factor = %factor;
	trigger.setSize(t2dVectorScale(%owner.getSize(), factor));
	return;
}
function ActivateTrigger::onEnter(%this, %obj)
{
	%owner = Owner;
	%delaySwitchOff = delaySwitchOff;
	if (onlyActivateOnLanded && isFalling)
	{
		subscribeToEvents(%this, "onPlayerLanded onPlayerDeath");
		return;
	}
	if (%delaySwitchOff)
	{
		if (delaySwitchOffSchedule != 0.0)
		{
			cancel(delaySwitchOffSchedule);
			%this.delaySwitchOffSchedule = "0";
		}
		else
		{
			%this.activate();
		}
	}
	else
	{
		if (camera.getIsRotating())
		{
			%this.onEnterBufferList = onEnterBufferList SPC %obj;
			onEnterBufferSet.add(%obj);
			subscribeToEvent(%this, "onRotationFinish");
			return;
			break;
		}
		%this.activate();
	}
	return;
}
function ActivateTrigger::onLeave(%this, %obj)
{
	%owner = Owner;
	%delaySwitchOff = delaySwitchOff;
	if (%delaySwitchOff)
	{
		%this.delaySwitchOffSchedule = %this.schedule("2000", "deactivate");
		return;
	}
	else
	{
		if (camera.getIsRotating())
		{
			%this.onLeaveBufferList = onLeaveBufferList SPC %obj;
			onLeaveBufferSet.add(%obj);
			subscribeToEvent(%this, "onRotationFinish");
			return;
		}
	}
	%this.deactivate();
	return;
}
function ActivateTrigger::onRotationFinish(%this, %obj)
{
	%this.onEnterBufferList = trim(onEnterBufferList);
	%this.onLeaveBufferList = trim(onLeaveBufferList);
	%i = 0;
	while (%i < onLeaveBufferSet.getCount())
	{
		%leaveCounter[onLeaveBufferSet.getObject(%i)] = 0;
		%i = %i + 1.0;
	}
	%i = 0;
	while (%i < getWordCount(onLeaveBufferList))
	{
		%leaveCounter[getWord(onLeaveBufferList, %i)] = %leaveCounter[getWord(onLeaveBufferList, %i)] + 1.0;
		%i = %i + 1.0;
	}
	%i = 0;
	while (%i < onEnterBufferSet.getCount())
	{
		%enterCounter[onEnterBufferSet.getObject(%i)] = 0;
		%i = %i + 1.0;
	}
	%i = 0;
	while (%i < getWordCount(onEnterBufferList))
	{
		%enterCounter[getWord(onEnterBufferList, %i)] = %enterCounter[getWord(onEnterBufferList, %i)] + 1.0;
		%i = %i + 1.0;
	}
	%i = 0;
	while (%i < onEnterBufferSet.getCount())
	{
		%obj = onEnterBufferSet.getObject(%i);
		if (%enterCounter[%obj] - %leaveCounter[%obj] > 0.0)
		{
			%this.onEnter(%obj);
		}
		%i = %i + 1.0;
	}
	%i = 0;
	while (%i < onLeaveBufferSet.getCount())
	{
		%obj = onLeaveBufferSet.getObject(%i);
		if (%leaveCounter[%obj] - %enterCounter[%obj] > 0.0)
		{
			%this.onLeave(%obj);
		}
		%i = %i + 1.0;
	}
	%this.onLeaveBufferList = "";
	onLeaveBufferSet.clear();
	%this.onEnterBufferList = "";
	onEnterBufferSet.clear();
	unSubscribeFromEvent(%this, "onRotationFinish");
	return;
}
function ActivateTrigger::onPlayerLanded(%this)
{
	debugEcho("player has landed: activating" SPC %this);
	%this.activate();
	unSubscribeFromEvents(%this, "onPlayerLanded onPlayerDeath");
	return;
}
function ActivateTrigger::onPlayerDeath(%this)
{
	unSubscribeFromEvents(%this, "onPlayerLanded onPlayerDeath");
	return;
}
function ActivateTrigger::activate(%this)
{
	%owner = Owner;
	if (%owner.isMethod("ownerSwitchOn"))
	{
		%owner.ownerSwitchOn();
	}
	%owner.callOnBehaviors("switchOn", "", BehaviorList);
	%owner.inPlayerView = "1";
	%activateBehavior = %owner.getBehavior("BeActivate");
	if (onlyActivateOnce)
	{
		debugEcho("removing activator" SPC %activateBehavior SPC "from" SPC %owner);
		%owner.removeBehavior(%activateBehavior);
	}
	return;
}
function ActivateTrigger::deactivate(%this)
{
	%owner = Owner;
	%delaySwitchOff = delaySwitchOff;
	if (%delaySwitchOff)
	{
		%this.delaySwitchOffSchedule = "0";
	}
	if (%owner.isMethod("ownerSwitchOff"))
	{
		%owner.ownerSwitchOff();
	}
	%owner.callOnBehaviors("switchOff", "", BehaviorList);
	%owner.inPlayerView = "0";
	return;
}
function ActivateTrigger::onStay(%this, %obj)
{
	%owner = Owner;
	%dist = t2dVectorDistance(%owner.getPosition(), viewWindow.getPosition());
	%inside = viewWindow.getCollisionRadius() + %owner.getCollisionRadius() * factor < %dist;
	if (inPlayerView && !(%inside))
	{
		%this.onEnter(%obj);
	}
	else
	{
		if (!(inPlayerView) && %inside)
		{
			%this.onLeave(%obj);
		}
	}
	return;
}
