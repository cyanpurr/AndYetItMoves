// properties.cs.dso
function _saveGameConfigurationData(%projectFile)
{
	%xml = new ScriptObject(Name : "")
	{
		class = "XML";
	}
	if (%xml.beginWrite(%projectFile))
	{
		%xml.writeClassBegin("TorqueGameConfiguration");
		%xml.writeField("Company", $Game::CompanyName);
		%xml.writeField("GameName", $Game::ProductName);
		%xml.writeField("Resolution", $Game::Resolution);
		%xml.writeField("FullScreen", $Game::FullScreen);
		%xml.writeField("CommonVer", $Game::CommonVersion);
		%xml.writeField("ConsoleKey", $Game::ConsoleBind);
		%xml.writeField("ScreenShotKey", $Game::ScreenshotBind);
		%xml.writeField("FullscreenKey", $Game::FullscreenBind);
		%xml.writeField("UsesNetwork", $Game::UsesNetwork);
		%xml.writeField("UsesAudio", $Game::UsesAudio);
		%xml.writeField("DefaultScene", $Game::DefaultScene);
		%xml.writeClassEnd();
		%xml.endWrite();
	}
	else
	{
		error("saveGameConfigurationData - Failed to write to file: " @ %projectFile);
		return "0";
	}
	%xml.delete();
	return "1";
	return "1";
}
function _loadGameConfigurationData(%projectFile)
{
	%xml = new ScriptObject(Name : "")
	{
		class = "XML";
	}
	if (%xml.beginRead(%projectFile))
	{
		if (%xml.readClassBegin("TorqueGameConfiguration"))
		{
			$Game::CompanyName = %xml.readField("Company");
			$Game::ProductName = %xml.readField("GameName");
			$Game::Resolution = %xml.readField("Resolution");
			$Game::FullScreen = %xml.readField("FullScreen");
			$Game::CommonVersion = %xml.readField("CommonVer");
			$Game::ConsoleBind = %xml.readField("ConsoleKey");
			$Game::ScreenshotBind = %xml.readField("ScreenShotKey");
			$Game::FullscreenBind = %xml.readField("FullscreenKey");
			$Game::UsesNetwork = %xml.readField("UsesNetwork");
			$Game::UsesAudio = %xml.readField("UsesAudio");
			$Game::DefaultScene = %xml.readField("DefaultScene");
			%xml.readClassEnd();
		}
		else
		{
			_defaultGameConfiguration();
		}
		%xml.endRead();
	}
	else
	{
		_defaultGameConfigurationData();
	}
	%xml.delete();
	setCompanyAndProduct($Game::CompanyName, $Game::ProductName);
	return;
}
function _defaultGameConfigurationData()
{
	$Game::CompanyName = "Independent";
	$Game::ProductName = "Untitled Game";
	$Game::Resolution = "800 600 32";
	$Game::FullScreen = "false";
	$Game::ConsoleBind = "ctrl tilde";
	$Game::ScreenshotBind = "ctrl p";
	$Game::FullscreenBind = "alt enter";
	$Game::UsesNetwork = 0;
	$Game::UsesAudio = 1;
	$Game::DefaultScene = "game/data/levels/untitled.t2d";
	return;
}
