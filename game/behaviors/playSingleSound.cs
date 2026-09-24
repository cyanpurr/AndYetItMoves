// playSingleSound.cs.dso
if (!(isObject(BePlaySingleSound)))
{
	%template = new BehaviorTemplate(Name : BePlaySingleSound);
	%template.friendlyName = "PlaySingleSound";
	%template.behaviorType = "Audio";
	%template.description = "plays a Single sound and calculates volume depending on distance to player";
	%template.addBehaviorField(AudioProfile, "the audio profile to play", string, "");
	%template.addBehaviorField(maxVolume, "the maximum volume of the sound", float, "1");
	%template.addBehaviorField(applyDistanceFactor, "if true, teh distance to the player will change the volume of the looping sound", bool, "1");
	%template.addBehaviorField(autoPlay, "should the sound play right away ?", bool, "0");
}
function BePlaySingleSound::onBehaviorAdd(%this)
{
	subscribeToEvents(%this, "onLevelLoadFinished");
	return;
}
function BePlaySingleSound::onLevelLoadFinished(%this)
{
	if (autoPlay)
	{
		%this.play();
	}
	if (customVolume $= "")
	{
		%this.customVolume = "1";
	}
	return;
}
function BePlaySingleSound::play(%this)
{
	if (alxIsPlaying(handle))
	{
		return alxIsPlaying(handle);
	}
	%owner = Owner;
	%this.handle = alxPlay(AudioProfile);
	if (applyDistanceFactor)
	{
		%factor = calcDistanceFactor(%owner);
	}
	else
	{
		%factor = 1;
	}
	if (customVolume $= "")
	{
		%this.customVolume = "1";
	}
	%this.currentVolume = maxVolume * calcDistanceFactor(%owner) * customVolume;
	alxSourcef(handle, AL_GAIN, currentVolume);
	if (applyDistanceFactor)
	{
		%this.updateDistanceFactor();
	}
	return;
}
function BePlaySingleSound::updateDistanceFactor(%this)
{
	%owner = Owner;
	if (!(%this.getIsPlaying()))
	{
		return %this.getIsPlaying();
	}
	if (customVolume $= "")
	{
		%this.customVolume = "1";
	}
	if (fader $= "")
	{
		%this.fader = "1";
	}
	%this.currentVolume = maxVolume * calcDistanceFactor(%owner) * customVolume * fader;
	alxSourcef(handle, AL_GAIN, currentVolume);
	%this.schedule = %this.schedule("100", "updateDistanceFactor");
	return;
}
function BePlaySingleSound::stop(%this)
{
	alxStop(handle);
	if (isEventPending(schedule))
	{
		cancel(schedule);
	}
	return;
}
function BePlaySingleSound::fadeOut(%this, %stopIt)
{
	if (!(alxIsPlaying(handle)))
	{
		debugEcho("nothing plays... no fadeOut");
		return;
	}
	if (decrease $= "")
	{
		%this.decrease = %this.getVolume() / 10.0;
	}
	%this.fader = %this.getVolume() - decrease;
	if (fader <= 0.0)
	{
		if (%stopIt)
		{
			%this.stop();
		}
		debugEcho("fadeout ready, volume =  " SPC %this.getVolume());
		return;
	}
	else
	{
		%this.schedule("100", "fadeOut");
		debugEcho("rescheduling fadeout");
	}
	return;
}
function BePlaySingleSound::fadeIn(%this)
{
	if (!(alxIsPlaying(handle)))
	{
		debugEcho("nothing plays... no fadein");
		return;
	}
	if (increase $= "")
	{
		%this.increase = maxVolume / 10.0;
	}
	%this.fader = %this.getVolume() + increase;
	if (fader >= maxVolume)
	{
		debugEcho("fadeinready, volume =  " SPC %this.getVolume());
		return;
	}
	else
	{
		%this.schedule("100", "fadeIn");
		debugEcho("rescheduling fadein");
	}
	return;
}
function BePlaySingleSound::getIsPlaying(%this)
{
	return alxIsPlaying(handle);
	return alxIsPlaying(handle);
}
function BePlaySingleSound::setAudioProfile(%this, %profile)
{
	%this.AudioProfile = %profile;
	return;
}
function BePlaySingleSound::getAudioProfile(%this)
{
	return AudioProfile;
	return AudioProfile;
}
function BePlaySingleSound::setVolume(%this, %volume)
{
	alxSourcef(handle, AL_GAIN, %volume);
	return;
}
function BePlaySingleSound::getVolume(%this)
{
	return currentVolume;
	return currentVolume;
}
function BePlaySingleSound::setCustomVolume(%this, %customVolume)
{
	if (%customVolume < 0.0)
	{
		%customVolume = 0;
	}
	else
	{
		if (%customVolume > 1.0)
		{
			%customVolume = 1;
		}
	}
	%this.customVolume = %customVolume;
	return;
}
function BePlaySingleSound::getCustomVolume(%this)
{
	return customVolume;
	return customVolume;
}
function BePlaySingleSound::switchOff(%this)
{
	%this.stop();
	return;
}
