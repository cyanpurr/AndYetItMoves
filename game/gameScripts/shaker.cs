// shaker.cs.dso
function createShaker()
{
	%shaker = new ScriptObject(Name : "")
	{
		class = shaker;
	}
	%shaker.Smoother = Smoother::createInstance();
	levelGarbageCollector.add(%shaker);
	levelGarbageCollector.add(Smoother);
	return %shaker;
	return %shaker;
}
function shaker::init(%this, %object, %duration, %variation, %mode)
{
	%this.object = %object;
	%this.initialPosition = %object.getPosition();
	%this.isShaking = "0";
	%this.objectIsMounted = isObject(%object.getBehavior("BeMountWithOffset"));
	if (objectIsMounted)
	{
		%localOffset = %object.getNormedLocalPoint(%variation);
		Smoother.init(%duration, "0 0", %localOffset, %mode, "1");
		%this.mountBehavior = %object.getBehavior("BeMountWithOffset");
	}
	else
	{
		Smoother.init(%duration, initialPosition, t2dVectorAdd(%object.getPosition(), %variation), %mode, "1");
	}
	return;
}
function shaker::start(%this)
{
	Smoother.start();
	%this.isShaking = "1";
	subscribeToEvents(%this, "onUpdateFrame");
	return;
}
function shaker::onUpdateFrame(%this)
{
	%smoothValue = Smoother.getValue();
	if (objectIsMounted)
	{
		%normedWorldOffset = t2dVectorSub(object.getWorldPoint(%smoothValue), object.getPosition());
		mountBehavior.setMountOffsetCorrection(%normedWorldOffset);
	}
	else
	{
		object.setPosition(%smoothValue);
	}
	if (Smoother.getIsFinished())
	{
		%this.stop();
	}
	return;
}
function shaker::stop(%this)
{
	unSubscribeFromEvents(%this, "onUpdateFrame");
	%this.isShaking = "0";
	if (objectIsMounted)
	{
		mountBehavior.resetMountOffset();
	}
	else
	{
		if (!(isObject(object)))
		{
			%bla = 1;
		}
		object.setPosition(initialPosition);
	}
	return;
}
