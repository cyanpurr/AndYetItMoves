// resourceDatabase.cs.dso
$instantResource = new ScriptObject(Name : "")
{
	class = "cave4Art";
	Name = "cave4Art";
	User = "TGB";
	LoadFunction = "cave4Art::LoadResource";
	UnloadFunction = "cave4Art::UnloadResource";
}
function cave4Art::LoadResource(%this)
{
	if ($LevelEditorActive)
	{
		GuiFormManager::BroadcastContentMessage("LevelBuilderSidebarCreate", "0", "refresh");
	}
	return;
}
function cave4Art::UnloadResource(%this)
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
	new t2dImageMapDatablock(Name : purgi_stalagmiteImageMap)
	{
		imageName = "./images/purgi_stalagmite.jpg";
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
	new t2dImageMapDatablock(Name : wall_tiled2ImageMap)
	{
		imageName = "./../cave1Art/images/wall_tiled2.jpg";
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
	new t2dImageMapDatablock(Name : wallbackground_tiled2ImageMap)
	{
		imageName = "./../cave2Art/images/wallbackground_tiled2.jpg";
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
		cellCountX = "2";
		cellCountY = "2";
		cellWidth = "255";
		cellHeight = "255";
		preload = "1";
		allowUnload = "0";
	}
	new t2dImageMapDatablock(Name : batInPaperImageMap)
	{
		imageName = "./images/batInPaper.png";
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
	new t2dImageMapDatablock(Name : saurianImageMap)
	{
		imageName = "./images/saurian.jpg";
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
	new t2dImageMapDatablock(Name : sauzungeImageMap)
	{
		imageName = "./images/sauzunge.png";
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
	new t2dImageMapDatablock(Name : rootImageMap)
	{
		imageName = "./images/root.png";
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
	new t2dImageMapDatablock(Name : waterDropImageMap)
	{
		imageName = "./images/waterDrop.png";
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
	new t2dImageMapDatablock(Name : rootsInPaper_1_animImageMap)
	{
		imageName = "./images/rootsInPaper_1_anim.png";
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
	new t2dAnimationDatablock(Name : rootsInPaper_1_animAnimation)
	{
		imageMap = "rootsInPaper_1_animImageMap";
		animationFrames = "0 1 2 3 4 5 6 7";
		animationTime = "1.14286";
		animationCycle = "0";
		randomStart = "0";
		startFrame = "0";
	}
	new t2dImageMapDatablock(Name : rootsInPaper_2u3_animImageMap)
	{
		imageName = "./images/rootsInPaper_2u3_anim.png";
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
	new t2dAnimationDatablock(Name : rootsInPaper_2_animAnimation)
	{
		imageMap = "rootsInPaper_2u3_animImageMap";
		animationFrames = "0 1 2 3 4 5 6 7";
		animationTime = "1";
		animationCycle = "0";
		randomStart = "0";
		startFrame = "0";
	}
	new t2dAnimationDatablock(Name : rootsInPaper_3_animAnimation)
	{
		imageMap = "rootsInPaper_2u3_animImageMap";
		animationFrames = "8 9 10 11 12 13 14 15";
		animationTime = "1";
		animationCycle = "0";
		randomStart = "0";
		startFrame = "0";
	}
	new t2dImageMapDatablock(Name : rootsInPaper_4u5_animImageMap)
	{
		imageName = "./images/rootsInPaper_4u5_anim.png";
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
	new t2dAnimationDatablock(Name : rootsInPaper_4_animAnimation)
	{
		imageMap = "rootsInPaper_4u5_animImageMap";
		animationFrames = "0 1 2 3";
		animationTime = "0.6";
		animationCycle = "0";
		randomStart = "0";
		startFrame = "0";
	}
	new t2dAnimationDatablock(Name : rootsInPaper_5_animAnimation)
	{
		imageMap = "rootsInPaper_4u5_animImageMap";
		animationFrames = "4 5 6 7";
		animationTime = "0.6";
		animationCycle = "0";
		randomStart = "0";
		startFrame = "0";
	}
	new t2dImageMapDatablock(Name : rootsInPaper_6_animImageMap)
	{
		imageName = "./images/rootsInPaper_6_anim.png";
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
	new t2dAnimationDatablock(Name : rootsInPaper_6_animAnimation)
	{
		imageMap = "rootsInPaper_6_animImageMap";
		animationFrames = "0 1 2 3";
		animationTime = "0.6";
		animationCycle = "0";
		randomStart = "0";
		startFrame = "0";
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
	new t2dImageMapDatablock(Name : bushesWideImageMap)
	{
		imageName = "./../jungleArt/textures/bushesWide.jpg";
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
	new t2dImageMapDatablock(Name : paper_jungleTransitionImageMap)
	{
		imageName = "./images/paper_jungleTransition.png";
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
	new t2dImageMapDatablock(Name : earthImageMap)
	{
		imageName = "./../jungle3Art/images/earth.jpg";
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
	Data.add(new AudioProfile(Name : GrowingRoots), new AudioProfile(Name : BatsFlying), new AudioProfile(Name : OneBatFlying), new AudioProfile(Name : OneBatCirculating), new AudioProfile(Name : MoreBatsCirculating), new AudioProfile(Name : SaurianSlurp), new AudioProfile(Name : SaurianDefeated), new AudioProfile(Name : WaterDrop2), new AudioProfile(Name : WaterDrop3), new AudioProfile(Name : WaterDrop4), new AudioProfile(Name : StoneShake), new AudioProfile(Name : StoneMediumNew1), new AudioProfile(Name : StoneMediumNew2), new AudioProfile(Name : StoneBreakSmall), new AudioProfile(Name : StoneBreakMedium), new AudioProfile(Name : StoneBreakBig), new AudioProfile(Name : GrowingRootsQuake));
}
