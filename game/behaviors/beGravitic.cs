// beGravitic.cs.dso
if (!(isObject(BeGravitic)))
{
	%template = new BehaviorTemplate(Name : BeGravitic);
	%template.friendlyName = "Gravitic";
	%template.behaviorType = "Physics";
	%template.description = "assigns gravity to this object";
}
function BeGravitic::onBehaviorAdd(%this)
{
	%owner = Owner;
	subscribeToEvents(%this, "onRotationFinish onRotationStart onLevelLoadFinished20");
	addShrinkerBehavior(%owner);
	%owner.setMaxLinearVelocity(MaxLinearVelocity);
	%owner.setMaxAngularVelocity(MaxAngularVelocity);
	if (!(isPlayer(%owner)))
	{
		%owner.setMaxLinearVelocity(MaxLinearVelocity * 0.800000011920929);
	}
	if (%owner.getBehavior("BeTrampolineBranch"))
	{
		%owner.setMaxLinearVelocity("500");
	}
	%this.applyGravity();
	return;
}
function BeGravitic::onBehaviorRemove(%this)
{
	unSubscribeFromEvents(%this, "onRotationFinish onRotationStart onLevelLoadFinished20");
	%this.stopGravity();
	return;
}
function BeGravitic::onLevelLoadFinished20(%this)
{
	Owner.escapeSwitch = "1";
	addShrinkerBehavior(Owner);
	return;
}
function addShrinkerBehavior(%obj)
{
	%obj.addDependentBehavior("BeShrinker");
	if (!(shrinkChildren))
	{
		return %obj.getBehavior("BeShrinker");
	}
	%kids = %obj.getMountedChildren();
	%i = 0;
	while (%i < getWordCount(%kids))
	{
		%kid = getWord(%kids, %i);
		if (!(dontShrink))
		{
			addShrinkerBehavior(%kid);
		}
		%i = %i + 1.0;
	}
	return getWordCount(%kids);
}
function BeGravitic::onRotationFinish(%this)
{
	%owner = Owner;
	%this.applyGravity();
	%owner.setLinearVelocity(rotateVector(linearVel, camera.getCurrentRotation() - camera.getLastRotation()));
	%owner.setAngularVelocity(angularVel);
	return;
}
function BeGravitic::onRotationStart(%this)
{
	%owner = Owner;
	%owner.linearVel = %owner.getLinearVelocity();
	%owner.angularVel = %owner.getAngularVelocity();
	%this.stopGravity();
	return;
}
function BeGravitic::stopGravity(%this)
{
	%owner = Owner;
	%owner.stopConstantForce();
	%owner.setAtRest();
	return;
}
function BeGravitic::resetSpeed(%this)
{
	%owner = Owner;
	%owner.linearVel = "0 0";
	%owner.angularVel = "0";
	return;
}
function BeGravitic::applyGravity(%this)
{
	%owner = Owner;
	%gravitalDirection = "0" SPC gravity;
	if (isObject(camera))
	{
		%gravitalDirection = absRot(%gravitalDirection);
	}
	%owner.setConstantForce(%gravitalDirection);
	%owner.setGraviticConstantForce("1");
	return;
}
function BeGravitic::switchOff(%this)
{
	%this.stopGravity();
	unSubscribeFromEvents(%this, "onRotationFinish onRotationStart");
	return;
}
function BeGravitic::switchOn(%this)
{
	debugEcho("switching on a gravitiv object" SPC %this);
	if (!(camera.getIsRotating()))
	{
		%this.applyGravity();
	}
	subscribeToEvents(%this, "onRotationFinish onRotationStart");
	return;
}
