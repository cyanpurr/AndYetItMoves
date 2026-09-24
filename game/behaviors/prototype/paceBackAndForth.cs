// paceBackAndForth.cs.dso
if (!(isObject(BePaceBackAndForth)))
{
	%template = new BehaviorTemplate(Name : BePaceBackAndForth);
	%template.friendlyName = "PaceBackAndForth";
	%template.behaviorType = "Positioning";
	%template.description = "lets this object walk in one direction until it collides with something and then walk back";
	%template.addBehaviorField(maxVelocity, "maximum speed it will move", float, "100");
	%template.addBehaviorField(accelFactor, "how fast the object should accelerate", float, "30");
	%template.addBehaviorField(autoPlay, "if the object should start moving right away", bool, "0");
	%template.addBehaviorField(CollisionGroups, "a space sep. list of groups the object will change direction if it collides with", string, "player moving collide");
	%template.addBehaviorField(turnAroundRightAway, "if true object woll not have a breaking phase", bool, "0");
}
function BePaceBackAndForth::onBehaviorAdd(%this)
{
	subscribeToEvents(%this, "onLevelLoadFinished2 onLevelLoadFinished10 onRotationStart onRotationFinish");
	return;
}
function BePaceBackAndForth::onLevelLoadFinished2(%this)
{
	%owner = Owner;
	%movingBehavior = %owner.addDependentBehavior("BeMoving");
	%movingBehavior.CollisionGroups = CollisionGroups;
	%movingBehavior.Friction = "0";
	return;
}
function BePaceBackAndForth::onLevelLoadFinished10(%this)
{
	%owner = Owner;
	%owner.addCollisionGroups(CollisionGroups);
	if (autoPlay)
	{
		%this.switchOn();
	}
	unSubscribeFromEvent(%this, "onLevelLoadFinished10");
	return;
}
function BePaceBackAndForth::switchOn(%this)
{
	%owner = Owner;
	subscribeToEvent(%this, "onUpdateTick10");
	Owner.setImmovable("0");
	%this.setBehaviorCollisionCallback("1");
	if (getRandom() > 0.5)
	{
	}
	else
	{
	}
	%this.direction = -1.0;
	%owner.setFlipX(direction + 1.0);
	return;
}
function BePaceBackAndForth::switchOff(%this)
{
	%this.setBehaviorCollisionCallback("0");
	unSubscribeFromEvent(%this, "onUpdateTick10");
	return;
}
function BePaceBackAndForth::onRotationStart(%this)
{
	unSubscribeFromEvent(%this, "onUpdateTick10");
	return;
}
function BePaceBackAndForth::onRotationFinish(%this)
{
	subscribeToEvent(%this, "onUpdateTick10");
	return;
}
function BePaceBackAndForth::onUpdateTick10(%this)
{
	%owner = Owner;
	%curVel = %owner.getLinearVelocity();
	if (collisionSwitchedOff && getSign(getHorizontalComponent(%curVel)) == direction && t2dVectorLength(%curVel) > 5.0)
	{
		%this.setBehaviorCollisionCallback("1");
		%this.collisionSwitchedOff = "0";
		%owner.setFlipX(direction + 1.0);
	}
	%dirVector = t2dVectorScale(horizontalVector, direction);
	%addVel = t2dVectorScale(%dirVector, accelFactor * duration);
	%nextVel = t2dVectorAdd(%curVel, %addVel);
	if (t2dVectorLength(%nextVel) < maxVelocity)
	{
		%owner.setLinearVelocity(%nextVel);
	}
	return;
}
function BePaceBackAndForth::onCollision(%this, , , , , %normal, , )
{
	if (!(wantsCollisionCallback))
	{
		return %this;
	}
	%owner = Owner;
	%normalAngle = t2dRelativeAngleBetween(horizontalVector, %normal);
	if (equalsTolerance(getHorizontalComponent(%normal), -1.0 * direction, "0.25"))
	{
		%this.direction = direction * -1.0;
		if (turnAroundRightAway)
		{
			%owner.setAtRest();
			%owner.setFlipX(direction + 1.0);
			break;
		}
		%this.setBehaviorCollisionCallback("0");
		%this.collisionSwitchedOff = "1";
	}
	return;
}
