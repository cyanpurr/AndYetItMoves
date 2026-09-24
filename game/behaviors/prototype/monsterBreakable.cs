// monsterBreakable.cs.dso
if (!(isObject(BeMonsterBreakable)))
{
	%template = new BehaviorTemplate(Name : BeMonsterBreakable);
	%template.friendlyName = "MonsterBreakable";
	%template.behaviorType = "LevelPrototype";
	%template.description = "makes this objet breakable on collision with the jungleMonster";
	%template.addBehaviorField(criticalSpeed, "the speed the object needs to break through", float, "50");
}
function BeMonsterBreakable::onBehaviorAdd(%this)
{
	%owner = Owner;
	subscribeToEvents(%this, "onLevelLoadFinished5");
	%owner.addDependentBehaviors("BeCollide");
	return;
}
function BeMonsterBreakable::onLevelLoadFinished5(%this)
{
	%owner = Owner;
	%react = %owner.addDependentBehavior("BeReactOnCollision");
	%react.initGroups("jungleMonsterHorns");
	%react.BehaviorList = "BeMonsterBreakable";
	%react.criticalSpeed = criticalSpeed;
	%react.quarryBehavior = %this;
	%react.switchOn();
	return;
}
function BeMonsterBreakable::reactOnCollision(%this, , , , , , , , )
{
	%owner = Owner;
	arenaMonster.stumble();
	playDistanceEventSound(%owner, MonsterWallCrush, "1");
	%effect = new t2dParticleEffect(Name : "")
	{
		scenegraph = daSceneGraph;
		effectFile = "~/data/particles/hamsterBreak.eff";
		useEffectCollisions = "1";
		effectMode = "KILL";
		effectTime = "0.3";
		canSaveDynamicFields = "1";
		size = %owner.getSize();
	}
	%effect.setPosition(%owner.getPosition());
	%effect.setRotation(%owner.getRotation());
	%effect.setLayer(%owner.getLayer());
	scenegraph.setLayerDrawOrder(%effect, "BACK");
	if (camera.getCurrentRotation() % 180 == 0.0)
	{
	}
	else
	{
	}
	%forceAngle = camera.getCurrentRotation() + 180.0;
	%effect.getEmitterObject("0").setFixedForceAngle(%forceAngle);
	%owner.setCollisionSuppress("1");
	%owner.setVisible("0");
	%owner.safeDelete();
	%effect.playEffect();
	return;
}
