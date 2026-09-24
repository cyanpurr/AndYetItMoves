// chatClient.cs.dso
function onConnect()
{
	if (waitingForServer.isAwake())
	{
		Canvas.popDialog(waitingForServer);
	}
	MessageBoxOK("Connection Established...", "Connection Established with the server!", "");
	return;
}
function clientCmdPassConnectionList(%connectionList)
{
	$clientConnectionsList = new ScriptObject(Name : clientConnectionsList);
	%list = explode(%connectionList, "|");
	%count = $clientConnectionCount;
	%i = 0;
	while (%i < %count)
	{
		$clientConnectionsList.contents[%i] = contents[%i];
		%i = %i + 1.0;
	}
	return;
}
function clientCmdPassNameList(%nameList)
{
	$clientNamesList = new ScriptObject(Name : clientNamesList);
	%list = explode(%nameList, "|");
	%count = $clientConnectionCount;
	%i = 0;
	while (%i < %count)
	{
		$clientNamesList.contents[%i] = contents[%i];
		%i = %i + 1.0;
	}
	return;
}
function clientCmdPassConnectionCount(%connectionCount)
{
	$clientConnectionCount = %connectionCount;
	return;
}
function clientCmdPassChatConnectionList(%connectionList)
{
	$clientChatConnectionsList = new ScriptObject(Name : clientChatConnectionsList);
	%list = explode(%connectionList, "|");
	%count = $clientChatConnectionCount;
	%i = 0;
	while (%i < %count)
	{
		$clientChatConnectionsList.contents[%i] = contents[%i];
		%i = %i + 1.0;
	}
	return;
}
function clientCmdPassChatNameList(%nameList)
{
	$clientChatNamesList = new ScriptObject(Name : clientChatNamesList);
	%list = explode(%nameList, "|");
	%count = $clientChatConnectionCount;
	%i = 0;
	while (%i < %count)
	{
		$clientChatNamesList.contents[%i] = contents[%i];
		%i = %i + 1.0;
	}
	if ($waitingForList)
	{
		chatGui::onGetList();
	}
	return;
}
function clientCmdPassChatConnectionCount(%connectionCount)
{
	$clientChatConnectionCount = %connectionCount;
	return;
}
function clientCmdupdateChatText(%clientName, %text)
{
	%string = %clientName @ ": " @ %text;
	chatVectorText.pushBackLine(%string, "0");
	return;
}
function clientCmdupdateChatClient(%clientName)
{
	if (!(isObject(clientChatInfo)))
	{
		new SimSet(Name : clientChatInfo);
	}
	if (clientCount $= "")
	{
		clientChatInfo.clientCount = "0";
	}
	chatClientList.addRow(clientCount, %clientName);
	$chat::clientCount = $chat::clientCount + 1.0;
	return;
}
function clientCmdremoveChatClient(%clientName)
{
	clientChatInfo.remove(%clientName);
	chatClientList.removeRow(chatClientList.findTextIndex(%clientName));
	$chat::clientCount = $chat::clientCount - 1.0;
	return;
}
function loadChat()
{
	if ($serverConnected)
	{
		if ($serverLocal)
		{
			initChat();
			Canvas.pushDialog(chatGui);
			sendChatLoaded();
		}
		else
		{
			commandToServer(PassConnectionList);
			Canvas.pushDialog(waitingForServer);
		}
	}
	else
	{
		MessageBoxOK("Loading Chat...", "Loading chat failed: No Server Connection Exists.", "");
	}
	return;
}
function clientCmdisChatLoaded(%isChatLoaded)
{
	if (waitingForServer.isAwake())
	{
		Canvas.popDialog(waitingForServer);
	}
	if (%isChatLoaded)
	{
		mainScreenGui.add(chatGui);
	}
	else
	{
		MessageBoxOK("Loading Chat...", "Loading chat failed: No Chat Loaded on Server.", "");
	}
	return;
}
function clientCmdChatClosed()
{
	serverData.chatLoaded = "0";
	if (!($serverLocal))
	{
		if (chatGui.isAwake())
		{
			mainScreenGui.remove(chatGui);
		}
	}
	return;
}
function explode(%string, %char)
{
	if (!(isObject(explode)))
	{
		new ScriptObject(Name : explode);
	}
	%explodeCount = 0;
	%lastFound = 0;
	%endChar = strlen(%string);
	%charLen = strlen(%char);
	%i = 0;
	while (%i < %endChar)
	{
		%charToCheck = getSubStr(%string, %i, %charLen);
		if (%charToCheck $= %char)
		{
			explode.contents[%explodeCount] = getSubStr(%string, %lastFound, %i - %lastFound);
			%lastFound = %i + %charLen;
			%explodeCount = %explodeCount + 1.0;
		}
		%i = %i + 1.0;
	}
	explode.contents[%explodeCount] = getSubStr(%string, %lastFound, %i - %lastFound);
	explode.count = %explodeCount + 1.0;
	return explode;
	return explode;
}
