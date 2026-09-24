// respawn.cs.dso
function respawn(%character)
{
	player::resetPlayerparts(%character);
	%character.setImmovable("0");
	if (!(isPlayer(%character)))
	{
		return isPlayer(%character);
	}
	if (spawnPathGroup.getCount())
	{
		%spawnNodeIndex = findNearestNode(player);
		%spawnPath = spawnPathGroup.getObject(getWord(%spawnNodeIndex, "0"));
		%nodeIndex = getWord(%spawnNodeIndex, "1");
		%spawnPath.transportPlayer(%nodeIndex);
	}
	else
	{
		spawnPoint.finishRespawn();
	}
	return;
}
function findNearestNode(%object)
{
	%pathIndex = 0;
	if (%object.getId() == player.getId() && useLastCollisionPoint)
	{
	}
	else
	{
	}
	%position = %object.getPosition();
	%actualNearestNode = spawnPathGroup.getObject("0").getNearestNode(%position);
	%nodeIndex = getWord(%actualNearestNode, "0");
	%lowestDistance = getWord(%actualNearestNode, "1");
	%p = 1;
	while (%p < spawnPathGroup.getCount())
	{
		%actualNearestNode = spawnPathGroup.getObject(%p).getNearestNode(%object.getPosition());
		%actualDistance = getWord(%actualNearestNode, "1");
		if (%actualDistance < %lowestDistance)
		{
			%lowestDistance = %actualDistance;
			%pathIndex = %p;
			%nodeIndex = getWord(%actualNearestNode, "0");
		}
		%p = %p + 1.0;
	}
	return %pathIndex SPC %nodeIndex;
	return %pathIndex SPC %nodeIndex;
}
