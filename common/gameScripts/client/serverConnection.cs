// serverConnection.cs.dso
function clientCmdSetConnectionError(%error)
{
	$ServerConnectionErrorMessage = %error;
	return;
}
function GameConnection::onConnectionAccepted()
{
	onConnect();
	return;
}
function GameConnection::onConnectionTimedOut()
{
	disconnectedCleanup();
	MessageBoxOK("TIMED OUT", "The server connection has timed out.");
	return;
}
function GameConnection::onConnectionDropped(, %msg)
{
	disconnectedCleanup();
	MessageBoxOK("DISCONNECT", "The server has dropped the connection: " @ %msg);
	return;
}
function GameConnection::onConnectionError(, %msg)
{
	disconnectedCleanup();
	MessageBoxOK("DISCONNECT", $ServerConnectionErrorMessage @ " (" @ %msg @ ")");
	return;
}
function GameConnection::onConnectRequestRejected(, %msg)
{
	if (%msg $= "CHR_PASSWORD")
	{
		if ($Client::Password $= "")
		{
			%error = "The server requires a password.";
		}
		else
		{
			%error = "The password you entered is incorrect.";
		}
	}
	else
	{
		if (%msg $= "CHR_PROTOCOL")
		{
			%error = "Incompatible protocol version: Your game version is not compatible with this server.";
			break;
		}
		%error = "Connection error. Please try another server. Error code: (" @ %msg @ ")";
	}
	disconnectedCleanup();
	MessageBoxOK("REJECTED", %error);
	return;
}
function GameConnection::onConnectRequestTimedOut()
{
	disconnectedCleanup();
	MessageBoxOK("TIMED OUT", "Your connection to the server timed out.");
	return;
}
