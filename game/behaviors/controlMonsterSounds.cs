// controlMonsterSounds.cs.dso
if (!(isObject(BeControlMonsterSounds)))
{
	%template = new BehaviorTemplate(Name : BeControlMonsterSounds);
	%template.friendlyName = "control Monster sounds";
	%template.behaviorType = "Audio";
	%template.description = "controls the sounds of the Monster, using events from the Monster-statemachine";
}
function BeControlMonsterSounds::onBehaviorAdd(%this)
{
	subscribeToEvents(%this, "onLevelLoadFinished");
	return;
}
function BeControlMonsterSounds::onLevelLoadFinished(%this)
{
	%this.BePlayLoopingSound = Owner.getBehavior("BePlayLoopingSound");
	%this.profileNameWalk = "MonsterWalk";
	%this.maxVolumeWalk = "0.1";
	%this.profileNameRun = "MonsterRun";
	%this.maxVolumeRun = "0.1";
	%this.profileNameRodeo = "MonsterRodeo";
	%this.maxVolumeRodeo = "1";
	%this.profileNameFall = "MonsterFall";
	%this.maxVolumeFall = "0.3";
	%this.profileNameStumbled = "MonsterStumbled";
	%this.maxVolumeStumbled = "1";
	return;
}
function BeControlMonsterSounds::onAddToScene(%this)
{
	%owner = Owner;
	subscribeToEvents(%this, "onMonsterStateChangewalk onMonsterStateChangerun onMonsterStateChangefall onMonsterStateChangestumbled");
	return;
}
function BeControlMonsterSounds::onMonsterStateChangewalk(%this)
{
	BePlayLoopingSound.stop();
	BePlayLoopingSound.play();
	return;
}
function BeControlMonsterSounds::onMonsterStateChangerun(%this)
{
	return;
}
function BeControlMonsterSounds::onMonsterStateChangefall(%this)
{
	BePlayLoopingSound.stop();
	%handle = alxPlay(profileNameFall);
	alxSourcef(%handle, AL_GAIN, maxVolumeFall * calcDistanceFactor(Owner));
	debugEcho("onMonsterStateChangefall");
	return;
}
function BeControlMonsterSounds::onMonsterStateChangestumbled(%this)
{
	BePlayLoopingSound.stop();
	%handle = alxPlay(profileNameStumbled);
	alxSourcef(%handle, AL_GAIN, maxVolumeStumbled * calcDistanceFactor(Owner));
	return;
}
function BeControlMonsterSounds::onBehaviorRemove(%this)
{
	BePlayLoopingSound.stop();
	return;
}
