// moving.cs.dso
if (!(isObject(BeMoving)))
{
	%template = new BehaviorTemplate(Name : BeMoving);
	%template.friendlyName = "Moving";
	%template.behaviorType = "Physics";
	%template.description = "lets this object move and fall according to gravity";
	%template.addBehaviorField(Density, "density of object", float, "0.03");
	%template.addBehaviorField(Friction, "friction of object", float, "0.25");
	%template.addBehaviorField(Restitution, "description of field", float, "0");
	%template.addBehaviorField(Damping, "", float, "0.3");
	%template.addBehaviorField(CollisionGroups, "a space sep. list of groups the object will react to (not player! player is handled by influencedByPlayer)", string, "moving collide");
	%template.addBehaviorField(influencedByPlayer, "if player pushes object", bool, "0");
	%template.addBehaviorField(useOwnLayer, "if true this object will keep its set layer in tgb otherwise it will b e set to moving layer", bool, "0");
	%template.addBehaviorField(isGravitic, "if the object shall get gravity or just float around", bool, "1");
	%template.addBehaviorField(isDekoMoving, "this moving will be mounted on dekolayers on start, and dismount as soon as its in the right place", bool, "0");
}
function BeMoving::onBehaviorAdd(%this)
{
	%owner = Owner;
	if (isGravitic)
	{
		%owner.addDependentBehavior("BeGravitic");
	}
	subscribeToEvents(%this, "onLevelLoadFinished7 onLevelLoadFinished30");
	return;
}
function BeMoving::onLevelLoadFinished7(%this)
{
	%this.init();
	return;
}
function BeMoving::onLevelLoadFinished30(%this)
{
	%owner = Owner;
	if (!(useOwnLayer))
	{
		%maskBehavior = %owner.getBehavior("BeMask");
		if (!(isObject(%maskBehavior)))
		{
			%maskBehavior.Layer = "moving";
		}
		%owner.setLayer($LAYER["moving"]);
		%mountedKids = %owner.getMountedChildren();
		%i = 0;
		while (%i < getWordCount(%mountedKids))
		{
			%mountedChild = getWord(%mountedKids, %i);
			if (%mountedChild.getBehavior("BeMask"))
			{
				%mountedChild.getBehavior("BeMask").Layer = "moving";
				%mountedChild.setLayer($LAYER["moving"]);
				break;
			}
			%i = %i + 1.0;
		}
	}
	%owner.setMountedCollidesNotImmovable();
	if (!(isGravitic))
	{
		addShrinkerBehavior(%owner);
	}
	if (isDekoMoving)
	{
		subscribeToEvents(%this, "onPlayerDeath onPlayerReanimate");
		%owner.setGraphGroup($GROUPS["dekoCollide"]);
		triggerShape.safeDelete();
	}
	return;
}
function BeMoving::init(%this, %rippedEdgeInteraction)
{
	%owner = Owner;
	if (%rippedEdgeInteraction $= "")
	{
		%rippedEdgeInteraction = 1;
	}
	%owner.setCollisionActive("1", "1");
	%owner.setCollisionPhysics("1", "0");
	%owner.setCollisionDetection("FULL");
	%owner.setCollisionResponse("RIGID");
	%owner.setCollisionMaxIterations("10");
	%owner.setGraphGroup($GROUPS["moving"]);
	%this.initMovingCollisionGroups(CollisionGroups);
	%this.applyPhysicConstants();
	if (%rippedEdgeInteraction)
	{
		%this.createTriggerShape();
	}
	return;
}
function BeMoving::onBehaviorRemove(%this)
{
	%owner = Owner;
	%owner.removeBehavior(%owner.getBehavior("BeGravitic"));
	if (isObject(triggerShape))
	{
		triggerShape.safeDelete();
	}
	return;
}
function BeMoving::createTriggerShape(%this)
{
	%owner = Owner;
	%owner.triggerShape = new t2dSceneObject(Name : "")
	{
		scenegraph = scenegraph;
		size = %owner.getSize();
	}
	triggerShape.setCollisionActive("1", "0");
	triggerShape.setCollisionPhysics("0", "0");
	triggerShape.setCollisionPolyCustom(%owner.getCollisionPolyCount(), %owner.getCollisionPoly());
	triggerShape.setCollisionDetection("POLYGON");
	triggerShape.setGraphGroup($GROUPS["dontCollide"]);
	triggerShape.setCollisionGroups($GROUPS["rippedEdgeTrigger"] SPC $GROUPS["rippedEdge"]);
	triggerShape.mount(%owner, "0 0", "0", "1");
	triggerShape.shapeParent = %owner;
	return;
}
function BeMoving::initMovingCollisionGroups(%this, %groupList)
{
	%owner = Owner;
	%this.CollisionGroups = %groupList;
	%i = 0;
	while (%i < getWordCount(CollisionGroups))
	{
		%groups = %groups SPC $GROUPS[getWord(CollisionGroups, %i)];
		%i = %i + 1.0;
	}
	if (influencedByPlayer)
	{
		%groups = %groups SPC $GROUPS["player"];
	}
	%owner.setCollisionGroups(%groups);
	return;
}
function BeMoving::applyPhysicConstants(%this)
{
	%owner = Owner;
	%owner.setDensity(Density);
	%owner.setFriction(Friction);
	%owner.setRestitution(Restitution);
	if (!(%owner.getBehavior("BeSwing")))
	{
		if (customDamping $= "")
		{
			%owner.setDamping(Damping);
			break;
		}
		%owner.setDamping(customDamping);
		%owner.setMaxLinearVelocity("350");
	}
	return;
}
function BeMoving::onPlayerDeath(%this)
{
	Owner.setPauseUpdateMount("0");
	return;
}
function BeMoving::onPlayerReanimate(%this)
{
	callNextFrame(Owner, "setPauseUpdateMount, true", "100");
	return;
}
