// flame.cs.dso
function createFlame(%flameable, %position, %isFixed)
{
	%flame = new Flame(Name : "")
	{
		scenegraph = scenegraph;
		animationName = "fireMaskAnimation";
		size = "13.828 29.297";
		flameable = %flameable;
		isFixed = %isFixed;
	}
	%flame.addDependentBehaviors("BeRotate");
	%flame.setGraphGroup($GROUPS["flame"]);
	%flame.setCollisionActive("0", "1");
	if (!(isFixed))
	{
		%flame.setCollisionSuppress("1");
	}
	%flame.setCollisionPhysics("0", "0");
	%flame.setCollisionPolyCustom("4", "0.086 0.928", "-0.796 0.611", "0.027 -0.952", "0.814 0.613");
	%maskBehavior = %flame.addDependentBehavior("BeMask");
	%maskBehavior.dontCollide = "0";
	%maskBehavior.Layer = "custom";
	%maskBehavior.setTexture(%flame.createFireTexture(), "1");
	%layer = $LAYER[Layer] + layerModification;
	%flame.setLayer(%layer);
	%flame.localFootPoint = "0 0.9";
	%flame.addLinkPoint(localFootPoint);
	%rotateBehavior = %flame.getBehavior("BeRotate");
	%rotateBehavior.autoPlay = "0";
	%position = t2dVectorSub(%position, t2dVectorMult(localFootPoint, t2dVectorScale(%flame.getSize(), "0.5")));
	%flame.setPosition(%position);
	%flame.duration = flameGrowDuration;
	%flame.invDuration = 1.0 / duration;
	%killplayerBehavior = %flame.addDependentBehavior("BeKillPlayer");
	%killplayerBehavior.deathType = "dieBurning";
	%killplayerBehavior.GraphGroup = "None";
	%killplayerBehavior.collisionDuration = "0.3";
	%killplayerBehavior.switchOn();
	subscribeToEvents(%flame, "onLevelLoadFinished40");
	return %flame;
	return %flame;
}
function Flame::onLevelLoadFinished40(%this)
{
	%this.pivot = pivot;
	pivot.originalSize = pivot.getSize();
	pivot.setSize("0", "0");
	%this.originalSize = %this.getSize();
	%this.setSize("0", "0");
	%this.deactivate();
	return;
}
function Flame::createFireTexture(%this)
{
	if (!(isObject(tex_FlameFireClip)))
	{
		new t2dAnimatedSprite(Name : tex_FlameFireClip)
		{
			scenegraph = scenegraph;
			animationName = "fireclipAnimation";
			size = "40.000 40.000";
			Position = "20.000 20.000";
			_behavior0 = "BeTexture	trackMaskFlip	0";
		}
	}
	return tex_FlameFireClip;
	return tex_FlameFireClip;
}
function Flame::activate(%this)
{
	%this.setVisible("1");
	%this.setCollisionSuppress("0");
	pivot.setRotation(-1.0 * camera.getCurrentRotation());
	if (isFixed)
	{
		controller.addFlame(%this);
	}
	return;
}
function Flame::deactivate(%this, %kill)
{
	%this.setVisible("0");
	%this.setCollisionSuppress("1");
	pivot.setAngularVelocity("0");
	if (isFixed)
	{
		controller.removeFlame(%this);
	}
	if (%kill)
	{
		%this.safeDelete();
	}
	return;
}
function Flame::initGrow(%this, %skipAnimation)
{
	if (isFixed)
	{
		%this.targetFactor = "1";
	}
	else
	{
		%this.targetFactor = floatRandom("0.8", "1.2");
	}
	pivot.setRotation(-1.0 * camera.getCurrentRotation());
	if (%skipAnimation)
	{
		%this.activate();
		%this.endGrow();
		return;
	}
	%randomDelay = getRandom("100", "300");
	%this.schedule(%randomDelay, "startGrow");
	return;
}
function Flame::startGrow(%this)
{
	%this.activate();
	%sizeVel = t2dVectorScale(originalSize, targetFactor * invDuration);
	%this.setSizeVelocity(%sizeVel);
	%sizeVel = t2dVectorScale(originalSize, targetFactor * invDuration);
	pivot.setSizeVelocity(%sizeVel);
	%this.schedule(duration * 1000.0, "endGrow");
	return;
}
function Flame::growFurther(%this)
{
	%targetFactor = floatRandom("0.9", "1.1");
	%sizeVel = t2dVectorScale(originalSize, %targetFactor * invDuration);
	%this.setSizeVelocity(%sizeVel);
	%sizeVel = t2dVectorScale(originalSize, %targetFactor * invDuration);
	pivot.setSizeVelocity(%sizeVel);
	%this.schedule(duration * 1000.0, "endGrow", "1");
	return;
}
function Flame::endGrow(%this, %dontResize)
{
	%this.setSizeVelocity("0", "0");
	pivot.setSizeVelocity("0", "0");
	if (%dontResize)
	{
		return;
	}
	%this.setSize(t2dVectorScale(originalSize, targetFactor));
	pivot.setSize(t2dVectorScale(originalSize, targetFactor));
	return;
}
function Flame::initShrink(%this, %skipAnimation)
{
	if (%skipAnimation)
	{
		%this.endShrink();
		return;
	}
	%randomDelay = getRandom("100", "300");
	%this.schedule(%randomDelay, "startShrink");
	return;
}
function Flame::startShrink(%this)
{
	%sizeVel = t2dVectorScale(%this.getSize(), -1.0 * invDuration);
	%this.setSizeVelocity(%sizeVel);
	%sizeVel = t2dVectorScale(pivot.getSize(), -1.0 * invDuration);
	pivot.setSizeVelocity(%sizeVel);
	%this.schedule(duration * 1000.0, "endShrink");
	return;
}
function Flame::endShrink(%this)
{
	%this.deactivate();
	%this.setSizeVelocity("0", "0");
	%this.setSize("0", "0");
	pivot.setSizeVelocity("0", "0");
	pivot.setSize("0", "0");
	return;
}
