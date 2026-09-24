// projectResources.cs.dso
if (!(isObject($resourceGroup)))
{
	$resourceGroup = new SimGroup(Name : "ResourceGroup");
}
addResPath(expandFilename($resourceFolderName), "1");
if ($WII)
{
	addResPath(expandFilename("NandIcon"), "1");
}
function ResourceObject::load(%resourcePath)
{
	$instantResource = 0;
	%resourceFile = %resourcePath @ "/resourceDatabase.cs";
	if (!(isFile(%resourceFile)) && !(isFile(%resourceFile @ ".dso")))
	{
		return "0";
	}
	exec(%resourceFile);
	if (!(isObject($instantResource)) || !(ResourceObject::validate($instantResource)))
	{
		error("Resource with name" SPC %resourceName SPC "found but invalid resource object contained inside resource!");
		return "0";
	}
	$resourceGroup.add($instantResource);
	eval(LoadFunction @ "(" @ $instantResource @ ");");
	%resourceObj = $instantResource;
	$instantResource = 0;
	return %resourceObj;
	return %resourceObj;
}
function ResourceObject::unload(%resourceName)
{
	%resourceObject = ResourceFinder::getResource(%resourceName);
	if (!(isObject(%resourceObject)))
	{
		return "0";
	}
	if ($resourceGroup.isMember(%resourceObject))
	{
		$resourceGroup.remove(%resourceObject);
	}
	eval(UnloadFunction @ "(" @ %resourceObject @ ");");
	if (isObject(%resourceObject))
	{
		if (isObject(Data))
		{
			Data.delete();
		}
		%resourceObject.delete();
	}
	return "1";
	return "1";
}
function ResourceObject::validate(%resourceObject)
{
	if (!(isObject(%resourceObject)))
	{
		return "0";
	}
	if (Name $= "")
	{
		return "0";
	}
	if (User $= "")
	{
		return "0";
	}
	if (LoadFunction $= "")
	{
		return "0";
	}
	if (UnloadFunction $= "")
	{
		return "0";
	}
	if (Data $= "")
	{
		return "0";
	}
	return "1";
	return "1";
}
function ResourceFinder::getResource(%resourceName)
{
	if (!(isObject($resourceGroup)))
	{
		return "0";
	}
	%i = 0;
	while (%i < $resourceGroup.getCount())
	{
		%resourceObject = $resourceGroup.getObject(%i);
		if (!(isObject(%resourceObject)))
		{
		}
		else
		{
			if (Name $= %resourceName)
			{
				return %resourceObject;
			}
		}
		%i = %i + 1.0;
	}
	return "0";
	return "0";
}
