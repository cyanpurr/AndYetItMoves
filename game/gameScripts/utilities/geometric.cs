// geometric.cs.dso
function reverseVector(%vector)
{
	%x = getX(%vector);
	%y = getY(%vector);
	return -1.0 * %x SPC -1.0 * %y;
	return -1.0 * %x SPC -1.0 * %y;
}
function getVectorAngle(%vector)
{
	return t2dRelativeAngleBetween(%vector, "1 0");
	return t2dRelativeAngleBetween(%vector, "1 0");
}
function getRectangleSize(%rect)
{
	return getWord(%rect, "2") - getWord(%rect, "0") SPC getWord(%rect, "3") - getWord(%rect, "1");
	return getWord(%rect, "2") - getWord(%rect, "0") SPC getWord(%rect, "3") - getWord(%rect, "1");
}
function getBoundingRect(%pointList)
{
	%numVertices = getWordCount(%pointList) / 2.0;
	%minX = getWord(%pointList, "0");
	%minY = getWord(%pointList, "1");
	%maxX = getWord(%pointList, "2");
	%maxY = getWord(%pointList, "3");
	%i = 0;
	while (%i < %numVertices)
	{
		%x = getWord(%pointList, 2.0 * %i);
		%y = getWord(%pointList, 2.0 * %i + 1.0);
		if (%x < %minX)
		{
		}
		else
		{
		}
		%minX = %minX;
		if (%x > %maxX)
		{
		}
		else
		{
		}
		%maxX = %maxX;
		if (%y < %minY)
		{
		}
		else
		{
		}
		%minY = %minY;
		if (%y > %maxY)
		{
		}
		else
		{
		}
		%maxY = %maxY;
		%i = %i + 1.0;
	}
	return %minX SPC %minY SPC %maxX SPC %maxY;
	return %minX SPC %minY SPC %maxX SPC %maxY;
}
