// selfDiscovery.cs.dso
function PreSelfDiscovery::onAdd(%this)
{
	subscribeToEvents(%this, "onLevelLoadFinished5 onLevelLoadFinished40 onLevelLoadFinished");
	return;
}
function PreSelfDiscovery::onLevelLoadFinished5(%this)
{
	SelfDiscovery::onLevelLoadFinished5(%this);
	return;
}
function PreSelfDiscovery::onLevelLoadFinished40(%this)
{
	SelfDiscovery::onLevelLoadFinished40(%this);
	return;
}
function PreSelfDiscovery::onLevelLoadFinished(%this)
{
	$PRE_SELFDISCOVERY[selfDiscoveryNR] = %this;
	SelfDiscovery::init(%this);
	return;
}
function PreSelfDiscovery::fadeIn(%this)
{
	SelfDiscovery::fadeIn(%this);
	return;
}
function PreSelfDiscovery::fadeOut(%this, %step, %mountToLayerNode)
{
	SelfDiscovery::fadeOut(%this, %step, %mountToLayerNode);
	return;
}
function PreSelfDiscovery::onUpdateFrame(%this)
{
	SelfDiscovery::setFadeValue(%this);
	return;
}
function SelfDiscovery::onAdd(%this)
{
	subscribeToEvents(%this, "onLevelLoadFinished5 onLevelLoadFinished40 onLevelLoadFinished");
	return;
}
function SelfDiscovery::onLevelLoadFinished5(%this)
{
	%i = 0;
	while (%i < %this.getCount())
	{
		%potentialRippedEdge = %this.getObject(%i).getBehavior("BeRippedEdge");
		if (isObject(%potentialRippedEdge))
		{
			%potentialRippedEdge.Immovable = "0";
		}
		%i = %i + 1.0;
	}
	return %this.getCount();
}
function SelfDiscovery::onLevelLoadFinished40(%this)
{
	%i = 0;
	while (%i < %this.getCount())
	{
		%obj = %this.getObject(%i);
		%mountedChilds = %obj.getMountedChildren();
		%j = 0;
		while (%j < getWordCount(%mountedChilds))
		{
			%child = getWord(%mountedChilds, %j);
			if (!(%this.isMember(%child)))
			{
				%this.add(%child);
			}
			%j = %j + 1.0;
		}
		%i = %i + 1.0;
	}
	SelfDiscovery::safeOriginalValues(%this);
	return;
}
function SelfDiscovery::onLevelLoadFinished(%this)
{
	$SELFDISCOVERY[selfDiscoveryNR] = %this;
	%this.init();
	%this.setActive("0");
	%this.setVisibility("0");
	return;
}
function SelfDiscovery::init(%this)
{
	%this.fader = Smoother::createInstance();
	levelGarbageCollector.add(fader);
	%this.fadingTime = "1";
	fader.init(fadingTime, "0", "1", "SMOOTH", "1");
	return;
}
function SelfDiscovery::setVisibility(%this, %visible)
{
	%i = 0;
	while (%i < %this.getCount())
	{
		%obj = %this.getObject(%i);
		if (sdcOriginalVisibility)
		{
			%obj.setVisible(%visible);
		}
		%i = %i + 1.0;
	}
	return %this.getCount();
}
function SelfDiscovery::fadeIn(%this)
{
	%this.fadeStep = "0";
	SelfDiscovery::setAlphaValue(%this, "0");
	SelfDiscovery::setVisibility(%this, "1");
	SelfDiscovery::setActive(%this, "1");
	fader.start();
	subscribeToEvents(%this, "onUpdateFrame");
	return;
}
function SelfDiscovery::fadeOut(%this, %step, %mountToLayerNode)
{
	%this.fadeStep = %step;
	%this.isBonusLevel = environment > 2.0;
	if (isBonusLevel && %step > 1.0)
	{
		return %this;
	}
	%this.mountToLayerNode = %mountToLayerNode;
	SelfDiscovery::setAlphaValue(%this, "1");
	if (%step == 1.0)
	{
		SelfDiscovery::initSpecialObjects(%this);
		%layerBefore = "main";
		%layerAfter = "background_2";
	}
	else
	{
		if (%step == 2.0)
		{
			%layerBefore = "background_2";
			%layerAfter = "background_3";
		}
	}
	%this.startBlending = $LAYER_BLENDING[%layerBefore];
	%this.endBlending = $LAYER_BLENDING[%layerAfter];
	%this.maskLayer = $LAYER[%layerAfter];
	%this.startScalingFactor = 1.0 - $LAYER_SPEEDFACTOR[%layerBefore];
	%this.endScalingFactor = 1.0 - $LAYER_SPEEDFACTOR[%layerAfter];
	%this.layerNode = $LAYER_NODE[%layerAfter];
	if (!(isBonusLevel))
	{
		SelfDiscovery::setLayerValue(%this, maskLayer);
	}
	%this.betweenBlending = endBlending - startBlending;
	%this.betweenScalingFactor = endScalingFactor - startScalingFactor;
	fader.start();
	subscribeToEvents(%this, "onUpdateFrame");
	return;
}
function SelfDiscovery::setLayerValue(%this, %layer)
{
	%i = 0;
	while (%i < %this.getCount())
	{
		%obj = %this.getObject(%i);
		if (isObject(%obj.getBehavior("BeRippedEdge")))
		{
			%obj.setLayer(%layer - 1.0);
		}
		else
		{
			%obj.setLayer(%layer);
		}
		%i = %i + 1.0;
	}
	return %this.getCount();
}
function SelfDiscovery::safeOriginalValues(%this)
{
	%i = 0;
	while (%i < %this.getCount())
	{
		%obj = %this.getObject(%i);
		%obj.sdcOriginalSize = %obj.getSize();
		%obj.sdcOriginalPosition = %obj.getPosition();
		%obj.sdcOriginalBlending = %obj.getBlendColor();
		%obj.sdcOriginalVisibility = %obj.getVisible();
		%i = %i + 1.0;
	}
	return %this.getCount();
}
function SelfDiscovery::initSpecialObjects(%this)
{
	%i = 0;
	while (%i < %this.getCount())
	{
		%obj = %this.getObject(%i);
		%coverageBehavior = %obj.getBehavior("BeSelfDiscoveryCoverage");
		%pulseBehavior = %obj.getBehavior("BePulsatingObject");
		if (isObject(%coverageBehavior))
		{
			%obj.setVisible("0");
			%this.remove(%obj);
			%obj.safeDelete();
			%i = %i - 1.0;
		}
		if (isObject(%pulseBehavior))
		{
			%pulseBehavior.stopPulsating();
			%obj.sdcOriginalSize = %obj.getSize();
		}
		%i = %i + 1.0;
	}
	return %this.getCount();
}
function PostSelfDiscovery::onAdd(%this)
{
	subscribeToEvents(%this, "onLevelLoadFinished5 onLevelLoadFinished40 onLevelLoadFinished");
	return;
}
function PostSelfDiscovery::onLevelLoadFinished5(%this)
{
	SelfDiscovery::onLevelLoadFinished5(%this);
	return;
}
function PostSelfDiscovery::onLevelLoadFinished40(%this)
{
	SelfDiscovery::onLevelLoadFinished40(%this);
	return;
}
function PostSelfDiscovery::onLevelLoadFinished(%this)
{
	$POST_SELFDISCOVERY[selfDiscoveryNR] = %this;
	SelfDiscovery::init(%this);
	SelfDiscovery::setVisibility(%this, "0");
	SelfDiscovery::setActive(%this, "0");
	return;
}
function PostSelfDiscovery::fadeIn(%this)
{
	SelfDiscovery::fadeIn(%this);
	return;
}
function PostSelfDiscovery::fadeOut(%this, %step, %mountToLayerNode)
{
	SelfDiscovery::fadeOut(%this, %step, %mountToLayerNode);
	return;
}
function PostSelfDiscovery::onUpdateFrame(%this)
{
	SelfDiscovery::setFadeValue(%this);
	return;
}
