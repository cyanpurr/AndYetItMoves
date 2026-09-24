// audio.cs.dso
function t2dSceneObject::audioSafeDelete(%this)
{
	if (!(isObject(soundobject)))
	{
		%this.safeDelete();
		return;
	}
	if (soundobject.isPlaying())
	{
		%this.schedule("300", "audioSafeDelete");
	}
	else
	{
		%this.safeDelete();
	}
	return;
}
function t2dSceneObject::addCollisionSoundBehavior(%this, %minSoundSpeed, %audioProfile, %keepOrder, %volume)
{
	%behavior = %this.addDependentBehavior("BePlayCollisionSound");
	%behavior.minSoundSpeed = %minSoundSpeed;
	%behavior.AudioProfile = %audioProfile;
	%behavior.keepOrder = %keepOrder;
	%behavior.volume = %volume;
	return;
}
function calcAverageDistanceFactor(%objectList)
{
	%objectCount = getWordCount(%objectList);
	%distanceFactor = 1;
	%i = 0;
	while (%i < %objectCount)
	{
		%distanceFactor = %distanceFactor + calcDistanceFactor(getWord(%objectList, %i));
		%i = %i + 1.0;
	}
	return %distanceFactor / %objectCount;
	return %distanceFactor / %objectCount;
}
