// selfDiscoverySwitch.cs.dso
if (!(isObject(BeSelfDiscoverySwitch)))
{
	%template = new BehaviorTemplate(Name : BeSelfDiscoverySwitch);
	%template.friendlyName = "SelfDiscoverySwitch";
	%template.behaviorType = "LevelTrip";
	%template.description = "the 'doors' for the self discovery parts";
	%template.addBehaviorField(enteringSelfDiscovery, "if the self discovery is entered at this trigger", bool, "1");
	%template.addBehaviorField(number, "the number of the self discovery", integer, "0");
	%template.addBehaviorField(directionToGo, "use to overwrite spawnpoint pointing direction: which direction the player should go to reach the goal from here", enum, "useOriginal", "useOriginal	right	left	up	down");
}
function BeSelfDiscoverySwitch::onBehaviorAdd(%this)
{
	subscribeToEvent(%this, "onLevelLoadFinished1");
	return;
}
function BeSelfDiscoverySwitch::onLevelLoadFinished1(%this)
{
	%owner = Owner;
	%owner.activeGlow = new t2dStaticSprite(Name : "")
	{
		scenegraph = scenegraph;
		imageMap = "spawnPointGlowImageMap";
		size = "22.000 44.000";
	}
	activeGlow.setLayer($LAYER["mainBehindPlayer"] + 1.0);
	activeGlow.mount(%owner, "0 0", "0", "1", "1", "0", "0");
	activeGlow.setVisible("0");
	return;
}
function BeSelfDiscoverySwitch::onEnter(%this)
{
	if (hasEntered || !(enteringSelfDiscovery))
	{
		return %this;
	}
	if (isFalling)
	{
		subscribeToEvents(%this, "onPlayerLanded onPlayerDeath");
	}
	else
	{
		%this.hasEntered();
	}
	return;
}
function BeSelfDiscoverySwitch::onPlayerLanded(%this)
{
	unSubscribeFromEvents(%this, "onPlayerLanded onPlayerDeath");
	%this.hasEntered();
	return;
}
function BeSelfDiscoverySwitch::hasEntered(%this)
{
	%this.hasEntered = "1";
	playEventSound(TripSelfdiscovery2, "0.8");
	$PRE_SELFDISCOVERY[number].fadeOut("1");
	safeSchedule("500", $SELFDISCOVERY[number], "fadeIn");
	return;
}
function BeSelfDiscoverySwitch::onPlayerDeath(%this)
{
	unSubscribeFromEvents(%this, "onPlayerLanded onPlayerDeath");
	return;
}
function BeSelfDiscoverySwitch::leave(%this)
{
	%owner = Owner;
	if (hasLeft)
	{
		return %this;
	}
	%this.hasLeft = "1";
	playEventSound(TripSelfdiscovery2, "0.8");
	$SELFDISCOVERY[number].fadeOut("1", "1");
	$PRE_SELFDISCOVERY[number].fadeOut("2", "1");
	safeSchedule("500", $POST_SELFDISCOVERY[number], "fadeIn");
	%owner.safeDelete();
	return;
}
