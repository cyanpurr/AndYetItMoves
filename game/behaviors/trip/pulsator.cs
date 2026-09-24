// pulsator.cs.dso
if (!(isObject(BePulsator)))
{
	if (!(isTorquePlayer()))
	{
		%template = new BehaviorTemplate(Name : BePulsator);
	}
	else
	{
		%template = new BePulsatorTemplate(Name : BePulsator);
	}
	%template.friendlyName = "Pulsator";
	%template.behaviorType = "GameplayMechanisms";
	%template.description = "this object will pulsate all objects that added themselves to it, meaning sending a (default) value from 0 to 1 or 1 to 0,";
	%template.addBehaviorField(syncToBeat, "if true timesettings will be ignored and taken from beatGenerator", bool, "0");
	%template.addBehaviorField(startValue, "use to sync many objects. smallest vlaue send by onPulse();", float, "0");
	%template.addBehaviorField(targetValue, "use to sync many objects. biggest value send by onPulse();", float, "1");
	%template.addBehaviorField(growTime, "how long the growing will take (in s)", float, "0.5");
	%template.addBehaviorField(stayBigTime, "how long the objects will stay big", float, "0.2");
	%template.addBehaviorField(shrinkTime, "how long the objects will take to shrink", float, "0.5");
	%template.addBehaviorField(staySmallTime, "how long the objects will stay small", float, "0.2");
	%template.addBehaviorField(keepPulsating, "wheter this object controls the pulsating itself or wait for grow() and shrink() calls", bool, "1");
	%template.addBehaviorField(startWithGrow, "if true it will start with growing", bool, "1");
	%template.addBehaviorField(startInBetween, "between 0 and 1, to start between star and target value", float, "0");
	%template.addBehaviorField(globalControled, "if this pulsator shall be countroled by a pulsator controler", bool, "1");
	%template.addBehaviorField(autoPlay, "wheter or not it start after level loaded", bool, "0");
	%template.addBehaviorField(overRideGlobalTargetValue, "wheter to use owns or global target value if globally controlled", bool, "0");
	%template.addBehaviorField(pauseOnRotate, "", bool, "0");
}
function BePulsator::onBehaviorAdd(%this)
{
	subscribeToEvents(%this, "onLevelLoadFinished7 onLevelLoadFinished40");
	%this.pulsateObjects = new SimSet(Name : "");
	levelGarbageCollector.add(pulsateObjects);
	return;
}
function BePulsator::onLevelLoadFinished7(%this)
{
	%this.Smoother = Smoother::createInstance();
	return;
}
function BePulsator::onLevelLoadFinished40(%this)
{
	if (syncToBeat)
	{
		subscribeToEvents(%this, "onBeatStarted");
		return;
	}
	if (!($WII) && keepPulsating && growTime == shrinkTime && stayBigTime == 0.0 && staySmallTime == 0.0)
	{
		%this.loop4ever = "1";
	}
	if (!($WII))
	{
		%this.pauseOnRotate = "0";
	}
	else
	{
		%this.pauseOnRotate = "1";
		%this.loop4ever = "0";
	}
	if (autoPlay)
	{
		subscribeToEvents(%this, "onFirstKeyPressed");
	}
	return;
}
function BePulsator::onFirstKeyPressed(%this)
{
	%this.play("1");
	return;
}
function BePulsator::onBeatStarted(%this)
{
	%this.play();
	return;
}
function BePulsator::play(%this, %dontPlayWhenSwitchedOff)
{
	if (isPulsating)
	{
		return %this;
	}
	if (syncToBeat)
	{
		%this.growTime = $currentBeatLength / 4.0;
		%this.shrinkTime = growTime;
		%this.stayBigTime = "0";
		%this.staySmallTime = "0";
		%this.startInBetween = growTime / duration;
		%this.loop4ever = "1";
	}
	if (loop4ever)
	{
		if (startWithGrow)
		{
			%this.lastSmootherValue = startValue;
			%this.smootherEndValue = targetValue;
		}
		else
		{
			%this.lastSmootherValue = targetValue;
			%this.smootherEndValue = startValue;
		}
		%this.loop(startInBetween, %dontPlayWhenSwitchedOff);
		%this.isPulsating = "1";
		return;
	}
	if (%dontPlayWhenSwitchedOff && isSwitchedOff)
	{
		return %this;
	}
	%this.isPulsating = "1";
	if (pauseOnRotate)
	{
		subscribeToEvents(%this, "onRotationStart onRotationFinish");
	}
	if (startWithGrow)
	{
		%this.lastSmootherValue = startValue;
		%this.grow(startInBetween);
	}
	else
	{
		%this.lastSmootherValue = targetValue;
		%this.shrink(startInBetween);
	}
	return;
}
function BePulsator::switchOn(%this, %callPlay)
{
	if (%callPlay || autoPlay && !(isPulsating))
	{
		%this.play();
	}
	%this.isSwitchedOff = "0";
	if (isPulsating)
	{
		%this.resume();
		if (pauseOnRotate)
		{
			subscribeToEvents(%this, "onRotationStart onRotationFinish");
		}
	}
	return;
}
function BePulsator::switchOff(%this)
{
	%this.isSwitchedOff = "1";
	if (!(isPulsating))
	{
		return %this;
	}
	%this.pause();
	if (pauseOnRotate)
	{
		unSubscribeFromEvents(%this, "onRotationStart onRotationFinish");
	}
	return;
}
function BePulsator::loop(%this, %inBetween, %dontPlayWhenSwitchedOff)
{
	Smoother.init(growTime, lastSmootherValue, smootherEndValue, "SMOOTH", "0", "1");
	Smoother.start("0", %inBetween);
	if (%dontPlayWhenSwitchedOff && isSwitchedOff)
	{
		return %this;
	}
	subscribeToEvents(%this, "onUpdateTick10");
	return;
}
function BePulsator::grow(%this, %inBetween)
{
	%this.growing = "1";
	%loopSmoother = 0;
	if (keepPulsating && shrinkTime == growTime && stayBigTime == 0.0 && staySmallTime == 0.0)
	{
		%loopSmoother = 1;
	}
	Smoother.init(growTime, lastSmootherValue, targetValue, "SMOOTH", "0", %loopSmoother);
	Smoother.start("0", %inBetween);
	subscribeToEvents(%this, "onUpdateTick10");
	return;
}
function BePulsator::shrink(%this, %inBetween)
{
	%this.growing = "0";
	%loopSmoother = 0;
	if (keepPulsating && shrinkTime == growTime && stayBigTime == 0.0 && staySmallTime == 0.0)
	{
		%loopSmoother = 1;
	}
	Smoother.init(shrinkTime, lastSmootherValue, startValue, "SMOOTH", "0", %loopSmoother);
	Smoother.start("0", %inBetween);
	subscribeToEvents(%this, "onUpdateTick10");
	return;
}
function BePulsator::targetReached(%this)
{
	if (!(keepPulsating))
	{
		unSubscribeFromEvents(%this, "onUpdateTick10");
		%this.resetObjectVelocities();
		return;
	}
	if (growing)
	{
		if (stayBigTime > 0.0)
		{
			unSubscribeFromEvents(%this, "onUpdateTick10");
			safeSchedule(stayBigTime * 1000.0, %this, "shrink");
		}
		else
		{
			%this.shrink();
		}
	}
	else
	{
		if (staySmallTime > 0.0)
		{
			unSubscribeFromEvents(%this, "onUpdateTick10");
			safeSchedule(staySmallTime * 1000.0, %this, "grow");
			break;
		}
		%this.grow();
	}
	return;
}
function BePulsator::resetObjectVelocities(%this)
{
	%i = pulsateObjects.getCount() - 1.0;
	while (%i >= 0.0)
	{
		pulsateObjects.getObject(%i).stopPulsating("1");
		%i = %i - 1.0;
	}
	return;
}
function BePulsator::onRotationStart(%this)
{
	%this.pause();
	return;
}
function BePulsator::onRotationFinish(%this)
{
	%this.resume();
	return;
}
function BePulsator::isMember(%this, %object)
{
	return pulsateObjects.isMember(%object);
	return pulsateObjects.isMember(%object);
}
function BePulsator::AddObject(%this, %object)
{
	pulsateObjects.add(%object);
	%this.objectsCount = pulsateObjects.getCount();
	return;
}
function BePulsator::removeObject(%this, %object)
{
	pulsateObjects.remove(%object);
	%this.objectsCount = pulsateObjects.getCount();
	return;
}
function BePulsator::getCount(%this)
{
	return pulsateObjects.getCount();
	return pulsateObjects.getCount();
}
function BePulsator::getObject(%this, %i)
{
	return pulsateObjects.getObject(%i);
	return pulsateObjects.getObject(%i);
}
function BePulsator::echoInfo(%this)
{
	%useLinkPoints = 0;
	%mounted = 0;
	%i = 0;
	while (%i < objectsCount)
	{
		%pbhv = pulsateObjects.getObject(%i);
		if (useLinkPoint > 0.0)
		{
			%useLinkPoints = %useLinkPoints + 1.0;
			if (objectIsMounted)
			{
				%mounted = %mounted + 1.0;
			}
		}
		%i = %i + 1.0;
	}
	debugEcho("#objects:" SPC pulsateObjects.getCount() SPC ", #linkPoint user:" SPC %useLinkPoints SPC ", #mounted:" SPC %mounted SPC ", isPlaying:" SPC isPulsating);
	return;
}
