// zoomFader.cs.dso
if (!(isObject(BeZoomFader)))
{
	if (!(isTorquePlayer()))
	{
		%template = new BehaviorTemplate(Name : BeZoomFader);
	}
	else
	{
		%template = new BeZoomFaderTemplate(Name : BeZoomFader);
	}
	%template.friendlyName = " Zoom Fader";
	%template.behaviorType = "GameplayMechanisms";
	%template.description = "Fades the camera dynamic larger or smaller when the player crosses the area";
	if (!(isTorquePlayer()))
	{
		%template.addBehaviorField(vertical, "if the camerazoom should be applied on vertical or horizontal traversal", bool, "0");
		%template.addBehaviorField(leftTopZoom, "value at the absolute left or top side of the trigger", float, "1");
		%template.addBehaviorField(rightBottomZoom, "value at the absolute right or bottom side of the trigger", float, "0.5");
	}
}
function BeZoomFader::onBehaviorAdd(%this)
{
	%owner = Owner;
	if (%owner.getClassName() != "t2dTrigger")
	{
		debugWarn("owner of camera scaling behavior is not a trigger!");
		%owner.removeBehavior(%this);
		return;
	}
	%triggerBehavior = %owner.addDependentBehavior("BeTrigger");
	subscribeToEvents(%this, "onLevelLoadFinished5 onLevelLoadFinished10");
	return;
}
function BeZoomFader::onLevelLoadFinished5(%this)
{
	%owner = Owner;
	if (%owner.getRotation() != 0.0)
	{
		debugWarn("attention: zoomfader" SPC %owner SPC "is rotated! zoom faders only support rotation 0 - use flag vertical!");
	}
	%this.Smoother = Smoother::createInstance();
	Smoother.init("1", leftTopZoom, rightBottomZoom);
	if (rightBottomZoom > leftTopZoom)
	{
	}
	else
	{
	}
	%this.maxZoom = leftTopZoom;
	if (rightBottomZoom < leftTopZoom)
	{
	}
	else
	{
	}
	%this.minZoom = leftTopZoom;
	zoomAdjusterGroup.add(%this);
	return;
}
function BeZoomFader::onLevelLoadFinished10(%this)
{
	%owner = Owner;
	%triggerBehavior = %owner.getBehavior("BeTrigger");
	%triggerBehavior.setTriggerCollisionGroup("zoomTrigger");
	%owner.setEnterCallback("1");
	%owner.setStayCallback("0");
	%owner.setLeaveCallback("1");
	return;
}
function BeZoomFader::onEnter(%this, %object)
{
	if ($disableZoomTrigger)
	{
		return;
	}
	if (%object != camera.getId() || isDead)
	{
		return player;
	}
	subscribeToEvents(%this, "onUpdateFrame10 onPlayerDeath");
	return;
}
function BeZoomFader::onLeave(%this, %object)
{
	if ($disableZoomTrigger)
	{
		return;
	}
	unSubscribeFromEvents(%this, "onUpdateFrame10 onPlayerDeath");
	if (%object != camera.getId() || isDead)
	{
		return player;
	}
	%this.onUpdateFrame10();
	triggerEvent("onZoomTriggerLeave");
	return;
}
function BeZoomFader::onPlayerDeath(%this)
{
	unSubscribeFromEvents(%this, "onUpdateFrame10 onPlayerDeath");
	return;
}
function BeZoomFader::switchOff(%this)
{
	Owner.setEnabled("0");
	return;
}
function BeZoomFader::switchOn(%this)
{
	Owner.setEnabled("1");
	return;
}
