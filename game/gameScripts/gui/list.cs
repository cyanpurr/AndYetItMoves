// list.cs.dso
$listGap = 10;
$scrollBarWidth = 40;
$listTitleBarOffset = 24;
$listTitleBarHeight = 32;
function listContainer::createInstance(%container, %columns, %alignments, %offsets, %makeTitle, %rowHeight, %notSelectable, %colorCodeBy)
{
	%position = "0 0";
	%extent = %container.getExtent();
	if (%makeTitle)
	{
		%columnContainerExtent = getX(%extent) - $scrollBarWidth SPC getY(%extent) - $listTitleBarOffset;
		%position = "0" SPC $listTitleBarOffset;
		%scrollContainerExtent = getX(%extent) SPC getY(%extent) - $listTitleBarOffset;
	}
	else
	{
		%columnContainerExtent = getX(%extent) - $scrollBarWidth SPC getY(%extent);
		%scrollContainerExtent = %extent;
	}
	%this = new GuiControl(Name : "")
	{
		canSaveDynamicFields = "1";
		isContainer = "1";
		class = "ListContainer";
		Profile = "AyimEmptyProfile";
		HorizSizing = "relative";
		VertSizing = "bottom";
		Position = "0 0";
		Extent = %columnContainerExtent;
		MinExtent = "8 2";
		canSave = "1";
		Visible = "1";
		hovertime = "1000";
	}
	%width = getX(%columnContainerExtent);
	%roundError = 0;
	%restWidth = %width;
	%i = 0;
	while (%i < getWordCount(%offsets))
	{
		%offset = getWord(%offsets, %i);
		if (%offset > 1.0)
		{
			%restWidth = %restWidth - %offset;
		}
		%i = %i + 1.0;
	}
	debugEcho("rest words are" SPC %restWidth SPC "from" SPC %width SPC "for" SPC %container);
	%i = 0;
	while (%i < getWordCount(%offsets))
	{
		%offset = getWord(%offsets, %i);
		if (%offset <= 1.0)
		{
			%offset = %restWidth * getWord(%offsets, %i);
		}
		%newValue = %offset;
		%roundError = %roundError + mRound(%newValue) - %newValue;
		%offsets = setWord(%offsets, %i, mRound(%newValue));
		%i = %i + 1.0;
	}
	%lastIndex = getWordCount(%offsets) - 1.0;
	%offsets = setWord(%offsets, %lastIndex, getWord(%offsets, %lastIndex) - mCeil(%roundError));
	if (%makeTitle)
	{
		%this.createTitleRow(%container, %columnContainerExtent);
	}
	%this.scrollline = new GuiChunkedBitmapCtrl(Name : "")
	{
		Profile = "GuiDefaultProfile";
		HorizSizing = "left";
		VertSizing = "relative";
		Position = getX(%position) + getX(%columnContainerExtent) SPC getY(%position) + 10.0;
		Extent = $scrollBarWidth SPC getY(%scrollContainerExtent) - 20.0;
		bitmap = ['"~/"', '$DataFolder', '"/images/Menu/scrollbar_line.png"'];
		tile = "1";
		Visible = "0";
	}
	%container.addGuiControl(scrollline);
	%scrollContainer = new GuiScrollCtrl(Name : "")
	{
		canSaveDynamicFields = "1";
		isContainer = "1";
		Profile = "AyimScrollProfile";
		HorizSizing = "width";
		VertSizing = "relative";
		Position = %position;
		MinExtent = "8 2";
		canSave = "1";
		Visible = "1";
		hovertime = "1000";
		willFirstRespond = "1";
		hScrollBar = "alwaysOff";
		vScrollBar = "dynamic";
		constantThumbHeight = "1";
		childMargin = "0 0";
	}
	%scrollContainer.setExtent(getX(%scrollContainerExtent), getY(%scrollContainerExtent));
	%container.addGuiControl(%scrollContainer);
	%scrollContainer.addGuiControl(%this);
	%this.scrollContainer = %scrollContainer;
	%container.listContainer = %this;
	%container.titleRow = %titleRow;
	if (%rowHeight $= "")
	{
		%this.rowHeight = "24";
	}
	else
	{
		%this.rowHeight = %rowHeight;
	}
	%this.columnTitles = %columns;
	%this.columns = new SimSet(Name : "");
	%x = 1;
	%i = 0;
	while (%i < getWordCount(%offsets))
	{
		if (%i > 0.0)
		{
			%x = %x + getWord(%offsets, %i - 1.0);
		}
		columns.add(ListColumn::createInstance(%this, %x, getField(%columns, %i), getWord(%alignments, %i), getWord(%offsets, %i) / getX(%columnContainerExtent), rowHeight));
		%i = %i + 1.0;
	}
	%this.notSelectable = %notSelectable;
	%this.selectedRow = -1.0;
	%this.rowMarkers = new SimSet(Name : "");
	%this.setColorCodeBy(%colorCodeBy);
	%this.addTitles();
	return %this;
	return %this;
}
function listContainer::createTitleRow(%this, %container, %extents)
{
	%this.titleRow = new GuiControl(Name : "")
	{
		canSaveDynamicFields = "0";
		isContainer = "1";
		Profile = "AyimEmptyProfile";
		HorizSizing = "relative";
		VertSizing = "relative";
		Position = "1 0";
		Extent = getX(%extents) SPC $listTitleBarHeight;
		canSave = "1";
		Visible = "1";
		tooltipprofile = "AyimToolTipProfile";
		hovertime = "1000";
	}
	%container.addGuiControl(titleRow);
	return;
}
function listContainer::addTitles(%this)
{
	if (!(isObject(titleRow)))
	{
		return isObject(titleRow);
	}
	%this.clearTitles();
	%currentOffset = 0;
	%i = 0;
	while (%i < columns.getCount())
	{
		%currentColumn = columns.getObject(%i);
		eval("%text = " @ Title @ ";");
		%titleExtent = getX(%currentColumn.getExtent()) - $listGap SPC $listTitleBarHeight;
		%newTitle = new GuiTextCtrl(Name : "")
		{
			canSaveDynamicFields = "1";
			isContainer = "0";
			Profile = "AyimMenuTextCenterProfile";
			HorizSizing = "relative";
			VertSizing = "top";
			Position = %currentOffset SPC "0";
			Extent = %titleExtent;
			MinExtent = getX(%titleExtent) SPC "0";
			canSave = "1";
			Visible = "1";
			hovertime = "1000";
			text = %text SPC "";
			maxLength = "1024";
			tooltipprofile = "AyimToolTipProfile";
			ToolTip = %text;
		}
		if (alignment $= "left")
		{
			%newTitle.setProfile("AyimMenuTextProfile");
		}
		else
		{
			if (alignment $= "right")
			{
				%newTitle.setProfile("AyimMenuTextRightProfile");
			}
		}
		titleRow.addGuiControl(%newTitle);
		%currentOffset = %currentOffset + getX(%currentColumn.getExtent());
		%i = %i + 1.0;
	}
	return columns.getCount();
}
function listContainer::clearTitles(%this)
{
	while (titleRow.getCount() > 0.0)
	{
		titleRow.getObject("0").delete();
	}
	return titleRow.getCount();
}
function listContainer::localizeTitles(%this, %titles)
{
	if (getWordCount(%titles) != columns.getCount())
	{
		debugWarn("WARNING: ListContainer::localizeTitles count of passed titles:" SPC getWordCount(%titles) SPC "is not equal to count of columns:" SPC columns.getCount() SPC "we will only use as many titles as there are columsn" SPC %titles);
	}
	%i = 0;
	while (%i < columns.getCount())
	{
		columns.getObject(%i).Title = getWord(%title, %i);
		%i = %i + 1.0;
	}
	%this.addTitles();
	return;
}
function listContainer::setColorCodeBy(%this, %colorCodeBy)
{
	%index = getFieldIndex(columnTitles, %colorCodeBy);
	if (%index > -1.0)
	{
		%this.colorCodeIndex = %index;
		%this.lastColorCodeValue = "";
		%this.color1 = "AyimListItemColor1Profile";
		%this.color2 = "AyimListItemColor2Profile";
		%this.currentColor = color1;
	}
	else
	{
		%this.currentColor = "AyimEmptyProfile";
	}
	return;
}
function listContainer::setVisible(%this, %visible)
{
	%this.getParent().setVisible(%visible);
	titleRow.setVisible(%visible);
	scrollline.setVisible("0");
	return;
}
function listContainer::addRow(%this, %items, %subTexts, %altTextProfileIndex, %altTextProfileName, %notAvailable)
{
	if (getFieldCount(%items) > columns.getCount())
	{
		echo("WARNING: listcontainer::addrow: trying to add a row with more items then columsn in this container:" SPC columns.getCount() SPC "will truncuate the items");
	}
	%this.setExtent(getX(%this.getExtent()), rowMarkers.getCount() + 1.0 * rowHeight);
	if (!(scrollline.isVisible()) && getY(%this.getExtent()) > getY(%this.getParent().getExtent()))
	{
		scrollline.setVisible("1");
	}
	if (getField(%items, colorCodeIndex) != lastColorCodeValue)
	{
		if (currentColor $= color1)
		{
			%this.currentColor = color2;
		}
		else
		{
			%this.currentColor = color1;
		}
		%this.lastColorCodeValue = getField(%items, colorCodeIndex);
	}
	%rowMarker = new GuiControl(Name : "")
	{
		canSaveDynamicFields = "1";
		isContainer = "0";
		Profile = currentColor;
		HorizSizing = "width";
		VertSizing = "bottom";
		Position = "0" SPC rowMarkers.getCount() * rowHeight;
		Extent = getX(%this.getExtent()) SPC rowHeight;
		MinExtent = "8 2";
		canSave = "1";
		Visible = "1";
		hovertime = "1000";
		colorCode = currentColor;
	}
	rowMarkers.add(%rowMarker);
	%this.addGuiControl(%rowMarker);
	%this.reorderChild(%rowMarker, columns.getObject("0"));
	%i = 0;
	while (%i < getFieldCount(%items))
	{
		if (%notAvailable)
		{
			%textProfile = "AyimListItemHeaderColorNAProfile AyimListItemColorNAProfile";
		}
		else
		{
			if (%i == %altTextProfileIndex)
			{
				%textProfile = %altTextProfileName;
				break;
			}
			%textProfile = "";
		}
		columns.getObject(%i).addItem(getField(%items, %i), getField(%subTexts, %i), %textProfile);
		%i = %i + 1.0;
	}
	return getFieldCount(%items);
}
function listContainer::setRowNotSelectable(%this, %index)
{
	%rowMarker = rowMarkers.getObject(%index);
	%rowMarker.notSelectable = "1";
	%rowMarker.setProfile("AyimListItemHighlightedProfile");
	return;
}
function listContainer::setRowNotAvailable(%this, %index)
{
	%rowMarker = rowMarkers.getObject(%index);
	%rowMarker.setProfile("AyimListItemNAProfile");
	return;
}
function listContainer::getColumns(%this)
{
	%columns = "";
	%i = 0;
	while (%i < columns.getCount())
	{
		%columns = %columns SPC columns.getObject(%i);
		%i = %i + 1.0;
	}
	return ltrim(%columns);
	return ltrim(%columns);
}
function listContainer::clearList(%this)
{
	%i = 0;
	while (%i < columns.getCount())
	{
		columns.getObject(%i).clearColumn();
		%i = %i + 1.0;
	}
	while (rowMarkers.getCount() > 0.0)
	{
		rowMarkers.getObject("0").delete();
	}
	%this.selectedRow = -1.0;
	scrollline.setVisible("0");
	%this.addTitles();
	return;
}
function listContainer::setRowSelected(%this, %index, %dontUnselect)
{
	if (%index < items.getCount() && !(notSelectable))
	{
		%unselectIndex = selectedRow;
		if (%unselectIndex >= 0.0 && !(%dontUnselect))
		{
			%selectedRow = rowMarkers.getObject(%unselectIndex);
			%selectedRow.setProfile(colorCode);
		}
		%this.selectedRow = %index;
		rowMarkers.getObject(%index).setProfile("AyimListItemSelectedProfile");
		triggerEvent("OnRowSelected");
	}
	else
	{
		echo("WARNING: listcontainer::setrowselected: couldn't select row. index is" SPC %index SPC "row count:" SPC items.getCount());
	}
	return;
}
function listContainer::setRowHighlighted(%this, %index, %highlight)
{
	if (%index < items.getCount())
	{
		%selectedRow = rowMarkers.getObject(%unselectIndex);
		if (%highlight)
		{
			%selectedRow.setProfile("AyimListItemHighlightedProfile");
			break;
		}
		%selectedRow.setProfile(colorCode);
	}
	return;
}
function listContainer::getSelectedItems(%this)
{
	if (selectedRow == -1.0)
	{
		return -1.0;
	}
	%items = selectedRow;
	%i = 0;
	while (%i < columns.getCount())
	{
		%items = %items TAB text;
		%i = %i + 1.0;
	}
	return %items;
	return %items;
}
function listContainer::getSelectedPosition(%this)
{
	if (selectedRow == -1.0)
	{
		return -1.0;
	}
	return items.getObject(selectedRow).getGlobalPosition();
	return items.getObject(selectedRow).getGlobalPosition();
}
function listContainer::getSelectedRow(%this)
{
	return selectedRow;
	return selectedRow;
}
function listContainer::scrollToIndex(%this, %index)
{
	%scrollToPosition = %index * rowHeight - getY(scrollContainer.getExtent()) * 0.6669999957084656 + rowHeight / 2.0;
	scrollContainer.setScrollPosition("0", %scrollToPosition);
	return;
}
function ListColumn::createInstance(%list, %x, %title, %alignment, %relativeWidth, %verticalOffset)
{
	%listWidth = getX(%list.getExtent());
	%this = new GuiControl(Name : "")
	{
		canSaveDynamicFields = "1";
		isContainer = "1";
		class = "ListColumn";
		Profile = "AyimEmptyProfile";
		HorizSizing = "relative";
		VertSizing = "height";
		Position = %x SPC "0";
		Extent = %listWidth * %relativeWidth SPC "0";
		MinExtent = "8 2";
		canSave = "1";
		Visible = "1";
		hovertime = "1000";
	}
	%list.addGuiControl(%this);
	%this.alignment = %alignment;
	%this.list = %list;
	%this.verticalOffset = %verticalOffset;
	%this.Width = %listWidth * %relativeWidth;
	%this.relativeWidth = %relativeWidth;
	%this.Title = %title;
	%this.items = new SimSet(Name : "");
	return %this;
	return %this;
}
function ListColumn::addItem(%this, %item, %subText, %textProfile)
{
	%newHeight = items.getCount() + 1.0 * verticalOffset;
	%parentExtent = %this.getParent().getExtent();
	if (getY(%parentExtent) < %newHeight)
	{
		%this.getParent().setExtent(getX(%parentExtent), %newHeight);
	}
	%this.setExtent(getX(%this.getExtent()), %newHeight);
	%positionY = items.getCount() * verticalOffset;
	if (Title $= "$lbl_levelDetail_ghost")
	{
		if (%item > -1.0)
		{
			%item = "â¢";
			break;
		}
		%item = "";
	}
	%listItem = new GuiControl(Name : "")
	{
		canSaveDynamicFields = "1";
		isContainer = "1";
		class = "ListItem";
		Profile = "AyimEmptyProfile";
		HorizSizing = "width";
		VertSizing = "bottom";
		text = %item;
		wrap = "0";
	}
	%listItem.setPosition("0", %positionY);
	%listItem.setExtent(getX(%this.getExtent()), verticalOffset);
	if (strpos(%item, "game/" @ $DataFolder @ "/images/Menu") != -1.0 || %subText != "")
	{
		if (strpos(%item, "game/" @ $DataFolder @ "/images/Menu") != -1.0)
		{
			%icon = new GuiBitmapCtrl(Name : "")
			{
				canSaveDynamicFields = "0";
				isContainer = "1";
				Profile = "AyimEmptyProfile";
				HorizSizing = "right";
				VertSizing = "bottom";
				Position = "8 8";
				Extent = "64 64";
				MinExtent = "8 2";
				canSave = "1";
				Visible = "1";
				hovertime = "1000";
				bitmap = %item;
				wrap = "0";
			}
			%listItem.addGuiControl(%icon);
		}
		else
		{
			%header = new GuiTextCtrl(Name : "")
			{
				canSaveDynamicFields = "0";
				isContainer = "0";
				Profile = "AyimHeader2Profile";
				HorizSizing = "right";
				VertSizing = "bottom";
				Position = "8 8";
				Extent = "8 24";
				text = %item;
				MinExtent = "8 2";
				canSave = "1";
				Visible = "1";
				hovertime = "1000";
				maxLength = "1024";
			}
			%listItem.addGuiControl(%header);
			%itemWidth = getX(%this.getExtent()) - 32.0;
			%description = new GuiMLTextCtrl(Name : "")
			{
				canSaveDynamicFields = "0";
				isContainer = "0";
				Profile = "AyimMenuTextProfile";
				HorizSizing = "width";
				VertSizing = "bottom";
				SizeMargin = "0 0";
				PositionAbsolute = "0";
				Position = "8 48";
				Extent = %itemWidth SPC verticalOffset - 32.0;
				MinExtent = "8 2";
				canSave = "1";
				Visible = "1";
				hovertime = "1000";
				lineSpacing = "10";
				allowColorChars = "0";
				maxChars = "-1";
				text = %subText;
			}
			%listItem.addGuiControl(%description);
			if (%textProfile != "")
			{
				%header.setProfile(getWord(%textProfile, "0"));
				%description.setProfile(getWord(%textProfile, "1"));
			}
		}
		if (!(notSelectable))
		{
			%button = new GuiButtonCtrl(Name : "")
			{
				canSaveDynamicFields = "1";
				isContainer = "0";
				class = "ListButton";
				Profile = "AyimListItemProfile";
				HorizSizing = "width";
				VertSizing = "height";
				Position = "0 0";
				Extent = Width SPC verticalOffset;
				MinExtent = "8 2";
				canSave = "1";
				Visible = "1";
				tooltipprofile = "AyimToolTipProfile";
				ToolTip = "";
				hovertime = "1500";
				groupNum = "-1";
				buttonType = "PushButton";
				UseMouseEvents = "0";
				bitmap = ['"game/"', '$DataFolder', '"/images/Menu/empty"'];
			}
			%button.listIndex = items.getCount();
			%button.listContainer = %this.getParent();
			%listItem.addGuiControl(%button);
		}
	}
	else
	{
		%button = new GuiButtonCtrl(Name : "")
		{
			canSaveDynamicFields = "1";
			class = "ListButton";
			isContainer = "0";
			Profile = "AyimListItemProfile";
			HorizSizing = "relative";
			VertSizing = "relative";
			MinExtent = "8 2";
			canSave = "1";
			Visible = "1";
			tooltipprofile = "AyimToolTipProfile";
			ToolTip = %item;
			hovertime = "1500";
			text = %item;
			groupNum = "-1";
			buttonType = "PushButton";
			UseMouseEvents = "0";
			bitmap = $BTN_EMPTY;
			dontHideDetail = "1";
		}
		%button.setPosition("0", "0");
		%button.setExtent(getX(%this.getExtent()) - $listGap, verticalOffset);
		if (alignment $= "left")
		{
			%button.setProfile("AyimListItemLeftProfile");
		}
		else
		{
			if (alignment $= "right")
			{
				%button.setProfile("AyimListItemRightProfile");
			}
		}
		if (%textProfile != "")
		{
			%button.setProfile(%textProfile);
		}
		%button.listIndex = items.getCount();
		%button.listContainer = %this.getParent();
		%listItem.addGuiControl(%button);
	}
	%this.addGuiControl(%listItem);
	items.add(%listItem);
	return;
}
function ListColumn::clearColumn(%this)
{
	while (items.getCount() > 0.0)
	{
		%currentItem = items.getObject("0");
		while (%currentItem.getCount() > 0.0)
		{
			%currentItem.getObject("0").delete();
		}
		%currentItem.delete();
	}
	return items.getCount();
}
function ListButton::onAction(%this)
{
	if (notSelectable)
	{
		return listContainer;
	}
	listContainer.setRowSelected(listIndex);
	return;
}
function ListItem::setItemSelected(%this, %selected)
{
	if (%selected)
	{
		%this.setProfile("AyimListItemSelectedProfile");
	}
	else
	{
		%this.setProfile("AyimEmptyProfile");
	}
	return;
}
