// spidernet.cs.dso
if (!(isObject(BeSpidernet)))
{
	if (!(isTorquePlayer()))
	{
		%template = new BehaviorTemplate(Name : BeSpidernet);
	}
	else
	{
		%template = new BeSpidernetTemplate(Name : BeSpidernet);
	}
	%template.friendlyName = "Spidernet";
	%template.behaviorType = "LevelCave3";
	%template.description = "give it to the spidernet";
	if (!(isTorquePlayer()))
	{
		%template.addBehaviorField(level, "level of this piece of the net", int, "1");
		%template.addBehaviorField(left, "if this is at the left or right netside", bool, "1");
	}
}
function BeSpidernet::onBehaviorAdd(%this)
{
	subscribeToEvents(%this, "onLevelLoadFinished10 onLevelShutdown");
	return;
}
function BeSpidernet::onLevelShutdown(%this)
{
	if (isObject(weighterGroup))
	{
		weighterGroup.delete();
	}
	if (isObject(spidernetController))
	{
		spidernetController.delete();
	}
	if (isObject(spidernetDekoGroup))
	{
		spidernetDekoGroup.delete();
	}
	return;
}
function BeSpidernet::onLevelLoadFinished10(%this)
{
	if (!(isObject(spidernetController)))
	{
		new SimObject(Name : spidernetController);
		levelGarbageCollector.add(spidernetController);
		spidernetController.lastWeight = -1000.0;
		spidernetController.weighterGroup = new SimSet(Name : spidernetWeighterGroup);
		levelGarbageCollector.add(weighterGroup);
		weighterGroup.hasChanged = "1";
		weighterGroup.lastClearTime = -1000.0;
		spidernetController.spidernetPartGroup = new SimSet(Name : "");
		levelGarbageCollector.add(spidernetPartGroup);
		%weighterBehavior = player.addDependentBehavior("BeWeighter");
		%weighterBehavior.targetGroups = spidernetWeighterGroup;
		subscribeToEvent(%this, "onLevelLoadFinished20");
		%this.isMainPart = "1";
	}
	Owner.controller = spidernetController;
	spidernetPartGroup.add(%this);
	return;
}
function BeSpidernet::onLevelLoadFinished20(%this)
{
	%controller = controller;
	%maxWeight = 0;
	%count = weighterGroup.getCount();
	%i = 0;
	while (%i < %count)
	{
		%maxWeight = %maxWeight + weighterGroup.getObject(%i).getMass();
		%i = %i + 1.0;
	}
	weighterGroup.clear();
	%avgWeight = %maxWeight / %count;
	%controller.playerSpecialMass = %avgWeight / 2.0;
	%controller.maxWeight = %maxWeight + playerSpecialMass / 2.0;
	%controller.maxWeight = maxWeight - 4.0 / 3.0 * %avgWeight;
	%controller.maxRotation = "35";
	%controller.clearInterval = "0.5";
	%controller.waitAfterClearing = clearInterval / 10.0;
	%controller.rotationFactor = maxRotation / maxWeight;
	%this.onUpdateTick32ms();
	Owner.setTimerOn(clearInterval * 1000.0);
	return;
}
function BeSpidernet::ripIt(%this)
{
	%controller = controller;
	%i = 0;
	while (%i < spidernetPartGroup.getCount())
	{
		%netPart = spidernetPartGroup.getObject(%i);
		Owner.setEnabled("0");
		unSubscribeFromEvents(%netPart, "onRotationFinish");
		%i = %i + 1.0;
	}
	%i = 0;
	while (%i < spidernetDekoGroup.getCount())
	{
		%netPart = spidernetDekoGroup.getObject(%i);
		%netPart.rip();
		%i = %i + 1.0;
	}
	%this.isRipped = "1";
	%this.onTimer();
	%this.switchOff();
	playEventSound(SpidernetRip, "0.7");
	return;
}
function BeSpidernet::onTimer(%this)
{
	%weighterGroup = weighterGroup;
	%i = 0;
	while (%i < %weighterGroup.getCount())
	{
		%weighter = %weighterGroup.getObject(%i);
		%weighter.currentGroup = "0";
		%weighter.getBehavior("BeWeighter").setBehaviorCollisionReceiveCallback("0");
		%i = %i + 1.0;
	}
	%weighterGroup.clear();
	%weighterGroup.lastClearTime = thisTime;
	%weighterGroup.hasChanged = "1";
	return;
}
function BeSpidernet::goToAngle(%this, %angle)
{
	%owner = Owner;
	%rotateBehavior = %owner.getBehavior("BeRotate");
	%rotateBehavior.setRotateAngle(t2dShortestAngleDifference(%owner.getRotation(), %angle));
	%rotateBehavior.initSmoother();
	%rotateBehavior.play("0", reachTargetFactor / 4.0);
	return;
}
function BeSpidernet::onBehaviorRemove(%this)
{
	if (isObject(controller))
	{
		if (isObject(weighterGroup))
		{
			weighterGroup.safeDelete();
		}
		controller.safeDelete();
	}
	return;
}
function BeSpidernet::switchOn(%this)
{
	if (isRipped)
	{
		return %this;
	}
	subscribeToEvents(%this, "onRotationFinish");
	if (!(equalsAngleTolerance(camera.getCurrentRotation(), "0", "45")))
	{
		return equalsAngleTolerance(camera.getCurrentRotation(), "0", "45");
	}
	%this.setBehaviorCollisionReceiveCallback("1");
	if (isMainPart)
	{
		subscribeToEvents(%this, "onUpdateTick32ms");
		Owner.setTimerOn(clearInterval * 1000.0);
		player.getBehavior("BeWeighter").switchOn();
	}
	return;
}
function BeSpidernet::switchOff(%this)
{
	%this.setBehaviorCollisionReceiveCallback("0");
	unSubscribeFromEvents(%this, "onRotationFinish");
	if (isMainPart)
	{
		unSubscribeFromEvents(%this, "onUpdateTick32ms");
		Owner.setTimerOff();
		player.getBehavior("BeWeighter").switchOff();
	}
	return;
}
function BeSpidernet::onRotationFinish(%this)
{
	if (equalsAngleTolerance(camera.getCurrentRotation(), "0", "45"))
	{
		%this.switchOn();
	}
	else
	{
		%this.setBehaviorCollisionReceiveCallback("0");
		if (isMainPart)
		{
			Owner.onTimer();
			weighterGroup.lastClearTime = -1000.0;
			%this.onUpdateTick32ms();
			unSubscribeFromEvents(%this, "onUpdateTick32ms");
			Owner.setTimerOff();
		}
	}
	return;
}
function SpidernetDeko::onLevelLoaded(%this)
{
	subscribeToEvent(%this, "onLevelLoadFinished2");
	return;
}
function SpidernetDeko::onLevelLoadFinished2(%this)
{
	if (!(isObject(spidernetDekoGroup)))
	{
		new SimSet(Name : spidernetDekoGroup);
		levelGarbageCollector.add(spidernetDekoGroup);
	}
	spidernetDekoGroup.add(%this);
	if (pendingNetPart)
	{
		%this.spidernetDekoBehavior = %this.addDependentBehavior("BeSpidernetDeko");
		%this.rotateBehavior = %this.addDependentBehavior("BeRotate");
		rotateBehavior.autoPlay = "0";
		rotateBehavior.use32msTick = "1";
	}
	return;
}
function SpidernetDeko::rip(%this)
{
	if (!(pendingNetPart))
	{
		%this.setEnabled("0");
		return;
	}
	subscribeToEvents(spidernetDekoBehavior, "onRotationStart onRotationAbort");
	if (%this.getId() == rippedNetRight.getId())
	{
		%angle = -90.0;
	}
	else
	{
		%angle = 90;
	}
	rotateBehavior.setupRotate(%angle, timeToRotate * 2.0, "SINC", "3 2 1", "1", "0", "0", "1");
	return;
}
function BeSpidernetDeko::onRotationAbort(%this)
{
	%rotateBehavior = rotateBehavior;
	Smoother.start(getWord(abortionParameters, "0"), getWord(abortionParameters, "1"));
	return;
}
function BeSpidernetDeko::onRotationStart(%this)
{
	%rotateBehavior = rotateBehavior;
	%angle = t2dShortestAngleDifference(%rotateBehavior.getPivotRotation(), normaliseAngle(targetRotation + 90.0));
	if (modulo(targetRotation, "180") != 0.0)
	{
		%angle = %angle * -1.0;
	}
	%rotateBehavior.setupRotate(%angle, timeToRotate * 2.0, "SINC", "3 2 1", "1", "0", "0", "1");
	return;
}
