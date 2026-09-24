// resourceDatabase.cs.dso
$instantResource = new ScriptObject(Name : "")
{
	class = "finalLevelArt";
	Name = "finalLevelArt";
	User = "TGB";
	LoadFunction = "finalLevelArt::LoadResource";
	UnloadFunction = "finalLevelArt::UnloadResource";
}
function finalLevelArt::LoadResource(%this)
{
	if ($LevelEditorActive)
	{
		GuiFormManager::BroadcastContentMessage("LevelBuilderSidebarCreate", "0", "refresh");
	}
	return;
}
function finalLevelArt::UnloadResource(%this)
{
	if (isObject(Data) && Data.getCount() > 0.0)
	{
		while (Data.getCount() > 0.0)
		{
			%datablockObj = Data.getObject("0");
			Data.remove(%datablockObj);
			if (isObject(%datablockObj))
			{
				%datablockObj.delete();
			}
		}
	}
	if ($LevelEditorActive)
	{
		GuiFormManager::BroadcastContentMessage("LevelBuilderSidebarCreate", "0", "refresh");
	}
	return;
}
$instantResource.Data = new SimGroup(Name : "")
{
	canSaveDynamicFields = "1";
}
if ($soundEnabled)
{
	Data.add(new AudioProfile(Name : Swing));
	if ($ambientEnabled)
	{
		Data.add(new AudioProfile(Name : TripAmbientBeat5), new AudioProfile(Name : TripArp1), new AudioProfile(Name : TripArp2), new AudioProfile(Name : TripArp3), new AudioProfile(Name : TripArp4));
	}
}
