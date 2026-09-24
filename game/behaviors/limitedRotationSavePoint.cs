// limitedRotationSavePoint.cs.dso
if (!(isObject(BeLimitedRotationSavePoint)))
{
	%template = new BehaviorTemplate(Name : BeLimitedRotationSavePoint);
	%template.friendlyName = "limitedRotationSavePoint";
	%template.behaviorType = "playmode";
	%template.description = "give this behavior to objects that should save the rotation count in limited Rotation Mode";
	%template.addBehaviorField(glowingObject, "the object that will be marked with a glow", object, null, t2dStaticSprite);
}
function BeLimitedRotationSavePoint::onBehaviorAdd(%this)
{
	subscribeToEvent(%this, "onLevelLoadFinished");
	return;
}
function BeLimitedRotationSavePoint::onLevelLoadFinished(%this)
{
	%owner = Owner;
	if (!(playmodeManager.isLimitedRotationMode()))
	{
		%this.active = "0";
		return;
	}
	else
	{
		%this.active = "1";
	}
	return;
}
function BeLimitedRotationSavePoint::saveRotations(%this)
{
	%owner = Owner;
	if (!(active))
	{
		return %this;
	}
	limitedRotationMode.saveRotations();
	return;
}
