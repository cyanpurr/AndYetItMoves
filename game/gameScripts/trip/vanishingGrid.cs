// vanishingGrid.cs.dso
if (!(isObject(BeVanishingGridPart)))
{
	%template = new BeVanishingGridPartTemplate(Name : BeVanishingGridPart);
}
function BeVanishingGridPart::onBehaviorAdd(%this)
{
	%this.controller = vanishingGridController;
	%this.fadeController = fadeController;
	%this.origPosition = Owner.getPosition();
	subscribeToEvents(%this, "onLevelLoadFinished onUpdateTick32ms");
	if (!(dontVanish))
	{
		subscribeToEvents(%this, "onLevelLoadFinished20");
	}
	%this.toggleSideCounter = "0";
	return;
}
function BeVanishingGridPart::onBehaviorRemove(%this)
{
	if (isObject(vanishingGridController))
	{
		vanishingGridController.delete();
	}
	if (isObject(fadeController))
	{
		fadeController.delete();
	}
	return;
}
function BeVanishingGridPart::onLevelLoadFinished20(%this)
{
	%owner = Owner;
	%this.masks = %owner.getMountedChildren();
	return;
}
function BeVanishingGridPart::switchOn(%this)
{
	subscribeToEvent(%this, "onUpdateTick32ms");
	return;
}
function BeVanishingGridPart::switchOff(%this)
{
	Owner.setLinearVelocity("0 0");
	unSubscribeFromEvent(%this, "onUpdateTick32ms");
	return;
}
function VanishingGridPart::onLevelLoaded(%this, )
{
	%this.addDependentBehavior("BeVanishingGridPart");
	subscribeToEvent(%this, "onLevelLoadFinished4");
	return;
}
function VanishingGridPart::onLevelLoadFinished4(%this)
{
	%translateBehavior = %this.getBehavior("BeTranslate");
	%offset = distanceVector;
	unSubscribeFromEvent(%translateBehavior, "onLevelLoadFinished5");
	%this.removeBehavior(%translateBehavior);
	%this.lethalSpeed = "220";
	if (getX(%offset) == 0.0)
	{
		%this.translateY = "1";
		%this.dir = getSign(getY(%offset));
	}
	else
	{
		%this.translateY = "0";
		%this.dir = getSign(getX(%offset));
	}
	return;
}
function vanishingGridController::onLevelLoaded(%this)
{
	subscribeToEvent(%this, "onFirstKeyPressed");
	return;
}
function vanishingGridController::onFirstKeyPressed(%this)
{
	%this.init();
	fadeController.init();
	return;
}
function vanishingGridController::init(%this)
{
	%this.lastValue = "0";
	%this.Smoother = Smoother::createInstance();
	Smoother.init("2", -25.0, "25", "SMOOTH", "0", "1");
	Smoother.start("0", "0.5");
	if ($WII)
	{
		%this.pauseOnRotation = "1";
	}
	subscribeToEvent(%this, "onUpdateTick32ms10");
	if (pauseOnRotation)
	{
		subscribeToEvents(%this, "onRotationStart onRotationFinish");
	}
	return;
}
function vanishingGridController::ownerSwitchOff(%this)
{
	unSubscribeFromEvent(%this, "onUpdateTick32ms10");
	if (!(pauseOnRotation))
	{
		return %this;
	}
	Smoother.pause();
	unsubscribeToEvents(%this, "onRotationStart onRotationFinish");
	return;
}
function vanishingGridController::ownerSwitchOn(%this)
{
	subscribeToEvent(%this, "onUpdateTick32ms10");
	if (!(pauseOnRotation))
	{
		return %this;
	}
	Smoother.start();
	subscribeToEvents(%this, "onRotationStart onRotationFinish");
	return;
}
function vanishingGridController::onUpdateTick32ms10(%this)
{
	%this.curValue = Smoother.getValue();
	%this.curPercentage = Smoother.getProgressPercentage();
	if (Smoother.getIsReverse())
	{
		%this.curPercentage = 1.0 - curPercentage;
	}
	return;
}
function vanishingGridController::onRotationStart(%this)
{
	Smoother.pause();
	Smoother.pause();
	return;
}
function vanishingGridController::onRotationFinish(%this)
{
	Smoother.start();
	Smoother.start();
	return;
}
function fadeController::init(%this)
{
	%this.lastValue = "0";
	%this.Smoother = Smoother::createInstance();
	levelGarbageCollector.add(Smoother);
	Smoother.init();
	Smoother.timeScale = "0.5";
	if ($WII)
	{
		%this.pauseOnRotation = "1";
	}
	return;
}
function fadeController::onUpdateTick32ms10(%this)
{
	%this.curValue = Smoother.getValue();
	%this.curPercentage = Smoother.getProgressPercentage();
	if (Smoother.getIsFinished())
	{
		unSubscribeFromEvent(%this, "onUpdateTick32ms10");
		%this.isFading = "0";
	}
	return;
}
function fadeController::startFade(%this, %fadeIn)
{
	if (isFading)
	{
		return %this;
	}
	%this.isFading = "1";
	if (%fadeIn)
	{
		Smoother.init("0.2", "0", "1");
	}
	else
	{
		Smoother.init("0.4", "1", "0");
	}
	Smoother.start("0", "0");
	subscribeToEvents(%this, "onUpdateTick32ms10");
	return;
}
