// breakIntoPieces.cs.dso
if (!(isObject(BeBreakIntoPieces)))
{
	if (!(isTorquePlayer()))
	{
		%template = new BehaviorTemplate(Name : BeBreakIntoPieces);
	}
	else
	{
		%template = new BeBreakIntoPiecesTemplate(Name : BeBreakIntoPieces);
	}
	%template.friendlyName = "BreakIntoPieces";
	%template.behaviorType = "LevelPrototype";
	%template.description = "onCollision this object will break apart and its parts will not collide with colliding objects anymore";
	if (!(isTorquePlayer()))
	{
		%template.addBehaviorField(numberOfPieces, "in how many pieces it will break", integer, "2");
		%template.addBehaviorField(breakSpeed, "the speed onCollision at which the object should break apart ", float, "50");
	}
}
function BeBreakIntoPieces::onBehaviorAdd(%this)
{
	%owner = Owner;
	%this.setBehaviorCollisionCallback("1");
	%this.viewSafeDeleteSchedule = safeSchedule("2000", %owner, "viewSafeDelete", "1.5");
	return;
}
function BeBreakIntoPieces::onBehaviorRemove(%this)
{
	if (isObject(viewSafeDeleteSchedule))
	{
		viewSafeDeleteSchedule.cancelSchedule();
	}
	if (isObject(spawner))
	{
		spawner.objectsAlive = objectsAlive - 1.0;
	}
	return spawner;
}
