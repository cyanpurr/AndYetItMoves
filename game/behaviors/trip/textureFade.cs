// textureFade.cs.dso
if (!(isObject(BeTextureFade)))
{
	%template = new BehaviorTemplate(Name : BeTextureFade);
	%template.friendlyName = "Texture Fade Object";
	%template.behaviorType = "LevelTrip";
	%template.description = "fades from one Texture to another via an alpha mask";
	%template.addBehaviorField(fadeTexture, "Texture that will vanish with the alphaMask", object, null, t2dSceneObject);
	%template.addBehaviorField(fadeMask, "alphaMask", object, "fadeMask", t2dSceneObject);
}
function BeTextureFade::onBehaviorAdd(%this)
{
	if (!(Owner.getBehavior("BeMask")))
	{
		Owner.removeBehavior(%this);
	}
	subscribeToEvents(%this, "onLevelLoadFinished2_5 onChangeUseShader");
	return;
}
function BeTextureFade::onLevelLoadFinished2_5(%this)
{
	%owner = Owner;
	%maskBehavior = %owner.getBehavior("BeMask");
	if (!(%maskBehavior))
	{
		debugWarn("fadeTexture without mask behavior");
		return;
	}
	if (!(isObject(fadeTexture)))
	{
		debugEcho("Mask: texture object with invalid image/animation:" SPC fadeTexture);
		return;
	}
	if (!(fadeTexture.getBehavior("BeTexture")))
	{
		debugWarn("fadeTexture has no texture behavior");
		return;
	}
	if (findWord($PARALAXLAYER_ENUM, Layer))
	{
		if (parallaxScrollTexture)
		{
			if (!(isObject(protoObject)))
			{
				%newName = fadeTexture.getName() @ "_" @ Layer;
				if (!(isObject(%newName)))
				{
					%newTexture = fadeTexture.createCopy(%newName);
					%newTexture.setName(%newName);
				}
				else
				{
					%newTexture = %newName;
				}
				%this.fadeTexture = %newTexture;
			}
			fadeTexture.getBehavior("BeTexture").Layer = Layer;
		}
	}
	subscribeToEvents(%this, "onLevelLoadFinished20");
	return;
}
function BeTextureFade::onLevelLoadFinished20(%this)
{
	%owner = Owner;
	%this.linkFadeTextures("1");
	fadeObjects.add(Owner);
	if (!(autoPlay))
	{
		%this.linkFadeTextures("0");
		%this.setFadeTexAsMain();
	}
	return;
}
function BeTextureFade::linkFadeTextures(%this, %doLink)
{
	%owner = Owner;
	if (%doLink)
	{
		if (!(getUseShader()))
		{
			%owner.setTexObjectName(fadeTexture, "1");
			%owner.setRepeatTexture("1", "1");
			break;
		}
		%owner.setTexObjectName(fadeMask, "1");
		%owner.setRepeatTexture("0", "1");
		%owner.setTexObjectName(fadeTexture, "2");
		%owner.setRepeatTexture("1", "2");
	}
	%owner.setRenderTexture(%doLink, "1");
	if (getUseShader())
	{
		%owner.setRenderTexture(%doLink, "2");
	}
	else
	{
		%owner.setRenderTexture("0", "2");
	}
	return;
}
function BeTextureFade::onChangeUseShader(%this)
{
	%owner = Owner;
	if (isRunning)
	{
		%this.linkFadeTextures("1");
	}
	return;
}
function BeTextureFade::setFadeTexAsMain(%this, %doLink)
{
	%owner = Owner;
	%owner.setTexObjectName(fadeTexture);
	return;
}
function BeTextureFade::resetMainTex(%this, %doLink)
{
	%owner = Owner;
	%owner.setTexObjectName(textureObject);
	return;
}
if (!(isObject(BeTextureFadeController)))
{
	%template = new BehaviorTemplate(Name : BeTextureFadeController);
	%template.friendlyName = "Texture Fade Controller";
	%template.behaviorType = "LevelTrip";
	%template.description = "controlls the fading process";
	%template.addBehaviorField(fadeMask, "this mask will be mounted on player and resized over time, resp with help of triggers", object, null, t2dSceneObject);
	%template.addBehaviorField(startMinSizeFactor, "how much of se original masksize should be preserved", float, "0.8");
	%template.addBehaviorField(shrinkVel, "how fast it shrinks (x/sec)", float, "50");
	%template.addBehaviorField(shrinkDur, "long one shrink cycle lasts (sec)", float, "1");
	%template.addBehaviorField(growVel, "how fast it grows (x/sec)", float, "80");
	%template.addBehaviorField(growDur, "long one grow cycle lasts (sec)", float, "0.25");
	%template.addBehaviorField(autoPlay, "if fadecontroller should be active right on levelstart, or not", bool, "0");
}
function BeTextureFadeController::onBehaviorAdd(%this)
{
	subscribeToEvents(%this, "onLevelLoadFinished2 onLevelLoadFinished");
	return;
}
function BeTextureFadeController::onLevelLoadFinished2(%this)
{
	%owner = Owner;
	%owner.setName("textureFadeController");
	%owner.fadeObjects = new SimSet(Name : "");
	levelGarbageCollector.add(fadeObjects);
	%this.maskAspectRatio = fadeMask.getSizeY() / fadeMask.getSizeX();
	%this.origMaskSize = fadeMask.getSize();
	%this.minSizeFactor = startMinSizeFactor;
	%this.curMinSize = t2dVectorScale(origMaskSize, minSizeFactor);
	%this.isRunning = "0";
	return;
}
function BeTextureFadeController::onLevelLoadFinished(%this)
{
	if (autoPlay)
	{
		%this.start();
	}
	return;
}
function BeTextureFadeController::start(%this)
{
	%owner = Owner;
	if (!(fadeMask.getIsMounted()))
	{
		fadeMask.mount(camera, "0 0", "0", "0", "1");
	}
	fadeMask.setVisible("0");
	%this.startAlphaFade();
	if (!(autoPlay))
	{
		%i = 0;
		while (%i < fadeObjects.getCount())
		{
			%fadeBehavior = fadeObjects.getObject(%i).getBehavior("BeTextureFade");
			%fadeBehavior.linkFadeTextures("1");
			%fadeBehavior.resetMainTex();
			%i = %i + 1.0;
		}
	}
	%this.isRunning = "1";
	%this.shrink();
	return;
}
function BeTextureFadeController::shrink(%this)
{
	%owner = Owner;
	%sizeDiff = t2dVectorSub(t2dVectorScale(curMinSize, 2.0 - camera.getZoom()), fadeMask.getSize());
	if (getX(curMinSize) == 0.0)
	{
		%sizeVel = t2dVectorScale(%sizeDiff, 1.0 / shrinkDur);
		fadeMask.setSizeVelocity(%sizeVel);
		%i = 0;
		while (%i < fadeObjects.getCount())
		{
			fadeTexture.setAlphaVelocity(-1.0);
			%i = %i + 1.0;
		}
		%this.schedule(shrinkDur * 1000.0, "endFade");
	}
	else
	{
		if (getX(%sizeDiff) > 0.0)
		{
			%this.grow();
			break;
		}
		%targetFactor = min(shrinkVel / mAbs(getX(%sizeDiff)) / shrinkDur, "1");
		%sizeVel = t2dVectorScale(%sizeDiff, %targetFactor / shrinkDur);
		fadeMask.setSizeVelocity(%sizeVel);
		%this.schedule(shrinkDur * 1000.0, "grow");
	}
	if (alphaControlTexture.getBlendAlpha() <= minSizeFactor)
	{
		if (isAlphaFading)
		{
			%this.stopAlphaFade();
		}
	}
	else
	{
		if (!(isAlphaFading))
		{
			%this.startAlphaFade();
		}
	}
	return;
}
function BeTextureFadeController::grow(%this)
{
	fadeMask.setSizeVelocity(growVel SPC growVel * maskAspectRatio);
	%this.schedule(growDur * 1000.0, "shrink");
	return;
}
function BeTextureFadeController::endFade(%this)
{
	%owner = Owner;
	%this.stopAlphaFade("1");
	%i = 0;
	while (%i < fadeObjects.getCount())
	{
		fadeObjects.getObject(%i).getBehavior("BeTextureFade").linkFadeTextures("0");
		%i = %i + 1.0;
	}
	fadeMask.setSizeVelocity("0 0");
	fadeMask.dismount();
	fadeMask.setEnabled("0");
	%this.isRunning = "0";
	return;
}
function BeTextureFadeController::stopAlphaFade(%this, %reset)
{
	%this.isAlphaFading = "0";
	%owner = Owner;
	%i = 0;
	while (%i < fadeObjects.getCount())
	{
		%fadeBehavior = fadeObjects.getObject(%i).getBehavior("BeTextureFade");
		fadeTexture.setAlphaVelocity("0");
		if (%reset)
		{
			fadeTexture.setBlendAlpha("1");
		}
		%i = %i + 1.0;
	}
	return fadeObjects.getCount();
}
function BeTextureFadeController::startAlphaFade(%this, )
{
	%owner = Owner;
	%this.isAlphaFading = "1";
	%i = 0;
	while (%i < fadeObjects.getCount())
	{
		%fadeBehavior = fadeObjects.getObject(%i).getBehavior("BeTextureFade");
		fadeTexture.setAlphaVelocity(-0.009999999776482582);
		if (!(isObject(alphaControlTexture)))
		{
			%owner.alphaControlTexture = fadeTexture;
		}
		%i = %i + 1.0;
	}
	return fadeObjects.getCount();
}
function BeTextureFadeController::switchOn(%this)
{
	%this.start();
	return;
}
if (!(isObject(BeTextureFadeTrigger)))
{
	%template = new BehaviorTemplate(Name : BeTextureFadeTrigger);
	%template.friendlyName = "Texture Fade Trigger";
	%template.behaviorType = "LevelTrip";
	%template.description = "controlls the fading process";
	%template.addBehaviorField(minSizeFactor, "how much of se original masksize should be preserved; -1 makes a complete fadeout: shrinks fast", float, -1.0);
}
function BeTextureFadeTrigger::onBehaviorAdd(%this)
{
	Owner.addDependentBehavior("BeTrigger");
	return;
}
function BeTextureFadeTrigger::onEnter(%this, )
{
	%controllerBehavior = textureFadeController.getBehavior("BeTextureFadeController");
	if (minSizeFactor < 0.0)
	{
		%controllerBehavior.curMinSize = "0 0";
	}
	else
	{
		if (minSizeFactor < minSizeFactor)
		{
			%controllerBehavior.minSizeFactor = minSizeFactor;
			%controllerBehavior.curMinSize = t2dVectorScale(origMaskSize, minSizeFactor);
		}
	}
	return;
}
