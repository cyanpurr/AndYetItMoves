// spawnObjects.cs.dso
if (!(isObject(BeSpawnObjects)))
{
	%template = new BehaviorTemplate(Name : BeSpawnObjects);
	%template.friendlyName = "spawnObjects";
	%template.behaviorType = "Positioning";
	%template.description = "spawns specified objects at this objects place";
	%template.addBehaviorField(spawnInterval, "how often the object should spawn Objects (in s), -1 turns intervall spawning off", float, "2");
	%template.addBehaviorField(spawnIntervalVariance, "how much the intervall should randomly differ (in s; [spawnInterval - spawnIntervalVariance; spawnInterval + spawnIntervalVariance])", float, "1");
	%template.addBehaviorField(spawnPositionVariance, "how much the spawnposition should differ from center or first link point", Vector, "0 0", t2dVector);
	%template.addBehaviorField(objectList, "a list of objects that will be cloned", string, "originalObject");
	%template.addBehaviorField(maxAliveObjects, "if this many objects are in the level we want spawn anymore. -1 unlimited", integer, -1.0);
	%template.addBehaviorField(autoStart, "if the object should start spawning object right away", bool, "0");
	%template.addBehaviorField(pauseOnRotate, "specify to switchOn on a certain degree of rotation (off on all other); -1 ignore rotation", integer, -1.0);
	%template.addBehaviorField(keepSpawningWhileRotation, "keep Spawning While Rotation?", bool, "0");
	%template.addBehaviorField(escapeSwitch, "false -> let it be detected by a switch which collects its children automatically", bool, "1");
}
function BeSpawnObjects::onBehaviorAdd(%this)
{
	if (spawnIntervalVariance > spawnInterval)
	{
		%this.spawnIntervalVariance = spawnInterval;
		debugWarn("spawnIntervalVariance too big; it could lead to a negative spawnIntervall.");
		debugWarn("setting spawnIntervalVariance to next possible value:" SPC spawnIntervalVariance);
	}
	%this.objectsAlive = "0";
	subscribeToEvents(%this, "onLevelLoadFinished20 onRainStart");
	return;
}
function BeSpawnObjects::onLevelLoadFinished20(%this)
{
	Owner.escapeSwitch = escapeSwitch;
	if (autoStart)
	{
		subscribeToEvents(%this, "onFirstKeyPressed");
	}
	return;
}
function BeSpawnObjects::onFirstKeyPressed(%this)
{
	%this.switchOn();
	return;
}
function BeSpawnObjects::switchOn(%this)
{
	%owner = Owner;
	if (pauseOnRotate != -1.0)
	{
		subscribeToEvents(%this, "onRotationFinish");
	}
	%this.resume();
	return;
}
function BeSpawnObjects::resume(%this)
{
	%owner = Owner;
	if (spawning)
	{
		return %this;
	}
	%this.spawning = "1";
	if (%owner.getLinkCount())
	{
	}
	else
	{
	}
	%this.spawnPosition = %owner.getPosition();
	if (spawnInterval < 0.0)
	{
		if (maxAliveObjects < 0.0 || maxAliveObjects > 0.0 && objectsAlive < maxAliveObjects)
		{
			%this.spawn();
		}
	}
	else
	{
		%this.regularlySpawn();
	}
	return;
}
function BeSpawnObjects::switchOff(%this)
{
	%this.pause();
	if (pauseOnRotate != -1.0)
	{
		unSubscribeFromEvents(%this, "onRotationFinish");
	}
	return;
}
function BeSpawnObjects::pause(%this)
{
	%this.spawning = "0";
	if (keepSpawningWhileRotation)
	{
		if (isEventPending(actualSchedule))
		{
			cancel(actualSchedule);
		}
	}
	else
	{
		if (isObject(safeScheduleObject))
		{
			safeScheduleObject.cancelSchedule();
		}
	}
	return;
}
function BeSpawnObjects::onRainStart(%this)
{
	%this.switchOff();
	return;
}
function BeSpawnObjects::spawn(%this)
{
	if (!(spawning))
	{
		debugWarn("tried too spawn allthough this spawner is switched off" SPC %this);
		return;
	}
	if (maxAliveObjects > 0.0 && objectsAlive >= maxAliveObjects)
	{
		if (spawnInterval > 0.0)
		{
			if (keepSpawningWhileRotation)
			{
				%this.schedule(spawnInterval * 1000.0, "spawn");
				break;
			}
			safeSchedule(spawnInterval * 1000.0, %this, "spawn");
		}
		return "0";
	}
	%owner = Owner;
	%objectName = getWord(objectList, getRandom("0", getWordCount(objectList) - 1.0));
	%this.object = %objectName.cloneWithBehaviors();
	object.setName("");
	object.setGraphGroup(%objectName.getGraphGroup());
	object.setImmovable("0");
	if (spawnPositionVariance != "0 0")
	{
	}
	else
	{
	}
	%position = spawnPosition;
	%mountedKids = %objectName.getMountedChildren();
	%i = 0;
	while (%i < getWordCount(%mountedKids))
	{
		%mountedChild = getWord(%mountedKids, %i);
		if (%mountedChild.getBehavior("BeMask"))
		{
			%mask = %mountedChild;
		}
		if (%mountedChild.getBehavior("BeTexture"))
		{
			%hasTextureMounted = 1;
		}
		%i = %i + 1.0;
	}
	if (isObject(%mask))
	{
		%newMask = %mask.cloneWithBehaviors();
		%newMask.getBehavior("BeMask").cloneTexture();
		%newMask.mount(object, "0 0", "0", "1", "1", "1", "1");
	}
	if (isObject(%objectName.getBehavior("BeMask")) && %hasTextureMounted)
	{
		object.getBehavior("BeMask").textureObject = textureObject;
		object.getBehavior("BeMask").cloneTexture();
	}
	if (isObject(%objectName.getBehavior("BePlayCollisionSound")))
	{
		object.getBehavior("BePlayCollisionSound").AudioProfile = AudioProfile;
	}
	%movingBehvaior = object.getBehavior("BeMoving");
	if (isObject(%movingBehvaior))
	{
		%movingBehvaior.createTriggerShape();
	}
	object.setPosition(%position);
	%this.objectsAlive = objectsAlive + 1.0;
	if (object.isCallableMethod("onSpawnFinished"))
	{
		object.onSpawnFinished(%this, %objectName);
	}
	object.callOnBehaviors("switchOn");
	return "1";
	return "1";
}
function BeSpawnObjects::regularlySpawn(%this)
{
	if (maxAliveObjects < 0.0 || maxAliveObjects > 0.0 && objectsAlive < maxAliveObjects)
	{
		%this.spawn();
	}
	if (spawnIntervalVariance)
	{
	}
	else
	{
	}
	%nextSpawn = spawnInterval;
	if (keepSpawningWhileRotation)
	{
		%this.actualSchedule = %this.schedule(%nextSpawn * 1000.0, "regularlySpawn");
	}
	else
	{
		%this.safeScheduleObject = safeSchedule(%nextSpawn * 1000.0, %this, "regularlySpawn");
	}
	return;
}
function BeSpawnObjects::objectDied(%this)
{
	if (objectsAlive >= 1.0)
	{
		%this.objectsAlive = objectsAlive - 1.0;
	}
	return %this;
}
function BeSpawnObjects::onRotationFinish(%this)
{
	%owner = Owner;
	%rotationOnTop = negModulo(-1.0 * camera.getCurrentRotation(), "360");
	if (pauseOnRotate == %rotationOnTop)
	{
		%this.pause();
	}
	else
	{
		%this.resume();
	}
	return;
}
