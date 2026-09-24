// flameable.cs.dso
if (!(isObject(BeFlameable)))
{
	if (!(isTorquePlayer()))
	{
		%template = new BehaviorTemplate(Name : BeFlameable);
	}
	else
	{
		%template = new BeFlameableTemplate(Name : BeFlameable);
	}
	%template.friendlyName = "Flameable";
	%template.behaviorType = "LevelJungle";
	%template.description = "a flameable area that begins to burn on contact with fire or sparks";
	%template.addBehaviorField(breakUp, "if the object shall break up when burning", bool, "0");
	%template.addBehaviorField(burningTime, "how long the object will burn (in sec); 0 is infinite", float, "8");
	%template.addBehaviorField(mustBeVisible, "if the inflame-point must be visible, to spread flames", bool, "0");
	%template.addBehaviorField(autoBurn, "if the flemable shall already burn when level starts", bool, "0");
	%template.addBehaviorField(createFlames, "if the flameable also creates extra flames on top", bool, "1");
	%template.addBehaviorField(texSizeFactor, "the flameables at end of level need to grow, so we need bigger texes", float, "1");
	%template.addBehaviorField(growFlamesBeforeRain, "the flameables in the end-area that shall grow 4 claustrophobia", bool, "0");
}
function BeFlameable::onBehaviorAdd(%this)
{
	%owner = Owner;
	%owner.addDependentBehavior("BeMask");
	%this.states["inflameable"] = "1";
	%this.states["burning"] = "2";
	%this.states["burnedOut"] = "3";
	subscribeToEvents(%this, "onLevelLoadFinished1 onLevelLoadFinished3 onLevelLoadFinished onChangeUseShader");
	return;
}
function BeFlameable::onLevelLoadFinished1(%this)
{
	%owner = Owner;
	%this.maskBehavior = %owner.getBehavior("BeMask");
	maskBehavior.dontCollide = "0";
	if (!(isObject(textureObject)))
	{
		maskBehavior.textureObject = tex_leaves;
	}
	maskBehavior.protoObject = "";
	maskBehavior.cloneTexture("1");
	%this.burnableTex = textureObject;
	burnableTex.removeBehavior(burnableTex.getBehavior("BeMountWithOffset"));
	burnableTex.setPosition(%owner.getPosition());
	burnableTex.setRotation(%owner.getRotation());
	return;
}
function BeFlameable::onLevelLoadFinished3(%this)
{
	%owner = Owner;
	%this.state = states["inflameable"];
	%this.flameGrowDuration = "1.5";
	%this.controller = getFlameController();
	%this.createFireTexture();
	%this.initFlames();
	%this.createBlendMask();
	%this.createBurnedOutTexture();
	%this.multiMaskBehavior = %owner.addDependentBehavior("BeMultiMask");
	multiMaskBehavior.alphaMask = blendMask;
	multiMaskBehavior.secondTex = fireClip;
	multiMaskBehavior.linkAtStart = autoBurn && !(mustBeVisible) && getUseShader();
	%this.createViewTrigger();
	%owner.setCollisionActive("0", "0");
	%owner.setCollisionPhysics("0", "0");
	if (!(autoBurn))
	{
		if (!(mustBeVisible))
		{
			%owner.setCollisionActiveSend("1");
			break;
		}
		%owner.setCollisionActiveSend("0");
	}
	%owner.setGraphGroup($GROUPS["flameable"]);
	%owner.setCollisionGroups($GROUPS["flame"] SPC $GROUPS["spark"]);
	%this.setBehaviorCollisionCallback("1");
	%this.setBehaviorCollisionReceiveCallback("1");
	return;
}
function BeFlameable::onLevelLoadFinished(%this)
{
	if (autoBurn && !(mustBeVisible))
	{
		%this.inflame();
	}
	return;
}
function BeFlameable::createViewTrigger(%this)
{
	%owner = Owner;
	%this.viewTrigger = new t2dTrigger(Name : "")
	{
		scenegraph = scenegraph;
		class = "FlameableTrigger";
		size = %owner.getSize();
		Rotation = %owner.getRotation();
		Position = %owner.getPosition();
		_behavior0 = "BeTrigger";
		Flameable = %this;
	}
	%triggerBehavior = viewTrigger.getBehavior("BeTrigger");
	%triggerBehavior.setTriggerCollisionDetection("CIRCLE");
	%triggerBehavior.setTriggerCollisionGroup("viewWindowTrigger");
	return;
}
function BeFlameable::createBlendMask(%this)
{
	%owner = Owner;
	%this.blendMask = new t2dStaticSprite(Name : "")
	{
		scenegraph = scenegraph;
		imageMap = "fireBlendMaskImageMap";
		frame = "0";
		canSaveDynamicFields = "1";
		_behavior0 = "BeTexture";
	}
	blendMask.setSize(t2dVectorScale(%owner.getSize(), texSizeFactor));
	blendMask.setPosition(%owner.getPosition());
	if (breakUp || Layer != "main")
	{
		blendMask.mount(%owner, "0 0", "0", "1", "1", "1", "0");
	}
	else
	{
		blendMask.setRotation(%owner.getRotation());
	}
	return;
}
function BeFlameable::createFireTexture(%this)
{
	%owner = Owner;
	if (!(isObject(tex_fireClip)))
	{
		new t2dAnimatedSprite(Name : tex_fireClip)
		{
			scenegraph = scenegraph;
			animationName = "fireclipAnimation";
			size = "40.000 40.000";
			Position = "0.000 0.000";
			_behavior0 = "BeTexture	trackMaskFlip	0";
		}
	}
	if (breakUp)
	{
		%this.fireClip = new t2dAnimatedSprite(Name : "")
		{
			scenegraph = scenegraph;
			animationName = animationName;
			size = size;
			Position = Position;
			_behavior0 = "BeTexture	trackMaskFlip	0";
		}
		fireClip.mount(%owner, "0 0", "0", "1", "1", "1", "0");
	}
	else
	{
		%this.fireClip = tex_fireClip;
	}
	return;
}
function BeFlameable::createBurnedOutTexture(%this)
{
	%owner = Owner;
	%this.burnOutTex = new t2dStaticSprite(Name : "")
	{
		scenegraph = scenegraph;
		imageMap = imageMap;
		size = size;
		_behavior0 = "BeTexture	trackMaskFlip	0";
	}
	burnOutTex.setPosition(burnableTex.getPosition());
	burnOutTex.setRotation(burnableTex.getRotation());
	if (breakUp)
	{
		%mwoBehavior = burnOutTex.addDependentBehavior("BeMountWithOffset");
		%mwoBehavior.mother = %owner;
		%mwoBehavior.trackRotation = "1";
		%mwoBehavior.MountOwned = "1";
	}
	return;
}
function BeFlameable::initFlames(%this)
{
	%owner = Owner;
	%distTreshold = 10;
	%this.allFlames = new SimSet(Name : "");
	levelGarbageCollector.add(allFlames);
	if (!(createFlames))
	{
		return %this;
	}
	%i = 0;
	while (%i < %owner.getLinkCount())
	{
		%flamePosition = %owner.getLinkPoint(%i + 1.0);
		%newFlame = createFlame(%this, %flamePosition, "1");
		allFlames.add(%newFlame);
		%i = %i + 1.0;
	}
	%protoFlameable = "flameable_proto_" @ %owner.getImageMap() @ "_" @ %owner.getFrame();
	if (!(isObject(%protoFlameable)))
	{
		warn("there is no proto linkpoint object for flameable:" SPC %protoFlameable);
		return;
	}
	%i = 0;
	while (%i < %protoFlameable.getLinkCount())
	{
		%flamePosition = %owner.getWorldPoint(%protoFlameable.getLocalLinkPoint(%i + 1.0));
		%canExist = 1;
		%j = 0;
		while (%j < %owner.getLinkCount())
		{
			%original = %owner.getLinkPoint(%j + 1.0);
			%distance = t2dVectorDistance(%original, %flamePosition);
			if (%distance < %distTreshold)
			{
				%canExist = 0;
				break;
			}
			%j = %j + 1.0;
		}
		if (%canExist)
		{
			%newFlame = createFlame(%this, %flamePosition);
			allFlames.add(%newFlame);
		}
		%i = %i + 1.0;
	}
	if (breakUp)
	{
		%i = 0;
		while (%i < allFlames.getCount())
		{
			%flame = allFlames.getObject(%i);
			%mwoBehavior = %flame.addDependentBehavior("BeMountWithOffset");
			%mwoBehavior.mother = %owner;
			%mwoBehavior.MountOwned = "1";
			%flame.getBehavior("BeMask").setTexture(fireClip, "1");
			%i = %i + 1.0;
		}
	}
	return allFlames.getCount();
}
function BeFlameable::inflame(%this)
{
	%owner = Owner;
	%this.startInflameAnimation();
	%this.state = states["burning"];
	%skipGrowing = !(isInView);
	%i = 0;
	while (%i < allFlames.getCount())
	{
		%flame = allFlames.getObject(%i);
		%flame.initGrow(%skipGrowing);
		%i = %i + 1.0;
	}
	if (isInView)
	{
		%this.addFlamesToController();
		%this.adaptVolume("1");
	}
	if (isInView)
	{
		%owner.setCollisionActiveReceive("1");
	}
	viewTrigger.setCollisionActiveReceive("1");
	if (%owner.getName() $= "beeHive")
	{
		%owner.disableHive();
	}
	if (burningTime > 0.0)
	{
		safeSchedule(burningTime * 1000.0, %this, "extinguish");
	}
	return;
}
function BeFlameable::growFurther(%this)
{
	%i = 0;
	while (%i < allFlames.getCount())
	{
		%flame = allFlames.getObject(%i);
		%flame.growFurther();
		%i = %i + 1.0;
	}
	return allFlames.getCount();
}
function BeFlameable::startInflameAnimation(%this)
{
	%owner = Owner;
	if (getUseShader())
	{
		multiMaskBehavior.alphaMask = blendMask;
		multiMaskBehavior.secondTex = fireClip;
		multiMaskBehavior.thirdTex = burnOutTex;
		multiMaskBehavior.init();
	}
	if (autoBurn && !(mustBeVisible))
	{
		%this.finishInflameAnimation();
		return;
	}
	%fadeSpeed = 1.0 / flameGrowDuration;
	blendMask.setBlendAlpha("0");
	blendMask.setAlphaVelocity(%fadeSpeed);
	burnOutTex.setBlendAlpha("0");
	burnOutTex.setAlphaVelocity(%fadeSpeed);
	if (getUseShader())
	{
		%this.schedule(flameGrowDuration * 1000.0 + 100.0, "finishInflameAnimation");
	}
	else
	{
		%this.finishInflameAnimation();
	}
	return;
}
function BeFlameable::finishInflameAnimation(%this)
{
	%owner = Owner;
	burnOutTex.setBlendAlpha("1");
	burnOutTex.setAlphaVelocity("0");
	blendMask.setBlendAlpha("1");
	blendMask.setAlphaVelocity("0");
	if (getUseShader())
	{
		%owner.getBehavior("BeMask").setTexture(burnOutTex, "1");
		multiMaskBehavior.setThirdTex("");
	}
	else
	{
		%owner.getBehavior("BeMask").setTexture(fireClip, "1");
	}
	return;
}
function BeFlameable::extinguish(%this)
{
	%owner = Owner;
	if (breakUp)
	{
		%this.breakUp();
		%this.adaptVolume(-1.0);
		return;
	}
	%this.startExtinguishAnimation();
	%owner.setCollisionActiveReceive("0");
	%skipShrink = !(isInView);
	%i = 0;
	while (%i < allFlames.getCount())
	{
		%flame = allFlames.getObject(%i);
		%flame.initShrink(%skipShrink);
		%i = %i + 1.0;
	}
	%this.schedule(flameGrowDuration * 1000.0 + 350.0, "finishExtinguish");
	return;
}
function BeFlameable::startExtinguishAnimation(%this)
{
	if (getUseShader())
	{
		%fadeSpeed = 1.0 / flameGrowDuration;
		blendMask.setAlphaVelocity(-1.0 * %fadeSpeed);
	}
	%this.schedule(flameGrowDuration * 1000.0, "finishExtinguishAnimation");
	return;
}
function BeFlameable::finishExtinguishAnimation(%this)
{
	%owner = Owner;
	multiMaskBehavior.unlink();
	blendMask.setAlphaVelocity("0");
	blendMask.setBlendAlpha("0");
	if (!(getUseShader()))
	{
		%owner.getBehavior("BeMask").setTexture(burnOutTex, "1");
	}
	return;
}
function BeFlameable::finishExtinguish(%this)
{
	%this.state = states["burnedOut"];
	if (isInView)
	{
		%this.removeFlamesFromController();
		%this.adaptVolume(-1.0);
	}
	if (burningTime > 0.0)
	{
		%this.schedule(burningTime * 700.0, "startRenew");
	}
	return;
}
function BeFlameable::startRenew(%this)
{
	%owner = Owner;
	if (getUseShader())
	{
		%owner.getBehavior("BeMask").setTexture(burnableTex, "1");
		multiMaskBehavior.alphaMask = "";
		multiMaskBehavior.secondTex = burnOutTex;
		multiMaskBehavior.thirdTex = "";
		multiMaskBehavior.init();
		burnOutTex.setBlendAlpha("1");
		burnOutTex.setAlphaVelocity(-1.0 / flameGrowDuration);
	}
	%this.schedule(flameGrowDuration * 1000.0, "finishRenew");
	return;
}
function BeFlameable::finishRenew(%this)
{
	%owner = Owner;
	multiMaskBehavior.unlink();
	burnOutTex.setAlphaVelocity("0");
	burnOutTex.setBlendAlpha("1");
	if (!(getUseShader()))
	{
		%owner.getBehavior("BeMask").setTexture(burnableTex, "1");
	}
	if (!(mustBeVisible) || isInView)
	{
		%owner.setCollisionActiveSend("1");
	}
	%this.state = states["inflameable"];
	return;
}
function BeFlameable::onChangeUseShader(%this)
{
	%owner = Owner;
	if (state $= states["burning"])
	{
		if (getUseShader())
		{
			multiMaskBehavior.alphaMask = blendMask;
			multiMaskBehavior.secondTex = fireClip;
			multiMaskBehavior.thirdTex = "";
			multiMaskBehavior.init();
			%owner.getBehavior("BeMask").setTexture(burnOutTex, "1");
		}
		else
		{
			multiMaskBehavior.unlink();
			%owner.getBehavior("BeMask").setTexture(fireClip, "1");
		}
	}
	else
	{
		if (state $= states["burnedOut"])
		{
			multiMaskBehavior.unlink();
			%owner.getBehavior("BeMask").setTexture(burnOutTex, "1");
		}
	}
	return;
}
function BeFlameable::breakUp(%this)
{
	%owner = Owner;
	viewTrigger.delete();
	%owner.removeBehavior("BeCollide");
	%owner.setImmovable("0");
	%owner.setCollisionSuppress("1");
	%parent = %owner.getMountedParent();
	if (isObject(%parent))
	{
		%parent.removeBehavior("BeCollide");
		%parent.setImmovable("0");
		%parent.setCollisionSuppress("1");
		%objectToMove = %parent;
	}
	else
	{
		%objectToMove = %owner;
	}
	%objectToMove.addDependentBehavior("BeMoving");
	%objectToMove.setAngularVelocity(getRandom("20", "40") * mRound(getRandom()) * 2.0 - 1.0);
	%owner.viewSafeDelete("1.3", "1", "1");
	%this.adaptVolume(-1.0);
	return;
}
function BeFlameable::switchOn(%this)
{
	viewTrigger.setEnabled("1");
	return;
}
function BeFlameable::switchOff(%this)
{
	viewTrigger.setEnabled("0");
	return;
}
function BeFlameable::adaptVolume(%this, %changed)
{
	if (%changed > 0.0)
	{
		if (influencedSound)
		{
			return %this;
		}
		%this.influencedSound = "1";
	}
	else
	{
		if (!(influencedSound))
		{
			return %this;
		}
		%this.influencedSound = "0";
	}
	$FLAMEABLESBURNING = $FLAMEABLESBURNING + %changed;
	%volume = mPow(t2dGetMin($FLAMEABLESBURNING / 10.0, "1"), "1.5");
	if (%changed > 0.0 && $FLAMEABLESBURNING == 1.0)
	{
		$FLAMESOUNDHANDLE = playHandleEventSound("Inferno", "Inferno", %volume, "1");
		debugEcho("starting fire sound" SPC %volume);
	}
	else
	{
		if (%changed < 0.0 && $FLAMEABLESBURNING == 0.0)
		{
			alxStop($FLAMESOUNDHANDLE);
			debugEcho("stopping fire sound");
			break;
		}
		alxSourcef($FLAMESOUNDHANDLE, AL_GAIN, %volume);
		debugEcho("adjusting fire sound" SPC %volume);
	}
	return;
}
function FlameableTrigger::onEnter(%this, )
{
	%flameable = Flameable;
	%flameable.isInView = "1";
	if (mustBeVisible)
	{
		if (state == states["inflameable"])
		{
			if (autoBurn)
			{
				%flameable.schedule("1500", "inflame");
				break;
			}
			Owner.setCollisionActiveSend("1");
		}
	}
	if (state == states["burning"])
	{
		Owner.setCollisionActiveReceive("1");
		%flameable.addFlamesToController();
		%flameable.adaptVolume("1");
	}
	return;
}
function FlameableTrigger::onLeave(%this, )
{
	%flameable = Flameable;
	%flameable.isInView = "0";
	if (mustBeVisible)
	{
		Owner.setCollisionActiveSend("0");
	}
	Owner.setCollisionActiveReceive("0");
	if (state == states["burning"])
	{
		%flameable.removeFlamesFromController();
		%flameable.adaptVolume(-1.0);
	}
	return;
}
