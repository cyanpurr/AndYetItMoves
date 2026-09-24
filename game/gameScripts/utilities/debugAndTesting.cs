// debugAndTesting.cs.dso
$stFuncs["0"] = "moveLeft";
$stProps["0"] = 0.20000000298023224;
$stDelays["0"] = 2000;
$stFuncs["1"] = "moveRight";
$stProps["1"] = 0.20000000298023224;
$stDelays["1"] = 2000;
$stFuncs["2"] = "jump";
$stProps["2"] = 0.10000000149011612;
$stDelays["2"] = 1000;
$stFuncs["3"] = "rotateLeft";
$stProps["3"] = 0.20000000298023224;
$stDelays["3"] = 1000;
$stFuncs["4"] = "rotateRight";
$stProps["4"] = 0.20000000298023224;
$stDelays["4"] = 1000;
$stFuncs["5"] = "resetLevel";
$stProps["5"] = 0.009999999776482582;
$stDelays["5"] = 10000;
$stFuncs["6"] = "startGame";
$stProps["6"] = 0.019999999552965164;
$stDelays["6"] = 10000;
$stFuncs["7"] = "jumpSpawnPoint";
$stProps["7"] = 0.07000000029802322;
$stDelays["7"] = 3000;
$ST_ACTION_CNT = 8;
$stStartLevel = 2;
$stCurPrepareLevel = $stStartLevel;
$stEndLevel = 22;
$stressTestEnabled = 0;
function prepareStressTest()
{
	if (!($enableDebugMap))
	{
		echo("tried to start stresstest without debug build. aborting!" SPC %this);
		return;
	}
	startGame($stCurPrepareLevel);
	if ($stCurPrepareLevel == $stEndLevel)
	{
		schedule("10000", "0", "startStressTest");
	}
	else
	{
		$stCurPrepareLevel = $stCurPrepareLevel + 1.0;
		schedule("10000", "0", "prepareStressTest");
	}
	return;
}
function startStressTest()
{
	$stressTestEnabled = 1;
	unloadCurrentLevel();
	takeSnapshot();
	dumpAccumulatedSnapshot();
	startGame(getRandom($stStartLevel, $stEndLevel));
	Canvas.toggleShowMemoryStats();
	nextStressTest();
	return;
}
function nextStressTest()
{
	if (isDead)
	{
		$stSchedule = schedule("3000", "0", "nextStressTest");
		return;
	}
	%randomValue = getRandom();
	debugEcho("stressTest random value:" SPC %randomValue);
	%lowerProbability = 0;
	%i = 0;
	while (%i < $ST_ACTION_CNT)
	{
		%upperProbability = $stProps[%i] + %lowerProbability;
		if (equalsMinMax(%randomValue, %lowerProbability, %upperProbability))
		{
			%nextAction = %i;
			break;
		}
		%lowerProbability = %upperProbability;
		%i = %i + 1.0;
	}
	%delay = $stDelays[%nextAction];
	if ($stFuncs[%nextAction] $= "moveLeft")
	{
		if (moveLeft)
		{
			%delay = 100;
		}
		moveLeft(!(moveLeft));
	}
	else
	{
		if ($stFuncs[%nextAction] $= "moveRight")
		{
			if (moveRight)
			{
				%delay = 100;
			}
			moveRight(!(moveRight));
			break;
		}
		if ($stFuncs[%nextAction] $= "jump")
		{
			if (jump)
			{
				%delay = 100;
			}
			jump(!(jump));
			break;
		}
		if ($stFuncs[%nextAction] $= "rotateLeft")
		{
			rotateKeyDown(-1.0);
			break;
		}
		if ($stFuncs[%nextAction] $= "rotateRight")
		{
			rotateKeyDown("1");
			break;
		}
		if ($stFuncs[%nextAction] $= "resetLevel")
		{
			resetLevel();
			break;
		}
		if ($stFuncs[%nextAction] $= "startGame")
		{
			unloadCurrentLevel();
			dumpAccumulatedSnapshotDifferences();
			OSReportFlush();
			startGame(getRandom($stStartLevel, $stEndLevel));
			break;
		}
		if ($stFuncs[%nextAction] $= "jumpSpawnPoint")
		{
			setCustomSpawnPoint(getRandom(spawnPointGroup.getCount() - 1.0));
		}
	}
	$stSchedule = schedule(%delay, "0", "nextStressTest");
	return;
}
function stopStressTest()
{
	cancel($stSchedule);
	return;
}
function unloadCurrentLevel()
{
	GameLevel::endLevel();
	%path = $resourceFolderName @ "/";
	GameLevel::updateResources("0", %path, environmentResource, "0");
	GameLevel::updateResources("0", %path, levelResource, "0");
	globals.currentLevelObject = "0";
	return;
}
function checkCommonMemoryUsage()
{
	return;
}
function checkDelayedRestart()
{
	startGame("2");
	$restartCounter = 1;
	$useDbgRestart = 1;
	return;
}
$dbgMaxRestarts = 1000;
$dbgRestartMod = 0;
function dbgRestart()
{
	if (!($useDbgRestart) || tickCounter < 400.0)
	{
		return $tickStats;
	}
	if ($restartCounter < $dbgMaxRestarts)
	{
		startGame("7");
		$restartCounter = $restartCounter + 1.0;
	}
	else
	{
		$useDbgRestart = 0;
		startGame("2");
	}
	return;
}
function checkResetLevel()
{
	%level1 = 2;
	%level2 = 5;
	$flagAllocs = 1;
	startGame(%level1);
	$flagAllocs = 0;
	%i = 0;
	while (%i < 20.0)
	{
		startGame(%level2);
		%i = %i + 1.0;
	}
	$dumpAllocs = 1;
	startGame(%level1);
	$dumpAllocs = 0;
	return;
}
function checkMemory()
{
	%i = 3;
	while (%i < 6.0)
	{
		startGame(%i);
		cls();
		%i = %i + 1.0;
	}
	$flagAllocs = 1;
	startGame("6");
	$flagAllocs = 0;
	%i = 3;
	while (%i < 6.0)
	{
		startGame(%i);
		cls();
		%i = %i + 1.0;
	}
	%i = 3;
	while (%i < 6.0)
	{
		startGame(%i);
		cls();
		%i = %i + 1.0;
	}
	$dumpAllocs = 1;
	startGame("6");
	$dumpAllocs = 0;
	return;
}
function startAllLevels()
{
	%i = 2;
	while (%i < 18.0)
	{
		startGame(%i);
		%i = %i + 1.0;
	}
	startGame("2");
	return;
}
function execAllLevels()
{
	%i = 0;
	while (%i < getWordCount($COMPLETE_LEVELLIST))
	{
		%levelName = getWord($COMPLETE_LEVELLIST, %i);
		exec(fileName);
		%i = %i + 1.0;
	}
	return getWordCount($COMPLETE_LEVELLIST);
}
function printPulsatorInfo()
{
	%pulsators = getObjectsWithBehavior(BePulsator);
	%i = 0;
	while (%i < getWordCount(%pulsators))
	{
		%pu = getWord(%pulsators, %i);
		%pu.echoInfo();
		%i = %i + 1.0;
	}
	return getWordCount(%pulsators);
}
function printAllSender()
{
	debugEcho("");
	%numFound = 0;
	%i = 0;
	while (%i < daSceneGraph.getSceneObjectCount())
	{
		%obj = daSceneGraph.getSceneObject(%i);
		if (%obj.getCollisionActiveSend())
		{
			debugEcho(%obj SPC %obj.getName() SPC %obj.getRealBehaviorList());
			%numFound = %numFound + 1.0;
		}
		%i = %i + 1.0;
	}
	debugEcho("there are" SPC %numFound SPC "sending objects");
	return;
}
function hideObjectsWithBehavior(%behaviorName, %hide)
{
	%allObjects = daSceneGraph.getSceneObjectList();
	%i = 0;
	while (%i < getWordCount(%allObjects))
	{
		%obj = getWord(%allObjects, %i);
		if (%obj.getBehavior(%behaviorName))
		{
			if (%hide && %obj.getVisible())
			{
				%obj.wasVisible = "1";
				%obj.setVisible("0");
			}
			else
			{
				if (wasVisible)
				{
					%obj.setVisible("1");
				}
			}
			%foundObjects = %foundObjects SPC %obj;
		}
		%i = %i + 1.0;
	}
	return ltrim(%foundObjects);
	return ltrim(%foundObjects);
}
function toggleAllSender(%turnoff)
{
	%i = 0;
	while (%i < daSceneGraph.getSceneObjectCount())
	{
		%obj = daSceneGraph.getSceneObject(%i);
		if (isPlayer(%obj))
		{
		}
		else
		{
			if (%turnoff)
			{
				if (%obj.getCollisionActiveSend())
				{
					%obj.debugWasSending = "1";
					%obj.setCollisionActiveSend("0");
				}
				break;
			}
			if (debugWasSending)
			{
				%obj.setCollisionActiveSend("1");
			}
		}
		%i = %i + 1.0;
	}
	return daSceneGraph.getSceneObjectCount();
}
function dbgToggleTextures(%on)
{
	%i = 0;
	while (%i < daSceneGraph.getSceneObjectCount())
	{
		%obj = daSceneGraph.getSceneObject(%i);
		if (!(%obj.getBehavior(BeMask)))
		{
		}
		else
		{
			%obj.setRenderTexture(%on);
		}
		%i = %i + 1.0;
	}
	return daSceneGraph.getSceneObjectCount();
}
function dbgResizeObjects()
{
	%i = 0;
	while (%i < daSceneGraph.getSceneObjectCount())
	{
		%obj = daSceneGraph.getSceneObject(%i);
		%obj.setSize("10", "10");
		%i = %i + 1.0;
	}
	return daSceneGraph.getSceneObjectCount();
}
function dbgStopAllRotators()
{
	%i = 0;
	while (%i < daSceneGraph.getSceneObjectCount())
	{
		%obj = daSceneGraph.getSceneObject(%i);
		if (%obj.getBehavior(BeRotate))
		{
			%obj.getBehavior(BeRotate).switchOff();
		}
		%i = %i + 1.0;
	}
	return daSceneGraph.getSceneObjectCount();
}
function dbgStartAllRotators()
{
	%i = 0;
	while (%i < daSceneGraph.getSceneObjectCount())
	{
		%obj = daSceneGraph.getSceneObject(%i);
		if (%obj.getBehavior(BeRotate))
		{
			%obj.getBehavior(BeRotate).switchOn();
		}
		%i = %i + 1.0;
	}
	return daSceneGraph.getSceneObjectCount();
}
function dbgSwitchOnAllSwitches()
{
	%i = 0;
	while (%i < daSceneGraph.getSceneObjectCount())
	{
		%obj = daSceneGraph.getSceneObject(%i);
		if (%obj.getBehavior(BeSwitch) && !(%obj.getBehavior(BeSwing)))
		{
			%obj.getBehavior(BeSwitch).switchOn();
		}
		%i = %i + 1.0;
	}
	return daSceneGraph.getSceneObjectCount();
}
function dbgDisableAllSwitches()
{
	%i = 0;
	while (%i < daSceneGraph.getSceneObjectCount())
	{
		%obj = daSceneGraph.getSceneObject(%i);
		if (%obj.getBehavior(BeSwitch) && !(%obj.getBehavior(BeSwing)))
		{
			%obj.setEnabled("0");
		}
		%i = %i + 1.0;
	}
	return daSceneGraph.getSceneObjectCount();
}
function dbgSetAllImmovable()
{
	%i = 0;
	while (%i < daSceneGraph.getSceneObjectCount())
	{
		%obj = daSceneGraph.getSceneObject(%i);
		if (!(isPlayer(%obj)))
		{
			%obj.setImmovable("1");
		}
		%i = %i + 1.0;
	}
	return daSceneGraph.getSceneObjectCount();
}
function dbgDismountAll()
{
	%i = 0;
	while (%i < daSceneGraph.getSceneObjectCount())
	{
		%obj = daSceneGraph.getSceneObject(%i);
		%obj.dismount();
		%i = %i + 1.0;
	}
	return daSceneGraph.getSceneObjectCount();
}
function dbgRemoveSprites()
{
	%i = 0;
	while (%i < daSceneGraph.getSceneObjectCount())
	{
		%obj = daSceneGraph.getSceneObject(%i);
		if (%obj.getClassName() $= "t2dStaticSprite" && %obj.getBehavior("BeMask") || %obj.getClassName() $= "t2dShapeVector")
		{
			%obj.safeDelete();
		}
		%i = %i + 1.0;
	}
	return daSceneGraph.getSceneObjectCount();
}
function dbgAddDummyObjects(%num, %enabled)
{
	%i = 0;
	while (%i < %num)
	{
		%bla = new t2dSceneObject(Name : "")
		{
			scenegraph = daSceneGraph;
			size = "1 1";
			Position = "0 0";
		}
		%bla.setCollisionActive("0", "0");
		%bla.setImmovable("0");
		%bla.setEnabled(%enabled);
		%i = %i + 1.0;
	}
	return;
}
function dbgToggleVisible(%visible, %condition)
{
	%i = 0;
	while (%i < daSceneGraph.getSceneObjectCount())
	{
		%obj = daSceneGraph.getSceneObject(%i);
		%evalString = "%doIt = " @ %condition;
		if (getSubStr(%evalString, strlen(%evalString) - 1.0, "1") != ";")
		{
			%evalString = %evalString @ ";";
		}
		eval(%evalString);
		if (%doIt)
		{
			if (%visible)
			{
				if (wasVisible)
				{
					%obj.setVisible("1");
				}
				break;
			}
			%obj.wasVisible = %obj.getVisible();
			%obj.setVisible("0");
		}
		%i = %i + 1.0;
	}
	return daSceneGraph.getSceneObjectCount();
}
function dbgToggleEnabled(%enabled, %condition)
{
	%i = 0;
	while (%i < daSceneGraph.getSceneObjectCount())
	{
		%obj = daSceneGraph.getSceneObject(%i);
		%evalString = "%doIt = " @ %condition;
		if (getSubStr(%evalString, strlen(%evalString) - 1.0, "1") != ";")
		{
			%evalString = %evalString @ ";";
		}
		eval(%evalString);
		if (%doIt)
		{
			if (%enabled)
			{
				if (wasEnabled)
				{
					%obj.Enabled = "1";
				}
				break;
			}
			%obj.wasEnabled = Enabled;
			%obj.Enabled = "0";
		}
		%i = %i + 1.0;
	}
	return daSceneGraph.getSceneObjectCount();
}
function dbgDisableFlameOrientation(%mode)
{
	if (%mode $= "")
	{
		%mode = "NO";
	}
	%i = 0;
	while (%i < daSceneGraph.getSceneObjectCount())
	{
		%obj = daSceneGraph.getSceneObject(%i);
		if (class $= "Flame")
		{
			%obj.getBehavior(BeKeepOrientation).setOrientationMode(%mode);
		}
		%i = %i + 1.0;
	}
	return daSceneGraph.getSceneObjectCount();
}
function dbgCreateOnUpdateTickSubscriber(%num)
{
	%i = 0;
	while (%i < %num)
	{
		%newObj = new t2dSceneObject(Name : "")
		{
			scenegraph = scenegraph;
			class = "DummySubscriber";
		}
		subscribeToEvent(%newObj, "onUpdateTick30");
		%i = %i + 1.0;
	}
	return;
}
function DummySubscriber::onUpdateTick30(%this)
{
	%this.bli();
	return;
}
function DummySubscriber::bli(%this)
{
	%this.bla();
	return;
}
function DummySubscriber::bla(%this)
{
	%this.blo();
	return;
}
function DummySubscriber::blo(%this)
{
	%this.blu();
	return;
}
function DummySubscriber::blu(%this)
{
	%blo = 1;
	return;
}
function debugEcho(%string)
{
	if ($enableDebugMap)
	{
		echo(%string);
	}
	return;
}
function debugWarn(%string)
{
	if ($enableDebugMap)
	{
		warn(%string);
	}
	return;
}
function wiiGuiTest()
{
	MBCustomText.setText("Der Speicher der Wii-Konsole ist belegt. Bitte rufe den Datenverwaltungsbildschirm auf, um Daten auf eine SD Card zu verschieben oder zu lÃ¶schen (erforderliche BlÃ¶cke: <X>).");
	MBCustomTextA.setText("Ohne Speichern fortfahren");
	MBCustomTextB.setText("ZurÃ¼ck zum Wii MenÃ¼");
	Canvas.pushDialog(MessageBoxCustomDlg);
	return;
}
function zipTest()
{
	%cave1Stats = new FileObject(Name : "");
	%cave1Stats.openForWrite("/tmp/stats/cave1.aym");
	%cave1Stats.writeLine("1: hellooooooo world1");
	%cave1Stats.writeLine("1: hellooooooo world2");
	%cave1Stats.close();
	%cave1Stats.delete();
	%cave2Stats = new FileObject(Name : "");
	%cave2Stats.openForWrite("/tmp/stats/cave2.aym");
	%cave2Stats.writeLine("2: hellooooooo world1");
	%cave2Stats.writeLine("2: hellooooooo world2");
	%cave2Stats.close();
	%cave2Stats.delete();
	%zip = new ZipObject(Name : "");
	%fileName = "test.zip";
	%result = %zip.openArchive(%fileName, "write");
	if (%result)
	{
		%zip.addFile("/tmp/stats/cave1.aym", "cave1.aym");
		echo("zipcount after adding cave1" SPC %zip.getFileEntryCount());
		%i = 0;
		while (%i < %zip.getFileEntryCount())
		{
			echo("zip:" SPC %i SPC %zip.getFileEntry(%i));
			%i = %i + 1.0;
		}
		%zip.closeArchive();
		%zip.delete();
		%zip = new ZipObject(Name : "");
		%zip.openArchive(%fileName, "ReadWrite");
		%zip.addFile("/tmp/stats/cave2.aym", "cave2.aym");
		echo("zipcount after adding cave2" SPC %zip.getFileEntryCount());
		%i = 0;
		while (%i < %zip.getFileEntryCount())
		{
			echo("zip:" SPC %i SPC %zip.getFileEntry(%i));
			%i = %i + 1.0;
		}
		%zip.closeArchive();
		%cave1Stats = new FileObject(Name : "");
		%cave1Stats.openForWrite("/tmp/stats/cave1.aym");
		%cave1Stats.writeLine("1(2): goodbye world!");
		%cave1Stats.writeLine("1(2): goodbye world2!");
		%cave1Stats.close();
		%cave1Stats.delete();
		%zip.openArchive(%fileName, "ReadWrite");
		%zip.addFile("/tmp/stats/cave1.aym", "cave1.aym");
		echo("zipcount after replacing cave1" SPC %zip.getFileEntryCount());
		%i = 0;
		while (%i < %zip.getFileEntryCount())
		{
			echo("zip:" SPC %i SPC %zip.getFileEntry(%i));
			%i = %i + 1.0;
		}
		%zip.closeArchive();
		%succ = %zip.openArchive(%fileName, "read");
		if (%succ)
		{
			%numFiles = %zip.getFileEntryCount();
			echo("statsCount:" SPC %numFiles);
			%i = 0;
			while (%i < %numFiles)
			{
				%fileNameIntern = getField(%zip.getFileEntry(%i), "0");
				%zip.extractFile(%fileNameIntern, "/tmp/ext" @ %i @ ".txt");
				echo("    extracted file:" SPC %fileNameIntern);
				%newFile = new FileObject(Name : "");
				%newFile.openForRead("/tmp/ext" @ %i @ ".txt");
				while (!(%newFile.isEOF()))
				{
					echo("readline:" TAB %newFile.readLine());
				}
				%newFile.close();
				%newFile.delete();
				%i = %i + 1.0;
			}
			%zip.closeArchive();
		}
		else
		{
			echo("stats not yet existent!");
		}
	}
	else
	{
		echo("couldn't create file" SPC %fileName);
	}
	%zip.delete();
	return;
}
function zipTest2(%this)
{
	return;
	%zip = new ZipObject(Name : "");
	if (%zip.openArchive("settings/blabla.gst", write))
	{
		debugEcho("adding bla.gst:" SPC %zip.addFile("settings/bla.gst", "bla.gst"));
		debugEcho("zipcount" SPC %zip.getFileEntryCount());
		%i = 0;
		while (%i < %zip.getFileEntryCount())
		{
			debugEcho("zip:" SPC %i SPC %zip.getFileEntry(%i));
			%i = %i + 1.0;
		}
		%zip.closeArchive();
	}
	else
	{
		debugEcho("couldn't create file" SPC "settings/blabla.gst");
	}
	debugEcho("created 2nd zip:" SPC isFile("settings/blabla.gst"));
	return;
}
function addJ()
{
	$j = $j + 1.0;
	return;
}
function addThree(%a, %b, %c)
{
	return %a + %b + %c;
	return %a + %b + %c;
}
function swapWords(%words, %first, %second)
{
	%numWords = getWordCount(%words);
	if (0.0 == %numWords || %first < 0.0 || %first >= %numWords || %second < 0.0 || %second >= %numWords || !(%first == %second))
	{
		%tmp = getWord(%words, %first);
		%words = setWord(%words, %first, getWord(%words, %second));
		%words = setWord(%words, %second, %tmp);
	}
	return %words;
	return %words;
}
function tsSpeedTest()
{
	%before = getRealTime();
	%i = 0;
	while (%i < 1000000.0)
	{
		$j = $j + 1.0;
		%i = %i + 1.0;
	}
	%after = getRealTime();
	echo("Test #1: " @ %after - %before @ " ms");
	%before = getRealTime();
	%i = 0;
	while (%i < 1000000.0)
	{
		getWord("abcd efgh", "1");
		%i = %i + 1.0;
	}
	%after = getRealTime();
	echo("Test #2: " @ %after - %before @ " ms");
	%before = getRealTime();
	%i = 0;
	while (%i < 1000000.0)
	{
		addJ();
		%i = %i + 1.0;
	}
	%after = getRealTime();
	echo("Test #3: " @ %after - %before @ " ms");
	%before = getRealTime();
	%i = 0;
	while (%i < 1000000.0)
	{
		%a = 1;
		%b = 2;
		%c = 3;
		%d = %a + %b + %c;
		%i = %i + 1.0;
	}
	%after = getRealTime();
	echo("Test #4: " @ %after - %before @ " ms");
	%before = getRealTime();
	%i = 0;
	while (%i < 1000000.0)
	{
		addThree("1", "2", "3");
		%i = %i + 1.0;
	}
	%after = getRealTime();
	echo("Test #5: " @ %after - %before @ " ms");
	%before = getRealTime();
	%i = 0;
	while (%i < 1000000.0)
	{
		swapWords("this is a test", "0", "3");
		%i = %i + 1.0;
	}
	%after = getRealTime();
	echo("Test #6: " @ %after - %before @ " ms");
	%before = getRealTime();
	%i = 0;
	while (%i < 1000000.0)
	{
		swapWords("this is a test that is longer", "0", "3");
		%i = %i + 1.0;
	}
	%after = getRealTime();
	echo("Test #7: " @ %after - %before @ " ms");
	return;
}
function resetInstructions()
{
	$settings::Wii::ShowedInstructions = "0 0 0 0";
	return;
}
function testDirectRendering()
{
	Canvas.pushDialog(menu_oneLineWarning);
	lbl_oneLineWarning_text_1.text = "loading";
	%i = 0;
	while (%i < 999999999.0)
	{
		if (%i % 20 == 0.0)
		{
			lbl_oneLineWarning_text_2.text = getWaitingString(%i, "1");
			menu_oneLineWarning.render();
		}
		%bla = %i / 12345.0;
		%i = %i + 1.0;
	}
	return;
}
function enterRallyMode()
{
	playmodeManager.setMode("Rally");
	$ACTUAL_MENU.enterMenu(menu_level);
	return;
}
function enterSurvivalMode()
{
	playmodeManager.setMode("Survival");
	$ACTUAL_MENU.enterMenu(menu_level);
	return;
}
function showLayers(%layerEnum)
{
	if (%layerEnum $= "")
	{
		%layerEnum = "main main_foreground main_background";
	}
	%i = 0;
	while (%i < scenegraph.getCount())
	{
		%object = scenegraph.getObject(%i);
		%mask = %object.getBehavior("BeMask");
		%partOfPlayer = %object.getBehavior("BePartOfPlayer");
		if (isObject(%mask) && !(isObject(%partOfPlayer)))
		{
			if (findWord(%layerEnum, Layer))
			{
				%object.setVisible("1");
				%object.setBlendColor("255 255 255");
			}
			else
			{
				%object.setVisible("0");
			}
		}
		else
		{
			%object.setVisible("0");
		}
		if (isObject(%object.getBehavior("BeZoomFader")) || isObject(%object.getBehavior("BeZoomScaling")))
		{
			%object.safeDelete();
		}
		%i = %i + 1.0;
	}
	return scenegraph.getCount();
}
function showMasksWithTextures(%texEnum, %layerEnum)
{
	if (%texEnum $= "")
	{
		return;
	}
	if (%layerEnum $= "")
	{
		%layerEnum = "main main_foreground main_background";
	}
	%i = 0;
	while (%i < scenegraph.getCount())
	{
		%object = scenegraph.getObject(%i);
		%mask = %object.getBehavior("BeMask");
		if (isObject(%mask) && isObject(textureObject))
		{
			%tex = textureObject.getImageMap();
			if (findWord(%texEnum, %tex) && findWord(%layerEnum, Layer))
			{
				%object.setVisible("1");
				%object.setBlendColor("255 255 255");
			}
			else
			{
				%object.setVisible("0");
			}
		}
		else
		{
			%object.setVisible("0");
		}
		if (isObject(%object.getBehavior("BeZoomFader")) || isObject(%object.getBehavior("BeZoomScaling")) || isObject(%object.getBehavior("BeCameraSnapping")))
		{
			%object.safeDelete();
		}
		%i = %i + 1.0;
	}
	return scenegraph.getCount();
}
function showWarningBorders()
{
	Canvas.pushDialog("hud_gui");
	warning_border.setVisible("1");
	wb_top.setVisible("1");
	wb_bottom.setVisible("1");
	wb_left.setVisible("1");
	wb_right.setVisible("1");
	debugEcho("top is" SPC wb_top.getPosition() SPC wb_top.getExtent());
	debugEcho("bottom is" SPC wb_bottom.getPosition() SPC wb_bottom.getExtent());
	debugEcho("left is" SPC wb_left.getPosition() SPC wb_left.getExtent());
	debugEcho("right is" SPC wb_right.getPosition() SPC wb_right.getExtent());
	return;
}
function showSafeFrame(%show)
{
	if (%show)
	{
		Canvas.pushDialog("safe_frame");
	}
	else
	{
		Canvas.popDialog("safe_frame");
	}
	return;
}
