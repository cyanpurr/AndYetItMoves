// pulsatingObject.cs.dso
if (!(isObject(BePulsatingObject)))
{
	if (!(isTorquePlayer()))
	{
		%template = new BehaviorTemplate(Name : BePulsatingObject);
	}
	else
	{
		%template = new BePulsatingObjectTemplate(Name : BePulsatingObject);
	}
	%template.friendlyName = "Pulsating Object";
	%template.behaviorType = "Visual";
	%template.description = "this object will grow and shrink according to a pulsator: the size will be determined like this: original size + syncValue * original size";
	%template.addBehaviorField(Pulsator, "the pulsator to get the pulse from", object, null, t2dSceneObject);
	%template.addBehaviorField(whichSide, "determines which side gets pulsated: -1=proportional; 0=width; 1=height, 2=(width invHeight), 3=(invWidth height), 4=invProportional ", int, -1.0);
	%template.addBehaviorField(useLinkPoint, "if a linkpoint shall be used to pivot-resize (number)", integer, "0");
}
function BePulsatingObject::onBehaviorAdd(%this)
{
	subscribeToEvents(%this, "collectObjectsInBehavior onLevelLoadFinished12 onLevelLoadFinished20 onLevelLoadFinished30");
	return;
}
function BePulsatingObject::onLevelLoadFinished12(%this)
{
	%owner = Owner;
	if (!(isObject(Pulsator)))
	{
		debugWarn("BePulsatingObject" SPC %this SPC "without pulsator (" SPC Pulsator SPC ") at position " SPC %owner.getPosition() SPC " with behavior list " SPC %owner.getRealBehaviorList() SPC "! -> removing behavior");
		%maskInfo = %owner.getBehavior(BeMask);
		if (%maskInfo)
		{
			debugWarn("its mask behavior has texObject:" SPC textureObject NL "protoObj:" SPC protoObject NL "layer:" SPC Layer);
		}
		%owner.removeBehavior(%this);
		return;
	}
	%this.originalSize = %owner.getSize();
	%this.originalPosition = %owner.getPosition();
	%this.objectIsMounted = isObject(%owner.getBehavior("BeMountWithOffset"));
	Pulsator.AddObject(%this);
	return;
}
function BePulsatingObject::onLevelLoadFinished20(%this)
{
	%owner = Owner;
	if (useLinkPoint > 0.0 && objectIsMounted)
	{
		%this.mountBehavior = %owner.getBehavior("BeMountWithOffset");
		%this.mountMother = mother;
		%this.motherLocalLinkPoint = mountMother.getLocalPoint(%owner.getLinkPoint(useLinkPoint));
		%owner.setUpdateMountOffsetFromVelocity("1");
	}
	return;
}
function BePulsatingObject::onLevelLoadFinished30(%this)
{
	%owner = Owner;
	%owner.setMountedCollidesNotImmovable();
	return;
}
