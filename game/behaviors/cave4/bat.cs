// bat.cs.dso
$BatCount = 0;
$BatsCounted = 0;
if (!(isObject(BeBat)))
{
	%template = new BehaviorTemplate(Name : BeBat);
	%template.friendlyName = "Bat";
	%template.behaviorType = "LevelCave4";
	%template.description = "a bat that always flys upward and hangs of walls when reaching them";
	%template.addBehaviorField(autoPlay, "if rotation starts on levelload", bool, "1");
	%template.addBehaviorField(deviation, "used for circulating: how much this bat changes radius", float, "0.5");
	%template.addBehaviorField(rotationTime, "how long for one circulation", float, "2");
	%template.addBehaviorField(Layer, "layer for bats, if main they behave like in cave4, else they need a circulator object", enum, "main", $LAYER_ENUM);
	%template.addBehaviorField(center, "object which marks center of circulation and basic radius if on dekolayers", object, null, t2dSceneObject);
}
function BeBat::onBehaviorAdd(%this)
{
	%owner = Owner;
	if (Layer $= "main")
	{
		%owner.setGraphGroup($GROUPS["bat"]);
		%owner.setCollisionGroups($GROUPS["collide"] SPC $GROUPS["batBlocker"] SPC $GROUPS["saurianTrigger"]);
		%owner.setCollisionActive("1", "1");
		%owner.setCollisionPhysics("1", "0");
		%owner.setBehaviorCollisionCallback("1");
		%owner.setCollisionDetection("POLYGON");
		%owner.setCollisionPolyCustom("2", "0 0 0 -0.9");
		%this.layerToSet = "mainBehindPlayer";
	}
	else
	{
		%owner.setCollisionActive("0", "0");
		%owner.setCollisionPhysics("0", "0");
		%this.layerToSet = Layer;
	}
	subscribeToEvents(%this, "onLevelLoadFinished3 onLevelLoadFinished");
	$BatCount = $BatCount + 1.0;
	return;
}
function BeBat::onLevelLoadFinished3(%this)
{
	%owner = Owner;
	%owner.escapeSwitch = "1";
	%this.circulator = new t2dSceneObject(Name : "")
	{
		scenegraph = scenegraph;
		_behavior0 = "BeDontCollide";
	}
	%rotateBehavior = circulator.addDependentBehavior("BeContinuouslyRotate");
	%rotateBehavior.AngularVelocity = 360.0 / rotationTime;
	%rotateBehavior.autoPlay = "0";
	%this.mountID = circulator.addLinkPoint("2 0");
	%owner.setLayer($LAYER[layerToSet]);
	%blendFactor = $LAYER_BLENDING[layerToSet];
	%owner.setBlendColor(%blendFactor, %blendFactor, %blendFactor, "1");
	return;
}
function BeBat::onLevelLoadFinished(%this, )
{
	%owner = Owner;
	%this.targetObject = new t2dSceneObject(Name : "")
	{
		scenegraph = scenegraph;
		Position = %owner.getPosition();
		_behavior0 = "BeDontCollide";
	}
	if (autoPlay)
	{
		%this.switchOn();
	}
	unSubscribeFromEvent(%this, "onLevelLoadFinished");
	if (Layer != "main")
	{
		center.setCollisionActive("0", "0");
		%this.circulate(center.getPosition(), center.getCollisionRadius());
	}
	else
	{
		BatTrigger::createInstance(%owner);
	}
	if ($WII)
	{
		%owner.setSize(t2dVectorScale(%owner.getSize(), 4.0 / 3.0));
	}
	return;
}
function BeBat::updateTarget(%this)
{
	%owner = Owner;
	%targetPos = %owner.getPosition();
	setHorizontalComponent(%targetPos, getHorizontalComponent(lastPosition));
	%targetOffset = getRandom(-50.0, "50") SPC getRandom(-80.0, -40.0);
	%targetPos = t2dVectorAdd(%targetPos, absRot(%targetOffset));
	targetObject.setPosition(%targetPos);
	if (!(%owner.getMountedParent()))
	{
		%owner.mount(targetObject, "0 0", "1.5", "0", "0", "0", "0");
	}
	return;
}
function BeBat::onTimer(%this)
{
	if (isCirculating)
	{
		%this.updateCircle();
	}
	else
	{
		%this.updateTarget();
	}
	return;
}
function BeBat::onCollision(%this, , , , , %normal, , )
{
	if (!(wantsCollisionCallback))
	{
		return %this;
	}
	if (isCirculating)
	{
		%this.isColliding = "1";
		return;
	}
	%owner = Owner;
	%realNormal = rotateVector(%normal, -1.0 * camera.getCurrentRotation());
	%normX = getX(%realNormal);
	%normY = getY(%realNormal);
	if (mAbs(%normX) > %normY * 0.8399999737739563)
	{
		return mAbs(%normX);
	}
	else
	{
		%owner.setAnimation(batInPaperSitAnimation);
		%this.stopMoving();
	}
	return;
}
function BeBat::startMoving(%this)
{
	%owner = Owner;
	%this.flying = "1";
	%owner.playAnimation(batInPaperFlyAnimation, "0", getRandom("1", "3"));
	if (isCirculating)
	{
		%this.stopCirculating();
		return;
	}
	%owner.setMountForce("1.5");
	%this.setBehaviorCollisionCallback("1");
	%this.lastPosition = %owner.getPosition();
	%this.updateTarget();
	%owner.setTimerOn("300");
	subscribeToEvents(%this, "onRotationStart onRotationFinish");
	return;
}
function BeBat::stopMoving(%this)
{
	%owner = Owner;
	%owner.dismount();
	%owner.setAtRest();
	%owner.setTimerOff();
	if (wantsCollisionCallback)
	{
		%this.setBehaviorCollisionCallback("0");
	}
	return;
}
function BeBat::circulate(%this, %point, %radius)
{
	if (isCirculating)
	{
		return %this;
	}
	%owner = Owner;
	%owner.playAnimation(batInPaperFlyAnimation, "0", getRandom("1", "3"));
	circulator.originalRadius = %radius;
	%rotStartAngle = getVectorAngle(t2dVectorSub(%owner.getPosition(), %point));
	circulator.setPosition(%point);
	circulator.setRotation(%rotStartAngle);
	%this.updateCircle();
	if (getRandom() > 0.5)
	{
		%rotationAngle = 360;
	}
	else
	{
		%rotationAngle = -360.0;
	}
	%continuouslyRotateBehavior = circulator.getBehavior("BeContinuouslyRotate");
	%continuouslyRotateBehavior.AngularVelocity = %rotationAngle / rotationTime;
	%owner.setTimerOn("300");
	%owner.setCollisionSuppress("1");
	targetObject.mountToLinkpoint(circulator, mountID, "0", "0", "1", "0", "0");
	if (!(%owner.getMountedParent()))
	{
		%owner.mount(targetObject, "0 0", "3", "0", "0", "0", "0");
	}
	if (Layer $= "main")
	{
		%owner.setLayer($LAYER["main"] - 2.0);
	}
	%this.isCirculating = "1";
	%continuouslyRotateBehavior.initRotating();
	return;
}
function BeBat::updateCircle(%this)
{
	%originalRadius = originalRadius;
	%radius = %originalRadius + getRandom(-1.0 * %originalRadius * deviation, %originalRadius * deviation);
	circulator.setSize(%radius, "1");
	return;
}
function BeBat::stopCirculating(%this)
{
	%owner = Owner;
	if (%owner.getCollisionSuppress())
	{
		%owner.setPhysicsSuppress("1");
		%owner.setCollisionSuppress("0");
		%this.setBehaviorCollisionCallback("1");
		%owner.setTimerOff();
		%owner.setMountForce("5");
		circulator.setSize(originalRadius, "1");
		%this.isColliding = "1";
	}
	if (isColliding)
	{
		%this.isColliding = "0";
		safeSchedule("100", %this, "stopCirculating");
		return;
	}
	%owner.setLayer($LAYER[layerToSet]);
	%owner.setPhysicsSuppress("0");
	%this.isCirculating = "0";
	targetObject.dismount();
	circulator.getBehavior("BeContinuouslyRotate").stopRotating();
	%this.startMoving();
	return;
}
function BeBat::onRotationStart(%this)
{
	if (Layer != "main")
	{
		return;
	}
	%this.stopMoving();
	$BatsCounted = $BatsCounted - 1.0;
	return;
}
function BeBat::onRotationFinish(%this)
{
	$BatsCounted = $BatsCounted + 1.0;
	%owner = Owner;
	%curRotation = -1.0 * camera.getCurrentRotation();
	%owner.setRotation(%curRotation);
	if (Layer != "main")
	{
		return;
	}
	if (isCirculating)
	{
		$BatsCirculating = $BatsCirculating + 1.0;
		%owner.mount(targetObject, "0 0", "3", "0", "0", "0", "0");
		%owner.setTimerOn("300");
	}
	else
	{
		if (camera.isInside(%owner, "CENTER", "RECT", "1"))
		{
			$BatsInsideCamera = $BatsInsideCamera + 1.0;
			$ownerlist = $ownerlist SPC %owner;
		}
		%this.startMoving();
	}
	%this.updateBatSounds();
	return;
}
function BeBat::switchOn(%this)
{
	%this.onRotationFinish();
	subscribeToEvents(%this, "onRotationStart onRotationFinish");
	return;
}
function BeBat::switchOff(%this)
{
	%this.onRotationStart();
	unsubscribeToEvents(%this, "onRotationStart onRotationFinish");
	return;
}
function BeBat::updateBatSounds(%this)
{
	if ($BatCount == $BatsCounted)
	{
		if ($BatsInsideCamera > 1.0)
		{
			playGameEventSound(ltrim($ownerlist), "FlyingBats");
		}
		else
		{
		}
		$BatsInsideCamera = 0;
		$ownerlist = "";
	}
	return;
}
function BatTrigger::createInstance(%owner)
{
	%reactTrigger = new t2dTrigger(Name : "")
	{
		class = "BatTrigger";
		scenegraph = scenegraph;
	}
	%reactTrigger.Bat = %owner;
	%reactTrigger.setGraphGroup($GROUPS["batTrigger"]);
	%reactTrigger.setCollisionActive("0", "1");
	%reactTrigger.setCollisionDetection("CIRCLE");
	%reactTrigger.setCollisionCircleScale("1");
	%reactTrigger.setCollisionCircleSuperscribed("0");
	%reactTrigger.setEnterCallback("1");
	%reactTrigger.setStayCallback("0");
	%reactTrigger.setLeaveCallback("0");
	%reactTrigger.setSize(t2dVectorScale(%owner.getSize(), 3.0 / 4.0));
	%reactTrigger.mount(%owner, "0 0", "0", "0", "1", "1", "0");
	return %reactTrigger;
	return %reactTrigger;
}
