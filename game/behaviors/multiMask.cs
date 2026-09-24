// multiMask.cs.dso
if (!(isObject(BeMultiMask)))
{
	%template = new BehaviorTemplate(Name : BeMultiMask);
	%template.friendlyName = "Multi Mask";
	%template.behaviorType = "Visual";
	%template.description = "add more textures to a mask";
	%template.addBehaviorField(alphaMask, "alphaMask object (alpha 1 shows 2nd texture, alpha 0 shows base texture)", object, null, t2dSceneObject);
	%template.addBehaviorField(repeatAlphaMask, "repeat or clamp", bool, "0");
	%template.addBehaviorField(secondTex, "other texture, if alphaMask is omitted this tex is blended directly with baseTex", object, null, t2dSceneObject);
	%template.addBehaviorField(repeatSecondTex, "repeat or clamp", bool, "1");
	%template.addBehaviorField(thirdTex, "if alphaMask is valid, this tex is blended directly with baseTex. else its not used", object, null, t2dSceneObject);
	%template.addBehaviorField(repeatThirdTex, "repeat or clamp", bool, "1");
	%template.addBehaviorField(linkAtStart, "...", bool, "1");
}
function BeMultiMask::onBehaviorAdd(%this)
{
	Owner.addDependentBehavior("BeMask");
	subscribeToEvent(%this, "onLevelLoadFinished40");
	return;
}
function BeMultiMask::onLevelLoadFinished40(%this)
{
	%this.init();
	if (!(linkAtStart))
	{
		%this.unlink();
	}
	return;
}
function BeMultiMask::init(%this)
{
	%owner = Owner;
	if (isObject(secondTex))
	{
		if (isObject(alphaMask))
		{
			%owner.setTexObjectName(alphaMask, "1");
			%owner.setRepeatTexture(repeatAlphaMask, "1");
			%owner.setTexObjectName(secondTex, "2");
			%owner.setRepeatTexture(repeatSecondTex, "2");
			if (isObject(thirdTex))
			{
				%owner.setTexObjectName(thirdTex, "3");
				%owner.setRepeatTexture(repeatThirdTex, "3");
			}
			break;
		}
		%owner.setTexObjectName(secondTex, "1");
		%owner.setRepeatTexture(repeatSecondTex, "1");
		if (isObject(thirdTex))
		{
			%owner.setTexObjectName(thirdTex, "2");
			%owner.setRepeatTexture(repeatThirdTex, "2");
		}
	}
	return;
}
function BeMultiMask::link(%this)
{
	%owner = Owner;
	if (isObject(secondTex))
	{
		if (isObject(alphaMask))
		{
			%owner.setRenderTexture("1", "1");
			%owner.setRenderTexture("1", "2");
			if (isObject(thirdTex))
			{
				%owner.setRenderTexture("1", "3");
			}
			break;
		}
		%owner.setRenderTexture("1", "1");
		if (isObject(thirdTex))
		{
			%owner.setRenderTexture("1", "2");
		}
	}
	return;
}
function BeMultiMask::unlink(%this)
{
	%owner = Owner;
	%owner.setRenderTexture("0", "1");
	%owner.setRenderTexture("0", "2");
	%owner.setRenderTexture("0", "3");
	return;
}
function BeMultiMask::setSecondTex(%this, %tex, %repeat)
{
	%owner = Owner;
	if (isObject(%tex))
	{
		%this.secondTex = %tex;
		if (%repeat $= "")
		{
		}
		else
		{
		}
		%this.repeatSecondTex = %repeat;
		%owner.setTexObjectName(secondTex, "2");
		%owner.setRepeatTexture(repeatSecondTex, "2");
	}
	else
	{
		%this.secondTex = "";
		%owner.setRenderTexture("0", "2");
	}
	return;
}
function BeMultiMask::setThirdTex(%this, %tex, %repeat)
{
	%owner = Owner;
	if (isObject(%tex))
	{
		%this.thirdTex = %tex;
		if (%repeat $= "")
		{
		}
		else
		{
		}
		%this.repeatThirdTex = %repeat;
		%owner.setTexObjectName(thirdTex, "3");
		%owner.setRepeatTexture(repeatThirdTex, "3");
	}
	else
	{
		%this.thirdTex = "";
		%owner.setRenderTexture("0", "3");
	}
	return;
}
