// client.cs.dso
function connectToServer(%ip)
{
	disconnect();
	if (%ip $= "")
	{
		MessageBoxOK("Cannot connect to server. IP address not specified.");
		return;
	}
	%conn = new GameConnection(Name : ServerConnection);
	%conn.setConnectArgs($pref::Player::Name);
	%conn.connect(%ip);
	$serverConnected = 1;
	$serverLocal = 0;
	return;
}
function disconnect()
{
	if (!($serverConnected))
	{
		return;
	}
	if (isObject(ServerConnection))
	{
		ServerConnection.delete();
	}
	if ($serverLocal)
	{
		destroyServer();
	}
	disconnectedCleanup();
	return;
}
function disconnectedCleanup()
{
	alxStopAll();
	clearTextureHolds();
	purgeResources();
	$serverConnected = 0;
	onDisconnect();
	return;
}
