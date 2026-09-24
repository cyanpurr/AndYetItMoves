// projectManagement.cs.dso
$currentProject = "";
function exitDemo()
{
	if ($runWithEditors)
	{
		toggleLevelEditor();
	}
	else
	{
		quit();
	}
	return;
}
function _initializeProject()
{
	if ($WII)
	{
		%dbFile = expandFilename("game/managed/wiidatablocks.cs");
		%userGUIProfileFile = expandFilename("game/gameScripts/guiProfilesWii.cs");
	}
	else
	{
		%dbFile = expandFilename("game/managed/datablocks.cs");
		%userGUIProfileFile = expandFilename("game/gameScripts/guiProfiles.cs");
	}
	%persistFile = expandFilename("game/managed/persistent.cs");
	%brushFile = expandFilename("game/managed/brushes.cs");
	%behaviorsDirectory = expandFilename("game/behaviors");
	%userDatablockFile = expandFilename("game/gameScripts/datablocks.cs");
	%languageDirectory = expandFilename("Language");
	%resPath = expandFilename($resourceFolderName);
	addResPath(%resPath);
	if (!(isObject($dependentResourceGroup)))
	{
		$dependentResourceGroup = new SimGroup(Name : "");
	}
	%resList = getDirectoryList(%resPath, "0");
	%resCount = getFieldCount(%resList);
	%resCount = 0;
	%i = 0;
	while (%i < %resCount)
	{
		%resName = getField(%resList, %i);
		%resFile = %resPath @ "/" @ %resName;
		%resObject = ResourceObject::load(%resFile);
		if (!(isObject(%resObject)))
		{
			error(" % Game Resources : FAILED Loading Resource" SPC %resName);
			while ()
			{
				while ()
				{
				}
				echo(" % Game Resources : Loaded Resource" SPC %resName);
			}
			%entry = new ScriptObject(Name : "")
			{
				Name = %resName;
			}
			$dependentResourceGroup.add(%entry);
		}
		%i = %i + 1.0;
	}
	if (isFile(%dbFile) || isFile(%dbFile @ ".dso"))
	{
		exec(%dbFile);
	}
	if (!(isObject($managedDatablockSet)))
	{
		$managedDatablockSet = new SimSet(Name : "");
	}
	if (isFile(%persistFile) || isFile(%persistFile @ ".dso"))
	{
		exec(%persistFile);
	}
	if (!(isObject($persistentObjectSet)))
	{
		$persistentObjectSet = new SimSet(Name : "");
	}
	if (isFile(%brushFile) || isFile(%brushFile @ ".dso"))
	{
		exec(%brushFile);
	}
	if (!(isObject($brushSet)))
	{
		$brushSet = new SimSet(Name : "");
	}
	addResPath(%userDatablockFile);
	if (isFile(%userDatablockFile))
	{
		exec(%userDatablockFile);
	}
	addResPath(%behaviorsDirectory);
	if (!($executeMinimum))
	{
		execBehaviors(%behaviorsDirectory);
	}
	addResPath(%userGUIProfileFile);
	if (isFile(%userGUIProfileFile))
	{
		exec(%userGUIProfileFile);
	}
	addResPath(%languageDirectory);
	return;
}
function execBehaviors(%behaviorsDir)
{
	exec(%behaviorsDir @ "/activate.cs");
	exec(%behaviorsDir @ "/audioFader.cs");
	exec(%behaviorsDir @ "/backgroundPaper.cs");
	exec(%behaviorsDir @ "/beGravitic.cs");
	exec(%behaviorsDir @ "/bePlayer.cs");
	exec(%behaviorsDir @ "/behaviorUtilities.cs");
	exec(%behaviorsDir @ "/breaking.cs");
	exec(%behaviorsDir @ "/cameraSnapping.cs");
	exec(%behaviorsDir @ "/cloak.cs");
	exec(%behaviorsDir @ "/collide.cs");
	exec(%behaviorsDir @ "/continuouslyRotate.cs");
	exec(%behaviorsDir @ "/controlMonsterSounds.cs");
	exec(%behaviorsDir @ "/customTextureAngle.cs");
	exec(%behaviorsDir @ "/customizeTextureName.cs");
	exec(%behaviorsDir @ "/dekoTilemap.cs");
	exec(%behaviorsDir @ "/delayedBreaking.cs");
	exec(%behaviorsDir @ "/demoEndTrigger.cs");
	exec(%behaviorsDir @ "/dontCollide.cs");
	exec(%behaviorsDir @ "/dropDownCallbacks.cs");
	exec(%behaviorsDir @ "/groupedBreaking.cs");
	exec(%behaviorsDir @ "/hintTrigger.cs");
	exec(%behaviorsDir @ "/killPlayer.cs");
	exec(%behaviorsDir @ "/levelSwitchPoint.cs");
	exec(%behaviorsDir @ "/limitedRotationSavePoint.cs");
	exec(%behaviorsDir @ "/lookAtRotate.cs");
	exec(%behaviorsDir @ "/mask.cs");
	exec(%behaviorsDir @ "/mountWithOffset.cs");
	exec(%behaviorsDir @ "/moving.cs");
	exec(%behaviorsDir @ "/multiMask.cs");
	exec(%behaviorsDir @ "/partOfPlayer.cs");
	exec(%behaviorsDir @ "/playCollisionSound.cs");
	exec(%behaviorsDir @ "/playLoopingSound.cs");
	exec(%behaviorsDir @ "/playSingleSound.cs");
	exec(%behaviorsDir @ "/playSound.cs");
	exec(%behaviorsDir @ "/quirkyMovement.cs");
	exec(%behaviorsDir @ "/reactOnCollision.cs");
	exec(%behaviorsDir @ "/reactiveBreaking.cs");
	exec(%behaviorsDir @ "/rippedEdge.cs");
	exec(%behaviorsDir @ "/rotate.cs");
	exec(%behaviorsDir @ "/screenRuler.cs");
	exec(%behaviorsDir @ "/shrinker.cs");
	exec(%behaviorsDir @ "/spawnObjects.cs");
	exec(%behaviorsDir @ "/spawnPath.cs");
	exec(%behaviorsDir @ "/spawnPoint.cs");
	exec(%behaviorsDir @ "/splashScreener.cs");
	exec(%behaviorsDir @ "/switch.cs");
	exec(%behaviorsDir @ "/texture.cs");
	exec(%behaviorsDir @ "/textureShaker.cs");
	exec(%behaviorsDir @ "/translate.cs");
	exec(%behaviorsDir @ "/trigger.cs");
	exec(%behaviorsDir @ "/zoomFader.cs");
	exec(%behaviorsDir @ "/zoomScaling.cs");
	exec(%behaviorsDir @ "/bonus/moveDirection.cs");
	exec(%behaviorsDir @ "/cave3/breakingPlatform.cs");
	exec(%behaviorsDir @ "/cave3/spidernet.cs");
	exec(%behaviorsDir @ "/cave3/weighter.cs");
	exec(%behaviorsDir @ "/cave4/bat.cs");
	exec(%behaviorsDir @ "/cave4/batBlocker.cs");
	exec(%behaviorsDir @ "/cave4/drip.cs");
	exec(%behaviorsDir @ "/cave4/growingRoot.cs");
	exec(%behaviorsDir @ "/cave4/saurian.cs");
	exec(%behaviorsDir @ "/finalLevel/randomRotationFade.cs");
	exec(%behaviorsDir @ "/jungle/banana.cs");
	exec(%behaviorsDir @ "/jungle/bug.cs");
	exec(%behaviorsDir @ "/jungle/dionaea.cs");
	exec(%behaviorsDir @ "/jungle/eatingMonkey.cs");
	exec(%behaviorsDir @ "/jungle/flameable.cs");
	exec(%behaviorsDir @ "/jungle/flintSpawner.cs");
	exec(%behaviorsDir @ "/jungle/flintstone.cs");
	exec(%behaviorsDir @ "/jungle/flyingSparks.cs");
	exec(%behaviorsDir @ "/jungle/pyrithe.cs");
	exec(%behaviorsDir @ "/jungle/rain.cs");
	exec(%behaviorsDir @ "/jungle/spawnBananas.cs");
	exec(%behaviorsDir @ "/jungle/throwingMonkey.cs");
	exec(%behaviorsDir @ "/prototype/arenaMonster.cs");
	exec(%behaviorsDir @ "/prototype/breakIntoPieces.cs");
	exec(%behaviorsDir @ "/prototype/followPlayer.cs");
	exec(%behaviorsDir @ "/prototype/monsterBreakable.cs");
	exec(%behaviorsDir @ "/prototype/paceBackAndForth.cs");
	exec(%behaviorsDir @ "/prototype/spawnStones.cs");
	exec(%behaviorsDir @ "/prototype/swing.cs");
	exec(%behaviorsDir @ "/prototype/trampolineBranch.cs");
	exec(%behaviorsDir @ "/trip/PulsatorController.cs");
	exec(%behaviorsDir @ "/trip/appearOnBeat.cs");
	exec(%behaviorsDir @ "/trip/counterRotate.cs");
	exec(%behaviorsDir @ "/trip/fadeOnRotation.cs");
	exec(%behaviorsDir @ "/trip/fadeOnSwitch.cs");
	exec(%behaviorsDir @ "/trip/pulsatingObject.cs");
	exec(%behaviorsDir @ "/trip/pulsator.cs");
	exec(%behaviorsDir @ "/trip/randomBlending.cs");
	exec(%behaviorsDir @ "/trip/reactOnSwitchesActive.cs");
	exec(%behaviorsDir @ "/trip/rhythmSwitch.cs");
	exec(%behaviorsDir @ "/trip/rhythmSwitchTrigger.cs");
	exec(%behaviorsDir @ "/trip/selfDiscoveryCoverage.cs");
	exec(%behaviorsDir @ "/trip/selfDiscoverySwitch.cs");
	exec(%behaviorsDir @ "/trip/snake.cs");
	exec(%behaviorsDir @ "/trip/textureFade.cs");
	exec(%behaviorsDir @ "/trip/textureToggle.cs");
	exec(%behaviorsDir @ "/trip/thunderController.cs");
	return;
}
