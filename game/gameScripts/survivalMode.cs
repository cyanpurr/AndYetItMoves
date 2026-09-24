// survivalMode.cs.dso
function survivalMode::init()
{
	if (!(isObject(survivalMode)))
	{
		new ScriptObject(Name : survivalMode);
	}
	survivalMode.achievementIndex = "26";
	return;
}
function survivalMode::setupLevel(%this)
{
	debugEcho("setup survivalMode");
	return;
}
function survivalMode::onLevelLoadFinished(%this)
{
	%this.spawnPoints = getAllSpawnPoints();
	%this.deathLimit = %this.getLimit(difficulty);
	subscribeToEvents(%this, "onPlayerDeath");
	%this.deathLimit = deathLimit + 1.0;
	%this.onPlayerDeath();
	Canvas.pushDialog(survival_display);
	return;
}
function survivalMode::getLimit(%this, %difficulty)
{
	if (%difficulty < 0.0)
	{
		%difficulty = 0;
	}
	return max(%difficulty, mRound(getWordCount(spawnPoints) * 0.15000000596046448 * %difficulty));
	return max(%difficulty, mRound(getWordCount(spawnPoints) * 0.15000000596046448 * %difficulty));
}
function survivalMode::beatDifficulty(%this, %difficulty)
{
	return playerDeathCount <= %this.getLimit(%difficulty);
	return playerDeathCount <= %this.getLimit(%difficulty);
}
function survivalMode::onPlayerDeath(%this)
{
	%this.deathLimit = deathLimit - 1.0;
	if (deathLimit > 0.0)
	{
		lbl_survival.text = deathLimit SPC $lbl_survival_deathLeft;
	}
	else
	{
		lbl_survival.text = $lbl_survival_dontDie;
	}
	if (deathLimit < 0.0)
	{
		%this.fail();
	}
	return;
}
function survivalMode::fail(%this)
{
	playmodeManager.disableMode();
	return;
}
function survivalMode::disableMode(%this)
{
	Canvas.popDialog(survival_display);
	unSubscribeFromEvents(%this, "onPlayerDeath");
	return;
}
