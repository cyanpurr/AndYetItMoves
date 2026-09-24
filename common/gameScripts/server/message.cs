// message.cs.dso
function messageClient(%client, %msgType, %msgString)
{
	commandToClient(%client, messageClient, %msgType, %msgString);
	return;
}
function messageTeam(%team, %msgType, %msgString)
{
	%count = ClientGroup.getCount();
	%i = 0;
	while (%i < %count)
	{
		%recipient = ClientGroup.getObject(%i);
		if (team == %team)
		{
			messageClient(%recipient, %msgType, %msgString);
		}
		%i = %i + 1.0;
	}
	return;
}
function messageTeamExcept(%client, %msgType, %msgString)
{
	%team = team;
	%count = ClientGroup.getCount();
	%i = 0;
	while (%i < %count)
	{
		%recipient = ClientGroup.getObject(%i);
		if (team == %team && %recipient != %client)
		{
			messageClient(%recipient, %msgType, %msgString);
		}
		%i = %i + 1.0;
	}
	return;
}
function messageAll(%msgType, %msgString)
{
	%count = ClientGroup.getCount();
	%i = 0;
	while (%i < %count)
	{
		%recipient = ClientGroup.getObject(%i);
		messageClient(%recipient, %msgType, %msgString);
		%i = %i + 1.0;
	}
	return;
}
function messageAllExcept(%client, %team, %msgType, %msgString)
{
	%count = ClientGroup.getCount();
	%i = 0;
	while (%i < %count)
	{
		%recipient = ClientGroup.getObject(%i);
		if (%recipient != %client && team != %team)
		{
			messageClient(%recipient, %msgType, %msgString);
		}
		%i = %i + 1.0;
	}
	return;
}
$SPAM_PROTECTION_PERIOD = 10000;
$SPAM_MESSAGE_THRESHOLD = 4;
$SPAM_PENALTY_PERIOD = 10000;
$SPAM_MESSAGE = FLOOD PROTECTION: You must wait another %1 seconds.;
function GameConnection::spamMessageTimeout(%this)
{
	if (spamMessageCount > 0.0)
	{
		%this.spamMessageCount = spamMessageCount - 1.0;
	}
	return %this;
}
function GameConnection::spamReset(%this)
{
	%this.isSpamming = "0";
	return;
}
function spamAlert(%client)
{
	if ($Pref::Server::FloodProtectionEnabled != 1.0)
	{
		return "0";
	}
	if (!(isSpamming) && spamMessageCount >= $SPAM_MESSAGE_THRESHOLD)
	{
		%client.spamProtectStart = getSimTime();
		%client.isSpamming = "1";
		%client.schedule($SPAM_PENALTY_PERIOD, spamReset);
	}
	if (isSpamming)
	{
		%wait = mFloor($SPAM_PENALTY_PERIOD - getSimTime() - spamProtectStart / 1000.0);
		messageClient(%client, "", $SPAM_MESSAGE, %wait);
		return "1";
	}
	%client.spamMessageCount = spamMessageCount + 1.0;
	%client.schedule($SPAM_PROTECTION_PERIOD, spamMessageTimeout);
	return "0";
	return "0";
}
function chatMessageClient(%client, %sender, %msgString)
{
	if (!(muted[%sender]))
	{
		commandToClient(%client, ndToClient, %sender, %msgString);
	}
	return;
}
function chatMessageTeam(%sender, %team, %msgString)
{
	if (%msgString $= "" || spamAlert(%sender))
	{
		return spamAlert(%sender);
	}
	%count = ClientGroup.getCount();
	%i = 0;
	while (%i < %count)
	{
		%receiver = ClientGroup.getObject(%i);
		if (team == team)
		{
			chatMessageClient(%receiver, %sender, %msgString);
		}
		%i = %i + 1.0;
	}
	return;
}
function chatMessageAll(%sender, %msgString)
{
	if (%msgString $= "" || spamAlert(%sender))
	{
		return spamAlert(%sender);
	}
	%count = ClientGroup.getCount();
	%i = 0;
	while (%i < %count)
	{
		%receiver = ClientGroup.getObject(%i);
		chatMessageClient(%receiver, %sender, %msgString);
		%i = %i + 1.0;
	}
	return;
}
