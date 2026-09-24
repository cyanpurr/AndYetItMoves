// resourceDatabase.cs.dso
$instantResource = new ScriptObject(Name : "")
{
	class = "trip6Art";
	Name = "trip6Art";
	User = "TGB";
	LoadFunction = "trip6Art::LoadResource";
	UnloadFunction = "trip6Art::UnloadResource";
}
function trip6Art::LoadResource(%this)
{
	if ($LevelEditorActive)
	{
		GuiFormManager::BroadcastContentMessage("LevelBuilderSidebarCreate", "0", "refresh");
	}
	return;
}
function trip6Art::UnloadResource(%this)
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
		imageName = "./../bonusLevelsArt/tiles/paper_white.jpg";
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
	new t2dImageMapDatablock(Name : davidSpiralImageMap)
	{
		imageName = "./../trip3Art/images/davidSpiral.png";
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
	new t2dImageMapDatablock(Name : stoneSpiralImageMap)
	{
		imageName = "./../trip4Art/images/stoneSpiral.png";
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
	new t2dImageMapDatablock(Name : cube2dImageMap)
	{
		imageName = "./images/cube2d.jpg";
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
	Data.add(new AudioProfile(Name : TripStoneNew1Part1), new AudioProfile(Name : TripStoneNew1Part2), new AudioProfile(Name : TripStoneNew1Part3), new AudioProfile(Name : TripStoneNew1Part4), new AudioProfile(Name : Chord1), new AudioProfile(Name : Chord2));
	Data.add(new AudioProfile(Name : Chord3), new AudioProfile(Name : Chord4), new AudioProfile(Name : Failed), new AudioProfile(Name : TripScannerFat1), new AudioProfile(Name : TripScannerFat2), new AudioProfile(Name : TripLastScanner1), new AudioProfile(Name : TripLastScanner2), new AudioProfile(Name : StoneShake), new AudioProfile(Name : StoneBreakMedium));
	Data.add(new AudioProfile(Name : TripBeat5_1), new AudioProfile(Name : TripBeat5_2), new AudioProfile(Name : TripBeat5_3), new AudioProfile(Name : TripBeat6_1), new AudioProfile(Name : TripBeat6_2), new ScriptObject(Name : TripBeat5Profiles), new ScriptObject(Name : TripBeat6Profiles));
}
