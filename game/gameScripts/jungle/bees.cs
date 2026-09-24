// bees.cs.dso
function bees::onLevelLoaded(%this)
{
	subscribeToEvents(%this, "onLevelShutdown onLevelLoadFinished1");
	return;
}
function bees::onLevelLoadFinished1(%this)
{
	%soundTrigger = new t2dTrigger(Name : "")
	{
		size = "200 200";
		scenegraph = scenegraph;
		_behavior0 = "BePlaySound	profileName	beesLoop	maxVolume	0.8	fullVolumeFactor	0.1";
	}
	%soundTrigger.mount(%this, "0 0", "0", "1", "1", "1", "0");
	return;
}
function bees::onLevelShutdown(%this)
{
	%this.stopEffect("0", "1");
	return;
}
function bees::attack(%this, %force)
{
	subscribeToEvents(%this, "onPlayerDeath onPlayerReanimate");
	if (%this.getIsMounted())
	{
		if (isPlayer(%this.getMountedParent()))
		{
			%this.setMountForce(%force);
			return;
		}
	}
	%this.attackForce = %force;
	%this.mount(player, "0 0", %force, "0", "0", "0", "0");
	return;
}
function bees::onPlayerDeath(%this)
{
	%nest = "beesNest" @ number;
	debugEcho("resetting bees:" SPC %nest);
	if (isObject(%nest))
	{
		%this.nest = %nest;
		%this.mount(%nest, "0 0", "3", "0", "0", "0", "0");
	}
	return;
}
function bees::onPlayerReanimate(%this)
{
	if (isObject(nest))
	{
		%this.mount(player, "0 0", attackForce, "0", "0", "0", "0");
	}
	return;
}
function bees::leave(%this, %home, %force)
{
	unSubscribeFromEvents(%this, "onPlayerDeath onPlayerReanimate");
	if (isBurning)
	{
		return %home;
	}
	if (!(isObject(%home)))
	{
		%this.dismount();
	}
	else
	{
		if (%this.getIsMounted())
		{
			if (%this.getMountedParent().getId() == %home.getId())
			{
				return %this.getMountedParent().getId();
			}
		}
		%this.mount(%home, "0 0", %force, "0", "0", "0", "0");
	}
	return;
}
function beeKillTrigger::onLevelLoaded(%this)
{
	if (killTime $= "")
	{
		%this.killTime = "1";
	}
	%this.killTime = killTime * 1000.0;
	return %this;
}
function beeKillTrigger::onEnter(%this, %obj)
{
	%this.killSchedule = %this.schedule(killTime, "killPlayer");
	return;
}
function beeKillTrigger::onLeave(%this, %obj)
{
	cancel(killSchedule);
	return;
}
function beeKillTrigger::KillPlayer(%this)
{
	player.lastDeathEvent = "bees";
	player.dieExploding();
	return;
}
function beeAttackTrigger::onLevelLoaded(%this)
{
	if (colMode != "")
	{
		%this.getBehavior("BeTrigger").setTriggerCollisionDetection(colMode);
	}
	return;
}
function beeAttackTrigger::onEnter(%this, %obj)
{
	if (banishBees)
	{
		%this.onLeave(%obj, "1");
		return;
	}
	if (force $= "")
	{
		%force = 2.5;
	}
	else
	{
		%force = force;
	}
	bees.attack(%force);
	return;
}
function beeAttackTrigger::onLeave(%this, %obj, %forceLeave)
{
	if (!(%forceLeave) && attractBees || banishBees)
	{
		return %this;
	}
	if (force $= "")
	{
		%force = 1.0;
	}
	else
	{
		%force = force;
	}
	bees.leave(beeNest, %force);
	return;
}
function beeHive::disableHive(%this)
{
	beeKillTrigger.setCollisionSuppress("1");
	beeAttackTrigger.setCollisionSuppress("1");
	%this.isBurning = "1";
	bees.leave(beeHive2, "0.05");
	return;
}
