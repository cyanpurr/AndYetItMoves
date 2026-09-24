// trampolineBranch.cs.dso
if (!(isObject(BeTrampolineBranch)))
{
	%isTP = isTorquePlayer();
	if (!(%isTP))
	{
		%template = new BehaviorTemplate(Name : BeTrampolineBranch);
	}
	else
	{
		%template = new BeTrampolineBranchTemplate(Name : BeTrampolineBranch);
	}
	%template.friendlyName = "trampoline branch";
	%template.behaviorType = "LevelPrototype";
	%template.description = "part of a trampoline, that is fixed on one side";
	if (!(%isTP))
	{
		%template.addBehaviorField(trampolineNumber, "number of tramp", integer, "0");
		%template.addBehaviorField(isPivot, "if this part of tramp is the tramp pivot", bool, "0");
		%template.addBehaviorField(maxBendAngle, "the most outer part of the tramp will bend to this angle if player collides with max speed; only used if this is pivot", float, "38");
		%template.addBehaviorField(bendDuration, "time (sec) till bendAngle is reached if player collides with max speed on most outer trampPart; together with bend angle this determines the force with which branch snaps back; only used if this is pivot", float, "0.45");
	}
}
function BeTrampolineBranch::onBehaviorAdd(%this)
{
	subscribeToEvents(%this, "onLevelLoadFinished1 onLevelLoadFinished2 onLevelLoadFinished10 onLevelLoadFinished20");
	return;
}
function BeTrampolineBranch::onLevelLoadFinished1(%this)
{
	%this.states["bend"] = "1";
	%this.states["snapBack"] = "2";
	%this.states["idle"] = "3";
	%owner = Owner;
	%owner.setLayer($LAYER["main"] + 1.0);
	if (isPivot)
	{
		%this.maxBendAngle = "38";
		%this.bendDuration = "0.45";
		%this.state = states["idle"];
		%this.branchGroup = new SimSet(Name : "");
		levelGarbageCollector.add(branchGroup);
		branchGroup.add(%owner);
		%this.setName("branchPivotBehavior" @ trampolineNumber);
		subscribeToEvents(%this, "onRotationFinish");
	}
	%rotateBehavior = %owner.addDependentBehavior("BeRotate");
	%rotateBehavior.autoPlay = "0";
	%rotateBehavior.loop = "0";
	%rotateBehavior.symmetricAngle = "0";
	%rotateBehavior.use32msTick = "1";
	%rotateBehavior.pauseOnRotation = "1";
	if (!(isPivot))
	{
		%mountBehavior = %owner.addDependentBehavior("BeMountWithOffset");
	}
	%this.setBehaviorCollisionReceiveCallback("1");
	return;
}
function BeTrampolineBranch::onLevelLoadFinished2(%this)
{
	%owner = Owner;
	%this.additionalLethalSpeed = "90";
	%this.maxImpactSpeed = lethalSpeed + additionalLethalSpeed;
	%rotateBehavior = %owner.getBehavior("BeRotate");
	%rotateBehavior.sincParams = "3 2 7";
	if (isPivot)
	{
		%rotateBehavior.setRotateCallbackObject(%this);
	}
	if (!(isPivot))
	{
		%mountBehavior = %owner.getBehavior("BeMountWithOffset");
		%mountBehavior.searchParentCondition = "%parent.getBehavior(BeTrampolineBranch)";
	}
	%owner.origRotation = %owner.getRotation();
	return;
}
function BeTrampolineBranch::onLevelLoadFinished10(%this)
{
	%owner = Owner;
	%pivotBehavior = "branchPivotBehavior" @ trampolineNumber;
	%pivot = Owner;
	%this.pivotTrampBehavior = %pivotBehavior;
	%this.pivotDistance = t2dVectorLength(t2dVectorSub(%owner.getPosition(), %pivot.getLinkPoint("1")));
	pivotTrampBehavior.cntTrampPart = cntTrampPart + 1.0;
	pivotTrampBehavior.maxDistance = max(pivotDistance, maxDistance);
	if (!(isPivot))
	{
		branchGroup.add(%owner);
	}
	return;
}
function BeTrampolineBranch::onLevelLoadFinished20(%this)
{
	%owner = Owner;
	%this.distanceFactor = mLog10(pivotDistance / maxDistance * 9.0 + 1.0);
	%owner.normalLethalSpeed = lethalSpeed + distanceFactor * additionalLethalSpeed;
	%owner.lethalSpeed = normalLethalSpeed;
	return;
}
function BeTrampolineBranch::setDelayJump(%this, %delay)
{
	if (state == states["idle"] && %delay == 1.0)
	{
		return %this;
	}
	%owner = Owner;
	%i = 0;
	while (%i < branchGroup.getCount())
	{
		%partOwner = branchGroup.getObject(%i);
		%partOwner.jumpDelayed = %delay;
		%i = %i + 1.0;
	}
	return branchGroup.getCount();
}
function BeTrampolineBranch::onPlayerJump(%this)
{
	%owner = Owner;
	if (state == states["bend"])
	{
		%i = 0;
		while (%i < branchGroup.getCount())
		{
			%partOwner = branchGroup.getObject(%i);
			%partOwner.BaseVelocityAdaptionFactor = "0";
			%i = %i + 1.0;
		}
	}
	else
	{
		if (state == states["snapBack"])
		{
			%this.correctPlayerSpeed();
		}
	}
	return;
}
function BeTrampolineBranch::onStartFalling(%this)
{
	%this.correctPlayerSpeed();
	unSubscribeFromEvent(%this, "onStartFalling");
	return;
}
function BeTrampolineBranch::correctPlayerSpeed(%this)
{
	player.setHorizontalVelocity(move * 25.0);
	%maxVerticalSpeed = 270.0;
	%verticalPlayerSpeed = player.getVerticalVelocity();
	if (mAbs(%verticalPlayerSpeed) > %maxVerticalSpeed)
	{
		player.setVerticalVelocity(getSign(%verticalPlayerSpeed) * %maxVerticalSpeed);
	}
	return;
}
function BeTrampolineBranch::onPlayerDeath(%this)
{
	%owner = Owner;
	if (state == states["bend"])
	{
		safeSchedule(bendDuration * 500.0, %this, "snapBack");
	}
	return;
}
function BeTrampolineBranch::onBehaviorRemove(%this)
{
	return;
}
