// dionaea.cs.dso
if (!(isObject(BeDionaea)))
{
	%template = new BehaviorTemplate(Name : BeDionaea);
	%template.friendlyName = "Dionaea";
	%template.behaviorType = "LevelJungle";
	%template.description = "a dionaea which will eat bugs and bite the player and throw him away if he comes to close";
	%template.addBehaviorField(bugSpawner, "the object that spawns the bugs, so the plant can call a new spawn", object, "bugSpawner", t2dSceneObject);
}
function BeDionaea::onBehaviorAdd(%this)
{
	subscribeToEvents(%this, "onLevelLoadFinished1 onLevelLoadFinished10");
	return;
}
function BeDionaea::onLevelLoadFinished1(%this)
{
	%owner = Owner;
	if (!(%owner.getLinkCount()))
	{
		%owner.setLinkPoint("0 0.9");
	}
	%rotateBehavior = %owner.addDependentBehavior("BeRotate");
	%rotateBehavior.autoPlay = "0";
	%rotateBehavior.symmetricAngle = "0";
	%rotateBehavior.loop = "0";
	%rotateBehavior.callbackObject = %this;
	%rotateBehavior.use32msTick = "1";
	%this.createHead();
	%this.createSnapTrigger();
	return;
}
function BeDionaea::createHead(%this)
{
	%owner = Owner;
	%position = %owner.getWorldPoint("-0.95 -0.0");
	%animation = new t2dAnimatedSprite(Name : dionaeaAnimation)
	{
		scenegraph = "daScenegraph";
		animationName = "dionaea_head_1Animation";
		canSaveDynamicFields = "1";
		Position = %position;
		size = "43.008 21.504";
		_behavior0 = "BeTexture";
		_behavior1 = "BeDontCollide";
	}
	%this.head = new t2dStaticSprite(Name : dioneaHead)
	{
		scenegraph = "daScenegraph";
		class = "DionaeaHead";
		imageMap = "laenglicheFormenImageMap";
		frame = "6";
		canSaveDynamicFields = "1";
		Position = %position;
		size = "38 20.496";
		CollisionPolyList = "-0.937 -0.120 -0.560 -0.811 0.473 -0.721 0.967 -0.198 0.632 0.532 0.057 0.944 -0.650 0.755";
		_behavior0 = "BeMask	layer	mainBehindPlayer	textureObject	dionaeaAnimation	dontCollide	0";
		dionaeaBehavior = %this;
	}
	%animation.mount(head, "0 0", "0", "1", "1", "1", "0");
	%mountBehavior = head.addDependentBehavior("BeMountWithOffset");
	%mountBehavior.mother = %owner;
	%mountBehavior.trackRotation = "1";
	return;
}
function BeDionaea::createSnapTrigger(%this)
{
	%owner = Owner;
	if (%owner.getWidth() > %owner.getHeight())
	{
	}
	else
	{
	}
	%diameter = 2.0 * %owner.getHeight();
	if (%owner.getLinkCount())
	{
	}
	else
	{
	}
	%position = %owner.getWorldPoint("0 0.9");
	%this.snapTrigger = new t2dTrigger(Name : "")
	{
		scenegraph = daSceneGraph;
		class = "DionaeaSnapTrigger";
		size = %diameter SPC %diameter;
		Position = %position;
		_behavior0 = "BeTrigger	collisionGroup	jungleDionaeaSnapTrigger";
		dionaeaBehavior = %this;
	}
	snapTrigger.setCollisionDetection("CIRCLE");
	snapTrigger.setCollisionCircleScale("0.8");
	return;
}
function BeDionaea::onLevelLoadFinished10(%this)
{
	%owner = Owner;
	head.setGraphGroup($GROUPS["jungleDionaeaHead"]);
	head.setCollisionActive("1", "0");
	head.setCollisionPhysics("0", "0");
	head.setCollisionGroups($GROUPS["player"] SPC $GROUPS["jungleBug"]);
	head.setOwnerCollisionCallback("1");
	%this.originalRotation = %owner.getBehavior("BeRotate").getPivotRotation();
	%this.startIdleAnimation();
	return;
}
function BeDionaea::attack(%this, %object)
{
	%owner = Owner;
	%rotateBehavior = %owner.getBehavior("BeRotate");
	%rotateBehavior.pause();
	%vectorToHead = t2dVectorNormalise(t2dVectorSub(head.getPosition(), %owner.getLinkPoint("1")));
	%vectorToObject = t2dVectorNormalise(t2dVectorSub(%object.getPosition(), %owner.getLinkPoint("1")));
	%angleHeadToObject = t2dRelativeAngleBetween(%vectorToHead, %vectorToObject);
	%oldRotation = %rotateBehavior.getPivotRotation();
	%rotateTo = t2dShortestAngleDifference(%oldRotation + %angleHeadToObject, %oldRotation);
	if (25.0 / t2dVectorLength(%object.getLinearVelocity()) > 1.0)
	{
	}
	else
	{
	}
	%time = 25.0 / t2dVectorLength(%object.getLinearVelocity());
	%rotateBehavior.angle = %rotateTo;
	%rotateBehavior.time = %time;
	%rotateBehavior.varianceFactor = "0";
	%rotateBehavior.callbackObject = %this;
	%rotateBehavior.loop = "0";
	%rotateBehavior.symmetricAngle = "0";
	%rotateBehavior.initSmoother();
	%rotateBehavior.play();
	return;
}
function BeDionaea::relax(%this)
{
	%owner = Owner;
	%rotateBehavior = %owner.getBehavior("BeRotate");
	%rotateBehavior.pause();
	%rotateBehavior.angle = originalRotation - %rotateBehavior.getPivotRotation();
	%rotateBehavior.time = "1";
	%rotateBehavior.varianceFactor = "0";
	%rotateBehavior.callbackObject = %this;
	%rotateBehavior.loop = "0";
	%rotateBehavior.symmetricAngle = "0";
	%rotateBehavior.initSmoother();
	%rotateBehavior.play();
	%this.isRelaxing = "1";
	return;
}
function BeDionaea::onRotatorFinish(%this)
{
	%owner = Owner;
	if (objectEntered != "" && munchingOn == 0.0)
	{
		%this.attack(objectEntered);
		debugEcho("attacking again" SPC objectEntered);
	}
	else
	{
		if (munchingOn == player.getId())
		{
			debugEcho("dionaea caught the player!" SPC munchingOn);
			%owner.getBehavior("BePlayLoopingSound").play();
			subscribeToEvents(%this, "onPlayerDeath");
			%this.SpitOut = safeSchedule("1000", %this, "spitPlayerOut");
			break;
		}
		if (isObject(munchingOn) && munchingOn.getGraphGroup() == $GROUPS["jungleBug"])
		{
			debugEcho("dionaea caught the bug!" SPC munchingOn);
			%owner.getBehavior("BePlayLoopingSound").play();
			safeSchedule("15000", %this, "digestBug");
		}
	}
	if (isRelaxing)
	{
		%this.isRelaxing = "0";
		%this.startIdleAnimation();
	}
	return;
}
function BeDionaea::onPlayerDeath(%this)
{
	if (player.getMountedParent() == head)
	{
		player.dismount();
	}
	safeSchedule("500", %this, "suppressAllCollisions", "false");
	%this.munchingOn = "0";
	spitPlayerOut.cancelSchedule();
	return;
}
function BeDionaea::startIdleAnimation(%this)
{
	%owner = Owner;
	%rotateBehavior = %owner.getBehavior("BeRotate");
	%rotateBehavior.callbackObject = "";
	%rotateBehavior.pause();
	%rotateBehavior.angle = "10";
	%rotateBehavior.time = "3";
	%rotateBehavior.loop = "1";
	%rotateBehavior.symmetricAngle = "1";
	%rotateBehavior.initSmoother();
	%rotateBehavior.play();
	return;
}
function BeDionaea::spitPlayerOut(%this)
{
	Owner.getBehavior("BePlayLoopingSound").stop();
	player.dismount();
	player.setLinearVelocity("-100 -40");
	player.setRotation(-1.0 * camera.getCurrentRotation());
	setControlsEnabled("1");
	safeSchedule("500", %this, "suppressAllCollisions", "false");
	%this.munchingOn = "0";
	playDistanceEventSound(Owner, SpitOut, "0.6");
	return;
}
function BeDionaea::DigestBug(%this)
{
	if (isPlayer(munchingOn, "1"))
	{
		debugEcho("WARNING: BeDionaea::digestBug wanted to delete player, spitting him out instead!");
		%this.spitPlayerOut();
		return;
	}
	if (munchingOn.getGraphGroup() != $GROUPS["jungleBug"])
	{
		debugEcho("WARNING: BeDionaea::digestBug wanted to delete something that's not a bug, doing nothing instead");
		return;
	}
	Owner.getBehavior("BePlayLoopingSound").stop();
	munchingOn.safeDelete();
	%this.munchingOn = "0";
	playDistanceEventSound(Owner, DigestBug, "0.5");
	safeSchedule("1000", %this, "suppressAllCollisions", "0");
	return;
}
function BeDionaea::suppressAllCollisions(%this, %suppress)
{
	head.setCollisionSuppress(%suppress);
	snapTrigger.setCollisionSuppress(%suppress);
	return;
}
function BeDionaea::switchOn(%this)
{
	%this.suppressAllCollisions("0");
	%this.relax();
	return;
}
function BeDionaea::switchOff(%this)
{
	%this.suppressAllCollisions("1");
	return;
}
function DionaeaSnapTrigger::onEnter(%this, %object)
{
	dionaeaBehavior.attack(%object);
	dionaeaBehavior.objectEntered = %object;
	return;
}
function DionaeaSnapTrigger::onLeave(%this, %object)
{
	dionaeaBehavior.objectEntered = "";
	dionaeaBehavior.relax();
	return;
}
function DionaeaHead::onCollision(%this, %dstObject, , , %time, , , )
{
	if (isPlayer(%dstObject, "1"))
	{
		player.setRotation("270");
		setControlsEnabled("0");
	}
	dionaeaBehavior.munchingOn = %dstObject.getId();
	if (%dstObject.getGraphGroup() == $GROUPS["jungleBug"])
	{
		%dstObject.getBehavior("BePlayLoopingSound").stop();
		%dstObject.removeBehavior(%dstObject.getBehavior("BePlayLoopingSound"));
		%dstObject.removeBehavior(%dstObject.getBehavior("BePaceBackAndForth"));
		%dstObject.removeBehavior(%dstObject.getBehavior("BeKeepOrientation"));
	}
	%dstObject.mount(%this, "0 0", "0", "1", "0", "0", "0");
	dionaeaBehavior.suppressAllCollisions("1");
	dionaeaBehavior.relax();
	return;
}
