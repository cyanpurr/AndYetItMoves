// chatServer.cs.dso
function initChat(%name)
{
	$chatStarted = 1;
	if (%name $= "")
	{
		%name = "Torque Game Builder Chat";
	}
	new SimSet(Name : chatGroup);
	chatGroup.Name = %name;
	return;
}
function onClientConnected(%client)
{
	return;
}
function onServerCreated()
{
	return;
}
function onServerDestroyed()
{
	return;
}
function getConnectionList(%client)
{
	$serverConnectionList = "";
	$serverNameList = "";
	$serverConnectionCount = 0;
	%connectionCount = ClientGroup.getCount();
	%count = 0;
	%i = 0;
	while (%i < %connectionCount)
	{
		%connection = ClientGroup.getObject(%i);
		%name = Name;
		$serverConnectionList = $serverConnectionList @ %connection;
		$serverConnectionList = $serverConnectionList @ "|";
		$serverNameList = $serverNameList @ %name;
		$serverNameList = $serverNameList @ "|";
		%count = %count + 1.0;
		%i = %i + 1.0;
	}
	$serverConnectionCount = %count;
	commandToClient(%client, , $serverConnectionCount);
	commandToClient(%client, me, $serverConnectionList);
	commandToClient(%client, d, $serverNameList);
	return;
}
function getChatConnectionList(%client)
{
	$serverChatConnectionList = "";
	$serverChatNameList = "";
	$serverChatConnectionCount = 0;
	%chatCount = chatGroup.getCount();
	%count = 0;
	%i = 0;
	while (%i < %chatCount)
	{
		%chat = chatGroup.getObject(%i);
		%name = Name;
		$serverChatConnectionList = $serverChatConnectionList @ %chat;
		$serverChatConnectionList = $serverChatConnectionList @ "|";
		$serverChatNameList = $serverChatNameList @ %name;
		$serverChatNameList = $serverChatNameList @ "|";
		%count = %count + 1.0;
		%i = %i + 1.0;
	}
	$serverChatConnectionCount = %count;
	commandToClient(%client, ated, $serverChatConnectionCount);
	commandToClient(%client, etConnectionList, $serverChatConnectionList);
	commandToClient(%client, rConnectionList, $serverChatNameList);
	return;
}
function serverCmdGetConnectionList(%client)
{
	getConnectionList(%client);
	return;
}
function serverCmdGetChatConnectionList(%client)
{
	getChatConnectionList(%client);
	return;
}
function serverCmdisChatting(%client)
{
	chatGroup.add(%client);
	updateChatClient(Name);
	return;
}
function serverCmdleftChat(%client)
{
	chatGroup.remove(%client);
	removeChatClient(Name);
	return;
}
function serverCmdupdateChatText(%client, %text)
{
	updateChatText(Name, %text);
	return;
}
function updateChatText(%clientName, %text)
{
	%count = chatGroup.getCount();
	%i = 0;
	while (%i < %count)
	{
		%client = chatGroup.getObject(%i);
		commandToClient(%client, serverNameList, %clientName, %text);
		%i = %i + 1.0;
	}
	return;
}
function updateChatClient(%clientName)
{
	%count = chatGroup.getCount();
	%i = 0;
	while (%i < %count)
	{
		%client = chatGroup.getObject(%i);
		commandToClient(%client, $serverConnectionCount, %clientName);
		%i = %i + 1.0;
	}
	return;
}
function removeChatClient(%clientName)
{
	%count = chatGroup.getCount();
	%i = 0;
	while (%i < %count)
	{
		%client = chatGroup.getObject(%i);
		commandToClient(%client, Count, %clientName);
		%i = %i + 1.0;
	}
	return;
}
function onClientDropped(%client)
{
	if ($chatStarted && isObject(chatGroup))
	{
		chatGroup.remove(%client);
		removeChatClient(Name);
	}
	return;
}
function sendChatLoaded()
{
	$ServerChatLoaded = 1;
	return;
}
function serverCmdisChatLoaded(%client)
{
	commandToClient(%client, ientGroup, $ServerChatLoaded);
	return;
}
function sendChatClosed(%clientName)
{
	%count = chatGroup.getCount();
	%i = 0;
	while (%i < %count)
	{
		%client = chatGroup.getObject(%i);
		commandToClient(%client, nnectionCount);
		%i = %i + 1.0;
	}
	chatGroup.clear();
	return;
}
