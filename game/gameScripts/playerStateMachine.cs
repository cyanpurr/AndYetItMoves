// playerStateMachine.cs.dso
function player::initAnimationNames(%this)
{
	%allStates = "stand standRun run runStand standJump fallStand softFallStand runJump fallRun softFallRun jump jumpFall fall highFall startFuchtelFall fuchtelFall startLethalFall lethalFall slide idleHair idleBalloon";
	%numStates = getWordCount(%allStates);
	%i = 0;
	while (%i < %numStates)
	{
		%this.stateId[getWord(%allStates, %i)] = %i;
		%i = %i + 1.0;
	}
	%this.animations["stand"] = playerIdleAnimation;
	%this.animations["standRun"] = playerStandRunAnimation;
	%this.animations["run"] = playerRunAnimation;
	%this.animations["runStand"] = playerRunStandAnimation;
	%this.animations["standJump"] = playerStandJumpAnimation;
	%this.animations["fallStand"] = playerJumpStandAnimation;
	%this.animations["softFallStand"] = playerSoftJumpStandAnimation;
	%this.animations["runJump"] = playerStandJumpAnimation;
	%this.animations["fallRun"] = playerJumpRunAnimation;
	%this.animations["softFallRun"] = playerSoftJumpRunAnimation;
	%this.animations["jump"] = playerJumpAnimation;
	%this.animations["jumpFall"] = playerJumpFallAnimation;
	%this.animations["fall"] = playerFallAnimation;
	%this.animations["highFall"] = playerHighFallAnimation;
	%this.animations["startFuchtelFall"] = playerFallNewFallAnimation;
	%this.animations["fuchtelFall"] = playerFuchtelFallAnimation;
	%this.animations["lethalFall"] = playerLethalFallAnimation;
	%this.animations["slide"] = playerSlideAnimation;
	%this.animations["idleHair"] = playerIdleHairAnimation;
	%this.animations["idleBalloon"] = baloonPlayerLinkImageAnimation;
	return;
}
function playerBalloon::onAnimationEnd(%this)
{
	%this.setEnabled("0");
	return;
}
