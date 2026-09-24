// demoEndTrigger.cs.dso
if (!(isObject(BeDemoEndTrigger)))
{
	%template = new BehaviorTemplate(Name : BeDemoEndTrigger);
	%template.friendlyName = "DemoEndTrigger";
	%template.behaviorType = "MetaGameMechanisms";
	%template.description = "used in the demoEndLevel";
	%template.addBehaviorField(goTo, "what should we do when player jumps in", enum, "playAgain", "playAgain	mainMenu	website	video	exit");
}
function BeDemoEndTrigger::onBehaviorAdd(%this)
{
	%owner = Owner;
	if (%owner.getClassName() != "t2dTrigger")
	{
		debugWarn("owner of demoendtrigger behavior is not a trigger!");
		%owner.removeBehavior(%this);
		return;
	}
	%owner.setCollisionCircleSuperscribed("0");
	%owner.setCollisionCircleScale("1");
	%triggerBehavior = %owner.addDependentBehavior("BeTrigger");
	subscribeToEvent(%this, "onLevelLoadFinished1");
	return;
}
function BeDemoEndTrigger::onLevelLoadFinished1(%this)
{
	if ($distributorName $= "BigFishGames")
	{
		if (goTo $= "website")
		{
			website_sentence.setFrame("11");
			website_sentence.setSize("33.9", "10.078");
			break;
		}
		if (goTo $= "playAgain")
		{
			playAgain_sentence.setFrame("12");
			playAgain_sentence.setSize("63.984", "11.016");
		}
	}
	return;
}
function BeDemoEndTrigger::onEnter(%this)
{
	%owner = Owner;
	player.getBehavior("BeShrinker").onShrinkFinished = "callback";
	subscribeToEvents(%this, "onShrinkFinished");
	return;
}
function BeDemoEndTrigger::onLeave(%this)
{
	%owner = Owner;
	if (!(isOuterSpace))
	{
		unSubscribeFromEvents(%this, "onShrinkFinished");
		player.getBehavior("BeShrinker").onShrinkFinished = "respawn";
	}
	else
	{
		debugEcho("leaving demoend but player is dying in rippededge" SPC %this);
	}
	return;
}
function BeDemoEndTrigger::onShrinkFinished(%this)
{
	if (goTo $= "playAgain")
	{
		demoLevelSwitchPoint.getBehavior("BeLevelSwitchPoint").nextLevelName = "level_gameMenu";
		menu_main.competeAfterDemo = "1";
		debugEcho("we should replay the level" SPC competeAfterDemo);
		demoLevelSwitchPoint.startLevelSwitch();
	}
	else
	{
		if (goTo $= "mainMenu")
		{
			debugEcho("we should go to the mainmenu" SPC %this);
			demoLevelSwitchPoint.getBehavior("BeLevelSwitchPoint").nextLevelName = "level_gameMenu";
			demoLevelSwitchPoint.startLevelSwitch();
			break;
		}
		if (goTo $= "website")
		{
			debugEcho("we should go to the website" SPC %this);
			menu_main.buyFullVersion();
			player.getBehavior("BeShrinker").onShrinkFinished = "respawn";
			player.getBehavior("BeShrinker").onShrinkFinished();
			unSubscribeFromEvents(%this, "onShrinkFinished");
			break;
		}
		if (goTo $= "video")
		{
			debugEcho("show the video now!" SPC %this);
			demoLevelSwitchPoint.getBehavior("BeLevelSwitchPoint").nextLevelName = "level_video";
			demoLevelSwitchPoint.getBehavior("BeLevelSwitchPoint").dontMountPlayer = "1";
			demoLevelSwitchPoint.prepareLevelSwitch();
			break;
		}
		if (goTo $= "exit")
		{
			debugEcho("quitting the game!" SPC %this);
			returnToWiiMenu();
		}
	}
	return;
}
