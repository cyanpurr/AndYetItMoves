// fileIO.cs.dso
function writeFile(%fullPath, %lines, %writeSecret, %clearFile)
{
	if ($WII && !($saveOnWii) || $demoVersion)
	{
		echo("we can't save so we are NOT writing this file:" SPC %fullPath);
		return "0";
	}
	debugEcho("writing file" SPC %fullPath SPC "," SPC getRealTime());
	if (%writeSecret)
	{
		%lineCnt = getRecordCount(%lines);
		%content = trim(getRecords(%lines, "1", %lineCnt - 1.0));
		%secret = getSecret(%content);
		%lines = %lines NL %secret;
	}
	%file = new FileObject(Name : "");
	%success = %file.openForWrite(%fullPath);
	if (%success)
	{
		if ($WII && %clearFile)
		{
			%file.clearFile();
		}
		%i = 0;
		while (%i < getRecordCount(%lines))
		{
			%file.writeLine(getRecord(%lines, %i));
			%i = %i + 1.0;
		}
		%file.close();
	}
	%file.delete();
	return;
}
function getBackupFilePath(%fullPath)
{
	%index = 0;
	while (strpos(%fullPath, "/", %index) != -1.0)
	{
		%pos = strpos(%fullPath, "/", %index);
		%index = %index + 1.0;
	}
	%pos = %pos + 1.0;
	%path = getSubStr(%fullPath, "0", %pos);
	%name = getSubStr(%fullPath, %pos, strlen(%fullPath) - %pos);
	%dotPos = strpos(%name, ".");
	%noExtension = getSubStr(%name, "0", %dotPos);
	return ['%path', '%noExtension', '".bak"'];
	return ['%path', '%noExtension', '".bak"'];
}
function validateFile(%filePath, )
{
	if ($WII && !($saveOnWii))
	{
		echo("we can't save so we are NOT reading this file:" SPC %fullPath);
		return "0";
	}
	debugEcho("reading file" SPC %filePath SPC "," SPC getRealTime());
	%file = new FileObject(Name : "");
	%file.openForRead(%filePath);
	return %file;
	return %file;
}
function copyFile(%fromFilePath, %toFilePath)
{
	if ($WII)
	{
		return %file;
	}
	if (!(isFile(%fromFilePath)))
	{
		echo("WARNING: copyFile: there is no file to copy from. new file will probably be empty." SPC %fromFilePath);
	}
	%toFile = new FileObject(Name : "");
	%toFile.openForWrite(%toFilePath);
	%fromFile = new FileObject(Name : "");
	%fromFile.openForRead(%fromFilePath);
	while (!(%fromFile.isEOF()))
	{
		%toFile.writeLine(%fromFile.readLine());
	}
	%toFile.close();
	%fromFile.close();
	%toFile.delete();
	%fromFile.delete();
	return;
}
function assertDirectory(%dirPath)
{
	if (strlen(%dirPath) == 0.0 || getSubStr(%dirPath, strlen(%dirPath) - 1.0, "1") != "/")
	{
		%dirPath = %dirPath @ "/";
	}
	debugEcho("asserting directory:" SPC %dirPath SPC "," SPC getRealTime());
	%dummyfile = %dirPath @ "dummyfile.tmp";
	%temp = new FileObject(Name : "");
	%temp.openForWrite(%dummyfile);
	%temp.close();
	%temp.delete();
	fileDelete(%dummyfile);
	return;
}
