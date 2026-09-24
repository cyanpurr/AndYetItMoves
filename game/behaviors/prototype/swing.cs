// swing.cs.dso
if (!(isObject(BeSwing)))
{
	%isTP = isTorquePlayer();
	if (!(%isTP))
	{
		%template = new BehaviorTemplate(Name : BeSwing);
	}
	else
	{
		%template = new BeSwingTemplate(Name : BeSwing);
	}
	%template.friendlyName = "swing";
	%template.behaviorType = "LevelPrototype";
	%template.description = "give this to a swing and to its pivot";
	if (!(%isTP))
	{
		%template.addBehaviorField(part, "which part of a swing is this", enum, "pivot", "pivot	platform	lianaleft	lianaright	auxPlatform");
		%template.addBehaviorField(number, "numba", integer, "1");
		%template.addBehaviorField(useDelayedBreaking, "if this swing should get (reactive + delayed) breaking behavior (only valid for platform)", bool, "0");
		%template.addBehaviorField(groupedBreakingNumber, "if this is set to a number != 0 the swing will add a groupedbrekaing beahvior", integer, "0");
		%template.addBehaviorField(switchOffAtStart, "switch this swing off when level starts (only valid for pivot)", bool, "1");
		%template.addBehaviorField(freeFallDamping, "damping if player doesnt stand on swing (only valid for pivot)", float, "0.5");
		%template.addBehaviorField(rideOnDamping, "damping if player stands on swing (only valid for pivot)", float, "0.2");
		%template.addBehaviorField(GraphGroup, "collisiongroup to use for this swing (you have to set this for pivot AND platform!)", string, "jungleSwing");
		%template.addBehaviorField(deactivatable, "if the plattform shall be deactivated or not", bool, "1");
		break;
	}
	%template.addBehaviorField(part, "which part of a swing is this", enum, "pivot", "pivot	platform	lianaleft	lianaright	auxPlatform");
	%template.addBehaviorField(useDelayedBreaking, "if this swing should get (reactive + delayed) breaking behavior (only valid for platform)", bool, "0");
	%template.addBehaviorField(groupedBreakingNumber, "if this is set to a number != 0 the swing will add a groupedbrekaing beahvior", integer, "0");
	%template.addBehaviorField(switchOffAtStart, "switch this swing off when level starts (only valid for pivot)", bool, "1");
	%template.addBehaviorField(GraphGroup, "collisiongroup to use for this swing (you have to set this for pivot AND platform!)", string, "jungleSwing");
	%template.addBehaviorField(deactivatable, "if the plattform shall be deactivated or not", bool, "1");
}
function BeSwing::onBehaviorAdd(%this)
{
	%this.bowleRadius = "10";
	%owner = Owner;
	if (part $= "platform")
	{
		subscribeToEvents(%this, "onLevelLoadFinished1 onLevelLoadFinished2 onLevelLoadFinished5 onLevelLoadFinished10 onLevelLoadFinished15 onLevelLoadFinished40");
	}
	else
	{
		if (part $= "pivot")
		{
			subscribeToEvents(%this, "onLevelLoadFinished1 onLevelLoadFinished2 onLevelLoadFinished5");
			break;
		}
		if (part $= "auxPlatform")
		{
			subscribeToEvents(%this, "onLevelLoadFinished2");
			break;
		}
		subscribeToEvents(%this, "onLevelLoadFinished1 onLevelLoadFinished2");
	}
	return;
}
function BeSwing::onLevelLoadFinished1(%this)
{
	%this.setName("beSwing" @ part @ number);
	return;
}
function BeSwing::onLevelLoadFinished2(%this)
{
	%owner = Owner;
	%this.beSwingPlatform = ['"beSwingplatform"', 'number'];
	%this.beSwingPivot = ['"beSwingpivot"', 'number'];
	%this.beSwingLianaLeft = ['"beSwinglianaLeft"', 'number'];
	%this.beSwingLianaRight = ['"beSwinglianaRight"', 'number'];
	%owner.escapeSwitch = "1";
	if (part $= "lianaLeft" || part $= "lianaRight")
	{
		%this.createLiana();
	}
	else
	{
		if (part $= "platform")
		{
			%this.realOrigDistance = t2dVectorDistance(Owner.getPosition(), Owner.getPosition());
			%this.origDistance = realOrigDistance - 1.0;
		}
	}
	if (part $= "platform" || part $= "auxPlatform")
	{
		%owner.setGraphGroup($GROUPS["jungleSwingPlatform"]);
		%owner.setCollisionActive("0", "1");
		%owner.setCollisionPhysics("0", "0");
		%owner.setCollisionDetection("POLYGON");
		%owner.setImmovable("0");
		%owner.BaseVelocityAdaptionFactor = "1";
		%this.defaultLethalSpeed = "220";
		%this.rideOnLethalSpeed = "300";
		%owner.lethalSpeed = defaultLethalSpeed;
		%this.setBehaviorCollisionReceiveCallback("1");
		%owner.state = "stretch";
		%this.minRestitution = "0.5";
	}
	return;
}
function BeSwing::onLevelLoadFinished5(%this)
{
	%owner = Owner;
	if (part $= "pivot")
	{
		%owner.setCollisionCircleSuperscribed("0");
		%owner.setCollisionActive("0", "1");
		%owner.setCollisionPhysics("0", "0");
		%owner.setImmovable("1");
		%owner.setCollisionDetection("CIRCLE");
		%owner.setGraphGroup($GROUPS[GraphGroup]);
		%owner.setSize("2 2");
		%radius = bowleRadius + realOrigDistance;
		%owner.setCollisionCircleScale(%radius);
		%switchBehavior = %owner.addDependentBehavior("BeSwitch");
		%switchBehavior.toggles = beSwingPlatform.getName();
		%switchBehvaior.switchOffAtStart = switchOffAtStart;
		%activateBehavior = %owner.getBehavior("BeActivate");
		%activateBehavior.delaySwitchOff = "1";
		%activateBehavior.BehaviorList = "BeSwitch";
		%activateBehavior.factor = %radius;
		%activateBehavior.useInnerCircle = "1";
		%activateBehavior.activator = "viewWindowTrigger";
		%activateBehavior.suppressOwnerCollision = "0";
	}
	else
	{
		if (part $= "platform")
		{
			%this.createBowle();
		}
	}
	return;
}
function BeSwing::onLevelLoadFinished10(%this)
{
	%owner = Owner;
	bowl.setRestitution(minRestitution);
	bowl.setGraphGroup($GROUPS["dontCollide"]);
	bowl.setCollisionDetection("CUSTOM");
	bowl.setCollisionActive("1", "0");
	bowl.setCollisionPhysics("1", "0");
	triggerShape.safeDelete();
	%owner.setLayer($LAYER["main"]);
	daSceneGraph.pushToBack(%owner);
	return;
}
function BeSwing::onLevelLoadFinished15(%this)
{
	%owner = Owner;
	%collideBehavior = bowl.getBehavior("BeCollide");
	if (%collideBehavior)
	{
		%owner.setCollisionSuppress("1");
		bowl.setCollisionGroups($GROUPS["player"]);
		bowl.setFriction("1");
	}
	%owner.setGraphGroup($GROUPS["jungleSwingPlatform"]);
	%owner.setImmovable("0");
	%this.switchOff();
	return;
}
function BeSwing::onUpdateTick32ms10(%this)
{
	return;
}
function BeSwing::onUpdateTick15(%this)
{
	%owner = Owner;
	%vel = t2dVectorScale(t2dVectorSub(parentPlatform.getPosition(), %owner.getPosition()), invDuration);
	%owner.setLinearVelocity(%vel);
	return;
}
function BeSwing::resetLiana(%this)
{
	%owner = Owner;
	%i = 1;
	while (%i < lianaPartGroup.getCount())
	{
		%part = lianaPartGroup.getObject(%i);
		%part.getBehavior("BeRotate").setPivotVelocity("0");
		%part.getBehavior("BeRotate").setPivotRotation("0");
		%i = %i + 1.0;
	}
	return lianaPartGroup.getCount();
}
function BeSwing::freezeLiana(%this)
{
	%owner = Owner;
	%i = 0;
	while (%i < lianaPartGroup.getCount())
	{
		lianaPartGroup.getObject(%i).getBehavior("BeRotate").setPivotVelocity("0");
		%i = %i + 1.0;
	}
	return lianaPartGroup.getCount();
}
function BeSwing::createLiana(%this)
{
	%owner = Owner;
	%owner.setCollisionActive("0", "0");
	%owner.setGraphGroup($GROUPS["dontCollide"]);
	%owner.lianaId = ['part', 'number'];
	%owner.setLayer($LAYER["main"] - 1.0);
	scenegraph.bringToFront(%owner);
	%linkPointLocalCoords = %owner.getLocalPoint(%owner.getLinkPoint("1"));
	%owner.setPivotRotation("0", %linkPointLocalCoords);
	%distVec = t2dVectorSub(Owner.getPosition(), Owner.getPosition());
	%dist = t2dVectorLength(%distVec);
	%distNorm = t2dVectorScale(%distVec, 1.0 / %dist);
	beSwingPlatform.baseLianaVec = t2dVectorNormalise(t2dVectorSub(%owner.getPosition(), %owner.getLinkPoint("1")));
	%angle = t2dRelativeAngleBetween(%distNorm, baseLianaVec);
	%owner.setPivotRotation(%angle, %linkPointLocalCoords);
	%offset = mAbs(getWord(%owner.getLocalPoint(%owner.getLinkPoint("1")), "0")) * %owner.getSizeX();
	%this.numParts = mCeil(%dist / %offset);
	%owner.minParts = "3";
	%owner.maxParts = "9";
	%owner.minIndices = "1";
	%mountBehavior = %owner.addDependentBehavior("BeMountWithOffset");
	%mountBehavior.searchParentCondition = "%parent.getBehavior(BeSwing).part $= platform";
	%rotateBehavior = %owner.addDependentBehavior("BeRotate");
	%rotateBehavior.autoPlay = "0";
	%this.lianaPartGroup = new SimSet(Name : "");
	levelGarbageCollector.add(lianaPartGroup);
	lianaPartGroup.add(%owner);
	%i = 1;
	while (%i < numParts)
	{
		%nextPart = new t2dStaticSprite(Name : "")
		{
			imageMap = imageMap;
			scenegraph = scenegraph;
			size = %owner.getSize();
			lianaId = lianaId;
			Rotation = %owner.getRotation();
		}
		%nextPart.setLayer($LAYER["main"] - 1.0);
		%nextPart.escapeSwitch = "1";
		%nextPart.addDependentBehavior("BeDontCollide");
		%nextPart.addLinkPoint(%linkPointLocalCoords);
		%mountBehavior = %nextPart.addDependentBehavior("BeMountWithOffset");
		%mountBehavior.searchParentCondition = "%parent.lianaId $= %owner.lianaId";
		%mountBehavior.trackRotation = "1";
		%rotateBehavior = %nextPart.addDependentBehavior("BeRotate");
		%rotateBehavior.autoPlay = "0";
		%nextPart.setPosition(t2dVectorAdd(%owner.getPosition(), t2dVectorScale(%distNorm, %offset * %i)));
		lianaPartGroup.add(%nextPart);
		%i = %i + 1.0;
	}
	return %this;
}
function BeSwing::switchOn(%this)
{
	if (part $= "platform")
	{
		%platform = Owner;
		%bowl = bowl;
		%bowl.setCollisionSuppress("0");
		if (!(%bowl.getBehavior("BeReactiveBreaking")))
		{
			%platform.setCollisionSuppress("0");
		}
		if (%bowl.getBehavior("BeMoving"))
		{
			subscribeToEvents(%this, "onUpdateTick10");
			%bowl.getBehavior("BeGravitic").switchOn();
		}
	}
	return;
}
function BeSwing::switchOff(%this)
{
	if (part $= "platform")
	{
		%owner = Owner;
		%platform = Owner;
		%bowl = bowl;
		unSubscribeFromEvents(%this, "onUpdateTick10");
		beSwingLianaLeft.freezeLiana();
		beSwingLianaRight.freezeLiana();
		if (!(deactivatable))
		{
			return %this;
		}
		%bowlGravitic = %bowl.getBehavior("BeGravitic");
		if (isObject(%bowlGravitic))
		{
			%bowlGravitic.switchOff();
		}
		%bowl.setCollisionSuppress("1");
		%platform.setAtRest();
		%platform.setCollisionSuppress("1");
	}
	return;
}
function BeSwing::onQuarryOut(%this)
{
	%owner = Owner;
	bowl.dismount();
	%owner.setCollisionSuppress("0");
	bowl.setCollisionDetection("CUSTOM");
	bowl.setCollisionActive("1", "0");
	bowl.setMaxAngularVelocity("0");
	bowl.removeBehavior(bowl.getBehavior("BeReactiveBreaking"));
	bowl.removeBehavior(bowl.getBehavior("BeDelayedBreaking"));
	bowl.removeBehavior(bowl.getBehavior("BeBreaking"));
	bowl.removeBehavior(bowl.getBehavior("BePlaySound"));
	bowl.removeBehavior(bowl.getBehavior("BeShrinker"));
	%this.switchOn();
	triggerShape.safeDelete();
	bowl.setFriction("0");
	return;
}
function SwingBowle::onCollision(%this, %dstObject, , , , , , )
{
	if (!(wantsCollisionCallback))
	{
		return %this;
	}
	if (%dstObject.getGraphGroup() == $GROUPS[GraphGroup])
	{
		%this.lianaCollisionCheck = "1";
	}
	else
	{
		if (%dstObject.getGraphGroup() == $GROUPS["collide"] && isSwingStopper)
		{
			if (thisTime - lastGroundCollisionTime > 0.20000000298023224)
			{
				Owner.BaseVelocityAdaptionFactor = "0";
				if (resetAdaptionFactorScheduleID)
				{
					cancel(resetAdaptionFactorScheduleID);
				}
				%this.resetAdaptionFactorScheduleID = %this.schedule("600", "resetAdaptionFactor");
			}
			%this.lastGroundCollisionTime = thisTime;
		}
	}
	return;
}
function SwingBowle::resetAdaptionFactor(%this)
{
	Owner.BaseVelocityAdaptionFactor = "1";
	return;
}
function BeSwing::changeLayer(%this, %above)
{
	BeShrinker::changeLayer(%this, %above);
	return;
}
function BeSwing::createBowle(%this)
{
	%owner = Owner;
	%bowle = new t2dSceneObject(Name : "")
	{
		class = "SwingBowle";
		size = %owner.getSize();
		Rotation = %owner.getRotation();
		Position = %owner.getPosition();
		CollisionPolyList = %owner.getCollisionPoly();
		scenegraph = daSceneGraph;
		escapeSwitch = "1";
	}
	if (!($levelLoadedFinished))
	{
		if (useDelayedBreaking)
		{
			%reactiveBreakingBehavior = %bowle.addDependentBehavior("BeReactiveBreaking");
			%reactiveBreakingBehavior.criticalSpeed = "0";
			%reactiveBreakingBehavior.beginShakeFactor = "1";
			%reactiveBreakingBehavior.playerMustStandOn = "1";
			%delayedBreakingBehavior = %bowle.addDependentBehavior("BeDelayedBreaking");
			%bowle.breakoutAudioProfile = "BranchBreak";
			%bowle.quarryOutCallbackObject = %this;
		}
		%platformMountBehavior = %owner.getBehavior("BeMountWithOffset");
		if (isObject(%platformMountBehavior))
		{
			%bowleMountBehavior = %bowle.addDependentBehavior("BeMountWithOffset");
			%bowleMountBehavior.mother = mother;
			%bowleMountBehavior.offset = offset;
			%bowleMountBehavior.rotationCorretion = rotationCorretion;
			%bowleMountBehavior.trackRotation = trackRotation;
			%owner.removeBehavior(%platformMountBehavior);
		}
	}
	%owner.mount(%bowle, "0 0", "0", "0", "1");
	%movingBehavior = %bowle.addDependentBehavior("BeMoving");
	%movingBehavior.CollisionGroups = GraphGroup SPC "collide" SPC "moving";
	%movingBehavior.Density = 0.03999999910593033 / %bowle.getSizeX() * %bowle.getSizeY();
	%movingBehavior.Friction = "0";
	%movingBehavior.Restitution = "0.7";
	%movingBehavior.init();
	%bowle.beSwingPlatform = %this;
	%bowle.lastGroundCollisionTime = thisTime;
	%bowle.setGraphGroup($GROUPS["dontCollide"]);
	%bowle.setCollisionCircleSuperscribed("0");
	%bowle.setCollisionCircleScale(bowleRadius * 2.0 / max(%bowle.getSizeX(), %bowle.getSizeY()));
	%bowle.setOwnerCollisionCallback("1");
	%bowle.setMaxAngularVelocity("0");
	%bowle.setDamping(freeFallDamping);
	%bowle.lastPosition = %bowle.getPosition();
	%bowle.removeBehavior(%bowle.getBehavior(BeShrinker));
	%bowle.setRestitution(minRestitution);
	%bowle.setGraphGroup($GROUPS["dontCollide"]);
	%bowle.setCollisionDetection("CUSTOM");
	%bowle.setCollisionActive("1", "0");
	%bowle.setCollisionPhysics("1", "0");
	triggerShape.safeDelete();
	%this.bowl = %bowle;
	%bowle.setMaxLinearVelocity("400");
	%owner.setMaxLinearVelocity(%bowle.getMaxLinearVelocity());
	return;
}
