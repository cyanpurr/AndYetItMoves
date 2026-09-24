// collide.cs.dso
if (!(isObject(BeCollide)))
{
	%template = new BehaviorTemplate(Name : BeCollide);
	%template.friendlyName = "Collide";
	%template.behaviorType = "Physics";
	%template.description = "makes the object collide with the player";
	%template.addBehaviorField(Immovable, "if the object's position should be fixed - not so for rotators, etc.", bool, "1");
	%template.addBehaviorField(GraphGroup, "defaults to collide, shouldnt be changed", string, "collide");
	%template.addBehaviorField(Layer, "1st, 2nd or 3rd(frontmost) foreroundlayer, mainlayer or 1st, 2nd or 3rd (all-the-way-back) backgroundlayer", enum, "collide", $LAYER_ENUM);
}
function BeCollide::onBehaviorAdd(%this)
{
	%owner = Owner;
	%owner.setGraphGroup($GROUPS[GraphGroup]);
	%owner.setCollisionGroups("");
	if (!(isObject(%owner.getBehavior("BeMask"))))
	{
		%owner.setLayer($LAYER[Layer]);
	}
	%owner.setCollisionActive("0", "1");
	%owner.setCollisionPhysics("0", "0");
	%owner.setCollisionDetection(POLYGON);
	%owner.setImmovable(Immovable);
	subscribeToEvents(%this, "onLevelLoadFinished3");
	return;
}
function BeCollide::onLevelLoadFinished3(%this)
{
	%owner = Owner;
	%owner.setLayer($LAYER[Layer]);
	if (findWord($PARALAXLAYER_ENUM, Layer))
	{
		%owner.initLayerPosition(Layer, "1", !(%owner.getIsMounted()));
		if (!(isObject(%owner.getBehavior("BeMountWithOffset"))) && !(%owner.getIsMounted()))
		{
			%mountWithOffsetBehavior = %owner.addDependentBehavior("BeMountWithOffset");
			%mountWithOffsetBehavior.mother = $LAYER_NODE[Layer];
		}
	}
	return;
}
function BeCollide::setCollideImmovable(%this, %immovable)
{
	%this.Immovable = %immovable;
	Owner.setImmovable(%immovable);
	return;
}
function BeCollide::switchOff(%this)
{
	Owner.setEnabled("0");
	return;
}
function BeCollide::switchOn(%this)
{
	Owner.setEnabled("1");
	return;
}
