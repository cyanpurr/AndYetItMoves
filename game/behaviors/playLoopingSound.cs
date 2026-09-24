// playLoopingSound.cs.dso
if (!(isObject(BePlayLoopingSound)))
{
	%template = new BehaviorTemplate(Name : BePlayLoopingSound);
	%template.friendlyName = "PlayLoopingSound";
	%template.behaviorType = "Audio";
	%template.description = "plays a looping sound and updates volume depending on distance to player";
	%template.addBehaviorField(AudioProfile, "the audio profile to play", string, "");
	%template.addBehaviorField(volume, "the maximum volume of the sound", float, "1");
	%template.addBehaviorField(applyDistanceFactor, "if true, teh distance to the player will change the volume of the looping sound", bool, "1");
	%template.addBehaviorField(autoPlay, "should the loop play right away ?", bool, "0");
}
function BePlayLoopingSound::onBehaviorAdd(%this)
{
	subscribeToEvent(%this, "onLevelLoadFinished");
	return;
}
function BePlayLoopingSound::onBehaviorRemove(%this)
{
	%this.stop();
	return;
}
function BePlayLoopingSound::onLevelLoadFinished(%this)
{
	if (autoPlay)
	{
		%this.play();
	}
	return;
}
function BePlayLoopingSound::play(%this)
{
	%owner = Owner;
	if (!(alxIsPlaying(handle)))
	{
		%this.handle = alxPlay(AudioProfile);
	}
	alxSourcef(handle, AL_GAIN, volume);
	if (applyDistanceFactor)
	{
		%this.updateDistanceFactor();
	}
	return;
}
function BePlayLoopingSound::updateDistanceFactor(%this)
{
	%owner = Owner;
	alxSourcef(handle, AL_GAIN, volume * calcDistanceFactor(%owner));
	%this.schedule = %this.schedule("500", "updateDistanceFactor");
	return;
}
function BePlayLoopingSound::stop(%this)
{
	%owner = Owner;
	alxStop(handle);
	if (isEventPending(schedule))
	{
		cancel(schedule);
	}
	return;
}
function BePlayLoopingSound::fadeOut(%this)
{
	if (decrease $= "")
	{
		%this.decrease = %this.getVolume() / 10.0;
	}
	%this.setVolume(%this.getVolume() - decrease);
	if (%this.getVolume() <= 0.0)
	{
		%this.stop();
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
function BePlayLoopingSound::getIsPlaying(%this)
{
	return alxIsPlaying(handle);
	return alxIsPlaying(handle);
}
function BePlayLoopingSound::setAudioProfile(%this, %profile)
{
	%this.AudioProfile = %profile;
	return;
}
function BePlayLoopingSound::getAudioProfile(%this)
{
	return AudioProfile;
	return AudioProfile;
}
function BePlayLoopingSound::setVolume(%this, %volume)
{
	%this.volume = %volume;
	return;
}
function BePlayLoopingSound::getVolume(%this)
{
	return volume;
	return volume;
}
function BePlayLoopingSound::switchOff(%this)
{
	%this.stop();
	return;
}
function BePlayLoopingSound::switchOn(%this)
{
	%this.play();
	return;
}
