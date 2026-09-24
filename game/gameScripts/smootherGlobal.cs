// smootherGlobal.cs.dso
function initGlobalSmoother()
{
	if (!(isObject(globalSmoother)))
	{
		new ScriptObject(Name : globalSmoother);
		levelGarbageCollector.add(globalSmoother);
	}
	globalSmoother.smootherList = "";
	globalSmoother.sm = "";
	subscribeToEvents(globalSmoother, "onUpdateTick1 onRotationStart onRotationFinish onLevelShutdown");
	return;
}
function globalSmoother::addSmoother(%this, %name, %loop, %rotationAware)
{
	if (isObject(sm[%name]))
	{
		return sm[%name];
	}
	%newSmootherWrapper = new ScriptObject(Name : ['"sm"', '%name'])
	{
		class = "SmootherWrapper";
		rotationAware = %rotationAware;
		loop = %loop;
	}
	levelGarbageCollector.add(%newSmootherWrapper);
	%newSmootherWrapper.Smoother = Smoother::createInstance();
	levelGarbageCollector.add(Smoother);
	%this.sm[%name] = %newSmootherWrapper;
	%this.smootherList = ltrim(smootherList SPC %name);
	return %newSmootherWrapper;
	return %newSmootherWrapper;
}
function SmootherWrapper::init(%this, %duration, %startValue, %endValue, %mode, %useFrameTime)
{
	Smoother.init(%duration, %startValue, %endValue, %mode, %useFrameTime);
	return;
}
function SmootherWrapper::start(%this, %reverse, %inBetween, %stayPaused)
{
	Smoother.start(%reverse, %inBetween, %stayPaused);
	return;
}
function SmootherWrapper::getValue(%this)
{
	return curValue;
	return curValue;
}
function SmootherWrapper::getIsFinished(%this)
{
	return isFinished;
	return isFinished;
}
function globalSmoother::getValue(%this, %name)
{
	return curValue;
	return curValue;
}
function globalSmoother::getIsFinished(%this, %name)
{
	return isFinished;
	return isFinished;
}
function globalSmoother::getValue(%this, %name)
{
	return curValue;
	return curValue;
}
function globalSmoother::onUpdateTick1(%this)
{
	%i = 0;
	while (%i < getWordCount(smootherList))
	{
		%curSmootherWrapper = sm[getWord(smootherList, %i)];
		%curSmootherWrapper.isFinished = Smoother.getIsFinished();
		if (Smoother.getIsFinished())
		{
			if (loop)
			{
				Smoother.start(Smoother.getIsFinished() + 1.0);
			}
		}
		%curSmootherWrapper.curValue = Smoother.getValue();
		%curSmootherWrapper.curPercentage = Smoother.getProgressPercentage();
		%i = %i + 1.0;
	}
	return getWordCount(smootherList);
}
function globalSmoother::onRotationStart(%this)
{
	%i = 0;
	while (%i < getWordCount(smootherList))
	{
		%curSmootherWrapper = sm[getWord(smootherList, %i)];
		if (rotationAware)
		{
			Smoother.pause();
		}
		%i = %i + 1.0;
	}
	return getWordCount(smootherList);
}
function globalSmoother::onRotationFinish(%this)
{
	%i = 0;
	while (%i < getWordCount(smootherList))
	{
		%curSmootherWrapper = sm[getWord(smootherList, %i)];
		if (rotationAware)
		{
			Smoother.start();
		}
		%i = %i + 1.0;
	}
	return getWordCount(smootherList);
}
function globalSmoother::onLevelShutdown(%this)
{
	%i = 0;
	while (%i < getWordCount(smootherList))
	{
		%sm = getWord(smootherList, %i);
		%sm.delete();
		%i = %i + 1.0;
	}
	%this.delete();
	return;
}
