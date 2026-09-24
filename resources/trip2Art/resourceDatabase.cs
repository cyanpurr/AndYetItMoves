// resourceDatabase.cs.dso
$instantResource = new ScriptObject(Name : "")
{
	class = "trip2Art";
	Name = "trip2Art";
	User = "TGB";
	LoadFunction = "trip2Art::LoadResource";
	UnloadFunction = "trip2Art::UnloadResource";
}
function trip2Art::LoadResource(%this)
{
	if ($LevelEditorActive)
	{
		GuiFormManager::BroadcastContentMessage("LevelBuilderSidebarCreate", "0", "refresh");
	}
	return;
}
function trip2Art::UnloadResource(%this)
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
		imageName = "./../jungleArt/tiles/paper_green.jpg";
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
	new t2dImageMapDatablock(Name : paper_greenFullImageMap)
	{
		imageName = "./../jungleArt/tiles/paper_green.jpg";
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
	new t2dImageMapDatablock(Name : paper_blueFullImageMap)
	{
		imageName = "./../tripArt/tiles/paper_blue.jpg";
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
		cellWidth = "256";
		cellHeight = "256";
		preload = "1";
		allowUnload = "0";
	}
	new t2dImageMapDatablock(Name : bark_tiled1ImageMap)
	{
		imageName = "./../jungleArt/tiles/bark_tiled1.jpg";
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
		imageName = "./../jungleArt/tiles/bark_tiled3.jpg";
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
	new t2dImageMapDatablock(Name : jungleFloorImageMap)
	{
		imageName = "./../jungleArt/tiles/jungleFloor.jpg";
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
		imageName = "./../jungleArt/textures/treeRoof.jpg";
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
	new t2dImageMapDatablock(Name : bloomsImageMap)
	{
		imageName = "./../jungle3Art/images/blooms.jpg";
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
	new t2dImageMapDatablock(Name : fadeonrotationtriangleImageMap)
	{
		imageName = "./images/fade-on-rotation-triangle.png";
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
	new t2dImageMapDatablock(Name : specialObject_1ImageMap)
	{
		imageName = "./images/specialObject_1.jpg";
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
	new t2dImageMapDatablock(Name : fadeOnRotationImageMap)
	{
		imageName = "./images/fadeOnRotation.jpg";
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
	Data.add(new AudioProfile(Name : BranchBreak), new AudioProfile(Name : BranchMedium2), new AudioProfile(Name : CounterRotate1), new AudioProfile(Name : FadeOnRotation1), new AudioProfile(Name : TripBeat1_1), new AudioProfile(Name : TripBeat1_2), new AudioProfile(Name : TripBeat1_3));
	Data.add(new AudioProfile(Name : TripBeat2_1), new AudioProfile(Name : TripBeat2_2), new AudioProfile(Name : TripBeat2_3), new ScriptObject(Name : TripBeat1Profiles), new ScriptObject(Name : TripBeat2Profiles));
	if ($ambientEnabled)
	{
		Data.add(new AudioProfile(Name : TripPerc1), new AudioProfile(Name : TripPerc2), new AudioProfile(Name : TripBirds1), new AudioProfile(Name : TripBirds2), new AudioProfile(Name : TripVeryCrazyBirds1), new AudioProfile(Name : TripVeryCrazyBirds2));
	}
}
