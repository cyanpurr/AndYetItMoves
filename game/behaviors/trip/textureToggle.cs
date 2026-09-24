// textureToggle.cs.dso
if (!(isObject(BeTextureToggle)))
{
	%template = new BehaviorTemplate(Name : BeTextureToggle);
	%template.friendlyName = "Texture Toggle";
	%template.behaviorType = "LevelTrip";
	%template.description = "switches a set of textures in a given intervall";
	%template.addBehaviorField(imageMapList, "list of imagemap-names, that shall be toggled; if the first word is 'frames', framenumbers can be used (actual imagemap)", string, "change_me_imageMap");
	%template.addBehaviorField(durationList, "list of durations in the same order as textures (in seconds)", string, "2");
	%template.addBehaviorField(Delay, "time until it starts (in seconds)", float, "0");
	%template.addBehaviorField(autoPlay, "if the texture toggeling shall begin at levelstart", bool, "0");
	%template.addBehaviorField(playSoundIndizes, "list of indizes of imageMapList which shall be linked with a sound", string, "");
	%template.addBehaviorField(audioProfileList, "list of sounds that shall be played at respective indizes", string, "");
}
function BeTextureToggle::onBehaviorAdd(%this)
{
	subscribeToEvents(%this, "onLevelLoadFinished10 onLevelLoadFinished");
	%this.defaultDuration = "2";
	return;
}
function BeTextureToggle::onLevelLoadFinished10(%this)
{
	%owner = Owner;
	%this.checkFields();
	%this.nextToggleEvent = "0";
	%owner.addDependentBehavior("BeTexture");
	thunderTextures.add(%owner);
	return;
}
function BeTextureToggle::onLevelLoadFinished(%this)
{
	if (autoPlay)
	{
		%this.startToggle();
	}
	return;
}
function BeTextureToggle::startToggle(%this)
{
	if (toggleNum $= "")
	{
		%this.toggleNum = "0";
	}
	if (textureCount > 0.0)
	{
		%this.schedule(Delay * 1000.0, "setTextureNumber", "0", "1");
	}
	return;
}
function BeTextureToggle::stopToggle(%this)
{
	%owner = Owner;
	if (nextToggleEvent != 0.0)
	{
		cancel(nextToggleEvent);
		%this.toggleNum = toggleNum - 1.0;
		%this.nextToggleEvent = "0";
	}
	%imagemap = getWord(imageMapList, "0");
	if (flipFrames)
	{
		%owner.setFrame(%imagemap);
	}
	else
	{
		%owner.setImageMap(%imagemap);
	}
	return;
}
function BeTextureToggle::setTextureNumber(%this, %index, %continue)
{
	%owner = Owner;
	%this.toggleNum = modulo(%index, textureCount);
	%imagemap = getWord(imageMapList, toggleNum);
	%soundIndex = getWordIndex(playSoundIndizes, toggleNum);
	if (%soundIndex > -1.0)
	{
		%audioProfile = getWord(audioProfileList, %soundIndex);
		playHandleEventSound("textureToggle", %audioProfile, "1");
	}
	if (flipFrames)
	{
		%owner.setFrame(%imagemap);
	}
	else
	{
		%owner.setImageMap(%imagemap);
	}
	if (%continue)
	{
		%nextDuration = 1000.0 * getWord(durationList, modulo(toggleNum, getWordCount(durationList)));
		%this.toggleNum = toggleNum + 1.0;
		%this.nextToggleEvent = %this.schedule(%nextDuration, "setTextureNumber", toggleNum, "1");
	}
	else
	{
		%this.toggleNum = toggleNum + 1.0;
	}
	return %this;
}
function BeTextureToggle::checkFields(%this)
{
	if (firstWord(imageMapList) $= "frames")
	{
		%this.flipFrames = "1";
		%this.imageMapList = restWords(imageMapList);
	}
	if (getWordCount(imageMapList) != getWordCount(durationList))
	{
		debugWarn("imageMapList and durationList in BeTextureToggle-Behavior are having an unequal length! repeating or cutting durationList");
	}
	%i = 0;
	while (%i < getWordCount(imageMapList))
	{
		if (flipFrames)
		{
			break;
		}
		%imagemap = getWord(imageMapList, %i);
		%removeItem = !(isObject(%imagemap));
		if (!(%removeItem))
		{
			%removeItem = %imagemap.getClassName() != "t2dImageMapDatablock";
		}
		if (%removeItem)
		{
			%this.imageMapList = removeWord(imageMapList, %i);
			%this.durationList = removeWord(durationList, %i);
			debugWarn("removed not valid texture '" @ %imagemap @ "' from imageMapList in BeTextureToggle-Behavior");
			%i = %i - 1.0;
		}
		%i = %i + 1.0;
	}
	%this.textureCount = getWordCount(imageMapList);
	%i = 0;
	while (%i < getWordCount(durationList))
	{
		%duration = getWord(durationList, %i);
		if (!(isNumberNoZero(%duration)))
		{
			%this.durationList = setWord(durationList, %i, defaultDuration);
			debugWarn("replaced invalid duration" SPC %duration SPC "with default value" SPC defaultDuration SPC "in BeTextureToggle-Behavior");
		}
		%i = %i + 1.0;
	}
	return getWordCount(durationList);
}
