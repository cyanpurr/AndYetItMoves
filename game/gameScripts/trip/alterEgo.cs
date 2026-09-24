// alterEgo.cs.dso
function alterEgoLoader::onLevelLoaded(%this, )
{
	subscribeToEvents(alterEgoLoader, "onLevelLoadFinished1 onLevelLoadFinished");
	return;
}
function alterEgoLoader::onLevelLoadFinished1(%this)
{
	createAlterEgo();
	return;
}
function alterEgoLoader::onLevelLoadFinished(%this)
{
	player.clonePlayerParts(alterEgo, $LAYER["player"], "1", "1");
	return;
}
function createAlterEgo()
{
	new AyimPlayer(Name : alterEgo)
	{
		class = "AlterEgo";
		superclass = "Player";
		scenegraph = scenegraph;
		Immovable = "0";
		animationName = "playerIdleAnimation";
		size = playerSize;
		canSaveDynamicFields = "1";
		_behavior0 = "BeAlterEgo";
		invertMovement = "1";
		origRotation = "180";
		spawnPoint = alterEgoSpawnpoint1;
		rippedEdgeMountForce = "15";
	}
	alterEgo.solvedPuzzle = "0";
	alterEgo.setInvertMainTex("1");
	alterEgo.setLayer($LAYER["player"]);
	scenegraph.pushToBack(alterEgo);
	alterEgo.setRotation(origRotation);
	alterEgo.addCollisionGroups(playerWalkOnGroups);
	subscribeToEvents(alterEgo, "onSpawnpointEnter onSpawnpointLeave onSpawnPointActivate");
	alterEgo.setActive("0");
	return;
}
function isAlterEgo(%obj, %alsoCheckTriggershape)
{
	if (!(isObject(%obj)))
	{
		return "0";
	}
	if (!(isObject(alterEgo)))
	{
		return "0";
	}
	%isAlterEgo = %obj.getId() == alterEgo.getId();
	if (%alsoCheckTriggershape)
	{
		%isAlterEgo = %obj.getId() == triggerShape.getId();
	}
	return %isAlterEgo;
	return %isAlterEgo;
}
function alterEgo::setActive(%this, %active)
{
	%this.isActive = %active;
	%this.setVisible(%active);
	%this.setPhysicsSuppress(%isAlterEgo || !(%active));
	%this.setCollisionSuppress(!(%active));
	triggerShape.setCollisionSuppress(!(%active));
	if (%active)
	{
		%this.getBehavior("BeGravitic").applyGravity();
		%this.getBehavior("BeAlterEgo").onRotationFinish();
		subscribeToEvents(alterEgo, "onDieFragged onDieExploding onDieOutside onPlayerReanimate");
	}
	else
	{
		%this.getBehavior("BeGravitic").stopGravity();
		%this.getBehavior("BeGravitic").resetSpeed();
		unSubscribeFromEvents(alterEgo, "onDieFragged onDieExploding onDieOutside onPlayerReanimate");
	}
	return;
}
function alterEgo::dieFragged(%this)
{
	if (!(hasDied))
	{
		%this.hasDied = "1";
		player::dieFragged(%this);
	}
	if (isDead)
	{
		return player;
	}
	%strength = t2dVectorLength(impactVelocityVector);
	player.dieExploding(%strength);
	return;
}
function alterEgo::dieSquashed(%this)
{
	if (!(hasDied))
	{
		%this.hasDied = "1";
		player::dieSquashed(%this);
	}
	if (isDead)
	{
		return player;
	}
	player.dieSquashed();
	return;
}
function alterEgo::dieExploding(%this, %strength)
{
	if (!(hasDied))
	{
		%this.hasDied = "1";
		player::dieExploding(%this, %strength);
	}
	if (isDead)
	{
		return player;
	}
	player.dieExploding(%strength);
	return;
}
function alterEgo::onShrinkStart(%this)
{
	%this.hasDied = "1";
	%this.stopConstantForce();
	if (!(isDead))
	{
		player.dieAlterEgoOutside();
	}
	return;
}
function alterEgo::reanimate(%this)
{
	return;
}
function alterEgo::onDieFragged(%this)
{
	if (hasDied)
	{
		return %this;
	}
	%this.hasDied = "1";
	%strength = t2dVectorLength(impactVelocityVector);
	player::dieExploding(%this, %strength);
	return;
}
function alterEgo::onDieExploding(%this)
{
	if (hasDied)
	{
		return %this;
	}
	%this.hasDied = "1";
	debugEcho("getting explodestrangth from player:" SPC lastExplodingStrenght);
	player::dieExploding(%this, lastExplodingStrenght);
	return;
}
function alterEgo::onDieOutside(%this)
{
	if (hasDied)
	{
		return %this;
	}
	%this.getBehavior("BeShrinker").startShrinking();
	%this.origMaxAngularVelocity = %this.getMaxAngularVelocity();
	%this.setMaxAngularVelocity("360");
	%this.setAngularVelocity("360");
	return;
}
function alterEgo::onPlayerReanimate(%this)
{
	if (!(isObject("alterEgoSpawnPoint" @ number)))
	{
		alterEgo.setActive("0");
		return;
	}
	debugEcho("reanimating alter ego");
	player::resetPlayerparts(%this);
	player::defrag(%this);
	if (isOuterSpace)
	{
		%this.dismount();
		%this.isOuterSpace = "0";
		unSubscribeFromEvent(%this.getBehavior("BeShrinker"), "onUpdateTick10");
	}
	%this.setSize(playerSize);
	%this.setSizeVelocity("0", "0");
	%this.setMaxAngularVelocity(origMaxAngularVelocity);
	%this.setAngularVelocity("0");
	%this.getBehavior("BeGravitic").stopGravity();
	%this.getBehavior("BeGravitic").resetSpeed();
	%this.spawnPoint = ['alterEgoSpawnPoint', 'number'];
	%this.upAngle = spawnPoint.getRotation() + 90.0;
	%this.getBehavior("BeKeepOrientation").origRotation = spawnPoint.getRotation();
	%this.getBehavior("BeKeepOrientation").onRotationUpdate();
	%this.setPosition(spawnPoint.getPosition());
	alterEgo.setActive("1");
	%this.hasDied = "0";
	return;
}
function alterEgoSpawnPoint::onLevelLoaded(%this, )
{
	subscribeToEvent(%this, "onLevelLoadFinished2");
	return;
}
function alterEgoSpawnPoint::onLevelLoadFinished2(%this)
{
	%this.setCollisionActive("1", "0");
	%this.setCollisionPhysics("0", "0");
	%this.setCollisionDetection("full");
	%this.setCollisionGroups($GROUPS["player"]);
	%this.setEnterCallback("0");
	%this.setStayCallback("1");
	%this.setLeaveCallback("1");
	%this.visualSpawnPoint = new t2dStaticSprite(Name : "")
	{
		scenegraph = scenegraph;
		imageMap = "pointingSpawnpointImageMap";
		frame = "1";
		size = "22.000 44.000";
	}
	visualSpawnPoint.setLayer($LAYER["mainBehindPlayer"]);
	visualSpawnPoint.mount(%this, "0 0", "0", "1", "1", "0", "0");
	return;
}
function alterEgoSpawnPoint::onStay(%this, %obj)
{
	if (isPlayer(%obj))
	{
		if (!(playerInAlterEgoSpawnpoint) && !(solvedPuzzle))
		{
			activeGlow.setVisible("1");
			playEventSound(TripAlterEgoActivate, "0.5");
		}
		alterEgo.playerInAlterEgoSpawnpoint = "1";
		if (isInSpawnpoint && !(isDead) && !(isFalling) && !(isDead) && !(isFalling))
		{
			%this.targetReached();
		}
	}
	return;
}
function alterEgoSpawnPoint::onLeave(%this, %obj)
{
	if (isPlayer(%obj))
	{
		alterEgo.playerInAlterEgoSpawnpoint = "0";
		activeGlow.setVisible("0");
		if (solvedPuzzle)
		{
			return alterEgo;
		}
		playEventSound(TripAlterEgoDeactivate, "0.3");
	}
	return;
}
function alterEgoSpawnPoint::targetReached(%this)
{
	alterEgo.solvedPuzzle = "1";
	activeGlow.setVisible("0");
	activeGlow.setVisible("0");
	alterEgo.playerInAlterEgoSpawnpoint = "0";
	alterEgo.isInSpawnpoint = "0";
	alterEgo.getBehavior("BeGravitic").switchOff();
	alterEgo.setCollisionSuppress("1");
	spawnPoint.setPosition(%this.getPosition());
	spawnPoint.setRotation(%this.getRotation());
	if (postDirection != "")
	{
		visualSpawnPoint.setFlipX(postDirection $= "right");
	}
	alterEgo.setActive("0");
	%selfDiscoverySwitch = %this.getBehavior("BeSelfDiscoverySwitch");
	debugEcho("changing spawnpoint direction" SPC directionToGo SPC %selfDiscoverySwitch);
	if (directionToGo != "useOriginal")
	{
		%spawnpointBehavior = spawnPoint.getBehavior("BeSpawnpoint");
		%spawnpointBehavior.spawnPointIsFlipped = directionToGo $= "right";
		%spawnpointBehavior.directionToGo = directionToGo;
		%spawnpointBehavior.activatePointingSpawnpoint();
	}
	%selfDiscoverySwitch.leave();
	return;
}
function alterEgo::onSpawnpointEnter(%this)
{
	if (!(isAlterEgo(enteredLast, "1")))
	{
		return isAlterEgo(enteredLast, "1");
	}
	%this.isInSpawnpoint = "1";
	activeGlow.setVisible("1");
	playEventSound(TripAlterEgoActivate, "0.5");
	return;
}
function alterEgo::onSpawnpointLeave(%this)
{
	if (!(isAlterEgo(lastLeft, "1")))
	{
		return isAlterEgo(lastLeft, "1");
	}
	%this.isInSpawnpoint = "0";
	activeGlow.setVisible("0");
	if (!(solvedPuzzle))
	{
		playEventSound(TripAlterEgoDeactivate, "0.3");
	}
	return;
}
function alterEgo::onSpawnpointActivate(%this)
{
	%this.onPlayerReanimate();
	return;
}
if (!(isObject(BeAlterEgo)))
{
	%template = new BehaviorTemplate(Name : BeAlterEgo);
	%template.friendlyName = "";
	%template.behaviorType = "";
	%template.description = "";
}
function BeAlterEgo::onBehaviorAdd(%this)
{
	BePlayer::onBehaviorAdd(%this);
	%owner = Owner;
	%owner.disableUpdateCallback();
	%owner.removeBehavior(%owner.getBehavior("BeControlPlayerSounds"));
	unSubscribeFromEvent(%this, "onLevelLoadFinished10");
	return;
}
function BeAlterEgo::onLevelLoadFinished20(%this)
{
	%owner = Owner;
	BePlayer::onLevelLoadFinished20(%this);
	unSubscribeFromEvent(%this, "onPlayerStateChange");
	%owner.getBehavior("BeKeepOrientation").origRotation = origRotation;
	%owner.getBehavior("BeKeepOrientation").useFrameUpdate = "1";
	%owner.getBehavior("BeShrinker").onShrinkFinished = "none";
	%owner.getBehavior("BeShrinker").shrinkStartCallback = "onShrinkStart";
	%owner.getBehavior("BeShrinker").resizeOnFinish = "1";
	%owner.getBehavior("BeShrinker").shrinkChildren = "0";
	return;
}
function BeAlterEgo::onUpdateTick32ms(%this)
{
	if (!(isActive))
	{
		return alterEgo;
	}
	BePlayer::onUpdateTick32ms(%this);
	return;
}
function BeAlterEgo::onRotationStart(%this)
{
	if (!(isActive))
	{
		return Owner;
	}
	BePlayer::onRotationStart(%this);
	return;
}
function BeAlterEgo::onRotationFinish(%this)
{
	if (!(isActive))
	{
		return Owner;
	}
	BePlayer::onRotationFinish(%this);
	%owner = Owner;
	%owner.angularVel = "0";
	%owner.setAngularVelocity("0");
	%alterGravity = rotateVector(%owner.getConstantForce(), spawnPoint.getRotation());
	%owner.setConstantForce(%alterGravity);
	%owner.setGraviticConstantForce("1");
	return;
}
function alterEgo::onMoveInputChange(%this, )
{
	player::onMoveInputChange(%this, !(invertMovement));
	return;
}
function alterEgo::doTheJump(%this)
{
	if (jumpDelayed)
	{
		return groundCollisionObject;
	}
	%this.jump = "0";
	groundCollisionObject.setCollisionSuppress("1");
	callNextFrame(groundCollisionObject, "setCollisionSuppress, false");
	if (modulo(spawnPoint.getRotation(), "180") == 0.0)
	{
		%groundJumpVel = getVerticalComponent(lastContactPointVelocity);
	}
	else
	{
		%groundJumpVel = getHorizontalComponent(lastContactPointVelocity);
	}
	%jumpSpeedFactor = -1.0;
	if (spawnPoint.getRotation() >= 180.0)
	{
		%jumpSpeedFactor = 1;
	}
	if (%groundJumpVel * -1.0 * %jumpSpeedFactor > 0.0)
	{
		%groundJumpVel = %groundJumpVel * 0.4000000059604645;
	}
	if (modulo(spawnPoint.getRotation(), "180") == 0.0)
	{
		%this.setVerticalVelocity(%jumpSpeedFactor * jumpSpeed + %groundJumpVel);
	}
	else
	{
		%this.setHorizontalVelocity(%jumpSpeedFactor * jumpSpeed + %groundJumpVel);
	}
	%this.isFalling = "1";
	return;
}
