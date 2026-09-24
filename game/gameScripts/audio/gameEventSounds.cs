// gameEventSounds.cs.dso
$globalSoundHandles = 0;
function playEventSound(%profile, %gain)
{
	%handle = alxPlay(%profile);
	alxSourcef(%handle, AL_GAIN, %gain);
	return %handle;
	return %handle;
}
function playDistanceEventSound(%object, %profile, %gain)
{
	%factor = calcDistanceFactor(%object);
	if (%factor == 0.0)
	{
		return;
	}
	%handle = alxPlay(%profile);
	alxSourcef(%handle, AL_GAIN, %gain * %factor);
	return %handle;
	return %handle;
}
function playHandleEventSound(%handleName, %profile, %gain, %force)
{
	if (alxIsPlaying($globalSoundHandles[%handleName]))
	{
		if (!(%force))
		{
			return $globalSoundHandles[%handleName];
			break;
		}
		alxStop($globalSoundHandles[%handleName]);
	}
	$globalSoundHandles[%handleName] = alxPlay(%profile);
	alxSourcef($globalSoundHandles[%handleName], AL_GAIN, %gain);
	return $globalSoundHandles[%handleName];
	return $globalSoundHandles[%handleName];
}
function playDistanceHandleEventSound(%object, %handleName, %profile, %gain, %force)
{
	if (alxIsPlaying($globalSoundHandles[%handleName]) && !(%force))
	{
		return $globalSoundHandles[%handleName];
	}
	%realGain = calcDistanceFactor(%object) * %gain;
	if (%realGain == 0.0)
	{
		return "0";
	}
	return playHandleEventSound(%handleName, %profile, %realGain, %force);
	return playHandleEventSound(%handleName, %profile, %realGain, %force);
}
function playGameEventSound(%owner, %action, )
{
	if (%action $= "FlyingBats")
	{
		%distanceFactor = calcAverageDistanceFactor(%owner);
		if (!(alxIsPlaying($FlyingBats)))
		{
			alxStop($FlyingBats);
		}
		$FlyingBats = alxPlay(BatsFlying);
		alxSourcef($FlyingBats, AL_GAIN, 0.20000000298023224 * %distanceFactor);
	}
	else
	{
		if (%action $= "StoneSmash")
		{
			%factor = calcDistanceFactor(%owner);
			$StoneSmash = alxPlay(getRandomWord("StoneSmash3 StoneSmash4"));
			pushSound($StoneSmash);
			alxSourcef($StoneSmash, AL_GAIN, 1.0 * %factor);
			break;
		}
		if (%action $= "FadeOnRotation")
		{
			$fadeOnRotationFactor = 0;
			%i = 0;
			while (%i < $allFadeonRotations.getCount())
			{
				%currentFactor = calcDistanceFactor(Owner);
				if (%currentFactor > $fadeOnRotationFactor)
				{
					$fadeOnRotationFactor = %currentFactor;
				}
				%i = %i + 1.0;
			}
			if ($fadeOnRotationFactor == 0.0)
			{
				return $allFadeonRotations.getCount();
			}
			if (alxIsPlaying($FadeOnRotation))
			{
				alxStop($FadeOnRotation);
			}
			$FadeOnRotation = alxPlay(fadeOnRotationProfile);
			alxSourcef($FadeOnRotation, AL_GAIN, $fadeOnRotationFactor * 0.4000000059604645);
		}
	}
	return;
}
