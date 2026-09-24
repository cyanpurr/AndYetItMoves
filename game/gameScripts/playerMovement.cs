// playerMovement.cs.dso
function moveLeft(%isKeyDown)
{
	if (!(firstKeyPressed()))
	{
		return firstKeyPressed();
	}
	if (creditsControl)
	{
		return globals;
	}
	player.moveLeft = %isKeyDown;
	player.onMoveInputChange();
	if (isObject(alterEgo))
	{
		if (!(isActive))
		{
			return alterEgo;
		}
		if (!(sameGravity) && !(invertMovement) || sameGravity && invertMovement)
		{
			alterEgo.moveRight = %isKeyDown;
		}
		else
		{
			alterEgo.moveLeft = %isKeyDown;
		}
		alterEgo.onMoveInputChange();
	}
	if (%isKeyDown)
	{
		achievements.moveKeyPressed = "1";
	}
	return;
}
function moveRight(%isKeyDown)
{
	if (!(firstKeyPressed()))
	{
		return firstKeyPressed();
	}
	if (creditsControl)
	{
		return globals;
	}
	player.moveRight = %isKeyDown;
	player.onMoveInputChange();
	if (isObject(alterEgo))
	{
		if (!(isActive))
		{
			return alterEgo;
		}
		if (!(sameGravity) && !(invertMovement) || sameGravity && invertMovement)
		{
			alterEgo.moveLeft = %isKeyDown;
		}
		else
		{
			alterEgo.moveRight = %isKeyDown;
		}
		alterEgo.onMoveInputChange();
	}
	if (%isKeyDown)
	{
		achievements.moveKeyPressed = "1";
	}
	return;
}
function jump(%isKeyDown)
{
	if (!(firstKeyPressed()))
	{
		return firstKeyPressed();
	}
	if (creditsControl)
	{
		return globals;
	}
	if (!(isFalling) && %isKeyDown)
	{
		player.jump = "1";
	}
	if (isObject(alterEgo))
	{
		if (!(isActive))
		{
			return alterEgo;
		}
		if (!(isFalling) && %isKeyDown)
		{
			alterEgo.jump = "1";
		}
	}
	if (%isKeyDown)
	{
		achievements.moveKeyPressed = "1";
	}
	return;
}
function player::onMoveInputChange(%this, %isAlterEgo)
{
	%this.move = moveRight - moveLeft;
	if (isAnimSlideSlope && isFalling)
	{
		return %this;
	}
	%directionBefore = %this.getFlipX();
	if (moveLeft && !(moveRight))
	{
		%this.setFlipX(%isAlterEgo);
	}
	else
	{
		if (!(moveLeft) && moveRight)
		{
			%this.setFlipX(!(%isAlterEgo));
		}
	}
	if (%directionBefore != %this.getFlipX())
	{
		triggerEvent("onMoveInputChange");
	}
	return;
}
function player::doTheJump(%this)
{
	%this.jump = "0";
	%this.lastJumpTime = thisTime;
	if (!(groundCollisionObject.getCollisionSuppress()))
	{
		groundCollisionObject.setCollisionSuppress("1");
		callNextFrame(groundCollisionObject, "setCollisionSuppress, false");
	}
	if (groundCollisionObject.getBehavior("BeTrampolineBranch"))
	{
		%groundVertVel = %this.getVerticalVelocity();
	}
	else
	{
		%groundVertVel = getVerticalComponent(lastContactPointVelocity);
		if (%groundVertVel > 0.0)
		{
			%groundVertVel = %groundVertVel * 0.4000000059604645;
		}
	}
	%this.setVerticalVelocity(-1.0 * jumpSpeed + %groundVertVel);
	%this.isFalling = "1";
	triggerEvent("onPlayerJump");
	return;
}
function player::onDelayedJump(%this)
{
	triggerEvent("onPlayerDelayedJump");
	groundCollisionObject.jumpDelayed = "0";
	%this.setState(stateId["jump"]);
	%this.doTheJump();
	return;
}
