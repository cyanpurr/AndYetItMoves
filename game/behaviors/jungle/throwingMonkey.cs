// throwingMonkey.cs.dso
if (!(isObject(BeThrowingMonkey)))
{
	%template = new BehaviorTemplate(Name : BeThrowingMonkey);
	%template.friendlyName = "ThrowingMonkey";
	%template.behaviorType = "LevelJungle";
	%template.description = "a monkey that throws coconuts and if he gets hit by one he falls of the tree";
	%template.addBehaviorField(coconutActivatioRadius, "how far away the coconuts have to be to get activated (in percent of owner)", float, "1.2");
	%template.addBehaviorField(aggressiveRadius, "the radius from which the monkey starts throwing hard and fast", float, "30");
	%template.addBehaviorField(throwOriginFactor, "how far to the left (negative; positive is to the right) the coconuts should be thrown of (in owner local space)", float, -1.0);
	%template.addBehaviorField(throwUpFactor, "how much he throws the cocnut upwards", float, "1.5");
	%template.addBehaviorField(imprecision, "how good he will directly aim at the player, the lower the better - gets scaled by distance to player", float, "10");
	%template.addBehaviorField(throwInterval, "how often the monkey should throw a cocnut (in s) - gets scaled by distance to player", float, "2");
	%template.addBehaviorField(throwSpeed, "how fast the monkey shall throw - gets scaled by distance to player", float, "1");
	%template.addBehaviorField(autoActivate, "if true this obejct will make its activate behavior with viewFactor 3", bool, "1");
}
function BeThrowingMonkey::onBehaviorAdd(%this)
{
	%owner = Owner;
	%owner.addDependentBehaviors("BeCollide");
	subscribeToEvents(%this, "onLevelLoadFinished9");
	return;
}
function BeThrowingMonkey::onLevelLoadFinished9(%this)
{
	%owner = Owner;
	if (autoActivate)
	{
		%viewSwitch = %owner.addDependentBehavior("BeActivate");
		%viewSwitch.factor = "3";
		%viewSwitch.BehaviorList = "BeThrowingMonkey BeReactOnCollision";
		%viewSwitch.activator = "viewWindowTrigger";
		%viewSwitch.suppressOwnerCollision = "0";
	}
	%owner.setCollisionActiveSend("1");
	%react = %owner.addDependentBehavior("BeReactOnCollision");
	%react.initGroups("jungleCoconutActive");
	%react.maxCollisions = "1";
	%react.BehaviorList = "BeThrowingMonkey";
	%owner.setGraphGroup($GROUPS["jungleMonkey"]);
	%this.aggresiveTrigger = new t2dTrigger(Name : aggressiveMonkeyTrigger)
	{
		scenegraph = daSceneGraph;
		Position = %owner.getPosition();
		callThrowOn = %this;
		_behavior0 = "BeTrigger";
	}
	aggresiveTrigger.setSize(aggressiveRadius, aggressiveRadius);
	aggresiveTrigger.setLeaveCallback("0");
	aggresiveTrigger.mount(%owner, "0 0", "0", "1", "1", "1", "1");
	%this.tenseSoundTrigger = new t2dTrigger(Name : "")
	{
		scenegraph = daSceneGraph;
		size = "600 600";
		_behavior0 = "BePlaySound	profileName	JungleTenseAmbientBase	maxVolume	0.7	fullVolumeFactor	0.1	distanceCalculationMode	quadratic	turnOnGroups	1	turnOffGroups	1";
	}
	tenseSoundTrigger.setCollisionDetection("CIRCLE");
	tenseSoundTrigger.setCollisionCircleScale("1");
	tenseSoundTrigger.setCollisionCircleSuperscribed("1");
	tenseSoundTrigger.mount(%owner, "0 0", "0", "1", "1", "1", "0");
	%allMountedChilds = %owner.getMountedChildren();
	%i = 0;
	while (%i < getWordCount(%allMountedChilds))
	{
		%child = getWord(%allMountedChilds, %i);
		%child.setMountOwned("1");
		%i = %i + 1.0;
	}
	return getWordCount(%allMountedChilds);
}
function BeThrowingMonkey::switchOn(%this)
{
	if (inPlayerView)
	{
		return Owner;
	}
	if (isObject(throwSchedule))
	{
		throwSchedule.cancelSchedule();
	}
	%this.throwSchedule = safeSchedule(throwInterval / 2.0 * 1000.0, %this, "throw");
	Owner.getBehavior(BePlayLoopingSound).switchOn();
	return;
}
function BeThrowingMonkey::switchOff(%this)
{
	if (isObject(throwSchedule))
	{
		throwSchedule.cancelSchedule();
	}
	Owner.getBehavior(BePlayLoopingSound).switchOff();
	return;
}
function BeThrowingMonkey::throw(%this)
{
	%owner = Owner;
	%projectile = %this.makeCoconut();
	%projectile.setPosition(%owner.getWorldPoint(throwOriginFactor SPC "0"));
	%projectile.setGraphGroup($GROUPS["jungleCoconut"]);
	%reactBehavior = %projectile.getBehavior("BeReactOnCollision");
	%reactBehavior.BehaviorList = %projectile;
	%direction = t2dVectorSub(player.getPosition(), %owner.getPosition());
	%imprecision = imprecision * t2dVectorLength(%direction);
	%direction = t2dVectorAdd(%direction, floatRandom(-1.0 * %imprecision, %imprecision) SPC floatRandom(-1.0 * %imprecision, %imprecision));
	%distance = t2dVectorLength(%direction);
	if (throwUpFactor > 0.0)
	{
		%direction = t2dVectorSub(%direction, t2dVectorScale(verticalVector, throwUpFactor * %distance));
	}
	%projectile.setLinearVelocity(t2dVectorScale(%direction, throwSpeed * aggressiveRadius / %distance));
	subscribeToEvents(%projectile, "onUpdateTick15");
	if (isObject(throwSchedule))
	{
		throwSchedule.cancelSchedule();
	}
	%this.throwSchedule = safeSchedule(throwInterval * 10.0 * %distance, %this, "throw");
	return;
}
function BeThrowingMonkey::makeCoconut(%this)
{
	%coconut = new t2dStaticSprite(Name : "")
	{
		scenegraph = daSceneGraph;
		imageMap = "coconutImageMap";
		frame = "0";
		size = "8 8";
		class = "Projectile";
		CollisionGroups = "";
		CollisionDetectionMode = "CIRCLE";
		CollisionCircleScale = "0.7";
		_behavior0 = "BeMoving	influencedByPlayer	1";
		_behavior1 = "BeReactOnCollision	reactToGroups	collide player	maxCollisions	0";
		thrower = Owner;
		activationFactor = coconutActivatioRadius;
	}
	%coconut.getBehavior(BeReactOnCollision).switchOn();
	return %coconut;
	return %coconut;
}
function BeThrowingMonkey::makeMonkey(%this)
{
	%monkey = new t2dStaticSprite(Name : "")
	{
		scenegraph = daSceneGraph;
		imageMap = "monkeyBodyImageMap";
		frame = "0";
		size = "15 16.2";
		class = "Projectile";
		CollisionGroups = "";
		CollisionPolyList = "0.117 -1.000 0.504 -0.857 0.546 -0.442 0.578 0.058 0.373 0.545 -0.019 0.908 -0.305 0.608 -0.643 0.040 -0.335 -0.739";
		_behavior0 = "BeMoving	influencedByPlayer	1";
		_behavior1 = "BeReactOnCollision	reactToGroups	collide player	maxCollisions	0";
		thrower = Owner;
		activationFactor = coconutActivatioRadius;
	}
	%monkey.getBehavior(BeReactOnCollision).switchOn();
	%head = new t2dStaticSprite(Name : "")
	{
		scenegraph = daSceneGraph;
		imageMap = "monkeyHeadImageMap";
		frame = "0";
		size = "8 8";
	}
	%monkey.head = %head;
	%head.mount(%monkey, "0 -0.8", "0", "1", "1", "1", "1");
	return %monkey;
	return %monkey;
}
function BeThrowingMonkey::reactOnCollision(%this, %dstObject, , , , %normal, , , )
{
	%owner = Owner;
	%owner.dismount();
	%owner.removeBehavior(%owner.getBehavior("BeCollide"));
	%owner.removeBehavior(%owner.getBehavior("BeReactOnCollision"));
	%movingBehavior = %owner.addDependentBehavior("BeMoving");
	%movingBehavior.init();
	%owner.setImmovable("0");
	%owner.setCollisionGroups($GROUPS["rippedEdgeTrigger"]);
	%this.switchOff();
	aggresiveTrigger.setEnterCallback("0");
	%owner.setLayer($LAYER["main_foreground"]);
	monkeyHead.setLayer($LAYER["main_foreground"] - 1.0);
	tenseSoundTrigger.getBehavior("BePlaySound").forcedFadeOut("3");
	playEventSound(MonkeyCry, "0.8");
	%owner.viewSafeDelete();
	return;
}
function aggressiveMonkeyTrigger::onEnter(%this)
{
	callThrowOn.throw();
	return;
}
function Projectile::onUpdateTick15(%this)
{
	%distanceToMonkey = mAbs(t2dVectorDistance(thrower.getPosition(), %this.getPosition()));
	if (thrower.getSizeX() > thrower.getSizeY())
	{
	}
	else
	{
	}
	%longMonkeySide = thrower.getSizeY();
	if (%distanceToMonkey > %longMonkeySide / 2.0 * activationFactor)
	{
		%this.beDangerousForMonkey();
		unSubscribeFromEvents(%this, "onUpdateTick15");
	}
	return;
}
function Projectile::beDangerousForMonkey(%this)
{
	%this.setGraphGroup($GROUPS["jungleCoconutActive"]);
	%reactBehavior = %this.getBehavior("BeReactOnCollision");
	%reactBehavior.initGroups("collide player jungleMonkey");
	%reactBehavior.maxCollisions = "1";
	%this.setCollisionActive("1", "1");
	return;
}
function Projectile::reactOnCollision(%this, %dstObject, , , , %normal, , , )
{
	if (isPlayer(%dstObject))
	{
		if (t2dVectorLength(%this.getLinearVelocity()) > 70.0)
		{
			player.lastDeathEvent = "monkey";
			player.dieExploding("100");
		}
	}
	%this.breakCoconut(%normal);
	return;
}
function Projectile::breakCoconut(%this, %normal)
{
	%size = %this.getWidth() / 2.0 SPC %this.getHeight();
	%halfACoconut = new t2dStaticSprite(Name : "")
	{
		scenegraph = daSceneGraph;
		imageMap = "coconutbrokenImageMap";
		frame = "0";
		Position = %this.getPosition();
		size = %size;
		_behavior0 = "BeMoving";
		_behavior1 = "BeMask	Layer	main_foreground	useOnlyParallaxing	1";
	}
	%halfACoconut.getBehavior("BeMask").initMaskLayer();
	%halfACoconut.setCollisionSuppress("1");
	%impactSpeed = getImpactSpeed(%this.getLinearVelocity(), %normal);
	%bounceVector = t2dVectorScale(rotateVector(horizontalVector, getRandom("30", "150")), %impactSpeed / 2.0);
	%halfACoconut.setLinearVelocity(%bounceVector);
	%halfACoconut2 = new t2dStaticSprite(Name : "")
	{
		scenegraph = daSceneGraph;
		imageMap = "coconutbrokenImageMap";
		frame = "1";
		Position = %this.getPosition();
		size = %size;
		_behavior0 = "BeMoving";
		_behavior1 = "BeMask	Layer	main_background	useOnlyParallaxing	1";
	}
	%halfACoconut2.getBehavior("BeMask").initMaskLayer();
	%halfACoconut2.setCollisionSuppress("1");
	%bounceVector = t2dVectorScale(rotateVector(horizontalVector, getRandom("30", "150")), %impactSpeed / 2.0);
	%halfACoconut2.setLinearVelocity(%bounceVector);
	playDistanceEventSound(%this, CoconutSmash, "0.8");
	callNextFrame(%this, "safeDelete");
	%halfACoconut.viewSafeDelete();
	%halfACoconut2.viewSafeDelete();
	return;
}
