// misc.cs.dso
function setUseShader(%useIt)
{
	setTorqueUseShader(%useIt);
	triggerEvent("onChangeUseShader");
	return;
}
function equalSentences(%s1, %s2)
{
	if (getWordCount(%s1) != getWordCount(%s2))
	{
		return "0";
	}
	%i = 0;
	while (%i < getWordCount(%s1))
	{
		%word = getWord(%s1, %i);
		%compare = getWord(%s2, %i);
		if (%word != %compare)
		{
			return "0";
		}
		%i = %i + 1.0;
	}
	return "1";
	return "1";
}
function findField(%sentence, %field)
{
	return getFieldIndex(%sentence, %field) > -1.0;
	return getFieldIndex(%sentence, %field) > -1.0;
}
function getFieldIndex(%sentence, %searchField)
{
	%i = 0;
	while (%i < getFieldCount(%sentence))
	{
		%field = getField(%sentence, %i);
		if (equalSentences(%field, %searchField))
		{
			return %i;
		}
		else
		{
		}
		%i = %i + 1.0;
	}
	return -1.0;
	return -1.0;
}
function getWordIndex(%sentence, %searchWord)
{
	%i = 0;
	while (%i < getWordCount(%sentence))
	{
		%word = getWord(%sentence, %i);
		if (equalSentences(%word, %searchWord))
		{
			return %i;
		}
		else
		{
		}
		%i = %i + 1.0;
	}
	return -1.0;
	return -1.0;
}
function containsAnyWord(%sentence, %wordlist)
{
	%i = 0;
	while (%i < getWordCount(%wordlist))
	{
		if (findWord(%sentence, getWord(%wordlist, %i)))
		{
			return "1";
		}
		%i = %i + 1.0;
	}
	return "0";
	return "0";
}
function getAverageFps()
{
	%total = getFrameCount() - 20.0 / thisTime - first20Time;
	%last15sec = thisFrameCount["15"] - lastFrameCount["15"] / 15.0;
	%last1sec = thisFrameCount["1"] - lastFrameCount["1"];
	debugEcho(['"total ("', 'thisTime', '" s):"'] SPC %total SPC "last 15 seconds:" SPC %last15sec SPC "last second:" SPC %last1sec);
	return;
}
function isNumberNoZero(%number)
{
	return 0.0 + %number != 0.0;
	return 0.0 + %number != 0.0;
}
function isGroup(%obj)
{
	if (!(isObject(%obj)))
	{
		return "0";
	}
	%class = %obj.getClassName();
	return %class $= "SimSet" || %class $= "SimGroup" || %class $= "t2dSceneObjectGroup" || %class $= "t2dSceneObjectSet";
	return %class $= "SimSet" || %class $= "SimGroup" || %class $= "t2dSceneObjectGroup" || %class $= "t2dSceneObjectSet";
}
function excludeBits(%bits)
{
	%excludedList = " " @ $MASK_ALL_LIST @ " ";
	%i = 0;
	while (%i < getWordCount(%bits))
	{
		%excludedList = strreplace(%excludedList, " " @ getWord(%bits, %i) @ " ", " ");
		%i = %i + 1.0;
	}
	return trim(%excludedList);
	return trim(%excludedList);
}
function getObjectsWithBehavior(%behaviorName)
{
	%allObjects = daSceneGraph.getSceneObjectList();
	%i = 0;
	while (%i < getWordCount(%allObjects))
	{
		%obj = getWord(%allObjects, %i);
		if (%obj.getBehavior(%behaviorName))
		{
			%foundObjects = %foundObjects SPC %obj;
		}
		%i = %i + 1.0;
	}
	return ltrim(%foundObjects);
	return ltrim(%foundObjects);
}
function getObjectsWithClass(%className)
{
	%allObjects = daSceneGraph.getSceneObjectList();
	%i = 0;
	while (%i < getWordCount(%allObjects))
	{
		%obj = getWord(%allObjects, %i);
		if (class $= %className)
		{
			%foundObjects = %foundObjects SPC %obj;
		}
		%i = %i + 1.0;
	}
	return ltrim(%foundObjects);
	return ltrim(%foundObjects);
}
function quoteString(%string)
{
	return ['"""', '%string', '"""'];
	return ['"""', '%string', '"""'];
}
function t2dSceneObject::listBehaviors(%this)
{
	%behaviorCount = %this.getBehaviorCount();
	%i = 0;
	while (%i < %behaviorCount)
	{
		debugEcho(template.getName());
		%i = %i + 1.0;
	}
	return;
}
function toggleProfiler(%fileName)
{
	if (!($profilerEnabled))
	{
		debugEcho("-----------------Starting profile session----------------------------");
		profilerReset();
		profilerEnable("1");
	}
	else
	{
		debugEcho("-----------------Ending profile session----------------------------");
		profilerDump();
		if (%fileName $= "")
		{
			%fileName = "profilerDump.log";
		}
		profilerDumpToFile(%fileName);
		profilerEnable("0");
	}
	$profilerEnabled = !($profilerEnabled);
	return;
}
function getLastToken(%word, %delimiter)
{
	while ("" != %word)
	{
		%word = NextToken(%word, "theToken", %delimiter);
	}
	return %theToken;
	return %theToken;
}
function convertToList(%simSet)
{
	%list = "";
	%i = 0;
	while (%i < %simSet.getCount())
	{
		%list = %list SPC %simSet.getObject(%i);
		%i = %i + 1.0;
	}
	return trim(%list);
	return trim(%list);
}
function setPresentationMode(%mode)
{
	if (%mode $= "1")
	{
		daSceneGraph.setDebugOn("5");
		%i = 0;
		while (%i < daSceneGraph.getSceneObjectCount())
		{
			%obj = daSceneGraph.getSceneObject(%i);
			if (!(%obj.getBehavior(BeCollide)) && %obj.getVisible() && !(isPlayer(%obj)))
			{
				%obj.setPresentationVisible("0");
			}
			%i = %i + 1.0;
		}
	}
	else
	{
		if (%mode $= "2")
		{
			daSceneGraph.setDebugOn("5");
			%i = 0;
			while (%i < daSceneGraph.getSceneObjectCount())
			{
				%obj = daSceneGraph.getSceneObject(%i);
				if (presentationWasVisible)
				{
					%obj.setPresentationVisible("1");
				}
				if (%obj.getClassName() $= "t2dShapeVector" || %obj.getLayer() != $LAYER["main"] && !(isPlayer(%obj)) && !(%obj.getBehavior(BeCollide)))
				{
					%obj.setPresentationVisible("0");
				}
				else
				{
					if (%obj.getBehavior(BeMask))
					{
						%obj.setRenderTexture("0");
					}
				}
				%i = %i + 1.0;
			}
			triggerEvent("onHideParalaxLayers");
			break;
		}
		if (%mode $= "3")
		{
			daSceneGraph.setDebugOff("5");
			%i = 0;
			while (%i < daSceneGraph.getSceneObjectCount())
			{
				%obj = daSceneGraph.getSceneObject(%i);
				if (presentationWasVisible)
				{
					%obj.setPresentationVisible("1");
				}
				if (%obj.getBehavior(BeMask))
				{
					%obj.setRenderTexture("1");
				}
				%i = %i + 1.0;
			}
			triggerEvent("onHideParalaxLayers");
			break;
		}
		if (%mode $= "4")
		{
			daSceneGraph.setDebugOff("5");
			%i = 0;
			while (%i < daSceneGraph.getSceneObjectCount())
			{
				%obj = daSceneGraph.getSceneObject(%i);
				if (presentationWasVisible)
				{
					%obj.setPresentationVisible("1");
				}
				if (%obj.getBehavior(BeMask))
				{
					%obj.setRenderTexture("1");
				}
				%i = %i + 1.0;
			}
			triggerEvent("onShowParalaxLayers");
		}
	}
	return;
}
function t2dSceneObject::setPresentationVisible(%this, %visible)
{
	if (%this.getVisible() && !(%visible))
	{
		%this.presentationWasVisible = "1";
	}
	%this.setVisible(%visible);
	return;
}
if (!(isObject(BeSpidernetDeko)))
{
	%template = new BehaviorTemplate(Name : BeSpidernetDeko);
}
function screenShotMode(%on)
{
	if (%on $= "")
	{
		%on = 1;
	}
	$disableZoomTrigger = %on;
	$makeAllSpawnpointsInvisible = %on;
	$disableSwitches = %on;
	return;
}
