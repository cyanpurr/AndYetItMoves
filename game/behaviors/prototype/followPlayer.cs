// followPlayer.cs.dso
if (!(isObject(BeFollowPlayer)))
{
	%template = new BehaviorTemplate(Name : BeFollowPlayer);
	%template.friendlyName = "followPlayer";
	%template.behaviorType = "Positioning";
	%template.description = "uthis object will constantly move towards the player, except when the player is dead then it will move away from him";
	%template.addBehaviorField(accelFactor, "how fast the object should accelerate", float, "30");
	%template.addBehaviorField(maxVelocity, "how fast the obejct can go maximum", float, "100");
	%template.addBehaviorField(autoPlay, "if movement towards player starts right away", bool, "1");
	%template.addBehaviorField(followVertical, "if object also follows player in vertical direction", bool, "0");
}
function BeFollowPlayer::onBehaviorAdd(%this)
{
	%owner = Owner;
	%this.moving = %owner.addDependentBehavior("BeMoving");
	moving.Friction = "0";
	subscribeToEvent(%this, "onLevelLoadFinished10");
	return;
}
function BeFollowPlayer::onLevelLoadFinished10(%this)
{
	%owner = Owner;
	if (autoPlay)
	{
		%this.play();
	}
	unSubscribeFromEvent(%this, "onLevelLoadFinished10");
	return;
}
function BeFollowPlayer::play(%this)
{
	subscribeToEvents(%this, "onUpdateTick32ms onRotationStart onRotationFinish");
	return;
}
function BeFollowPlayer::pause(%this)
{
	debugEcho("followplayer pause" SPC %this);
	unSubscribeFromEvents(%this, "onUpdateTick32ms onRotationStart onRotationFinish");
	return;
}
function BeFollowPlayer::onRotationStart(%this)
{
	unSubscribeFromEvents(%this, "onUpdateTick32ms");
	return;
}
function BeFollowPlayer::onRotationFinish(%this)
{
	subscribeToEvents(%this, "onUpdateTick32ms");
	%this.getDirection();
	return;
}
function BeFollowPlayer::onUpdateTick32ms(%this)
{
	%owner = Owner;
	%monsterBehavior = %owner.getBehavior("BeArenaMonster");
	if (%monsterBehavior && isFalling)
	{
		return %monsterBehavior;
	}
	%curVel = %owner.getLinearVelocity();
	%this.getDirection();
	%addVel = t2dVectorScale(direction, accelFactor * duration32ms);
	%nextVel = t2dVectorAdd(%curVel, %addVel);
	if (t2dVectorLength(%nextVel) < maxVelocity)
	{
		%owner.setLinearVelocity(%nextVel);
	}
	return;
}
function BeFollowPlayer::getDirection(%this)
{
	%owner = Owner;
	if (!(keepOldDirection))
	{
		if (isDead)
		{
			%vector = t2dVectorSub(%owner.getPosition(), spawnPoint.getPosition());
		}
		else
		{
			%vector = t2dVectorSub(player.getPosition(), %owner.getPosition());
		}
		if (!(followVertical))
		{
			%vector = setVerticalComponent(%vector, "0");
		}
		%this.direction = t2dVectorNormalise(%vector);
	}
	return;
}
