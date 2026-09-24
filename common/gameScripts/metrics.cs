// metrics.cs.dso
exec("~/gui/FrameOverlayGui.gui");
function fpsMetricsCallback()
{
	return ['" FPS: "', '$fps::real', '"  mspf: "', <torque.Div object at 0x0000021715A36CF0>];
	return ['" FPS: "', '$fps::real', '"  mspf: "', <torque.Div object at 0x0000021715A36CF0>];
}
function videoMetricsCallback()
{
	return [<torque.FuncCall object at 0x0000021715A353A0>, '"  Video -"', '"  TC: "', <torque.Add object at 0x0000021715A788C0>, '"  PC: "', <torque.Add object at 0x0000021715A78830>, '"  T_T: "', '$OpenGL::triCount1', '"  T_P: "', '$OpenGL::primCount1', '"  I_T: "', '$OpenGL::triCount2', '"  I_P: "', '$OpenGL::primCount2', '"  TS_T: "', '$OpenGL::triCount3', '"  TS_P: "', '$OpenGL::primCount3', '"  ?_T: "', '$OpenGL::triCount0', '"  ?_P: "', '$OpenGL::primCount0'];
	return [<torque.FuncCall object at 0x0000021715A353A0>, '"  Video -"', '"  TC: "', <torque.Add object at 0x0000021715A788C0>, '"  PC: "', <torque.Add object at 0x0000021715A78830>, '"  T_T: "', '$OpenGL::triCount1', '"  T_P: "', '$OpenGL::primCount1', '"  I_T: "', '$OpenGL::triCount2', '"  I_P: "', '$OpenGL::primCount2', '"  TS_T: "', '$OpenGL::triCount3', '"  TS_P: "', '$OpenGL::primCount3', '"  ?_T: "', '$OpenGL::triCount0', '"  ?_P: "', '$OpenGL::primCount0'];
}
function textureMetricsCallback()
{
	return [<torque.FuncCall object at 0x0000021715A786E0>, '"  Texture --"', '"  NTL: "', '$Video::numTexelsLoaded', '"  TRP: "', '$Video::texResidentPercentage', '"  TCM: "', '$Video::textureCacheMisses'];
	return [<torque.FuncCall object at 0x0000021715A786E0>, '"  Texture --"', '"  NTL: "', '$Video::numTexelsLoaded', '"  TRP: "', '$Video::texResidentPercentage', '"  TCM: "', '$Video::textureCacheMisses'];
}
function timeMetricsCallback()
{
	return [<torque.FuncCall object at 0x0000021715A78590>, '"  Time -- "', '"  Sim Time: "', <torque.FuncCall object at 0x0000021715A78560>, '"  Mod: "', <torque.Mod object at 0x0000021715A78500>];
	return [<torque.FuncCall object at 0x0000021715A78590>, '"  Time -- "', '"  Sim Time: "', <torque.FuncCall object at 0x0000021715A78560>, '"  Mod: "', <torque.Mod object at 0x0000021715A78500>];
}
function audioMetricsCallback()
{
	return [<torque.FuncCall object at 0x0000021715A783B0>, '"  Audio --"', '" OH:  "', '$Audio::numOpenHandles', '" OLH: "', '$Audio::numOpenLoopingHandles', '" AS:  "', '$Audio::numActiveStreams', '" NAS: "', '$Audio::numNullActiveStreams', '" LAS: "', '$Audio::numActiveLoopingStreams', '" LS:  "', '$Audio::numLoopingStreams', '" ILS: "', '$Audio::numInactiveLoopingStreams', '" CLS: "', '$Audio::numCulledLoopingStreams'];
	return [<torque.FuncCall object at 0x0000021715A783B0>, '"  Audio --"', '" OH:  "', '$Audio::numOpenHandles', '" OLH: "', '$Audio::numOpenLoopingHandles', '" AS:  "', '$Audio::numActiveStreams', '" NAS: "', '$Audio::numNullActiveStreams', '" LAS: "', '$Audio::numActiveLoopingStreams', '" LS:  "', '$Audio::numLoopingStreams', '" ILS: "', '$Audio::numInactiveLoopingStreams', '" CLS: "', '$Audio::numCulledLoopingStreams'];
}
function debugMetricsCallback()
{
	return [<torque.FuncCall object at 0x0000021715A78260>, '"  Debug --"', '"  NTL: "', '$Video::numTexelsLoaded', '"  TRP: "', '$Video::texResidentPercentage', '"  NP:  "', '$Metrics::numPrimitives', '"  NT:  "', '$Metrics::numTexturesUsed', '"  NO:  "', '$Metrics::numObjectsRendered'];
	return [<torque.FuncCall object at 0x0000021715A78260>, '"  Debug --"', '"  NTL: "', '$Video::numTexelsLoaded', '"  TRP: "', '$Video::texResidentPercentage', '"  NP:  "', '$Metrics::numPrimitives', '"  NT:  "', '$Metrics::numTexturesUsed', '"  NO:  "', '$Metrics::numObjectsRendered'];
}
function metrics(%expr)
{
	if (%expr $= "audio")
	{
		%cb = "audioMetricsCallback()";
	}
	else
	{
		if (%expr $= "debug")
		{
			%cb = "debugMetricsCallback()";
			break;
		}
		if (%expr $= "fps")
		{
			%cb = "fpsMetricsCallback()";
			break;
		}
		if (%expr $= "time")
		{
			%cb = "timeMetricsCallback()";
			break;
		}
		if (%expr $= "texture")
		{
			GLEnableMetrics("1");
			%cb = "textureMetricsCallback()";
			break;
		}
		if (%expr $= "video")
		{
			%cb = "videoMetricsCallback()";
		}
	}
	if (%cb != "")
	{
		Canvas.pushDialog(FrameOverlayGui, "1000");
		TextOverlayControl.setValue(%cb);
	}
	else
	{
		GLEnableMetrics("0");
		Canvas.popDialog(FrameOverlayGui);
	}
	return;
}
