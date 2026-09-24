// resourceDatabase.cs.dso
$instantResource = new ScriptObject(Name : "")
{
	class = "cave3Art";
	Name = "cave3Art";
	User = "TGB";
	LoadFunction = "cave3Art::LoadResource";
	UnloadFunction = "cave3Art::UnloadResource";
}
function cave3Art::LoadResource(%this)
{
	if ($LevelEditorActive)
	{
		GuiFormManager::BroadcastContentMessage("LevelBuilderSidebarCreate", "0", "refresh");
	}
	return;
}
function cave3Art::UnloadResource(%this)
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
$hintPicPath = "./../hintArt/" @ $settings::Personal::Language @ "/hint_slope_death_pic.png";
if (!(isFile($hintPicPath)))
{
	$hintPicPath = "./../hintArt/english//hint_slope_death_pic.png";
}
$instantResource.Data = new SimGroup(Name : "")
{
	new t2dImageMapDatablock(Name : hint_slope_death_picImageMap)
	{
		imageName = $hintPicPath;
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
	new t2dImageMapDatablock(Name : hint_slope_death_1ImageMap)
	{
		imageName = ['"./../hintArt/"', '$settings::Personal::Language', '"/hint_slope_death_1.png"'];
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
	new t2dImageMapDatablock(Name : purgi_stalagmiteImageMap)
	{
		imageName = "./../cave4Art/images/purgi_stalagmite.jpg";
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
	new t2dImageMapDatablock(Name : wallTiled6_otherColor6ImageMap)
	{
		imageName = "./images/wallTiled6_otherColor6.jpg";
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
	new t2dImageMapDatablock(Name : hint_backgroundImageMap)
	{
		imageName = "./../cave1Art/images/hint_background.png";
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
	Data.add(new AudioProfile(Name : StoneBreaking2), new AudioProfile(Name : StoneShake), new AudioProfile(Name : SpidernetRip), new AudioProfile(Name : StoneMediumNew1), new AudioProfile(Name : StoneMediumNew2), new AudioProfile(Name : StoneBigNew2), new AudioProfile(Name : StoneBreakMedium), new AudioProfile(Name : StoneBreakBig));
}
