// defaultPrefs.cs.dso
if ($WII)
{
	$pref::Torque::skipSplashes = 0;
}
$Pref::Net::LagThreshold = 400;
$pref::Net::Port = 28000;
$pref::Input::LinkMouseSensitivity = 1;
$pref::Input::MouseEnabled = 0;
$pref::Input::JoystickEnabled = 0;
$pref::Input::KeyboardTurnSpeed = 0.10000000149011612;
$pref::Audio::driver = "OpenAL";
$pref::Audio::forceMaxDistanceUpdate = 0;
$pref::Audio::environmentEnabled = 0;
$pref::Audio::masterVolume = 1.0;
$pref::Audio::channelVolume1 = 1.0;
$pref::Audio::channelVolume2 = 1.0;
$pref::Audio::channelVolume3 = 1.0;
$pref::Audio::channelVolume4 = 1.0;
$pref::Audio::channelVolume5 = 1.0;
$pref::Audio::channelVolume6 = 1.0;
$pref::Audio::channelVolume7 = 1.0;
$pref::Audio::channelVolume8 = 1.0;
$pref::T2D::dualCollisionCallbacks = 1;
$pref::T2D::imageMapDumpTextures = 0;
$pref::T2D::imageMapEchoErrors = 1;
$pref::T2D::imageMapFixedMaxTextureError = 1;
$pref::T2D::imageMapFixedMaxTextureSize = 0;
$pref::T2D::imageMapShowPacking = 0;
$pref::T2D::imageMapPreloadDefault = 1;
$pref::T2D::imageMapAllowUnloadDefault = 0;
$pref::T2D::particleEngineQuantityScale = 1.0;
$pref::T2D::renderContactChange = 0.5;
$pref::T2D::renderContactMax = 16;
$pref::T2D::warnFileDeprecated = 1;
$pref::T2D::warnSceneOccupancy = 1;
$pref::Server::Name = "TGB Server";
$pref::Player::Name = "TGB Player";
$pref::Server::port = 28000;
$pref::Server::MaxPlayers = 32;
$pref::Server::RegionMask = 2;
$pref::Net::RegionMask = 2;
$pref::Master0 = "2:master.garagegames.com:28002";
$pref::ts::detailAdjust = 0.44999998807907104;
$pref::Video::appliedPref = 0;
$pref::Video::disableVerticalSync = 1;
$pref::Video::monitorNum = 0;
$pref::Video::screenShotFormat = "PNG";
$pref::OpenGL::gammaCorrection = 0.5;
$pref::OpenGL::force16BitTexture = "0";
$pref::OpenGL::forcePalettedTexture = "0";
$pref::OpenGL::maxHardwareLights = 3;
$pref::VisibleDistanceMod = 1.0;
initDisplayDeviceInfo();
$pref::Video::displayDevice = "OpenGL";
$pref::Video::preferOpenGL = 1;
$pref::Video::allowOpenGL = 1;
$pref::Video::allowD3D = 1;
$pref::Video::fullScreen = 0;
if ($PCI_VEN $= "VEN_8086")
{
	$pref::Video::displayDevice = "D3D";
	$pref::Video::allowOpenGL = 0;
	if ($PCI_DEV $= "DEV_1132" || $PCI_DEV $= "DEV_7125")
	{
		$pref::Video::fullScreen = "1";
	}
}
else
{
	if ($PCI_VEN $= "VEN_1039")
	{
		$pref::Video::allowOpenGL = 0;
		$pref::Video::displayDevice = "D3D";
		break;
	}
	if ($PCI_VEN $= "VEN_1106")
	{
		$pref::Video::allowOpenGL = 0;
		$pref::Video::displayDevice = "D3D";
		break;
	}
	if ($PCI_VEN $= "VEN_5333")
	{
		$pref::Video::allowOpenGL = 0;
		$pref::Video::displayDevice = "D3D";
		break;
	}
	if ($PCI_VEN $= "VEN_1002")
	{
		$pref::Video::displayDevice = "OpenGL";
		if ($PCI_DEV $= "DEV_5446")
		{
			$pref::Video::displayDevice = "D3D";
			$pref::Video::allowOpenGL = 0;
		}
		break;
	}
	if ($PCI_VEN $= "VEN_10DE")
	{
		$pref::Video::displayDevice = "OpenGL";
	}
}
echo("
Using " @ $pref::Video::displayDevice @ " rendering. Fullscreen: " @ $pref::Video::fullScreen @ "
");
