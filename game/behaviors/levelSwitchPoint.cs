// levelSwitchPoint.cs.dso
if (!(isObject(BeLevelSwitchPoint)))
{
	%template = new BehaviorTemplate(Name : BeLevelSwitchPoint);
	%template.friendlyName = "Level switch Point";
	%template.behaviorType = "MetaGameMechanisms";
	%template.description = "this trigger marks end of level";
	%template.addBehaviorField(nextLevelName, "this is the next level to load. if this is empty switchpoint will just increment to next level", string, "");
}
function BeLevelSwitchPoint::onBehaviorAdd(%this)
{
	%owner = Owner;
	if (%owner.getClassName() != "t2dTrigger")
	{
		debugWarn("owner of spawn point behavior is not a trigger!");
		%owner.removeBehavior(%this);
		return;
	}
	%owner.addDependentBehavior("BeTrigger");
	%owner.setLeaveCallback("1");
	%owner.setStayCallback("0");
	subscribeToEvents(%this, "onLevelLoadFinished1 onLevelLoadFinished10 onLevelLoadFinished onRotationStart onRotationFinish onMenuReplayLevel onResetLevel");
	return;
}
function BeLevelSwitchPoint::onLevelLoadFinished1(%this)
{
	%owner = Owner;
	%this.spawnPoint = new t2dTrigger(Name : "")
	{
		scenegraph = scenegraph;
		_behavior0 = "BeSpawnPoint	number	100	Invisible	1";
	}
	spawnPoint.getBehavior("BeSpawnPoint").isLevelSwitchSpawnPoint = "1";
	spawnPoint.setCollisionSuppress("1");
	spawnPoint.mount(%owner, "0 0", "0", "1", "1", "1", "0");
	return;
}
function BeLevelSwitchPoint::onLevelLoadFinished10(%this)
{
	%owner = Owner;
	if ($WII)
	{
		blackTex.setImageMap(whiteImageMap1);
	}
	%this.visualPoint = new t2dStaticSprite(Name : "")
	{
		scenegraph = daSceneGraph;
		imageMap = "playerRun1ImageMap";
		frame = "0";
		_behavior0 = "BeMask	Layer	mainBehindPlayer	textureObject	blackTex	usePositive	1";
		_behavior1 = "BeDontCollide";
	}
	visualPoint.setSize(playerSize);
	visualPoint.setFlipX("1");
	visualPoint.mount(%owner, "0 0", "0", "1", "1", "1", "0");
	%bgObjects = scenegraph.pickPoint(%owner.getPosition(), $MASK_ALL_LIST, $LAYER["mainBehindPlayer"], "0", %owner);
	%numFound = getWordCount(%bgObjects);
	%i = 0;
	while (%i < %numFound)
	{
		%obj = getWord(%bgObjects, %i);
		if (%obj.getClassNamespace() $= "LevelSwitchMask")
		{
			%this.visualBackground = %obj;
			break;
		}
		%i = %i + 1.0;
	}
	%owner.setSize(player.getCollisionPolySize());
	return;
}
function BeLevelSwitchPoint::onLevelLoadFinished(%this)
{
	%owner = Owner;
	%number = getSpawnPointCount();
	spawnPoint.number = %number;
	spawnPoint.getBehavior("BeSpawnpoint").number = %number;
	globals.levelswitchPoint = %owner;
	if ($makeAllSpawnpointsInvisible)
	{
		visualBackground.setVisible("0");
		visualPoint.setVisible("0");
	}
	return;
}
function BeLevelSwitchPoint::onBehaviorRemove(%this)
{
	if (isObject(effect))
	{
		effect.stopEffect("0", "1");
	}
	return;
}
function BeLevelSwitchPoint::onEnter(%this, )
{
	achievements.onKamikaze();
	subscribeToEvents(%this, "onUpdateTick32ms onPlayerDeath");
	return;
}
function BeLevelSwitchPoint::onRotationStart(%this)
{
	%this.playerCantEnter = "1";
	return;
}
function BeLevelSwitchPoint::onRotationFinish(%this)
{
	%this.playerCantEnter = "0";
	%this.enterInTicks = "3";
	return;
}
function BeLevelSwitchPoint::onUpdateTick32ms(%this)
{
	%owner = Owner;
	if (isFalling || playerCantEnter || enterInTicks > 0.0)
	{
		%this.enterInTicks = enterInTicks - 1.0;
		return %this;
	}
	if (%this.enterCondition())
	{
		unSubscribeFromEvents(%this, "onPlayerDeath onUpdateTick32ms");
		%this.prepareLevelSwitch();
	}
	return;
}
function BeLevelSwitchPoint::enterCondition(%this)
{
	%owner = Owner;
	%enterGranted = mAbs(t2dShortestAngleDifference(camera.getCurrentRotation(), -1.0 * %owner.getRotation())) < 45.0;
	return %enterGranted;
	return %enterGranted;
}
function BeLevelSwitchPoint::onLeave(%this, )
{
	%owner = Owner;
	unSubscribeFromEvents(%this, "onPlayerDeath onUpdateTick32ms");
	return;
}
function BeLevelSwitchPoint::onPlayerDeath(%this)
{
	unSubscribeFromEvents(%this, "onPlayerDeath onUpdateTick32ms");
	return;
}
function BeLevelSwitchPoint::prepareLevelSwitch(%this)
{
	echo("level ended");
	setControlsEnabled("0");
	resistentMoveMap.pop();
	%owner = Owner;
	player.skipStateMachine = "1";
	callNextFrame(player, "setState, stand");
	player.setCollisionSuppress("1");
	fallOutPoly.setCollisionSuppress("1");
	player.getBehavior("BeGravitic").stopGravity();
	player.setRotation("0");
	if (!(dontMountPlayer))
	{
		player.mount(%owner, "0 0", "7", "1", "0", "0", "0");
	}
	player.setFlipX("1");
	player.setLayer($LAYER["mainBehindPlayer"] - 1.0);
	%this.createFadeShape();
	spawnPoint.setAsCurrent();
	triggerEvent("onEnterLevelSwitch");
	%this.startLevelSwitch();
	return;
}
function BeLevelSwitchPoint::createFadeShape(%this)
{
	if (isObject(fadeShape))
	{
		return isObject(fadeShape);
	}
	%this.fadeShape = new t2dShapeVector(Name : "")
	{
		scenegraph = scenegraph;
		PolyList = "-1.000 -1.000 1.000 -1.000 1.000 1.000 -1.000 1.000";
		FillMode = "1";
		Layer = "0";
	}
	%shapeSize = t2dVectorLength(viewWindow.getSize()) * 1.2000000476837158;
	fadeShape.setSize(%shapeSize, %shapeSize);
	fadeShape.mount(viewWindow, "0 0", "0", "1", "1", "0", "0");
	fadeShape.setBlendColor("0", "0", "0", "0");
	return;
}
function BeLevelSwitchPoint::startLevelSwitch(%this)
{
	%this.createParticleEffect();
	playEventSound(LevelFinishedNew, "0.65");
	%this.schedule("300", "startFadeOut");
	%this.schedule("3000", "endFadeOut");
	%this.schedule("2800", "fadeOutScreen");
	if (active && nextLevelName $= "" && !($watchAsReplay) || wholeEnvironment)
	{
		%this.schedule("3500", "displayStatistics");
	}
	else
	{
		%this.schedule("4000", "switchLevel");
	}
	return;
}
function BeLevelSwitchPoint::startFadeOut(%this)
{
	visualPoint.setVisible("0");
	player.setAlphaVelocity(-0.4000000059604645);
	return;
}
function BeLevelSwitchPoint::endFadeOut(%this)
{
	if (!($watchAsReplay))
	{
		triggerEvent("onLevelFadeOutEnded");
	}
	player.setAlphaVelocity("0");
	return;
}
function BeLevelSwitchPoint::fadeOutScreen(%this)
{
	%this.fadeShapeAlpha("1");
	return;
}
function BeLevelSwitchPoint::fadeShapeAlpha(%this, %alphaVelocity)
{
	%this.createFadeShape();
	fadeAllMusicAudioChannels("0");
	fadeShape.setAlphaVelocity(%alphaVelocity);
	return;
}
function BeLevelSwitchPoint::displayStatistics(%this)
{
	%this.statisticsDisplayed = "1";
	if (wholeEnvironment)
	{
		Statistics.environmentLevel = environmentLevel + 1.0;
	}
	if (wholeEnvironment && !(endOfEnvironment))
	{
		echo("starting next level" SPC currentLevelNumber + 1.0 SPC "ghosts" SPC environmentGhosts);
		if (environmentGhosts == -1.0)
		{
			echo("levelswitchpoint: displaystatistics: ghost not loaded" SPC getWord(environmentGhosts, environmentLevel) SPC "of environment" SPC environment);
		}
		else
		{
			echo("loading ghost in env" SPC environment SPC "ghostlist:" SPC environmentGhosts SPC "| current:" SPC environmentLevel);
			$ghostToLoad.delete();
			$ghostToLoad = Replay::load(environment, getWord(environmentGhosts, environmentLevel));
		}
		%this.schedule("500", "nextEnvironmentLevel");
		return;
	}
	if ($watchAsReplay)
	{
		%this.schedule("500", "switchLevel");
	}
	else
	{
		fadeShape.setAlphaVelocity("0");
		fadeShape.setBlendAlpha("0.7");
		$ACTUAL_MENU.enterMenu(menu_statistics);
	}
	return;
}
function BeLevelSwitchPoint::onMenuReplayLevel(%this, %levelCompleted)
{
	if (%levelCompleted $= "")
	{
		%levelCompleted = 1;
	}
	if (statisticsDisplayed)
	{
	}
	else
	{
	}
	%fadeSpeed = 4.0;
	if (statisticsDisplayed)
	{
	}
	else
	{
	}
	%schedLenght = 300;
	%this.fadeShapeAlpha(%fadeSpeed);
	%this.schedule(%schedLenght, "switchLevel", "0", %levelCompleted);
	return;
}
function BeLevelSwitchPoint::onResetLevel(%this)
{
	%this.onMenuReplayLevel("0");
	return;
}
function BeLevelSwitchPoint::nextEnvironmentLevel(%this)
{
	fadeShape.setAlphaVelocity("0");
	fadeShape.setBlendAlpha("1");
	if (!($watchAsReplay))
	{
		triggerEvent("onLevelCompleted");
	}
	startGame(currentLevelNumber + 1.0);
	return;
}
function BeLevelSwitchPoint::switchLevel(%this, %next, %levelCompleted)
{
	if (%next $= "")
	{
		%next = 1;
	}
	if (%levelCompleted $= "")
	{
		%levelCompleted = 1;
	}
	fadeShape.setAlphaVelocity("0");
	fadeShape.setBlendAlpha("1");
	if ($watchAsReplay)
	{
		menu_statistics.goOn("main", "1");
		return;
	}
	if (%levelCompleted)
	{
		triggerEvent("onLevelCompleted");
		triggerEvent("onSave");
	}
	if (%next)
	{
	}
	else
	{
	}
	%addToCurrentLevel = 0;
	%nextLevelNumber = currentLevelNumber + %addToCurrentLevel;
	echo("switching level: next one is" SPC %nextLevelNumber);
	if (nextLevelName != "")
	{
		debugEcho("nextlevelname:" SPC nextLevelName);
		startGame(nextLevelName);
		return;
	}
	else
	{
		if (%next && $previewVersion && %nextLevelNumber >= 8.0)
		{
			debugEcho("last level for wii-preview, going to menu!");
			menu_main.goToSubMenu = menu_level;
			MenuAction::loadLevel("level_gameMenu");
			subscribeToEvents(menu_main, "onMenuLoadFinished");
			break;
		}
		if (%next && %nextLevelNumber >= 19.0)
		{
			$settings::Secrets::LevelsUnlocked = 22;
			menu_main.goToEpilog = "1";
			menu_main.goToSubMenu = menu_level;
			MenuAction::loadLevel("level_gameMenu");
			subscribeToEvents(menu_main, "onMenuLoadFinished");
			break;
		}
		if ($SnowdriftlandSpecial)
		{
			MenuAction::loadLevel("level_gameMenu");
			break;
		}
		if (wholeEnvironment)
		{
			MenuAction::loadLevel("level_" @ environment);
			break;
		}
		MenuAction::loadLevel(getLevelScriptObject(%nextLevelNumber));
	}
	return;
}
function BeLevelSwitchPoint::createParticleEffect(%this)
{
	%owner = Owner;
	%effect = new t2dParticleEffect(Name : "")
	{
		scenegraph = daSceneGraph;
		effectFile = "~/data/particles/levelFinished.eff";
		useEffectCollisions = "0";
		effectMode = "KILL";
		effectTime = "1.5";
	}
	%emitter = %effect.getEmitterObject("0");
	%emitter.setAttachPositionToEmitter("1");
	%emitter.setAttachRotationToEmitter("1");
	%effect.mount(%owner, "0 0", "0", "1", "1", "0", "0");
	%effect.playEffect("0");
	%this.effect = %effect;
	return;
}
function LevelSwitchMask::onLevelLoaded(%this)
{
	%maskBehavior = %this.getBehavior("BeMask");
	%maskBehavior.Layer = "mainBehindPlayer";
	return;
}
