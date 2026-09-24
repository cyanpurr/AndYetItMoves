// keepOrientation.cs.dso
if (!(isObject(BeKeepOrientation)))
{
	if (!(isTorquePlayer()))
	{
		new BehaviorTemplate(Name : BeKeepOrientation);
		break;
	}
	new BeKeepOrientationTemplate(Name : BeKeepOrientation);
}
function BeKeepOrientation::onBehaviorAdd(%this)
{
	%this.origRotation = "0";
	%this.useFrameUpdate = "0";
	%this.orientationMode = "NO";
	subscribeToEvent(%this, "onLevelLoadFinished");
	return;
}
function BeKeepOrientation::onLevelLoadFinished(%this)
{
	%this.setOrientationMode(orientationMode);
	return;
}
function BeKeepOrientation::onRotationStart(%this)
{
	subscribeToEvents(%this, "onUpdateTick10");
	return;
}
function BeKeepOrientation::setOrientationMode(%this, %mode)
{
	%this.orientationMode = %mode;
	if (!(useFrameUpdate))
	{
		if (%mode $= "FULL")
		{
			subscribeToEvents(%this, "onRotationFinish onRotationStart");
		}
		else
		{
			if (%mode $= "STEP")
			{
				subscribeToEvent(%this, "onRotationFinish");
				unSubscribeFromEvent(%this, "onRotationStart");
				break;
			}
			if (%mode $= "NO")
			{
				unSubscribeFromEvents(%this, "onRotationFinish onRotationStart");
			}
		}
	}
	else
	{
		if (%mode $= "FULL")
		{
			subscribeToEvents(%this, "onRotationFinish onRotationUpdate");
			break;
		}
		if (%mode $= "STEP")
		{
			subscribeToEvent(%this, "onRotationFinish");
			unSubscribeFromEvent(%this, "onRotationUpdate");
			break;
		}
		if (%mode $= "NO")
		{
			unSubscribeFromEvents(%this, "onRotationFinish onRotationUpdate");
		}
	}
	return;
}
