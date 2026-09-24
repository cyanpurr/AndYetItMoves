// rhythmSwitch.cs.dso
if (!(isObject(BeRhythmSwitch)))
{
	%template = new BehaviorTemplate(Name : BeRhythmSwitch);
	%template.friendlyName = "RhythmSwitch";
	%template.behaviorType = "LevelTrip";
	%template.description = "this object will be part of a group of switches which have to be activated within the time of one beat (created from teh beatgenerator)";
	%template.addBehaviorField(number, "the number of the note in the beat", integer, "1");
}
function BeRhythmSwitch::onBehaviorAdd(%this)
{
	%owner = Owner;
	subscribeToEvents(%this, "onLevelLoadFinished30 onPlayerLanded");
	return;
}
function BeRhythmSwitch::onLevelLoadFinished30(%this)
{
	%owner = Owner;
	%this.originalBlendColor = getWords(%owner.getBlendColor(), "0", "2");
	if (!(isObject(allRhythmSwitches)))
	{
		new SimSet(Name : allRhythmSwitches);
		levelGarbageCollector.add(allRhythmSwitches);
	}
	allRhythmSwitches.add(%this);
	%currentColor = $RHYTHMCOLOR[number];
	%this.newBlendColor = multVector(%currentColor, originalBlendColor);
	%this.visualizeColorRed = "1 0 0";
	%this.visualizeColorGreen = "0 1 0";
	%this.visualizeColorBlue = "0 0 1";
	%this.blink("0");
	%this.deactivate();
	return;
}
function BeRhythmSwitch::activate(%this)
{
	%this.activated = "1";
	return;
}
function BeRhythmSwitch::deactivate(%this)
{
	%this.activated = "0";
	return;
}
function BeRhythmSwitch::clearFailure(%this)
{
	%this.failure = "0";
	return;
}
function BeRhythmSwitch::setSwitchTriggerObject(%this, %switchTrigger)
{
	%this.RhythmSwitchTrigger = %switchTrigger;
	return;
}
function BeRhythmSwitch::onPlayerLanded(%this)
{
	%owner = Owner;
	%behavior = groundCollisionObject.getBehavior("BeRhythmSwitch");
	if (!(%behavior))
	{
		return;
	}
	if (%behavior == %this)
	{
		if (!(activated) && $stepSequenceIsActive)
		{
			triggerEvent("onRythmSwitchFailure");
			return;
		}
		if (!(activated))
		{
			return %this;
		}
		%this.playSound("0.6");
		%this.blink("200", visualizeColorBlue, "0.85");
		%this.deactivate();
		RhythmSwitchTrigger.selectNextTarget();
	}
	return;
}
function BeRhythmSwitch::playSound(%this, %volume)
{
	%profile = "Chord" @ number;
	%this.audioHandle = alxPlay(%profile);
	alxSourcef(audioHandle, AL_GAIN, %volume);
	return;
}
function BeRhythmSwitch::blink(%this, %timeMS, %visualizeColor, %alphaValue)
{
	%owner = Owner;
	if (%timeMS > 0.0)
	{
		%owner.setBlendColor(getR(%visualizeColor), getG(%visualizeColor), getB(%visualizeColor));
		if (%alphaValue != "")
		{
			%owner.setBlendAlpha(%alphaValue);
		}
	}
	%owner.schedule(%timeMS, "setBlendColor", getR(newBlendColor), getG(newBlendColor), getB(newBlendColor));
	return;
}
