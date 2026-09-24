// arenaMonster.cs.dso
if (!(isObject(BeArenaMonster)))
{
	%template = new BehaviorTemplate(Name : BeArenaMonster);
	%template.friendlyName = "ArenaMonster";
	%template.behaviorType = "LevelPrototype";
	%template.description = "folows the player, kill him on touch and is unconscious for a short time after strong collision";
	%template.addBehaviorField(stumbleSpeed, "the velocity at which the monster will stumble after a collision after falling", float, "100");
	%template.addBehaviorField(runSpeed, "the speed at which the monster stumbles if running into the wall", float, "70");
}
function BeArenaMonster::onBehaviorAdd(%this)
{
	%owner = Owner;
	%owner.addDependentBehaviors("BeFollowPlayer BeKeepOrientation BeControlMonsterSounds");
	subscribeToEvents(%this, "onLevelLoadFinished10");
	return;
}
function BeArenaMonster::onLevelLoadFinished10(%this)
{
	%owner = Owner;
	%owner.getBehavior("BeShrinker").shrinkStartCallback = "isKilled";
	%owner.setName("arenaMonster");
	%this.setBehaviorCollisionCallback("0");
	%owner.getBehavior("BeMoving").influencedByPlayer = "0";
	%this.maxSlope = maxSlope;
	%this.timeToFall = timeToFall;
	%this.curState = "walk";
	%this.playerColCounter = "0";
	%this.horns = new t2dSceneObject(Name : jungleMonsterHorns)
	{
		scenegraph = daSceneGraph;
		Position = %owner.getLinkPoint("1");
		size = "16 8";
		_behavior0 = "BeReactOnCollision	reactToGroups	player	continuousCollisionTime	0.3	criticalSpeed	30	useOwnerSpeed	1";
		monster = %owner;
	}
	horns.setGraphGroup($GROUPS["jungleMonsterHorns"]);
	horns.setCollisionGroups("");
	horns.setCollisionActive("1", "1");
	horns.mountToLinkpoint(%owner, "1", "0", "1", "1", "1", "1");
	horns.getBehavior("BeReactOnCollision").BehaviorList = horns;
	%this.stars = new t2dAnimatedSprite(Name : stumbledStars)
	{
		scenegraph = scenegraph;
		animationName = "stumbledStarsAnimation";
		size = "30 15";
	}
	stars.setLayer(%owner.getLayer());
	stars.setVisible("0");
	%owner.localFootPoint = %owner.getLocalPoint(%owner.getLinkPoint("2"));
	%this.followBehavior = %owner.getBehavior("BeFollowPlayer");
	followBehavior.pause();
	%owner.getBehavior("BeKeepOrientation").orientationMode = "FULL";
	%owner.getBehavior("BeKeepOrientation").useFrameUpdate = "1";
	unSubscribeFromEvent(%this, "onLevelLoadFinished10");
	%this.currentVelocityDirection = "1";
	return;
}
function BeArenaMonster::switchOn(%this)
{
	%owner = Owner;
	%this.setBehaviorCollisionCallback("1");
	%owner.forwardPlayerCollisionCallback = "1";
	horns.getBehavior("BeReactOnCollision").switchOn();
	followBehavior.play();
	%this.setState("walk");
	subscribeToEvents(%this, "onUpdateTick32ms onRotationStart onRotationFinish");
	return;
}
function BeArenaMonster::switchOff(%this)
{
	%owner = Owner;
	%this.setBehaviorCollisionCallback("0");
	%owner.forwardPlayerCollisionCallback = "0";
	horns.getBehavior("BeReactOnCollision").switchOff();
	followBehavior.pause();
	unSubscribeFromEvents(%this, "onUpdateTick32ms onRotationStart onRotationFinish");
	return;
}
function BeArenaMonster::onUpdateTick32ms(%this)
{
	%owner = Owner;
	%timeSinceLastGroundCollision = thisTime - groundCollisionTime;
	if (!(isFalling))
	{
		%this.isFalling = %timeSinceLastGroundCollision > timeToFall;
		%curVel = %owner.getLinearVelocity();
		if (equalsTolerance(getHorizontalComponent(%owner.getPosition()), getHorizontalComponent(oldPosition), "0.2"))
		{
			%this.standingStillCounter = standingStillCounter + 1.0;
		}
		else
		{
			%this.standingStillCounter = "0";
		}
		%curVelDir = mRound(getHorizontalComponent(%curVel));
		if (mAbs(%curVelDir) > 5.0)
		{
			%this.currentVelocityDirection = getSign(%curVelDir);
		}
		if (getSign(getHorizontalComponent(direction)) != currentVelocityDirection)
		{
			%this.isBreaking = "1";
		}
		else
		{
			if (isBreaking)
			{
				%this.isBreaking = "0";
			}
		}
		%owner.setFlipX(!(currentVelocityDirection + 1.0));
	}
	if (playerCollision)
	{
		if (thisTime - playerCollisionTime > 3.0 * duration)
		{
			%this.playerCollision = "0";
		}
	}
	%this.updateStateMachine();
	%this.oldPosition = %owner.getPosition();
	return;
}
function BeArenaMonster::updateStateMachine(%this)
{
	%owner = Owner;
	if (curState $= "walk")
	{
		if (isFalling)
		{
			%this.setState("fall");
		}
		else
		{
			if (thisTime - walkStartTime > walkDuration)
			{
				%this.setState("run");
				break;
			}
			if (playerCollision)
			{
				%this.setState("rodeo");
			}
		}
	}
	else
	{
		if (curState $= "run")
		{
			if (isFalling)
			{
				%this.setState("fall");
			}
			else
			{
				if (isBreaking)
				{
					%this.setState("break");
					break;
				}
				if (playerCollision)
				{
					%this.setState("rodeo");
				}
			}
			break;
		}
		if (curState $= "rodeo")
		{
			if (isFalling)
			{
				%this.setState("fall");
			}
			if (!(playerCollision))
			{
				%this.setState("walk");
			}
			if (standingStillCounter > 10.0)
			{
				followBehavior.direction = rotateVector(direction, "180");
				%this.standingStillCounter = "0";
			}
			break;
		}
		if (curState $= "fall")
		{
			if (!(isFalling))
			{
				%this.setState("walk");
			}
			break;
		}
		if (curState $= "break")
		{
			if (isFalling)
			{
				%this.setState("fall");
			}
			if (!(isBreaking))
			{
				%this.setState("walk");
			}
		}
	}
	return;
}
function BeArenaMonster::setState(%this, %state)
{
	%owner = Owner;
	triggerEvent("onMonsterStateChange" @ curState @ %state);
	triggerEvent("onMonsterStateChange" @ %state);
	debugEcho("monster StateChange" SPC curState SPC %state);
	%this.curState = %state;
	followBehavior.keepOldDirection = "0";
	%owner.setMaxAngularVelocity("20");
	if (curState $= "break")
	{
		followBehavior.accelFactor = "100";
	}
	else
	{
		if (curState $= "walk")
		{
			followBehavior.keepOldDirection = "0";
			%this.walkStartTime = thisTime;
			%this.walkDuration = floatRandom("0.5", "2");
			followBehavior.play();
			followBehavior.accelFactor = "50";
			followBehavior.maxVelocity = "50";
			horns.setCollisionSuppress("0");
			horns.getBehavior("BeReactOnCollision").initGroups("player");
			%owner.playAnimation("stieranimaitonAnimation");
			break;
		}
		if (curState $= "run")
		{
			followBehavior.accelFactor = "80";
			followBehavior.maxVelocity = "120";
			break;
		}
		if (curState $= "rodeo")
		{
			followBehavior.accelFactor = "100";
			followBehavior.maxVelocity = "140";
			break;
		}
		if (curState $= "fall")
		{
			followBehavior.pause();
			followBehavior.keepOldDirection = "1";
			Owner.setMaxAngularVelocity("0");
			horns.getBehavior("BeReactOnCollision").initGroups("");
		}
	}
	return;
}
function BeArenaMonster::onPlayerCollision(%this, , , , , %normal, , %contacts)
{
	if (isDead)
	{
		return player;
	}
	%owner = Owner;
	%owner.localFootPoint = %owner.getLocalPoint(getWords(%contacts, "0", "1"));
	%this.playerCollision = "1";
	%this.playerCollisionTime = thisTime;
	return;
}
function BeArenaMonster::onCollision(%this, , , , , %normal, , %contacts)
{
	%owner = Owner;
	%impactSpeed = getImpactSpeed(%owner.getLinearVelocity(), %normal);
	%normalAngle = t2dRelativeAngleBetween(horizontalVector, %normal);
	if (!(camera.getIsRotating()) && !(isStumbled))
	{
		if (equalsAngleTolerance(%normalAngle, "90", "45"))
		{
			if (%impactSpeed > stumbleSpeed)
			{
				callNextFrame(%this, "callNextFrame");
			}
			break;
		}
		%localX = getWord(%owner.getLocalPoint(getWords(%contacts, "0", "1")), "0");
		if (%owner.getFlipX())
		{
		}
		else
		{
		}
		%localX = %localX;
		if (%impactSpeed > runSpeed && %localX > 0.0)
		{
			%this.stumble();
		}
	}
	if (equalsAngleTolerance(%normalAngle, "90", maxSlope))
	{
		%this.groundCollisionTime = thisTime;
		%this.isFalling = "0";
	}
	return;
}
function jungleMonsterHorns::reactOnCollision(%this, , , , , %normal, , %contacts, )
{
	if (groundCollisionObject.getImmovable())
	{
		debugEcho("killing the player with se horns" SPC isFalling);
		player.lastDeathEvent = "monster";
		player.dieFragged();
	}
	return;
}
function BeArenaMonster::stumble(%this)
{
	%owner = Owner;
	%this.setState("stumbled");
	%owner.playAnimation("stieranimaitonAnimationStumbled");
	stars.mountToLinkpoint(%owner, "3", "10", "1", "1", "0", "1");
	stars.setVisible("1");
	followBehavior.pause();
	%owner.setAtRest();
	horns.setCollisionSuppress("1");
	unSubscribeFromEvent(%this, "onUpdateTick32ms");
	safeSchedule("2000", %this, "standUp");
	return;
}
function BeArenaMonster::standUp(%this)
{
	stars.dismount();
	stars.setRotation("0");
	stars.setVisible("0");
	if (!(isFalling))
	{
		%owner = Owner;
		%this.setState("walk");
		subscribeToEvent(%this, "onUpdateTick32ms");
	}
	else
	{
		safeSchedule("300", %this, "standUp");
	}
	return;
}
function BeArenaMonster::onRotationStart(%this)
{
	unSubscribeFromEvents(%this, "onUpdateTick32ms");
	return;
}
function BeArenaMonster::onRotationFinish(%this)
{
	if (curState != "stumble")
	{
		subscribeToEvents(%this, "onUpdateTick32ms");
	}
	return;
}
function BeArenaMonster::isKilled(%this)
{
	if (!(achievements.getAchieved("Slaughterer")))
	{
		achievements.setAchieved("Slaughterer");
	}
	return;
}
