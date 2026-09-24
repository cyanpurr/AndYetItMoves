// appearOnBeat.cs.dso
if (!(isObject(BeAppearOnBeat)))
{
	%template = new BehaviorTemplate(Name : BeAppearOnBeat);
	%template.friendlyName = "AppearOnBeat";
	%template.behaviorType = "LevelTrip";
	%template.description = "this object will appear according to the registered events coming from the beatgenerator";
	%template.addBehaviorField(beatNumber, "number of the beat (1-4) on which this object should appear", integer, "1");
	%template.addBehaviorField(moreBeatNumbers, "this vector can be filled with more beatnumbers, so the platform will appear on them too", string, "");
	%template.addBehaviorField(bpm, "tempo of the music", integer, "100");
	%template.addBehaviorField(layersToClone, "a list of layers where clones of ourselves will be made", string, "");
	%template.addBehaviorField(colorMask, "if the mask should get a color, or not", bool, "1");
}
function BeAppearOnBeat::onBehaviorAdd(%this)
{
	%owner = Owner;
	subscribeToEvents(%this, "onLevelLoadFinished1 onLevelLoadFinished15 onBeat");
	return;
}
function BeAppearOnBeat::onLevelLoadFinished1(%this)
{
	%owner = Owner;
	%owner.addDependentBehaviors("BeCollide");
	%owner.setVisible("0");
	%owner.setCollisionSuppress("1");
	%this.timePerBeat = 1.0 / bpm / 60.0 / 4.0;
	%this.timeUntilFadeOut = timePerBeat * 1.5;
	%this.timeBetweenSwitchoffs = timePerBeat / 16.0;
	%this.fadeoutTime = timePerBeat / 2.0;
	%this.loseCollisionFactor = "0.125";
	if (layersToClone != "")
	{
		%this.cloning = "1";
	}
	else
	{
		%this.cloning = "0";
	}
	if (cloning)
	{
		%this.clonesSet = new SimSet(Name : "");
		levelGarbageCollector.add(clonesSet);
		%i = 0;
		while (%i < getWordCount(layersToClone))
		{
			%layerName = getWord(layersToClone, %i);
			%clone = new t2dStaticSprite(Name : "")
			{
				scenegraph = scenegraph;
				imageMap = imageMap;
				frame = frame;
				size = size;
				Rotation = Rotation;
				Position = Position;
				Visible = "0";
			}
			%maskBehavior = %clone.addDependentBehavior("BeMask");
			copyBehaviorFields(BeMask, %owner.getBehavior("BeMask"), %maskBehavior);
			%maskBehavior.dontCollide = "1";
			%maskBehavior.Layer = %layerName;
			%pulsatorBehavior = %clone.addDependentBehavior("BePulsatingObject");
			copyBehaviorFields(BePulsatingObject, %owner.getBehavior("BePulsatingObject"), %pulsatorBehavior);
			if (%layerName $= "background_3")
			{
				%clone.fadeOutDelay = -1.0 * timeBetweenSwitchoffs * 3.0;
			}
			else
			{
				if (%layerName $= "background_2")
				{
					%clone.fadeOutDelay = -1.0 * timeBetweenSwitchoffs * 2.0;
					break;
				}
				if (%layerName $= "background_1")
				{
					%clone.fadeOutDelay = -1.0 * timeBetweenSwitchoffs * 1.0;
					break;
				}
				if (%layerName $= "foreground_1")
				{
					%clone.fadeOutDelay = -1.0 * timeBetweenSwitchoffs * 1.0;
					break;
				}
				if (%layerName $= "foreground_2")
				{
					%clone.fadeOutDelay = -1.0 * timeBetweenSwitchoffs * 2.0;
					break;
				}
				if (%layerName $= "foreground_3")
				{
					%clone.fadeOutDelay = -1.0 * timeBetweenSwitchoffs * 3.0;
					break;
				}
				%clone.fadeOutDelay = "0";
			}
			clonesSet.add(%clone);
			%i = %i + 1.0;
		}
	}
	return getWordCount(layersToClone);
}
function BeAppearOnBeat::onLevelLoadFinished15(%this)
{
	%owner = Owner;
	%this.originalBlendColor = getWords(%owner.getBlendColor(), "0", "2");
	if (!(%owner.getBehavior("BeMountWithOffset")))
	{
		%owner.BaseVelocityAdaptionFactor = "0";
	}
	return;
}
function BeAppearOnBeat::fadeOut(%this)
{
	%owner = Owner;
	%this.fadedOut = "1";
	if (cloning)
	{
		%owner.setVisible("0");
		%owner.setCollisionSuppress("1");
		callNextFrame(%owner, "setCollisionSuppress, true", "2");
	}
	else
	{
		%owner.setAlphaVelocity(-1.0 / fadeoutTime);
		%this.schedule(fadeoutTime * 1000.0, "finishFade");
		%owner.schedule(fadeoutTime * 1000.0 * loseCollisionFactor, "setCollisionSuppress", "1");
	}
	return;
}
function BeAppearOnBeat::finishFade(%this)
{
	%owner = Owner;
	%owner.setAlphaVelocity("0");
	%owner.setBlendAlpha("0");
	%owner.setVisible("0");
	return;
}
function BeAppearOnBeat::onBeat(%this)
{
	%owner = Owner;
	if (beatNumber == $beatNumber)
	{
		%this.currentBeatNumber = $beatNumber;
	}
	else
	{
		if (moreBeatNumbers != "")
		{
			%i = 0;
			while (%i < getWordCount(moreBeatNumbers))
			{
				if (getWord(moreBeatNumbers, %i) $= $beatNumber)
				{
					%this.currentBeatNumber = $beatNumber;
				}
				%i = %i + 1.0;
			}
			break;
		}
		%this.currentBeatNumber = "0";
	}
	if (currentBeatNumber == $beatNumber)
	{
		%this.appear();
	}
	return;
}
function BeAppearOnBeat::appear(%this)
{
	%owner = Owner;
	if (colorMask)
	{
		%appearOnBeatColor = $RHYTHMCOLOR[currentBeatNumber];
		%owner.setBlendColor(getR(%appearOnBeatColor), getG(%appearOnBeatColor), getB(%appearOnBeatColor));
	}
	%owner.setVisible("1");
	%owner.setCollisionSuppress("0");
	%owner.setBlendAlpha("1");
	if (!(cloning))
	{
		%this.schedule(timeUntilFadeOut - fadeoutTime * 1.0 - loseCollisionFactor * 1000.0, "fadeOut");
	}
	else
	{
		if (isEventPending(fadeOutSchedule))
		{
			cancel(fadeOutSchedule);
		}
		%this.fadeOutSchedule = %this.schedule(timeUntilFadeOut * 1000.0, "fadeOut");
		%i = 0;
		while (%i < clonesSet.getCount())
		{
			%obj = clonesSet.getObject(%i);
			%newBlendColor = multVector(%appearOnBeatColor, layerBlendColor);
			%obj.setBlendColor(getR(%newBlendColor), getG(%newBlendColor), getB(%newBlendColor));
			%obj.schedule(mAbs(fadeOutDelay) * 1000.0, "setVisible", "true");
			%obj.schedule(timeUntilFadeOut + fadeOutDelay * 1000.0, "setVisible", "false");
			%i = %i + 1.0;
		}
	}
	return clonesSet.getCount();
}
