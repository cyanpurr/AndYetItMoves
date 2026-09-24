// options.cs.dso
function OptionsDlg::onWake(%this)
{
	FullscreenToggle.setValue($pref::Video::fullScreen);
	%buffer = getDisplayDeviceList();
	%count = getFieldCount(%buffer);
	GraphicsDriverMenu.clear();
	%i = 0;
	while (%i < %count)
	{
		GraphicsDriverMenu.add(getField(%buffer, %i), %i);
		%i = %i + 1.0;
	}
	%selId = GraphicsDriverMenu.findText($pref::Video::displayDevice);
	if (%selId == -1.0)
	{
		%selId = 0;
	}
	GraphicsDriverMenu.setSelected(%selId);
	GraphicsDriverMenu.onSelect(%selId, "");
	MusicAudioVolume.setValue($pref::Audio::channelVolume[$musicAudioType]);
	EffectsAudioVolume.setValue($pref::Audio::channelVolume[$effectsAudioType]);
	ScreenshotMenu.clear();
	ScreenshotMenu.add("PNG", "0");
	ScreenshotMenu.add("JPEG", "1");
	ScreenshotMenu.setValue($pref::Video::screenShotFormat);
	initializeKeybindOptions();
	return;
}
function GraphicsDriverMenu::onSelect(%this, , )
{
	%prevRes = getWords(getRes(), "0", "1");
	if (FullscreenToggle.getValue())
	{
		if (BPPMenu.size() > 0.0)
		{
			%prevBPP = BPPMenu.getText();
			break;
		}
		%prevBPP = getWord($pref::Video::windowedRes, "2");
	}
	if (isDeviceFullScreenOnly(%this.getText()))
	{
		FullscreenToggle.setValue("1");
		FullscreenToggle.setActive("0");
		FullscreenToggle.onAction();
	}
	else
	{
		FullscreenToggle.setActive("1");
	}
	ResolutionMenu.init(%this.getText(), FullscreenToggle.getValue());
	BPPMenu.init(%this.getText());
	%selId = ResolutionMenu.findText(%prevRes);
	if (%selId == -1.0)
	{
		%selId = ResolutionMenu.size() - 1.0;
	}
	ResolutionMenu.setSelected(%selId);
	if (FullscreenToggle.getValue())
	{
		%selId = BPPMenu.findText(%prevBPP);
		if (%selId == -1.0)
		{
			%selId = 0;
		}
		BPPMenu.setSelected(%selId);
		BPPMenu.setText(BPPMenu.getTextById(%selId));
	}
	else
	{
		BPPMenu.setText("Default");
	}
	return;
}
function ResolutionMenu::init(%this, %device, %fullScreen)
{
	%this.clear();
	if (%fullScreen $= "")
	{
		%fullScreen = isFullScreen();
	}
	%resList = getResolutionList(%device);
	%resCount = getFieldCount(%resList);
	%deskRes = getDesktopResolution();
	%count = 0;
	%i = 0;
	while (%i < %resCount)
	{
		%res = getWords(getField(%resList, %i), "0", "1");
		if (!(%fullScreen))
		{
			if ($Video::WindowedDesktopSize != "")
			{
				%deskRes = $Video::WindowedDesktopSize;
			}
			if (firstWord(%res) >= firstWord(%deskRes))
			{
			}
			else
			{
				if (getWord(%res, "1") >= getWord(%deskRes, "1"))
				{
					break;
				}
			}
			if (%this.findText(%res) == -1.0)
			{
				%this.add(%res, %count);
				%count = %count + 1.0;
			}
		}
		%i = %i + 1.0;
	}
	return;
}
function BPPMenu::init(%this, %device)
{
	%this.clear();
	if (%device $= "Voodoo2")
	{
		%this.add("16", "0");
	}
	else
	{
		%resList = getResolutionList(%device);
		%resCount = getFieldCount(%resList);
		%count = 0;
		%i = 0;
		while (%i < %resCount)
		{
			%bpp = getWord(getField(%resList, %i), "2");
			if (%this.findText(%bpp) == -1.0)
			{
				%this.add(%bpp, %count);
				%count = %count + 1.0;
			}
			%i = %i + 1.0;
		}
	}
	return;
}
function FullscreenToggle::onAction(%this)
{
	%prevRes = ResolutionMenu.getText();
	ResolutionMenu.init(GraphicsDriverMenu.getText(), %this.getValue());
	%selId = -1.0;
	if (%this.getValue() == 0.0 && $pref::Video::windowedRes != "")
	{
		%selId = ResolutionMenu.findText(getWords($pref::Video::windowedRes, "0", "1"));
	}
	else
	{
		if (%this.getValue() == 1.0 && $pref::Video::Resolution != "")
		{
			%selId = ResolutionMenu.findText(getWords($pref::Video::Resolution, "0", "1"));
		}
	}
	if (%selId == -1.0)
	{
		%selId = ResolutionMenu.findText(%prevRes);
	}
	if (%selId != -1.0)
	{
		ResolutionMenu.setSelected(%selId);
	}
	else
	{
		ResolutionMenu.setSelected(ResolutionMenu.size() - 1.0);
	}
	return;
}
$AudioTestHandle = 0;
function updateChannelVolume(%channel, %volume)
{
	if (%channel < 1.0 || %channel > 8.0)
	{
		return;
	}
	alxSetChannelVolume(%channel, %volume);
	$pref::Audio::channelVolume[%channel] = %volume;
	if (!(alxIsPlaying($AudioTestHandle)))
	{
		$AudioTestHandle = alxCreateSource("AudioChannel" @ %channel, expandFilename("~/data/audio/volumeTest.wav"));
		alxPlay($AudioTestHandle);
	}
	return;
}
function applyAVOptions()
{
	%newDriver = GraphicsDriverMenu.getText();
	%newRes = ResolutionMenu.getText();
	%newBpp = BPPMenu.getText();
	%newFullScreen = FullscreenToggle.getValue();
	$pref::Video::screenShotFormat = ScreenshotMenu.getText();
	if (%newFullScreen && !(isFullScreen()))
	{
		$Video::WindowedDesktopSize = getDesktopResolution();
	}
	if (%newDriver != $pref::Video::displayDevice)
	{
		setDisplayDevice(%newDriver, firstWord(%newRes), getWord(%newRes, "1"), %newBpp, %newFullScreen);
	}
	else
	{
		setScreenMode(firstWord(%newRes), getWord(%newRes, "1"), %newBpp, %newFullScreen);
	}
	Canvas.popDialog(OptionsDlg);
	Canvas.pushDialog(OptionsDlg);
	FullscreenToggle.setValue(%newFullScreen);
	return;
}
function revertAVOptions()
{
	%selId = ResolutionMenu.findText("800 600");
	if (%selId == -1.0)
	{
		%selId = 0;
	}
	ResolutionMenu.setSelected(%selId);
	FullscreenToggle.setValue("0");
	BPPMenu.setText("Default");
	%selId = GraphicsDriverMenu.findText("OpenGL");
	if (%selId == -1.0)
	{
		%selId = 0;
	}
	GraphicsDriverMenu.setSelected(%selId);
	GraphicsDriverMenu.onSelect(%selId, "");
	EffectsAudioVolume.setValue("0.8");
	MusicAudioVolume.setValue("0.8");
	updateChannelVolume($effectsAudioType, "0.8");
	updateChannelVolume($musicAudioType, "0.8");
	ScreenshotMenu.clear();
	ScreenshotMenu.setValue("PNG");
	return;
}
function revertAVOptionChanges()
{
	FullscreenToggle.setValue($pref::Video::fullScreen);
	if (FullscreenToggle.getValue())
	{
		%selId = ResolutionMenu.findText(getWords($pref::Video::Resolution, "0", "1"));
	}
	else
	{
		%selId = ResolutionMenu.findText(getWords($pref::Video::windowedRes, "0", "1"));
	}
	if (%selId == -1.0)
	{
		%selId = 0;
	}
	ResolutionMenu.setSelected(%selId);
	if (FullscreenToggle.getValue())
	{
		%selId = BPPMenu.findText(getWord($pref::Video::Resolution, "2"));
		if (%selId == -1.0)
		{
			%selId = 0;
		}
		BPPMenu.setSelected(%selId);
		BPPMenu.setText(BPPMenu.getTextById(%selId));
	}
	else
	{
		BPPMenu.setText("Default");
	}
	%selId = GraphicsDriverMenu.findText($pref::Video::displayDevice);
	if (%selId == -1.0)
	{
		%selId = 0;
	}
	GraphicsDriverMenu.setSelected(%selId);
	GraphicsDriverMenu.onSelect(%selId, "");
	ScreenshotMenu.setValue($pref::Video::screenShotFormat);
	return;
}
