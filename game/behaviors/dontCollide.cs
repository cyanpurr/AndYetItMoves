// dontCollide.cs.dso
if (!(isObject(BeDontCollide)))
{
	%template = new BehaviorTemplate(Name : BeDontCollide);
	%template.friendlyName = "Dont Collide with anything";
	%template.behaviorType = "Physics";
	%template.description = "this object wont collide with anything";
}
function BeDontCollide::onBehaviorAdd(%this)
{
	%owner = Owner;
	%owner.setGraphGroup($GROUPS["dontCollide"]);
	%owner.setCollisionActive("0", "0");
	%owner.setCollisionPhysics("0", "0");
	return;
}
