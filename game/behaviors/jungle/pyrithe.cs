// pyrithe.cs.dso
if (!(isObject(BePyrithe)))
{
	%template = new BehaviorTemplate(Name : BePyrithe);
	%template.friendlyName = "Pyrithe";
	%template.behaviorType = "LevelJungle";
	%template.description = "if a flintstone hits this area, sparks are spread";
}
function BePyrithe::onBehaviorAdd(%this)
{
	%owner = Owner;
	%owner.addDependentBehavior("BeCollide");
	subscribeToEvent(%this, "onLevelLoadFinished");
	return;
}
function BePyrithe::onLevelLoadFinished(%this)
{
	%owner = Owner;
	%this.setBehaviorCollisionReceiveCallback("1");
	unSubscribeFromEvent(%this, "onLevelLoadFinished");
	return;
}
function BePyrithe::onCollisionReceive(%this, %dstObj, , , , , , %contacts)
{
	%flintstoneBehavior = %dstObj.getBehavior("BeFlintstone");
	if (!(%flintstoneBehavior))
	{
		return;
	}
	%speed = t2dVectorLength(%dstObj.getLinearVelocity());
	if (%speed > 50.0 && %flintstoneBehavior.isActive())
	{
		%x = getWord(%contacts, "0");
		%y = getWord(%contacts, "1");
		%spark = Spark::createInstance(%x, %y);
		%spark.Spark();
		%dstObj.getBehavior("BePlayCollisionSound").stop();
		playDistanceEventSound(Owner, SparkSound, "0.5");
		%flintstoneBehavior.madeSpark();
	}
	return;
}
