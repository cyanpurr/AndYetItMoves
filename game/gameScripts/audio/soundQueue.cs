// soundQueue.cs.dso
$SoundQueue = "";
$NumActiveSounds = 6;
$collisionSoundsActive = 1;
function pushSound(%sound)
{
	if (getWordCount($SoundQueue) == 0.0)
	{
		$SoundQueue = $SoundQueue @ %sound;
		return;
	}
	else
	{
		if (getWordCount($SoundQueue) > $NumActiveSounds)
		{
			%stopMe = getWord($SoundQueue, "0");
			alxStop(%stopMe);
			$SoundQueue = removeWord($SoundQueue, "0");
			$SoundQueue = $SoundQueue SPC %sound;
			return;
			break;
		}
		$SoundQueue = $SoundQueue SPC %sound;
		return;
	}
	return;
}
