// main.cs.dso
function onStart()
{
	Parent::onStart();
	echo(" % - Initializing Common");
	exec("./preferences/defaultPrefs.cs");
	exec("./gameScripts/xml.cs");
	exec("./gameScripts/properties.cs");
	_defaultGameConfigurationData();
	_loadGameConfigurationData(expandFilename("./commonConfig.xml"));
	exec("./gameScripts/common.cs");
	initializeCommon();
	return;
}
function onExit()
{
	_saveGameConfigurationData(expandFilename("./commonConfig.xml"));
	_shutdownCommon();
	Parent::onExit();
	return;
}
function loadKeybindings()
{
	$keybindCount = 0;
	if (isFunction("setupKeybinds"))
	{
		setupKeybinds();
	}
	return;
}
function displayHelp()
{
	Parent::displayHelp();
	error("Common Mod options:
" @ "  -fullscreen            Starts game in full screen mode
" @ "  -windowed              Starts game in windowed mode
" @ "  -autoVideo             Auto detect video, but prefers OpenGL
" @ "  -openGL                Force OpenGL acceleration
" @ "  -directX               Force DirectX acceleration
" @ "  -voodoo2               Force Voodoo2 acceleration
" @ "  -prefs <configFile>    Exec the config file
");
	return;
}
function parseArgs()
{
	Parent::parseArgs();
	%i = 1;
	while (%i < $Game::argc)
	{
		%arg = $Game::argv[%i];
		%nextArg = $Game::argv[%i + 1.0];
		%hasNextArg = $Game::argc - %i > 1.0;
		if (%arg $= "-compileAllScripts")
		{
			loadPath("resources");
			$hashingDir = ".";
			compileAllScripts();
			quit();
		}
		else
		{
			if (%arg $= "-fullscreen")
			{
				$pref::Video::fullScreen = 1;
				$argUsed[%i] = $argUsed[%i] + 1.0;
				break;
			}
			if (%arg $= "-windowed")
			{
				$pref::Video::fullScreen = 0;
				$argUsed[%i] = $argUsed[%i] + 1.0;
				break;
			}
			if (%arg $= "-openGL")
			{
				$pref::Video::displayDevice = "OpenGL";
				$argUsed[%i] = $argUsed[%i] + 1.0;
				break;
			}
			if (%arg $= "-directX")
			{
				$pref::Video::displayDevice = "D3D";
				$argUsed[%i] = $argUsed[%i] + 1.0;
				break;
			}
			if (%arg $= "-voodoo2")
			{
				$pref::Video::displayDevice = "Voodoo2";
				$argUsed[%i] = $argUsed[%i] + 1.0;
				break;
			}
			if (%arg $= "-autoVideo")
			{
				$pref::Video::displayDevice = "";
				$argUsed[%i] = $argUsed[%i] + 1.0;
				break;
			}
			if (%arg $= "-prefs")
			{
				$argUsed[%i] = $argUsed[%i] + 1.0;
				if (%hasNextArg)
				{
					exec(%nextArg, "1", "1");
					$argUsed[%i + 1.0] = $argUsed[%i + 1.0] + 1.0;
					%i = %i + 1.0;
				}
				else
				{
					error("Error: Missing Command Line argument. Usage: -prefs <path/script.cs>");
				}
				break;
			}
			if (%arg $= "-jSave")
			{
				$argUsed[%i] = $argUsed[%i] + 1.0;
				if (%hasNextArg)
				{
					echo("Saving event log to journal: " @ %nextArg);
					saveJournal(%nextArg);
					$argUsed[%i + 1.0] = $argUsed[%i + 1.0] + 1.0;
					%i = %i + 1.0;
				}
				else
				{
					error("Error: Missing Command Line argument. Usage: -jSave <journal_name>");
				}
				break;
			}
			if (%arg $= "-jPlay")
			{
				$argUsed[%i] = $argUsed[%i] + 1.0;
				if (%hasNextArg)
				{
					playJournal(%nextArg, "0");
					$argUsed[%i + 1.0] = $argUsed[%i + 1.0] + 1.0;
					%i = %i + 1.0;
				}
				else
				{
					error("Error: Missing Command Line argument. Usage: -jPlay <journal_name>");
				}
				break;
			}
			if (%arg $= "-jDebug")
			{
				$argUsed[%i] = $argUsed[%i] + 1.0;
				if (%hasNextArg)
				{
					playJournal(%nextArg, "1");
					$argUsed[%i + 1.0] = $argUsed[%i + 1.0] + 1.0;
					%i = %i + 1.0;
					break;
				}
				error("Error: Missing Command Line argument. Usage: -jDebug <journal_name>");
			}
		}
		%i = %i + 1.0;
	}
	return;
}
function compileAllScripts()
{
	setScriptHashing("1", "1");
	%spec = "*.cs";
	%file = findFirstFile(%spec);
	while (%file != "")
	{
		compile(%file);
		%file = findNextFile(%spec);
	}
	%spec = "*.gui";
	%file = findFirstFile(%spec);
	while (%file != "")
	{
		compile(%file);
		%file = findNextFile(%spec);
	}
	%spec = "*.t2d";
	%file = findFirstFile(%spec);
	while (%file != "")
	{
		compile(%file);
		%file = findNextFile(%spec);
	}
	setScriptHashing("0");
	createHashControlFile();
	return;
}
activatePackage(CommonPackage);
