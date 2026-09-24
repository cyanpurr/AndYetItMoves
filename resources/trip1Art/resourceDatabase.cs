// resourceDatabase.cs.dso
$instantResource = new ScriptObject(Name : "")
{
	class = "trip1Art";
	Name = "trip1Art";
	User = "TGB";
	LoadFunction = "trip1Art::LoadResource";
	UnloadFunction = "trip1Art::UnloadResource";
}
function trip1Art::LoadResource(%this)
{
	if ($LevelEditorActive)
	{
		GuiFormManager::BroadcastContentMessage("LevelBuilderSidebarCreate", "0", "refresh");
	}
	return;
}
function trip1Art::UnloadResource(%this)
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
	new t2dImageMapDatablock(Name : leaves_burnedImageMap)
	{
		imageName = "./../jungle6Art/images/leaves_burned.jpg";
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
	new t2dImageMapDatablock(Name : waterDropImageMap)
	{
		imageName = "./../cave4Art/images/waterDrop.png";
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
	new t2dImageMapDatablock(Name : snakeImageMap)
	{
		imageName = "./images/snake.png";
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
	new t2dImageMapDatablock(Name : barkTiled_stylize_findEdgesImageMap)
	{
		imageName = "./images/barkTiled_stylize_findEdges.jpg";
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
	new t2dImageMapDatablock(Name : barkTiled_stylize_glowingEdgesImageMap)
	{
		imageName = "./images/barkTiled_stylize_glowingEdges.jpg";
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
	new t2dImageMapDatablock(Name : floor_stylize_findEdgesImageMap)
	{
		imageName = "./images/floor_stylize_findEdges.jpg";
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
	new t2dImageMapDatablock(Name : floor_stylize_glowingEdgesImageMap)
	{
		imageName = "./images/floor_stylize_glowingEdges.jpg";
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
	new t2dImageMapDatablock(Name : rootsInPaper_1_animImageMap)
	{
		imageName = "./../cave4Art/images/rootsInPaper_1_anim.png";
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
	new t2dImageMapDatablock(Name : rootsInPaper_2u3_animImageMap)
	{
		imageName = "./../cave4Art/images/rootsInPaper_2u3_anim.png";
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
		cellHeight = "64";
		preload = "1";
		allowUnload = "0";
	}
	new t2dImageMapDatablock(Name : rootsInPaper_4u5_animImageMap)
	{
		imageName = "./../cave4Art/images/rootsInPaper_4u5_anim.png";
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
		cellWidth = "128";
		cellHeight = "64";
		preload = "1";
		allowUnload = "0";
	}
	new t2dImageMapDatablock(Name : rootsInPaper_6_animImageMap)
	{
		imageName = "./../cave4Art/images/rootsInPaper_6_anim.png";
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
		cellWidth = "128";
		cellHeight = "128";
		preload = "1";
		allowUnload = "0";
	}
	new t2dImageMapDatablock(Name : banana_squashedImageMap)
	{
		imageName = "./../jungle1Art/images/banana_squashed.png";
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
		cellWidth = "64";
		cellHeight = "128";
		preload = "1";
		allowUnload = "0";
	}
	new t2dImageMapDatablock(Name : bananaImageMap)
	{
		imageName = "./../jungle1Art/images/banana.png";
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
		cellWidth = "64";
		cellHeight = "128";
		preload = "1";
		allowUnload = "0";
	}
	new t2dImageMapDatablock(Name : batInPaperImageMap)
	{
		imageName = "./../cave4Art/images/batInPaper.png";
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
		cellWidth = "126";
		cellHeight = "126";
		preload = "1";
		allowUnload = "0";
	}
	new t2dAnimationDatablock(Name : batInPaperFlyAnimation)
	{
		imageMap = "batInPaperImageMap";
		animationFrames = "0 1 2 1";
		animationTime = "0.5";
		animationCycle = "1";
		randomStart = "1";
		startFrame = "0";
	}
	new t2dAnimationDatablock(Name : batInPaperSitAnimation)
	{
		imageMap = "batInPaperImageMap";
		animationFrames = "3";
		animationTime = "1";
		animationCycle = "0";
		randomStart = "0";
		startFrame = "0";
	}
	new t2dImageMapDatablock(Name : fruitImageMap)
	{
		imageName = "./images/fruit.jpg";
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
	new t2dImageMapDatablock(Name : fruitsImageMap)
	{
		imageName = "./images/fruits.jpg";
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
	new t2dImageMapDatablock(Name : poisonImageMap)
	{
		imageName = "./images/poison.png";
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
	Data.add(new AudioProfile(Name : BranchBreak), new AudioProfile(Name : BranchMedium2), new AudioProfile(Name : Rain));
	Data.add(new AudioProfile(Name : Footsteps), new AudioProfile(Name : PlayerJump), new AudioProfile(Name : ThunderSingleStrike), new AudioProfile(Name : SnakeBite), new AudioProfile(Name : MoreBatsCirculating));
	if ($ambientEnabled)
	{
		Data.add(new AudioProfile(Name : JungleBat), new AudioProfile(Name : JungleBird), new AudioProfile(Name : JungleSalamander));
		Data.add(new AudioProfile(Name : JungleAnimal), new AudioProfile(Name : TripBirds1), new AudioProfile(Name : TripBirds2));
		Data.add(new AudioProfile(Name : TripThunder1), new AudioProfile(Name : TripThunder2), new AudioProfile(Name : TripThunder3), new AudioProfile(Name : TripThunder4), new AudioProfile(Name : TripTenseAmbientBase), new AudioProfile(Name : TripTenseAmbientMelody));
	}
}
