// PulsatorController.cs.dso
if (!(isObject(BePulsatorController)))
{
	%template = new BehaviorTemplate(Name : BePulsatorController);
	%template.friendlyName = "Pulsator Controller";
	%template.behaviorType = "LevelTrip";
	%template.description = "will interpolate the targetValue of the pulsators from startValue to targetValue over the duration of timeToEndValue";
	%template.addBehaviorField(startValue, "where to start", float, "0");
	%template.addBehaviorField(targetValue, "where to end", float, "0.1");
	%template.addBehaviorField(stepInterval, "how smooth will we interpolate to the targetValue", float, "0.005");
	%template.addBehaviorField(timeToEndValue, "how long until we reach the endvalue in sec.", float, "30");
	%template.addBehaviorField(autoPlay, "wheter or not to start right away", bool, "0");
}
function BePulsatorController::onBehaviorAdd(%this)
{
	subscribeToEvent(%this, "onLevelLoadFinished10");
	return;
}
function BePulsatorController::onLevelLoadFinished10(%this)
{
	%this.pulsators = getObjectsWithBehavior("BePulsator");
	%i = 0;
	while (%i < getWordCount(pulsators))
	{
		%pulsatorBehavior = getWord(pulsators, %i).getBehavior("BePulsator");
		if (!(globalControled))
		{
			%this.pulsators = removeWord(pulsators, %i);
			%i = %i - 1.0;
		}
		%i = %i + 1.0;
	}
	if (autoPlay)
	{
		%this.switchOn();
	}
	return;
}
function BePulsatorController::switchOn(%this)
{
	%this.updateInterval = timeToEndValue * stepInterval / targetValue;
	%this.amountOfCalls = targetValue / stepInterval;
	%this.callCount = "0";
	%i = 0;
	while (%i < getWordCount(pulsators))
	{
		%pulsatorBehavior = getWord(pulsators, %i).getBehavior("BePulsator");
		%pulsatorBehavior.originalTargetValue = targetValue;
		%pulsatorBehavior.targetValue = startValue;
		%pulsatorBehavior.switchOn("1");
		%i = %i + 1.0;
	}
	%this.scheduleID = %this.schedule(updateInterval * 1000.0, "nextStep");
	return;
}
function BePulsatorController::nextStep(%this)
{
	%this.callCount = callCount + 1.0;
	%i = 0;
	while (%i < getWordCount(pulsators))
	{
		%pulsatorBehavior = getWord(pulsators, %i).getBehavior("BePulsator");
		if (overRideGlobalTargetValue)
		{
			%pulsatorBehavior.targetValue = targetValue + stepInterval * originalTargetValue;
		}
		else
		{
			%pulsatorBehavior.targetValue = targetValue + stepInterval;
		}
		%i = %i + 1.0;
	}
	if (callCount < amountOfCalls)
	{
		%this.scheduleID = %this.schedule(updateInterval * 1000.0, "nextStep");
	}
	return;
}
function BePulsatorController::switchOff(%this)
{
	if (isObject(scheduleID))
	{
		scheduleID.cancel();
	}
	return;
}
