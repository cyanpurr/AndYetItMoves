// spawnPath.cs.dso
if (!(isObject(BeSpawnPath)))
{
	%template = new BehaviorTemplate(Name : BeSpawnPath);
	%template.friendlyName = "Respawn Path";
	%template.behaviorType = "MetaGameMechanisms";
	%template.description = "A path the player moves along, to the last spawnpoint, when died";
	%template.addBehaviorField(firstPointIndex, "index of the first spawnpoint in level or nearest to mother path", integer, "1");
	%template.addBehaviorField(lastPointIndex, "index of the last point in path after last spawnpoint", integer, "2");
	%template.addBehaviorField(mother, "the main path for this side path", object, null, t2dSceneObject);
	%template.addBehaviorField(useLastCollisionPoint, "if, on player death, the player should be mounted to a node close to where he jumped off, instead of where he died", bool, "0");
}
function BeSpawnPath::onBehaviorAdd(%this)
{
	if (Owner.getClassName() != "t2dPath")
	{
		debugWarn("owner is not a path!");
		Owner.removeBehavior(%this);
		return;
	}
	%owner = Owner;
	%owner.class = "SpawnPath";
	subscribeToEvents(%this, "onLevelLoadFinished");
	return;
}
function BeSpawnPath::onLevelLoadFinished(%this)
{
	%owner = Owner;
	spawnPathGroup.add(%owner);
	%owner.elevator = new t2dSceneObject(Name : "")
	{
		scenegraph = %owner.getSceneGraph();
		Position = %owner.getPosition();
		_behavior0 = "BeDontCollide";
	}
	%diff = firstPointIndex - lastPointIndex;
	%this.forward = %diff == 1.0 || %diff == -1.0 * %owner.getNodeCount() - 1.0;
	return;
}
function SpawnPath::getLogicalIndex(%this, %internalIndex)
{
	%behavior = %this.getBehavior("BeSpawnPath");
	if (%internalIndex > %this.getNodeCount() - 1.0)
	{
		debugWarn("path index out of bounds for obj" SPC %this);
		return -1.0;
	}
	if (forward)
	{
		%internalIndex = %internalIndex - firstPointIndex;
		if (%internalIndex < 0.0)
		{
			%internalIndex = %internalIndex + %this.getNodeCount();
		}
	}
	else
	{
		%internalIndex = %internalIndex + %this.getNodeCount() - 1.0 - firstPointIndex % %this.getNodeCount();
		%internalIndex = -1.0 * %internalIndex + %this.getNodeCount() - 1.0;
	}
	return %internalIndex;
	return %internalIndex;
}
function SpawnPath::getNearestNode(%this, %position)
{
	%nearestNodeIndex = 0;
	%lowestDistance = t2dVectorDistance(%position, %this.getNodePosition("0"));
	%n = 1;
	while (%n < %this.getNodeCount())
	{
		%actualDistance = t2dVectorDistance(%position, %this.getNodePosition(%n));
		if (%actualDistance < %lowestDistance)
		{
			%lowestDistance = %actualDistance;
			%nearestNodeIndex = %n;
		}
		%n = %n + 1.0;
	}
	return %nearestNodeIndex SPC %lowestDistance;
	return %nearestNodeIndex SPC %lowestDistance;
}
function SpawnPath::transportPlayer(%this, %nodeIndex)
{
	%behavior = %this.getBehavior("BeSpawnPath");
	%this.detachObject(elevator);
	elevator.setPosition(%this.getNodePosition(%nodeIndex));
	elevator.setRotation("0");
	sceneWindow2d.setMountForce("7");
	camera.setMountForce("7");
	player.mount(elevator, "0 0", "0", "0", "0", "0", "0");
	%pathIndex = pathIndex;
	%pathPoint = pathPoint;
	%pathObject = spawnPathGroup.getObject(%pathIndex);
	if (%pathObject.getId() == %this.getId())
	{
		%spawnIndex = %pathPoint;
	}
	else
	{
		if (isObject(mother))
		{
			%spawnIndex = firstPointIndex;
			break;
		}
		%pathIntersection = %pathObject.getNodePosition(firstPointIndex);
		%spawnIndex = getWord(%this.getNearestNode(%pathIntersection), "0");
	}
	%logicalSpawnIndex = %this.getLogicalIndex(%spawnIndex);
	%logicalDeathIndex = %this.getLogicalIndex(%nodeIndex);
	if (%logicalSpawnIndex == %logicalDeathIndex)
	{
		%this.onPathFinished(%logicalSpawnIndex);
	}
	else
	{
		if (%logicalSpawnIndex - %logicalDeathIndex > 0.0)
		{
		}
		else
		{
		}
		%direction = -1.0;
		if (!(forward))
		{
			%direction = -1.0 * mRound(%direction);
		}
		%this.attachObject(elevator, respawnSpeed, %direction, %nodeIndex, %spawnIndex, "WRAP", "1", "1");
	}
	return;
}
function SpawnPath::onPathFinished(%this, )
{
	%pathIndex = pathIndex;
	%pathObject = spawnPathGroup.getObject(%pathIndex);
	if (%pathObject.getId() == %this.getId())
	{
		spawnPoint.finishRespawn();
	}
	else
	{
		%pathObject.transportPlayer(getWord(%pathObject.getNearestNode(elevator.getPosition()), "0"));
	}
	return;
}
function SpawnPath::onUpdateTick10(%this)
{
	return;
}
