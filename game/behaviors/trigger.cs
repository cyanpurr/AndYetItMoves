// trigger.cs.dso
if (!(isObject(BeTrigger)))
{
	%template = new BehaviorTemplate(Name : BeTrigger);
	%template.friendlyName = "Trigger";
	%template.behaviorType = "MetaGameMechanisms";
	%template.description = "!only use this with Trigger! sets up colission";
	%template.addBehaviorField(collisionGroup, "collision group of this trigger", enum, "playerTrigger", $GROUPS_ENUM);
}
function BeTrigger::onBehaviorAdd(%this)
{
	%owner = Owner;
	if (%owner.getClassName() != "t2dTrigger")
	{
		debugWarn("owner of trigger behavior is not a trigger!");
		%owner.removeBehavior(%this);
		return;
	}
	%this.setTriggerCollisionGroup(collisionGroup);
	%owner.setCollisionActive("0", "1");
	%owner.setCollisionPhysics("0", "0");
	%owner.setCollisionDetection("POLYGON");
	%owner.setEnterCallback("1");
	%owner.setStayCallback("0");
	%owner.setLeaveCallback("1");
	return;
}
function BeTrigger::setTriggerCollisionDetection(%this, %mode)
{
	%owner = Owner;
	if (stricmp("full", %mode) || stricmp("circle", %mode) || stricmp("polygon", %mode) || !(stricmp("custom", %mode)))
	{
		debugWarn(['"tried to set wrone mode ("', '%mode', '") in Betrigger::setTriggerCollisionDetection() for "'] SPC %owner SPC "setting to default: FULL");
		%mode = "FULL";
	}
	%owner.setCollisionDetection(%mode);
	return;
}
function BeTrigger::setTriggerCollisionGroup(%this, %group)
{
	%owner = Owner;
	%owner.setGraphGroup($GROUPS[%group]);
	return;
}
