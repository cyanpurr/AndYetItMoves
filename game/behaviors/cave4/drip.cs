// drip.cs.dso
if (!(isObject(BeDrip)))
{
	%template = new BehaviorTemplate(Name : BeDrip);
	%template.friendlyName = "Drip";
	%template.behaviorType = "LevelCave4";
	%template.description = "makes this object drip water from its link point";
	%template.addBehaviorField(dripInterval, "how often the object should drip (in s)", float, "4");
	%template.addBehaviorField(dripVariance, "how much the intervall should randomly differ (in s; [dripInterval - dripVariance; dripInterval + dripVariance])", float, "2");
	%template.addBehaviorField(dropSize, "how big should the drop grow (vector)", Vector, "9 9", t2dVector);
	%template.addBehaviorField(dropDensity, "density of object", float, "0.01");
	%template.addBehaviorField(dropFriction, "friction of object", float, "0.3");
	%template.addBehaviorField(dropRestitution, "description of field", float, "0");
	%template.addBehaviorField(autoStart, "if the object should start dripping right away", bool, "0");
	%template.addBehaviorField(dripRotation, "drip when camera is in this rotation", float, "0");
	%template.addBehaviorField(cloakObjects, "a space seperated list of objectnames that should reveal the ripped edge", string, "");
}
function BeDrip::onBehaviorAdd(%this)
{
	subscribeToEvents(%this, "onLevelLoadFinished4 onLevelLoadFinished20");
	return;
}
function BeDrip::onLevelLoadFinished4(%this)
{
	%owner = Owner;
	if (%owner.getLinkCount())
	{
	}
	else
	{
	}
	%this.dripPosition = %owner.getPosition();
	if (dripVariance > dripInterval)
	{
		%this.dripVariance = dripInterval;
		debugWarn("dripVariance too big; it could lead to a negative dripIntervall.");
		debugWarn("setting dripVariance to next possible value:" SPC dripVariance);
	}
	if (autoStart)
	{
		%this.startDripping();
	}
	%delayedBreak = %owner.addDependentBehavior("BeDelayedBreaking");
	%delayedBreak.setBreakDelay("0.5");
	%delayedBreak.setCloakObjects(cloakObjects);
	%owner.setImmovable("1");
	%this.dropTrigger = DropCollisionTrigger::createInstance(dripPosition);
	return;
}
function BeDrip::onLevelLoadFinished20(%this)
{
	%owner = Owner;
	%owner.setCollisionGroups($GROUPS["growingRoot"]);
	%owner.setCollisionActive("1", "1");
	%owner.setGraphGroup($GROUPS["collide"]);
	%this.setBehaviorCollisionCallback("1");
	return;
}
function BeDrip::onCollision(%this, %dstObj, , , , , , )
{
	if (!(wantsCollisionReceiveCallback))
	{
		return %this;
	}
	%this.breakOff();
	return;
}
function BeDrip::switchOn(%this)
{
	%this.startDripping();
	Owner.setCollisionSuppress("0");
	return;
}
function BeDrip::switchOff(%this)
{
	%this.stopDripping("1");
	Owner.setCollisionSuppress("1");
	return;
}
function BeDrip::startDripping(%this)
{
	if (dripping)
	{
		return %this;
	}
	%this.dripping = "1";
	%this.Drip();
	return;
}
function BeDrip::Drip(%this)
{
	if (dripVariance)
	{
	}
	else
	{
	}
	%nextDrip = dripInterval;
	if (equalsAngleTolerance(camera.getCurrentRotation(), dripRotation, "40"))
	{
		%this.waterDrop = waterDrop::createInstance(%nextDrip, %this);
	}
	%this.safeScheduleObject = safeSchedule(%nextDrip * 1000.0, %this, "drip");
	return;
}
function BeDrip::stopDripping(%this, %killCurrenDrop)
{
	%this.dripping = "0";
	if (isObject(safeScheduleObject))
	{
		safeScheduleObject.cancelSchedule();
	}
	if (%killCurrenDrop && isObject(waterDrop))
	{
		waterDrop.kill();
	}
	return;
}
function BeDrip::breakOff(%this)
{
	%owner = Owner;
	if (dripping)
	{
		%this.stopDripping("1");
	}
	%owner.getBehavior("BeDelayedBreaking").quarry();
	%owner.setCollisionSuppress("1");
	dropTrigger.safeDelete();
	%owner.viewSafeDelete();
	playHandleEventSound("dripBreakOff", StoneBreakMedium, "0.7");
	return;
}
function DropCollisionTrigger::createInstance(%position)
{
	%trigger = new t2dTrigger(Name : "")
	{
		scenegraph = daSceneGraph;
		class = DropCollisionTrigger;
		Position = %position;
		size = "7 7";
		CollisionDetectionMode = "CIRCLE";
		CollisionCircleSuperscribed = "0";
		_behavior0 = "BeTrigger";
	}
	%trigger.setGraphGroup($GROUPS["dropCollisionTrigger"]);
	%trigger.setCollisionDetection("CIRCLE");
	%trigger.setEnterCallback("0");
	return %trigger;
	return %trigger;
}
function DropCollisionTrigger::onLeave(%this, %obj)
{
	%movingBehavior = %obj.getBehavior("BeMoving");
	%movingBehavior.initMovingCollisionGroups("player collide moving growingRoot batTrigger");
	%obj.setOwnerCollisionCallback("1");
	%obj.setCollisionActive("1", "0");
	return;
}
function waterDrop::createInstance(%timeToGrow, %dripBehavior)
{
	%drop = new t2dStaticSprite(Name : "")
	{
		imageMap = "waterDropImageMap";
		frame = "0";
		scenegraph = daSceneGraph;
		class = "WaterDrop";
		size = "0.01 0.01";
		Position = dripPosition;
		CollisionPolyList = "-0.099 -0.744 0.454 -0.367 0.605 0.333 0.219 0.800 -0.441 0.793 -0.752 0.239 -0.638 -0.394";
		timeToGrow = %timeToGrow;
		growToSize = dropSize;
		dripBehavior = %dripBehavior;
		_behavior0 = "BeDontCollide";
	}
	%drop.effect = new t2dParticleEffect(Name : "")
	{
		scenegraph = daSceneGraph;
		effectFile = "~/data/particles/dropSplash.eff";
		Layer = %drop.getLayer();
	}
	%drop.growVelocity = t2dVectorScale(growToSize, 1.0 / timeToGrow);
	%drop.isGrowing = "1";
	%drop.setSizeVelocity(growVelocity);
	subscribeToEvents(%drop, "onRotationStart onRotationFinish");
	safeSchedule(timeToGrow * 1000.0, %drop, "drop");
	return %drop;
	return %drop;
}
function waterDrop::splash(%this)
{
	%this.safeDelete();
	effect.setPosition(%this.getPosition());
	effect.getEmitterObject("0").setFixedForceAngle(-1.0 * camera.getCurrentRotation());
	effect.setRotation(-1.0 * camera.getCurrentRotation());
	effect.playEffect();
	effect.setSize(%this.getSize());
	effect.setEffectLifeMode(kill, "0.2");
	playEventSound(WaterDrop4, floatRandom("0.2", "0.4"));
	return;
}
function waterDrop::Drop(%this)
{
	%this.setSizeVelocity("0", "0");
	unSubscribeFromEvents(%this, "onRotationStart onRotationFinish");
	%this.removeBehavior(%this.getBehavior("BeDontCollide"));
	%moveBehavior = %this.addDependentBehavior("BeMoving");
	%moveBehavior.Density = dropDensity;
	%moveBehavior.Friction = dropFriction;
	%moveBehavior.Restitution = dropRestitution;
	%moveBehavior.CollisionGroups = "";
	%moveBehavior.init("0");
	%this.removeBehavior(%this.getBehavior("BeShrinker"));
	%this.setCollisionActive("1", "0");
	%this.setCollisionGroups($GROUPS["dropCollisionTrigger"]);
	%this.setCollisionDetection("CIRCLE");
	%this.setCollisionCircleScale("0.7");
	playHandleEventSound("waterDrop", getRandomWord("WaterDrop2 WaterDrop3"), floatRandom("0.2", "0.4"));
	return;
}
function waterDrop::onRotationStart(%this)
{
	%this.setSizeVelocity("0", "0");
	return;
}
function waterDrop::onRotationFinish(%this)
{
	%this.setSizeVelocity(growVelocity);
	return;
}
function waterDrop::onCollision(%this, %dstObj, , , , , , )
{
	if (%dstObj.getClassNamespace() $= "BatTrigger")
	{
		debugEcho("You're a bat wetter!!!");
		achievements.setAchieved("BatWetter");
	}
	%this.splash();
	return;
}
