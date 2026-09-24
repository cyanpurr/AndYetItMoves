// language.cs.dso
$langTable = new LangTable(Name : "");
$I18N::default = $langTable;
setCoreLangTable($I18N::default);
$LanguageCode = getLanguageCode();
if (getOS() != "wii")
{
	$LanguageCode = 1;
}
if ($LanguageCode == 2.0)
{
	$langTable.addLanguage("Language/GermanEU.lso", "German");
}
else
{
	if ($LanguageCode == 3.0)
	{
		$langTable.addLanguage("Language/FrenchEU.lso", "French");
		break;
	}
	if ($LanguageCode == 4.0)
	{
		$langTable.addLanguage("Language/SpanishEU.lso", "Spanish");
		break;
	}
	if ($LanguageCode == 5.0)
	{
		$langTable.addLanguage("Language/ItalianEU.lso", "Italian");
		break;
	}
	if ($LanguageCode == 6.0)
	{
		$langTable.addLanguage("Language/DutchEU.lso", "Dutch");
		break;
	}
	$langTable.addLanguage("Language/EnglishEU.lso", "English");
}
exec("Language/EnglishEU.cs");
$langTable.setDefaultLanguage("0");
$langTable.setCurrentLanguage("0");
function L(%phraseID)
{
	echo("L pID" SPC %phraseID SPC "langTable" SPC $langTable SPC "return" SPC $langTable.getString(%phraseID));
	return $langTable.getString(%phraseID);
	return $langTable.getString(%phraseID);
}
function LIDStr(%phraseIDString)
{
	if (%phraseIDString $= "")
	{
		return "";
	}
	eval("%phraseID = $" @ %phraseIDString @ ";");
	return $langTable.getString(%phraseID);
	return $langTable.getString(%phraseID);
}
