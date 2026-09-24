// resourceDatabase.cs.dso
$instantResource = new ScriptObject(Name : "")
{
	class = "jungleArt";
	Name = "jungleArt";
	User = "TGB";
	LoadFunction = "jungleArt::LoadResource";
	UnloadFunction = "jungleArt::UnloadResource";
}
function jungleArt::LoadResource(%this)
{
	if ($LevelEditorActive)
	{
		GuiFormManager::BroadcastContentMessage("LevelBuilderSidebarCreate", "0", "refresh");
	}
	return;
}
function jungleArt::UnloadResource(%this)
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
	new t2dImageMapDatablock(Name : paper_greenImageMap)
	{
		imageName = "./tiles/paper_green.jpg";
		imageMode = "CELL";
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
		cellWidth = "256";
		cellHeight = "256";
		preload = "1";
		allowUnload = "0";
	}
	new t2dImageMapDatablock(Name : bark_tiled1ImageMap)
	{
		imageName = "./tiles/bark_tiled1.jpg";
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
	new t2dImageMapDatablock(Name : bark_tiled2ImageMap)
	{
		imageName = "./tiles/bark_tiled2.jpg";
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
	new t2dImageMapDatablock(Name : bark_tiled3ImageMap)
	{
		imageName = "./tiles/bark_tiled3.jpg";
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
	new t2dImageMapDatablock(Name : swingsImageMap)
	{
		imageName = "./objects/swings.png";
		imageMode = "CELL";
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
		cellWidth = "256";
		cellHeight = "128";
		preload = "1";
		allowUnload = "0";
	}
	new t2dImageMapDatablock(Name : jungleFloorImageMap)
	{
		imageName = "./tiles/jungleFloor.jpg";
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
	new t2dImageMapDatablock(Name : treeRoofImageMap)
	{
		imageName = "./textures/treeRoof.jpg";
		imageMode = "CELL";
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
		cellWidth = "512";
		cellHeight = "512";
		preload = "1";
		allowUnload = "0";
	}
	new t2dImageMapDatablock(Name : bushesImageMap1)
	{
		imageName = "./textures/bushes.jpg";
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
		cellHeight = "254";
		preload = "1";
		allowUnload = "0";
	}
	new t2dImageMapDatablock(Name : plantLeavesImageMap)
	{
		imageName = "./textures/plantLeaves.jpg";
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
		cellHeight = "254";
		preload = "1";
		allowUnload = "0";
	}
	new t2dImageMapDatablock(Name : bushesWideImageMap)
	{
		imageName = "./textures/bushesWide.jpg";
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
		cellCountX = "1";
		cellCountY = "2";
		cellWidth = "510";
		cellHeight = "254";
		preload = "1";
		allowUnload = "0";
	}
	new t2dImageMapDatablock(Name : bushesWide2ImageMap)
	{
		imageName = "./textures/bushesWide2.jpg";
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
		cellCountX = "1";
		cellCountY = "2";
		cellWidth = "510";
		cellHeight = "254";
		preload = "1";
		allowUnload = "0";
	}
	new t2dImageMapDatablock(Name : lianeImageMap)
	{
		imageName = "./objects/liane.png";
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
	new t2dImageMapDatablock(Name : bambooImageMap)
	{
		imageName = "./objects/bamboo.png";
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
		imageName = "./paperAndMasks/branches_small.png";
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
		imageName = "./paperAndMasks/branches_small_edges.png";
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
	new t2dImageMapDatablock(Name : thin_trunkImageMap)
	{
		imageName = "./tiles/thin_trunk.jpg";
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
}
if ($soundEnabled)
{
	Data.add(new AudioProfile(Name : Footsteps), new AudioProfile(Name : PlayerJump), new AudioProfile(Name : JungleTrampSnapBack), new AudioProfile(Name : JungleTrampBend), new AudioProfile(Name : BranchBreak), new AudioProfile(Name : BranchMedium2), new AudioProfile(Name : Swing));
	if ($ambientEnabled)
	{
		Data.add(new AudioProfile(Name : JungleMelody1), new AudioProfile(Name : JungleMelody2), new AudioProfile(Name : JungleHigher1), new AudioProfile(Name : JungleHigher2), new AudioProfile(Name : JungleBeat1), new AudioProfile(Name : JungleBeat2), new AudioProfile(Name : JungleBeat3), new AudioProfile(Name : JungleBeat4));
		Data.add(new AudioProfile(Name : JungleTenseAmbientBase), new AudioProfile(Name : JungleTenseAmbientMelody), new AudioProfile(Name : JungleWind), new AudioProfile(Name : JungleBat), new AudioProfile(Name : JungleBird), new AudioProfile(Name : JungleSalamander), new AudioProfile(Name : JungleAnimal), new AudioProfile(Name : JungleMonkeyFX1), new AudioProfile(Name : JungleMonkeyFX2), new AudioProfile(Name : JungleMonkeyFX3));
	}
}
