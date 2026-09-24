// splashScreener.cs.dso
if (!(isObject(BeSplashScreener)))
{
	%template = new BehaviorTemplate(Name : BeSplashScreener);
	%template.friendlyName = "SplashScreener";
	%template.behaviorType = "specify a type";
	%template.description = "user friendly description of the behavior";
	%template.addBehaviorField(screenGroups, "list of groups, that contain splashes", string, "");
	%template.addBehaviorField(screenDuration, "space seperatedlist of how long every screen stays (s)", string, "2.5");
	%template.addBehaviorField(fadeTime, "fade in/out time (s)", float, "0.5");
	%template.addBehaviorField(pauseTime, "pause between fades (s)", float, "0.5");
}
function BeSplashScreener::onBehaviorAdd(%this)
{
	%owner = Owner;
	$splashScreenerObject = %this;
	subscribeToEvents(%this, "onLevelLoadFinished10 onLevelLoadFinished");
	%owner.setLayer("1");
	return;
}
function BeSplashScreener::onLevelLoadFinished10(%this)
{
	%owner = Owner;
	if ($splashScreenShown)
	{
		%i = 0;
		while (%i < getWordCount(screenGroups))
		{
			%this.setAllVisible("0", %i);
			%i = %i + 1.0;
		}
		%this.setAllVisible("1", getWordCount(screenGroups) - 1.0);
		%owner.setVisible("0");
		%owner.setBlendAlpha("0");
		return;
	}
	if ($WII)
	{
		tex_brokenRules.setSize("96 96");
	}
	%i = 0;
	while (%i < getWordCount(screenGroups))
	{
		%this.setAllVisible("0", %i);
		if (getWord(screenGroups, %i) $= "esrb")
		{
			%ersbIndex = %i;
		}
		if (getWord(screenGroups, %i) $= "bfg")
		{
			%bigFishIndex = %i;
		}
		%i = %i + 1.0;
	}
	if (!($demoVersion) || $WII && !(showESRB()))
	{
		%this.screenGroups = removeWord(screenGroups, %esrbIndex);
		%this.screenDuration = removeWord(screenDuration, %esrbIndex);
		if (%ersbIndex < %bigFishIndex)
		{
			%bigFishIndex = %bigFishIndex - 1.0;
		}
	}
	if ($distributorName != "BigFishGames")
	{
		%this.screenGroups = removeWord(screenGroups, %bigFishIndex);
		%this.screenDuration = removeWord(screenDuration, %bigFishIndex);
	}
	%this.activeScreenIndex = -1.0;
	%this.alphaVelocity = 1.0 / fadeTime;
	return;
}
function BeSplashScreener::onLevelLoadFinished(%this)
{
	if ($splashScreenShown)
	{
		camera.initialFadeCompleted = "1";
		return;
	}
	if ($settings::Controls::SixenseEnabled)
	{
		$splashScreenObject = %this;
		setupSixenseCalibration("1");
	}
	else
	{
		%this.startSplashes();
	}
	return;
}
function BeSplashScreener::startSplashes(%this, %time)
{
	echo("in BeSplashScreener::startSplashes");
	if (%time $= "")
	{
		%time = 2.5;
	}
	splashScreenMap.push();
	if ($settings::Controls::SixenseEnabled)
	{
		if (isObject(sixenseManager))
		{
			sixenseManager.setInput("splash");
		}
	}
	echo("starting next fadein in" SPC %time SPC "seconds");
	%this.fadeSchedule = %this.schedule(%time * 1000.0, "fadeInNext");
	return;
}
function BeSplashScreener::fadeInNext(%this)
{
	echo("started fadeInNext");
	%owner = Owner;
	%this.activeScreenIndex = activeScreenIndex + 1.0;
	if (activeScreenIndex + 1.0 == getWordCount(screenGroups) - 1.0)
	{
		$splashesDone = 1;
	}
	%this.state = "fadein";
	%this.setAllVisible("1", activeScreenIndex);
	%owner.setAlphaVelocity(-1.0 * alphaVelocity);
	%this.fadeSchedule = %this.schedule(fadeTime * 1000.0, "screenDisplayed");
	return;
}
function BeSplashScreener::screenDisplayed(%this)
{
	echo("started screenDisplayed");
	%owner = Owner;
	%this.state = "displayed";
	%owner.setAlphaVelocity("0");
	%owner.setBlendAlpha("0");
	if (activeScreenIndex + 1.0 == getWordCount(screenGroups) - 1.0)
	{
		$splashesDone = 1;
		%this.fadeSchedule = %this.schedule("1000", "gotoMenu");
		return;
	}
	if (getWordCount(screenDuration) == getWordCount(screenGroups))
	{
		%duration = getWord(screenDuration, activeScreenIndex);
	}
	else
	{
		%duration = getWord(screenDuration, "0");
	}
	%this.fadeSchedule = %this.schedule(%duration * 1000.0, "fadeOutScreen", fadeTime * 1000.0);
	return;
}
function BeSplashScreener::fadeOutScreen(%this, %durationMS)
{
	echo("started fadeOutScreen");
	%owner = Owner;
	%this.state = "fadeout";
	%fadeOutVelocity = 1.0 / fadeTime;
	%owner.setAlphaVelocity(%fadeOutVelocity);
	debugEcho("fading out with" SPC %durationMS SPC %fadeOutVelocity);
	%this.fadeSchedule = %this.schedule(%durationMS, "pauseScreen");
	return;
}
function BeSplashScreener::pauseScreen(%this)
{
	echo("started pauseScreen");
	%owner = Owner;
	%this.state = "paused";
	%owner.setAlphaVelocity("0");
	%owner.setBlendAlpha("1");
	%this.setAllVisible("0", activeScreenIndex);
	%this.fadeSchedule = %this.schedule(pauseTime * 1000.0, "fadeInNext");
	return;
}
function BeSplashScreener::setAllVisible(%this, %visible, %groupIndex)
{
	%screen = getWord(screenGroups, %groupIndex);
	%i = 0;
	while (%i < %screen.getCount())
	{
		%object = %screen.getObject(%i);
		if (%visible)
		{
			%object.setVisible(originalVisibility);
		}
		else
		{
			%object.originalVisibility = %object.getVisible();
			%object.setVisible("0");
		}
		%i = %i + 1.0;
	}
	return %screen.getCount();
}
function BeSplashScreener::gotoMenu(%this)
{
	echo("splash screeners done. going to menu");
	splashScreenMap.pop();
	if ($settings::Controls::SixenseEnabled)
	{
		if (isObject(sixenseManager))
		{
			sixenseManager.setInput("menu");
		}
	}
	sceneWindow2d.setUseWindowMouseEvents("0");
	$splashScreenShown = 1;
	MenuAction::loadLevel(level_gameMenu);
	return;
}
function BeSplashScreener::keyPressed()
{
	echo("key or mouse pressed during splash screens shown.");
	if ($splashesDone)
	{
		return;
	}
	if (state $= "fadein")
	{
		%fadeOutTime = fadeTime * 1000.0 - getEventTimeLeft(fadeSchedule);
	}
	else
	{
		if (state $= "displayed")
		{
			%fadeOutTime = fadeTime * 1000.0;
			break;
		}
		return $splashScreenerObject;
	}
	debugEcho("fading out with " SPC state SPC %fadeOutTime);
	cancel(fadeSchedule);
	$splashScreenerObject.fadeOutScreen(%fadeOutTime);
	return;
}
function BeSplashScreener::onMouseDown(%this)
{
	BeSplashScreener::keyPressed();
	return;
}
function onSplashScreenKeyPressed()
{
	BeSplashScreener::keyPressed();
	return;
}
