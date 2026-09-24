// message.cs.dso
function addMessageCallback(%msgType, %func)
{
	$MSGCB[%msgType] = %func;
	return;
}
function defaultMessageCallback(%msgString)
{
	onServerMessage(detag(%msgString));
	return;
}
addMessageCallback(ClientJoined, clientJoined);
function clientJoined()
{
	return;
}
addMessageCallback(ClientDropped, clientDropped);
function clientDropped()
{
	return;
}
function clientCmdServerMessage(%msgType, %msgString)
{
	if ($MSGCB[%msgType] != "")
	{
		call($MSGCB[%msgType], %msgString);
		return;
	}
	defaultMessageCallback(%msgString);
	return;
}
function clientCmdChatMessage(%sender, %message)
{
	onChatMessage(%sender, detag(%message));
	return;
}
