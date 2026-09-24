// finalLevelStuff.cs.dso
function finalLevelBreakOutTrigger::onEnter(%this, )
{
	if (!(brokeOut))
	{
		%i = 1;
		while (%i < 6.0)
		{
			%stone = stone @ %i;
			%stone.getBehavior(BeBreakingPlatform).breakOutOnlyInView = "0";
			%stone.quarry();
			%i = %i + 1.0;
		}
		%this.brokeOut = "1";
	}
	return;
}
