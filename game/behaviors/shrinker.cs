// shrinker.cs.dso
if (!(isObject(BeShrinker)))
{
	%template = new BehaviorTemplate(Name : BeShrinker);
	%template.friendlyName = "shrink this object over time";
	%template.behaviorType = "MetaGameMechanisms";
	%template.description = "update size of object every frame til it reaches 0 over duration";
	%template.addBehaviorField(duration, "in seconds", float, "1");
	%template.addBehaviorField(onShrinkFinished, "how this object should react on a collision with ripped edge and after shrinking", enum, "kill", "kill	respawn	none	callback");
	%template.addBehaviorField(resizeOnFinish, "if object should be resized when shrink is finished", bool, "0");
	%template.addBehaviorField(shrinkChildren, "if all mounted children shoult also be shrinked", bool, "1");
}
function BeShrinker::setOnShrinkFinished(%this, %reaction)
{
	%this.onShrinkFinished = %reaction;
	return;
}
function BeShrinker::setShrinkChildren(%this, %shrinkIt)
{
	%this.shrinkChildren = %shrinkIt;
	return;
}
function BeShrinker::startShrinking(%this)
{
	%owner = Owner;
	if (shrinkChildren)
	{
		%kids = %owner.getMountedChildren();
		%i = 0;
		while (%i < getWordCount(%kids))
		{
			%kid = getWord(%kids, %i);
			if (%kid.getBehavior("BeShrinker"))
			{
				%kid.getBehavior("BeShrinker").startShrinking();
			}
			%i = %i + 1.0;
		}
	}
	%owner.origSize = %owner.getSize();
	%this.wasVisibleBefore = %owner.getVisible();
	%targetSizeVec = t2dVectorSub(t2dVectorScale(%owner.getSize(), "0.001"), %owner.getSize());
	%owner.setSizeVelocity(t2dVectorScale(%targetSizeVec, 1.0 / duration));
	if (shrinkStartCallback != "")
	{
		%owner.call(shrinkStartCallback);
	}
	if (!(isPlayer(%owner)))
	{
		playDistanceEventSound(%owner, PlayerEdgeDeathNew, "0.2");
	}
	%this.schedule(duration * 1000.0, "onShrinkFinished");
	return;
}
function BeShrinker::onShrinkFinished(%this)
{
	%owner = Owner;
	%owner.setSizeVelocity("0", "0");
	if (onShrinkFinished $= "kill")
	{
		%owner.safeDelete();
	}
	else
	{
		if (onShrinkFinished $= "respawn")
		{
			if (isObject(spawnPoint))
			{
				%owner.dismount();
				%owner.setPosition(spawnPoint.getPosition());
				if (resizeOnFinish)
				{
					%owner.setSize(origSize);
				}
				%owner.isOuterSpace = "0";
				%owner.setVisible(wasVisibleBefore);
				%owner.setMaxAngularVelocity(origMaxAngularVelocity);
				%owner.setAngularVelocity("0");
				%owner.setLinearVelocity("0 0");
				%owner.reanimate();
			}
			break;
		}
		if (onShrinkFinished $= "none")
		{
			break;
		}
		if (onShrinkFinished $= "callback")
		{
			triggerEvent("onShrinkFinished");
		}
	}
	return;
}
function BeShrinker::changeLayer(%this, %aboveRippedEdge)
{
	%owner = Owner;
	if (%aboveRippedEdge)
	{
		%owner.origLayer = %owner.getLayer();
		%owner.setLayer($LAYER["rippedEdge"] - 1.0);
	}
	else
	{
		%owner.setLayer(origLayer);
	}
	if (shrinkChildren)
	{
		%kids = %owner.getMountedChildren();
		%i = 0;
		while (%i < getWordCount(%kids))
		{
			%kid = getWord(%kids, %i);
			if (%kid.getBehavior("BeShrinker"))
			{
				%kid.changeLayer(%aboveRippedEdge);
			}
			%i = %i + 1.0;
		}
	}
	return getWordCount(%kids);
}
