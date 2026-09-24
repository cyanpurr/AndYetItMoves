// scriptDoc.cs.dso
function writeOutFunctions()
{
	new ConsoleLogger(Name : logger, "scriptFunctions.txt", "0");
	dumpConsoleFunctions();
	logger.delete();
	return;
}
function writeOutClasses()
{
	new ConsoleLogger(Name : logger, "scriptClasses.txt", "0");
	dumpConsoleClasses();
	logger.delete();
	return;
}
