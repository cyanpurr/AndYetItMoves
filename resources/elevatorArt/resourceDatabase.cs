// resourceDatabase.cs.dso
$instantResource = new ScriptObject(Name : "")
{
	class = "elevatorArt";
	Name = "elevatorArt";
	User = "TGB";
	LoadFunction = "elevatorArt::LoadResource";
	UnloadFunction = "elevatorArt::UnloadResource";
}
function elevatorArt::LoadResource(%this)
{
	if ($LevelEditorActive)
	{
		GuiFormManager::BroadcastContentMessage("LevelBuilderSidebarCreate", "0", "refresh");
	}
	return;
}
function elevatorArt::UnloadResource(%this)
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
	Data.add(new AudioProfile(Name : StoneSmallNew1), new AudioProfile(Name : StoneSmallNew2));
}
