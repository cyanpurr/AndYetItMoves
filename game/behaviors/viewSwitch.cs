// viewSwitch.cs.dso
if (!(isObject(BeViewSwitch)))
{
	%template = new BehaviorTemplate(Name : BeViewSwitch);
	%template.friendlyName = "ViewSwitch";
	%template.behaviorType = "GameplayMechanisms";
	%template.description = "the circle around the owner relative to the current view window size - onEnter(camera) call %owner.switchOn(); onLeave(camera) call %owner.switchOff()";
	%template.addBehaviorField(factor, "the factor relative to current view window size - determines the size of the circle", float, "1");
	%template.addBehaviorField(BehaviorList, "a list of behaviors on which the switchOn/Off function get called - leave empty to call all behaviors, put 0 to call no Behaviors", string, "");
}
function BeViewSwitch::onBehaviorAdd(%this)
{
	subscribeToEvent(%this, "onLevelLoadFinished10");
	return;
}
function BeViewSwitch::onLevelLoadFinished10(%this)
{
	%this.init();
	return;
}
function BeViewSwitch::init(%this)
{
	%owner = Owner;
	%this.maxViewSizeTrigger = new t2dTrigger(Name : "")
	{
		class = "ViewSwitchTrigger";
		scenegraph = daSceneGraph;
		factor = factor;
		Owner = %owner;
		BehaviorList = BehaviorList;
		_behavior0 = "BeTrigger";
	}
	%this.getMinZoom();
	%maxViewWindow = t2dVectorScale(sceneWindow2d.getCurrentCameraSize(), factor / levelsMinZoom);
	maxViewSizeTrigger.setSize(%maxViewWindow);
	maxViewSizeTrigger.setStayCallback("1");
	maxViewSizeTrigger.mount(Owner, "0 0", "0", "0", "1", "1", "1");
	return;
}
function BeViewSwitch::setFactor(%this, %factor)
{
	%this.factor = %factor;
	if (isObject(maxViewSizeTrigger))
	{
		maxViewSizeTrigger.factor = %factor;
	}
	else
	{
		%this.init();
	}
	return;
}
function BeViewSwitch::getMinZoom(%this)
{
	%this.levelsMinZoom = maxZoom;
	%i = 0;
	while (%i < zoomAdjusterGroup.getCount())
	{
		if (minZoom < levelsMinZoom)
		{
			%this.levelsMinZoom = minZoom;
		}
		%i = %i + 1.0;
	}
	if (levelsMinZoom == maxZoom)
	{
	}
	else
	{
	}
	%this.levelsMinZoom = levelsMinZoom;
	return;
}
function ViewSwitchTrigger::onEnter(%this)
{
	%this.updateExtents();
	subscribeToEvent(%this, "onZoom");
	return;
}
function ViewSwitchTrigger::onStay(%this, )
{
	if (camera.getIsRotating())
	{
		return camera.getIsRotating();
	}
	if (camera.getCurrentRotation() % 180)
	{
		%this.extents = getWord(extents, "1") SPC getWord(extents, "0");
	}
	%topLeftCorner = t2dVectorSub(%this.getPosition(), t2dVectorScale(extents, "0.5"));
	if (mPointInRect(%topLeftCorner, extents, camera.getPosition()))
	{
		if (!(isSwitchedOn))
		{
			%this.isSwitchedOn = "1";
			%this.switchAll("1");
		}
	}
	else
	{
		if (isSwitchedOn)
		{
			%this.isSwitchedOn = "0";
			%this.switchAll("0");
		}
	}
	return;
}
function ViewSwitchTrigger::onLeave(%this)
{
	unSubscribeFromEvent(%this, "onZoom");
	return;
}
function ViewSwitchTrigger::onZoom(%this)
{
	%this.updateExtents();
	return;
}
function ViewSwitchTrigger::updateExtents(%this)
{
	%this.extents = t2dVectorScale(sceneWindow2d.getCurrentCameraSize(), 1.0 / sceneWindow2d.getCurrentCameraZoom());
	%this.extents = t2dVectorScale(extents, factor);
	return;
}
function ViewSwitchTrigger::switchAll(%this, %on)
{
	%owner = Owner;
	if (BehaviorList $= "")
	{
	}
	else
	{
	}
	%behaviorsToCall = BehaviorList;
	if (%behaviorsToCall != 0.0)
	{
		%i = 0;
		while (%i < getWordCount(%behaviorsToCall))
		{
			%behavior = %owner.getBehavior(getWord(%behaviorsToCall, %i));
			if (%on)
			{
				%behavior.switchOn();
			}
			else
			{
				%behavior.switchOff();
			}
			%i = %i + 1.0;
		}
	}
	if (%on)
	{
		%owner.inPlayerView = "1";
	}
	else
	{
		%owner.inPlayerView = "0";
	}
	return;
}
