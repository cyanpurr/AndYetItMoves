// resourceDatabase.cs.dso
$instantResource = new ScriptObject(Name : "")
{
	class = "bonusLevelsArt";
	Name = "bonusLevelsArt";
	User = "TGB";
	LoadFunction = "bonusLevelsArt::LoadResource";
	UnloadFunction = "bonusLevelsArt::UnloadResource";
}
function bonusLevelsArt::LoadResource(%this)
{
	if ($LevelEditorActive)
	{
		GuiFormManager::BroadcastContentMessage("LevelBuilderSidebarCreate", "0", "refresh");
	}
	return;
}
function bonusLevelsArt::UnloadResource(%this)
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
	new t2dImageMapDatablock(Name : paper_white_fullImageMap)
	{
		imageName = "./tiles/paper_white.jpg";
		imageMode = "FULL";
		frameCount = "-1";
		filterMode = "SMOOTH";
		filterPad = "0";
		preferPerf = "1";
		cellRowOrder = "1";
		cellOffsetX = "0";
		cellOffsetY = "0";
		cellStrideX = "0";
		cellStrideY = "0";
		cellCountX = "-1";
		cellCountY = "-1";
		cellWidth = "0";
		cellHeight = "0";
		preload = "1";
		allowUnload = "0";
	}
	new t2dImageMapDatablock(Name : paper_black_fullImageMap)
	{
		imageName = "./tiles/paper_black.jpg";
		imageMode = "FULL";
		frameCount = "-1";
		filterMode = "SMOOTH";
		filterPad = "0";
		preferPerf = "1";
		cellRowOrder = "1";
		cellOffsetX = "0";
		cellOffsetY = "0";
		cellStrideX = "0";
		cellStrideY = "0";
		cellCountX = "-1";
		cellCountY = "-1";
		cellWidth = "0";
		cellHeight = "0";
		preload = "1";
		allowUnload = "0";
	}
	new t2dImageMapDatablock(Name : branches_smallImageMap)
	{
		imageName = "./../jungleArt/paperAndMasks/branches_small.png";
		imageMode = "CELL";
		frameCount = "-1";
		filterMode = "SMOOTH";
		filterPad = "1";
		preferPerf = "1";
		cellRowOrder = "1";
		cellOffsetX = "0";
		cellOffsetY = "0";
		cellStrideX = "0";
		cellStrideY = "0";
		cellCountX = "-1";
		cellCountY = "-1";
		cellWidth = "254";
		cellHeight = "62";
		preload = "1";
		allowUnload = "0";
	}
	new t2dImageMapDatablock(Name : branches_small_edgesImageMap)
	{
		imageName = "./../jungleArt/paperAndMasks/branches_small_edges.png";
		imageMode = "CELL";
		frameCount = "-1";
		filterMode = "SMOOTH";
		filterPad = "1";
		preferPerf = "1";
		cellRowOrder = "1";
		cellOffsetX = "0";
		cellOffsetY = "0";
		cellStrideX = "0";
		cellStrideY = "0";
		cellCountX = "-1";
		cellCountY = "-1";
		cellWidth = "254";
		cellHeight = "62";
		preload = "1";
		allowUnload = "0";
	}
	new t2dImageMapDatablock(Name : rippedEdgeHoleImageMap)
	{
		imageName = "./../tripArt/paperAndMasks/rippedEdgeHole.png";
		imageMode = "FULL";
		frameCount = "-1";
		filterMode = "SMOOTH";
		filterPad = "1";
		preferPerf = "1";
		cellRowOrder = "1";
		cellOffsetX = "0";
		cellOffsetY = "0";
		cellStrideX = "0";
		cellStrideY = "0";
		cellCountX = "-1";
		cellCountY = "-1";
		cellWidth = "0";
		cellHeight = "0";
		preload = "1";
		allowUnload = "0";
	}
}
if ($soundEnabled)
{
	Data.add(new AudioProfile(Name : Footsteps), new AudioProfile(Name : PlayerJump), new AudioProfile(Name : StoneBreaking2), new AudioProfile(Name : StoneShake), new AudioProfile(Name : StoneBigNew2), new AudioProfile(Name : StoneBreakBig), new AudioProfile(Name : BranchBreak), new AudioProfile(Name : JungleTrampSnapBack), new AudioProfile(Name : JungleTrampBend), new AudioProfile(Name : TripAlterEgoActivate), new AudioProfile(Name : TripAlterEgoDeactivate));
}
