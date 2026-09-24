// orientationPoint.cs.dso
function initOrientationPoint()
{
	%orientationPointPosition = "0" SPC getWord(statsWindow.getCurrentCameraSize(), "1") / 2.0 * -0.800000011920929;
	new t2dSceneGraph(Name : statsScenegraph)
	{
		new t2dAnimatedSprite(Name : orientationPointAnim)
		{
			animationName = "orientationPointAnimationRight";
			size = "47.438 11.859667";
			Position = %orientationPointPosition;
			Visible = "0";
		}
		new t2dAnimatedSprite(Name : orientationPointAnimReverse)
		{
			animationName = "orientationPointAnimationRightReverse";
			size = "47.438 11.859667";
			Position = %orientationPointPosition;
		}
	}
	statsWindow.setSceneGraph(statsScenegraph);
	subscribeToEvents(orientationPointAnim, "onRotationStart onRotationFinish");
	return;
}
function orientationPoint::onRotationStart(%this)
{
	%this.startRotation = camera.getCurrentRotation();
	orientationPointAnimReverse.setVisible("0");
	if (rotationDirection == $CW)
	{
		orientationPointAnim.setVisible("1");
		orientationPointAnim.playAnimation(orientationPointAnimationRight);
	}
	else
	{
		orientationPointAnim.setVisible("1");
		orientationPointAnim.playAnimation(orientationPointAnimationLeft);
	}
	return;
}
function orientationPoint::onRotationFinish(%this)
{
	orientationPointAnim.setVisible("0");
	orientationPointAnimReverse.setVisible("1");
	if (rotationDirection == $CW)
	{
		orientationPointAnimReverse.playAnimation(orientationPointAnimationRightReverse);
	}
	else
	{
		orientationPointAnimReverse.playAnimation(orientationPointAnimationLeftReverse);
	}
	return;
}
