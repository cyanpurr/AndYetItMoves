// saurian.cs.dso
if (!(isObject(BeSaurian)))
{
	%template = new BehaviorTemplate(Name : BeSaurian);
	%template.friendlyName = "Saurian";
	%template.behaviorType = "LevelCave4";
	%template.description = "a cave saurian that reacts on bats and prevents player from getting through";
	%template.addBehaviorField(batReactRadius, "the radius from the area the saurian reacts on bats", float, "50");
	%template.addBehaviorField(batReactCount, "the number of bats, that let the saurian leave", integer, "3");
	%template.addBehaviorField(tongueObject, "the tongue of the saurian", object, null, t2dSceneObject);
	%template.addBehaviorField(hideOut, "the hideOut of the saurian where he retracts after too many bats", object, null, t2dSceneObject);
	%template.addBehaviorField(soundTriggerSize, "how big the trigger is that starts the tense sound = how far away the player starts hearing the sound", float, "270");
}
function BeSaurian::onBehaviorAdd(%this)
{
	%owner = Owner;
	subscribeToEvents(%this, "onLevelLoadFinished4 onLevelLoadFinished10");
	%owner.isFled = "0";
	return;
}
function BeSaurian::onLevelLoadFinished4(%this)
{
	%owner = Owner;
	%this.translateBehavior = %owner.addDependentBehavior("BeTranslate");
	if ($WII)
	{
		hideOut.setImageMap(hideOutImageMap);
	}
	return;
}
function BeSaurian::onLevelLoadFinished10(%this)
{
	%owner = Owner;
	%this.reactTrigger = SaurianTrigger::createInstance(%owner);
	%this.shaker = createShaker();
	if (isObject(tongueObject))
	{
		%this.tongueSize = tongueObject.getSize();
		tongueObject.setPivotSize("0 0", tongueObject.getLocalPoint(tongueObject.getLinkPoint("1")));
	}
	else
	{
		debugWarn("saurian without a tongue:" SPC Owner);
	}
	if (isObject(hideOut))
	{
		%dist = t2dVectorSub(hideOut.getPosition(), %owner.getPosition());
		translateBehavior.setupTranslate(%dist, "2", "0", "0", "0", "0");
	}
	else
	{
		%owner.removeBehavior(translateBehavior);
		debugWarn("no hideOut set for saurian:" SPC %owner);
	}
	%this.limRotSavePoint = %owner.addDependentBehavior("BeLimitedRotationSavePoint");
	limRotSavePoint.glowingObject = %owner;
	%this.tenseSoundTrigger = new t2dTrigger(Name : "")
	{
		scenegraph = daSceneGraph;
		size = soundTriggerSize SPC soundTriggerSize;
		_behavior0 = "BePlaySound	profileName	CaveTenseAmbientBase	maxVolume	0.5	fullVolumeFactor	0.3	distanceCalculationMode	quadratic";
	}
	tenseSoundTrigger.setCollisionDetection("CIRCLE");
	tenseSoundTrigger.setCollisionCircleScale("1");
	tenseSoundTrigger.setCollisionCircleSuperscribed("1");
	tenseSoundTrigger.mount(%owner, "0 0", "0", "1", "1", "1", "0");
	return;
}
function BeSaurian::onBehaviorRemove(%this)
{
	return;
}
function BeSaurian::flee(%this)
{
	%owner = Owner;
	if (camera.isInside(reactTrigger, "CIRCLE", "RECT", -0.5))
	{
		%owner.isFled = "1";
		if (shakeSchedule)
		{
			shakeSchedule.cancelSchedule();
		}
		%owner.getBehavior("BeTranslate").init();
		%owner.getBehavior("BeTranslate").play();
		playDistanceHandleEventSound(%owner, "saurianFlees", SaurianDefeated, "0.5");
		safeSchedule("2000", %this, "releaseBats");
		limRotSavePoint.saveRotations();
		tenseSoundTrigger.getBehavior("BePlaySound").forcedFadeOut("5");
		safeSchedule("6000", tenseSoundTrigger, "safeDelete");
	}
	else
	{
		safeSchedule("300", %this, "flee");
	}
	return;
}
function BeSaurian::releaseBats(%this)
{
	%loopingsound = Owner.getBehavior("BePlayLoopingSound");
	%loopingsound.fadeOut();
	%i = 0;
	while (%i < batIntruders.getCount())
	{
		%bat = batIntruders.getObject(%i);
		%bat.stopCirculating();
		%i = %i + 1.0;
	}
	reactTrigger.safeDelete();
	return;
}
function BeSaurian::attack(%this)
{
	%owner = Owner;
	%angleToPlayer = getVectorAngle(t2dVectorSub(player.getPosition(), tongueObject.getLinkPoint("1")));
	tongueObject.setPivotRotation(%angleToPlayer, tongueObject.getLocalPoint(tongueObject.getLinkPoint("1")));
	tongueObject.setPivotSize(tongueSize, tongueObject.getLocalPoint(tongueObject.getLinkPoint("1")));
	safeSchedule("800", tongueObject, "setPivotSize", quoteString("0 0") @ ", " @ quoteString(tongueObject.getLocalPoint(tongueObject.getLinkPoint("1"))));
	playEventSound(SaurianSlurp, "0.9");
	player.lastDeathEvent = "saurian";
	player.dieExploding("50", "10");
	return;
}
function BeSaurian::beAngry(%this, %factor)
{
	if (shakeSchedule)
	{
		shakeSchedule.cancelSchedule();
	}
	%owner = Owner;
	shaker.init(%owner, 1.5 * %factor, t2dVectorScale("5 5", %factor), "INV_LONGSINC");
	shaker.start();
	%this.shakeSchedule = safeSchedule(1000.0 * getRandom() + 0.5, %this, "beAngry", %factor);
	return;
}
function SaurianTrigger::createInstance(%owner)
{
	%reactTrigger = new t2dTrigger(Name : "")
	{
		class = "SaurianTrigger";
		scenegraph = scenegraph;
	}
	%reactTrigger.Saurian = %owner;
	%saurianBehavior = %owner.getBehavior("BeSaurian");
	%reactTrigger.saurianBehavior = %saurianBehavior;
	%reactTrigger.batIntruders = new SimSet(Name : "");
	levelGarbageCollector.add(batIntruders);
	%reactTrigger.setGraphGroup($GROUPS["saurianTrigger"]);
	%reactTrigger.setCollisionActive("0", "1");
	%reactTrigger.setCollisionDetection("CIRCLE");
	%reactTrigger.setCollisionCircleScale("1");
	%reactTrigger.setCollisionCircleSuperscribed("0");
	%reactTrigger.setEnterCallback("1");
	%reactTrigger.setStayCallback("0");
	%reactTrigger.setLeaveCallback("0");
	%reactTrigger.setSize(2.0 * batReactRadius, 2.0 * batReactRadius);
	%reactTrigger.mount(%owner, "0 0", "0", "0", "1", "1", "0");
	return %reactTrigger;
	return %reactTrigger;
}
function SaurianTrigger::onEnter(%this, %intruder)
{
	if (isFled)
	{
		return Saurian;
	}
	if (%intruder.getId() == triggerShape.getId())
	{
		Saurian.attack();
	}
	else
	{
		if (!(batIntruders.isMember(%intruder)))
		{
			if (!(%intruder.getBehavior("BeBat")))
			{
				echo("WARNING: SaurianTrigger::onEnter intruder is not a bat but the code thinks so...returning" SPC %intruder);
				return;
			}
			batIntruders.add(%intruder);
			%intruder.circulate(%this.getPosition(), batReactRadius);
			%curBatCount = batIntruders.getCount();
			debugEcho("a new bat has entered:" SPC %intruder SPC "count:" SPC %curBatCount);
			%loopingsound = Saurian.getBehavior("BePlayLoopingSound");
			%loopingsound.volume = "0.2";
			if (%curBatCount == 1.0)
			{
				%loopingsound.setAudioProfile(OneBatCirculating);
				if (!(%loopingsound.getIsPlaying()))
				{
					%loopingsound.play();
				}
			}
			else
			{
				if (%curBatCount > 1.0)
				{
					%loopingsound.stop();
					%loopingsound.setAudioProfile(MoreBatsCirculating);
					%loopingsound.play();
				}
			}
			if (%curBatCount == batReactCount)
			{
				safeSchedule("500", saurianBehavior, "flee");
				break;
			}
			if (%curBatCount < batReactCount)
			{
				saurianBehavior.beAngry(%curBatCount / batReactCount);
			}
		}
	}
	return;
}
