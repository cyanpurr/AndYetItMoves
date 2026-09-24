// spawnPoint.cs.dso
$spawnPointTriggerSize = "13 38";
$makeAllSpawnpointsInvisible = 0;
if (!(isObject(BeSpawnPoint)))
{
	if (!(isTorquePlayer()))
	{
		%template = new BehaviorTemplate(Name : BeSpawnPoint);
	}
	else
	{
		%template = new BeSpawnPointTemplate(Name : BeSpawnPoint);
	}
	%template.friendlyName = "Spawn Point";
	%template.behaviorType = "MetaGameMechanisms";
	%template.description = "makes this trigger a spawn point for the player";
	%template.addBehaviorField(number, "number of spawnpoint in order of levelappearence", integer, "1");
	%template.addBehaviorField(mainPoint, "if the spawnpoint is the main point in it's 'group'-number", bool, "1");
	%template.addBehaviorField(zoomFactor, "the zoomfactor the camera should be at this spawnpoint(just for debug?)", float, "1");
	%template.addBehaviorField(directionToGo, "which direction the player should go to reach the goal from here", enum, "right", "right	left	up	down");
	%template.addBehaviorField(lookOtherDirection, "only works for up n down.. just an optical issue", bool, "0");
	%template.addBehaviorField(animateActivated, "if visual should switch to animation when sp is activated", bool, "1");
	%template.addBehaviorField(Invisible, "if this spawonpoint should be hidden", bool, "0");
}
function getSpawnPoint(%number)
{
	%i = 0;
	while (%i < spawnPointGroup.getCount())
	{
		%spawnPoint = spawnPointGroup.getObject(%i);
		if (number == %number && mainPoint)
		{
			return %spawnPoint;
		}
		%i = %i + 1.0;
	}
	debugWarn("no main spawnpoint with number:" SPC %number);
	return "0";
	return "0";
}
function getAllSpawnPoints()
{
	%spawnPointList = "";
	%i = 0;
	while (%i < spawnPointGroup.getCount())
	{
		%spawnPoint = spawnPointGroup.getObject(%i);
		if (mainPoint)
		{
			%spawnPointList = %spawnPointList SPC %spawnPoint;
		}
		%i = %i + 1.0;
	}
	%spawnPointList = ltrim(%spawnPointList);
	if (%spawnPointList)
	{
		return %spawnPointList;
	}
	else
	{
		debugWarn("no main spawnpoints are set in this level.");
		return "0";
	}
	return "0";
}
function getSpawnPointCount()
{
	return spawnPoints + 1.0;
	return spawnPoints + 1.0;
}
function BeSpawnPoint::onBehaviorAdd(%this)
{
	%owner = Owner;
	if (%owner.getClassName() != "t2dTrigger")
	{
		debugWarn("owner of spawn point behavior is not a trigger!");
		%owner.removeBehavior(%this);
		return;
	}
	%owner.setSize($spawnPointTriggerSize);
	if (number == 1.0)
	{
		$initialSpawnPoint = Owner;
	}
	%owner.addDependentBehavior("BeTrigger");
	%owner.setLeaveCallback("1");
	subscribeToEvents(%this, "onLevelLoadFinished3 onLevelLoadFinished7 onLevelLoadFinished16");
	return;
}
function BeSpawnPoint::onLevelLoadFinished3(%this)
{
	if (playmodeManager.isTimeMode())
	{
		%this.initTimeDisplay();
	}
	Owner.escapeSwitch = "1";
	return;
}
function BeSpawnPoint::onLevelLoadFinished7(%this)
{
	%owner = Owner;
	%potentialZoomScalers = scenegraph.pickPoint(%owner.getPosition(), $MASK_ALL_LIST, "0", "0", %owner);
	%i = 0;
	while (%i < getWordCount(%potentialZoomScalers))
	{
		%zoomScaler = getWord(%potentialZoomScalers, %i).getBehavior("BeZoomScaling");
		if (%zoomScaler)
		{
			%this.zoomFactor = %zoomScaler.getZoomFactor(%owner.getPosition());
			break;
		}
		%i = %i + 1.0;
	}
	%owner.number = number;
	%owner.zoomFactor = zoomFactor;
	%owner.mainPoint = mainPoint;
	%this.activated = "0";
	%this.spawnPointIsFlipped = directionToGo $= "right" || lookOtherDirection && directionToGo $= "up" || directionToGo $= "down";
	spawnPointGroup.add(%owner);
	if (!(Invisible))
	{
		%this.createParticleEffect();
		%this.visualSpawnPoint = new t2dParticleEffect(Name : "")
		{
			scenegraph = scenegraph;
			effectFile = "~/data/particles/inactiveSpawnpoint-randomMovement.eff";
			useEffectCollisions = "0";
			effectMode = "INFINITE";
			effectTime = "0";
			canSaveDynamicFields = "1";
			size = "30.000 30.000";
			CollisionMaxIterations = "1";
			BaseVelocityAdaptionFactor = "-1";
			_behavior0 = "BeDontCollide";
		}
		visualSpawnPoint.getEmitterObject("0").setFlipX(spawnPointIsFlipped);
		visualSpawnPoint.setLayer($LAYER["mainBehindPlayer"]);
		visualSpawnPoint.mount(%owner, "0 0", "0", "1", "1", "0", "0");
		visualSpawnPoint.playEffect("1");
		if ($makeAllSpawnpointsInvisible)
		{
			visualSpawnPoint.setVisible("0");
		}
		%this.createPointingSpawnpoint();
		%owner.originalRotation = %owner.getRotation();
	}
	return;
}
function BeSpawnPoint::onLevelLoadFinished16(%this)
{
	$cheatsWereUsed = 0;
	%this.init();
	if (number == 1.0)
	{
		$firstSpawnPoint = Owner;
	}
	if (number > spawnPoints && number != 100.0)
	{
		globals.spawnPoints = number;
	}
	return;
}
function BeSpawnPoint::onBehaviorRemove(%this)
{
	if (isObject(effect))
	{
		effect.stopEffect("0", "1");
	}
	if (isObject(visualSpawnPoint))
	{
		visualSpawnPoint.stopEffect("0", "1");
	}
	return;
}
function BeSpawnPoint::initTimeDisplay(%this)
{
	%owner = Owner;
	if (number == 1.0)
	{
		return %this;
	}
	%this.timeDisplaySet = new SimSet(Name : "");
	levelGarbageCollector.add(timeDisplaySet);
	%rotation = %owner.getRotation();
	%offset = 0.5;
	%startpoint = -1.0 * %offset * 4.0 SPC -1.2000000476837158;
	%this.timeDisplayBackground = new t2dStaticSprite(Name : "")
	{
		scenegraph = scenegraph;
		imageMap = "ziffernImageMap";
		frame = "14";
		canSaveDynamicFields = "1";
		size = "48 7";
		_behavior0 = "BeKeepOrientation";
	}
	timeDisplayBackground.setPosition(%owner.getWorldPoint("0 -1.2"));
	timeDisplayBackground.setRotation(%rotation);
	timeDisplayBackground.setLayer("1");
	%keepBehavior = timeDisplayBackground.addDependentBehavior("BeKeepOrientation");
	%keepBehavior.orientationMode = "FULL";
	%keepBehavior.useFrameUpdate = "0";
	timeDisplayBackground.addLinkPoint(timeDisplayBackground.getLocalPoint(%owner.getPosition()));
	%mountBehavior = timeDisplayBackground.addDependentBehavior("BeMountWithOffset");
	%mountBehavior.mother = %owner;
	%mountBehavior.trackRotation = "0";
	%rotateBehavior = timeDisplayBackground.addDependentBehavior("BeRotate");
	%rotateBehavior.autoPlay = "0";
	%i = 0;
	while (%i < 9.0)
	{
		%pic = new t2dStaticSprite(Name : "")
		{
			scenegraph = scenegraph;
			imageMap = "ziffernImageMap";
			frame = "15";
			canSaveDynamicFields = "1";
			size = "6 6";
			BaseVelocityAdaptionFactor = "-1";
		}
		%pic.setLayer("1");
		%pic.mount(timeDisplayBackground, timeDisplayBackground.getLocalPoint(%owner.getWorldPoint(t2dVectorAdd(%startpoint, %offset * %i SPC "0"))), "0", "1", "1", "1", "0");
		timeDisplaySet.add(%pic);
		%i = %i + 1.0;
	}
	return;
}
function BeSpawnPoint::hideTimeDisplay(%this)
{
	if (number == 1.0)
	{
		return %this;
	}
	timeDisplayBackground.setVisible("0");
	%i = 0;
	while (%i < timeDisplaySet.getCount())
	{
		%digit = timeDisplaySet.getObject(%i);
		%digit.setVisible("0");
		%i = %i + 1.0;
	}
	return timeDisplaySet.getCount();
}
function BeSpawnPoint::init(%this)
{
	%owner = Owner;
	if (spawnPathGroup.getCount())
	{
		%pathPoint = findNearestNode(%owner);
		%owner.pathIndex = getWord(%pathPoint, "0");
		%owner.pathPoint = getWord(%pathPoint, "1");
	}
	return;
}
function BeSpawnPoint::onEnter(%this, %object)
{
	%owner = Owner;
	%owner.enteredLast = %object;
	$lastSpawnpointEntered = %this;
	triggerEvent("onSpawnpointEnter");
	if (activated || number == 1.0)
	{
		return %this;
	}
	%this.timeEntered = Statistics.getLevelTime();
	if (number < number)
	{
		if (isFalling)
		{
			subscribeToEvents(%this, "onPlayerLanded onPlayerDeath");
			break;
		}
		%this.setAsCurrent();
	}
	return;
}
function BeSpawnPoint::onLeave(%this, %object)
{
	%owner = Owner;
	%owner.lastLeft = %object;
	triggerEvent("onSpawnpointLeave");
	return;
}
function BeSpawnPoint::onPlayerLanded(%this)
{
	%this.setAsCurrent();
	unSubscribeFromEvents(%this, "onPlayerLanded onPlayerDeath");
	return;
}
function BeSpawnPoint::onPlayerDeath(%this)
{
	unSubscribeFromEvents(%this, "onPlayerLanded onPlayerDeath");
	return;
}
function BeSpawnPoint::setAsCurrent(%this)
{
	%owner = Owner;
	if (isLevelSwitchSpawnPoint)
	{
		%this.timeEntered = Statistics.getLevelTime();
	}
	player.spawnPoint = %owner;
	%i = 0;
	while (%i < spawnPointGroup.getCount())
	{
		%actualSpawnPointBehavior = spawnPointGroup.getObject(%i).getBehavior("BeSpawnpoint");
		if (number <= number)
		{
			%actualSpawnPointBehavior.activate();
		}
		%i = %i + 1.0;
	}
	triggerEvent("onSpawnPointActivate");
	if (!(Invisible))
	{
		playHandleEventSound("spActivate", SpawnPointNew, "0.3");
	}
	return;
}
function BeSpawnPoint::activate(%this)
{
	%owner = Owner;
	if (activated)
	{
		return %this;
	}
	%this.activated = "1";
	if (!(Invisible))
	{
		effect.playEffect("1");
		%this.schedule(effectTime * 500.0, "activatePointingSpawnpoint");
	}
	return;
}
function BeSpawnPoint::finishRespawn(%this)
{
	player.mount(Owner, "0 0", spawnMountForce, "0", "0", "0", "0");
	%this.timeMounted = thisTime;
	subscribeToEvents(%this, "onUpdateTick10");
	return;
}
function BeSpawnPoint::createParticleEffect(%this)
{
	%owner = Owner;
	%this.effect = new t2dParticleEffect(Name : "")
	{
		scenegraph = daSceneGraph;
		class = "SpawnpointActivateEffect";
		effectFile = "~/data/particles/spawnPointActive.eff";
		useEffectCollisions = "0";
		effectMode = "KILL";
		effectTime = "1.5";
	}
	effect.setSize(spawnPointSize);
	effect.setLayer($LAYER["mainBehindPlayer"]);
	%emitter = effect.getEmitterObject("0");
	%emitter.setAttachPositionToEmitter("1");
	%emitter.setAttachRotationToEmitter("1");
	%emitter.setFlipX(spawnPointIsFlipped);
	if ($makeAllSpawnpointsInvisible)
	{
		%emitter.setVisible("0");
	}
	effect.mount(%owner, "0 0", "0", "1", "1", "0", "0");
	return;
}
function BeSpawnPoint::createPointingSpawnpoint(%this)
{
	%owner = Owner;
	%this.pointingSpawnPoint = new t2dAnimatedSprite(Name : "")
	{
		scenegraph = scenegraph;
		class = "SpawnPointAnimation";
		animationName = "spawnPointInactiveAnimation";
		size = spawnPointSize;
		_behavior0 = "BeDontCollide";
	}
	pointingSpawnPoint.spawnpointBehavior = %this;
	pointingSpawnPoint.setFlipX(spawnPointIsFlipped);
	pointingSpawnPoint.setLayer($LAYER["mainBehindPlayer"]);
	pointingSpawnPoint.mount(%owner, "0 0", "0", "1", "1", "0", "0");
	pointingSpawnPoint.setVisible("0");
	return;
}
function BeSpawnPoint::activatePointingSpawnpoint(%this)
{
	%owner = Owner;
	visualSpawnPoint.safeDelete();
	if (!($makeAllSpawnpointsInvisible))
	{
		pointingSpawnPoint.setVisible("1");
	}
	if (directionToGo $= "up")
	{
		pointingSpawnPoint.setAnimation("spawnpointPointingUpStartAnimation");
	}
	else
	{
		if (directionToGo $= "down")
		{
			pointingSpawnPoint.setAnimation("spawnpointPointingDownStartAnimation");
			break;
		}
		pointingSpawnPoint.setFlipX(spawnPointIsFlipped);
		pointingSpawnPoint.setAnimation("spawnpointPointingStartAnimation");
	}
	pointingSpawnPoint.activated = "1";
	return;
}
function BeSpawnPoint::switchOff(%this)
{
	Owner.setCollisionSuppress("1");
	return;
}
function BeSpawnPoint::switchOn(%this)
{
	Owner.setCollisionSuppress("0");
	return;
}
function SpawnPointAnimation::onAnimationEnd(%this)
{
	if (activated && !(Invisible))
	{
		if (directionToGo $= "up")
		{
			%this.setAnimation("spawnpointPointingUpAnimation");
			break;
		}
		if (directionToGo $= "down")
		{
			%this.setAnimation("spawnpointPointingDownAnimation");
			break;
		}
		%this.setAnimation("spawnpointPointingAnimation");
	}
	return;
}
function initSpawnPointTextures()
{
	if (!(isObject(tex_main)))
	{
		debugWarn("no main texture 'tex_main' found for this level!");
		return;
	}
	return;
}
function setSpawnPointsInvisible(%flag)
{
	$makeAllSpawnpointsInvisible = %flag;
	return;
}
