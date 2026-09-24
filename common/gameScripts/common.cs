// common.cs.dso
$Game::CommonVersion = 114;
function initializeCommon()
{
	GlobalActionMap.bind(keyboard, $Game::ConsoleBind, toggleConsole);
	GlobalActionMap.bind(keyboard, $Game::ScreenshotBind, doScreenShot);
	GlobalActionMap.bindCmd(keyboard, $Game::FullscreenBind, "toggleFullScreen();", "");
	exec("./audio.cs");
	exec("./canvas.cs");
	exec("./cursor.cs");
	setRandomSeed();
	if ($Game::UsesNetwork)
	{
		setNetPort("0");
	}
	initializeCanvas($Game::ProductName);
	if ($Game::UsesAudio)
	{
		initializeOpenAL();
	}
	exec("~/gui/profiles.cs");
	exec("~/gui/cursors.cs");
	exec("~/gui/console.gui");
	if ($WII)
	{
		exec("~/gui/MessageBoxCustomDlg.gui");
		exec("~/gui/messagePopup.gui");
	}
	if ($WII)
	{
		exec("~/gui/StartupGui.gui");
		exec("~/gui/splashWristStrap.gui");
	}
	exec("~/gui/messageBox.cs");
	exec("~/gui/help.cs");
	exec("./screenshot.cs");
	exec("./metrics.cs");
	exec("./scriptDoc.cs");
	exec("./keybindings.cs");
	exec("./options.cs");
	exec("./levelManagement.cs");
	exec("./projectManagement.cs");
	exec("./projectResources.cs");
	exec("./align.cs");
	if ($Game::UsesNetwork)
	{
		initBaseClient();
		initBaseServer();
	}
	if ($WII)
	{
		Canvas.setCursor("0", ayimCursor);
		Canvas.setCursor("1", defaultCursor);
	}
	else
	{
		Canvas.setCursor(defaultCursor);
	}
	loadKeybindings();
	$commonInitialized = 1;
	return;
}
function _shutdownCommon()
{
	if (isFunction("shutdownProject"))
	{
		shutdownProject();
	}
	shutdownOpenAL();
	return;
}
function dumpKeybindings()
{
	%i = 0;
	while (%i < $keybindCount)
	{
		if (isObject($keybindMap[%i]))
		{
			if (%i == 0.0)
			{
			}
			else
			{
			}
			$keybindMap[%i].save("~/prefs/bind.cs", "1");
			$keybindMap[%i].delete();
		}
		%i = %i + 1.0;
	}
	return;
}
function initBaseClient()
{
	exec("./client/client.cs");
	exec("./client/message.cs");
	exec("./client/serverConnection.cs");
	exec("./client/chatClient.cs");
	return;
}
function initBaseServer()
{
	exec("./server/server.cs");
	exec("./server/message.cs");
	exec("./server/clientConnection.cs");
	exec("./server/kickban.cs");
	exec("./server/chatServer.cs");
	return;
}
function loadDir(%dir)
{
	setModPaths(getModPaths() @ ";" @ %dir);
	exec(%dir @ "/main.cs");
	return;
}
function mRound(%num)
{
	if (%num - mFloor(%num) >= 0.5)
	{
		%value = mCeil(%num);
	}
	else
	{
		%value = mFloor(%num);
	}
	return %value;
	return %value;
}
