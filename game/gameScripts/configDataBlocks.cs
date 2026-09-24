// configDataBlocks.cs.dso
new t2dSceneObjectDatablock(Name : PlayerCDB)
{
	Layer = "10";
	CollisionPhysicsReceive = "0";
	CollisionPhysicsSend = "1";
	CollisionActiveReceive = "1";
	CollisionActiveSend = "1";
	CollisionCallback = "true";
	CollisionResponseMode = "RIGID";
	CollisionDetectionMode = "FULL";
	GraphGroup = "1";
	CollisionGroups = bits("3 5 13");
	MaxAngularVelocity = "0";
	Density = "0.01";
	Friction = "0.01";
	Restitution = "0.01";
	CollisionMaxIterations = "4";
	timeToFall = "0.05";
	moveSpeed = "30";
	jumpForce = "45";
	maxSlope = "0.4";
	moveGroundAccel = "10";
	moveAirAccel = "1";
	jumpAccel = "10";
	energyDownTime = "2000";
	energyUpTime = "4000";
	lethalSpeed = "50 100";
	myDensity = "0.01";
	myFriction = "0.2";
	myRestitution = "0.0";
}
new t2dSceneObjectDatablock(Name : FraggedPlayerCDB)
{
	class = "FraggedPlayerPart";
	superclass = "ClusterMember";
	Layer = "10";
	CollisionPhysicsReceive = "0";
	CollisionPhysicsSend = "1";
	CollisionActiveReceive = "1";
	CollisionActiveSend = "1";
	CollisionResponseMode = "RIGID";
	CollisionDetectionMode = "FULL";
	CollisionMaxIterations = "4";
	GraphGroup = "2";
	CollisionGroups = bits("3 5 13");
	MaxAngularVelocity = "360";
	MaxLinearVelocity = "1000";
	Damping = "2";
	parentObject = "fraggedPlayerRoot";
	jointForce = "3";
	animationMode = -1.0;
}
new t2dSceneObjectDatablock(Name : AbstractObejctsCDB)
{
	Layer = "0";
	CollisionPhysicsReceive = "0";
	CollisionPhysicsSend = "0";
	CollisionActiveReceive = "0";
	CollisionActiveSend = "0";
	BlendingEnabled = "0";
	GraphGroup = "31";
}
new t2dSceneObjectDatablock(Name : StaticSceneElementFrontCDB)
{
	Layer = "8";
	CollisionPhysicsReceive = "0";
	CollisionPhysicsSend = "0";
	CollisionActiveReceive = "0";
	CollisionActiveSend = "0";
	GraphGroup = "31";
}
new t2dSceneObjectDatablock(Name : StaticSceneElementBackCDB)
{
	Layer = "12";
	CollisionPhysicsReceive = "0";
	CollisionPhysicsSend = "0";
	CollisionActiveReceive = "0";
	CollisionActiveSend = "0";
	GraphGroup = "31";
}
new t2dSceneObjectDatablock(Name : CollisionObjectsCDB)
{
	CollisionPhysicsReceive = "0";
	CollisionPhysicsSend = "0";
	CollisionActiveReceive = "1";
	CollisionActiveSend = "0";
	Immovable = "1";
	GraphGroup = "3";
	Layer = "1";
}
new t2dSceneObjectDatablock(Name : WorldLimitCDB)
{
	class = "WorldLimit";
	CollisionPhysicsReceive = "0";
	CollisionPhysicsSend = "0";
	CollisionActiveReceive = "1";
	CollisionActiveSend = "0";
	CollisionCallback = "1";
	Layer = "3";
	Immovable = "1";
	CollisionGroups = bits("1 2 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24 25 26 27 28 29 30");
	GraphGroup = "3";
	defragVelocity = "0 0";
}
new t2dSceneObjectDatablock(Name : MovingObjectsCDB)
{
	class = "MovingObject";
	CollisionPhysicsReceive = "0";
	CollisionPhysicsSend = "1";
	CollisionActiveReceive = "1";
	CollisionActiveSend = "1";
	CollisionResponseMode = "RIGID";
	CollisionDetectionMode = "FULL";
	GraphGroup = "5";
	CollisionGroups = bits("1 3 5");
	myDensity = "0.01";
	myFriction = "0.1";
	myRestitution = "0";
}
new t2dSceneObjectDatablock(Name : MaskObjectsFrontCDB)
{
	class = "MaskObject";
	Layer = "8";
	CollisionActiveSend = "0";
	CollisionActiveReceive = "0";
	CollisionPhysicsSend = "0";
	CollisionPhysicsReceive = "0";
	GraphGroup = "31";
	texObject = "textureName";
	usePositive = "0";
	protoObject = "protoName";
}
new t2dSceneObjectDatablock(Name : MaskObjectsBackCDB)
{
	class = "MaskObject";
	Layer = "12";
	CollisionActiveSend = "0";
	CollisionActiveReceive = "0";
	CollisionPhysicsSend = "0";
	CollisionPhysicsReceive = "0";
	texObject = "textureName";
	usePositive = "0";
	protoObject = "protoName";
}
new t2dSceneObjectDatablock(Name : ActiveMaskObjectsCDB)
{
	class = "ActiveMaskObject";
	superclass = "MaskObject";
	CollisionActiveSend = "1";
	CollisionActiveReceive = "0";
	CollisionPhysicsSend = "0";
	CollisionPhysicsReceive = "0";
	CollisionCallback = "true";
	CollisionResponseMode = "RIGID";
	CollisionDetectionMode = "FULL";
	GraphGroup = "4";
	CollisionGroups = bits("1 5");
	texObject = "textureName";
}
new t2dSceneObjectDatablock(Name : TexturesCDB)
{
	Layer = "30";
	CollisionPhysicsReceive = "0";
	CollisionPhysicsSend = "0";
	CollisionActiveReceive = "0";
	CollisionActiveSend = "0";
	GraphGroup = "31";
	trackMaskFlip = "1";
	noFreeze = "1";
}
new t2dSceneObjectDatablock(Name : ActiveTexturesCDB)
{
	class = "ActiveTexture";
	Layer = "30";
	CollisionPhysicsReceive = "0";
	CollisionPhysicsSend = "0";
	CollisionActiveReceive = "0";
	CollisionActiveSend = "0";
	GraphGroup = "31";
	trackMaskFlip = "1";
}
new t2dSceneObjectDatablock(Name : GlobalCDB)
{
	Layer = "5";
	cameraSpeed = "5";
	gravity = "80";
	noCursor = "1";
	jitterAngle = "1";
	rotSpeedRange = "0 100";
	rotSpeedTol = "400";
	rotSpeedTolStep = "20";
	isBugActive = "0";
}
new t2dSceneObjectDatablock(Name : NoFreezeCDB)
{
	noFreeze = "1";
}
new t2dSceneObjectDatablock(Name : SnapperCDB)
{
	class = "Snapper";
	snapToDireciton = "0";
	snapToAngle = "0";
	snapToVelocity = "50";
	accelerationFactor = "0";
	currentDirection = "0";
	isSnapping = "0";
	noFreeze = "1";
}
new t2dSceneObjectDatablock(Name : ActionTriggerCDB)
{
	class = "ActionTrigger";
	LeaveCallback = "0";
	Layer = "3";
	CollisionPhysicsReceive = "0";
	CollisionPhysicsSend = "0";
	CollisionGroups = bits("1 7");
	GraphGroup = "6";
	activateObjects = "0";
	deactivateObjects = "0";
	loadGroups = "0";
	unloadGroups = "0";
}
new t2dSceneObjectDatablock(Name : SpawnTriggerCDB)
{
	class = "SpawnTrigger";
	Layer = "3";
	LeaveCallback = "0";
	CollisionPhysicsReceive = "0";
	CollisionPhysicsSend = "0";
	CollisionGroups = bits("1 7");
	GraphGroup = "6";
	isSpawnPoint = "0";
	spawnPointNumber = "0";
	forward = "1";
	spawnPathPoint = "0";
}
new t2dSceneObjectDatablock(Name : SpawnPointCDB)
{
	CollisionPhysicsReceive = "0";
	CollisionPhysicsSend = "0";
	CollisionActiveReceive = "0";
	CollisionActiveSend = "0";
	GraphGroup = "31";
}
new t2dSceneObjectDatablock(Name : StoneCDB)
{
	class = "Stone";
	superclass = "MovingObject";
	CollisionPhysicsReceive = "0";
	CollisionPhysicsSend = "1";
	CollisionActiveReceive = "1";
	CollisionActiveSend = "1";
	CollisionCallback = "true";
	CollisionResponseMode = "RIGID";
	CollisionDetectionMode = "FULL";
	CollisionMaxIterations = "10";
	GraphGroup = "5";
	CollisionGroups = bits("3 5");
	MaxAngularVelocity = "100";
	myDensity = "0.04";
	myDamping = "0";
	myFriction = "0.4";
	stoneView = "0";
}
new t2dSceneObjectDatablock(Name : WallPartCDB)
{
	class = "WallPart";
	superclass = "MaskObject";
	CollisionPhysicsReceive = "1";
	CollisionPhysicsSend = "1";
	CollisionActiveReceive = "1";
	CollisionActiveSend = "1";
	CollisionCallback = "true";
	CollisionResponseMode = "RIGID";
	CollisionDetectionMode = "FULL";
	GraphGroup = "12";
	CollisionGroups = bits("1 3 5 8");
	MaxAngularVelocity = "100";
	texObject = "wallPartTex";
	myDensity = "0.04";
	myDamping = "0";
	myFriction = "0.4";
	breakFirst = "0";
}
new t2dSceneObjectDatablock(Name : MonkeyTriggerCDB)
{
	Layer = "2";
	CollisionActiveReceive = "0";
	CollisionPhysicsReceive = "0";
	CollisionPhysicsSend = "0";
	CollisionGroups = bits("1");
	GraphGroup = "6";
	noPlayerReaction = "1";
}
new t2dSceneObjectDatablock(Name : CameraPathPoint)
{
	Layer = "2";
	class = "CameraPathPoint";
	CollisionPhysicsReceive = "0";
	CollisionPhysicsSend = "0";
	CollisionActiveReceive = "0";
	CollisionActiveSend = "0";
	BlendingEnabled = "0";
	GraphGroup = "31";
	number = "0";
	rotate = "0";
}
new t2dSceneObjectDatablock(Name : BurningBushCDB)
{
	class = "FlameArea";
	superclass = "MaskObject";
	Layer = "12";
	CollisionPhysicsReceive = "0";
	CollisionPhysicsSend = "0";
	CollisionActiveReceive = "1";
	CollisionActiveSend = "0";
	CollisionCallback = "1";
	GraphGroup = "11";
	usePositive = "0";
	protoObject = "protoBush";
}
new t2dSceneObjectDatablock(Name : FlameCDB)
{
	class = "Flame";
	superclass = "MaskObject";
	CollisionPhysicsReceive = "0";
	CollisionPhysicsSend = "0";
	CollisionActiveReceive = "0";
	CollisionActiveSend = "1";
	CollisionCallback = "1";
	Visible = "1";
	BlendingEnabled = "1";
	CollisionGroups = bits("1");
	GraphGroup = "15";
	dontTrack = "1";
	burnSchedule = -1.0;
}
new t2dSceneObjectDatablock(Name : MonsterRotatesTriggerCDB)
{
	class = "monsterRotatesTrigger";
	LeaveCallback = "0";
	Layer = "2";
	CollisionPhysicsReceive = "0";
	CollisionPhysicsSend = "0";
	CollisionGroups = bits("1 10");
	GraphGroup = "6";
	angle = "0";
	count = "1";
}
new t2dSceneObjectDatablock(Name : PullersCDB)
{
	class = "Pullers";
	LeaveCallback = "0";
	Layer = "2";
	CollisionPhysicsReceive = "0";
	CollisionPhysicsSend = "0";
	CollisionGroups = bits("1");
	GraphGroup = "6";
	killPoint = "0";
	killForce = "2";
}
new t2dSceneObjectDatablock(Name : BeesCDB)
{
	CollisionPhysicsReceive = "0";
	CollisionPhysicsSend = "0";
	CollisionActiveReceive = "0";
	CollisionActiveSend = "1";
	CollisionCallback = "1";
	CollisionResponseMode = "RIGID";
	CollisionDetectionMode = "FULL";
	GraphGroup = "5";
	CollisionGroups = bits("1");
	aggression = "3";
	noFreeze = "1";
}
new t2dSceneObjectDatablock(Name : RotatorCDB)
{
	class = "ClusterMember";
	parentObject = "parentName";
	threshold = "1";
	destination = "20";
	accelerationRange = "0.0 0.0";
	acceleration = "1";
	speedRange = "0.0 0.0";
	speed = "10";
	animationMode = "2";
	randomMovementFactor = "0";
}
new t2dSceneObjectDatablock(Name : ClusterMemberCDB)
{
	class = "ClusterMember";
	parentObject = "parentName";
	threshold = "1";
	animationMode = "-1";
	jointForce = "0";
}
