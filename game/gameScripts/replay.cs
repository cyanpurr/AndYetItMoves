// replay.cs.dso
$eventID["animation"] = 1;
$eventID["flip"] = 2;
$eventID["death"] = 3;
$eventID["reanimate"] = 4;
$eventID["layer"] = 5;
$eventID["endBurning"] = 6;
$eventID["finish"] = 10;
$watchAsReplay = 0;
function Replay::init()
{
	if (active && isObject($ghostToLoad))
	{
		debugEcho("we are initing the ghost" SPC $ghostToLoad);
		subscribeToEvents($ghostToLoad, "onLevelLoadFinished");
	}
	if ($watchAsReplay)
	{
		return;
	}
	new t2dSceneObject(Name : ghostRecorder)
	{
		scenegraph = scenegraph;
		Visible = "0";
	}
	subscribeToEvents(ghostRecorder, "onLevelShutdown");
	%this = ghostRecorder;
	%this.environmentPath = "";
	if (wholeEnvironment)
	{
		%name = currentLevelObject.getName();
		if (strpos(%name, "cave") > -1.0)
		{
			%level = "cave";
		}
		if (strpos(%name, "jungle") > -1.0)
		{
			%level = "jungle";
		}
		if (strpos(%name, "trip") > -1.0)
		{
			%level = "trip";
		}
		%this.environmentPath = %level;
	}
	%this.level = getLastToken(currentLevelObject.getName(), "_");
	ghostRecorder.ghostFilePosPath = getGhostFilePath(level, "current", "pos");
	ghostRecorder.ghostFilePos = new FileObject(Name : "");
	ghostRecorder.ghostFileRotPath = getGhostFilePath(level, "current", "rot");
	ghostRecorder.ghostFileRot = new FileObject(Name : "");
	ghostRecorder.ghostFileEvtPath = getGhostFilePath(level, "current", "evt");
	ghostRecorder.ghostFileEvt = new FileObject(Name : "");
	subscribeToEvents(ghostRecorder, "onLevelLoadFinished");
	return;
}
function ghostRecorder::onLevelShutdown(%this)
{
	%this.stopRecording();
	%this.delete();
	return;
}
function Replay::load(%level, %id)
{
	if (!(isObject(replayObject)))
	{
		%replay = new ScriptObject(Name : replayObject)
		{
			class = "GhostRun";
		}
	}
	else
	{
		%replay = replayObject;
	}
	%replay.filePath = [<torque.FuncCall object at 0x0000021716CD4830>, '%id'];
	%replay.level = %level;
	%replay.makeCurrentGhostFiles();
	return %replay;
	return %replay;
}
function ghostRecorder::onLevelLoadFinished(%this)
{
	if ($WII)
	{
		return %replay;
	}
	ghostFilePos.openForWrite(ghostFilePosPath);
	ghostFileRot.openForWrite(ghostFileRotPath);
	subscribeToEvents(%this, "onRotationStart onRotationAbort");
	ghostFileEvt.openForWrite(ghostFileEvtPath);
	subscribeToEvents(%this, "onFirstKeyPressed");
	return;
}
function ghostRecorder::onFirstKeyPressed(%this)
{
	subscribeToEvents(%this, "onPlayerStateChange onMoveInputChange onPlayerSwitchedLayer");
	subscribeToEvents(%this, "onPlayerDeath onDieFragged onDieOutside onDieAlterEgoOutside onDieExploding onDiePoisoned onDieBurning onEndDieBurning onPlayerReanimate");
	%this.setTimerOn(ghostRunPositionIntervall);
	return;
}
function ghostRecorder::onTimer(%this)
{
	%time = %this.getTime();
	%pos = player.getPosition();
	ghostFilePos.writeLine(%time TAB %pos);
	return;
}
function ghostRecorder::onRotationStart(%this)
{
	%time = %this.getTime();
	%this.lastRotationParameters = actualRotationParameters;
	ghostFileRot.writeLine(%time TAB lastRotationParameters);
	return;
}
function ghostRecorder::onRotationAbort(%this)
{
	%time = %this.getTime();
	%abortionParameters = abortionParameters;
	%reverse = getField(%abortionParameters, "0");
	%inBetween = getField(%abortionParameters, "1");
	if (%reverse)
	{
		%startIndex = 1;
		%targetIndex = 0;
	}
	else
	{
		%startIndex = 0;
		%targetIndex = 1;
	}
	%startAngle = getField(lastRotationParameters, %startIndex);
	%targetAngle = getField(lastRotationParameters, %targetIndex);
	ghostFileRot.writeLine(%time TAB %startAngle TAB %targetAngle TAB %inBetween);
	return;
}
function ghostRecorder::onPlayerStateChange(%this)
{
	%time = %this.getTime();
	%animState = player.getState();
	%eventID = $eventID["animation"];
	ghostFileEvt.writeLine(%time TAB %eventID TAB %animState);
	return;
}
function ghostRecorder::onMoveInputChange(%this)
{
	%time = %this.getTime();
	%direction = player.getFlipX();
	%eventID = $eventID["flip"];
	ghostFileEvt.writeLine(%time TAB %eventID TAB %direction);
	return;
}
function ghostRecorder::onPlayerDeath(%this)
{
	return;
}
function ghostRecorder::onDieFragged(%this)
{
	if (ghostIsDead)
	{
		return %this;
	}
	%this.saveDeathEvent("fragged" TAB impactVelocityVector);
	return;
}
function ghostRecorder::onDieOutside(%this)
{
	if (ghostIsDead)
	{
		return %this;
	}
	%this.saveDeathEvent("outside" TAB shrinkAngularVelocity TAB spawnPoint.getPosition());
	return;
}
function ghostRecorder::onDieAlterEgoOutside(%this)
{
	%this.onDieOutside();
	return;
}
function ghostRecorder::onDieExploding(%this)
{
	if (ghostIsDead)
	{
		return %this;
	}
	%this.saveDeathEvent("exploding" TAB lastExplodingStrenght);
	return;
}
function ghostRecorder::onDiePoisoned(%this)
{
	if (ghostIsDead)
	{
		return %this;
	}
	%this.saveDeathEvent("poisoned");
	%this.ghostIsDead = "0";
	return;
}
function ghostRecorder::onDieBurning(%this)
{
	if (ghostIsDead)
	{
		return %this;
	}
	%this.saveDeathEvent("burning");
	return;
}
function ghostRecorder::onEndDieBurning(%this)
{
	%time = %this.getTime();
	%eventID = $eventID["endBurning"];
	ghostFileEvt.writeLine(%time TAB %eventID);
	return;
}
function ghostRecorder::saveDeathEvent(%this, %parameters)
{
	%this.ghostIsDead = "1";
	%time = %this.getTime();
	%eventID = $eventID["death"];
	ghostFileEvt.writeLine(%time TAB %eventID TAB %parameters);
	return;
}
function ghostRecorder::onPlayerReanimate(%this)
{
	%this.ghostIsDead = "0";
	%time = %this.getTime();
	%eventID = $eventID["reanimate"];
	ghostFileEvt.writeLine(%time TAB %eventID);
	return;
}
function ghostRecorder::onPlayerSwitchedLayer(%this)
{
	%time = %this.getTime();
	%layer = player.getLayer();
	%eventID = $eventID["layer"];
	ghostFileEvt.writeLine(%time TAB %eventID TAB %layer);
	return;
}
function ghostRecorder::finishLevel(%this)
{
	%time = %this.getTime();
	%eventID = $eventID["finish"];
	ghostFileEvt.writeLine(%time TAB %eventID);
	return;
}
function ghostRecorder::saveGhost(%this, %saveToLevel)
{
	if ($WII)
	{
		return;
	}
	%endings = "pos rot evt";
	if (%saveToLevel)
	{
		%subFolder = level;
	}
	else
	{
		%subFolder = "toSubmit";
	}
	%saveToPath = getGhostFilePath(%subFolder);
	%zipFile = new ZipObject(Name : "");
	assertDirectory(%saveToPath);
	nextUniqueId("ghost", %subFolder);
	%saveToPath = getGhostFilePath(%subFolder, $settings::Ghost::highestID);
	if (%zipFile.openArchive(%saveToPath, "write"))
	{
		%i = 0;
		while (%i < getWordCount(%endings))
		{
			%ending = getWord(%endings, %i);
			%zipFile.addFile(getGhostFilePath(level, "current", %ending), "ghost." @ %ending);
			%i = %i + 1.0;
		}
	}
	else
	{
		warn("WARNING: ghostRecorder::saveGhost: couldn't make zip object with path:" SPC %saveToPath);
	}
	%zipFile.closeArchive();
	%zipFile.delete();
	return;
}
function ghostRecorder::saveEnvironmentGhosts(%this, %ghostIds, %environmentPath)
{
	if ($WII)
	{
		return;
	}
	if (%environmentPath $= "")
	{
		%environmentPath = environmentPath;
	}
	if (%environmentPath != "")
	{
		%zipFile = new ZipObject(Name : "");
		%archivePath = getGhostFilePath(%environmentPath);
		assertDirectory(%archivePath);
		nextUniqueId("ghost", %environmentPath);
		%zipFilePath = getGhostFilePath(%environmentPath, $settings::Ghost::highestID);
		if (%zipFile.openArchive(%zipFilePath, "write"))
		{
			%i = 0;
			while (%i < getWordCount(%ghostIds))
			{
				%currentId = getWord(%ghostIds, %i);
				%zipFile.addFile(getGhostFilePath("toSubmit", %currentId), "ghost" @ %i @ ".gst");
				%i = %i + 1.0;
			}
		}
		else
		{
			warn("WARNING: ghostRecorder::saveEnvironmentGhosst: couldn't make zip object with path:" SPC %zipFilePath);
		}
		%zipFile.closeArchive();
		%zipFile.delete();
	}
	return;
}
function ghostRecorder::stopRecording(%this, %justEvents)
{
	if ($WII)
	{
		return;
	}
	%this.setTimerOff();
	unSubscribeFromEvents(%this, "onPlayerStateChange onMoveInputChange onPlayerSwitchedLayer");
	unSubscribeFromEvents(%this, "onPlayerDeath onDieFragged onDieOutside onDieAlterEgoOutside onDieExploding onDiePoisoned onDieBurning onPlayerReanimate");
	if (!($gameIsQuitting) && %justEvents)
	{
		%this.finishLevel();
	}
	if (%justEvents)
	{
		return;
	}
	if (isObject(ghostFilePos))
	{
		ghostFilePos.close();
		ghostFilePos.delete();
	}
	if (isObject(ghostFileRot))
	{
		ghostFileRot.close();
		ghostFileRot.delete();
	}
	if (isObject(ghostFileEvt))
	{
		ghostFileEvt.close();
		ghostFileEvt.delete();
	}
	return;
}
function ghostRecorder::getTime(%this)
{
	return mFloatLength(Statistics.getLevelTime(), "5");
	return mFloatLength(Statistics.getLevelTime(), "5");
}
function GhostRun::onLevelLoadFinished(%this)
{
	%this.ghostCharacter = new t2dAnimatedSprite(Name : "")
	{
		scenegraph = scenegraph;
		animationName = "playerIdleAnimation";
		size = playerSize;
		escapeSwitch = "1";
	}
	%alphaBlending = 0.4000000059604645;
	ghostCharacter.localFootPoint = localFootPoint;
	ghostCharacter.setBlendAlpha(%alphaBlending);
	ghostCharacter.setName("");
	ghostCharacter.setPosition(player.getPosition());
	player.clonePlayerParts(ghostCharacter, $LAYER["player"], %alphaBlending);
	%this.nextPosEventNR = "0";
	%this.actualRotEventNR = "0";
	%this.actualEventNR = "0";
	if (isObject(ghostFilePos))
	{
		ghostFilePos.close();
	}
	else
	{
		%this.ghostFilePos = new FileObject(Name : "");
	}
	ghostFilePos.openForRead(getGhostFilePath("loaded", "ghost", "pos"));
	if (isObject(ghostFileRot))
	{
		ghostFileRot.close();
	}
	else
	{
		%this.ghostFileRot = new FileObject(Name : "");
	}
	ghostFileRot.openForRead(getGhostFilePath("loaded", "ghost", "rot"));
	if (isObject(ghostFileEvt))
	{
		ghostFileEvt.close();
	}
	else
	{
		%this.ghostFileEvt = new FileObject(Name : "");
	}
	ghostFileEvt.openForRead(getGhostFilePath("loaded", "ghost", "evt"));
	if ($watchAsReplay)
	{
		sceneWindow2d.mount(ghostCharacter, "0 0", cameraMountForce, "0");
		camera.mount(ghostCharacter, "0 0", cameraMountForce, "0", "1", "0", "0");
		viewWindow.mount(ghostCharacter, "0 0", cameraMountForce, "0", "1");
	}
	%nextPosEvent = ghostFilePos.peekLine();
	%this.nextPosEventNR = "0";
	%this.nextTime = %this.extractTime(%nextPosEvent);
	%this.nextPosition = %this.extractPosition(%nextPosEvent);
	ghostCharacter.setPosition(nextPosition);
	%this.worldRotation = Smoother::createInstance();
	%maskBehavior = ghostCharacter.addDependentBehavior("BeMask");
	%maskBehavior.layer = "custom";
	ghostCharacter.setLayer($LAYER["player"]);
	%shrinkBehavior = ghostCharacter.addDependentBehavior("BeShrinker");
	%shrinkBehavior.setOnShrinkFinished("none");
	%shrinkBehavior.setShrinkChildren("0");
	subscribeToEvents(%this, "onFirstKeyPressed onLevelShutdown");
	return;
}
function GhostRun::makeCurrentGhostFiles(%this)
{
	%endings = "pos rot evt";
	assertDirectory(getGhostFilePath("loaded"));
	%zipFile = new ZipObject(Name : "");
	if (%zipFile.openArchive(filePath @ ".gst", "read"))
	{
		%i = 0;
		while (%i < getWordCount(%endings))
		{
			%ending = getWord(%endings, %i);
			%zipFile.extractFile("ghost." @ %ending, getGhostFilePath("loaded", "ghost", %ending));
			%i = %i + 1.0;
		}
	}
	else
	{
		warn("WARNING: GhostRun:makeCurrentGhostFiles: couldn't open zipfile" SPC filePath SPC ".gst");
	}
	%zipFile.closeArchive();
	%zipFile.delete();
	return;
}
function GhostRun::onFirstKeyPressed(%this)
{
	debugEcho("starting level with offset:" SPC Statistics.getEnvironmentLevelOffset());
	%offset = Statistics.getEnvironmentLevelOffset();
	if (%offset > 0.0 && 0)
	{
		%this.skipToOffset(%offset);
	}
	else
	{
		%this.getNextPosition();
		%this.getNextRotation();
		%this.getNextEvent();
	}
	return;
}
function GhostRun::onLevelShutdown(%this)
{
	%this.endReplay();
	return;
}
function GhostRun::endReplay(%this)
{
	debugEcho("ending replay of" SPC %this);
	%this.actualPosTime = "0";
	%this.actualPosition = "0";
	%this.realPosEventNR = "0";
	%this.nextPosTime = "0";
	%this.nextPosition = "0";
	%this.nextPosEventNR = "0";
	%this.actualRotEventNR = "0";
	%this.actualRotTime = "0";
	%this.actualStartRotation = "0";
	%this.actualTargetRotation = "0";
	%this.actualRotationInBetween = "0";
	%this.actualEventNR = "0";
	%this.actualEventTime = "0";
	if (isObject(fraggedPlayerPartGroup))
	{
		fraggedPlayerPartGroup.clear();
		fraggedPlayerPartGroup.delete();
	}
	if (isObject(ghostFilePos))
	{
		ghostFilePos.close();
		ghostFilePos.delete();
	}
	if (isObject(ghostFileRot))
	{
		ghostFileRot.close();
		ghostFileRot.delete();
	}
	if (isObject(ghostFileEvt))
	{
		ghostFileEvt.close();
		ghostFileEvt.delete();
	}
	ghostCharacter.safeDelete();
	worldRotation.delete();
	%this.cancelOpenSchedules();
	return;
}
function GhostRun::getTime(%this)
{
	return Statistics.getLevelTime();
	return mFloatLength(Statistics.getLevelTime() + Statistics.getEnvironmentLevelOffset(), "5");
	return mFloatLength(Statistics.getLevelTime() + Statistics.getEnvironmentLevelOffset(), "5");
}
function GhostRun::cancelOpenSchedules(%this)
{
	if (isEventPending(nextPositionSchedule))
	{
		cancel(nextPositionSchedule);
	}
	if (isEventPending(lastPositionSchedule))
	{
		cancel(lastPositionSchedule);
	}
	if (isEventPending(nextRotationSchedule))
	{
		cancel(nextRotationSchedule);
	}
	if (isEventPending(nextEventSchedule))
	{
		cancel(nextEventSchedule);
	}
	return;
}
function GhostRun::skipToOffset(%this, %offsetTime, %type)
{
	%offsetTime = mFloatLength(%offsetTime, "5");
	if (%type $= "pos" || %type $= "")
	{
		while (1)
		{
			%actualPos = ghostFilePos.readLine();
			%this.nextPosEventNR = nextPosEventNR + 1.0;
			%nextEvent = ghostFilePos.peekLine();
			%nextTime = %this.extractTime(%nextEvent);
			if (%nextTime > %offsetTime || ghostFilePos.isEOF())
			{
				debugEcho("setting to last of skipped positions:" SPC %actualPos);
				ghostCharacter.setPosition(%this.extractPosition(%actualPos));
				if (ghostFilePos.isEOF())
				{
					break;
				}
				%nextPosEvent = ghostFilePos.readLine();
				%this.nextPosEventNR = nextPosEventNR + 1.0;
				%this.nextPosTime = %this.extractTime(%nextPosEvent);
				%this.nextPosition = %this.extractPosition(%nextPosEvent);
				%this.getNextPosition();
				break;
			}
		}
	}
	if (%type $= "rot" || %type $= "")
	{
		while (1)
		{
			%actualRot = ghostFileRot.readLine();
			%this.actualRotEventNR = actualRotEventNR + 1.0;
			%nextEvent = ghostFileRot.peekLine();
			%nextTime = %this.extractTime(%nextEvent);
			if (%nextTime > %offsetTime || ghostFileRot.isEOF())
			{
				debugEcho("setting skipped rotation:" SPC %actualRot);
				%eventTime = %this.extractTime(%actualRot);
				%startRotation = %this.extractStartRotation(%actualRot);
				%targetRotation = %this.extractTargetRotation(%actualRot);
				%inBetween = min(%this.extractInBetween(%actualRot) + %offsetTime - %eventTime / timeToRotate, "1");
				%this.setNextRotation(%startRotation, %targetRotation, %inBetween);
				break;
			}
		}
	}
	if (%type $= "evt" || %type $= "")
	{
		%skippedEvents = "";
		while (1)
		{
			%actualEvt = ghostFileEvt.readLine();
			%this.actualEventNR = actualEventNR + 1.0;
			%skippedEvents = setRecord(%skippedEvents, %this.extractEventID(%actualEvt), %actualEvt);
			%nextEvent = ghostFileEvt.peekLine();
			%nextTime = %this.extractTime(%nextEvent);
			if (%nextTime > %offsetTime || ghostFileEvt.isEOF())
			{
				debugEcho("setting the skipped events:" SPC %skippedEvents);
				%i = 0;
				while (%i < getRecordCount(%skippedEvents))
				{
					%event = getRecord(%skippedEvents, %i);
					if (%event $= "")
					{
						while ()
						{
							while ()
							{
							}
							%this.setNextEvent(%event);
						}
					}
					%i = %i + 1.0;
				}
				break;
			}
		}
	}
	return getRecordCount(%skippedEvents);
}
function GhostRun::getNextPosition(%this)
{
	%this.actualPosEventNR = nextPosEventNR;
	%skippedStates = 0;
	if (1)
	{
		%this.actualPosTime = nextPosTime;
		%this.actualPosition = nextPosition;
		%this.realPosEventNR = nextPosEventNR;
		%nextPosEvent = ghostFilePos.readLine();
		%this.nextPosEventNR = nextPosEventNR + 1.0;
		%this.nextPosTime = %this.extractTime(%nextPosEvent);
		%this.nextPosition = %this.extractPosition(%nextPosEvent);
		%now = %this.getTime();
		if (actualPosTime > %now || ghostFilePos.isEOF())
		{
			while ()
			{
				while ()
				{
				}
				%skippedStates = %skippedStates + 1.0;
			}
		}
	}
	if (!(ghostFilePos.isEOF()))
	{
		%timeLeft = actualPosTime - %now * 1000.0;
		%this.nextPositionSchedule = %this.schedule(%timeLeft, "setNextPosition", actualPosition);
	}
	else
	{
		%timeLeft = nextPosTime - %now * 1000.0;
		if (%timeLeft <= 0.0)
		{
			%this.setLastPosition(nextPosition);
			break;
		}
		%this.lastPositionSchedule = %this.schedule(%timeLeft, "setLastPosition", nextPosition);
	}
	if (%skippedStates > 0.0)
	{
		debugEcho("GhostRun" SPC %this SPC ": skipped" SPC %skippedStates SPC "positions (" SPC actualPosEventNR SPC "-" SPC realPosEventNR - 1.0 SPC ")");
	}
	return;
}
function GhostRun::setNextPosition(%this, )
{
	%actualPosition = ghostCharacter.getPosition();
	%actualTime = %this.getTime();
	%timeToNextEvent = nextPosTime - %actualTime;
	if (%timeToNextEvent > 0.0)
	{
		%velocity = t2dVectorScale(t2dVectorSub(nextPosition, %actualPosition), 1.0 / %timeToNextEvent);
	}
	else
	{
		%velocity = "0 0";
	}
	ghostCharacter.setLinearVelocity(%velocity);
	%this.getNextPosition();
	return;
}
function GhostRun::setLastPosition(%this, %pos)
{
	ghostCharacter.setPosition(%pos);
	ghostCharacter.setAtRest();
	return;
}
function GhostRun::getNextRotation(%this)
{
	if (ghostFileRot.isEOF())
	{
		return ghostFileRot.isEOF();
	}
	%nextEvent = ghostFileRot.readLine();
	%this.actualRotEventNR = actualRotEventNR + 1.0;
	%this.actualRotTime = %this.extractTime(%nextEvent);
	%this.actualStartRotation = %this.extractStartRotation(%nextEvent);
	%this.actualTargetRotation = %this.extractTargetRotation(%nextEvent);
	%this.actualRotationInBetween = %this.extractInBetween(%nextEvent);
	%now = %this.getTime();
	if (actualRotTime <= %now)
	{
		%interpolatedInBetween = actualRotationInBetween + %now - actualRotTime / timeToRotate;
		if (%interpolatedInBetween > 1.0)
		{
			%interpolatedInBetween = 1;
		}
		debugEcho("GhostRun" SPC %this SPC ": missed rotation event (" SPC actualRotEventNR SPC ") calculated new inBeween:" SPC actualRotationInBetween SPC "->" SPC %interpolatedInBetween);
		%this.setNextRotation(actualStartRotation, actualTargetRotation, %interpolatedInBetween);
	}
	else
	{
		%timeLeft = actualRotTime - %now * 1000.0;
		%this.nextRotationSchedule = %this.schedule(%timeLeft, "setNextRotation", actualStartRotation, actualTargetRotation, actualRotationInBetween);
	}
	return;
}
function GhostRun::setNextRotation(%this, %startRotation, %targetRotation, %inBetween)
{
	if (!(worldRotation.getIsFinished()) && worldRotation.getIsInitialized())
	{
		worldRotation.pause();
	}
	worldRotation.init(timeToRotate, %startRotation, %targetRotation, "SMOOTH", "1");
	worldRotation.start("0", %inBetween);
	subscribeToEvents(%this, "onUpdateFrame");
	%this.getNextRotation();
	return;
}
function GhostRun::onUpdateFrame(%this)
{
	%rotation = worldRotation.getValue();
	ghostCharacter.setPivotRotation(-1.0 * %rotation, localFootPoint);
	if ($watchAsReplay)
	{
		sceneWindow.setCameraRotation(%rotation);
	}
	if (worldRotation.getIsFinished())
	{
		unSubscribeFromEvents(%this, "onUpdateFrame");
	}
	return;
}
function GhostRun::getNextEvent(%this)
{
	if (ghostFileEvt.isEOF())
	{
		return ghostFileEvt.isEOF();
	}
	%nextEvent = ghostFileEvt.readLine();
	%this.actualEventNR = actualEventNR + 1.0;
	%this.actualEventTime = %this.extractTime(%nextEvent);
	%now = %this.getTime();
	if (actualEventTime <= %now)
	{
		debugEcho("GhostRun" SPC %this SPC ": missed event-time (" SPC actualEventNR SPC ") and applied it directly");
		%this.setNextEvent(%nextEvent);
	}
	else
	{
		%timeLeft = actualEventTime - %now * 1000.0;
		%this.nextEventSchedule = %this.schedule(%timeLeft, "setNextEvent", %nextEvent);
	}
	return;
}
function GhostRun::setNextEvent(%this, %eventString)
{
	%this.applyEvent(%eventString);
	%this.getNextEvent();
	return;
}
function GhostRun::applyEvent(%this, %eventString)
{
	%type = %this.extractEventID(%eventString);
	if (%type == 1.0)
	{
		%animation = %this.extractAnimation(%eventString);
		ghostCharacter.playAnimation(animations[%animation]);
	}
	else
	{
		if (%type == 2.0)
		{
			%flip = %this.extractFlip(%eventString);
			ghostCharacter.setFlipX(%flip);
			break;
		}
		if (%type == 3.0)
		{
			%deathType = %this.extractDeath(%eventString);
			%param1 = %this.extractDeathParameter(%eventString, "1");
			%param2 = %this.extractDeathParameter(%eventString, "2");
			%this.applyDeath(%deathType, %param1, %param2);
			break;
		}
		if (%type == 4.0)
		{
			if (diedOutside)
			{
				unSubscribeFromEvent(ghostCharacter.getBehavior("BeShrinker"), "onUpdateTick10");
				ghostCharacter.setSize(playerSize);
				ghostCharacter.setPosition(ghostWillRespawnAt);
				ghostCharacter.setLinearVelocity("0 0");
				ghostCharacter.setAngularVelocity("0");
				%this.diedOutside = "0";
			}
			ghostCharacter.setVisible("1");
			player::defrag(ghostCharacter);
			player::resetPlayerparts(ghostCharacter);
			break;
		}
		if (%type == 5.0)
		{
			%layer = %this.extractLayer(%eventString);
			ghostCharacter.setLayer(%layer);
			break;
		}
		if (%type == 6.0)
		{
			player::removeFire(ghostCharacter);
			break;
		}
		if (%type == 10.0)
		{
			%this.endRun();
			break;
		}
		debugEcho("GhostRun" SPC %this SPC ": delivered eventID" SPC %type SPC "unknown... ignoring");
	}
	return;
}
function GhostRun::applyDeath(%this, %type, %param1, %param2)
{
	if (%type $= "fragged")
	{
		ghostCharacter.impactVelocityVector = %param1;
		player::dieFragged(ghostCharacter);
	}
	else
	{
		if (%type $= "outside")
		{
			%this.diedOutside = "1";
			ghostCharacter.setAngularVelocity(%param1);
			%this.ghostWillRespawnAt = %param2;
			%shrinker = ghostCharacter.getBehavior("BeShrinker");
			%shrinker.startShrinking();
			ghostCharacter.schedule(duration * 850.0, "setVisible", "0");
			player::dieOutside(ghostCharacter);
			break;
		}
		if (%type $= "exploding")
		{
			player::dieExploding(ghostCharacter, %param1);
			break;
		}
		if (%type $= "poisoned")
		{
			player::dieSnakeBite(ghostCharacter);
			break;
		}
		if (%type $= "burning")
		{
			player::dieBurning(ghostCharacter);
			break;
		}
		debugEcho("GhostRun" SPC %this SPC ": death-type" SPC %type SPC "unknown... ignoring");
	}
	return;
}
function GhostRun::endRun(%this)
{
	%this.cancelOpenSchedules();
	ghostCharacter.playAnimation(playerIdleAnimation);
	ghostCharacter.setLinearVelocity("0 0");
	ghostCharacter.setRotation("0");
	ghostCharacter.setFlipX("1");
	ghostCharacter.setLayer($LAYER["mainBehindPlayer"]);
	ghostCharacter.mount(levelswitchPoint, "0 0", "7", "1", "0", "0", "0");
	if ($watchAsReplay)
	{
		SpeedRunMode.stopTime();
		levelswitchPoint.startLevelSwitch();
	}
	return;
}
function GhostRun::extractTime(%this, %event)
{
	return getField(%event, "0");
	return getField(%event, "0");
}
function GhostRun::extractPosition(%this, %event)
{
	return getField(%event, "1");
	return getField(%event, "1");
}
function GhostRun::extractStartRotation(%this, %event)
{
	return getField(%event, "1");
	return getField(%event, "1");
}
function GhostRun::extractTargetRotation(%this, %event)
{
	return getField(%event, "2");
	return getField(%event, "2");
}
function GhostRun::extractInBetween(%this, %event)
{
	return getField(%event, "3");
	return getField(%event, "3");
}
function GhostRun::extractEventID(%this, %event)
{
	return getField(%event, "1");
	return getField(%event, "1");
}
function GhostRun::extractAnimation(%this, %event)
{
	return getField(%event, "2");
	return getField(%event, "2");
}
function GhostRun::extractFlip(%this, %event)
{
	return getField(%event, "2");
	return getField(%event, "2");
}
function GhostRun::extractDeath(%this, %event)
{
	return getField(%event, "2");
	return getField(%event, "2");
}
function GhostRun::extractDeathParameter(%this, %event, %nr)
{
	return getField(%event, 2.0 + %nr);
	return getField(%event, 2.0 + %nr);
}
function GhostRun::extractLayer(%this, %event)
{
	return getField(%event, "2");
	return getField(%event, "2");
}
