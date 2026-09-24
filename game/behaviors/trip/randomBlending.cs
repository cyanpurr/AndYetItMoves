// randomBlending.cs.dso
if (!(isObject(BeRandomBlending)))
{
	%template = new BehaviorTemplate(Name : BeRandomBlending);
	%template.friendlyName = "RandomBlending";
	%template.behaviorType = "LevelTrip";
	%template.description = "this object will get setBlending to a random number in the given timeIntervals";
	%template.addBehaviorField(blendDuration, "how long the blending itself iwll take (in s)", float, "0.5");
	%template.addBehaviorField(blendDurationVariation, "how much the blendDuration should vary", float, "0");
	%template.addBehaviorField(pauseBetweenBlends, "how long a color will stay. the pause between belnds (in s)", float, "1");
	%template.addBehaviorField(pauseVariation, "how much the pause should vary", float, "0");
	%template.addBehaviorField(minimumSum, "min. sum of the 3 color values (0.0-2.5)", float, "0.4");
	%template.addBehaviorField(autoPlay, "wheter or not this behavior should start right away", bool, "0");
	%template.addBehaviorField(syncToBeat, "should it happen only on beats?", bool, "0");
}
function BeRandomBlending::onBehaviorAdd(%this)
{
	subscribeToEvent(%this, "onLevelLoadFinished");
	if (syncToBeat)
	{
		subscribeToEvent(%this, "onBeat");
	}
	return;
}
function BeRandomBlending::onLevelLoadFinished(%this)
{
	%this.Smoother = Smoother::createInstance();
	levelGarbageCollector.add(Smoother);
	if (minimumSum > 2.5)
	{
		debugWarn("minimum sum" SPC minimumSum SPC "is too high... setting to 2.5");
		%this.minimumSum = "2.5";
	}
	if (autoPlay)
	{
		%this.start();
	}
	return;
}
function BeRandomBlending::start(%this)
{
	%this.startBlend();
	return;
}
function BeRandomBlending::onBeat(%this)
{
	%this.startBlend();
	return;
}
function BeRandomBlending::startBlend(%this)
{
	%owner = Owner;
	%owner.isBlending = "1";
	if (syncToBeat)
	{
		%duration = $currentBeatLength / 4.0;
	}
	else
	{
		if (blendDurationVariation != 0.0)
		{
		}
		else
		{
		}
		%duration = blendDuration;
	}
	while (%red + %green + %blue < minimumSum)
	{
		%red = getRandom();
		%green = getRandom();
		%blue = getRandom();
	}
	%currentColor = getWord(%owner.getBlendColor(), "0") SPC getWord(%owner.getBlendColor(), "1") SPC getWord(%owner.getBlendColor(), "2");
	%newColor = %red SPC %green SPC %blue;
	Smoother.init(%duration, %currentColor, %newColor);
	Smoother.start();
	subscribeToEvents(%this, "onUpdateTick10");
	return;
}
function BeRandomBlending::onUpdateTick10(%this)
{
	%owner = Owner;
	if (Smoother.getIsFinished())
	{
		%owner.isBlending = "0";
		%this.startPause();
		unSubscribeFromEvents(%this, "onUpdateTick10");
	}
	else
	{
		%owner.setBlendColour(Smoother.getValue());
	}
	return;
}
function BeRandomBlending::startPause(%this)
{
	%owner = Owner;
	if (!(syncToBeat))
	{
		if (pauseVariation)
		{
		}
		else
		{
		}
		%duration = pauseBetweenBlends;
		safeSchedule(%duration * 1000.0, %this, "startBlend");
	}
	return;
}
