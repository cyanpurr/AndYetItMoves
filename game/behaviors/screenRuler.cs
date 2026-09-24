// screenRuler.cs.dso
$possibleScreenRatios = 4.0 / 3.0 TAB 16.0 / 10.0 TAB 16.0 / 9.0;
$zoomFactors = "";
%i = 0.4000000059604645;
while (%i <= 2.0)
{
	$zoomFactors = %i TAB $zoomFactors;
	%i = %i + 0.05000000074505806;
}
$zoomFactors = trim($zoomFactors);
if (!(isObject(BeScreenRuler)))
{
	%template = new BehaviorTemplate(Name : BeScreenRuler);
	%template.friendlyName = "ScreenRuler";
	%template.behaviorType = "TGB Tool";
	%template.description = "scales a screenruler object";
	%template.addBehaviorField(screenRatio, "ratio of screen (worst case - widest) - 4:3, 16:10, 16:9", enum, "1.6", $possibleScreenRatios);
	%template.addBehaviorField(zoomFactor, "zoom factor (worst case - smallest - biggest camera) of actual position", enum, "0.5", $zoomFactors);
	%template.addBehaviorField(layerView, "view for a specific layer", enum, "main", "main" TAB $PARALAXLAYER_ENUM);
}
function BeScreenRuler::onBehaviorAdd(%this)
{
	%owner = Owner;
	%this.applySettings();
	%owner.setCollisionDetection("CIRCLE");
	%owner.setCollisionActiveSend("1");
	subscribeToEvents(%this, "onLevelLoadFinished2");
	return;
}
function BeScreenRuler::onLevelLoadFinished2(%this)
{
	%owner = Owner;
	%parent = %owner.getMountedParent();
	%owner.delete();
	if (isObject(%parent) && 0)
	{
		%parent.delete();
	}
	return;
}
function BeScreenRuler::applySettings(%this)
{
	%owner = Owner;
	%height = 180;
	%width = %height * screenRatio;
	%size = t2dVectorScale(%width SPC %height, 1.0 / zoomFactor);
	%speedFactor = $LAYER_SPEEDFACTOR[layerView];
	%scalingFactor = 1.0 - %speedFactor;
	%size = t2dVectorScale(%size, 1.0 / %scalingFactor);
	%owner.setSize(%size);
	return;
}
function ScreenRatioDropDown::onSelect(%this, , )
{
	object.applySettings();
	return;
}
function ZoomFactorDropDown::onSelect(%this, , )
{
	object.applySettings();
	return;
}
function LayerViewDropDown::onSelect(%this, , )
{
	object.applySettings();
	return;
}
