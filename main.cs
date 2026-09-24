
//---------------------------------------------------------------------------------------------
// Torque Game Builder
// Copyright (C) GarageGames.com, Inc.
//---------------------------------------------------------------------------------------------
$WII = ( getOs() $= "wii" ); //true;//
if ($WII)
	$resourceFolderName = "wiisources";
else
	$resourceFolderName = "resources";

$firstStartupDone = false;		//this will be set to true if the first startup process is done


/// Player Initialization Procedure
/// 
function onStart()
{

}

function onExit()
{
}

//---------------------------------------------------------------------------------------------
// Load the paths we need access to
//---------------------------------------------------------------------------------------------<
function loadPath( %path )
{
   setModPaths( getModPaths() @ ";" @ %path );
   exec(%path @ "/main.cs");

}

//---------------------------------------------
// Do some bootstrap voodoo to get the game to 
// the initializeProject phase of loading and 
// pass off to the user
//---------------------------------------------

loadPath( "common" );

loadPath( "game" );

parseArgs();

onStart();

//AYIM begin: moved that here, to be able to log into the path loaded from common/main.cs
// Output a console log
setLogMode(6);
//AYIM end

// Initialized
echo("\nTorque Game Builder (" @ getT2DVersion() @ ") initialized...");

if( !isFunction( "initializeProject" ) || !isFunction( "_initializeProject" ) )
{
   messageBox( "Game Startup Error", "'initializeProject' function could not be found." @
               "\nThis could indicate a bad or corrupt common directory for your game." @
               "\n\nThe Game will now shutdown because it cannot properly function", "Ok", "MIStop" );
   quit();
}

_initializeProject();

// Startup the project
initializeProject();


