// mountWithOffset.cs.dso
if (!(isObject(bemountwithoffset)))
{
	%template = new BehaviorTemplate(Name : bemountwithoffset);
	%template.friendlyName = "Mount With Offset";
	%template.behaviorType = "Positioning";
	%template.description = "mounts this object to given parent with the offset it has in TGB";
	%template.addBehaviorField(mother, "the object ot mount to", object, null, t2dSceneObject);
	%template.addBehaviorField(force, "the force with wich it should be mounted", float, "0");
	%template.addBehaviorField(trackRotation, "should we track the rotation of the parent", bool, "0");
	%template.addBehaviorField(MountOwned, "delete this when parent is deleted", bool, "0");
	%template.addBehaviorField(searchParentCondition, "if mother is omitted, you can search for mount parent by this string. default is searching for a parent with the same class as owner.", string, "%parent.getClassNamespace() $= %owner.getClassNamespace()");
}
function bemountwithoffset::onBehaviorAdd(%this)
{
	subscribeToEvents(%this, "onLevelLoadFinished0 onLevelLoadFinished4 onLevelLoadFinished4_5 onLevelLoadFinished15");
	return;
}
function bemountwithoffset::onLevelLoadFinished0(%this)
{
	%owner = Owner;
	%owner.preMountPosition = %owner.getPosition();
	return;
}
function bemountwithoffset::onLevelLoadFinished4(%this)
{
	$mwoCounter = $mwoCounter + 1.0;
	%this.init();
	return;
}
function bemountwithoffset::init(%this)
{
	%owner = Owner;
	if (!(isObject(mother)))
	{
		if (searchParentCondition $= "")
		{
			%owner.removeBehavior(%this);
			return;
		}
		if (%owner.getLinkCount() > 0.0)
		{
		}
		else
		{
		}
		%pointToPick = %owner.getPosition();
		%potentialParents = daSceneGraph.pickPoint(%pointToPick);
		%i = 0;
		while (%i < getWordCount(%potentialParents))
		{
			%parent = getWord(%potentialParents, %i);
			%isParent = 0;
			%evalString = "%isParent = " @ searchParentCondition;
			if (getSubStr(%evalString, strlen(%evalString) - 1.0, "1") != ";")
			{
				%evalString = %evalString @ ";";
			}
			eval(%evalString);
			if (%isParent && %parent.getId() != %owner.getId())
			{
				%this.mother = %parent;
			}
			%i = %i + 1.0;
		}
	}
	if (!(isObject(mother)))
	{
		debugEcho("mountcondition failed:" SPC searchParentCondition SPC "found no mother for" SPC %owner SPC "pos:" SPC %owner.getPosition());
		unSubscribeFromEvents(%this, "onLevelLoadFinished4_5 onLevelLoadFinished15");
		return;
	}
	return;
}
function bemountwithoffset::onLevelLoadFinished4_5(%this)
{
	%owner = Owner;
	if (!(%owner.getBehavior("BeRotate")))
	{
		%this.findMountRoot();
	}
	%this.calculateOffset();
	return;
}
function bemountwithoffset::onLevelLoadFinished15(%this)
{
	%owner = Owner;
	%this.mountWithOffset();
	return;
}
function bemountwithoffset::calculateOffset(%this)
{
	%owner = Owner;
	if (%owner.getBehavior("BeRotate"))
	{
	}
	else
	{
	}
	%pos = %owner.getPosition();
	%this.offset = %this.getPositionOffset(%pos);
	if (trackRotation)
	{
		%this.rotationCorretion = mother.getRotation();
	}
	return;
}
function bemountwithoffset::findMountRoot(%this)
{
	%owner = Owner;
	%cnt = 0;
	while (1)
	{
		%cnt = %cnt + 1.0;
		if (!(isObject(mother)) || %cnt > 100.0)
		{
			debugEcho(mother SPC "actually set!! with bahaviors" SPC mother.getRealBehaviorList());
			debugEcho("ATTENTION: broke searching absolute mount root after" SPC %cnt SPC " loops for" SPC %owner);
			return;
		}
		%allBehaviors = mother.getRealBehaviorList();
		%rootBehaviors = "BeContinuouslyRotate BeTranslate BePulsatingObject BeBreakingPlatform BeTrampolineBranch BeBreaking";
		if (!(findWord(%allBehaviors, "BeMountWithOffset")) || containsAnyWord(%allBehaviors, %rootBehaviors))
		{
			break;
		}
		if (findWord(%allBehaviors, "BeRotate"))
		{
			%motherRotateBehavior = mother.getBehavior("BeRotate");
			%this.mother = pivot;
			break;
		}
		%motherMountBehavior = mother.getBehavior("BeMountWithOffset");
		%this.mother = mother;
	}
	return;
}
function bemountwithoffset::setupMountWithOffset(%this, %mother, %force, %trackRotation, %mountOwned)
{
	if (%mother $= "")
	{
	}
	else
	{
	}
	%this.mother = %mother;
	if (%force $= "")
	{
	}
	else
	{
	}
	%this.force = %force;
	if (%trackRotation $= "")
	{
	}
	else
	{
	}
	%this.trackRotation = %trackRotation;
	if (%mountOwned $= "")
	{
	}
	else
	{
	}
	%this.MountOwned = %mountOwned;
	return;
}
function bemountwithoffset::mountWithOffset(%this)
{
	%owner = Owner;
	%child = Owner;
	while (%child.getIsMounted())
	{
		%child = %child.getMountedParent();
	}
	if (getWordCount(offset) != 2.0)
	{
		%this.offset = %this.getPositionOffset(%child.getPosition());
	}
	if (trackRotation)
	{
		if (rotationCorretion $= "")
		{
			%this.rotationCorretion = mother.getRotation();
		}
		%child.setRotation(%child.getRotation() - rotationCorretion);
	}
	%this.originalRotation = %owner.getRotation();
	%this.mountID = %child.mount(mother, offset, force, trackRotation, "1", MountOwned, "0");
	return;
}
function bemountwithoffset::getPositionOffset(%this, %position)
{
	%offset = mother.getLocalPoint(%position);
	if (mother.getFlipX())
	{
		%offset = setWord(%offset, "0", -1.0 * getWord(%offset, "0"));
	}
	if (mother.getFlipY())
	{
		%offset = setWord(%offset, "1", -1.0 * getWord(%offset, "1"));
	}
	return %offset;
	return %offset;
}
function bemountwithoffset::setMountOffsetCorrection(%this, %normedWorldCorrection)
{
	%localCorrection = mother.getNormedLocalPoint(%normedWorldCorrection);
	%correctedOffset = t2dVectorAdd(offset, %localCorrection);
	mother.setLinkPoint(mountID, getX(%correctedOffset), getY(%correctedOffset));
	return;
}
function bemountwithoffset::resetMountOffset(%this)
{
	mother.setLinkPoint(mountID, getX(offset), getY(offset));
	return;
}
function bemountwithoffset::remount(%this, %sendToMount, %resetRotation)
{
	%owner = Owner;
	if (%resetRotation)
	{
		if (%owner.getIsMounted())
		{
			%owner.dismount();
		}
		%owner.setRotation(originalRotation);
	}
	%this.mountID = %owner.mount(mother, offset, force, trackRotation, %sendToMount, MountOwned, "0");
	return;
}
function bemountwithoffset::setMother(%this, %mother)
{
	%this.mother = %mother;
	return;
}
function bemountwithoffset::setForce(%this, %force)
{
	%this.force = %force;
	return;
}
function t2dSceneObject::saveDismount(%this)
{
	if (!(%this.getIsMounted()))
	{
		return %this.getIsMounted();
	}
	%this.saveDismountParent = %this.getMountedParent();
	%this.saveDismountOffset = MountOffset;
	%this.saveDismountForce = %this.getMountForce();
	%this.saveDismountTrackRotation = %this.getMountTrackRotation();
	%this.saveDismountOwnedByMount = %this.getMountOwned();
	%this.saveDismountInheritAttributes = %this.getMountInheritAttributes();
	%this.saveDismountRotation = %this.getRotation() - %this.getMountedParent().getRotation();
	%this.dismount();
	if (saveDismountTrackRotation)
	{
		%this.setRotation(saveDismountRotation);
	}
	return;
}
function t2dSceneObject::saveRemount(%this)
{
	if (!(isObject(saveDismountParent)))
	{
		return isObject(saveDismountParent);
	}
	if (!(saveDismountParent.getSceneGraph()))
	{
		if (!(isObject(childrenToRemountSet)))
		{
			saveDismountParent.childrenToRemountSet = new SimSet(Name : "");
			levelGarbageCollector.add(childrenToRemountSet);
		}
		childrenToRemountSet.add(%this);
	}
	else
	{
		%this.mount(saveDismountParent, saveDismountOffset, saveDismountForce, saveDismountTrackRotation, "1", saveDismountOwnedByMount, saveDismountInheritAttributes);
	}
	return;
}
