// menuStatistics.cs.dso
function menu_statistics::onDialogPush(%this)
{
	if ($WII)
	{
		Canvas.showCursor("0");
		Canvas.setCursor("0", ayimCursor);
	}
	else
	{
		Canvas.showCursor();
	}
	if ($distributorName $= "BigFishGames")
	{
		lbl_statistics_globalRank.setVisible("0");
		lbl_statistics_globalNumber.setVisible("0");
		%globalRank = -1.0;
	}
	else
	{
		if (Webclient.isOnline())
		{
			%globalRank = Webclient.getRank(totalTime);
			break;
		}
		%globalRank = -1.0;
	}
	%this.times = totalTime;
	%this.competitorTimes = Statistics.getTime(oldData);
	if (oldData $= "")
	{
	}
	else
	{
	}
	%this.timeDiffs = times - competitorTimes;
	if (timeDiffs < 0.0 || timeDiffs $= "*")
	{
	}
	else
	{
	}
	lbl_statistics_competitorHeader.text = $lbl_youLost;
	lbl_statistics_name.text = getUserName(-1.0);
	lbl_statistics_competitorName.text = Statistics.getUser(oldData);
	if (wholeEnvironment)
	{
		%this.levelNames = "Total";
		if (strpos(currentLevelObject.getName(), "cave") > -1.0)
		{
			%this.levelNames = levelNames TAB $caveLevelNames;
		}
		else
		{
			if (strpos(currentLevelObject.getName(), "jungle") > -1.0)
			{
				%this.levelNames = levelNames TAB $jungleLevelNames;
				break;
			}
			if (strpos(currentLevelObject.getName(), "trip") > -1.0)
			{
				%this.levelNames = levelNames TAB $tripLevelNames;
			}
		}
		%newLevelTimes = Statistics.getLevelTimesOfSpanwpointTimes(spawnpointTimes);
		%this.times = times SPC %newLevelTimes;
		%oldLevelTimes = Statistics.getLevelTimes(oldData);
		%this.competitorTimes = competitorTimes SPC %oldLevelTimes;
		%i = 0;
		while (%i < getWordCount(%newLevelTimes))
		{
			if (oldData $= "")
			{
			}
			else
			{
			}
			%newTimeDiff = getWord(%newLevelTimes, %i) - getWord(%oldLevelTimes, %i);
			%this.timeDiffs = timeDiffs SPC %newTimeDiff;
			%i = %i + 1.0;
		}
	}
	else
	{
		%oldSpawnpointTimes = removeWord(Statistics.getSpawnpointTimes(oldData), "0");
		%newSpawnpointTimes = removeWord(spawnpointTimes, "0");
		%newSpawnpointTimes = removeWord(%newSpawnpointTimes, getWordCount(%newSpawnpointTimes) - 1.0);
		%oldSpawnpointTimes = removeWord(%oldSpawnpointTimes, getWordCount(%oldSpawnpointTimes) - 1.0);
		%i = 0;
		while (%i < getWordCount(%newSpawnpointTimes))
		{
			%this.times = times SPC getWord(%newSpawnpointTimes, %i);
			%this.competitorTimes = competitorTimes SPC getWord(%oldSpawnpointTimes, %i);
			if (getWord(%newSpawnpointTimes, %i) $= "*" || getWord(%oldSpawnpointTimes, %i) $= "*" || oldData $= "")
			{
				%this.timeDiffs = timeDiffs SPC "*";
			}
			else
			{
				%this.timeDiffs = timeDiffs SPC getWord(%newSpawnpointTimes, %i) - getWord(%oldSpawnpointTimes, %i);
			}
			%i = %i + 1.0;
		}
	}
	%this.currentTime = "0";
	debugEcho("times:" SPC times SPC "timeDiffs" SPC timeDiffs SPC "competitorTimes:" SPC competitorTimes SPC "oSP:" SPC %oldSpawnpointTimes SPC "nSp" SPC %newSpawnpointTimes SPC "st.spT:" SPC spawnpointTimes);
	%this.showTime();
	lbl_statistics_localNumber.text = "23";
	if (wholeEnvironment)
	{
	}
	else
	{
	}
	%levelscriptOject = currentLevelObject;
	Webclient.setPermanentFilter("level", levelID);
	if (%globalRank > -1.0)
	{
		lbl_statistics_globalNumber.text = %globalRank;
	}
	else
	{
		lbl_statistics_globalNumber.text = $lbl_not_online;
	}
	%this.setSubmitSentence();
	%buttons = "mainMenu replay";
	if ($settings::Personal::Language $= "french")
	{
		%i = 0;
		while (%i < getWordCount(%buttons))
		{
			%currentButton = "btn_statistics_" @ getWord(%buttons, %i);
			%currentButton.Extent = "200 90";
			%i = %i + 1.0;
		}
	}
	else
	{
		if (Extent != "180 90")
		{
			%i = 0;
			while (%i < getWordCount(%buttons))
			{
				%currentButton = "btn_statistics_" @ getWord(%buttons, %i);
				%currentButton.Extent = "180 90";
				%i = %i + 1.0;
			}
		}
	}
	return getWordCount(%buttons);
}
function menu_statistics::onEscapePressed(%this)
{
	return getWordCount(%buttons);
}
function menu_statistics::previousTime(%this)
{
	%this.currentTime = currentTime - 1.0;
	if (currentTime < 0.0)
	{
		%this.currentTime = getWordCount(times) + currentTime;
	}
	else
	{
		%this.currentTime = currentTime % getWordCount(times);
	}
	debugEcho("previousTime:" SPC currentTime);
	%this.showTime();
	return;
}
function menu_statistics::nextTime(%this)
{
	%this.currentTime = currentTime + 1.0;
	%this.currentTime = currentTime % getWordCount(times);
	debugEcho("next:" SPC currentTime);
	%this.showTime();
	return;
}
function menu_statistics::showTime(%this)
{
	if (wholeEnvironment)
	{
		lbl_statistics_timeHeader.text = getField(levelNames, currentTime);
	}
	else
	{
		if (currentTime == 0.0)
		{
			lbl_statistics_timeHeader.text = $lbl_total;
			lbl_statistics_timeHeader.setProfile("AyimMenuTextCenterProfile");
			break;
		}
		lbl_statistics_timeHeader.text = $lbl_spawnpoint SPC currentTime;
		if ($settings::Personal::Language $= "japanese")
		{
			lbl_statistics_timeHeader.setProfile("AyimMenuTextCenterSmallProfile");
			break;
		}
		lbl_statistics_timeHeader.setProfile("AyimMenuTextCenterProfile");
	}
	lbl_statistics_time.text = Statistics.getReadableTime(getWord(times, currentTime));
	lbl_statistics_competitorTime.text = Statistics.getReadableTime(getWord(competitorTimes, currentTime));
	%timeDiff = getWord(timeDiffs, currentTime);
	debugEcho("showing time of" SPC text SPC "with timediff" SPC %timeDiff);
	if (%timeDiff $= "*")
	{
		lbl_statistics_timeDiff.setProfile("AyimHeader1CenterGreenProfile");
	}
	else
	{
		if (%timeDiff > 0.0)
		{
			lbl_statistics_timeDiff.setProfile("AyimHeader1CenterRedProfile");
			break;
		}
		lbl_statistics_timeDiff.setProfile("AyimHeader1CenterGreenProfile");
	}
	lbl_statistics_timeDiff.text = Statistics.getReadableTime(%timeDiff, "0", "1");
	return;
}
function menu_statistics::setSubmitSentence(%this)
{
	btn_statistics_dontSubmit.setVisible("0");
	btn_statistics_submitTime.setVisible("0");
	btn_statistics_submitGhost.setVisible("0");
	if ($distributorName $= "BigFishGames")
	{
		lbl_statistics_submitLine1.setVisible("0");
		lbl_statistics_submitLine2.setVisible("0");
		btn_statistics_change.setVisible("0");
	}
	else
	{
		if ($cheatsWereUsed)
		{
			lbl_statistics_submitLine1.setProfile("AyimMenuTextCenterRedProfile");
			lbl_statistics_submitLine2.setProfile("AyimMenuTextCenterRedProfile");
			lbl_statistics_submitLine1.text = $lbl_mode_nosaving;
			lbl_statistics_submitLine2.text = "";
			btn_statistics_change.setActive("0");
			break;
		}
		if ($currentVersion < Webclient.getCurrentVersion())
		{
			lbl_statistics_submitLine1.setProfile("AyimMenuTextCenterRedProfile");
			lbl_statistics_submitLine2.setProfile("AyimMenuTextCenterRedProfile");
			lbl_statistics_submitLine1.text = $lbl_updateToSubmit_line1;
			lbl_statistics_submitLine2.text = $lbl_updateToSubmit_line2;
			btn_statistics_change.setActive("0");
			break;
		}
		if (!($WII))
		{
			lbl_statistics_submitLine1.setProfile("AyimMenuTextCenterProfile");
			lbl_statistics_submitLine2.setProfile("AyimMenuTextCenterProfile");
			btn_statistics_change.setActive("1");
			if ($settings::Statistics::SubmitTime $= "" && $settings::Statistics::SubmitGhost $= "")
			{
				%this.showSubmitQuestion();
				break;
			}
			if ($settings::Statistics::SubmitTime && !($settings::Statistics::SubmitGhost))
			{
				lbl_statistics_submitLine1.text = $lbl_submitTime_line1;
				lbl_statistics_submitLine2.text = $lbl_submitTime_line2;
				btn_statistics_change.setVisible("1");
				break;
			}
			if ($settings::Statistics::SubmitTime && $settings::Statistics::SubmitGhost)
			{
				lbl_statistics_submitLine1.text = $lbl_submitGhost_line1;
				lbl_statistics_submitLine2.text = $lbl_submitGhost_line2;
				btn_statistics_change.setVisible("1");
				break;
			}
			lbl_statistics_submitLine1.text = $lbl_submitNothing_line1;
			lbl_statistics_submitLine2.text = $lbl_submitNothing_line2;
			btn_statistics_change.setVisible("1");
		}
	}
	return;
}
function menu_statistics::changeSubmission(%this, %submissionMode)
{
	if (%submissionMode == 1.0)
	{
		$settings::Statistics::SubmitTime = 1;
		$settings::Statistics::SubmitGhost = 0;
	}
	else
	{
		if (%submissionMode == 2.0)
		{
			$settings::Statistics::SubmitTime = 1;
			$settings::Statistics::SubmitGhost = 1;
			break;
		}
		if (%submissionMode == 3.0)
		{
			$settings::Statistics::SubmitTime = 0;
			$settings::Statistics::SubmitGhost = 0;
			break;
		}
		$settings::Statistics::SubmitTime = "";
		$settings::Statistics::SubmitGhost = "";
	}
	%this.setSubmitSentence();
	return;
}
function menu_statistics::showSubmitQuestion(%this)
{
	lbl_statistics_submitLine1.text = $lbl_submitQuestion_line1;
	lbl_statistics_submitLine2.text = $lbl_submitQuestion_line2;
	btn_statistics_change.setVisible("0");
	btn_statistics_dontSubmit.setVisible("1");
	btn_statistics_submitTime.setVisible("1");
	if ($settings::Personal::Language $= "japanese")
	{
		btn_statistics_submitGhost.setProfile("AyimButtonSmallTextProfile");
	}
	else
	{
		btn_statistics_submitGhost.setProfile("AyimButtonProfile");
	}
	btn_statistics_submitGhost.setVisible("1");
	return;
}
function menu_statistics::goOn(%this, %target, %dontSubmitTime)
{
	debugEcho("going on with target" SPC %target);
	if (!(%dontSubmitTime))
	{
		Statistics.submitOnline();
	}
	%replayLevel = 0;
	if (%target $= "main")
	{
		%thisLevel = currentLevelObject;
		menu_main.gotoCompetition = "1";
		if (wholeEnvironment)
		{
			menu_main.levelButtonToPress = ['"btn_level_"', 'environment'];
		}
		else
		{
			menu_main.levelButtonToPress = ['"btn_"', '%thisLevel'];
		}
		playmodeManager.backToMenu();
	}
	else
	{
		if (%target $= "replay")
		{
			%this.back();
			%replayLevel = 1;
			stopMenuMusic();
			triggerEvent("onMenuReplayLevel");
		}
	}
	Statistics.reset(%replayLevel);
	return;
}
function menu_statistics::loadFasterGhost(%this, %loadGhost)
{
	if (%loadGhost)
	{
		if (playingLocal)
		{
			%savedData = Statistics.getSavedTimes();
			if (rank < 2.0)
			{
			}
			else
			{
			}
			%newCompetitorRank = rank - 1.0;
			%oldScore = getRecord(%savedData, %newCompetitorRank - 1.0);
			Statistics.oldData = %oldScore;
			Statistics.oldRank = %newCompetitorRank;
			%ghost = Statistics.getGhost(%oldScore);
			SpeedRunMode.setupSpeedRun(currentLevelObject.getName(), wholeEnvironment, %ghost, "0", playingLocal);
			break;
		}
		echo("TODO: this behavior is not implemented for global scores yet");
	}
	menu_dialog.back();
	triggerEvent("onMenuReplayLevel");
	Statistics.reset();
	return;
}
