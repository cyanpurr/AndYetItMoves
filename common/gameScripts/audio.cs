// audio.cs.dso
$musicAudioType = 1;
new AudioDescription(Name : AudioChannel1)
{
	volume = "1";
	isLooping = "0";
	is3D = "0";
	Type = "1";
}
$effectsAudioType = 2;
new AudioDescription(Name : AudioChannel2)
{
	volume = "1";
	isLooping = "0";
	is3D = "0";
	Type = "2";
}
function initializeOpenAL()
{
	shutdownOpenAL();
	echo("OpenAL Driver Init:");
	if ($pref::Audio::driver $= "OpenAL")
	{
		if (!(OpenALInitDriver()))
		{
			error("   Failed to initialize driver.");
			$Audio::initFailed = 1;
		}
		else
		{
			echo("   Vendor: " @ alGetString("AL_VENDOR"));
			echo("   Version: " @ alGetString("AL_VERSION"));
			echo("   Renderer: " @ alGetString("AL_RENDERER"));
			echo("   Extensions: " @ alGetString("AL_EXTENSIONS"));
			alxListenerf(AL_GAIN_LINEAR, $pref::Audio::masterVolume);
			%channel = 1;
			while (%channel <= 8.0)
			{
				alxSetChannelVolume(%channel, $pref::Audio::channelVolume[%channel]);
				%channel = %channel + 1.0;
			}
			echo("");
		}
	}
	else
	{
		error("   Failed to initialize audio system. Invalid driver.");
	}
	return;
}
function shutdownOpenAL()
{
	OpenALShutdownDriver();
	return;
}
