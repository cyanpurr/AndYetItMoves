// achievements.cs.dso
$achievement::time["ChapterOne"] = 5;
$achievement::time["ChapterTwo"] = 20;
$achievement::time["ChapterThree"] = 25;
$achievement::sucidalTendenciesDeathCount = 1000;
$achievement::addictPlayTime = 210;
$achievement::surfDistance = 300;
$achievement::snowboardDistance = $achievement::surfDistance * 0.800000011920929;
$achievement::swingBuckets = 16;
$achievement::swingMinDistanceFactor = 0.699999988079071;
function initAchievements()
{
	%this = new ScriptObject(Name : achievements);
	gameGarbageCollector.add(achievements);
	%this.internalNames = "TimeChallengeChapterOne" TAB "TimeChallengeChapterTwo" TAB "TimeChallengeChapterThree" TAB "Gift" TAB "BonusLevel" TAB "Slaughterer" TAB "DemolitionMan" TAB "Rip" TAB "AroundTheWorld" TAB "Stuntman" TAB "Surfer" TAB "Frozen" TAB "Kamikaze" TAB "AyimAddicted" TAB "Random" TAB "SuicidalTendencies" TAB "Zookeeper" TAB "5LimitedRotationMode" TAB "5RallyMode" TAB "5SurvivalMode" TAB "Perfectionist" TAB "Crazy" TAB "Casey" TAB "Cheater" TAB "Cave" TAB "Jungle" TAB "Trip" TAB "AllBoni" TAB "BatWetter" TAB "Snowboarder";
	%this.icons = "timeCave" TAB "timeJungle" TAB "timeTrip" TAB "gift" TAB "bonusLevel" TAB "slaughterer" TAB "demolitionMan" TAB "rip" TAB "aroundTheWorld" TAB "stuntman" TAB "surfer" TAB "frozen" TAB "kamikaze" TAB "ayimAddicted" TAB "random" TAB "suicidalTendencies" TAB "zookeeper" TAB "gregor" TAB "mcColinRay" TAB "robinsonCrusoe" TAB "perfectionist" TAB "crazy" TAB "casey" TAB "pwn3d" TAB "caveDweller" TAB "tarzan" TAB "guru" TAB "allBonus" TAB "batWetter" TAB "snowboarder";
	%this.Path = ['"game/"', '$DataFolder', '"/images/Menu/Achievements/"'];
	%i = 0;
	while (%i < getFieldCount(internalNames))
	{
		%this.indices[getField(internalNames, %i)] = %i;
		%i = %i + 1.0;
	}
	return getFieldCount(internalNames);
}
function achievements::localize(%this)
{
	%this.names = $lbl_achievement_name_TimeChallengeChapterOne TAB $lbl_achievement_name_TimeChallengeChapterTwo TAB $lbl_achievement_name_TimeChallengeChapterThree TAB $lbl_achievement_name_Gift TAB $lbl_achievement_name_BonusLevel TAB $lbl_achievement_name_Slaughterer TAB $lbl_achievement_name_DemolitionMan TAB $lbl_achievement_name_Rip TAB $lbl_achievement_name_AroundTheWorld TAB $lbl_achievement_name_Stuntman TAB $lbl_achievement_name_Surfer TAB $lbl_achievement_name_Frozen TAB $lbl_achievement_name_Kamikaze TAB $lbl_achievement_name_AyimAddicted TAB $lbl_achievement_name_Random TAB $lbl_achievement_name_SuicidalTendencies TAB $lbl_achievement_name_Zookeeper TAB $lbl_achievement_name_5LimitedRotation TAB $lbl_achievement_name_5Rally TAB $lbl_achievement_name_5Survival TAB $lbl_achievement_name_Perfectionist TAB $lbl_achievement_name_Crazy TAB $lbl_achievement_name_Casey TAB $lbl_achievement_name_Cheater TAB $lbl_achievement_name_Cave TAB $lbl_achievement_name_Jungle TAB $lbl_achievement_name_Trip TAB $lbl_achievement_name_AllBoni TAB $lbl_achievement_name_BatWetter TAB $lbl_achievement_name_Snowboarder;
	%addictionHourFormatted = mFloor($achievement::addictPlayTime / 60.0) @ ":" @ modulo($achievement::addictPlayTime, "60");
	%surferMeters = 25;
	%snowboardMeters = 20;
	%this.descriptions = $lbl_achievement_dscrptn_TimeChallengeChapterOne_1 SPC $achievement::time["ChapterOne"] SPC $lbl_achievement_dscrptn_TimeChallengeChapter_2 TAB $lbl_achievement_dscrptn_TimeChallengeChapterTwo_1 SPC $achievement::time["ChapterTwo"] SPC $lbl_achievement_dscrptn_TimeChallengeChapter_2 TAB $lbl_achievement_dscrptn_TimeChallengeChapterThree_1 SPC $achievement::time["ChapterThree"] SPC $lbl_achievement_dscrptn_TimeChallengeChapter_2 TAB $lbl_achievement_dscrptn_Gift TAB $lbl_achievement_dscrptn_BonusLevel TAB $lbl_achievement_dscrptn_Slaughterer TAB $lbl_achievement_dscrptn_DemolitionMan TAB $lbl_achievement_dscrptn_Rip TAB $lbl_achievement_dscrptn_AroundTheWorld TAB $lbl_achievement_dscrptn_Stuntman TAB $lbl_achievement_dscrptn_Surfer_1 SPC %surferMeters SPC $lbl_achievement_dscrptn_Surfer_2 TAB $lbl_achievement_dscrptn_Frozen TAB $lbl_achievement_dscrptn_Kamikaze TAB $lbl_achievement_dscrptn_AyimAddicted_1 SPC %addictionHourFormatted SPC $lbl_achievement_dscrptn_AyimAddicted_2 TAB $lbl_achievement_dscrptn_Random TAB $lbl_achievement_dscrptn_SuicidalTendencies_1 SPC $achievement::sucidalTendenciesDeathCount SPC $lbl_achievement_dscrptn_SuicidalTendencies_2 TAB $lbl_achievement_dscrptn_Zookeeper TAB $lbl_achievement_dscrptn_5LimitedRotation TAB $lbl_achievement_dscrptn_5Rally TAB $lbl_achievement_dscrptn_5Survival TAB $lbl_achievement_dscrptn_Perfectionist TAB $lbl_achievement_dscrptn_Crazy TAB $lbl_achievement_dscrptn_Casey TAB $lbl_achievement_dscrptn_Cheater TAB $lbl_achievement_dscrptn_Cave TAB $lbl_achievement_dscrptn_Jungle TAB $lbl_achievement_dscrptn_Trip TAB $lbl_achievement_dscrptn_AllBoni TAB $lbl_achievement_dscrptn_BatWetter TAB $lbl_achievement_dscrptn_Snowboarder_1 SPC %snowboardMeters SPC $lbl_achievement_dscrptn_Snowboarder_2;
	if ($settings::Personal::Language $= "japanese")
	{
		%this.descriptions = setField(descriptions, "13", $lbl_achievement_dscrptn_AyimAddicted_1 SPC $lbl_achievement_dscrptn_AyimAddicted_2);
	}
	if (achievemenSorted != "")
	{
		%this.prependDemoString("1");
	}
	return;
}
function achievements::prependDemoString(%this, %force)
{
	if ($WII && $demoVersion)
	{
		%this.availability = "0 0 0 0 0 1 1 1 1 1 1 0 0 0 1 1 1 0 0 0 0 1 0 0 0 0 0 0 0 0";
	}
	else
	{
		if ($demoVersion)
		{
			%this.availability = "0 0 0 1 0 0 1 1 1 1 1 0 0 0 1 1 1 0 0 0 0 1 0 0 0 0 0 0 0 0";
			break;
		}
		%this.availability = "1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1";
	}
	if (!(checkedForAvailability) || %force)
	{
		%i = 0;
		while (%i < getWordCount(achievemenSorted))
		{
			if (!(getWord(availability, %i)))
			{
				%descIndex = indices[getWord(achievemenSorted, %i)];
				%desc = ['"("', '$lbl_requires_full_version', '")"'] SPC getField(descriptions, %descIndex);
				%this.descriptions = setField(descriptions, %descIndex, %desc);
			}
			%i = %i + 1.0;
		}
		%this.checkedForAvailability = "1";
	}
	return;
}
function achievements::setup(%this)
{
	%this.achievemenSorted = "TimeChallengeChapterOne" SPC "TimeChallengeChapterTwo" SPC "TimeChallengeChapterThree" SPC "DemolitionMan" SPC "Slaughterer" SPC "AroundTheWorld" SPC "Kamikaze" SPC "Stuntman" SPC "Snowboarder" SPC "Surfer" SPC "Frozen" SPC "Gift" SPC "BonusLevel" SPC "Rip" SPC "SuicidalTendencies" SPC "AyimAddicted" SPC "Random" SPC "BatWetter" SPC "Zookeeper" SPC "Casey" SPC "5LimitedRotationMode" SPC "5RallyMode" SPC "5SurvivalMode" SPC "Cave" SPC "Jungle" SPC "Trip" SPC "Cheater" SPC "AllBoni" SPC "Perfectionist" SPC "Crazy";
	if ($distributorName $= "Steam")
	{
		%this.achieveableIndizes = "0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24 25 26 27 28 29";
	}
	else
	{
		%this.achieveableIndizes = "0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24 25 26 27";
	}
	%this.prependDemoString();
	%this.lastPopupShownTime = "0";
	%this.demolitionManCounter = "0";
	%this.demolitionManObjects = "0";
	%this.randomTime = "0";
	%this.baseRandomChance = "0.0035";
	%this.levelDeathCount = "0";
	%this.moveKeyPressed = "0";
	if (!(%this.getAchieved("Surfer")) || !(%this.getAchieved("Snowboarder")) || !(%this.getAchieved("Casey")))
	{
		subscribeToEvents(%this, "onStartFalling onRotationStart");
		%this.accumulatedSurfDist = "0";
	}
	if (wholeEnvironment)
	{
		%levelName = getLastToken(currentLevelObject.getName(), "_");
		%envName = getSubStr(%levelName, "0", strlen(%levelName) - 1.0);
		%chapterMapping["cave"] = "One";
		%chapterMapping["jungle"] = "Two";
		%chapterMapping["trip"] = "Three";
		%this.chapterToBeat = ['"Chapter"', <torque.ArrayAccess object at 0x0000021717956ED0>];
		if (!(%this.getAchieved("TimeChallenge" @ chapterToBeat)))
		{
			subscribeToEvents(%this, "onEnvironmentCompleted");
		}
	}
	if (!(%this.getAchieved("Gift")) || !(%this.getAchieved("BonusLevel")) || !(%this.getAchieved("Frozen")) || !(%this.getAchieved("AllBoni")))
	{
		if ($WII)
		{
			subscribeToEvents(%this, "onEnterLevelSwitch");
			break;
		}
		subscribeToEvents(%this, "onLevelFadeOutEnded");
	}
	if (!(%this.getAchieved("Random")) || !(%this.getAchieved("Kamikaze")) || !(%this.getAchieved("AyimAddicted")))
	{
		subscribeToEvents(%this, "onFirstKeyPressed");
	}
	if (!(%this.getAchieved("Rip")))
	{
		subscribeToEvents(%this, "onDieFragged onDieOutside onDieBurning onPlayerReanimate");
	}
	if (!(%this.getAchieved("AroundTheWorld")))
	{
		%this.resetSwingStats();
	}
	$deathCallbackSuizidal = !(%this.getAchieved("SuicidalTendencies"));
	$deathCallbackKamikaze = 0;
	subscribeToEvents(%this, "onPlayerDeath onLevelShutdown");
	return;
}
function menu_achievements::onDialogPush(%this)
{
	if (isUpdateDiscoverable("achievementnum28"))
	{
		setUpdateDiscovered("achievementnum28");
	}
	if (isUpdateDiscoverable("steamSummerAchievement"))
	{
		setUpdateDiscovered("steamSummerAchievement");
	}
	if ($WII && $demoVersion)
	{
		lbl_achievements_title.text = text;
	}
	else
	{
		lbl_achievements_title.text = $lbl_achievementsOf SPC getUserName(profileId);
	}
	list.clearList();
	%columns = list.getColumns();
	%unlocked = getProfileData(profileId, "Secrets", "Achieved");
	debugEcho("reading achievements of this profile" SPC profileId SPC "unlcked" SPC %unlocked);
	if (getWordCount(achievemenSorted) != getFieldCount(internalNames))
	{
		debugEcho("WARNING: menu_achievements::onDialogPush: sorted (and displayed) achievements are not as many as internalNames. Check achievements coutn!");
	}
	%listIndex = 0;
	%i = 0;
	while (%i < getWordCount(achievemenSorted))
	{
		%index = indices[getWord(achievemenSorted, %i)];
		if (!(achievements.isIndexAchieveable(%index)))
		{
		}
		else
		{
			if (findWord(%unlocked, %index))
			{
				list.addRow(['Path', <torque.FuncCall object at 0x000002171792E3C0>, '".jpg"'] TAB getField(names, %index), "" TAB getField(descriptions, %index));
				list.setRowSelected(%listIndex, "1");
			}
			else
			{
				list.addRow(['Path', <torque.FuncCall object at 0x000002171792D490>, '"_locked.jpg"'] TAB getField(names, %index), "" TAB getField(descriptions, %index), "", "", !(getWord(availability, %i)));
			}
			if (!(getWord(availability, %i)))
			{
				list.setRowNotAvailable(%listIndex);
			}
			%listIndex = %listIndex + 1.0;
		}
		%i = %i + 1.0;
	}
	return getWordCount(achievemenSorted);
}
function achievements::isIndexAchieveable(%this, %index)
{
	return findWord(achieveableIndizes, %index);
	return findWord(achieveableIndizes, %index);
}
function achievements::getAchieved(%this, %achievementName)
{
	return findWord($settings::Secrets::Achieved, indices[%achievementName]);
	return findWord($settings::Secrets::Achieved, indices[%achievementName]);
}
function achievements::setAchieved(%this, %achievementName, %saveNow)
{
	%achievementIndex = indices[%achievementName];
	if (!(achievements.isIndexAchieveable(%achievementIndex)))
	{
		return achievements.isIndexAchieveable(%achievementIndex);
	}
	debugEcho("yeah, you achieved:" SPC %achievementName);
	if (%saveNow $= "")
	{
		%saveNow = 1;
	}
	if (isExternalUser(-1.0) && isExternalOnline())
	{
		if (!(getExternalAchievement(%achievementName)))
		{
			setExternalAchievement(%achievementName, %saveNow);
			if (getPublisherName() $= "Greenhouse" && !(%this.getAchieved(%achievementName)))
			{
				%this.showAchieved(%achievementIndex);
			}
		}
	}
	else
	{
		if (!(%this.getAchieved(%achievementName)))
		{
			%this.showAchieved(%achievementIndex);
		}
	}
	if (isObject(playmodeManager))
	{
		playmodeManager.unlockPlaymode(%achievementIndex);
	}
	unlockCheats(%achievementIndex);
	if (!(%this.getAchieved(%achievementName)))
	{
		setSecretData("Achieved", $settings::Secrets::Achieved SPC %achievementIndex, !($WII));
		if (getWordCount($settings::Secrets::Achieved) >= getWordCount(internalNames) - 1.0)
		{
			%this.checkCrazy();
		}
	}
	return;
}
function achievements::getExternalList(%this)
{
	%list = "";
	%i = 0;
	while (%i < getFieldCount(internalNames))
	{
		%name = getField(internalNames, %i);
		if (getExternalAchievement(%name))
		{
			%list = %list SPC indices[%name];
		}
		%i = %i + 1.0;
	}
	return trim(%list);
	return trim(%list);
}
function achievements::onFirstKeyPressed(%this)
{
	if (!(%this.getAchieved("Random")))
	{
		%this.onRandom();
	}
	if (!(%this.getAchieved("AyimAddicted")))
	{
		%this.addictID = $settings::Personal::Id;
		debugEcho("checking posible addict:" SPC addictID SPC "time left" SPC addictPlaytimeLeft[addictID]);
		if (addictPlaytimeLeft[addictID] $= "")
		{
			%this.addictPlaytimeLeft[addictID] = $achievement::addictPlayTime * 60.0 * 1000.0;
			debugEcho("new profile in this session possible addict:" SPC addictID);
		}
		%this.addictionSchedule = %this.schedule(addictPlaytimeLeft[addictID], "onAddicted");
	}
	%this.kamikazeStartObject = groundCollisionObject;
	return;
}
function achievements::onRandom(%this)
{
	%minSchedTime = 3;
	%maxSchedTime = 40;
	%deathInfluence = 0.0010000000474974513;
	%lastTimeInfluence = 0.009999999776482582;
	%maxRandomChanceValue = 0.02500000037252903;
	if (randomTime > 0.0)
	{
		%deathChance = min(playerDeathCount * %deathInfluence, 1.0 / 300.0);
		%timeChance = 1.0 - randomTime - %minSchedTime / %maxSchedTime - %minSchedTime * %lastTimeInfluence;
		%randomChance = floatRandom(-1.0 * %maxRandomChanceValue, %maxRandomChanceValue);
		if (getRandom() > $PI / 100.0)
		{
		}
		else
		{
		}
		%strenghtOfPIChance = $PI;
		%youWillGetIt = baseRandomChance + %deathChance + %timeChance + %randomChance * %strenghtOfPIChance * 100.0;
		%luckyNumber = getRandom() * 100.0;
		if (%luckyNumber < %youWillGetIt)
		{
			if (getRandom() < 0.6660000085830688)
			{
				echo("an evil alien spaceship took your nearly achieved random achievement and flushed it down the toilet");
				%this.baseRandomChance = max(baseRandomChance - baseRandomChance * getRandom() / $HALF_PI, "0");
				break;
			}
			%this.setAchieved("Random");
			return;
		}
	}
	%this.randomTime = getRandom(%minSchedTime, %maxSchedTime);
	%this.randomSchedule = %this.schedule(randomTime * 1000.0, "onRandom");
	return;
}
function achievements::onAddicted(%this)
{
	%this.setAchieved("AyimAddicted");
	return;
}
function achievements::onKamikaze(%this)
{
	if (!(achievements.getAchieved("Kamikaze")))
	{
		$deathCallbackKamikaze = 1;
	}
	return achievements.getAchieved("Kamikaze");
}
function achievements::onQuarryOut(%this)
{
	%this.demolitionManCounter = demolitionManCounter + 1.0;
	if (demolitionManCounter >= demolitionManObjects)
	{
		%this.setAchieved("DemolitionMan");
	}
	return;
}
function achievements::onLevelFadeOutEnded(%this)
{
	%this.levelHasEnded();
	return;
}
function achievements::onEnterLevelSwitch(%this)
{
	%this.levelHasEnded();
	return;
}
function achievements::levelHasEnded(%this)
{
	%this.checkProgressAchievements();
	if (!(%this.getAchieved("Gift")) && currentLevelObject.getId() == level_credits.getId())
	{
		%this.setAchieved("Gift");
	}
	if (!(%this.getAchieved("BonusLevel")) && currentLevelObject.getId() == level_finalLevel.getId() && levelDeathCount == 0.0)
	{
		%this.setAchieved("BonusLevel");
	}
	if (!(%this.getAchieved("Frozen")) && !(moveKeyPressed))
	{
		%this.setAchieved("Frozen");
	}
	if (!(%this.getAchieved("AllBoni")) && currentLevelNumber >= 19.0)
	{
		if (getWordIndex($settings::Secrets::BonusLevelsFinished, currentLevelNumber) == -1.0)
		{
			$settings::Secrets::BonusLevelsFinished = ltrim($settings::Secrets::BonusLevelsFinished SPC currentLevelNumber);
		}
		debugEcho("we finished this bonus level:" SPC currentLevelNumber SPC "finished levels:" SPC $settings::Secrets::BonusLevelsFinished);
		if (getWordCount($settings::Secrets::BonusLevelsFinished) >= 4.0)
		{
			%this.setAchieved("AllBoni");
		}
	}
	return;
}
function achievements::checkProgressAchievements(%this)
{
	%unlockedLevels = max($settings::Secrets::LevelsUnlocked, currentLevelNumber - 1.0);
	if (!(%this.getAchieved("Cave")) && %unlockedLevels >= 4.0)
	{
		achievements.setAchieved("Cave");
	}
	if (!(%this.getAchieved("Jungle")) && %unlockedLevels >= 10.0)
	{
		achievements.setAchieved("Jungle");
	}
	if (!(%this.getAchieved("Trip")) && %unlockedLevels >= 16.0)
	{
		achievements.setAchieved("Trip");
	}
	return;
}
function achievements::onEnvironmentCompleted(%this)
{
	if (totalTime <= $achievement::time[chapterToBeat] * 60.0)
	{
		%this.setAchieved("TimeChallenge" @ chapterToBeat);
	}
	return;
}
function achievements::onPlayerDeath(%this)
{
	%this.levelDeathCount = levelDeathCount + 1.0;
	if ($deathCallbackKamikaze)
	{
		$deathCallbackKamikaze = 0;
		if (groundCollisionObject == kamikazeStartObject)
		{
			%this.setAchieved("Kamikaze");
		}
	}
	if ($deathCallbackSuizidal)
	{
		%totalDeathCount = $settings::Secrets::TotalDeathCount + levelDeathCount;
		if (%totalDeathCount >= $achievement::sucidalTendenciesDeathCount)
		{
			%this.setAchieved("SuicidalTendencies");
			$deathCallbackSuizidal = 0;
		}
	}
	%this.accumulatedSurfDist = "0";
	return;
}
function achievements::clearAll(%this)
{
	%i = 0;
	while (%i < getFieldCount(internalNames))
	{
		%name = getField(names, %i);
		clearExternalAchievement(%name);
		%i = %i + 1.0;
	}
	$settings::Secrets::Achieved = "";
	return;
}
function achievements::onDeathEvent(%this, %reason)
{
	%possibleReasons = "squash fall strike rippededge saurian monkey monster bees fire";
	debugEcho("achievements death reason:" SPC %reason);
	if (!(findWord($achievement::deathEvents[$settings::Personal::Id], %reason)))
	{
		$achievement::deathEvents[$settings::Personal::Id] = trim($achievement::deathEvents[$settings::Personal::Id] SPC %reason);
		debugEcho("got new death event:" SPC %reason SPC "having now:" SPC $achievement::deathEvents[$settings::Personal::Id]);
		if (getWordCount($achievement::deathEvents[$settings::Personal::Id]) == getWordCount(%possibleReasons))
		{
			unSubscribeFromEvents(%this, "onDieFragged onDieOutside onDieBurning onPlayerReanimate");
			%this.setAchieved("Rip");
		}
	}
	return;
}
function achievements::onDieFragged(%this)
{
	if (lastDeathEvent $= "")
	{
		%this.onDeathEvent("fall");
	}
	else
	{
		if (lastDeathEvent != "fire")
		{
			%this.onDeathEvent(lastDeathEvent);
		}
	}
	return;
}
function achievements::onDieOutside(%this)
{
	%this.onDeathEvent("rippededge");
	return;
}
function achievements::onDieBurning(%this)
{
	%this.onDeathEvent("fire");
	return;
}
function achievements::onPlayerReanimate(%this)
{
	player.lastDeathEvent = "";
	return;
}
function achievements::showAchieved(%this, %achievementId)
{
	%bitmap = Path @ getField(icons, %achievementId);
	%title = $lbl_unlocked;
	%name = getField(names, %achievementId);
	%this.showPopup(%bitmap, %title, %name);
	return;
}
function achievements::showPopup(%this, %bitmapPath, %titleText, %nameText)
{
	%timeToShow = lastPopupShownTime + 2.0 - thisTime;
	if (%timeToShow - 0.30000001192092896 > 0.0)
	{
		debugEcho("showing achievement popup delayed:" SPC lastPopupShownTime SPC %timeToShow SPC thisTime);
		%this.showPopupSchedule = %this.schedule(%timeToShow * 1000.0, "showPopUp", %bitmapPath, %titleText, %nameText);
		return;
	}
	if (isEventPending(hidePopupSchedule))
	{
		cancel(hidePopupSchedule);
	}
	%this.lastPopupShownTime = thisTime;
	debugEcho("showing popup of achievement" SPC %nameText SPC "time:" SPC lastPopupShownTime);
	if (!(popupShown))
	{
		Canvas.pushDialog(popup_unlocked);
	}
	%this.popupShown = "1";
	bm_unlocked.bitmap = %bitmapPath;
	lbl_unlocked_unlocked.text = %titleText;
	lbl_unlocked_name.text = %nameText;
	%this.hidePopupSchedule = %this.schedule("5000", "hidePopUp");
	return;
}
function achievements::hidePopUp(%this)
{
	%this.popupShown = "0";
	Canvas.popDialog(popup_unlocked);
	return;
}
function achievements::checkSurferCondition(%this, %surfBoard)
{
	if (%surfBoard.getBehavior("BeMoving") && %surfBoard.getBehavior("BeBreaking") && !(!(quarriedOut)))
	{
		if (isObject(arenaMonster) && %surfBoard.getId() == arenaMonster.getId())
		{
			return %surfBoard.getId();
		}
		if (!(isObject(lastSurfBoard)))
		{
			%this.lastSurfBoard = %surfBoard;
		}
		if (%surfBoard.getId() != lastSurfBoard.getId())
		{
			%this.lastBoardPosition = "";
		}
		if (lastBoardPosition != "")
		{
			%dist = t2dVectorDistance(lastBoardPosition, %surfBoard.getPosition());
			if (%dist > 0.05000000074505806)
			{
				%this.accumulatedSurfDist = accumulatedSurfDist + %dist;
				if (accumulatedSurfDist > $achievement::snowboardDistance)
				{
					if (environment == 1.0)
					{
						%this.setAchieved("Snowboarder");
					}
				}
			}
		}
		%this.lastSurfBoard = %surfBoard;
		%this.lastBoardPosition = %surfBoard.getPosition();
	}
	return;
}
function achievements::finishSurferCondition(%this, %surfBoard)
{
	if (%surfBoard.getBehavior("BeMoving") && %surfBoard.getBehavior("BeBreaking") && !(!(!(quarriedOut))))
	{
		if (accumulatedSurfDist > $achievement::surfDistance)
		{
			%this.setAchieved("Surfer");
		}
		if (isObject(thrower))
		{
			if (lastSurfBoard.getId() == thrower.getId())
			{
				%this.setAchieved("Casey");
			}
		}
		%this.accumulatedSurfDist = "0";
	}
	return;
}
function achievements::checkCrazy(%this)
{
	if (achievements.getAchieved("Perfectionist") && achievements.getAchieved("Crazy"))
	{
		return achievements.getAchieved("Crazy");
	}
	debugEcho("checking for perfectionist & crazy");
	%modes = "RallyMode LimitedRotationMode SurvivalMode";
	%i = 0;
	while (%i < getWordCount(%modes))
	{
		%mode = getWord(%modes, %i);
		debugEcho("mode" SPC %mode SPC "stamps:" SPC $settings::Playmodes::Stamps[%mode]);
		%j = 1;
		if (%j < getWordCount($settings::Playmodes::Stamps[%mode]))
		{
			%stamp = getWord($settings::Playmodes::Stamps[%mode], %j);
			if (%stamp == 2.0)
			{
				%notAllStamps = 1;
				%notAllGoldStamps = 1;
				while ()
				{
					while ()
					{
					}
					if (%stamp > -1.0)
					{
						%notAllGoldStamps = 1;
					}
				}
				%j = %j + 1.0;
			}
		}
		if (%notAllStamps)
		{
			break;
		}
		%i = %i + 1.0;
	}
	if (!(%notAllStamps))
	{
		achievements.setAchieved("Perfectionist");
		if (!(%notAllGoldStamps) && getWordCount($settings::Secrets::Achieved) >= getWordCount(internalNames) - 1.0)
		{
			achievements.setAchieved("Crazy");
		}
	}
	return;
}
function achievements::resetSwingStats(%this)
{
	%this.swingBucketSize = 360.0 / $achievement::swingBuckets;
	%this.actualBucket = -1.0;
	%this.swingDirection = "0";
	return;
}
function achievements::onSwingRotation(%this, %rotation, %distanceFactor, %swing)
{
	if (groundCollisionObject.getId() != %swing.getId())
	{
		%this.resetSwingStats();
	}
	else
	{
		if (%distanceFactor > $achievement::swingMinDistanceFactor && !(isFalling))
		{
			%bucketIndex = mFloor(%rotation / swingBucketSize);
			if (actualBucket == -1.0)
			{
				%this.actualBucket = %bucketIndex;
				%this.initialBucket = %bucketIndex;
				break;
			}
			if (%bucketIndex != actualBucket)
			{
				%bucketDist = %bucketIndex - actualBucket;
				if (mAbs(%bucketDist) == $achievement::swingBuckets - 1.0)
				{
					%bucketDist = -1.0 * getSign(%bucketDist);
				}
				if (swingDirection == 0.0)
				{
					%this.swingDirection = %bucketDist;
				}
				if (mAbs(%bucketDist) > 1.0 || swingDirection != %bucketDist)
				{
					%this.resetSwingStats();
					return;
				}
				if (negModulo(%bucketIndex + %bucketDist, $achievement::swingBuckets) == initialBucket)
				{
					%this.setAchieved("AroundTheWorld");
				}
				%this.actualBucket = %bucketIndex;
			}
		}
	}
	return;
}
function achievements::onStartFalling(%this)
{
	%this.lastBoardPosition = "";
	return;
}
function achievements::onRotationStart(%this)
{
	%this.lastBoardPosition = "";
	return;
}
function achievements::onLevelShutdown(%this)
{
	if (isEventPending(showPopupSchedule))
	{
		cancel(showPopupSchedule);
	}
	if (isEventPending(hidePopupSchedule))
	{
		cancel(hidePopupSchedule);
		%this.hidePopUp();
	}
	if (isEventPending(randomSchedule))
	{
		cancel(randomSchedule);
	}
	if (isEventPending(addictionSchedule))
	{
		%this.addictPlaytimeLeft[addictID] = getEventTimeLeft(addictionSchedule);
		cancel(addictionSchedule);
	}
	return;
}
function achievements::checkAchievementStatus(%this)
{
	if (checkedProfile $= $settings::Personal::Id)
	{
		return;
	}
	%this.checkedProfile = $settings::Personal::Id;
	%this.checkProgressAchievements();
	%playmodes = "LimitedRotationMode RallyMode SurvivalMode";
	%i = 0;
	while (%i < getWordCount(%playmodes))
	{
		%mode = getWord(%playmodes, %i);
		playmodeManager.checkStampAchievement(%mode);
		%i = %i + 1.0;
	}
	%this.checkCrazy();
	%i = 0;
	while (%i < getWordCount($settings::Secrets::Achieved))
	{
		%achievementIndex = getWord($settings::Secrets::Achieved, %i);
		unlockCheats(%achievementIndex);
		%i = %i + 1.0;
	}
	return getWordCount($settings::Secrets::Achieved);
}
