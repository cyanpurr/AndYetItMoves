// objectManipulation.cs.dso
function t2dSceneObject::getRealAngularVelocity(%this)
{
	if (hasCustomAngularVelocity)
	{
		return t2dShortestAngleDifference(lastRotation, curRotation) * invDuration;
	}
	else
	{
		return %this.getAngularVelocity();
	}
	return %this.getAngularVelocity();
}
function t2dSceneObject::createCopy(%this)
{
	return %this.cloneWithBehaviors();
	return %this.cloneWithBehaviors();
}
function t2dSceneObject::getCollisionPolyExtent(%this)
{
	if (collisionPolyExtent $= "")
	{
		%this.collisionPolyExtent = getBoundingRect(%this.getCollisionPoly());
	}
	return collisionPolyExtent;
	return collisionPolyExtent;
}
function t2dSceneObject::getCollisionPolyWorldExtent(%this)
{
	if (collisionPolyWorldExtent $= "")
	{
		%poly = %this.getCollisionPoly();
		%i = 0;
		while (%i < %this.getCollisionPolyCount())
		{
			%point = %this.getWorldPoint(getWords(%poly, %i * 2.0, %i * 2.0 + 1.0));
			%poly = setWord(%poly, %i * 2.0, getX(%point));
			%poly = setWord(%poly, %i * 2.0 + 1.0, getY(%point));
			%i = %i + 1.0;
		}
		%this.collisionPolyWorldExtent = getBoundingRect(%poly);
	}
	return collisionPolyWorldExtent;
	return collisionPolyWorldExtent;
}
function t2dSceneObject::getCollisionPolyAbsWorldExtent(%this, %rotation)
{
	if (%rotation $= "")
	{
		%rotation = camera.getCurrentRotation();
	}
	%polyExtent = %this.getCollisionPolyWorldExtent();
	if (%rotation == 90.0)
	{
		%polyExtent = getWord(%polyExtent, "0") SPC getWord(%polyExtent, "3") SPC getWord(%polyExtent, "2") SPC getWord(%polyExtent, "1");
	}
	else
	{
		if (%rotation == 180.0)
		{
			%polyExtent = getWord(%polyExtent, "2") SPC getWord(%polyExtent, "3") SPC getWord(%polyExtent, "0") SPC getWord(%polyExtent, "1");
			break;
		}
		if (%rotation == 270.0)
		{
			%polyExtent = getWord(%polyExtent, "2") SPC getWord(%polyExtent, "1") SPC getWord(%polyExtent, "0") SPC getWord(%polyExtent, "3");
		}
	}
	return %polyExtent;
	return %polyExtent;
}
function t2dSceneObject::getCollisionPolySize(%this)
{
	return getRectSize(%this.getCollisionPolyWorldExtent());
	return getRectSize(%this.getCollisionPolyWorldExtent());
}
function t2dSceneObject::getAbsLowestPoint(%this)
{
	%poly = %this.getCollisionPoly();
	if (%poly $= "")
	{
		return "";
	}
	%i = 0;
	while (%i < %this.getCollisionPolyCount())
	{
		%lowestPoint = absRot(%this.getWorldPoint(getWords(%poly, %i, %i + 1.0)));
		%actPoint = absRot(%this.getWorldPoint(getWords(%poly, %i, %i + 1.0)));
		if (%lowestPoint $= "" || getY(%lowestPoint) < getY("1"))
		{
			%lowestPoint = %actPoint;
		}
		%i = %i + 1.0;
	}
	return %lowestPoint;
	return %lowestPoint;
}
function t2dSceneObject::getIsPointInObjectRange(%this, %x, %y, %dimension, %range, %absolute)
{
	if (getWordCount(%x) > 1.0)
	{
		%absolute = %range;
		%range = %dimension;
		%dimension = %y;
		%y = getY(%x);
		%x = getX(%x);
	}
	if (!(%this.getIsPointInObject(%x, %y)) || %range == 0.0)
	{
		return "0";
	}
	%cameraRotation = camera.getCurrentRotation();
	if (%absolute)
	{
		if (%cameraRotation == 180.0 || %cameraRotation == 90.0 && !(%dimension) || %cameraRotation == 270.0 && %dimension)
		{
			%range = %range * -1.0;
		}
		if (%cameraRotation == 90.0 || %cameraRotation == 270.0)
		{
			%dimension = !(%dimension);
		}
	}
	%localPoint = %this.getLocalPoint(%x, %y);
	%rightBottomRange = %range > 0.0;
	if (%rightBottomRange)
	{
	}
	else
	{
	}
	%range = mAbs(%range);
	%range = mAbs(%range);
	%extent = %this.getCollisionPolyExtent();
	%lower = getWord(%extent, %dimension);
	%upper = getWord(%extent, 2.0 + %dimension);
	%between = getWord(%localPoint, %dimension);
	%betweenInPercent = %between - %lower / %upper - %lower;
	if (%rightBottomRange)
	{
		return %betweenInPercent >= %range;
	}
	else
	{
		return %betweenInPercent <= %range;
	}
	return %betweenInPercent <= %range;
}
function t2dSceneObject::setPivotSize(%this, %size, %localPivot)
{
	if (%size $= "" || %localPivot $= "")
	{
		debugWarn("missing parameter in function 'setPivotSize'");
		return;
	}
	if (getX(%localPivot) == 0.0 && getY(%localPivot) == 0.0)
	{
		%this.setSize(%size);
		return;
	}
	%oldPivot = %this.getWorldPoint(%localPivot);
	%this.setSize(%size);
	%newPivot = %this.getWorldPoint(%localPivot);
	%offset = t2dVectorSub(%oldPivot, %newPivot);
	%this.setPosition(t2dVectorAdd(%this.getPosition(), %offset));
	return;
}
function t2dSceneObject::getNormedWorldPoint(%this, %localPoint)
{
	return t2dVectorScale(t2dVectorMult(%localPoint, %this.getSize()), "0.5");
	return t2dVectorScale(t2dVectorMult(%localPoint, %this.getSize()), "0.5");
}
function t2dSceneObject::getNormedLocalPoint(%this, %normedWorldPosition)
{
	return t2dVectorScale(t2dVectorDivide(%normedWorldPosition, %this.getSize()), "2");
	return t2dVectorScale(t2dVectorDivide(%normedWorldPosition, %this.getSize()), "2");
}
function t2dSceneObject::addCollisionGroups(%this, %groups)
{
	if (%groups $= "")
	{
		return;
	}
	%i = 0;
	while (%i < getWordCount(%groups))
	{
		%number = $GROUPS[getWord(%groups, %i)];
		if (%number $= "")
		{
			debugWarn("this group:" SPC getWord(%groups, %i) SPC "is not in $GROUPS");
		}
		else
		{
			%groupNumbers = %groupNumbers SPC %number;
		}
		%i = %i + 1.0;
	}
	%this.setCollisionGroups(%this.getCollisionGroups() SPC trim(%groupNumbers));
	return;
}
function t2dSceneObject::isCallableMethod(%this, %funcName)
{
	if (%this.isMethod(%funcName))
	{
		return "1";
	}
	%i = 0;
	while (%i < %this.getBehaviorCount())
	{
		%behavior = %this.getBehaviorByIndex(%i);
		if (%behavior.isMethod(%funcName))
		{
			return "1";
		}
		%i = %i + 1.0;
	}
	return "0";
	return "0";
}
function t2dSceneObject::setMountedCollidesNotImmovable(%this)
{
	%collideBehavior = %this.getBehavior("BeCollide");
	if (isObject(%collideBehavior))
	{
		%collideBehavior.setCollideImmovable("0");
	}
	%allChilds = %this.getMountedChildren();
	%i = 0;
	while (%i < getWordCount(%allChilds))
	{
		%child = getWord(%allChilds, %i);
		%child.setMountedCollidesNotImmovable();
		%i = %i + 1.0;
	}
	return getWordCount(%allChilds);
}
function t2dAnimatedSprite::getFrameCount(%this)
{
	return getWordCount(animationFrames);
	return getWordCount(animationFrames);
}
function t2dAnimatedSprite::getFrameTime(%this)
{
	return %this.getAnimationTime() / %this.getFrameCount();
	return %this.getAnimationTime() / %this.getFrameCount();
}
function GuiControl::setAllChildrenActive(%this, %state)
{
	%i = 0;
	while (%i != %this.getCount())
	{
		%currentObject = %this.getObject(%i);
		if (%currentObject.getClassName() $= "GuiControl" && isContainer)
		{
			%currentObject.setAllChildrenActive(%state);
		}
		%currentObject.setActive(%state);
		%i = %i + 1.0;
	}
	return %this.getCount();
}
