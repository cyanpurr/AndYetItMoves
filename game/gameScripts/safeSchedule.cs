// safeSchedule.cs.dso
function safeSchedule::createInstance(%time, %object, %func, %args)
{
	return new ScriptObject(Name : "");
	return new ScriptObject(Name : "");
}
function safeSchedule(%time, %object, %func, %args)
{
	if (%func $= "")
	{
		debugWarn("called safeSchedule without delivering functionname");
		return;
	}
	if (%object $= "")
	{
		%object = 0;
	}
	if (isObject(%object))
	{
		%object = %object.getId();
	}
	else
	{
		debugWarn("called safeSchedule on a not valid object ID:" SPC %object SPC "(" SPC %func SPC ")");
		return;
	}
	if (!(%object.isMethod(%func)))
	{
		%foundMethod = 0;
		%i = 0;
		while (%i < %object.getBehaviorCount())
		{
			if (%object.getBehaviorByIndex(%i).isMethod(%func))
			{
				%foundMethod = 1;
				break;
			}
			%i = %i + 1.0;
		}
		if (!(%foundMethod))
		{
			debugWarn("called safeSchedule on object" SPC %object SPC "with a inexistent function:" SPC %func);
			return;
		}
	}
	%safeScheduleObject = safeSchedule::createInstance(%time, %object, %func, %args);
	subscribeToEvents(%safeScheduleObject, "onRotationStart onRotationFinish onLevelShutdown");
	if (!(camera.getIsRotating()))
	{
		%safeScheduleObject.resumeSchedule();
	}
	safeScheduleList.add(%safeScheduleObject);
	return %safeScheduleObject;
	return %safeScheduleObject;
}
function t2dSceneObject::safeSchedule(%this, %time, %func, %args)
{
	return safeSchedule(%time, %this, %func, %args);
	return safeSchedule(%time, %this, %func, %args);
}
function safeSchedule::resumeSchedule(%this)
{
	if (timeLeft == 0.0 || timeLeft $= "")
	{
		%this.scheduleEnd();
	}
	else
	{
		%this.scheduleID = %this.schedule(timeLeft, "scheduleEnd");
	}
	return;
}
function safeSchedule::scheduleEnd(%this)
{
	unSubscribeFromEvents(%this, "onRotationStart onRotationFinish");
	if (trim(args) $= "")
	{
		if (isObject(object))
		{
			object.call(func);
		}
		else
		{
			call(func);
		}
	}
	else
	{
		if (isObject(object))
		{
			eval("%this.object.call( %this.func, " @ args @ " );");
			break;
		}
		eval("call( %this.func, " @ args @ " );");
	}
	if (isObject(%this))
	{
		%this.delete();
	}
	return;
}
function safeSchedule::pauseSchedule(%this)
{
	if (isEventPending(scheduleID))
	{
		%this.timeLeft = getEventTimeLeft(scheduleID);
		cancel(scheduleID);
	}
	return;
}
function safeSchedule::cancelSchedule(%this)
{
	if (isEventPending(scheduleID))
	{
		cancel(scheduleID);
	}
	%this.delete();
	return;
}
function safeSchedule::isRunning(%this)
{
	return isEventPending(scheduleID);
	return isEventPending(scheduleID);
}
function safeSchedule::onRotationStart(%this)
{
	%this.pauseSchedule();
	return;
}
function safeSchedule::onRotationFinish(%this)
{
	%this.resumeSchedule();
	return;
}
function safeSchedule::onLevelShutdown(%this)
{
	%this.cancelSchedule();
	return;
}
