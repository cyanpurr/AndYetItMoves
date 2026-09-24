// callNextFrame.cs.dso
function callNextFrame(%object, %methodAndArgs, %numTicks, %useFrame)
{
	%callNF = new ScriptObject(Name : "")
	{
		class = callNextFrame;
		object = %object;
		methodAndArgs = %methodAndArgs;
	}
	levelGarbageCollector.add(%callNF);
	%callNF.numTicks = %numTicks;
	if (%useFrame $= "")
	{
		%callNF.tickCalled = getTickCount();
		subscribeToEvent(%callNF, "onUpdateTick10");
	}
	else
	{
		%callNF.frameCalled = getFrameCount();
		subscribeToEvent(%callNF, "onUpdateFrame");
	}
	return;
}
function onUpdateTick10(%this)
{
	%thisTick = getTickCount();
	if (%thisTick >= tickCalled + numTicks)
	{
		%this.doTheCall();
	}
	return;
}
function onUpdateFrame(%this)
{
	%thisFrame = getFrameCount();
	if (%thisFrame >= frameCalled + numTicks)
	{
		%this.doTheCall();
	}
	return;
}
function doTheCall(%this)
{
	if (isObject(object))
	{
		eval(object @ ".call(" @ methodAndArgs @ ");");
	}
	else
	{
		eval("call(" @ methodAndArgs @ ");");
	}
	%this.delete();
	return;
}
