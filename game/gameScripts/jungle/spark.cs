// spark.cs.dso
function Spark::createInstance(%x, %y)
{
	%sparkMask = new t2dStaticSprite(Name : "")
	{
		scenegraph = daSceneGraph;
		imageMap = "quadrate_kreise_kleinImageMap";
		class = "Spark";
		frame = "0";
		size = "7.000 7.000";
	}
	%sparkClip = new t2dAnimatedSprite(Name : "")
	{
		scenegraph = daSceneGraph;
		animationName = "sparkAnimation";
		size = "7.000 7.000";
	}
	%sparkMask.sparkClip = %sparkClip;
	%textureBehavior = %sparkClip.addDependentBehavior("BeTexture");
	%maskBehavior = %sparkMask.addDependentBehavior("BeMask");
	%maskBehavior.setTexture(%sparkClip, "1");
	%maskBehavior.Layer = "main";
	%maskBehavior.layerModification = -5.0;
	%maskBehavior.dontCollide = "0";
	%maskBehavior.initMaskLayer();
	%sparkClip.mount(%sparkMask, "0 0", "0", "1", "1", "1", "0");
	%sparkMask.setPosition(%x, %y);
	%sparkMask.setVisible("0");
	%sparkMask.setGraphGroup($GROUPS["spark"]);
	%sparkMask.setCollisionActive("0", "1");
	%sparkMask.setCollisionPhysics("0", "0");
	%sparkMask.setCollisionDetection("CIRCLE");
	%sparkMask.setCollisionCircleScale("1.2");
	%sparkMask.setCollisionCircleSuperscribed("0");
	%sparkMask.setCollisionSuppress("1");
	return %sparkMask;
	return %sparkMask;
}
function Spark::Spark(%this)
{
	%this.setCollisionSuppress("0");
	%this.setVisible("1");
	%this.safeSchedule("750", "extinguish");
	return;
}
function Spark::extinguish(%this)
{
	%this.setVisible("0");
	%this.setCollisionSuppress("1");
	sparkClip.safeDelete();
	%this.safeDelete();
	return;
}
