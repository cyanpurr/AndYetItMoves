// clientConnection.cs.dso
function GameConnection::onConnectRequest(%client, %netAddress)
{
	echo("Connect request from: " @ %netAddress);
	return "";
	return "";
}
function GameConnection::onConnect(%client, %name)
{
	commandToClient(%client, nection, $Pref::Server::ConnectionError);
	%client.setPlayerName(%name);
	echo("Client Connected: " @ %client @ " " @ %client.getAddress());
	$Server::PlayerCount = $Server::PlayerCount + 1.0;
	messageAllExcept(%client, -1.0, ndToClient, %client);
	%count = ClientGroup.getCount();
	%i = 0;
	while (%i < %count)
	{
		%other = ClientGroup.getObject(%i);
		if (%other != %client)
		{
			messageClient(%client, ndToClient, %other);
		}
		%i = %i + 1.0;
	}
	onClientConnected(%client);
	return;
}
function GameConnection::onDrop(%client, )
{
	removeTaggedString(Name);
	echo("Client Dropped: " @ %client @ " " @ %client.getAddress());
	$Server::PlayerCount = $Server::PlayerCount - 1.0;
	messageAllExcept(%client, -1.0, er::ConnectionError, %client);
	onClientDropped(%client);
	return;
}
function GameConnection::setPlayerName(%client, %name)
{
	echo("name = " @ %name);
	%name = trim(%name);
	if (%name $= "")
	{
		%name = "TGB Gamer";
	}
	%nameTest = %name;
	%count = 0;
	while (!(isNameUnique(%nameTest)))
	{
		%count = %count + 1.0;
		%nameTest = %name @ %count;
	}
	%client.Name = %nameTest;
	return;
}
function isNameUnique(%name)
{
	%count = ClientGroup.getCount();
	%i = 0;
	while (%i < %count)
	{
		%client = ClientGroup.getObject(%i);
		%test = detag(getTaggedString(Name));
		if (strcmp(%name, %test) == 0.0)
		{
			return "0";
		}
		%i = %i + 1.0;
	}
	return "1";
	return "1";
}
