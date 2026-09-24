// playSound.cs.dso
if (!(isObject(BePlaySound)))
{
	%template = new BehaviorTemplate(Name : BePlaySound);
	%template.friendlyName = "PlaySound";
	%template.behaviorType = "Audio";
	%template.description = "for environmental audio objects. their volume can depend on player position. markov chained sounds on wii have to be streamed";
	%template.addBehaviorField(profileName, "name of the audioprofile. ignored if isMarkovChained", string, "");
	%template.addBehaviorField(fixedVolume, "if set to true, this objects volume does not depend on the players postion. if set to false owner must be trigger", bool, "0");
	%template.addBehaviorField(fadeDuration, "if fixedVolume = true, use this duration to fade the sound in and out. in seconds", float, "3");
	%template.addBehaviorField(fadeWholeChannel, "if set to true, channel volume will fade instead of soundhandle volume, thus affecting all sounds on this channel", bool, "0");
	%template.addBehaviorField(maxVolume, "maximal volume of this sound [0 ; 1]", float, "1");
	%template.addBehaviorField(fullVolumeFactor, "percent of collision radius where full volume is reached. only used with fixedVolume == false", float, "0.5");
	%template.addBehaviorField(distanceCalculationMode, "how the volume is calculated using the distance. can be LINEAR, QUADRATIC or CUBIC. only used with fixedVolume == false", string, "LINEAR");
	%template.addBehaviorField(isMarkovChained, "if this soundobject shall receive markovchained soundsamples, profileName is ignored if true", bool, "0");
	%template.addBehaviorField(markovChain, "the scriptobject in which the transition matrix is stored", string, "");
	%template.addBehaviorField(delayRange, "for markovchains: the random-delay-range, as a 2 element vector in ms", string, "");
	%template.addBehaviorField(autoPlay, "play on level load or use owner trigger to switch on/off this sound (if owner is a trigger)", bool, "0");
	%template.addBehaviorField(turnOnGroups, "space sep. list of $MusicAudioChannel indices. turn all BePlaySounds of these channels on on leave. owner must be trigger", string, "");
	%template.addBehaviorField(turnOffGroups, "space sep. list of $MusicAudioChannel indices. turn all BePlaySounds of these channels off on enter. owner must be trigger", string, "");
	%template.addBehaviorField(toggleSide, "side of trigger on which turning on(enter) resp. off(onLeave) occurs", enum, "ALL", "ALL	LEFT	TOP	RIGHT	BOTTOM");
	%template.addBehaviorField(enclosedSpawnPoints, "if toggleSide is active, set list of spawnpoint numbers which lie between the on and off trigger for player death sound reset", string, "");
	%template.addBehaviorField(belongsTo, "if used with toggleSide, name of other beplaysound owner which starts/stops this sound. all fields except toggleSide are copied from belongsTo object", object, null, t2dSceneObject);
	%template.addBehaviorField(disableOnLeave, "disables this and the belonging trigger first time the player leaves the sound area. DO NOT SET ON MAIN TRIIGER ONLY ON 'BELONGS TO'", bool, "0");
}
function BePlaySound::onBehaviorAdd(%this)
{
	if ($WII && getOS() != "wii")
	{
		return;
	}
	subscribeToEvent(%this, "onLevelLoadFinished");
	if (Owner.getClassName() != "t2dTrigger")
	{
		Owner.setCollisionActive("0", "1");
	}
	return;
}
function BePlaySound::onLevelLoadFinished(%this)
{
	if (!(isObject(profileName)) && isObject(markovChain))
	{
		%this.profileName = getWord(profiles, "0");
	}
	%this.audioChannelObject = getAudioChannelObject(profileName);
	sounds.add(%this);
	%this.handle = "0";
	%this.isPlaying = "0";
	%this.curVolume = maxVolume;
	%owner = Owner;
	if (!(fixedVolume) || turnOnGroups != "" || turnOffGroups != "" && %owner.getClassName() != "t2dTrigger")
	{
		debugWarn("owner of bePlaysound object with distance dependent volume or turnOn/OffGroups is not a trigger!");
		return;
	}
	if (%owner.getClassName() $= "t2dTrigger")
	{
		%owner.setGraphGroup($GROUPS["zoomTrigger"]);
		%owner.setCollisionGroups($GROUPS["noCollision"]);
		%owner.setCollisionActive("0", "1");
		%owner.setEnterCallback("1");
		%owner.setLeaveCallback("1");
		if (!(fixedVolume))
		{
			%owner.setStayCallback("1");
			%owner.setCollisionDetection("CIRCLE");
		}
		%this.startDistance = %owner.getCollisionRadius();
		%this.fullVolumeDistance = startDistance * fullVolumeFactor;
	}
	if (isMarkovChained && isObject(markovChain))
	{
		markovChain.curPlaySoundBehavior = %this;
	}
	if (autoPlay)
	{
		%this.start();
	}
	if (toggleSide != "ALL" && enclosedSpawnPoints != "" && !(isObject(belongsTo)))
	{
		subscribeToEvent(%this, "onPlayerReanimate");
	}
	return;
}
function BePlaySound::fadeIn(%this, %duration)
{
	if (%this.isFadingIn())
	{
		debugWarn("called fadeIn on BePlaySound, which is already fading in. aborting");
		return;
	}
	if (!(%this.isFading()))
	{
		%this.curVolume = $minFadeFactor;
	}
	%this.fadeDelta = maxVolume - $minFadeFactor / %duration;
	%this.start();
	subscribeToEvent(%this, "onUpdateFrame");
	return;
}
function BePlaySound::fadeInWithChannel(%this)
{
	if (!(audioChannelObject.isFadingIn()))
	{
		audioChannelObject.fadeFactor = $minFadeFactor;
	}
	audioChannelObject.fadeIn(fadeDuration);
	%this.start();
	return;
}
function BePlaySound::start(%this)
{
	if (isPlaying)
	{
		return %this;
	}
	if (isMarkovChained)
	{
		%this.startMarkov();
	}
	else
	{
		%this.handle = playEventSound(profileName, curVolume);
	}
	%this.isPlaying = "1";
	return;
}
function BePlaySound::fadeOut(%this, %duration)
{
	if (%this.isFadingOut())
	{
		debugWarn("called fadeOut on BePlaySound, which is already fading out. aborting");
		return;
	}
	if (!(%this.isFading()))
	{
		%this.curVolume = maxVolume;
	}
	%this.fadeDelta = -1.0 * maxVolume - $minFadeFactor / %duration;
	subscribeToEvent(%this, "onUpdateFrame");
	return;
}
function BePlaySound::forcedFadeOut(%this, %duration)
{
	%owner = Owner;
	%owner.setStayCallback("0");
	%owner.setLeaveCallback("0");
	if (turnOnGroups != "")
	{
		fadeAllChannelSounds(turnOnGroups, "1");
	}
	%this.fadeOut(%duration);
	return;
}
function BePlaySound::fadeOutWithChannel(%this)
{
	audioChannelObject.fadeOut(fadeDuration);
	subscribeToEvent(%this, "onAudioChannelFadeOutFinished");
	return;
}
function BePlaySound::onAudioChannelFadeOutFinished(%this, %channelNumber)
{
	if (channelNumber == %channelNumber)
	{
		%this.stop();
	}
	unSubscribeFromEvent(%this, "onAudioFadeOutFinished");
	return;
}
function BePlaySound::stop(%this)
{
	if (isMarkovChained)
	{
		%this.stopMarkov();
	}
	else
	{
		alxStop(handle);
	}
	%this.isPlaying = "0";
	return;
}
function BePlaySound::onUpdateFrame(%this)
{
	%newVol = curVolume + duration * fadeDelta;
	%this.curVolume = min(maxVolume, max($minFadeFactor, %newVol));
	if (curVolume != %newVol)
	{
		if (%this.isFadingOut())
		{
			%this.stop();
		}
		%this.fadeDelta = "0";
		unSubscribeFromEvent(%this, "onUpdateFrame");
	}
	else
	{
		alxSourcef(handle, AL_GAIN, curVolume);
	}
	return;
}
function BePlaySound::onEnter(%this, %obj)
{
	if (%obj.getId() != camera.getId())
	{
		return %obj.getId();
	}
	if (!(%this.verifyTriggerSide(%obj)))
	{
		return %this.verifyTriggerSide(%obj);
	}
	if (isObject(belongsTo))
	{
		%this = belongsTo.getBehavior("BePlaySound");
	}
	%this.enterSoundArea();
	return;
}
function BePlaySound::enterSoundArea(%this)
{
	if (turnOffGroups != "")
	{
		fadeAllChannelSounds(turnOffGroups, "0");
	}
	if (fixedVolume)
	{
		if (fadeWholeChannel)
		{
			%this.fadeInWithChannel();
		}
		else
		{
			%this.fadeIn(fadeDuration);
		}
	}
	else
	{
		if (isObject(audioChannelObject))
		{
			audioChannelObject.resetFade();
		}
		%this.start();
	}
	return;
}
function BePlaySound::onLeave(%this, %obj)
{
	if (%obj.getId() != camera.getId())
	{
		return %obj.getId();
	}
	if (!(%this.verifyTriggerSide(%obj)))
	{
		return %this.verifyTriggerSide(%obj);
	}
	if (disableOnLeave)
	{
		debugEcho("leaving sound area and disabling it!");
		unSubscribeFromEvents(belongsTo, "onPlayerReanimate");
		Owner.setCollisionSuppress("1");
		belongsTo.setCollisionSuppress("1");
	}
	if (isObject(belongsTo))
	{
		%this = belongsTo.getBehavior("BePlaySound");
	}
	%this.leaveSoundArea();
	return;
}
function BePlaySound::leaveSoundArea(%this)
{
	if (fixedVolume)
	{
		if (fadeWholeChannel)
		{
			%this.fadeOutWithChannel();
		}
		else
		{
			%this.fadeOut(fadeDuration);
		}
	}
	else
	{
		%this.stop();
	}
	if (turnOnGroups != "")
	{
		fadeAllChannelSounds(turnOnGroups, "1");
	}
	return;
}
function BePlaySound::onPlayerReanimate(%this)
{
	%number = number;
	%numEnclosed = getWordCount(enclosedSpawnPoints);
	%isEnclosedSpawnpoint = 0;
	%i = 0;
	while (%i < %numEnclosed)
	{
		if (getWord(enclosedSpawnPoints, %i) == %number)
		{
			%isEnclosedSpawnpoint = 1;
			break;
		}
		%i = %i + 1.0;
	}
	if (%isEnclosedSpawnpoint && !(isPlaying) || %this.isFadingOut())
	{
		%this.enterSoundArea();
	}
	else
	{
		if (!(%isEnclosedSpawnpoint) && isPlaying)
		{
			if (fadeWholeChannel && !(audioChannelObject.isFadingOut()) || !(fadeWholeChannel) && !(%this.isFadingOut()))
			{
				%this.leaveSoundArea();
			}
		}
	}
	return;
}
function BePlaySound::onStay(%this, %obj)
{
	if (!(alxIsPlaying(handle)))
	{
		return alxIsPlaying(handle);
	}
	%gain = curVolume * calcDistanceFactor(Owner, startDistance, fullVolumeDistance, distanceCalculationMode);
	alxSourcef(handle, AL_GAIN, %gain);
	return;
}
function BePlaySound::startMarkov(%this)
{
	%this.profileName = getWord(profiles, "0");
	%randomDelay = getRandom(getWord(delayRange, "0"), getWord(delayRange, "1")) * 0.5;
	%this.markovScheduleId = %this.schedule(%randomDelay, "playNextMarkovState");
	return;
}
function BePlaySound::stopMarkov(%this)
{
	cancel(markovScheduleId);
	alxStop(handle);
	return;
}
function BePlaySound::playNextMarkovState(%this)
{
	%randomDelay = getRandom(getWord(delayRange, "0"), getWord(delayRange, "1"));
	%randomValue = getRandom();
	%lowerProbability = 0;
	%numProfiles = getWordCount(profiles);
	%i = 0;
	while (%i < %numProfiles)
	{
		%profile = getWord(profiles, %i);
		%upperProbability = transitions[profileName, %profile] + %lowerProbability;
		if (equalsMinMax(%randomValue, %lowerProbability, %upperProbability))
		{
			%this.profileName = %profile;
			%this.handle = playEventSound(profileName, curVolume);
			if ($WII)
			{
				%duration = alxGetStreamDuration(handle) * 1000.0;
			}
			else
			{
				%duration = alxGetWaveLen(fileName);
			}
			%this.markovScheduleId = %this.schedule(%duration + %randomDelay, "playNextMarkovState");
			break;
		}
		%lowerProbability = %upperProbability;
		%i = %i + 1.0;
	}
	return;
}
function BePlaySound::isFadingOut(%this)
{
	return fadeDelta < 0.0;
	return fadeDelta < 0.0;
}
function BePlaySound::isFadingIn(%this)
{
	return fadeDelta > 0.0;
	return fadeDelta > 0.0;
}
function BePlaySound::isFading(%this)
{
	return fadeDelta != 0.0;
	return fadeDelta != 0.0;
}
function fadeAllChannelSounds(%channelList, %in)
{
	%numGroups = getWordCount(%channelList);
	%i = 0;
	while (%i < %numGroups)
	{
		%simSet = sounds;
		%numSounds = %simSet.getCount();
		%j = 0;
		while (%j < %numSounds)
		{
			if (%in)
			{
				%simSet.getObject(%j).fadeInWithChannel();
			}
			else
			{
				%simSet.getObject(%j).fadeOutWithChannel();
			}
			%j = %j + 1.0;
		}
		%i = %i + 1.0;
	}
	return;
}
function BePlaySound::verifyTriggerSide(%this, %obj)
{
	if (toggleSide $= "ALL")
	{
		return "1";
	}
	%owner = Owner;
	if (toggleSide $= "TOP")
	{
		if (%obj.getPositionY() < %owner.getPositionY())
		{
			return "1";
		}
	}
	else
	{
		if (toggleSide $= "BOTTOM")
		{
			if (%obj.getPositionY() > %owner.getPositionY())
			{
				return "1";
			}
			break;
		}
		if (toggleSide $= "LEFT")
		{
			if (%obj.getPositionX() < %owner.getPositionX())
			{
				return "1";
			}
			break;
		}
		if (toggleSide $= "RIGHT")
		{
			if (%obj.getPositionX() > %owner.getPositionX())
			{
				return "1";
			}
		}
	}
	return "0";
	return "0";
}
function startAllEnvironmentSounds()
{
	fadeAllChannelSounds("1 3", "1");
	return;
}
function stopAllEnvironmentSounds()
{
	fadeAllChannelSounds("1 3", "0");
	return;
}
