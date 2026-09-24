// fadeOnRotation.cs.dso
if (!(isObject(BeFadeOnRotation)))
{
	%template = new BehaviorTemplate(Name : BeFadeOnRotation);
	%template.friendlyName = "FadeOnRotation";
	%template.behaviorType = "LevelTrip";
	%template.description = "this object will fade out if the camera has the same rotaiton as itself";
	%template.addBehaviorField(fadeoutTime, "how long after rotation is finished the object takes to fade out (sec)", float, "1");
	%template.addBehaviorField(fadeOutOnSameRotation, "wheter this object should fade out if the camera has the same rotation or every other case", bool, "1");
	%template.addBehaviorField(onlyDeko, "if the object is used in a back or foreground layer", bool, "0");
	%template.addBehaviorField(onRotation, "if is a deko object, the rotation on wich the object shall fade in (out)", enum, useOwnRotation, "useOwnRotation	0	90	180	270");
	%template.addBehaviorField(onRotation2, "if is a deko object, a second rotation on wich the object shall fade in (out)", enum, None, "None	0	90	180	270");
}
function BeFadeOnRotation::onBehaviorAdd(%this)
{
	%owner = Owner;
	subscribeToEvents(%this, "onLevelLoadFinished1 onLevelLoadFinished onRotationFinish");
	%owner.setCollisionActiveReceive(!(onlyDeko));
	return;
}
function BeFadeOnRotation::onLevelLoadFinished1(%this)
{
	%owner = Owner;
	if (!(onlyDeko))
	{
		%owner.addDependentBehaviors("BeCollide");
	}
	%this.fadeSmoothly = "1";
	%this.faderRotation2 = "666666";
	if (onlyDeko && onRotation != "useOwnRotation")
	{
		%this.faderRotation = onRotation;
		if (onRotation2 $= "None")
		{
		}
		else
		{
		}
		%this.faderRotation2 = onRotation2;
	}
	else
	{
		%ownerRotation = %owner.getRotation();
		if (%owner.getFlipY())
		{
			%ownerRotation = %ownerRotation + 180.0;
		}
		%this.faderRotation = modulo(mRound(%ownerRotation / 90.0), "4") * 90.0;
	}
	if (fadeOnRotationProfile != "")
	{
		return;
	}
	if (fadeOutOnSameRotation)
	{
		%owner.fadeOnRotationProfile = FadeOnRotation1;
	}
	else
	{
		%owner.fadeOnRotationProfile = FadeOnRotation1Reverse;
	}
	return;
}
function BeFadeOnRotation::onLevelLoadFinished(%this)
{
	if (onlyDeko)
	{
		return %this;
	}
	%owner = Owner;
	%maskBehavior = %owner.getBehavior("BeMask");
	if (getLastToken(textureObject, "_") != faderRotation)
	{
		debugWarn("rotation fader" SPC %owner SPC "has wrong texture set on:" SPC %owner.getPosition());
		return;
	}
	if (faderRotation == 0.0)
	{
		%texVelVector = "0 -1";
	}
	else
	{
		if (faderRotation == 90.0)
		{
			%texVelVector = "1 0";
			break;
		}
		if (faderRotation == 180.0)
		{
			%texVelVector = "0 1";
			break;
		}
		if (faderRotation == 270.0)
		{
			%texVelVector = "-1 0";
			break;
		}
		debugWarn("this case shouldnt happen! somethings wrong with roation fader rotation calculation!");
	}
	if (fadeOutOnSameRotation)
	{
		%texVelVector = t2dVectorMult(%texVelVector, "-1 -1");
	}
	%texVelVector = t2dVectorScale(%texVelVector, "5");
	textureObject.setLinearVelocity(%texVelVector);
	if ($allFadeonRotations $= "")
	{
		$allFadeonRotations = new SimGroup(Name : "");
	}
	$allFadeonRotations.add(%this);
	return;
}
function BeFadeOnRotation::onRotationFinish(%this, %switchingOn)
{
	%rotationOnTop = negModulo(-1.0 * camera.getCurrentRotation(), "360");
	%owner = Owner;
	if (fadeOutOnSameRotation)
	{
		if (equalsAngleTolerance(faderRotation, %rotationOnTop, "45") || equalsAngleTolerance(faderRotation2, %rotationOnTop, "45"))
		{
			if (fadedOut)
			{
				return %this;
			}
			%this.fadeOut();
		}
		else
		{
			if (!(fadedOut))
			{
				return %this;
			}
			%this.fadeIn();
		}
	}
	else
	{
		if (equalsAngleTolerance(faderRotation, %rotationOnTop, "45") || equalsAngleTolerance(faderRotation2, %rotationOnTop, "45"))
		{
			if (!(fadedOut))
			{
				return %this;
			}
			%this.fadeIn();
			break;
		}
		if (fadedOut)
		{
			return %this;
		}
		%this.fadeOut();
	}
	if (!(onlyDeko) && !(%switchingOn))
	{
		playGameEventSound(%owner, "FadeOnRotation");
	}
	return;
}
function BeFadeOnRotation::fadeOut(%this)
{
	%owner = Owner;
	%this.fadedOut = "1";
	if (fadeSmoothly)
	{
		%actualAlpha = %owner.getBlendAlpha();
		%actualFadeOutDur = %actualAlpha * fadeoutTime;
		%owner.setAlphaVelocity(-1.0 / fadeoutTime);
		if (finishFadeSchedule)
		{
			cancel(finishFadeSchedule);
		}
		%this.finishFadeSchedule = %this.schedule(%actualFadeOutDur * 1000.0, "finishFade");
		if (toggleCollisionSchedule)
		{
			cancel(toggleCollisionSchedule);
		}
		if (!(%owner.getCollisionSuppress()))
		{
			%this.toggleCollisionSchedule = %this.schedule(%actualFadeOutDur * 630.0, "toggleCollision");
		}
	}
	else
	{
		%owner.setBlendAlpha("0");
		%owner.setCollisionSuppress("1");
		%owner.setEnabled("0");
	}
	return;
}
function BeFadeOnRotation::fadeIn(%this)
{
	%owner = Owner;
	%this.fadedOut = "0";
	%owner.setEnabled("1");
	if (fadeSmoothly)
	{
		%actualAlpha = %owner.getBlendAlpha();
		%actualFadeOutDur = 1.0 - %actualAlpha * fadeoutTime;
		%owner.setAlphaVelocity(1.0 / fadeoutTime);
		if (finishFadeSchedule)
		{
			cancel(finishFadeSchedule);
		}
		%this.finishFadeSchedule = %this.schedule(%actualFadeOutDur * 1000.0, "finishFade");
		if (toggleCollisionSchedule)
		{
			cancel(toggleCollisionSchedule);
		}
		if (%owner.getCollisionSuppress())
		{
			%this.toggleCollisionSchedule = %this.schedule(%actualFadeOutDur * 200.0, "toggleCollision");
		}
	}
	else
	{
		%owner.setBlendAlpha("1");
		%owner.setCollisionSuppress("0");
	}
	return;
}
function BeFadeOnRotation::finishFade(%this)
{
	%owner = Owner;
	%owner.setAlphaVelocity("0");
	if (fadedOut)
	{
		%owner.setBlendAlpha("0");
		%owner.setEnabled("0");
	}
	else
	{
		%owner.setBlendAlpha("1");
	}
	return;
}
function BeFadeOnRotation::toggleCollision(%this)
{
	Owner.setCollisionSuppress(!(Owner.getCollisionSuppress()));
	return;
}
function BeFadeOnRotation::switchOn(%this)
{
	subscribeToEvent(%this, "onRotationFinish");
	%this.onRotationFinish("1");
	%this.fadeSmoothly = "1";
	return;
}
function BeFadeOnRotation::switchOff(%this)
{
	unSubscribeFromEvent(%this, "onRotationFinish");
	%this.fadeSmoothly = "0";
	return;
}
