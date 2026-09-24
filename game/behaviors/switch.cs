// switch.cs.dso
$disableSwitches = 0;
if (!(isObject(BeSwitch)))
{
	if (!(isTorquePlayer()))
	{
		%template = new BehaviorTemplate(Name : BeSwitch);
	}
	else
	{
		%template = new BeSwitchTemplate(Name : BeSwitch);
	}
	%template.friendlyName = "Switch";
	%template.behaviorType = "MetaGameMechanisms";
	%template.description = "the object will call switchOn/switchOff on the specified objects";
	if (!(isTorquePlayer()))
	{
		%template.addBehaviorField(ons, "a space sep. list of objects or groups that will be ONLY switched on (onEnter)", string, "");
		%template.addBehaviorField(onContainer, "all objects covered by this sceneObject will be switched on (use collision detection mode CIRCLE or POLY to determine object detection)", object, null, t2dSceneObject);
		%template.addBehaviorField(offs, "a space sep. list of objects or groups that will be ONLY switched off (onEnter)", string, "");
		%template.addBehaviorField(offContainer, "all objects covered by this sceneObject will be switched on (use collision detection mode CIRCLE or POLY to determine object detection)", object, null, t2dSceneObject);
		%template.addBehaviorField(toggles, "a space sep. list of objects or groups that will be toggled (switchOn onEnter, switchOff onLeave)", string, "");
		%template.addBehaviorField(switchBehaviors, "only behaviors in this list will be switched", string, "");
		%template.addBehaviorField(excludeBehaviors, "prevent these behaviors from being switched", string, "");
		%template.addBehaviorField(toggleInnerObjects, "if objects lying under owner should alle be switched on/off on enter/leave", bool, "0");
		%template.addBehaviorField(nearestZoomFactor, "to resize the switch to use it with layerobjects correctly, we need to know the highest camera zoom factor around this switch", float, "1");
		%template.addBehaviorField(excludeObjects, "if using a container to determine objects, exclude this space sep. list of objects or groups from toggling", string, "");
		%template.addBehaviorField(excludeClasses, "if using a container to determine objects, exclude objects which have one of this space sep. list of classes", string, "");
		%template.addBehaviorField(excludeObjectsWitchBehaviors, "if toggleInnerObjects == true, exclude objects which have one of this space sep. list of behaviors", string, "");
		%template.addBehaviorField(switchOffAtStart, "switch on-and-toggle Objects off when level starts (switchOff objects wont be touched by this)", bool, "1");
		%template.addBehaviorField(callPlay, "call play() instaed of switchOn() on objects", bool, "0");
	}
}
function BeSwitch::onBehaviorAdd(%this)
{
	%owner = Owner;
	if ($disableSwitches)
	{
		return;
	}
	%abcBehavior = %owner.addDependentBehavior("BeActivate");
	%owner.setLayer("0");
	subscribeToEvents(%this, "onLevelLoadFinished0 onLevelLoadFinished");
	return;
}
function BeSwitch::onLevelLoadFinished0(%this)
{
	%abcBehavior = Owner.getBehavior("BeActivate");
	%abcBehavior.BehaviorList = "BeSwitch";
	%abcBehavior.delaySwitchOff = "1";
	return;
}
