// limitedRotationMode.cs.dso
function limitedRotationMode::init()
{
	if (!(isObject(limitedRotationMode)))
	{
		new ScriptObject(Name : limitedRotationMode);
	}
	limitedRotationMode.achievementIndex = "25";
	limitedRotationMode.Smoother = Smoother::createInstance();
	Smoother.init("0.75", -75.0, -10.0);
	return;
}
function limitedRotationMode::setupLevel(%this)
{
	debugEcho("setup limitedRotationMode");
	if (rotationLimit <= 0.0)
	{
		debugEcho("WARNING: there's no rotationLimit set for this level:" SPC levelName SPC "cancelling limitedRotationMode");
		%this.hasSetLimits = "0";
		return;
	}
	%this.hasSetLimits = "1";
	%this.rotationLimit = %this.getLimit(difficulty);
	debugEcho("setting up lRM of level" SPC levelName SPC rotationLimit SPC rotationLimit);
	return;
}
function limitedRotationMode::getLimit(%this, %difficulty)
{
	if (%difficulty < 0.0)
	{
		%difficulty = 0;
	}
	return mRound(rotationLimit * 1.0 + 0.25 * %difficulty);
	return mRound(rotationLimit * 1.0 + 0.25 * %difficulty);
}
function limitedRotationMode::onLevelLoadFinished(%this)
{
	subscribeToEvents(%this, "onRotationFinish onPlayerDeath onPlayerReanimate onMenuReplayLevel onSpawnPointActivate");
	%this.rotationCount = "0";
	%this.lastRotationCount = "0";
	%this.rotationsLeft = rotationLimit;
	%this.lastRotationsLeft = rotationLimit;
	%this.lastCameraRotation = -1.0 * $firstSpawnPoint.getRotation();
	Canvas.pushDialog(rotation_display);
	return;
}
function limitedRotationMode::onRotationFinish(%this)
{
	if (justReanimated)
	{
		%this.justReanimated = "0";
		return;
	}
	if (camera.getCurrentRotation() == camera.getLastRotation())
	{
		return camera.getCurrentRotation();
	}
	if (hasSetLimits)
	{
		if (rotationsLeft - 1.0 < 0.0)
		{
			%this.fail();
		}
		else
		{
			%this.rotationCount = rotationCount + 1.0;
			%this.setRotationsLeft(rotationsLeft - 1.0);
		}
	}
	else
	{
		%this.rotationCount = rotationCount + 1.0;
		%this.setRotationsLeft(rotationCount);
	}
	return;
}
function limitedRotationMode::setRotationsLeft(%this, %rotations)
{
	%this.rotationsLeft = %rotations;
	lbl_rotation_left.text = rotationsLeft;
	if (rotationsLeft < 3.0)
	{
		AyimRotationTextProfile.fontColor = "190 10 10";
	}
	else
	{
		AyimRotationTextProfile.fontColor = "10 150 10";
	}
	return;
}
function limitedRotationMode::onSpawnpointActivate(%this)
{
	if (number == 1.0)
	{
		return spawnPoint;
	}
	%this.saveRotations();
	%this.lastCameraRotation = camera.getCurrentRotation();
	return;
}
function limitedRotationMode::saveRotations(%this)
{
	%this.lastRotationsLeft = rotationsLeft;
	%this.lastRotationCount = rotationCount;
	if (showingRotationsSaved)
	{
		cancel(hidingRotationsSavedSchedule);
	}
	Smoother.start();
	subscribeToEvents(%this, "onUpdateFrame");
	%this.showingRotationsSaved = "1";
	lbl_rotationsSaved.text = rotationsLeft SPC $lbl_rotationsSaved;
	return;
}
function limitedRotationMode::onUpdateFrame(%this)
{
	if (!(Smoother.getIsFinished()))
	{
		ctrl_rotationsSaved.setPosition(getX(ctrl_rotationsSaved.getPosition()), Smoother.getValue());
	}
	else
	{
		if (showingRotationsSaved)
		{
			%this.hidingRotationsSavedSchedule = %this.schedule("4000", "hideRotationsSaved");
		}
		unSubscribeFromEvents(%this, "onUpdateFrame");
	}
	return;
}
function limitedRotationMode::hideRotationsSaved(%this)
{
	if (!(Smoother.getIsFinished()) || !(showingRotationsSaved))
	{
		return %this;
	}
	%this.showingRotationsSaved = "0";
	Smoother.start("1");
	subscribeToEvents(%this, "onUpdateFrame");
	return;
}
function limitedRotationMode::onPlayerDeath(%this)
{
	unSubscribeFromEvents(%this, "onRotationFinish");
	return;
}
function limitedRotationMode::onPlayerReanimate(%this)
{
	%this.justReanimated = "1";
	%this.rotationCount = lastRotationCount;
	%this.setRotationsLeft(lastRotationsLeft);
	subscribeToEvents(%this, "onRotationFinish");
	return;
}
function limitedRotationMode::beatDifficulty(%this, %difficulty)
{
	return rotationCount <= %this.getLimit(%difficulty);
	return rotationCount <= %this.getLimit(%difficulty);
}
function limitedRotationMode::fail(%this)
{
	player.dieExploding();
	playmodeManager.disableMode();
	return;
}
function limitedRotationMode::disableMode(%this)
{
	Canvas.popDialog(rotation_display);
	unSubscribeFromEvents(%this, "onUpdateFrame");
	cancel(hidingRotationsSavedSchedule);
	ctrl_rotationsSaved.setPosition(getX(ctrl_rotationsSaved.getPosition()), -75.0);
	debugEcho("disabling limitedRotationMode" SPC %this);
	unSubscribeFromEvents(%this, "onRotationFinish onPlayerDeath onPlayerReanimate onMenuReplayLevel onSpawnPointActivate");
	return;
}
