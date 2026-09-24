// partOfPlayer.cs.dso
if (!(isObject(BePartOfPlayer)))
{
	%template = new BehaviorTemplate(Name : BePartOfPlayer);
	%template.friendlyName = "Part of fragged Player";
	%template.behaviorType = "Object";
	%template.description = "makes this object a part of the fragged player that flies around when the player dies";
}
function BePartOfPlayer::onBehaviorAdd(%this)
{
	%owner = Owner;
	%mountBehavior = %owner.addDependentBehavior("BeMountWithOffset");
	%mountBehavior.trackRotation = "1";
	%mountBehavior.setMother(player);
	%mountBehavior.setForce(spawnMountForce);
	%owner.setGraphGroup($GROUPS["partOfPlayer"]);
	%owner.setCollisionGroups($GROUPS["collide"] SPC $GROUPS["moving"] SPC $GROUPS["partOfPlayer"] SPC $GROUPS["rippedEdgeTrigger"]);
	%owner.setLayer($LAYER["player"]);
	%owner.setCollisionLayers(excludeBits($LAYER["player"] + 1.0));
	%owner.setCollisionActive("1", "1");
	%owner.setCollisionPhysics("1", "0");
	%owner.setCollisionResponse("RIGID");
	%owner.setCollisionDetection("POLYGON");
	%owner.setMaxAngularVelocity("360");
	%owner.setMaxLinearVelocity(MaxLinearVelocity);
	%owner.setDamping("2");
	subscribeToEvents(%this, "onLevelLoadFinished10 onLevelLoadFinished20");
	return;
}
function BePartOfPlayer::onLevelLoadFinished10(%this)
{
	%owner = Owner;
	%owner.addCollisionGroups(playerWalkOnGroups);
	%owner.setVisible("0");
	%owner.setCollisionSuppress("1");
	%owner.originalRotation = %owner.getRotation();
	%owner.escapeSwitch = "1";
	return;
}
function BePartOfPlayer::onLevelLoadFinished20(%this)
{
	%owner = Owner;
	%owner.setMountForce("0");
	%maskBehavior = %owner.addDependentBehavior("BeMask");
	%maskBehavior.usePositive = "1";
	%maskBehavior.dontCollide = "0";
	return;
}
function BePartOfPlayer::changeLayer(%this, %aboveRippedEdge)
{
	%owner = Owner;
	if (%aboveRippedEdge)
	{
		%owner.origLayer = %owner.getLayer();
		%owner.setLayer($LAYER["rippedEdge"] - 1.0);
	}
	return;
}
function initPlayerParts()
{
	player.fraggedPlayerPartGroup = new SimSet(Name : "");
	levelGarbageCollector.add(fraggedPlayerPartGroup);
	new t2dStaticSprite(Name : player_foot_left)
	{
		scenegraph = "daScenegraph";
		imageMap = "playerPartsImageMap";
		frame = "2";
		canSaveDynamicFields = "1";
		size = "4.000 8.733";
		Rotation = "5";
		CollisionPolyList = "0.681 0.358 -0.754 0.877 -0.536 -0.867 0.072 -0.883 0.580 -0.735";
	}
	fraggedPlayerPartGroup.add(player_foot_left);
	new t2dStaticSprite(Name : player_foot_right)
	{
		scenegraph = "daScenegraph";
		imageMap = "playerPartsImageMap";
		frame = "4";
		canSaveDynamicFields = "1";
		size = "3.725 7.512";
		Rotation = "5";
		CollisionPolyList = "0.749 0.028 0.419 0.712 -0.590 0.804 -0.647 -0.893 0.094 -0.994 0.550 -0.877";
	}
	fraggedPlayerPartGroup.add(player_foot_right);
	new t2dStaticSprite(Name : player_leg_left)
	{
		scenegraph = "daScenegraph";
		imageMap = "playerPartsImageMap";
		frame = "6";
		canSaveDynamicFields = "1";
		size = "4.098 8.405";
		CollisionPolyList = "-0.647 -0.851 0.598 -0.799 0.598 -0.198 0.548 0.799 -0.548 0.799 -0.697 -0.037";
	}
	fraggedPlayerPartGroup.add(player_leg_left);
	new t2dStaticSprite(Name : player_leg_right)
	{
		scenegraph = "daScenegraph";
		imageMap = "playerPartsImageMap";
		frame = "5";
		canSaveDynamicFields = "1";
		size = "4.294 8.177";
		CollisionPolyList = "0.720 0.613 0.434 0.811 -0.049 0.834 -0.434 0.667 -0.703 -0.235 -0.709 -0.776 -0.011 -0.898 0.654 -0.744";
	}
	fraggedPlayerPartGroup.add(player_leg_right);
	new t2dStaticSprite(Name : player_hand_left)
	{
		scenegraph = "daScenegraph";
		imageMap = "playerPartsImageMap";
		frame = "8";
		canSaveDynamicFields = "1";
		size = "2.900 7.000";
		Rotation = "175";
		FlipX = "1";
		CollisionPolyList = "0.368 0.692 -0.493 0.789 -0.775 0.643 -0.420 -0.578 0.551 -0.766";
	}
	fraggedPlayerPartGroup.add(player_hand_left);
	new t2dStaticSprite(Name : player_arm_left)
	{
		scenegraph = "daScenegraph";
		imageMap = "playerPartsImageMap";
		frame = "7";
		canSaveDynamicFields = "1";
		size = "3.106 6.855";
		Rotation = "15";
		CollisionPolyList = "-0.179 0.717 -0.590 0.586 -0.590 -0.593 0.036 -0.749 0.472 -0.508 0.518 0.726";
	}
	fraggedPlayerPartGroup.add(player_arm_left);
	new t2dStaticSprite(Name : player_head)
	{
		scenegraph = "daScenegraph";
		imageMap = "playerPartsImageMap";
		frame = "0";
		canSaveDynamicFields = "1";
		size = "5.852 8.486";
		Rotation = "30";
		CollisionPolyList = "0.244 0.794 -0.218 0.815 -0.670 0.634 -0.930 -0.241 -0.771 -0.589 -0.152 -0.809 0.336 -0.831 0.965 -0.289";
	}
	fraggedPlayerPartGroup.add(player_head);
	new t2dStaticSprite(Name : player_torso)
	{
		scenegraph = "daScenegraph";
		imageMap = "playerPartsImageMap";
		frame = "1";
		canSaveDynamicFields = "1";
		size = "7.198 11.074";
		CollisionPolyList = "0.529 -0.789 0.685 0.929 -0.499 0.705 -0.693 -0.205 -0.633 -0.699 -0.257 -0.908 0.421 -0.911";
	}
	fraggedPlayerPartGroup.add(player_torso);
	new t2dStaticSprite(Name : player_hand_right)
	{
		scenegraph = "daScenegraph";
		imageMap = "playerPartsImageMap";
		frame = "3";
		canSaveDynamicFields = "1";
		size = "3.000 9.388";
		Rotation = "5";
		CollisionPolyList = "0.543 0.577 -0.147 0.781 -0.379 0.635 -0.526 0.288 -0.603 -0.440 -0.569 -0.775 0.276 -0.854";
	}
	fraggedPlayerPartGroup.add(player_hand_right);
	new t2dStaticSprite(Name : player_arm_right)
	{
		scenegraph = "daScenegraph";
		imageMap = "playerPartsImageMap";
		frame = "7";
		canSaveDynamicFields = "1";
		size = "3.346 6.617";
		Rotation = "355";
		CollisionPolyList = "-0.179 0.717 -0.590 0.586 -0.590 -0.593 0.036 -0.749 0.472 -0.508 0.518 0.726";
	}
	fraggedPlayerPartGroup.add(player_arm_right);
	if (!(isObject(playerPartOffsets)))
	{
		new ScriptObject(Name : playerPartOffsets)
		{
			player_foot_left = "-2.136 11.384";
			player_foot_right = "1.599 11.720";
			player_leg_left = "-1.798 4.808";
			player_leg_right = "1.503 5.162";
			player_hand_left = "-3.290 0.425";
			player_arm_left = "-2.830 -5.317";
			player_head = "0.988 -13.049";
			player_torso = "-0.191 -3.696";
			player_hand_right = "3.618 1.063";
			player_arm_right = "2.989 -5.764";
		}
	}
	%referencePlayerSize = "22.178 44.358";
	%playerScaledFactor = player.getSizeY() / getWord(%referencePlayerSize, "1");
	%i = 0;
	while (%i < fraggedPlayerPartGroup.getCount())
	{
		%part = fraggedPlayerPartGroup.getObject(%i);
		%part.setSize(t2dVectorScale(%part.getSize(), %playerScaledFactor));
		%offset = eval("t2dVectorScale( playerPartOffsets." @ %part.getName() @ ", %playerScaledFactor );");
		%part.setPosition(t2dVectorAdd(player.getPosition(), %offset));
		%partOfPlayerBehavior = %part.addDependentBehavior("BePartOfPlayer");
		%i = %i + 1.0;
	}
	return fraggedPlayerPartGroup.getCount();
}
