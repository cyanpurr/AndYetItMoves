// dropDownCallbacks.cs.dso
function LayerDropDown::onSelect(%this, %index, %value)
{
	if (%value != "custom")
	{
		Owner.setLayer($LAYER[%value]);
	}
	return;
}
function DekoLayerDropDown::onSelect(%this, %index, %value)
{
	LayerDropDown::onSelect(%this, %index, %value);
	return;
}
function CollisionDetectionDropDown::onSelect(%this, %index, %value)
{
	if (%value != "")
	{
		Owner.setCollisionDetection(%value);
	}
	return;
}
function hintDropDown::onSelect(%this, %index, %value)
{
	eval("%this.object.hintObject.text = $" @ hint @ ";");
	return;
}
