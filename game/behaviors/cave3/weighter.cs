// weighter.cs.dso
if (!(isObject(BeWeighter)))
{
	if (!(isTorquePlayer()))
	{
		%template = new BehaviorTemplate(Name : BeWeighter);
	}
	else
	{
		%template = new BeWeighterTemplate(Name : BeWeighter);
	}
	%template.friendlyName = "Weighter";
	%template.behaviorType = "LevelCave3";
	%template.description = "influence targets with owners weight together with other weighters with same targets";
	%template.addBehaviorField(targetGroups, "list of names of groups which should be weightened by the owner", string, "");
}
function BeWeighter::onBehaviorAdd(%this)
{
	subscribeToEvent(%this, "onLevelLoadFinished15");
	return;
}
function BeWeighter::onLevelLoadFinished15(%this, )
{
	%i = 0;
	while (%i < getWordCount(targetGroups))
	{
		if (isGroup(getWord(targetGroups, %i)))
		{
			getWord(targetGroups, %i).add(Owner);
		}
		%i = %i + 1.0;
	}
	return getWordCount(targetGroups);
}
