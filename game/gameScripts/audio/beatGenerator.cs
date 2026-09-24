// beatGenerator.cs.dso
$beatNumber = 0;
function BeatGenerator::onLevelLoaded(%this)
{
	subscribeToEvents(%this, "onLevelLoadFinished10 onLevelLoadFinished onLevelShutdown onPauseGame");
	return;
}
function BeatGenerator::onLevelLoadFinished10(%this)
{
	%this.isRunning = "0";
	%this.secondsPerBeat = 1.0 / bpm / 60.0 / 4.0;
	%this.triggerEvents = "1";
	$currentBeatLength = secondsPerBeat;
	$fadingOut = 0;
	$startVolume = 0.4000000059604645;
	return;
}
function BeatGenerator::onLevelLoadFinished(%this)
{
	if (autoPlay)
	{
		%this.start();
	}
	return;
}
function BeatGenerator::start(%this)
{
	if (isRunning)
	{
		return %this;
	}
	triggerEvent("onBeatStarted");
	%this.isRunning = "1";
	%this.startBeat();
	return;
}
function BeatGenerator::onPauseGame(%this)
{
	if (isRunning)
	{
		alxStop($beat);
		%this.hasPaused = "1";
	}
	return;
}
function BeatGenerator::startBeat(%this, )
{
	$volume = $startVolume;
	$beatSelecta = 0;
	%this.nextBeat();
	return;
}
function BeatGenerator::nextBeat(%this)
{
	%this.beatScheduleId = %this.schedule(secondsPerBeat * 1000.0, "nextBeat");
	if ($beatNumber == 0.0)
	{
		$previousBeatNumber = 4;
	}
	else
	{
		$previousBeatNumber = $beatNumber;
	}
	$beatNumber = modulo($beatNumber, "4") + 1.0;
	%beatpartcount = getWordCount($currentBeatSequence) * 4.0;
	if ($beatSelecta >= %beatpartcount)
	{
		$beatSelecta = modulo($beatSelecta, "4");
		if (%beatpartcount > 11.0)
		{
			$beatSelecta = $beatSelecta + 4.0;
		}
	}
	debugEcho("selecting beat" SPC $beatNumber SPC "|" SPC $beatSelecta SPC "|" SPC modulo($beatSelecta, "4") SPC "|" SPC startMusic);
	%isFirstBeat = modulo($beatSelecta, "4") == 0.0;
	if (startMusic && %isFirstBeat || !(alxIsPlaying($beat)))
	{
		if (%isFirstBeat && hasPaused)
		{
			alxStop($beat);
			%this.hasPaused = "0";
		}
		%beatIndex = mFloor($beatSelecta / 4.0 + 0.0010000000474974513);
		$beat = alxPlay(getWord($currentBeatSequence, %beatIndex));
		alxSourcef($beat, AL_GAIN, $volume);
	}
	if (triggerEvents)
	{
		triggerEvent("onBeat");
		triggerEvent("onBeatNumber" @ $beatNumber);
	}
	$beatSelecta = $beatSelecta + 1.0;
	return;
}
function BeatGenerator::stop(%this)
{
	echo("stopping BeatGenerator with id: " @ beatScheduleId);
	cancel(beatScheduleId);
	%this.isRunning = "0";
	$beatNumber = 0;
	return;
}
function BeatGenerator::fadeoutAudio(%this)
{
	$fadingOut = 1;
	if (decrease $= "")
	{
		%this.decrease = $volume / 20.0;
	}
	$volume = $volume - decrease;
	alxSourcef($beat, AL_GAIN, $volume);
	if ($volume <= 0.0)
	{
		%this.stop();
		if (!($ambientSoundIsRunning))
		{
			startAllEnvironmentSounds();
		}
		$fadingOut = 0;
		return;
	}
	else
	{
		%this.fadeoutScheduleId = %this.schedule("500", "fadeOutAudio");
	}
	return;
}
function BeatGenerator::stopFadeOut(%this)
{
	cancel(fadeoutScheduleId);
	$volume = $startVolume;
	$fadingOut = 0;
	return;
}
function BeatGenerator::onLevelShutdown(%this)
{
	$beatNumber = 0;
	return;
}
function BeatGeneratorTrigger::onLevelLoaded(%this)
{
	subscribeToEvents(%this, "onLevelLoadFinished");
	return;
}
function BeatGeneratorTrigger::onEnter(%this, )
{
	if (action $= "start")
	{
		if (startMusic != "")
		{
			currentBeatName.startMusic = startMusic;
		}
		if (startEvents != "")
		{
			currentBeatName.triggerEvents = startEvents;
		}
		if (!(isRunning))
		{
			$currentBeatSequence = profiles;
			if (volume != "")
			{
				$startVolume = volume;
			}
			currentBeatName.start();
			stopAllEnvironmentSounds();
		}
		else
		{
			if ($fadingOut)
			{
				currentBeatName.stopFadeOut();
			}
			$currentBeatSequence = profiles;
			subscribeToEvents(%this, "onPlayerReanimate");
		}
	}
	else
	{
		if (action $= "fadeOut" || action $= "stop")
		{
			if (isRunning)
			{
				if (isFalling)
				{
					subscribeToEvents(%this, "onPlayerLanded onPlayerReanimate");
					break;
				}
				%this.onPlayerLanded();
			}
		}
	}
	return;
}
function BeatGeneratorTrigger::onPlayerLanded(%this)
{
	unSubscribeFromEvents(%this, "onPlayerLanded onPlayerDeath");
	if (action $= "fadeOut")
	{
		currentBeatName.fadeoutAudio();
	}
	else
	{
		if (action $= "stop")
		{
			currentBeatName.stop();
			startAllEnvironmentSounds();
			break;
		}
	}
	return;
}
function BeatGeneratorTrigger::onPlayerReanimate(%this)
{
	unSubscribeFromEvents(%this, "onPlayerLanded onPlayerReanimate");
	if (action $= "start")
	{
		if (nextPlatform != "")
		{
			if (modulo($previousBeatNumber, "4") == nextPlatform - 1.0)
			{
				$beatNumber = nextPlatform;
				break;
			}
			if (nextPlatform == 1.0)
			{
				$beatNumber = 4;
				break;
			}
			$beatNumber = nextPlatform - 1.0;
		}
	}
	return %this;
}
