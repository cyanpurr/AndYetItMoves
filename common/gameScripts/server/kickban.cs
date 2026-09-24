// kickban.cs.dso
function kick(%client)
{
	messageAll(kick, l, Name);
	if (!(%client.isAIControlled()))
	{
		BanList::add(guid, %client.getAddress(), $Pref::Server::KickBanTime);
	}
	%client.delete("You have been kicked from this server");
	return;
}
function ban(%client)
{
	messageAll(kick, :Server::KickBanTime, Name);
	if (!(%client.isAIControlled()))
	{
		BanList::add(guid, %client.getAddress(), $Pref::Server::BanTime);
	}
	%client.delete("You have been banned from this server");
	return;
}
