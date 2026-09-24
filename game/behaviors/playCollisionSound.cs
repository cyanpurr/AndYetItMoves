// playCollisionSound.cs.dso
if (!(isObject(BePlayCollisionSound)))
{
	if (!(isTorquePlayer()))
	{
		%template = new BehaviorTemplate(Name : BePlayCollisionSound);
	}
	else
	{
		%template = new BePlayCollisionSoundTemplate(Name : BePlayCollisionSound);
	}
	%template.friendlyName = "PlayCollisionSound";
	%template.behaviorType = "Audio";
	%template.description = "plays a collision sound";
	if (!(isTorquePlayer()))
	{
		%template.addBehaviorField(minSoundSpeed, "how hard the collision has to be to start a sound", float, "40");
		%template.addBehaviorField(AudioProfile, "the audio profile(s) to play: a vector of 1 or more profiles, if more, one will be randomly chosen", string, "");
		%template.addBehaviorField(keepOrder, "if more than 1 profiles is given, keepOrder will let them play in the same order as supplied, no randomness will be applied", boolean, "0");
		%template.addBehaviorField(volume, "the volume of the sound", float, "1");
	}
}
function BePlayCollisionSound::onBehaviorAdd(%this)
{
	if (keepOrder)
	{
		%this.profileCount = getWordCount(AudioProfile);
		%this.currentIndex = "0";
	}
	%this.setBehaviorCollisionCallback("1");
	return;
}
function BePlayCollisionSound::stop(%this)
{
	if (alxIsPlaying(handle))
	{
		alxStop(handle);
	}
	return;
}
function BePlayCollisionSound::switchOn(%this)
{
	%this.setBehaviorCollisionCallback("1");
	return;
}
function BePlayCollisionSound::switchOff(%this)
{
	%this.setBehaviorCollisionCallback("0");
	return;
}
