// batBlocker.cs.dso
if (!(isObject(BeBatBlocker)))
{
	%template = new BehaviorTemplate(Name : BeBatBlocker);
	%template.friendlyName = "BatBlocker";
	%template.behaviorType = "LevelCave4";
	%template.description = "an obejcts that collides only with bats";
}
function BeBatBlocker::onBehaviorAdd(%this)
{
	%owner = Owner;
	%owner.setGraphGroup($GROUPS["batBlocker"]);
	%owner.setCollisionGroups("");
	%owner.setLayer($LAYER["collide"]);
	%owner.setCollisionActive("0", "1");
	%owner.setCollisionPhysics("0", "0");
	return;
}
