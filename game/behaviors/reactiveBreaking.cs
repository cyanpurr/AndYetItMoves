// reactiveBreaking.cs.dso
if (!(isObject(BeReactiveBreaking)))
{
	if (!(isTorquePlayer()))
	{
		%template = new BehaviorTemplate(Name : BeReactiveBreaking);
	}
	else
	{
		%template = new BeReactiveBreakingTemplate(Name : BeReactiveBreaking);
	}
	%template.friendlyName = "ReactiveBreaking";
	%template.behaviorType = "CommonObjects";
	%template.description = "this object will break out on collission if specified weight and speed are met, USER MUST MANUALLY ADD DELAYED BREAKING if they need that";
	if (!(isTorquePlayer()))
	{
		%template.addBehaviorField(criticalWeight, "the weight an object needs to break through", float, "1.5");
		%template.addBehaviorField(criticalSpeed, "the speed the object needs to break through", float, "100");
		%template.addBehaviorField(beginShakeFactor, "at what percent of the criticalspeed the object shoudl start to shake", float, "0.2");
		%template.addBehaviorField(timeToShake, "how long the object should shake maximum on a weak hit ", float, "1");
		%template.addBehaviorField(shakeOffset, "how far the object should move away from origin (maximum) on shake", float, "2");
		%template.addBehaviorField(playerMustStandOn, "if quarries out only if player stands on owner", bool, "0");
		%template.addBehaviorField(shakeAudioProfile, "the sound this object should play during shaking", string, "");
		%template.addBehaviorField(autoPlay, "wheter or not this behavior works out of the box", bool, "1");
	}
}
function BeReactiveBreaking::onBehaviorAdd(%this)
{
	%owner = Owner;
	%owner.addDependentBehavior("BeBreaking");
	subscribeToEvents(%this, "onLevelLoadFinished");
	return;
}
function BeReactiveBreaking::onLevelLoadFinished(%this)
{
	%this.init();
	Owner.escapeSwitch = "1";
	if (!(autoPlay))
	{
		%this.switchOff();
	}
	return;
}
function BeReactiveBreaking::init(%this)
{
	%owner = Owner;
	%owner.setCollisionActive("0", "1");
	%this.setBehaviorCollisionReceiveCallback("1");
	%owner.setCollisionGroups($GROUPS["player"] SPC $GROUPS["moving"]);
	%this.isCheckingForPlayerDeath = "0";
	%this.shaker = createShaker();
	%this.criticalMix = criticalSpeed * 1.2200000286102295 * criticalWeight;
	return;
}
function BeReactiveBreaking::checkPlayerDeath(%this, %normal)
{
	if (isDead)
	{
		%variation = t2dVectorScale(%normal, shakeOffset);
		shaker.init(Owner, timeToShake, %variation, "INV_LONGSINC");
		shaker.start();
		%this.isCheckingForPlayerDeath = "0";
	}
	else
	{
		%this.behaviorQuarry();
		if (quarriedOut)
		{
			%this.setBehaviorCollisionReceiveCallback("0");
		}
	}
	return;
}
function BeReactiveBreaking::setCriticalSpeed(%this, %criticalSpeed)
{
	if (%criticalSpeed != "")
	{
		%this.criticalSpeed = %criticalSpeed;
	}
	return;
}
function BeReactiveBreaking::behaviorQuarry(%this)
{
	%owner = Owner;
	if (!(quarryBehavior))
	{
		%this.quarryBehavior = %owner.getBehavior("BeGroupedBreaking");
	}
	if (!(quarryBehavior))
	{
		%this.quarryBehavior = %owner.getBehavior("BeDelayedBreaking");
	}
	if (!(quarryBehavior))
	{
		%this.quarryBehavior = %owner.getBehavior("BeBreaking");
	}
	quarryBehavior.quarry();
	return;
}
function BeReactiveBreaking::switchOn(%this)
{
	if (!(quarriedOut))
	{
		Owner.setCollisionSuppress("0");
	}
	return;
}
function BeReactiveBreaking::switchOff(%this)
{
	if (!(quarriedOut))
	{
		Owner.setCollisionSuppress("1");
	}
	return;
}
