// align.cs.dso
$AlignTools::Horizontal = 0;
$AlignTools::Vertical = 1;
$AlignTools::Left = -1.0;
$AlignTools::Right = 1;
$AlignTools::Top = -1.0;
$AlignTools::Bottom = 1;
$AlignTools::Center = 0;
function AlignTools::sort(%objects, %side, %dir)
{
	%count = %objects.getCount();
	%i = 0;
	while (%i < %count)
	{
		%object = %objects.getObject(%i);
		%position = getWord(%object.getPosition(), %dir);
		%halfSize = getWord(%object.getSize(), %dir) * 0.5;
		%object.sortPos = %position + %halfSize * %side;
		%i = %i + 1.0;
	}
	%i = 0;
	while (%i < %count - 1.0)
	{
		%min = %i;
		%j = %i + 1.0;
		while (%j < %count)
		{
			%objMin = %objects.getObject(%min);
			%obj = %objects.getObject(%j);
			if (sortPos < sortPos)
			{
				%min = %j;
			}
			%j = %j + 1.0;
		}
		%objMin = %objects.getObject(%min);
		%obj = %objects.getObject(%i);
		%objects.reorderChild(%objMin, %obj);
		%i = %i + 1.0;
	}
	return;
}
function AlignTools::deleteSorted(%objects)
{
	%count = %objects.getCount();
	%i = 0;
	while (%i < %count)
	{
		%object = %objects.getObject(%i);
		%object.sortPos = "";
		%i = %i + 1.0;
	}
	return;
}
function AlignTools::align(%objects, %side, %dir)
{
	%objectPos = getWord(%objects.getPosition(), %dir);
	%halfSize = getWord(%objects.getSize(), %dir) * 0.5;
	%alignPos = %objectPos + %halfSize * %side;
	AlignTools::alignToPosition(%objects, %side, %dir, %alignPos);
	return;
}
function AlignTools::alignToCamera(%objects, %scenegraph, %side, %dir)
{
	%camPos = getWord(cameraPosition, %dir);
	%camHalfSize = getWord(cameraSize, %dir) * 0.5;
	%alignPos = %camPos + %camHalfSize * %side;
	AlignTools::alignToPosition(%objects, %side, %dir, %alignPos);
	return;
}
function AlignTools::alignToPosition(%objects, %side, %dir, %alignPos)
{
	%count = %objects.getCount();
	%i = 0;
	while (%i < %count)
	{
		%object = %objects.getObject(%i);
		%position = %object.getPosition();
		%halfSize = getWord(%object.getSize(), %dir) * 0.5;
		%newPosition = %alignPos + %halfSize * -1.0 * %side;
		%position = setWord(%position, %dir, %newPosition);
		%object.setPosition(%position);
		%i = %i + 1.0;
	}
	return;
}
function AlignTools::distribute(%objects, %side, %dir)
{
	%count = %objects.getCount();
	if (%count < 1.0)
	{
		return;
	}
	%alignLeftPos = sortPos;
	%alignRightPos = sortPos;
	%spacing = %alignRightPos - %alignLeftPos / %count - 1.0;
	AlignTools::distributeToSpacing(%objects, %side, %dir, %spacing);
	return;
}
function AlignTools::distributeToCamera(%objects, %scenegraph, %side, %dir)
{
	%count = %objects.getCount();
	if (%count < 2.0)
	{
		return;
	}
	%camPos = getWord(cameraPosition, %dir);
	%camHalfSize = getWord(cameraSize, %dir) * 0.5;
	%leftCam = %camPos - %camHalfSize;
	%rightCam = %camPos + %camHalfSize;
	%firstObj = %objects.getObject("0");
	%firstPos = getWord(Position, %dir);
	%firstHalfSize = getWord(size, %dir) * 0.5;
	%firstNewPos = %leftCam + %firstHalfSize;
	%diff = %firstNewPos - %firstPos;
	%firstObj.Position = setWord(Position, %dir, %firstNewPos);
	%firstObj.sortPos = sortPos + %diff;
	%lastObj = %objects.getObject(%count - 1.0);
	%lastPos = getWord(Position, %dir);
	%lastHalfSize = getWord(size, %dir) * 0.5;
	%lastNewPos = %rightCam - %lastHalfSize;
	%diff = %lastNewPos - %lastPos;
	%lastObj.Position = setWord(Position, %dir, %lastNewPos);
	%lastObj.sortPos = sortPos + %diff;
	%spacing = sortPos - sortPos / %count - 1.0;
	AlignTools::distributeToSpacing(%objects, %side, %dir, %spacing);
	return;
}
function AlignTools::distributeToSpacing(%objects, %side, %dir, %spacing)
{
	%count = %objects.getCount();
	%i = 1;
	while (%i < %count - 1.0)
	{
		%prevObject = %objects.getObject(%i - 1.0);
		%object = %objects.getObject(%i);
		%space = sortPos - sortPos;
		%diff = %spacing - %space;
		%newPos = getWord(Position, %dir) + %diff;
		%object.Position = setWord(Position, %dir, %newPos);
		%object.sortPos = sortPos + %diff;
		%i = %i + 1.0;
	}
	return %object;
}
function AlignTools::matchSize(%objects, %dir)
{
	if (%dir == 2.0)
	{
		AlignTools::matchSize(%objects, "0");
		AlignTools::matchSize(%objects, "1");
		return;
	}
	%count = %objects.getCount();
	%i = 0;
	while (%i < %count)
	{
		%object = %objects.getObject(%i);
		%newSize = getWord(size, %dir);
		if (%biggest $= "" || %newSize > %biggest)
		{
			%biggest = %newSize;
		}
		%i = %i + 1.0;
	}
	AlignTools::matchSizeToAmount(%objects, %dir, %biggest);
	return;
}
function AlignTools::matchSizeToCamera(%objects, %scenegraph, %dir)
{
	if (%dir == 2.0)
	{
		AlignTools::matchSizeToCamera(%objects, %scenegraph, "0");
		AlignTools::matchSizeToCamera(%objects, %scenegraph, "1");
		return;
	}
	%size = getWord(cameraSize, %dir);
	AlignTools::matchSizeToAmount(%objects, %dir, %size);
	return;
}
function AlignTools::matchSizeToAmount(%objects, %dir, %size)
{
	%count = %objects.getCount();
	%i = 0;
	while (%i < %count)
	{
		%object = %objects.getObject(%i);
		%object.size = setWord(size, %dir, %size);
		%i = %i + 1.0;
	}
	return;
}
function AlignTools::space(%objects, %dir)
{
	%count = %objects.getCount();
	if (%count < 2.0)
	{
		return;
	}
	%firstObject = %objects.getObject("0");
	%left = getWord(Position, %dir) - getWord(size, %dir) * 0.5;
	%lastObject = %objects.getObject(%count - 1.0);
	%right = getWord(Position, %dir) + getWord(size, %dir) * 0.5;
	%totalSpace = %right - %left;
	%occupiedSpace = 0;
	%i = 0;
	while (%i < %count)
	{
		%object = %objects.getObject(%i);
		%space = getWord(size, %dir);
		%occupiedSpace = %occupiedSpace + %space;
		%i = %i + 1.0;
	}
	%spacing = %totalSpace - %occupiedSpace / %count - 1.0;
	AlignTools::spaceToAmount(%objects, %dir, %spacing);
	return;
}
function AlignTools::spaceToCamera(%objects, %scenegraph, %dir)
{
	%count = %objects.getCount();
	if (%count < 2.0)
	{
		return;
	}
	%camPos = getWord(cameraPosition, %dir);
	%camHalfSize = getWord(cameraSize, %dir) * 0.5;
	%leftCam = %camPos - %camHalfSize;
	%rightCam = %camPos + %camHalfSize;
	%firstObj = %objects.getObject("0");
	%firstPos = getWord(Position, %dir);
	%firstHalfSize = getWord(size, %dir) * 0.5;
	%firstNewPos = %leftCam + %firstHalfSize;
	%diff = %firstNewPos - %firstPos;
	%firstObj.Position = setWord(Position, %dir, %firstNewPos);
	%firstObj.sortPos = sortPos + %diff;
	%lastObj = %objects.getObject(%count - 1.0);
	%lastPos = getWord(Position, %dir);
	%lastHalfSize = getWord(size, %dir) * 0.5;
	%lastNewPos = %rightCam - %lastHalfSize;
	%diff = %lastNewPos - %lastPos;
	%lastObj.Position = setWord(Position, %dir, %lastNewPos);
	%lastObj.sortPos = sortPos + %diff;
	%totalSpace = %rightCam - %leftCam;
	%occupiedSpace = 0;
	%i = 0;
	while (%i < %count)
	{
		%object = %objects.getObject(%i);
		%space = getWord(size, %dir);
		%occupiedSpace = %occupiedSpace + %space;
		%i = %i + 1.0;
	}
	%spacing = %totalSpace - %occupiedSpace / %count - 1.0;
	AlignTools::spaceToAmount(%objects, %dir, %spacing);
	return;
}
function AlignTools::spaceToAmount(%objects, %dir, %spacing)
{
	%count = %objects.getCount();
	%i = 1;
	while (%i < %count - 1.0)
	{
		%prevObject = %objects.getObject(%i - 1.0);
		%object = %objects.getObject(%i);
		%pos = getWord(Position, %dir);
		%pos = %pos + getWord(size, %dir) * 0.5;
		%pos = %pos + %spacing;
		%pos = %pos + getWord(size, %dir) * 0.5;
		%object.Position = setWord(Position, %dir, %pos);
		%i = %i + 1.0;
	}
	return;
}
