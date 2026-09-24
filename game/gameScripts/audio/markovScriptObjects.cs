// markovScriptObjects.cs.dso
new ScriptObject(Name : waterDrops)
{
	profiles = "CaveDropPart1 CaveDropPart2 CaveDropPart3";
	transitions["CaveDropPart1", "CaveDropPart1"] = "0.2";
	transitions["CaveDropPart1", "CaveDropPart2"] = "0.4";
	transitions["CaveDropPart1", "CaveDropPart3"] = "0.4";
	transitions["CaveDropPart2", "CaveDropPart1"] = "0.2";
	transitions["CaveDropPart2", "CaveDropPart2"] = "0.4";
	transitions["CaveDropPart2", "CaveDropPart3"] = "0.4";
	transitions["CaveDropPart3", "CaveDropPart1"] = "0.4";
	transitions["CaveDropPart3", "CaveDropPart2"] = "0.3";
	transitions["CaveDropPart3", "CaveDropPart3"] = "0.3";
}
new ScriptObject(Name : caveBass)
{
	profiles = "CaveBassPart1 CaveBassPart2";
	transitions["CaveBassPart1", "CaveBassPart1"] = "0.3";
	transitions["CaveBassPart1", "CaveBassPart2"] = "0.7";
	transitions["CaveBassPart2", "CaveBassPart1"] = "0.7";
	transitions["CaveBassPart2", "CaveBassPart2"] = "0.3";
}
new ScriptObject(Name : caveHigher)
{
	profiles = "CaveHigherPart1 CaveHigherPart2 CaveHigherPart3";
	transitions["CaveHigherPart1", "CaveHigherPart1"] = "0.2";
	transitions["CaveHigherPart1", "CaveHigherPart2"] = "0.4";
	transitions["CaveHigherPart1", "CaveHigherPart3"] = "0.4";
	transitions["CaveHigherPart2", "CaveHigherPart1"] = "0.4";
	transitions["CaveHigherPart2", "CaveHigherPart2"] = "0.2";
	transitions["CaveHigherPart2", "CaveHigherPart3"] = "0.4";
	transitions["CaveHigherPart3", "CaveHigherPart1"] = "0.4";
	transitions["CaveHigherPart3", "CaveHigherPart2"] = "0.4";
	transitions["CaveHigherPart3", "CaveHigherPart3"] = "0.2";
}
new ScriptObject(Name : caveAmbienceBeat)
{
	profiles = "CaveAmbienceBeat1 CaveAmbienceBeat2 CaveAmbienceBeat3 CaveAmbienceBeat4";
	transitions["CaveAmbienceBeat1", "CaveAmbienceBeat1"] = "0";
	transitions["CaveAmbienceBeat1", "CaveAmbienceBeat2"] = "0.3";
	transitions["CaveAmbienceBeat1", "CaveAmbienceBeat3"] = "0.4";
	transitions["CaveAmbienceBeat1", "CaveAmbienceBeat4"] = "0.3";
	transitions["CaveAmbienceBeat2", "CaveAmbienceBeat1"] = "0.3";
	transitions["CaveAmbienceBeat2", "CaveAmbienceBeat2"] = "0";
	transitions["CaveAmbienceBeat2", "CaveAmbienceBeat3"] = "0.3";
	transitions["CaveAmbienceBeat2", "CaveAmbienceBeat4"] = "0.4";
	transitions["CaveAmbienceBeat3", "CaveAmbienceBeat1"] = "0.4";
	transitions["CaveAmbienceBeat3", "CaveAmbienceBeat2"] = "0.3";
	transitions["CaveAmbienceBeat3", "CaveAmbienceBeat3"] = "0";
	transitions["CaveAmbienceBeat3", "CaveAmbienceBeat4"] = "0.3";
	transitions["CaveAmbienceBeat4", "CaveAmbienceBeat1"] = "0.3";
	transitions["CaveAmbienceBeat4", "CaveAmbienceBeat2"] = "0.4";
	transitions["CaveAmbienceBeat4", "CaveAmbienceBeat3"] = "0.3";
	transitions["CaveAmbienceBeat4", "CaveAmbienceBeat4"] = "0";
}
new ScriptObject(Name : jungleMelody)
{
	profiles = "JungleMelody1 JungleMelody2";
	transitions["JungleMelody1", "JungleMelody1"] = "0.3";
	transitions["JungleMelody1", "JungleMelody2"] = "0.7";
	transitions["JungleMelody2", "JungleMelody1"] = "0.7";
	transitions["JungleMelody2", "JungleMelody2"] = "0.3";
}
new ScriptObject(Name : jungleHigher)
{
	profiles = "JungleHigher1 JungleHigher2";
	transitions["JungleHigher1", "JungleHigher1"] = "0.3";
	transitions["JungleHigher1", "JungleHigher2"] = "0.7";
	transitions["JungleHigher2", "JungleHigher1"] = "0.7";
	transitions["JungleHigher2", "JungleHigher2"] = "0.3";
}
new ScriptObject(Name : jungleBeatShort)
{
	profiles = "JungleBeat1 JungleBeat2";
	transitions["JungleBeat1", "JungleBeat1"] = "0.5";
	transitions["JungleBeat1", "JungleBeat2"] = "0.5";
	transitions["JungleBeat2", "JungleBeat1"] = "0.5";
	transitions["JungleBeat2", "JungleBeat2"] = "0.5";
}
new ScriptObject(Name : jungleBeatAll)
{
	profiles = "JungleBeat1 JungleBeat2 JungleBeat3 JungleBeat4";
	transitions["JungleBeat1", "JungleBeat1"] = "0.1";
	transitions["JungleBeat1", "JungleBeat2"] = "0.3";
	transitions["JungleBeat1", "JungleBeat3"] = "0.3";
	transitions["JungleBeat1", "JungleBeat4"] = "0.3";
	transitions["JungleBeat2", "JungleBeat1"] = "0.3";
	transitions["JungleBeat2", "JungleBeat2"] = "0.1";
	transitions["JungleBeat2", "JungleBeat3"] = "0.3";
	transitions["JungleBeat2", "JungleBeat4"] = "0.3";
	transitions["JungleBeat3", "JungleBeat1"] = "0.3";
	transitions["JungleBeat3", "JungleBeat2"] = "0.3";
	transitions["JungleBeat3", "JungleBeat3"] = "0.1";
	transitions["JungleBeat3", "JungleBeat4"] = "0.3";
	transitions["JungleBeat4", "JungleBeat1"] = "0.3";
	transitions["JungleBeat4", "JungleBeat2"] = "0.3";
	transitions["JungleBeat4", "JungleBeat3"] = "0.3";
	transitions["JungleBeat4", "JungleBeat4"] = "0.1";
}
new ScriptObject(Name : jungleAnimals)
{
	profiles = "JungleBat JungleBird JungleSalamander JungleAnimal";
	transitions["JungleBat", "JungleBat"] = "0.25";
	transitions["JungleBat", "JungleBird"] = "0.25";
	transitions["JungleBat", "JungleSalamander"] = "0.25";
	transitions["JungleBat", "JungleAnimal"] = "0.25";
	transitions["JungleBird", "JungleBat"] = "0.25";
	transitions["JungleBird", "JungleBird"] = "0.25";
	transitions["JungleBird", "JungleSalamander"] = "0.25";
	transitions["JungleBird", "JungleAnimal"] = "0.25";
	transitions["JungleSalamander", "JungleBat"] = "0.25";
	transitions["JungleSalamander", "JungleBird"] = "0.25";
	transitions["JungleSalamander", "JungleSalamander"] = "0.25";
	transitions["JungleSalamander", "JungleAnimal"] = "0.25";
	transitions["JungleAnimal", "JungleBat"] = "0.25";
	transitions["JungleAnimal", "JungleBird"] = "0.25";
	transitions["JungleAnimal", "JungleSalamander"] = "0.25";
	transitions["JungleAnimal", "JungleAnimal"] = "0.25";
}
new ScriptObject(Name : jungleMonkeyFX)
{
	profiles = "JungleMonkeyFX1 JungleMonkeyFX2 JungleMonkeyFX3";
	transitions["JungleMonkeyFX1", "JungleMonkeyFX1"] = "0.2";
	transitions["JungleMonkeyFX1", "JungleMonkeyFX2"] = "0.4";
	transitions["JungleMonkeyFX1", "JungleMonkeyFX3"] = "0.4";
	transitions["JungleMonkeyFX2", "JungleMonkeyFX1"] = "0.4";
	transitions["JungleMonkeyFX2", "JungleMonkeyFX2"] = "0.2";
	transitions["JungleMonkeyFX2", "JungleMonkeyFX3"] = "0.4";
	transitions["JungleMonkeyFX3", "JungleMonkeyFX1"] = "0.4";
	transitions["JungleMonkeyFX3", "JungleMonkeyFX2"] = "0.4";
	transitions["JungleMonkeyFX3", "JungleMonkeyFX3"] = "0.2";
}
new ScriptObject(Name : tripBirds)
{
	profiles = "TripBirds1 TripBirds2";
	transitions["TripBirds1", "TripBirds1"] = "0.5";
	transitions["TripBirds1", "TripBirds2"] = "0.5";
	transitions["TripBirds2", "TripBirds1"] = "0.5";
	transitions["TripBirds2", "TripBirds2"] = "0.5";
}
new ScriptObject(Name : tripVeryCrazyBirds)
{
	profiles = "TripVeryCrazyBirds1 TripVeryCrazyBirds2";
	transitions["TripVeryCrazyBirds1", "TripVeryCrazyBirds1"] = "0.5";
	transitions["TripVeryCrazyBirds1", "TripVeryCrazyBirds2"] = "0.5";
	transitions["TripVeryCrazyBirds2", "TripVeryCrazyBirds1"] = "0.5";
	transitions["TripVeryCrazyBirds2", "TripVeryCrazyBirds2"] = "0.5";
}
new ScriptObject(Name : tripThunder)
{
	profiles = "TripThunder1 TripThunder2 TripThunder3 TripThunder4";
	transitions["TripThunder1", "TripThunder1"] = "0.25";
	transitions["TripThunder1", "TripThunder2"] = "0.25";
	transitions["TripThunder1", "TripThunder3"] = "0.25";
	transitions["TripThunder1", "TripThunder4"] = "0.25";
	transitions["TripThunder2", "TripThunder1"] = "0.25";
	transitions["TripThunder2", "TripThunder2"] = "0.25";
	transitions["TripThunder2", "TripThunder3"] = "0.25";
	transitions["TripThunder2", "TripThunder4"] = "0.25";
	transitions["TripThunder3", "TripThunder1"] = "0.25";
	transitions["TripThunder3", "TripThunder2"] = "0.25";
	transitions["TripThunder3", "TripThunder3"] = "0.25";
	transitions["TripThunder3", "TripThunder4"] = "0.25";
	transitions["TripThunder4", "TripThunder1"] = "0.25";
	transitions["TripThunder4", "TripThunder2"] = "0.25";
	transitions["TripThunder4", "TripThunder3"] = "0.25";
	transitions["TripThunder4", "TripThunder4"] = "0.25";
}
new ScriptObject(Name : tripPerc)
{
	profiles = "TripPerc1 TripPerc2";
	transitions["TripPerc1", "TripPerc1"] = "0.5";
	transitions["TripPerc1", "TripPerc2"] = "0.5";
	transitions["TripPerc2", "TripPerc1"] = "0.5";
	transitions["TripPerc2", "TripPerc2"] = "0.5";
}
new ScriptObject(Name : tripArps)
{
	profiles = "TripArp1 TripArp2 TripArp3 TripArp4";
	transitions["TripArp1", "TripArp1"] = "0.25";
	transitions["TripArp1", "TripArp2"] = "0.25";
	transitions["TripArp1", "TripArp3"] = "0.25";
	transitions["TripArp1", "TripArp4"] = "0.25";
	transitions["TripArp2", "TripArp1"] = "0.25";
	transitions["TripArp2", "TripArp2"] = "0.25";
	transitions["TripArp2", "TripArp3"] = "0.25";
	transitions["TripArp2", "TripArp4"] = "0.25";
	transitions["TripArp3", "TripArp1"] = "0.25";
	transitions["TripArp3", "TripArp2"] = "0.25";
	transitions["TripArp3", "TripArp3"] = "0.25";
	transitions["TripArp3", "TripArp4"] = "0.25";
	transitions["TripArp4", "TripArp1"] = "0.25";
	transitions["TripArp4", "TripArp2"] = "0.25";
	transitions["TripArp4", "TripArp3"] = "0.25";
	transitions["TripArp4", "TripArp4"] = "0.25";
}
new ScriptObject(Name : tripAmbientBeats)
{
	profiles = "TripAmbientBeat1 TripAmbientBeat2 TripAmbientBeat3 TripAmbientBeat4";
	transitions["TripAmbientBeat1", "TripAmbientBeat1"] = "0.25";
	transitions["TripAmbientBeat1", "TripAmbientBeat2"] = "0.25";
	transitions["TripAmbientBeat1", "TripAmbientBeat3"] = "0.25";
	transitions["TripAmbientBeat1", "TripAmbientBeat4"] = "0.25";
	transitions["TripAmbientBeat2", "TripAmbientBeat1"] = "0.25";
	transitions["TripAmbientBeat2", "TripAmbientBeat2"] = "0.25";
	transitions["TripAmbientBeat2", "TripAmbientBeat3"] = "0.25";
	transitions["TripAmbientBeat2", "TripAmbientBeat4"] = "0.25";
	transitions["TripAmbientBeat3", "TripAmbientBeat1"] = "0.25";
	transitions["TripAmbientBeat3", "TripAmbientBeat2"] = "0.25";
	transitions["TripAmbientBeat3", "TripAmbientBeat3"] = "0.25";
	transitions["TripAmbientBeat3", "TripAmbientBeat4"] = "0.25";
	transitions["TripAmbientBeat4", "TripAmbientBeat1"] = "0.25";
	transitions["TripAmbientBeat4", "TripAmbientBeat2"] = "0.25";
	transitions["TripAmbientBeat4", "TripAmbientBeat3"] = "0.25";
	transitions["TripAmbientBeat4", "TripAmbientBeat4"] = "0.25";
}
new ScriptObject(Name : TripScannerLong)
{
	profiles = "TripScanner35LongPart1 TripScanner35LongPart2";
	transitions["TripScanner35LongPart1", "TripScanner35LongPart2"] = "1";
	transitions["TripScanner35LongPart2", "TripScanner35LongPart1"] = "1";
}
new ScriptObject(Name : TripScanner)
{
	profiles = "TripScanner5part1 TripScanner5part3";
	transitions["TripScanner5part1", "TripScanner5part1"] = "0";
	transitions["TripScanner5part1", "TripScanner5part3"] = "1";
	transitions["TripScanner5part3", "TripScanner5part1"] = "1";
	transitions["TripScanner5part3", "TripScanner5part3"] = "0";
}
new ScriptObject(Name : TripScannerLower)
{
	profiles = "TripScanner5part1lower TripScanner5part2lower";
	transitions["TripScanner5part1lower", "TripScanner5part1lower"] = "0";
	transitions["TripScanner5part1lower", "TripScanner5part2lower"] = "1";
	transitions["TripScanner5part2lower", "TripScanner5part1lower"] = "1";
	transitions["TripScanner5part2lower", "TripScanner5part2lower"] = "0";
}
new ScriptObject(Name : TripLastScanner)
{
	profiles = "TripLastScanner1 TripLastScanner2";
	transitions["TripLastScanner1", "TripLastScanner1"] = "0";
	transitions["TripLastScanner1", "TripLastScanner2"] = "1";
	transitions["TripLastScanner2", "TripLastScanner1"] = "1";
	transitions["TripLastScanner2", "TripLastScanner2"] = "0";
}
new ScriptObject(Name : TripScannerFat)
{
	profiles = "TripScannerFat1 TripScannerFat2";
	transitions["TripScannerFat1", "TripScannerFat1"] = "0";
	transitions["TripScannerFat1", "TripScannerFat2"] = "1";
	transitions["TripScannerFat2", "TripScannerFat1"] = "1";
	transitions["TripScannerFat2", "TripScannerFat2"] = "0";
}
new ScriptObject(Name : tripAmbientBeat)
{
	profiles = "TripAmbientBeat5";
	transitions["TripAmbientBeat5", "TripAmbientBeat5"] = "1";
}
