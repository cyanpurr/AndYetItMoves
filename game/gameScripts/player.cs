// player.cs.dso
function player::onLevelShutdown(%this, )
{
	if (isObject(fraggedPlayerPartGroup))
	{
		fraggedPlayerPartGroup.clear();
		fraggedPlayerPartGroup.delete();
	}
	return;
}
function initPlayer()
{
	%position = $initialSpawnPoint.getPosition();
	new AyimPlayer(Name : player)
	{
		scenegraph = daSceneGraph;
		animationName = "playerIdleAnimation";
		size = playerSize;
		Position = %position;
		canSaveDynamicFields = "1";
		_behavior0 = "BePlayer";
	}
	globals.setPlayer(player);
	player.stuckSlideCounter = "0";
	player.addCollisionGroups(playerWalkOnGroups);
	player.escapeSwitch = "1";
	subscribeToEvents(player, "onLevelShutdown onResetLevel");
	return;
}
function isPlayer(%obj, %alsoCheckTriggershape)
{
	if (!(isObject(player)))
	{
		return "0";
	}
	%isPlayer = %obj.getId() == player.getId();
	if (%alsoCheckTriggershape)
	{
		%isPlayer = %obj.getId() == triggerShape.getId();
	}
	return %isPlayer;
	return %isPlayer;
}
function player::setLayer(%this, %layer)
{
	t2dSceneObject::setLayer(%this, %layer);
	triggerEvent("onPlayerSwitchedLayer");
	return;
}
