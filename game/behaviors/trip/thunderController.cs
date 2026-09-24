// thunderController.cs.dso
if (!(isObject(BeThunderController)))
{
	%template = new BehaviorTemplate(Name : BeThunderController);
	%template.friendlyName = "ThunderController";
	%template.behaviorType = "LevelTrip";
	%template.description = "controlls the texture toggles, that visualize thunder";
	%template.addBehaviorField(autoPlay, "start at level begin", bool, "0");
}
function BeThunderController::onBehaviorAdd(%this)
{
	subscribeToEvents(%this, "onLevelLoadFinished");
	%this.thunderTextures = new SimSet(Name : "");
	levelGarbageCollector.add(thunderTextures);
	return;
}
function BeThunderController::onLevelLoadFinished(%this)
{
	if (autoPlay)
	{
		%this.play();
	}
	return;
}
function BeThunderController::switchOn(%this)
{
	%this.play();
	return;
}
function BeThunderController::play(%this)
{
	%i = 0;
	while (%i < thunderTextures.getCount())
	{
		%obj = thunderTextures.getObject(%i);
		%toggleTexBehavior = %obj.getBehavior("BeTextureToggle");
		if (!(isObject(%toggleTexBehavior)))
		{
			return isObject(%toggleTexBehavior);
		}
		%delayLayerFactor = 0;
		if (%obj.getLayer() == $LAYER["foreground_2"])
		{
			%delayLayerFactor = 1;
		}
		else
		{
			if (%obj.getLayer() == $LAYER["foreground_1"])
			{
				%delayLayerFactor = 2;
				break;
			}
			if (%obj.getLayer() == $LAYER["main"])
			{
				%delayLayerFactor = 3;
				break;
			}
			if (%obj.getLayer() == $LAYER["background_1"])
			{
				%delayLayerFactor = 4;
				break;
			}
			if (%obj.getLayer() == $LAYER["background_2"])
			{
				%delayLayerFactor = 5;
				break;
			}
			if (%obj.getLayer() == $LAYER["background_3"])
			{
				%delayLayerFactor = 6;
				break;
			}
			%delayLayerFactor = 3;
		}
		%toggleTexBehavior.Delay = 0.019999999552965164 * %delayLayerFactor;
		%toggleTexBehavior.durationList = "18 0.1 0.1 0.1 0.1";
		%toggleTexBehavior.startToggle();
		%i = %i + 1.0;
	}
	return thunderTextures.getCount();
}
function BeThunderController::stop(%this)
{
	%i = 0;
	while (%i < thunderTextures.getCount())
	{
		%obj = thunderTextures.getObject(%i);
		%toggleTexBehavior = %obj.getBehavior("BeTextureToggle");
		if (!(isObject(%toggleTexBehavior)))
		{
			return isObject(%toggleTexBehavior);
		}
		%toggleTexBehavior.stopToggle();
		%i = %i + 1.0;
	}
	return thunderTextures.getCount();
}
