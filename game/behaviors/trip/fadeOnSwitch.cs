// fadeOnSwitch.cs.dso
if (!(isObject(BeFadeOnSwitch)))
{
	%template = new BehaviorTemplate(Name : BeFadeOnSwitch);
	%template.friendlyName = "Fade On Switch";
	%template.behaviorType = "LevelTrip";
	%template.description = "this object will fade in when switchOn is called. if it has becollide it will be set to collisionsuppress to false, a bemask will fade visually.";
	%template.addBehaviorField(startFadeState, "switch on(1) / off(0) at level start, or keep unchanged(-1)", int, -1.0);
	%template.addBehaviorField(fadeTime, "the time for fading in/out in sec", int, "1");
}
function BeFadeOnSwitch::onBehaviorAdd(%this)
{
	subscribeToEvents(%this, "onLevelLoadFinished30");
	return;
}
function BeFadeOnSwitch::onLevelLoadFinished30(%this)
{
	%this.fadeVelocity = 1.0 / fadeTime;
	%this.fadeTimeMS = fadeTime * 1000.0 + 10.0;
	if (startFadeState == 0.0)
	{
		%this.switchOff();
	}
	else
	{
		if (startFadeState == 1.0)
		{
			%this.switchOn();
		}
	}
	return;
}
function BeFadeOnSwitch::switchOff(%this)
{
	%owner = Owner;
	if (%owner.getBehavior("BeCollide"))
	{
		%owner.setCollisionSuppress("1");
	}
	if (%owner.getBehavior("BeMask"))
	{
		%owner.setBlendAlpha("1");
		%owner.setAlphaVelocity(-1.0 * fadeVelocity);
		%this.schedule(fadeTimeMS, "stopFadeOut");
	}
	return;
}
function BeFadeOnSwitch::switchOn(%this)
{
	%owner = Owner;
	if (%owner.getBehavior("BeCollide"))
	{
		%owner.setCollisionSuppress("0");
	}
	if (%owner.getBehavior("BeMask"))
	{
		%owner.setVisible("1");
		%owner.setBlendAlpha("0");
		%owner.setAlphaVelocity(fadeVelocity);
		%this.schedule(fadeTimeMS, "stopFadeIn");
	}
	return;
}
function BeFadeOnSwitch::stopFadeOut(%this)
{
	Owner.setAlphaVelocity("0");
	Owner.setVisible("0");
	return;
}
function BeFadeOnSwitch::stopFadeIn(%this)
{
	Owner.setBlendAlpha("1");
	Owner.setAlphaVelocity("0");
	return;
}
