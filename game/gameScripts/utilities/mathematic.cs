// mathematic.cs.dso
function getRectSize(%rect)
{
	return mAbs(getWord(%rect, "2") - getWord(%rect, "0")) SPC mAbs(getWord(%rect, "3") - getWord(%rect, "1"));
	return mAbs(getWord(%rect, "2") - getWord(%rect, "0")) SPC mAbs(getWord(%rect, "3") - getWord(%rect, "1"));
}
function multVector(%v1, %v2)
{
	return getWord(%v1, "0") * getWord(%v2, "0") SPC getWord(%v1, "1") * getWord(%v2, "1") SPC getWord(%v1, "2") * getWord(%v2, "2");
	return getWord(%v1, "0") * getWord(%v2, "0") SPC getWord(%v1, "1") * getWord(%v2, "1") SPC getWord(%v1, "2") * getWord(%v2, "2");
}
function subVector(%v1, %v2)
{
	return getWord(%v1, "0") - getWord(%v2, "0") SPC getWord(%v1, "1") - getWord(%v2, "1") SPC getWord(%v1, "2") - getWord(%v2, "2");
	return getWord(%v1, "0") - getWord(%v2, "0") SPC getWord(%v1, "1") - getWord(%v2, "1") SPC getWord(%v1, "2") - getWord(%v2, "2");
}
function getRatio(%vector)
{
	return getX(%vector) / getY(%vector);
	return getX(%vector) / getY(%vector);
}
