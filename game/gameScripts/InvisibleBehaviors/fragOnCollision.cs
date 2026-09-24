// fragOnCollision.cs.dso
if (!(isObject(BeFragOnCollision)))
{
	%template = new BeFragOnCollisionTemplate(Name : BeFragOnCollision);
	%template.friendlyName = "FragOnCollision";
	%template.behaviorType = "GameplayMechanisms";
	%template.description = "on a collision with downward speed it will frag into a particle effect";
}
function BeFragOnCollision::onBehaviorAdd(%this)
{
	%this.setBehaviorCollisionCallback("1");
	%this.viewSafeDeleteSchedule = safeSchedule("2000", Owner, "viewSafeDelete", "1.5");
	return;
}
function BeFragOnCollision::onBehaviorRemove(%this)
{
	%this.setBehaviorCollisionCallback("0");
	if (isObject(viewSafeDeleteSchedule))
	{
		viewSafeDeleteSchedule.cancelSchedule();
	}
	return;
}
function BeFragOnCollision::createParticle(%this)
{
	%this.effect = new t2dParticleEffect(Name : "")
	{
		scenegraph = scenegraph;
	}
	return;
}
