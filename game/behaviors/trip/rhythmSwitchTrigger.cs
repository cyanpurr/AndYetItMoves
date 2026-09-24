// rhythmSwitchTrigger.cs.dso
if (!(isObject(BeRhythmSwitchTrigger)))
{
	%template = new BehaviorTemplate(Name : BeRhythmSwitchTrigger);
	%template.friendlyName = "RhythmSwitchTrigger";
	%template.behaviorType = "LevelTrip";
	%template.description = "this trigger activates the rhythm switch challenge";
	%template.addBehaviorField(stepsDuration, "a list of numbers of notes for every step", string, "4 6 8");
	%template.addBehaviorField(allowOpposite, "a list of bools if opposite switches are allowed for every step", string, "false false true");
	%template.addBehaviorField(quantification, "ticksounds per interval", integer, "8");
	%template.addBehaviorField(timeInBeats, "the time of 'timeInBeats' beats in which player can reach the next note", integer, "1");
}
function BeRhythmSwitchTrigger::onBehaviorAdd(%this)
{
	%owner = Owner;
	%owner.addDependentBehavior("BeTrigger");
	subscribeToEvents(%this, "onLevelLoadFinished40 onBeat");
	return;
}
function BeRhythmSwitchTrigger::onLevelLoadFinished40(%this)
{
	%stepSwitchIndices = "";
	%i = 0;
	while (%i < getWordCount(stepsDuration))
	{
		%actualStepDuration = getWord(stepsDuration, %i);
		%allowOpposition = getWord(allowOpposite, %i);
		%this.lastRhythmSwitchIndex = -1.0;
		%j = 0;
		while (%j < %actualStepDuration)
		{
			%nextRhythmSwitchIndex = %this.generateSwitchIndex(lastRhythmSwitchIndex, %allowOpposition);
			%stepSwitchIndices = ['%stepSwitchIndices', '%nextRhythmSwitchIndex'] SPC "";
			%this.lastRhythmSwitchIndex = %nextRhythmSwitchIndex;
			%j = %j + 1.0;
		}
		%stepSwitchIndices = %stepSwitchIndices TAB "";
		%i = %i + 1.0;
	}
	%this.stepSwitchIndices = trim(%stepSwitchIndices);
	debugEcho(stepSwitchIndices);
	%this.actualStep = "0";
	%this.setupInterval(quantification, timeInBeats);
	$stepSequenceIsActive = 0;
	return;
}
function BeRhythmSwitchTrigger::generateSwitchIndex(%this, %excludeIndex, %allowOppositeSwitch)
{
	%rhythmSwitchNR = allRhythmSwitches.getCount();
	debugEcho("%rhythmSwitchNR" SPC %rhythmSwitchNR);
	while (1)
	{
		%foundIndex = getRandom("0", %rhythmSwitchNR - 1.0);
		if (%excludeIndex == -1.0)
		{
			break;
		}
		if (%excludeIndex == %foundIndex)
		{
		}
		else
		{
			%switchesDistance = number - number;
			%switchesDistance = modulo(mAbs(%switchesDistance), "2");
			if (%allowOppositeSwitch || %switchesDistance == 1.0)
			{
				break;
			}
		}
	}
	return %foundIndex;
	return %foundIndex;
}
function BeRhythmSwitchTrigger::setupInterval(%this, %ticks, %beats)
{
	%this.tickSoundsPerInterval = %ticks;
	%this.intervallTimeMS = $currentBeatLength * 1000.0 * %beats;
	%this.tickSoundTimeMS = intervallTimeMS / tickSoundsPerInterval;
	return;
}
function BeRhythmSwitchTrigger::nextTickInMS(%this)
{
	%timeSinceLastBeatMS = scenegraph.getSceneTime() - lastBeatTime * 1000.0;
	%nextTickSoundMS = tickSoundTimeMS - modulo(%timeSinceLastBeatMS, tickSoundTimeMS);
	return %nextTickSoundMS;
	return %nextTickSoundMS;
}
function BeRhythmSwitchTrigger::onEnter(%this, )
{
	if (isActive || activateOnLanded)
	{
		return %this;
	}
	%this.activateOnLanded = "1";
	if (isFalling)
	{
		subscribeToEvents(%this, "onPlayerLanded onPlayerDeath");
	}
	else
	{
		%this.onPlayerLanded();
	}
	return;
}
function BeRhythmSwitchTrigger::onPlayerLanded(%this)
{
	%this.isActive = "1";
	%this.activateOnLanded = "0";
	unSubscribeFromEvents(%this, "onPlayerLanded");
	subscribeToEvents(%this, "onRythmSwitchFailure");
	triggerEvent("onSwitchesActive");
	%this.schedule("1500", "setup");
	return;
}
function BeRhythmSwitchTrigger::onPlayerDeath(%this)
{
	if (activateOnLanded)
	{
		%this.activateOnLanded = "0";
		unSubscribeFromEvents(%this, "onPlayerLanded onPlayerDeath");
		return;
	}
	%this.resetCurrentState();
	return;
}
function BeRhythmSwitchTrigger::setup(%this)
{
	debugEcho("setting up RhyrhmSwitchTrigger ");
	%this.actualSwitchIndices = getField(stepSwitchIndices, actualStep);
	%i = 0;
	while (%i < allRhythmSwitches.getCount())
	{
		%switch = allRhythmSwitches.getObject(%i);
		%switch.setSwitchTriggerObject(%this);
		%i = %i + 1.0;
	}
	%this.lastRhythmSwitchIndex = -1.0;
	%this.playStepMelody();
	return;
}
function BeRhythmSwitchTrigger::playStepMelody(%this)
{
	%this.actualStepIndex = "0";
	%this.nextNoteSoundSchedule = %this.schedule(%this.nextTickInMS(), "playPreviewNote");
	return;
}
function BeRhythmSwitchTrigger::playPreviewNote(%this)
{
	%noteIndex = getWord(actualSwitchIndices, actualStepIndex);
	%this.actualStepIndex = actualStepIndex + 1.0;
	%currentSwitch = allRhythmSwitches.getObject(%noteIndex);
	%currentSwitch.playSound("0.6");
	%currentSwitch.blink("200", visualizeColorGreen, "0.85");
	if (actualStepIndex == getWordCount(actualSwitchIndices))
	{
		%this.actualStepIndex = "0";
		%this.selectNextTarget();
		$stepSequenceIsActive = 1;
	}
	else
	{
		%this.nextNoteSoundSchedule = %this.schedule(tickSoundTimeMS, "playPreviewNote");
	}
	return;
}
function BeRhythmSwitchTrigger::onBeat(%this)
{
	%this.lastBeatTime = scenegraph.getSceneTime();
	return;
}
function BeRhythmSwitchTrigger::selectNextTarget(%this)
{
	if (actualStepIndex == getWordCount(actualSwitchIndices))
	{
		%this.setNextStep();
		return;
	}
	%nextRhythmSwitchIndex = getWord(actualSwitchIndices, actualStepIndex);
	%this.actualStepIndex = actualStepIndex + 1.0;
	%switch = allRhythmSwitches.getObject(%nextRhythmSwitchIndex);
	%switch.activate();
	%this.lastRhythmSwitchIndex = %nextRhythmSwitchIndex;
	return;
}
function BeRhythmSwitchTrigger::setNextStep(%this)
{
	%this.actualStep = actualStep + 1.0;
	if (actualStep == getWordCount(stepsDuration))
	{
		unSubscribeFromEvents(%this, "onPlayerDeath onRythmSwitchFailure");
		%this.schedule("500", "allDone");
	}
	else
	{
		$stepSequenceIsActive = 0;
		%this.schedule("500", "oneLevelDone");
		%this.schedule("2000", "setup");
	}
	return;
}
function BeRhythmSwitchTrigger::oneLevelDone(%this)
{
	%this.handle = alxPlay(SpawnPointNew);
	alxSourcef(handle, AL_GAIN, "0.4");
	%this.oneLevelVictoryBlink();
	return;
}
function BeRhythmSwitchTrigger::allDone(%this)
{
	triggerEvent("onSwitchesFinish");
	$stepSequenceIsActive = 0;
	switchBeat.action = "stop";
	switchBeat.onEnter();
	alxPlay(LevelFinishedNew);
	%this.victoryBlink("0");
	return;
}
function BeRhythmSwitchTrigger::resetCurrentState(%this)
{
	$stepSequenceIsActive = 0;
	debugEcho("resetting current state");
	if (isEventPending(nextNoteSoundSchedule))
	{
		cancel(nextNoteSoundSchedule);
	}
	%this.failedHandle = alxPlay(Failed);
	alxSourcef(failedHandle, AL_GAIN, "1");
	%i = 0;
	while (%i < allRhythmSwitches.getCount())
	{
		%switch = allRhythmSwitches.getObject(%i);
		%switch.deactivate();
		%switch.clearFailure();
		%switch.blink("200", visualizeColorRed, "0.85");
		%i = %i + 1.0;
	}
	%this.schedule("1500", "setup");
	return;
}
function BeRhythmSwitchTrigger::onRythmSwitchFailure(%this)
{
	debugEcho("onRythmSwitchFailure");
	%this.resetCurrentState(%this);
	return;
}
function BeRhythmSwitchTrigger::victoryBlink(%this, %index)
{
	if (%index > 3.0)
	{
		%index = 0;
	}
	%i = 0;
	while (%i < allRhythmSwitches.getCount())
	{
		if (%index + 1.0 == number)
		{
			%currentSwitch = allRhythmSwitches.getObject(%i);
			%currentSwitch.blink("200", visualizeColorGreen, "0.85");
			%index = %index + 1.0;
			%this.schedule("500", "victoryBlink", %index + 1.0);
			break;
		}
		%i = %i + 1.0;
	}
	return allRhythmSwitches.getCount();
}
function BeRhythmSwitchTrigger::oneLevelVictoryBlink(%this)
{
	%i = 0;
	while (%i < allRhythmSwitches.getCount())
	{
		%currentSwitch = allRhythmSwitches.getObject(%i);
		%currentSwitch.blink("200", visualizeColorGreen, "0.85");
		%i = %i + 1.0;
	}
	return allRhythmSwitches.getCount();
}
