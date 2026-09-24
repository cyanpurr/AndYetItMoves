// xml.cs.dso
function XML::beginWrite(%this, %fileName)
{
	if (!(isWriteableFileName(%fileName)))
	{
		error("XML::beginWrite - Failed to write to file" @ %fileName @ ".");
		return "0";
	}
	%this.FileObject = new SimXMLDocument(Name : "");
	FileObject.addHeader();
	%this.fileName = %fileName;
	return "1";
	return "1";
}
function XML::beginRead(%this, %fileName)
{
	%this.FileObject = new SimXMLDocument(Name : "");
	if (!(FileObject.loadFile(%fileName)))
	{
		return "0";
	}
	%this.index = "0";
	%this.parentIndex = "";
	return "1";
	return "1";
}
function XML::endWrite(%this)
{
	FileObject.saveFile(fileName);
	FileObject.delete();
	return;
}
function XML::endRead(%this)
{
	FileObject.delete();
	return;
}
function XML::writeHeader(%this, %documentType, %target, %version, %creator)
{
	FileObject.addComment("Torque Game Builder - http://www.garagegames.com");
	FileObject.addComment("Type: " @ %documentType);
	FileObject.addComment("Target: " @ %target);
	FileObject.addComment("Version: " @ %version);
	FileObject.addComment("Creator: Torque Game Builder");
	return;
}
function XML::readHeader(%this)
{
	FileObject.readComment("1");
	%documentType = FileObject.readComment("2");
	%target = FileObject.readComment("3");
	%version = FileObject.readComment("4");
	%creator = FileObject.readComment("5");
	%documentType = getSubStr(%documentType, "6", strlen(%documentType));
	%target = getSubStr(%target, "8", strlen(%target));
	%version = getSubStr(%version, "9", strlen(%version));
	%creator = getSubStr(%creator, "9", strlen(%creator));
	return %documentType TAB %target TAB %version TAB %creator;
	return %documentType TAB %target TAB %version TAB %creator;
}
function XML::writeDocument(%this, %document)
{
	%document.save(%this);
	return FileObject.saveFile(fileName);
	return FileObject.saveFile(fileName);
}
function XML::readDocument(%this, %document)
{
	if (!(%document.isMethod("load")))
	{
		return "";
	}
	%document.load(%this);
	return %document;
	return %document;
}
function XML::writeClassBegin(%this, %class, %name)
{
	FileObject.pushNewElement(%class);
	if (%name != "")
	{
		FileObject.setAttribute("name", %name);
	}
	return;
}
function XML::writeClassEnd(%this)
{
	FileObject.popElement();
	return;
}
function XML::readNextClass(%this)
{
	%class = "";
	%name = "";
	if (FileObject.pushChildElement(index))
	{
		%this.parentIndex = index SPC parentIndex;
		%this.index = "0";
		%class = FileObject.elementValue();
		%name = FileObject.attribute("name");
	}
	if (%class $= "")
	{
		return "";
	}
	return trim(%class SPC %name);
	return trim(%class SPC %name);
}
function XML::readClassBegin(%this, %class, %index)
{
	if (FileObject.pushFirstChildElement(%class))
	{
		if (%index $= "")
		{
			%index = 0;
		}
		%i = 0;
		while (%i < %index)
		{
			if (!(FileObject.nextSiblingElement(%class)))
			{
				FileObject.popElement();
				return "0";
			}
			%i = %i + 1.0;
		}
		%this.parentIndex = index SPC parentIndex;
		%this.index = "0";
		return "1";
	}
	return "0";
	return "0";
}
function XML::readClassEnd(%this)
{
	FileObject.popElement();
	%this.index = firstWord(parentIndex) + 1.0;
	%this.parentIndex = removeWord(parentIndex, "0");
	return;
}
function XML::writeField(%this, %field, %value)
{
	FileObject.pushNewElement(%field);
	FileObject.addText(%value);
	FileObject.popElement();
	return;
}
function XML::readField(%this, %field)
{
	%value = "";
	if (FileObject.pushFirstChildElement(%field))
	{
		%this.index = index + 1.0;
		%value = FileObject.getText();
		FileObject.popElement();
	}
	return %value;
	return %value;
}
function XML::writePoint2F(%this, %field, %value)
{
	FileObject.pushNewElement(%field);
	%this.writeField("X", getWord(%value, "0"));
	%this.writeField("Y", getWord(%value, "1"));
	FileObject.popElement();
	return;
}
function XML::readPoint2F(%this, %field)
{
	%value = "";
	if (FileObject.pushFirstChildElement(%field))
	{
		%index = index;
		%value = %this.readField("X") SPC %this.readField("Y");
		FileObject.popElement();
		%this.index = %index + 1.0;
	}
	return %value;
	return %value;
}
function XML::writePoint3F(%this, %field, %value)
{
	FileObject.pushNewElement(%field);
	%this.writeField("X", getWord(%value, "0"));
	%this.writeField("Y", getWord(%value, "1"));
	%this.writeField("Z", getWord(%value, "2"));
	FileObject.popElement();
	return;
}
function XML::readPoint3F(%this, %field)
{
	%value = "";
	if (FileObject.pushFirstChildElement(%field))
	{
		%index = index;
		%value = %this.readField("X") SPC %this.readField("Y") SPC %this.readField("Z");
		FileObject.popElement();
		%this.index = %index + 1.0;
	}
	return %value;
	return %value;
}
function XML::writePoint4F(%this, %field, %value)
{
	FileObject.pushNewElement(%field);
	%this.writeField("X", getWord(%value, "0"));
	%this.writeField("Y", getWord(%value, "1"));
	%this.writeField("Z", getWord(%value, "2"));
	%this.writeField("W", getWord(%value, "3"));
	FileObject.popElement();
	return;
}
function XML::readPoint4F(%this, %field)
{
	%value = "";
	if (FileObject.pushFirstChildElement(%field))
	{
		%index = index;
		%value = %this.readField("X") SPC %this.readField("Y") SPC %this.readField("Z") SPC %this.readField("W");
		FileObject.popElement();
		%this.index = %index + 1.0;
	}
	return %value;
	return %value;
}
function XML::writeAttribute(%this, %field, %value)
{
	FileObject.setAttribute(%field, %value);
	return;
}
function XML::readAttribute(%this, %field)
{
	return FileObject.attribute(%field);
	return FileObject.attribute(%field);
}
function XML::writeData(%this, %field, %value)
{
	FileObject.pushNewElement(%field);
	FileObject.addData(%value);
	FileObject.popElement();
	return;
}
function XML::readData(%this, %field)
{
	%value = "";
	if (FileObject.pushFirstChildElement(%field))
	{
		%this.index = index + 1.0;
		%value = FileObject.getData();
		FileObject.popElement();
	}
	return %value;
	return %value;
}
function XML::writeBool(%this, %field, %value)
{
	%write = "false";
	if (%value)
	{
		%write = "true";
	}
	%this.writeField(%field, %write);
	return;
}
function XML::readBool(%this, %field)
{
	%val = %this.readField(%field);
	if (%val $= "")
	{
		return "";
	}
	if (%val $= "true")
	{
		return "1";
	}
	return "0";
	return "0";
}
function XML::writeValue(%this, %value)
{
	FileObject.addText(%value);
	return;
}
function XML::readValue(%this)
{
	return FileObject.getText();
	return FileObject.getText();
}
