// rain.cs.dso
if (!(isObject(BeRain)))
{
	%template = new BehaviorTemplate(Name : BeRain);
	%template.friendlyName = "Rain";
	%template.behaviorType = "Visual";
	%template.description = "turns rain emitter with the camera";
	%template.addBehaviorField(startDelay, "time it takes after switchOn to really start raining (in s)", float, "5");
	%template.addBehaviorField(extinguishDelay, "dealy (after rain starts) until flameables get extinguished (in s) +- random", float, "5");
	%template.addBehaviorField(stopRainDelay, "delay (after extinguish) until the rain stops again (in s); 0 for infinite rain", float, "5");
	%template.addBehaviorField(autoPlay, "description of field", bool, "0");
}
function BeRain::onBehaviorAdd(%this)
{
	%owner = Owner;
	if (%owner.getClassName() != "t2dParticleEffect")
	{
		debugWarn("owner of Rain behavior is not a particelEffect!");
		%owner.removeBehavior(%this);
		return;
	}
	subscribeToEvents(%this, "onLevelLoadFinished");
	return;
}
function BeRain::onLevelLoadFinished(%this)
{
	%owner = Owner;
	%owner.addDependentBehaviors("BeDontCollide");
	if (autoPlay)
	{
		%this.switchOn();
	}
	else
	{
		%owner.stopEffect("0", "0");
	}
	%this.allFlameables = getObjectsWithBehavior("BeFlameable");
	%this.endAreaFlames = "";
	%i = 0;
	while (%i < getWordCount(allFlameables))
	{
		%flameable = getWord(allFlameables, %i).getBehavior("BeFlameable");
		if (growFlamesBeforeRain)
		{
			%this.endAreaFlames = endAreaFlames SPC %flameable;
		}
		%i = %i + 1.0;
	}
	%this.endAreaFlames = trim(endAreaFlames);
	%owner.setLayer($LAYER["rippedEdge"] + 1.0);
	%emitter = %owner.findEmitterObject("UntitledEmitter");
	%emitter.setInverseRotation("1");
	%owner.escapeSwitch = "1";
	%emitter.escapeSwitch = "1";
	return;
}
function BeRain::onBehaviorRemove(%this)
{
	return;
}
function BeRain::switchOn(%this)
{
	%owner = Owner;
	if (startDelay > 0.0)
	{
		%i = 0;
		while (%i < getWordCount(endAreaFlames))
		{
			%flameable = getWord(endAreaFlames, %i);
			%flameable.growFurther();
			%i = %i + 1.0;
		}
		%this.schedule(startDelay * 1000.0, "switchOn");
		%this.startDelay = "0";
		return;
	}
	triggerEvent("onRainStart");
	%width = getWord(viewWindow.getSize(), "0");
	%owner.setSize(%width * 1.5, "2");
	%owner.mount(viewWindow, "0 -1", "0", "1", "1", "1", "0");
	%owner.setEffectLifeMode(INFINITE);
	%owner.playEffect("1");
	%owner.getBehavior("BePlaySound").start();
	if (isObject(endThunder))
	{
		%thunderSound = endThunder.getBehavior("BePlaySound");
		%thunderSound.start();
	}
	%this.extinguishFlameables();
	return;
}
function BeRain::switchOff(%this)
{
	%owner = Owner;
	%owner.selectGraph("quantity_scale");
	%this.origianlQuantity = getY(%owner.getDataKey("0"));
	%this.rainEndSmoother = Smoother::createInstance();
	levelGarbageCollector.add(rainEndSmoother);
	rainEndSmoother.init("30", "1", "0");
	rainEndSmoother.start("0", "0", "1");
	subscribeToEvents(%this, "onUpdateTick10");
	if (isObject(rainSound))
	{
		rainSound.getBehavior("BePlaySound").fadeOut("30");
	}
	return;
}
function BeRain::onUpdateTick10(%this)
{
	%owner = Owner;
	if (!(rainEndSmoother.getIsFinished()))
	{
		%owner.selectGraph("quantity_scale");
		%owner.addDataKey("0", rainEndSmoother.getValue());
	}
	else
	{
		%owner.setEffectLifeMode(stop);
		%owner.stopEffect("1", "0");
		if (isObject(ThunderController))
		{
			ThunderController.getBehavior("BeThunderController").stop();
		}
		unSubscribeFromEvents(%this, "onRotationStart onRotationUpdate onRotationFinish onRotationUpdate onUpdateTick10");
	}
	return;
}
function BeRain::extinguishFlameables(%this)
{
	%i = 0;
	while (%i < getWordCount(allFlameables))
	{
		%flameable = getWord(allFlameables, %i).getBehavior("BeFlameable");
		%variance = floatRandom(-2.0, "2");
		if (breakUp)
		{
			%variance = -3.0;
		}
		%flameable.schedule(extinguishDelay + %variance * 1000.0, "extinguish");
		%i = %i + 1.0;
	}
	return getWordCount(allFlameables);
}
