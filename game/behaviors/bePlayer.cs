// bePlayer.cs.dso
if (!(isObject(BePlayer)))
{
	%template = new BehaviorTemplate(Name : BePlayer);
	%template.friendlyName = "Be Player";
	%template.behaviorType = "Object";
	%template.description = "assigns this to the player";
}
function BePlayer::onBehaviorAdd(%this)
{
	%owner = Owner;
	if (!($watchAsReplay))
	{
		%owner.addDependentBehaviors("BeGravitic BeControlPlayerSounds BeKeepOrientation");
		%owner.setCollisionActive("1", "1");
		%owner.setCollisionPhysics("1", "0");
		%owner.setOwnerCollisionCallback("1");
	}
	else
	{
		%owner.setVisible("0");
		%owner.setCollisionActive("0", "0");
		%owner.setCollisionPhysics("0", "0");
		%owner.setOwnerCollisionCallback("0");
	}
	%owner.setLayer($LAYER["player"]);
	%owner.origLayer = %owner.getLayer();
	%owner.setGraphGroup($GROUPS["player"]);
	%owner.setCollisionGroups($GROUPS["collide"] SPC $GROUPS["moving"]);
	%owner.setCollisionMaxIterations("10");
	%owner.setDensity("0.01");
	%owner.setFriction("0.01");
	%owner.setRestitution("0.01");
	%owner.setCollisionPolyCustom("6", "-0.141 -0.697", "0.162 -0.697", "0.368 -0.099", "0.177 0.648", "-0.198 0.646", "-0.394 -0.067");
	%owner.moveSpeed = "48";
	%owner.jumpSpeed = "85";
	%owner.groundAccel = "800";
	%owner.groundBreak = "500";
	%owner.landingSlideFactor = "45";
	%owner.airAccel = "50";
	%owner.timeToFall = "0.3";
	%owner.maxSpeed = "300";
	%owner.maxSlope = "55";
	%owner.slideAnimSlope = "75";
	%owner.slideSlope = "89.9";
	%owner.upAngle = "90";
	%owner.lethalSpeed = "200";
	%owner.respawnSpeed = "300";
	%owner.spawnMountForce = "5";
	%owner.startFuchtelSpeed = lethalSpeed * 0.20000000298023224;
	%owner.startLethalFuchtelSpeed = lethalSpeed * 0.699999988079071;
	%owner.softLandSpeed = lethalSpeed * 0.30000001192092896;
	%owner.squashAngleTolerance = "30";
	%owner.squashCounterLimit = "6";
	%owner.impactDirMultiplier = "1";
	%owner.impactFragTreshold = "300";
	%owner.BaseVelocityAdaptionFactor = "1";
	%owner.baseVelocityAdaptionLimit = "1000";
	%owner.moveRight = "0";
	%owner.moveLeft = "0";
	%owner.baseVelocity = "0 0";
	%owner.localFootPoint = "0 0.35";
	%owner.initStateMachine();
	%owner.initAnimationNames();
	%owner.lastJumpTime = "0";
	%owner.isInCameraSnapper = "0";
	%owner.triggerShape = new t2dSceneObject(Name : "")
	{
		scenegraph = scenegraph;
		size = %owner.getSize();
		CollisionPolyList = %owner.getCollisionPoly();
		class = playerTrigger;
	}
	if (!($watchAsReplay))
	{
		triggerShape.setCollisionActive("1", "0");
		triggerShape.setCollisionPhysics("0", "0");
		triggerShape.setOwnerCollisionCallback("1");
	}
	else
	{
		triggerShape.setCollisionActive("0", "0");
		triggerShape.setCollisionPhysics("0", "0");
		triggerShape.setOwnerCollisionCallback("0");
	}
	triggerShape.setCollisionDetection("POLYGON");
	triggerShape.setGraphGroup($GROUPS["dontCollide"]);
	triggerShape.setCollisionGroups($GROUPS["playerTrigger"] SPC $GROUPS["rippedEdgeTrigger"]);
	triggerShape.addCollisionGroups(playerTriggerGroups);
	triggerShape.mount(%owner, "0 0", "0", "1");
	triggerShape.shapeParent = %owner;
	if (!(isAlterEgo(%owner)))
	{
		%this.rotationEffect = new t2dParticleEffect(Name : "")
		{
			scenegraph = scenegraph;
			effectFile = "~/data/particles/rotation.eff";
			useEffectCollisions = "0";
			effectMode = "INFINITE";
			effectTime = "1";
			canSaveDynamicFields = "1";
			Position = "0 0";
			size = "30.000 30.000";
			CollisionMaxIterations = "1";
			BaseVelocityAdaptionFactor = "-1";
		}
		rotationEffect.setLayer($LAYER["mainBehindPlayer"]);
		rotationEffect.mount(%owner, "0 0", "0", "0", "1", "1", "0");
	}
	%owner.getBehavior("BeKeepOrientation").useFrameUpdate = "1";
	%owner.getBehavior("BeKeepOrientation").orientationMode = "FULL";
	%owner.balloon = new t2dAnimatedSprite(Name : playerBalloon)
	{
		scenegraph = scenegraph;
		animationName = "baloonlinkImageAnimation";
		size = "44 44";
		Layer = $LAYER["player"];
	}
	balloon.setEnabled("0");
	subscribeToEvents(%this, "onLevelLoadFinished1 onLevelLoadFinished10 onLevelLoadFinished20 onUpdateFirstTick30");
	return;
}
function BePlayer::onLevelLoadFinished1(%this)
{
	%owner = Owner;
	if (isAlterEgo(%owner))
	{
		return isAlterEgo(%owner);
	}
	new t2dStaticSprite(Name : hintCircles)
	{
		scenegraph = scenegraph;
		imageMap = "circlesImageMap";
		size = $circlesSize;
		Layer = "0";
		_behavior0 = "BeTexture";
	}
	hintCircles.mount(player, "0 0", "0", "1", "1", "1");
	new t2dStaticSprite(Name : speedHint)
	{
		scenegraph = scenegraph;
		imageMap = "blendersImageMap";
		frame = "2";
		Position = player.getPosition();
		canSaveDynamicFields = "1";
		size = "10 70";
		GraphGroup = "31";
		LinkPoints = "0.0 -0.7";
		_behavior0 = "BeMountWithOffset	mother	player	trackRotation	0";
		_behavior1 = "BeRotate	autoPlay	0";
		_behavior2 = "BeMask	layer	custom	usePositive	1	textureObject	hintCircles";
		escapeSwitch = "1";
	}
	player.speedHintRotate = speedHint.getBehavior("BeRotate");
	speedHint.originalSize = speedHint.getSize();
	speedHint.setVisible("0");
	if ($WII)
	{
		if (environment == 0.0)
		{
			%envBlendColor = "0.059 0.749 0.898";
		}
		else
		{
			if (environment == 1.0)
			{
				%envBlendColor = "0.937 0.369 0.957";
				break;
			}
			if (environment == 2.0)
			{
				if (levelID $= "c31")
				{
					%envBlendColor = "0.937 0.369 0.957";
				}
				else
				{
					%envBlendColor = "0.094 0.745 0.310";
				}
				break;
			}
			if (environment == 3.0)
			{
				%envBlendColor = "0.749 0.039 0.043";
				break;
			}
			%envBlendColor = "1 0 0";
		}
		%envBlendColor = "1 1 1";
		speedHint.setBlendColor(%envBlendColor);
		%i = 0;
		while (%i < getWordCount(%envBlendColor))
		{
			%envBlendColor = setWord(%envBlendColor, %i, mRound(getWord(%envBlendColor, %i) * 255.0));
			%i = %i + 1.0;
		}
		rotation_marker.BlendColor = %envBlendColor SPC "255";
		rotation_marker_2side.BlendColor = %envBlendColor SPC "255";
		threshold_marker_1.BlendColor = "255 255 255 255";
		threshold_marker_2.BlendColor = "255 255 255 255";
		threshold_marker_2side.BlendColor = "255 255 255 255";
	}
	speedHint.setLayer("0");
	speedHint.setPosition(t2dVectorSub(player.getPosition(), t2dVectorSub(speedHint.getLinkPoint("1"), speedHint.getPosition())));
	return;
}
function BePlayer::onLevelLoadFinished10(%this)
{
	%owner = Owner;
	%shrinker = Owner.getBehavior("BeShrinker");
	%shrinker.setOnShrinkFinished("respawn");
	%shrinker.setShrinkChildren("0");
	%shrinker.resizeOnFinish = "1";
	return;
}
function BePlayer::onLevelLoadFinished20(%this)
{
	%owner = Owner;
	subscribeToEvents(%this, "onUpdateTick32ms onRotationStart onRotationUpdate onRotationFinish onPlayerStateChange");
	subscribeToEvents(%owner, "onRainStart");
	%owner.setMaxLinearVelocity(maxSpeed);
	%owner.fallOutPoly = new t2dSceneObject(Name : "")
	{
		scenegraph = %owner.getSceneGraph();
		size = "8.476 18.162";
		CollisionPolyList = "-0.708 -1.000 0.77 -1.000 0.796 -0.821 0.495 0.998 -0.490 0.998 -0.8 -0.732";
	}
	fallOutPoly.setCollisionActive("1", "0");
	fallOutPoly.setCollisionPhysics("0", "0");
	fallOutPoly.setCollisionGroups($GROUPS["rippedEdge"]);
	fallOutPoly.setRotation("0");
	fallOutPoly.setCollisionSuppress("1");
	fallOutPoly.fallOutParent = %owner;
	speedHint.setRepeatTexture("0");
	return;
}
function BePlayer::onBehaviorRemove(%this)
{
	if (isObject(rotationEffect))
	{
		rotationEffect.stopEffect("0", "1");
	}
	return;
}
function BePlayer::onUpdateTick32ms(%this)
{
	%owner = Owner;
	if (!(isDead))
	{
		%owner.updateMovement();
	}
	if (!(skipStateMachine))
	{
		%owner.updateStateMachine();
	}
	return;
}
function BePlayer::onUpdateFirstTick30(%this)
{
	setCustomSpawnPoint("1", "1");
	return;
}
function BePlayer::onPlayerStateChange(%this)
{
	return;
}
function BePlayer::onRotationStart(%this)
{
	%owner = Owner;
	%owner.isFalling = "1";
	%owner.jump = "0";
	unSubscribeFromEvents(%this, "onUpdateTick32ms");
	if (!(isAlterEgo(%owner)))
	{
		%emitter = rotationEffect.getEmitterObject("0");
		%animation = player.getAnimation();
		%frame = getWord(animationFrames, player.getAnimationFrame());
		%emitter.setImageMap(imageMap, %frame);
		rotationEffect.getEmitterObject("0").setFlipX(player.getFlipX());
		rotationEffect.playEffect("1");
	}
	player.setPauseAnimation("1");
	subscribeToEvents(Owner.getBehavior("BeGravitic"), "onRotationFinish");
	return;
}
function BePlayer::onRotationUpdate(%this)
{
	%owner = Owner;
	if (!(isAlterEgo(%owner)))
	{
		rotationEffect.setRotation(camera.getCurrentRotation());
	}
	if (Visible)
	{
		%shr = -1.0 * t2dRelativeAngleBetween(verticalVector, t2dVectorNormalise(linearVel)) - camera.getCurrentRotation();
		speedHintRotate.setPivotRotation(%shr);
	}
	return;
}
function BePlayer::onRotationFinish(%this)
{
	%owner = Owner;
	subscribeToEvents(%this, "onUpdateTick32ms");
	%owner.baseVelocity = "0 0";
	if (isMarkedForCollisionActivation)
	{
		%owner.isMarkedForCollisionActivation = "0";
		%owner.setCollisionSuppress("0");
		%owner.setPhysicsSuppress("0");
		triggerShape.setCollisionSuppress("0");
		%owner.setPosition(spawnPoint.getPosition());
	}
	if (!(isAlterEgo(%owner)))
	{
		rotationEffect.stopEffect("1");
	}
	player.setPauseAnimation("0");
	return;
}
