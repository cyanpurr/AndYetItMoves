// reactOnSwitchesActive.cs.dso
if (!(isObject(BeReactOnSwitchesActive)))
{
	%template = new BehaviorTemplate(Name : BeReactOnSwitchesActive);
	%template.friendlyName = "ReactOnSwitchesActive";
	%template.behaviorType = "LevelTrip";
	%template.description = "this object will react on activated switches and will be deleted (or it can do other stuff later)";
	%template.addBehaviorField(fadeIn, "if the plattform shall be faded in; if false it will be displayed from the begin", bool, "1");
	%template.addBehaviorField(fadeOut, "if the plattform shall be faded out; if false it will be stay diplayed at the end", bool, "1");
}
function BeReactOnSwitchesActive::onBehaviorAdd(%this)
{
	subscribeToEvents(%this, "onLevelLoadFinished30 onSwitchesActive onSwitchesFinish");
	return;
}
function BeReactOnSwitchesActive::onLevelLoadFinished30(%this)
{
	if (fadeIn)
	{
		Owner.setEnabled("0");
	}
	return;
}
function BeReactOnSwitchesActive::onSwitchesActive(%this)
{
	%owner = Owner;
	if (fadeIn)
	{
		%owner.setEnabled("1");
	}
	return;
}
function BeReactOnSwitchesActive::onSwitchesFinish(%this)
{
	%owner = Owner;
	if (fadeOut)
	{
		%owner.setEnabled("0");
	}
	return;
}
