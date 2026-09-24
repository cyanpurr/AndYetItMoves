// keybindings.cs.dso
$keybindCount = 0;
function ActionMap::bind(%this, %device, %action, %callback, %description)
{
	if (%description $= "")
	{
	}
	else
	{
	}
	$keybindName[$keybindCount] = %description;
	$keybindCommand[$keybindCount] = %callback;
	$keybindDefaultKey[$keybindCount] = %action;
	$keybindMap[$keybindCount] = %this;
	$keybindCount = $keybindCount + 1.0;
	Parent::bind(%this, %device, %action, %callback);
	return;
}
function initializeKeybindOptions()
{
	KeysTextList.clear();
	%i = 0;
	while (%i < $keybindCount)
	{
		%currentBinding = $keybindMap[%i].getBinding($keybindCommand[%i]);
		%action = getWord(%currentBinding, "1");
		if (strstr(%action, "ctrl") >= 0.0 || strstr(%action, "alt") >= 0.0 || strstr(%action, "shift") >= 0.0 || strstr(%action, "cmd") >= 0.0 || strstr(%action, "opt") >= 0.0)
		{
			%action = %action SPC getWord(%currentBinding, "2");
		}
		KeysTextList.addRow(%i, $keybindName[%i] TAB getFriendlyKeyName(%action));
		%i = %i + 1.0;
	}
	return;
}
function isModifierKey(%k)
{
	if (%k $= "lctrl" || %k $= "rctrl" || %k $= "lshift" || %k $= "rshift")
	{
		return "0";
	}
	if (strstr(%k, "ctrl") >= 0.0 || strstr(%k, "alt") >= 0.0 || strstr(%k, "shift") >= 0.0 || strstr(%k, "cmd") >= 0.0 || strstr(%k, "opt") >= 0.0)
	{
		return "1";
	}
	return "0";
	return "0";
}
function RemapInput::unbindAllActionsForCommand(%this, %map, %device, %cmd)
{
	%oldAction = %map.getBinding(%cmd);
	if (%oldAction != "")
	{
		%cnt = getWordCount(%oldAction);
		%i = 1;
		while (%i < %cnt)
		{
			%a = getWord(%oldAction, %i);
			if (isModifierKey(%a))
			{
				%i = %i + 1.0;
				if (%i < %cnt)
				{
					%a = %a SPC getWord(%oldAction, %i);
				}
			}
			%map.unbind(%device, %a);
			%i = %i + 1.0;
		}
	}
	return;
}
function RemapInput::onInputEvent(%this, %device, %action)
{
	if (%device $= "" || %action $= "")
	{
		return;
	}
	Canvas.popDialog(RemapGui);
	%cmd = $keybindCommand[index];
	%name = $keybindName[index];
	%map = $keybindMap[index];
	%prevMap = %map.getCommand(%device, %action);
	if (%prevMap != %cmd)
	{
		if (%prevMap $= "")
		{
			%this.unbindAllActionsForCommand(%map, %device, %cmd);
			%map.bind(%device, %action, %cmd);
			break;
		}
		%prevMapIndex = findCommandIndex(%prevMap);
		if (%prevMapIndex != -1.0)
		{
			MessageBoxOKCancel("", getFriendlyKeyName(%action) @ " is already bound to " @ $keybindName[%prevMapIndex] @ ". Changing the bind " @ "will switch the keys for the two actions. Proceed?", ['"forceUpdateBind("', 'index', '", "', '%prevMapIndex', '", ""', '%device'] SPC %action SPC "", " SPC """ SPC %map.getBinding(%cmd) SPC "");");
			break;
		}
		MessageBoxOK("", "Cannot bind key " @ getFriendlyName(%prevMap) @ ". It is reserved.");
	}
	initializeKeybindOptions();
	return;
}
function revertControlOptions()
{
	%i = 0;
	while (%i < $keybindCount)
	{
		if (isObject($keybindMap[%i]))
		{
			$keybindMap[%i].delete();
		}
		%i = %i + 1.0;
	}
	$keybindCount = 0;
	activatePackage(KeybindPackage);
	loadKeybindings();
	deactivatePackage(KeybindPackage);
	initializeKeybindOptions();
	return;
}
function changeBinding()
{
	%index = KeysTextList.getSelectedId();
	if (%index < 0.0)
	{
		return;
	}
	RemapInput.index = %index;
	Canvas.pushDialog(RemapGui);
	RemapText.setText("Enter key for " @ $keybindName[%index]);
	return;
}
function forceUpdateBind(%index, %prevIndex, %action, %prevAction)
{
	%newAction = getWord(%action, "1");
	%oldAction = getWord(%prevAction, "1");
	if (isModifierKey(%newAction))
	{
		%newAction = %newAction SPC getWord(%action, "2");
	}
	if (isModifierKey(%oldAction))
	{
		%oldAction = %oldAction SPC getWord(%prevAction, "2");
	}
	$keybindMap[%index].bind(getWord(%action, "0"), %newAction, $keybindCommand[%index]);
	$keybindMap[%index].bind(getWord(%prevAction, "0"), %oldAction, $keybindCommand[%prevIndex]);
	initializeKeybindOptions();
	return;
}
function findCommandIndex(%command)
{
	%i = 0;
	while (%i < $keybindCount)
	{
		if ($keybindCommand[%i] $= %command)
		{
			return %i;
		}
		%i = %i + 1.0;
	}
	return -1.0;
	return -1.0;
}
function getFriendlyKeyName(%action)
{
	%name = strlwr(%action);
	%len = strlen(%action);
	if (%len == 1.0)
	{
		%name = strupr(%action);
	}
	%mark = strpos(%action, " ");
	if (%mark >= 0.0 && %len - %mark == 2.0)
	{
		%chunk1 = getSubStr(%action, "0", %mark);
		%chunk2 = getSubStr(%action, %mark, %len);
		%name = %chunk1 @ strupr(%chunk2);
	}
	%f = getSubStr(%action, "0", "1");
	if (%f $= "F" && %len == 2.0 || %len == 3.0)
	{
		%name = strupr(%action);
	}
	else
	{
		if (%action $= "BUTTON0")
		{
			%name = "Left Mouse";
			break;
		}
		if (%action $= "BUTTON1")
		{
			%name = "Right Mouse";
			break;
		}
		if (%action $= "BUTTON2")
		{
			%name = "Middle Mouse";
			break;
		}
		if (%action $= "XAXIS")
		{
			%name = "Mouse X";
			break;
		}
		if (%action $= "YAXIS")
		{
			%name = "Mouse Y";
			break;
		}
		if (%action $= "ZAXIS")
		{
			%name = "Scroll Wheel";
		}
	}
	return %name;
	return %name;
}
