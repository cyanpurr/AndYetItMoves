// preExecStuff.cs.dso
new SimSet(Name : levelGarbageCollector);
new SimSet(Name : gameGarbageCollector);
gameGarbageCollector.add(levelGarbageCollector);
$NUM_OLLFS_OFFSET = 0;
function resetLoadingString()
{
	%i = 0;
	while (%i < $NUM_OLLFS + $NUM_OLLFS_OFFSET)
	{
		$loadingString[%i] = 0;
		%i = %i + 1.0;
	}
	$numLoadingStringCalls = 0;
	return ['$loadingString', '%i'];
}
function setNextLoadingString()
{
	if ($NUM_OLLFS < $numLoadingStringCalls)
	{
		return ['$loadingString', '%i'];
	}
	%ws = ".";
	%empty = "";
	%point = "";
	%i = 0;
	while (%i < $NUM_OLLFS + $NUM_OLLFS_OFFSET - $numLoadingStringCalls)
	{
		%empty = %empty @ " ";
		%i = %i + 1.0;
	}
	%i = 0;
	while (%i < $numLoadingStringCalls)
	{
		%point = %point @ "..";
		%i = %i + 1.0;
	}
	%ws = %ws @ %empty @ %point @ %empty @ ".";
	lbl_loading.text = %ws;
	$numLoadingStringCalls = $numLoadingStringCalls + 1.0;
	return;
}
function getWaitingString(%maxCharCnt, %reset, %dotAtEnd, %reverse)
{
	if (%dotAtEnd $= "")
	{
		%dotAtEnd = 0;
	}
	if (%reverse $= "")
	{
		%reverse = 0;
	}
	if (%reset $= "")
	{
		%reset = 0;
	}
	if (%reset || $numWaitingChars == %maxCharCnt)
	{
		$numWaitingChars = 0;
	}
	%waitStr = "";
	if (%reverse && %dotAtEnd)
	{
		%waitStr = ".";
		%i = $numWaitingChars + 1.0;
		while (%i < %maxCharCnt)
		{
			%waitStr = %waitStr @ " ";
			%i = %i + 1.0;
		}
	}
	%i = 0;
	while (%i < $numWaitingChars)
	{
		%waitStr = %waitStr @ ".";
		%i = %i + 1.0;
	}
	if (!(%reverse) && %dotAtEnd)
	{
		%i = $numWaitingChars + 1.0;
		while (%i < %maxCharCnt)
		{
			%waitStr = %waitStr @ " ";
			%i = %i + 1.0;
		}
		%waitStr = %waitStr @ ".";
	}
	$numWaitingChars = $numWaitingChars + 1.0;
	return %waitStr;
	return %waitStr;
}
