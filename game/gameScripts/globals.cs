// globals.cs.dso
$COMPLETE_LEVELLIST = "level_gameMenu level_cave1 level_cave2 level_cave3 level_cave4 level_jungle1 level_jungle2 level_jungle3 level_jungle4 level_jungle5 level_jungle6 level_trip1 level_trip2 level_trip3 level_trip4 level_trip5 level_trip6 level_credits level_finalLevel level_demoEnd level_presentation";
$LAYER_ENUM = "last	background_paper	background_3	background_2	background_1	main_background	mainBehindPlayer	player	moving	main	mainBeforePlayer	main_foreground	foreground_1	foreground_2	foreground_3	rippedEdge	collide	custom";
$PARALAXLAYER_ENUM = "background_paper	background_3	background_2	background_1	foreground_1	foreground_2	foreground_3";
$GROUPS_ENUM = "player	rippedEdge	collide	viewWindow	moving	trigger	soundTrigger	partOfPlayer	zoomTrigger	soundObject	playerTrigger	viewWindowTrigger	playerFallOutPoly	dontCollide	bat	drop	dropCollisionTrigger	stalagtite	root	jungleMonkey	jungleCoconut	jungleCoconutActive	jungleSwing	jungleSwing2	jungleSwingPlatform	jungleMonsterHorns	jungleBanana	flameable	flame";
$SMOOTHING_FUNCTIONS = "SMOOTH	SINC	LONGSINC	INV_LONGSINC	SOFTSINC	INV_SOFTSINC	LIN	DOWNSINC";
$OLLF_NUMBERS = "0 1 2 2_5 3 4 4_3 4_5 5 7 9 10 12 15 16 20 30 40";
$NUM_OLLFS = getWordCount($OLLF_NUMBERS);
if (getShaderSupport())
{
	$performanceLevels = "0 1 0" TAB "0 1 1" TAB "1 1 1";
}
else
{
	$performanceLevels = "0 1 0" TAB "0 1 1" TAB "0 1 1";
}
%i = 0;
while (%i < getFieldCount($LAYER_ENUM))
{
	$LAYER_NUM[getField($LAYER_ENUM, %i)] = %i;
	$LAYER_BLENDING[getField($LAYER_ENUM, %i)] = 1;
	%i = %i + 1.0;
}
$LAYER["last"] = 31;
$LAYER_BLENDING["last"] = 1.0;
$LAYER["background_paper"] = 29;
$LAYER_BLENDING["background_paper"] = 1.0;
$LAYER_SPEEDFACTOR["background_paper"] = 0.3499999940395355;
$LAYER["background_static_2"] = 28;
$LAYER_BLENDING["background_static_2"] = 0.20000000298023224;
$LAYER_SPEEDFACTOR["background_static_2"] = 0.0;
$LAYER["background_static_1"] = 27;
$LAYER_BLENDING["background_static_1"] = 0.20000000298023224;
$LAYER_SPEEDFACTOR["background_static_1"] = 0.0;
$LAYER["background_3"] = 26;
$LAYER_BLENDING["background_3"] = 0.30000001192092896;
$LAYER_SPEEDFACTOR["background_3"] = 0.30000001192092896;
$LAYER["background_2"] = 23;
$LAYER_BLENDING["background_2"] = 0.4000000059604645;
$LAYER_SPEEDFACTOR["background_2"] = 0.20000000298023224;
$LAYER["background_1"] = 20;
$LAYER_BLENDING["background_1"] = 0.5;
$LAYER_SPEEDFACTOR["background_1"] = 0.10000000149011612;
$LAYER["main_background"] = 17;
$LAYER_BLENDING["main_background"] = 0.6000000238418579;
$LAYER["mainBehindPlayer"] = 15;
$LAYER_BLENDING["mainBehindPlayer"] = 1.0;
$LAYER["player"] = 14;
$LAYER_BLENDING["player"] = 1.0;
$LAYER["moving"] = 13;
$LAYER_BLENDING["moving"] = 1.0;
$LAYER["main"] = 12;
$LAYER_BLENDING["main"] = 1.0;
$LAYER["mainBeforePlayer"] = 11;
$LAYER_BLENDING["mainBeforePlayer"] = 1.0;
$LAYER["main_foreground"] = 10;
$LAYER_BLENDING["main_foreground"] = 0.6000000238418579;
$LAYER["foreground_1"] = 8;
$LAYER_BLENDING["foreground_1"] = 0.5;
$LAYER_SPEEDFACTOR["foreground_1"] = -0.10000000149011612;
$LAYER["foreground_2"] = 6;
$LAYER_BLENDING["foreground_2"] = 0.4000000059604645;
$LAYER_SPEEDFACTOR["foreground_2"] = -0.22499999403953552;
$LAYER["foreground_3"] = 4;
$LAYER_BLENDING["foreground_3"] = 0.30000001192092896;
$LAYER_SPEEDFACTOR["foreground_3"] = -0.3499999940395355;
$LAYER["rippedEdge"] = 2;
$LAYER_BLENDING["rippedEdge"] = 1.0;
$LAYER["collide"] = 1;
$LAYER_BLENDING["collide"] = 1.0;
$GROUPS["player"] = 1;
$GROUPS["rippedEdge"] = 2;
$GROUPS["collide"] = 3;
$GROUPS["viewWindow"] = 4;
$GROUPS["moving"] = 5;
$GROUPS["trigger"] = 6;
$GROUPS["soundTrigger"] = 6;
$GROUPS["partOfPlayer"] = 7;
$GROUPS["zoomTrigger"] = 8;
$GROUPS["soundObject"] = 9;
$GROUPS["rippedEdgeTrigger"] = 10;
$GROUPS["playerTrigger"] = 11;
$GROUPS["viewWindowTrigger"] = 12;
$GROUPS["breakingStopper"] = 13;
$GROUPS["dontCollide"] = 31;
$GROUPS["bat"] = 20;
$GROUPS["drop"] = 21;
$GROUPS["dropCollisionTrigger"] = 22;
$GROUPS["growingRoot"] = 24;
$GROUPS["batBlocker"] = 25;
$GROUPS["saurianTrigger"] = 26;
$GROUPS["batTrigger"] = 27;
$GROUPS["jungleSwing"] = 24;
$GROUPS["jungleSwing2"] = 25;
$GROUPS["jungleSwingPlatform"] = 26;
$GROUPS["jungleBug"] = 20;
$GROUPS["jungleBugStopper"] = 21;
$GROUPS["jungleDionaeaHead"] = 22;
$GROUPS["jungleDionaeaSnapTrigger"] = 23;
$GROUPS["jungleMonkey"] = 21;
$GROUPS["jungleCoconut"] = 22;
$GROUPS["jungleCoconutActive"] = 23;
$GROUPS["jungleBanana"] = 28;
$GROUPS["jungleMonsterHorns"] = 27;
$GROUPS["jungleSmallStones"] = 20;
$GROUPS["rockMass"] = 21;
$GROUPS["flintstone"] = 22;
$GROUPS["spark"] = 27;
$GROUPS["flameable"] = 28;
$GROUPS["flame"] = 29;
$GROUPS["dekoCollide"] = 26;
$GROUPS["stoneBlocker"] = 25;
$GROUPS["elevator"] = 20;
$GROUPS["peggleNails"] = 21;
$RHYTHMCOLOR["1"] = "0.6 1 1";
$RHYTHMCOLOR["2"] = "1 0.6 1";
$RHYTHMCOLOR["3"] = "1 1 0.6";
$RHYTHMCOLOR["4"] = "0.7 0.7 1";
$PI = 3.1415927410125732;
$HALF_PI = $PI / 2.0;
$THREE_HALF_PI = $HALF_PI * 3.0;
$MASK_ALL_LIST = 0;
%i = 1;
while (%i <= 31.0)
{
	$MASK_ALL_LIST = $MASK_ALL_LIST SPC %i;
	%i = %i + 1.0;
}
$MASK_ALL = bits($MASK_ALL_LIST);
$CW = 1;
$CCW = -1.0;
$tab = "" TAB "";
$STARTDISTANCE = 300;
$FULLVOLUMEDISTANCE = 40;
$MAINGROUPLIST = "maskObjectGroup mountWithOffsetGroup spawnPathGroup spawnPointGroup soundObjectGroup callNextFrameGroup zoomAdjusterGroup hintsGroup allScriptObjects";
if ($SnowdriftlandSpecial)
{
	$FULLLEVELLIST = "level_gameMenu level_elevator";
}
else
{
	$FULLLEVELLIST = "level_gameMenu level_cave1 level_cave2 level_cave3 level_cave4 level_jungle1 level_jungle2 level_jungle3 level_jungle4 level_jungle5 level_jungle6 level_trip1 level_trip2 level_trip3 level_trip4 level_trip5 level_trip6 level_credits level_finalLevel level_elevator level_labyrinth level_chase";
}
$WIILEVELLIST = $FULLLEVELLIST;
$PREVIEWLEVELLIST = "level_gameMenu level_cave1 level_cave3 level_jungle3 level_jungle4 level_trip2 level_trip3 level_elevator";
if ($WII)
{
	$DEMOLEVELLIST = "level_gameMenu level_cave2 level_jungle2 level_trip2 level_video level_demoEnd";
}
else
{
	$DEMOLEVELLIST = "level_gameMenu level_cave1 level_cave3 level_jungle3 level_demoEnd";
}
$ALL_WII_INPUT_MODES = "driver grabber keyhole classic";
$compassSize = "120 120";
$circlesSize = "140 140";
function initGlobals()
{
	new ScriptGlobals(Name : globals)
	{
		gravity = "140";
		MaxLinearVelocity = "300";
		MaxAngularVelocity = "720";
		cameraMountForce = "5";
		controlsEnabled = "1";
		inGameCursor = defaultCursor;
		noReactionGraphGroups = "6";
		levelList = $LEVELLIST;
		currentLevelObject = "";
		scenegraph = "";
		currentLevelNumber = "1";
		allEnvironment = "0";
		spawnPoints = "0";
		totalAchievements = "18";
	}
	globals.updateLayerData();
	globals.setAchievements(achievements);
	new ScriptObject(Name : standards)
	{
		playerSize = "22.178 44.358";
		spawnPointSize = "23.675 47.350";
		flameGrowTime = "1.5";
	}
	new ScriptObject(Name : settings)
	{
		ghostRunPositionIntervall = 1000.0 / 20.0;
		showHints = "1";
	}
	return;
}
exec("./utilities/TGBtools.cs");
echo("****** globals loaded ******");
