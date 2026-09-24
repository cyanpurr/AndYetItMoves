// audioChannels.cs.dso
$AllAudioChannels = new SimSet(Name : "");
gameGarbageCollector.add($AllAudioChannels);
$minFadeFactor = 0.05000000074505806;
$FXAudioChannel = new ScriptObject(Name : "")
{
	class = "AudioChannel";
	channelNumber = "1";
}
$AllAudioChannels.add($FXAudioChannel);
$MenuAudioChannel = new ScriptObject(Name : "")
{
	class = "AudioChannel";
	channelNumber = "2";
}
$AllAudioChannels.add($MenuAudioChannel);
$MusicAudioChannel["1"] = new ScriptObject(Name : "")
{
	class = "AudioChannel";
	channelNumber = "3";
}
$AllAudioChannels.add($MusicAudioChannel["1"]);
$MusicAudioChannel["2"] = new ScriptObject(Name : "")
{
	class = "AudioChannel";
	channelNumber = "4";
}
$AllAudioChannels.add($MusicAudioChannel["2"]);
$MusicAudioChannel["3"] = new ScriptObject(Name : "")
{
	class = "AudioChannel";
	channelNumber = "5";
}
$AllAudioChannels.add($MusicAudioChannel["3"]);
$MusicAudioChannel["4"] = new ScriptObject(Name : "")
{
	class = "AudioChannel";
	channelNumber = "6";
}
$AllAudioChannels.add($MusicAudioChannel["4"]);
%i = 0;
while (%i < $AllAudioChannels.getCount())
{
	%channel = $AllAudioChannels.getObject(%i);
	%channel.baseVolume = "1";
	%channel.fadeFactor = "1";
	%channel.fadeDelta = "0";
	%channel.turnOffCounter = "0";
	%channel.sounds = new SimSet(Name : "");
	gameGarbageCollector.add(sounds);
	%i = %i + 1.0;
}
function resetAllAudioChannelVolumes()
{
	%i = 0;
	while (%i < $AllAudioChannels.getCount())
	{
		%channel = $AllAudioChannels.getObject(%i);
		%channel.fadeFactor = "1";
		%channel.fadeDelta = "0";
		%channel.turnOffCounter = "0";
		%channel.updateVolume();
		%i = %i + 1.0;
	}
	return $AllAudioChannels.getCount();
}
function AudioChannel::setVolume(%this, %volume)
{
	%this.baseVolume = %volume;
	alxSetChannelVolume(channelNumber, baseVolume * fadeFactor);
	return;
}
function AudioChannel::resetFade(%this)
{
	%this.fadeFactor = "1";
	%this.updateVolume();
	return;
}
function AudioChannel::updateVolume(%this)
{
	alxSetChannelVolume(channelNumber, baseVolume * fadeFactor);
	return;
}
function AudioChannel::fadeIn(%this, %delta)
{
	if (turnOffCounter > 0.0)
	{
		%this.turnOffCounter = turnOffCounter - 1.0;
	}
	if (turnOffCounter != 0.0)
	{
		return %this;
	}
	if (%this.isFadingOut())
	{
		triggerEvent("onAudioChannelFadeOutFinished", channelNumber);
	}
	else
	{
		%this.fadeFactor = $minFadeFactor;
	}
	subscribeToEvent(%this, "onUpdateFrame");
	%this.fadeDelta = %delta;
	return;
}
function AudioChannel::fadeOut(%this, %delta)
{
	%this.turnOffCounter = turnOffCounter + 1.0;
	if (%this.isFadingIn())
	{
		triggerEvent("onAudioChannelFadeInFinished", channelNumber);
	}
	else
	{
		%this.fadeFactor = "1";
	}
	subscribeToEvent(%this, "onUpdateFrame");
	%this.fadeDelta = -1.0 * %delta;
	return;
}
function AudioChannel::stopFade(%this)
{
	unSubscribeFromEvent(%this, "onUpdateFrame");
	%this.fadeDelta = "0";
	return;
}
function AudioChannel::isFadingOut(%this)
{
	return fadeDelta < 0.0;
	return fadeDelta < 0.0;
}
function AudioChannel::isFadingIn(%this)
{
	return fadeDelta > 0.0;
	return fadeDelta > 0.0;
}
function AudioChannel::onUpdateFrame(%this)
{
	%this.fadeFactor = fadeFactor + fadeDelta * 0.01600000075995922;
	if (fadeFactor > 1.0 || fadeFactor < $minFadeFactor)
	{
		%this.fadeDelta = "0";
		unSubscribeFromEvent(%this, "onUpdateFrame");
	}
	if (fadeFactor > 1.0)
	{
		%this.fadeFactor = "1";
		triggerEvent("onAudioChannelFadeInFinished", channelNumber);
	}
	else
	{
		if (fadeFactor < $minFadeFactor)
		{
			%this.fadeFactor = $minFadeFactor;
			triggerEvent("onAudioChannelFadeOutFinished", channelNumber);
		}
	}
	%this.updateVolume();
	return;
}
function getAudioChannelObject(%audioProfile)
{
	if (Type == 3.0)
	{
		return $MusicAudioChannel["1"];
	}
	else
	{
		if (Type == 4.0)
		{
			return $MusicAudioChannel["2"];
			break;
		}
		if (Type == 5.0)
		{
			return $MusicAudioChannel["3"];
			break;
		}
		debugWarn("no channel object found for audioProfile" SPC %audioProfile);
	}
	return;
}
function fadeAllMusicAudioChannels(%in)
{
	%i = 0;
	while (%i < $AllAudioChannels.getCount())
	{
		%channel = $AllAudioChannels.getObject(%i);
		if (%channel.getId() == $MenuAudioChannel.getId() || %channel.getId() == $FXAudioChannel.getId())
		{
		}
		else
		{
			if (%in)
			{
				%channel.fadeIn("0.5");
				break;
			}
			%channel.fadeOut("1");
		}
		%i = %i + 1.0;
	}
	return $AllAudioChannels.getCount();
}
function muteAllGameAudioChannels(%mute)
{
	%i = 0;
	while (%i < $AllAudioChannels.getCount())
	{
		%channel = $AllAudioChannels.getObject(%i);
		if (%channel.getId() == $MenuAudioChannel.getId())
		{
		}
		else
		{
			if (%mute)
			{
				%channel.setVolume("0");
				break;
			}
			if (%channel.getId() == $FXAudioChannel.getId())
			{
				%channel.setVolume($settings::Audio::Effects);
				break;
			}
			%channel.setVolume($settings::Audio::Music);
		}
		%i = %i + 1.0;
	}
	return $AllAudioChannels.getCount();
}
