// flameControler.cs.dso
if (!(isObject(BeFlameController)))
{
	if (!(isTorquePlayer()))
	{
		%template = new BehaviorTemplate(Name : BeFlameController);
	}
	else
	{
		%template = new BeFlameControllerTemplate(Name : BeFlameController);
	}
	%template.friendlyName = "";
	%template.behaviorType = "";
	%template.description = "";
}
function getFlameController()
{
	if (isObject(flameController))
	{
		return flameController.getBehavior("BeFlameController");
	}
	new t2dSceneObject(Name : flameController)
	{
		scenegraph = scenegraph;
		_behavior0 = "BeFlameController";
	}
	return flameController.getBehavior("BeFlameController");
	return flameController.getBehavior("BeFlameController");
}
function BeFlameController::onBehaviorAdd(%this)
{
	%owner = Owner;
	%koBehavior = %owner.addDependentBehavior("BeKeepOrientation");
	%koBehavior.setOrientationMode("FULL");
	%this.visibleFlames = new SimSet(Name : "");
	levelGarbageCollector.add(visibleFlames);
	subscribeToEvents(%this, "onRotationStart onRotationFinish");
	return;
}
function BeFlameController::onRotationStart(%this)
{
	subscribeToEvents(%this, "onUpdateTick20");
	return;
}
function BeFlameController::onRotationFinish(%this)
{
	%owner = Owner;
	unSubscribeFromEvents(%this, "onUpdateTick20");
	%i = 0;
	while (%i < visibleFlames.getCount())
	{
		%flame = visibleFlames.getObject(%i);
		pivot.setAngularVelocity("0");
		pivot.setRotation(-1.0 * camera.getCurrentRotation());
		%i = %i + 1.0;
	}
	return visibleFlames.getCount();
}
function flameController::deleteInstance(%this)
{
	if (!(isObject(flameController)))
	{
		return isObject(flameController);
	}
	visibleFlames.delete();
	flameController.delete();
	return;
}
