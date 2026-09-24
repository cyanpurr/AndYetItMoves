// backgroundPaper.cs.dso
if (!(isObject(BeBackgroundPaper)))
{
	%template = new BehaviorTemplate(Name : BeBackgroundPaper);
	%template.friendlyName = "Background Paper";
	%template.behaviorType = "Visual";
	%template.description = "fill the owner wir texture, random distribution of frames is done automatically";
	%template.addBehaviorField(Texture, "use this texture to fill the sceneobject, random distribution of frames is done automatically", object, null, t2dSceneObject);
}
function BeBackgroundPaper::onBehaviorAdd(%this)
{
	subscribeToEvents(%this, "onLevelLoadFinished1");
	return;
}
function BeBackgroundPaper::onLevelLoadFinished1(%this)
{
	%owner = Owner;
	%imagemap = Texture.getImageMap();
	%numFrames = %imagemap.getFrameCount();
	%this.probabilities = "0.72 0.13 0.13 0.02";
	%i = 0;
	while (%i < %numFrames)
	{
		%probability[%i] = getWord(probabilities, %i);
		%i = %i + 1.0;
	}
	%size = %owner.getSize();
	%sizeX = getWord(%size, "0");
	%sizeY = getWord(%size, "1");
	%left = %owner.getPositionX() - %sizeX / 2.0;
	%top = %owner.getPositionY() - %sizeY / 2.0;
	%tileSize = Texture.getSize();
	%sizeScale = 1.2999999523162842;
	%tileSizeX = getWord(%tileSize, "0") * %sizeScale;
	%tileSizeY = getWord(%tileSize, "1") * %sizeScale;
	%numTilesX = mCeil(%sizeX / %tileSizeX);
	%numTilesY = mCeil(%sizeY / %tileSizeY);
	%tileMap = new t2dTileMap(Name : "")
	{
		scenegraph = scenegraph;
		size = %size;
		Visible = "1";
		Immovable = "0";
	}
	%layer = %tileMap.createTileLayer(%numTilesX, %numTilesY, %tileSizeX, %tileSizeY);
	%layer.setPosition(%owner.getPosition());
	%layer.setSize(%size);
	scenegraph.setLayerDrawOrder(%layer, "BACK");
	%i = 0;
	while (%i < %numTilesX)
	{
		%j = 0;
		while (%j < %numTilesY)
		{
			%rnd = getRandom();
			%totalP = 0;
			%p = 0;
			while (%p < %numFrames)
			{
				%totalP = %totalP + %probability[%p];
				if (%rnd < %totalP)
				{
					%frame = %p;
					break;
				}
				%p = %p + 1.0;
			}
			%success = %layer.setStaticTile(%i, %j, %imagemap, %frame);
			%j = %j + 1.0;
		}
		%i = %i + 1.0;
	}
	%owner.safeDelete();
	%layer.addDependentBehavior("BeDekoTilemap");
	return;
}
