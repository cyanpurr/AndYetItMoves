// resourceDatabase.cs.dso
$instantResource = new ScriptObject(Name : "")
{
	class = "jungle2Art";
	Name = "jungle2Art";
	User = "TGB";
	LoadFunction = "jungle2Art::LoadResource";
	UnloadFunction = "jungle2Art::UnloadResource";
}
function jungle2Art::LoadResource(%this)
{
	if ($LevelEditorActive)
	{
		GuiFormManager::BroadcastContentMessage("LevelBuilderSidebarCreate", "0", "refresh");
	}
	return;
}
function jungle2Art::UnloadResource(%this)
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
	new t2dImageMapDatablock(Name : trunk1ImageMap)
	{
		imageName = "./../jungle1Art/images/trunk1.png";
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
	new t2dImageMapDatablock(Name : trunk2ImageMap)
	{
		imageName = "./../jungle1Art/images/trunk2.png";
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
	new t2dImageMapDatablock(Name : trunk3ImageMap)
	{
		imageName = "./../jungle1Art/images/trunk3.png";
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
	new t2dImageMapDatablock(Name : leavesImageMap)
	{
		imageName = "./../jungle6Art/images/leaves.jpg";
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
		filterPad = "1";
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
		filterPad = "1";
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
		filterPad = "1";
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
	new t2dImageMapDatablock(Name : bug_bodyImageMap)
	{
		imageName = "./images/bug_body.png";
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
	new t2dImageMapDatablock(Name : bug_kleinImageMap)
	{
		imageName = "./images/bug_klein.jpg";
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
		cellHeight = "64";
		preload = "1";
		allowUnload = "0";
	}
	new t2dAnimationDatablock(Name : bug_kleinAnimation)
	{
		imageMap = "bug_kleinImageMap";
		animationFrames = "0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15";
		animationTime = "0.533333";
		animationCycle = "1";
		randomStart = "0";
		startFrame = "0";
	}
	new t2dImageMapDatablock(Name : dionaea_head_1ImageMap)
	{
		imageName = "./images/dionaea_head_1.jpg";
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
	new t2dAnimationDatablock(Name : dionaea_head_1Animation)
	{
		imageMap = "dionaea_head_1ImageMap";
		animationFrames = "0 1 2 3 4 5 6 7 6 5 4 3 2 1 0";
		animationTime = "1";
		animationCycle = "1";
		randomStart = "0";
		startFrame = "0";
	}
	new t2dImageMapDatablock(Name : stipeImageMap)
	{
		imageName = "./../jungle3Art/images/stipe.jpg";
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
	Data.add(new AudioProfile(Name : Bug), new AudioProfile(Name : DigestBug), new AudioProfile(Name : Munching), new AudioProfile(Name : SpitOut));
}
