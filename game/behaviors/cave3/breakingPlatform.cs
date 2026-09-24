// breakingPlatform.cs.dso
if (!(isObject(BeBreakingPlatform)))
{
	%template = new BehaviorTemplate(Name : BeBreakingPlatform);
	%template.friendlyName = "BreakingPlatform";
	%template.behaviorType = "LevelCave3";
	%template.description = "a breakable platform that only breaks if in view";
	%template.addBehaviorField(criticalSpeed, "the speed the object needs to break through", float, "90");
	%template.addBehaviorField(delayTime, "a delay before breaking (in seconds)", float, "1");
	%template.addBehaviorField(shakeAudioProfile, "the audioprofile that this object should play during shaking", string, "");
	%template.addBehaviorField(breakingAudioProfile, "the audioprofile that this object play during breaking", string, "");
	%template.addBehaviorField(breakoutAudioProfile, "the audioprofile that should be played on breakout", string, "");
	%template.addBehaviorField(collisionAudioProfile, "the audioprofile that this object should recevie for collisions (after its breakout", string, "");
	%template.addBehaviorField(stopper, "an object that lets the breakign off look more natural because collision with it will add angularVelocity (see branches)", object, null, t2dSceneObject);
	%template.addBehaviorField(breakOutOnlyInView, "break out if its in view", bool, "1");
	%template.addBehaviorField(influencedByPlayer, "if player pushes object", bool, "0");
	%template.addBehaviorField(isFreeFloater, "if this breaking thing should float without gravity", bool, "0");
}
function BeBreakingPlatform::onBehaviorAdd(%this)
{
	subscribeToEvents(%this, "onLevelLoadFinished9 onLevelLoadFinished20");
	return;
}
function BeBreakingPlatform::onLevelLoadFinished9(%this)
{
	%owner = Owner;
	%owner.escapeSwitch = "1";
	%reactiveBreak = %owner.addDependentBehavior("BeReactiveBreaking");
	%reactiveBreak.setCriticalSpeed(criticalSpeed);
	%reactiveBreak.quarryBehavior = %this;
	%reactiveBreak.shakeOffset = "10";
	%moveBehavior = %owner.getBehavior("BeMoving");
	%moveBehavior.Density = "0.05";
	%moveBehavior.Friction = "0.3";
	%moveBehavior.Restitution = "0";
	%moveBehavior.influencedByPlayer = influencedByPlayer;
	if (isFreeFloater)
	{
		%moveBehavior.isGravitic = "0";
		%moveBehavior.influencedByPlayer = "1";
	}
	if (isObject(stopper))
	{
		stopper.setCollisionActive("0", "1");
		stopper.setCollisionPhysics("0", "0");
		stopper.setGraphGroup($GROUPS["breakingStopper"]);
		stopper.setCollisionGroups($GROUPS["moving"]);
		%moveBehavior.CollisionGroups = CollisionGroups SPC "breakingStopper";
	}
	return;
}
function BeBreakingPlatform::onBehaviorRemove(%this)
{
	return;
}
function BeBreakingPlatform::onLevelLoadFinished20(%this)
{
	%owner = Owner;
	%this.delayedBreak = %owner.addDependentBehavior("BeDelayedBreaking");
	delayedBreak.setCloakObjects(cloakObjects);
	delayedBreak.breakDelay = delayTime;
	delayedBreak.breakingAudioProfile = breakingAudioProfile;
	%owner.shakeAudioProfile = shakeAudioProfile;
	%owner.breakoutAudioProfile = breakoutAudioProfile;
	%owner.collisionAudioProfile = collisionAudioProfile;
	if (collisionAudioProfile != "")
	{
		%owner.addCollisionSoundBehavior("40", collisionAudioProfile, "0", "0.6");
	}
	Owner.removeBehavior(breakingBehavior, "0");
	Owner.removeBehavior(delayedBreak, "0");
	if (currentLevelObject.getId() == level_cave3.getId())
	{
		%owner.quarryOutCallbackObject = achievements;
		achievements.demolitionManObjects = demolitionManObjects + 1.0;
		debugEcho("now having" SPC demolitionManObjects SPC "stones to break out");
	}
	return;
}
function BeBreakingPlatform::quarry(%this)
{
	%owner = Owner;
	if (breakOutOnlyInView && camera.isInside(%owner) || !(breakOutOnlyInView))
	{
		%owner.addBehavior(delayedBreak);
		delayedBreak.init();
		delayedBreak.quarry();
		if (isObject(stopper))
		{
			safeSchedule("1000", stopper, "safeDelete");
		}
		%owner.removeBehavior(%this);
		%owner.getBehavior("BeReactiveBreaking").quarryBehavior = "0";
	}
	return;
}
