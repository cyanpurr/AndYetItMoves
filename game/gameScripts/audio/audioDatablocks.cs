// audioDatablocks.cs.dso
$audioPreloadFlag = 1;
if (!($soundEnabled))
{
	return;
}
$enableStreaming = 1;
new AudioDescription(Name : FXLoop)
{
	volume = "1";
	isLooping = "1";
	is3D = "0";
	Type = channelNumber;
	isStreaming = "0";
}
new AudioDescription(Name : FXLoopStream)
{
	volume = "1";
	isLooping = "1";
	is3D = "0";
	Type = channelNumber;
	isStreaming = $enableStreaming;
}
new AudioDescription(Name : FXOneshot)
{
	volume = "1";
	isLooping = "0";
	is3D = "0";
	Type = channelNumber;
	isStreaming = "0";
}
new AudioDescription(Name : FXOneshotStream)
{
	volume = "1";
	isLooping = "0";
	is3D = "0";
	Type = channelNumber;
	isStreaming = $enableStreaming;
}
new AudioDescription(Name : FXOneshotMote)
{
	volume = "1";
	isLooping = "0";
	is3D = "0";
	Type = channelNumber;
	isStreaming = "0";
	wiimote = "0";
}
new AudioDescription(Name : MusicLoop)
{
	volume = "1";
	isLooping = "1";
	is3D = "0";
	Type = channelNumber;
	isStreaming = "0";
}
new AudioDescription(Name : MusicLoopStream)
{
	volume = "1";
	isLooping = "1";
	is3D = "0";
	Type = channelNumber;
	isStreaming = $enableStreaming;
}
new AudioDescription(Name : MusicOneshot)
{
	volume = "1";
	isLooping = "0";
	is3D = "0";
	Type = channelNumber;
	isStreaming = "0";
}
new AudioDescription(Name : MusicOneshotStream)
{
	volume = "1";
	isLooping = "0";
	is3D = "0";
	Type = channelNumber;
	isStreaming = $enableStreaming;
}
new AudioDescription(Name : MusicLoop2)
{
	volume = "1";
	isLooping = "1";
	is3D = "0";
	Type = channelNumber;
	isStreaming = "0";
}
new AudioDescription(Name : MusicLoopStream2)
{
	volume = "1";
	isLooping = "1";
	is3D = "0";
	Type = channelNumber;
	isStreaming = $enableStreaming;
}
new AudioDescription(Name : MusicOneshot2)
{
	volume = "1";
	isLooping = "0";
	is3D = "0";
	Type = channelNumber;
	isStreaming = "0";
}
new AudioDescription(Name : MusicOneshotStream2)
{
	volume = "1";
	isLooping = "0";
	is3D = "0";
	Type = channelNumber;
	isStreaming = $enableStreaming;
}
new AudioDescription(Name : MusicLoopStream3)
{
	volume = "1";
	isLooping = "1";
	is3D = "0";
	Type = channelNumber;
	isStreaming = $enableStreaming;
}
new AudioDescription(Name : MusicLoop3)
{
	volume = "1";
	isLooping = "1";
	is3D = "0";
	Type = channelNumber;
	isStreaming = "0";
}
new AudioDescription(Name : MusicLoopStream4)
{
	volume = "1";
	isLooping = "1";
	is3D = "0";
	Type = channelNumber;
	isStreaming = $enableStreaming;
}
new AudioDescription(Name : MusicLoop4)
{
	volume = "1";
	isLooping = "1";
	is3D = "0";
	Type = channelNumber;
	isStreaming = "0";
}
new AudioDescription(Name : MenuLoop)
{
	volume = "1";
	isLooping = "1";
	is3D = "0";
	Type = channelNumber;
	isStreaming = "0";
}
new AudioDescription(Name : MenuLoopStream)
{
	volume = "1";
	isLooping = "1";
	is3D = "0";
	Type = channelNumber;
	isStreaming = "1";
}
new AudioDescription(Name : MenuOneshot)
{
	volume = "1";
	isLooping = "0";
	is3D = "0";
	Type = channelNumber;
	isStreaming = $enableStreaming;
}
function loadCommonAudioProfiles()
{
	if ($WII)
	{
		if (isObject(PlayerDeath1))
		{
			PlayerDeath1.delete();
		}
		new AudioProfile(Name : PlayerDeath1)
		{
			fileName = ['"~/"', '$DataFolder', '"/audio/player/zrtschNew1.ogg"'];
			description = "FXOneshot";
			preload = $audioPreloadFlag;
		}
		if (isObject(PlayerEdgeDeathNew))
		{
			PlayerEdgeDeathNew.delete();
		}
		new AudioProfile(Name : PlayerEdgeDeathNew)
		{
			fileName = ['"~/"', '$DataFolder', '"/audio/player/edgeDeathNew1.ogg"'];
			description = "FXOneshot";
			preload = $audioPreloadFlag;
		}
		if (isObject(SpawnPointNew))
		{
			SpawnPointNew.delete();
		}
		new AudioProfile(Name : SpawnPointNew)
		{
			fileName = ['"~/"', '$DataFolder', '"/audio/metagame/spawnpointNew1.ogg"'];
			description = "FXOneshot";
			preload = $audioPreloadFlag;
		}
		if (isObject(LevelFinishedNew))
		{
			LevelFinishedNew.delete();
		}
		new AudioProfile(Name : LevelFinishedNew)
		{
			fileName = ['"~/"', '$DataFolder', '"/audio/metagame/levelfinishedNew4.ogg"'];
			description = "FXOneshotStream";
			preload = $audioPreloadFlag;
		}
		if ($footStepSoundFileName $= "")
		{
			return;
		}
		if (isObject(Footsteps))
		{
			Footsteps.delete();
		}
		new AudioProfile(Name : Footsteps)
		{
			fileName = ['"~/"', '$DataFolder', '"/audio/player/"', '$footStepSoundFileName'];
			description = "FXOneshot";
			preload = $audioPreloadFlag;
		}
		if (isObject(PlayerJump))
		{
			PlayerJump.delete();
		}
		new AudioProfile(Name : PlayerJump)
		{
			fileName = ['"~/"', '$DataFolder', '"/audio/player/"', '$jumpSoundFileName'];
			description = "FXOneshot";
			preload = $audioPreloadFlag;
		}
	}
	return;
}
loadCommonAudioProfiles();
if ($WII)
{
	new AudioProfile(Name : MenuMusic)
	{
		fileName = ['"~/"', '$DataFolder', '"/audio/music/ayimMenu01.ogg"'];
		description = "MenuLoopStream";
		preload = "0";
	}
}
else
{
	new AudioProfile(Name : PlayerDeath1)
	{
		fileName = ['"~/"', '$DataFolder', '"/audio/player/zrtschNew1.ogg"'];
		description = "FXOneshot";
		preload = $audioPreloadFlag;
	}
	new AudioProfile(Name : PlayerEdgeDeathNew)
	{
		fileName = ['"~/"', '$DataFolder', '"/audio/player/edgeDeathNew1.ogg"'];
		description = "FXOneshot";
		preload = $audioPreloadFlag;
	}
	new AudioProfile(Name : SpawnPointNew)
	{
		fileName = ['"~/"', '$DataFolder', '"/audio/metagame/spawnpointNew1.ogg"'];
		description = "FXOneshot";
		preload = $audioPreloadFlag;
	}
	new AudioProfile(Name : LevelFinishedNew)
	{
		fileName = ['"~/"', '$DataFolder', '"/audio/metagame/levelfinishedNew4.ogg"'];
		description = "FXOneshot";
		preload = $audioPreloadFlag;
	}
	new AudioProfile(Name : MenuMusic)
	{
		fileName = ['"~/"', '$DataFolder', '"/audio/music/ayimMenu01.ogg"'];
		description = "MenuLoop";
		preload = $audioPreloadFlag;
	}
}
