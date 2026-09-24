// controlPlayerSounds.cs.dso
if (!(isObject(BeControlPlayerSounds)))
{
	%template = new BehaviorTemplate(Name : BeControlPlayerSounds);
	%template.friendlyName = "control player sounds";
	%template.behaviorType = "Audio";
	%template.description = "controls the sounds of the player, using events from the player-statemachine";
}
function BeControlPlayerSounds::onBehaviorAdd(%this)
{
	%this.profileNameFootsteps = "Footsteps";
	%this.profileNameJump = "PlayerJump";
	if ($WII)
	{
		%this.maxVolumeFootsteps = $footStepSoundVolume;
		%this.maxVolumeJump = $jumpSoundVolume;
	}
	else
	{
		%this.maxVolumeFootsteps = volume;
		%this.maxVolumeJump = volume;
	}
	%this.profileNameCrushdeath = "PlayerDeath1";
	%this.maxVolumeCrushdeath = "0.5";
	%this.profileNameEdgedeath = "PlayerEdgeDeathNew";
	%this.maxVolumeEdgedeath = "0.2";
	return;
}
function BeControlPlayerSounds::onAddToScene(%this)
{
	%owner = Owner;
	subscribeToEvents(%this, "onPlayerDelayedJump");
	subscribeToEvents(%this, "onDieFragged onDieOutside onPlayerDeath onRotationStart onRotationFinish onEnterLevelSwitch onPauseGame");
	return;
}
function BeControlPlayerSounds::onPlayerDelayedJump(%this)
{
	%this.stopFootstepSound();
	return;
}
function BeControlPlayerSounds::onDieFragged(%this)
{
	%this.playCrushDeathSound();
	return;
}
function BeControlPlayerSounds::onDieOutside(%this)
{
	%this.playEdgeDeathSound();
	return;
}
function BeControlPlayerSounds::onPlayerDeath(%this)
{
	%this.stopFootstepSound();
	return;
}
function BeControlPlayerSounds::onRotationStart(%this)
{
	%this.stopFootstepSound();
	return;
}
function BeControlPlayerSounds::onEnterLevelSwitch(%this)
{
	%this.stopFootstepSound();
	return;
}
function BeControlPlayerSounds::onRotationFinish(%this)
{
	if (player.getState() $= "run" && !(alxIsPlaying(footstepSound)))
	{
		%this.playFootstepSound();
	}
	return;
}
function BeControlPlayerSounds::playFootstepSound(%this)
{
	%this.footstepSound = alxPlay(profileNameFootsteps);
	alxSourcef(footstepSound, AL_GAIN, maxVolumeFootsteps);
	return;
}
function BeControlPlayerSounds::stopFootstepSound(%this)
{
	alxStop(footstepSound);
	return;
}
function BeControlPlayerSounds::playJumpSound(%this)
{
	%this.jumpSound = alxPlay(profileNameJump);
	alxSourcef(jumpSound, AL_GAIN, maxVolumeJump);
	return;
}
function BeControlPlayerSounds::stopJumpSound(%this)
{
	alxStop(jumpSound);
	return;
}
function BeControlPlayerSounds::playCrushDeathSound(%this)
{
	%this.crushDeathSound = alxPlay(profileNameCrushdeath);
	alxSourcef(crushDeathSound, AL_GAIN, maxVolumeCrushdeath);
	return;
}
function BeControlPlayerSounds::stopCrushDeathSound()
{
	alxStop(crushDeathSound);
	return;
}
function BeControlPlayerSounds::playEdgeDeathSound(%this)
{
	%this.edgeDeathSound = alxPlay(profileNameEdgedeath);
	alxSourcef(edgeDeathSound, AL_GAIN, maxVolumeEdgedeath);
	return;
}
function BeControlPlayerSounds::stopEdgeDeathSound(%this)
{
	alxStop(edgeDeathSound);
	return;
}
function BeControlPlayerSounds::onPauseGame(%this)
{
	%this.stopFootstepSound();
	return;
}
