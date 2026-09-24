// growingRoot.cs.dso
if (!(isObject(BeGrowingRoot)))
{
	%template = new BehaviorTemplate(Name : BeGrowingRoot);
	%template.friendlyName = "Growing Root";
	%template.behaviorType = "LevelCave4";
	%template.description = "behavior for a root animation";
	%template.addBehaviorField(growthStage, "0 for water receiver, 1 for first root stage, 2...", integer, "1");
	%template.addBehaviorField(firstRootPart, "the first root part that starts the chain reaction", bool, "0");
	%template.addBehaviorField(groupNumber, "number of the wallgroup - 0 makes a single part without group - ONLY FOR THE ROOT-ROOT", integer, "1");
	%template.addBehaviorField(spreadList, "a list of framenumbers (begin with 0) at which the root(s) at the respective linkpoint start to grow (-1 is at last) - leave empty if no spreading", string, "");
	%template.addBehaviorField(objectList, "a list of objects which are infected by this rootat the respective linkpoint at frame from frameList", string, "");
}
function BeGrowingRoot::onBehaviorAdd(%this)
{
	%owner = Owner;
	subscribeToEvents(%this, "onLevelLoadFinished5 onLevelLoadFinished15");
	%owner.setLayer($LAYER["main"] - 1.0);
	return;
}
function BeGrowingRoot::onLevelLoadFinished5(%this)
{
	%owner = Owner;
	%this.receiveWaterDrops = growthStage == 0.0;
	if (receiveWaterDrops)
	{
		%groupedBreak = %owner.addDependentBehavior("BeGroupedBreaking");
		%groupedBreak.setGroupNumber(groupNumber);
		%this.growNR = "0";
	}
	else
	{
		%owner.setCollisionSuppress("1");
		%owner.setCollisionGroups("");
		%owner.setVisible("0");
		%this.animationName = %owner.getAnimationName();
		if (firstRootPart)
		{
			rootsGroup.add(%this);
		}
	}
	%owner.setCollisionPhysics("0", "0");
	return;
}
function BeGrowingRoot::onLevelLoadFinished15(%this)
{
	%owner = Owner;
	%owner.setGraphGroup($GROUPS["growingRoot"]);
	%owner.setCollisionActive("0", "1");
	if (receiveWaterDrops)
	{
		%this.setBehaviorCollisionReceiveCallback("1");
		%this.limRotSavePoint = %owner.addDependentBehavior("BeLimitedRotationSavePoint");
		limRotSavePoint.glowingObject = %owner;
	}
	return;
}
function BeGrowingRoot::onCollisionReceive(%this, %dstObj, , , , , , )
{
	if (!(wantsCollisionReceiveCallback))
	{
		return %this;
	}
	if (class != "WaterDrop")
	{
		return;
	}
	%owner = Owner;
	if (!(receiveWaterDrops))
	{
		debugWarn("this part:" SPC %owner SPC "should not receive collision on its behavior!");
		return;
	}
	if (!(isGrowing))
	{
		%this.growNR = growNR + 1.0;
		if (growNR == 3.0)
		{
			safeSchedule("3000", %owner.getBehavior("BeGroupedBreaking"), "quarry", "true");
			schedule("3000", "0", playEventSound, StoneBreakBig, "0.6");
			schedule("3250", "0", playEventSound, StoneBreakSmall, "0.5");
			schedule("3600", "0", playEventSound, StoneBreakMedium, "0.4");
			limRotSavePoint.saveRotations();
		}
		camera.shake(growNR * 2.0 + 5.0, "2");
		%this.setIsGrowing("1");
		%i = 0;
		while (%i < rootsGroup.getCount())
		{
			%obj = rootsGroup.getObject(%i);
			if (growthStage == growNR)
			{
				%obj.startGrow();
			}
			%i = %i + 1.0;
		}
	}
	%this.schedule("3000", "setIsGrowing", "false");
	return;
}
function BeGrowingRoot::setIsGrowing(%this, %growing)
{
	%this.isGrowing = %growing;
	return;
}
function BeGrowingRoot::startGrow(%this)
{
	%owner = Owner;
	%animFrameTime = %owner.getFrameTime();
	%lastFrameNR = %owner.getFrameCount() - 1.0;
	%owner.setVisible("1");
	%owner.playAnimation(animationName);
	playHandleEventSound("rootsGrow", GrowingRoots, "0.5");
	playHandleEventSound("growingRootsQuake", GrowingRootsQuake, "0.5");
	%i = 0;
	while (%i < getWordCount(spreadList))
	{
		%spreadAt = getWord(spreadList, %i);
		if (%spreadAt == -1.0)
		{
			%spreadAt = %lastFrameNR;
		}
		%nextObject = getWord(objectList, %i);
		%startIn = %spreadAt + 1.0 * %animFrameTime;
		%rootBehavior = %nextObject.getBehavior("BeGrowingRoot");
		%dripBehavior = %nextObject.getBehavior("BeDrip");
		if (isObject(%rootBehavior))
		{
			%rootBehavior.schedule(%startIn * 1000.0, "startGrow");
		}
		else
		{
			if (isObject(%dripBehavior))
			{
				%dripBehavior.schedule(%startIn * 1000.0, "breakOff");
				break;
			}
			debugWarn("unknown object type given in BeBreakingRoot - objectList (root" SPC %owner SPC "):" SPC %nextObject);
		}
		%i = %i + 1.0;
	}
	return getWordCount(spreadList);
}
