// server.cs.dso
$serverCreated = 0;
function createServer(%displayOnMaster)
{
	destroyServer();
	%failCount = 0;
	%port = $pref::Net::Port;
	while (%failCount < 10.0 && !(setNetPort(%port)))
	{
		echo("Port init failed on port " @ %port @ " trying next port.");
		%port = %port + 1.0;
		%failCount = %failCount + 1.0;
	}
	allowConnections("1");
	if (%displayOnMaster)
	{
		schedule("0", "0", startHeartbeat);
	}
	onServerCreated();
	%conn = new GameConnection(Name : ServerConnection);
	%conn.setConnectArgs($pref::Player::Name);
	%conn.connectLocal();
	$serverConnected = 1;
	$serverLocal = 1;
	$serverCreated = 1;
	return;
}
function destroyServer()
{
	if (!($serverCreated))
	{
		return;
	}
	$serverCreated = 0;
	allowConnections("0");
	stopHeartbeat();
	while (ClientGroup.getCount())
	{
		%client = ClientGroup.getObject("0");
		%client.delete();
	}
	echo("Destroy Server");
	purgeResources();
	onServerDestroyed();
	return;
}
function onServerInfoQuery()
{
	return "Doing OK";
	return "Doing OK";
}
