// rippedEdge.cs.dso
if (!(isObject(BeRippedEdge)))
{
	%template = new BehaviorTemplate(Name : BeRippedEdge);
	%template.friendlyName = "Ripped Edge";
	%template.behaviorType = "MetaGameMechanisms";
	%template.description = "lets object fall out of the world here";
	%template.addBehaviorField(hasObjectCollision, "set this to true if objects should be able to fall out of the paper here", bool, "0");
	%template.addBehaviorField(Immovable, "guess what", bool, "1");
	%template.addBehaviorField(MountForce, "how strong should object be mounted to ripped edge mid-point", float, "3");
	%template.addBehaviorField(useStandardCollisionPoly, "false if you wanna use your own collision poly", bool, "1");
}
function BeRippedEdge::onBehaviorAdd(%this)
{
	%owner = Owner;
	if (%owner.getClassName() $= "t2dShapeVector")
	{
		%owner.setFillMode("1");
		%owner.setFillColor("0 0 0");
		%owner.setBlendColor("0 0 0");
	}
	subscribeToEvents(%this, "onLevelLoadFinished10");
	%owner.setLayer($LAYER["rippedEdge"]);
	return;
}
function BeRippedEdge::onLevelLoadFinished10(%this)
{
	%owner = Owner;
	if (%owner.getClassName() $= "t2dShapeVector")
	{
		%owner.setLineColor("0 0 0 0");
	}
	%owner.setGraphGroup($GROUPS["rippedEdge"]);
	%owner.setLayer($LAYER["rippedEdge"]);
	%owner.setImmovable(Immovable);
	if (!(hasObjectCollision))
	{
		if ($showDekoRippedEdges)
		{
			%owner.setBlendColor("1", "0", "0");
		}
		%owner.setCollisionActive("0", "0");
		%owner.setCollisionPhysics("0", "0");
		%this.setBehaviorCollisionCallback("0");
	}
	else
	{
		%owner.setCollisionActive("0", "1");
		%owner.setCollisionPhysics("0", "1");
		%this.setBehaviorCollisionReceiveCallback("1");
		%owner.setCollisionSuppress("1");
		if (useStandardCollisionPoly)
		{
			if (%owner.getImageMap() $= "risskante_tiledImageMap")
			{
				%owner.setCollisionPolyCustom("4", "-0.800 -1.000 1.000 -1.000 1.000 1.000 -0.800 1.000");
				break;
			}
			if (%owner.getImageMap() $= "risskante_small_tiledImageMap")
			{
				%owner.setCollisionPolyCustom("4", "-0.200 -1.000 1.000 -1.000 1.000 1.000 -0.200 1.000");
				break;
			}
			%owner.setCollisionPolyCustom("4", "-1.000 -1.000 1.000 -1.000 1.000 1.000 -1.000 1.000");
		}
		if (%owner.getLinkCount() > 0.0)
		{
			%this.mountPosition = %owner.getLocalPoint(%owner.getLinkPoint("1"));
		}
		else
		{
			if (%owner.getImageMap() $= "risskante_small_tiledImageMap")
			{
				%this.mountPosition = "2.0 0.0";
				break;
			}
			%this.mountPosition = "0.0 0.0";
		}
		%trigga = new t2dTrigger(Name : "")
		{
			scenegraph = daSceneGraph;
			class = "RippedEdgeSwitchTrigger";
			Position = %owner.getPosition();
			size = %owner.getSize();
		}
		%trigga.setGraphGroup($GROUPS["rippedEdgeTrigger"]);
		%trigga.setCollisionActive("0", "1");
		%trigga.setEnterCallback("1");
		%trigga.setStayCallback("0");
		%trigga.setLeaveCallback("1");
		%this.trigger = %trigga;
		%trigga.collisionShape = Owner;
		%trigga.setImmovable(Immovable);
		if (!(Immovable))
		{
			%trigga.mount(%owner, "0 0", "0", "1", "1", "1", "0");
		}
		else
		{
			%trigga.setRotation(%owner.getRotation());
		}
		%trigga.rippedEdgeEnterCounter = "0";
	}
	return;
}
function BeRippedEdge::onCollisionReceive(%srcObj, %dstObj, , , , , , )
{
	%this = %srcObj;
	%owner = Owner;
	if (isObject(shapeParent))
	{
		%triggerShape = %dstObj;
		%triggerShape.setCollisionSuppress("1");
		%dstObj = shapeParent;
	}
	if (isObject(fallOutParent))
	{
		%dstObj = fallOutParent;
	}
	if (isOuterSpace || camera.getIsRotating())
	{
		if (isObject(%triggerShape))
		{
			%triggerShape.setCollisionSuppress("0");
		}
		return;
	}
	%dstObj.shrinkAngularVelocity = 360.0 * getSign(%dstObj.getAngularVelocity());
	if (isPlayer(%dstObj))
	{
		%dstObj.dieOutside();
	}
	else
	{
		%dstObj.setPhysicsSuppress("1");
	}
	%dstObj.origMaxAngularVelocity = %dstObj.getMaxAngularVelocity();
	%dstObj.setMaxAngularVelocity("360");
	%dstObj.setAngularVelocity(shrinkAngularVelocity);
	%dstObj.isOuterSpace = "1";
	if (rippedEdgeMountForce $= "")
	{
	}
	else
	{
	}
	%force = rippedEdgeMountForce;
	%dstObj.mount(%owner, mountPosition, %force, "0", "0", "0", "0");
	if (!(isPlayer(%dstObj)))
	{
		%dstObj.BaseVelocityAdaptionFactor = "0";
	}
	%dstObj.getBehavior("BeShrinker").startShrinking();
	return;
}
function BeRippedEdge::switchOn(%this)
{
	Owner.setEnabled("1");
	if (hasObjectCollision)
	{
		trigger.setEnabled("1");
	}
	return;
}
function BeRippedEdge::switchOff(%this)
{
	Owner.setEnabled("0");
	if (hasObjectCollision)
	{
		trigger.setEnabled("0");
	}
	return;
}
function RippedEdgeSwitchTrigger::onEnter(%this, %obj)
{
	if (isObject(shapeParent))
	{
		%obj = shapeParent;
	}
	if (rippedEdgeEnterCounter == 0.0)
	{
		%obj.changeLayer("1");
	}
	if (isObject(fallOutPoly))
	{
		fallOutPoly.setRotation("0");
		fallOutPoly.setCollisionSuppress("0");
		fallOutPoly.mount(%obj, "-0.01 0.24", "0", "1", "1", "1", "0");
	}
	%obj.rippedEdgeEnterCounter = rippedEdgeEnterCounter + 1.0;
	%this.rippedEdgeEnterCounter = rippedEdgeEnterCounter + 1.0;
	collisionShape.setCollisionSuppress("0");
	return;
}
function RippedEdgeSwitchTrigger::onLeave(%this, %obj)
{
	if (isObject(shapeParent))
	{
		%obj = shapeParent;
	}
	%obj.rippedEdgeEnterCounter = rippedEdgeEnterCounter - 1.0;
	%this.rippedEdgeEnterCounter = rippedEdgeEnterCounter - 1.0;
	if (rippedEdgeEnterCounter == 0.0)
	{
		collisionShape.setCollisionSuppress("1");
	}
	if (isOuterSpace)
	{
		return %obj;
	}
	if (rippedEdgeEnterCounter == 0.0)
	{
		%obj.changeLayer("0");
		if (isObject(fallOutPoly))
		{
			fallOutPoly.setCollisionSuppress("1");
			fallOutPoly.dismount();
		}
	}
	return;
}
