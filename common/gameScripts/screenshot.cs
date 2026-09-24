// screenshot.cs.dso
function formatImageNumber(%number)
{
	if (%number < 10.0)
	{
		%number = "0" @ %number;
	}
	if (%number < 100.0)
	{
		%number = "0" @ %number;
	}
	if (%number < 1000.0)
	{
		%number = "0" @ %number;
	}
	if (%number < 10000.0)
	{
		%number = "0" @ %number;
	}
	return %number;
	return %number;
}
function formatSessionNumber(%number)
{
	if (%number < 10.0)
	{
		%number = "0" @ %number;
	}
	if (%number < 100.0)
	{
		%number = "0" @ %number;
	}
	return %number;
	return %number;
}
function recordMovie(%movieName, %fps)
{
	$timeAdvance = 1000.0 / %fps;
	$screenGrabThread = schedule($timeAdvance, "0", movieGrabScreen, %fileName, "0");
	return;
}
function movieGrabScreen(%movieName, %frameNumber)
{
	screenShot(%movieName @ formatImageNumber(%frameNumber) @ ".png", "PNG");
	$screenGrabThread = schedule($timeAdvance, "0", movieGrabScreen, %movieName, %frameNumber + 1.0);
	return;
}
function stopMovie()
{
	$timeAdvance = 0;
	cancel($screenGrabThread);
	return;
}
$screenshotNumber = 0;
function doScreenShot(%val)
{
	if (%val || %val $= "")
	{
		if ($pref::Video::screenShotSession $= "")
		{
			$pref::Video::screenShotSession = 0;
		}
		if ($screenshotNumber == 0.0)
		{
			$pref::Video::screenShotSession = $pref::Video::screenShotSession + 1.0;
		}
		if ($pref::Video::screenShotSession > 999.0)
		{
			$pref::Video::screenShotSession = 1;
		}
		%name = expandFilename("game/data/screenshots/" @ formatSessionNumber($pref::Video::screenShotSession) @ "-" @ formatImageNumber($screenshotNumber));
		$screenshotNumber = $screenshotNumber + 1.0;
		if ($pref::Video::screenShotFormat $= "JPEG" || $pref::Video::screenShotFormat $= "JPG")
		{
			screenShot(%name @ ".jpg", "JPEG");
			break;
		}
		if ($pref::Video::screenShotFormat $= "PNG")
		{
			screenShot(%name @ ".png", "PNG");
			break;
		}
		screenShot(%name @ ".png", "PNG");
	}
	return;
}
