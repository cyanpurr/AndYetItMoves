// menuSound.cs.dso
function startMenuMusic()
{
	muteAllGameAudioChannels("1");
	if ($dontplaymenusound)
	{
		return;
	}
	if (!(alxIsPlaying($menuMusic)) && $settings::Audio::Music != 0.0)
	{
		$menuMusic = alxPlay(MenuMusic);
		alxSourcef($menuMusic, AL_GAIN, "0.5");
	}
	return;
}
function stopMenuMusic()
{
	muteAllGameAudioChannels("0");
	alxStop($menuMusic);
	return;
}
