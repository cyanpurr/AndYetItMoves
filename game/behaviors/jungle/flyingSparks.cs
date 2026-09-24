// flyingSparks.cs.dso
if (!(isObject(BeFlyingSparks)))
{
	%template = new BehaviorTemplate(Name : BeFlyingSparks);
	%template.friendlyName = "FlyingSparks";
	%template.behaviorType = "LevelJungle";
	%template.description = "flying burning pieces that will fly up and die after some time. kill player on colission ";
	%template.addBehaviorField(lifeLength, "how long this spark should live (in s)", float, "1");
	%template.addBehaviorField(autoPlay, "wheter or not this spark should fly right after elvel loaded (will turn on autoplay for quirky movement and kill player as well)", bool, "0");
}
function BeFlyingSparks::onBehaviorAdd(%this)
{
	subscribeToEvents(%this, "onLevelLoadFinished7");
	Owner.addDependentBehaviors("BeQuirkyMovement BeMask BeKillPlayer");
	return;
}
function BeFlyingSparks::onLevelLoadFinished7(%this)
{
	%this.init();
	if (autoPlay)
	{
		%this.switchOn();
	}
	return;
}
function BeFlyingSparks::init(%this)
{
	%owner = Owner;
	if (autoPlay)
	{
		%behavior = %owner.getBehavior("BeQuirkyMovement");
		%behavior.autoPlay = "1";
	}
	return;
}
function BeFlyingSparks::switchOn(%this)
{
	%owner = Owner;
	subscribeToEvents(%this, "onRotationFinish onRotationStart");
	%this.shrinkVel = t2dVectorScale(%owner.getSize(), -1.0 / lifeLength);
	%owner.setSizeVelocity(shrinkVel);
	safeSchedule(lifeLength * 900.0, %this, "vanish");
	return;
}
function BeFlyingSparks::vanish(%this)
{
	%owner = Owner;
	%owner.safeDelete();
	Source.objectsAlive = objectsAlive - 1.0;
	return Source;
}
function BeFlyingSparks::onSpawnFinished(%this)
{
	%owner = Owner;
	%this.Source = %source;
	%killplayerBehavior = %owner.getBehavior("BeKillPlayer");
	%killplayerBehavior.deathType = "dieBurning";
	%this.init();
	playDistanceEventSound(Owner, FlyingSpark, "0.5");
	return;
}
function BeFlyingSparks::onRotationStart(%this)
{
	Owner.setSizeVelocity("0", "0");
	return;
}
function BeFlyingSparks::onRotationFinish(%this)
{
	Owner.setSizeVelocity(shrinkVel);
	return;
}
