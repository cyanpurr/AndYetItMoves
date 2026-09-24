// randomRotationFade.cs.dso
if (!(isObject(BeRandomRotationFade)))
{
	%template = new BehaviorTemplate(Name : BeRandomRotationFade);
	%template.friendlyName = "RandomRotationFade";
	%template.behaviorType = "LevelTrip";
	%template.description = "randomizes rotations of a fadeOnRotation object after each cam rotation";
}
function BeRandomRotationFade::onBehaviorAdd(%this)
{
	return;
}
function BeRandomRotationFade::onRotationFinish(%this)
{
	%fadeBehavior = Owner.getBehavior("BeFadeOnRotation");
	if (!(fadeOutOnSameRotation))
	{
		%fadeBehavior.fadeOutOnSameRotation = "1";
		%fadeBehavior.faderRotation = "180";
	}
	%fadeBehavior.faderRotation2 = getRandom("0", "3") * 90.0;
	return;
}
function BeRandomRotationFade::switchOn(%this)
{
	subscribeToEvents(%this, "onRotationFinish");
	return;
}
