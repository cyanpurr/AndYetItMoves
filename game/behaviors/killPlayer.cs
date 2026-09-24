// killPlayer.cs.dso
if (!(isObject(BeKillPlayer)))
{
	%template = new BehaviorTemplate(Name : BeKillPlayer);
	%template.friendlyName = "KillPlayer";
	%template.behaviorType = "CommonObjects";
	%template.description = "kills player when he touches this";
	%template.addBehaviorField(deathType, "which torturous death the player shall die", enum, "dieFragged", "dieFragged	dieExploding	dieBurning");
	%template.addBehaviorField(deathParams, "parameters (as string) if deathtype uses", string, "");
	%template.addBehaviorField(Delay, "galgenfrist in seconds", float, "0");
	%template.addBehaviorField(autoPlay, "wheter or not this behavior should be active form the start", bool, "0");
	%template.addBehaviorField(GraphGroup, "object will receive collision, so which graphgroup should owner have? none doesnt change owners collision settings", enum, "playerTrigger", "None" TAB $GROUPS_ENUM);
	%template.addBehaviorField(collisionDuration, "player needs to collide for some time until he gets killed", float, "0");
}
function BeKillPlayer::onBehaviorAdd(%this)
{
	%owner = Owner;
	if (autoPlay)
	{
		%this.switchOn();
	}
	%this.collisionStartTime = "0";
	return;
}
function BeKillPlayer::onCollisionReceive(%this, %dstObj, , , , , , )
{
	if (!(wantsCollisionReceiveCallback))
	{
		return %this;
	}
	if (camera.getIsRotating())
	{
		return camera.getIsRotating();
	}
	if (!(isPlayer(%dstObj, "1")))
	{
		return isPlayer(%dstObj, "1");
	}
	if (collisionDuration > 0.0)
	{
		if (isObject(continuousCollisionSchedule))
		{
			continuousCollisionSchedule.cancelSchedule();
		}
		%this.continuousCollisionSchedule = safeSchedule(collisionDuration * 1100.0, Owner, "resetCollisionStartTime");
		if (collisionStartTime == 0.0)
		{
			%this.collisionStartTime = thisTime;
			return;
		}
		else
		{
			if (thisTime - collisionStartTime < collisionDuration)
			{
				return $tickStats;
			}
		}
		%this.killHim();
	}
	else
	{
		%this.setBehaviorCollisionReceiveCallback("0");
		if (Delay)
		{
			%this.deathSchedule = safeSchedule(Delay * 1000.0, %this, "killHim");
			subscribeToEvents(%this, "onPlayerDeath");
			break;
		}
		%this.killHim();
	}
	return;
}
function BeKillPlayer::resetCollisionStartTime(%this)
{
	%this.collisionStartTime = "0";
	return;
}
function BeKillPlayer::onPlayerDeath(%this)
{
	%owner = Owner;
	deathSchedule.cancelSchedule();
	%this.setBehaviorCollisionReceiveCallback("1");
	unSubscribeFromEvents(%this, "onPlayerDeath");
	return;
}
function BeKillPlayer::killHim(%this)
{
	%owner = Owner;
	unSubscribeFromEvents(%this, "onPlayerDeath");
	%this.setBehaviorCollisionReceiveCallback("1");
	player.call(deathType, deathParams);
	return;
}
function BeKillPlayer::switchOn(%this)
{
	%owner = Owner;
	if (GraphGroup != "None")
	{
		%owner.setGraphGroup($GROUPS[GraphGroup]);
		%owner.setCollisionPhysics("0", "0");
		%owner.setCollisionActive("0", "1");
	}
	%this.setBehaviorCollisionReceiveCallback("1");
	return;
}
function BeKillPlayer::switchOff(%this)
{
	%owner = Owner;
	return;
}
