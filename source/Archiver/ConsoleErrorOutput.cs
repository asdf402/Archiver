namespace Archiver
{
    public static class ConsoleErrorOutput
    {
        public static void WriteWrongPrimaryArgumentError(string parsedArgument)
        {
            Console.WriteLine(
$@"{parsedArgument} is not a valid primary argument.
please use a main command as the primary command, a list of main commands:");
            WritePrimaryCommands();
        }

        public static void WriteWrongSecondaryArgumentsError(string parsedArgument)
        {
            Console.WriteLine(
$@"{parsedArgument} is not a valid secondary argument.
please use a valid secondary command after the main command:");
            WriteSecondaryCommands();
        }

        public static void WriteMissingParameterForArgumentError(string parsedArgument)
        {
            Console.WriteLine(
$@"{parsedArgument} should follow a describing parameter:
    -r or --retry and a number between 1 and 10 (used to specify the numbers of retries after a failed attempted)
    -w or --wait and a number between 1 and 10 (used to specify the time between retries in seconds)
    -s or --source and a path
    -d or --destination and a path");
        }

        public static void WriteNoErrorOccurred(string parsedArgument)
        {
            Console.WriteLine($"no error occurred at {parsedArgument}");
        }

        public static void WriteNoArgumentsToParse(string parsedArgument)
        {
            Console.WriteLine("please use one of the following commands as the primary command:");
            WritePrimaryCommands();

            Console.WriteLine("\nafter that you can use the secondary commands:");
            WriteSecondaryCommands();
        }

        public static void WriteWrongArgumentCombination(string mainCommand)
        {
            Console.WriteLine($"this is a wrong combination of commands for {mainCommand}");
            WritePrimaryCommands();
        }

        public static void WriteWrongRetryAmount(string wrongRetryAmount)
        {
            Console.WriteLine($"the retry amount should be between 1 and 10, yours {wrongRetryAmount}");
        }

        public static void WriteWrongWaitTime(string wrongWaitTime)
        {
            Console.WriteLine($"the wait time should be between 1 and 10 seconds, yours {wrongWaitTime}");
        }

        public static void WriteCouldNotOpenDestination(string wrongArgument)
        {
            Console.WriteLine("Could not open or missed rights to read from the destination");
        }

        public static void WriteCouldNotOpenSource(string wrongArgument)
        {
            Console.WriteLine("Could not open or missed rights to read from the source");
        }

        private static void WritePrimaryCommands()
        {
            Console.WriteLine(
@$"     -c or--create(creates an archive); needs - s and - d
    - a or--append(appends things to an existing archive); needs - s and - d
    - x or--extract(extracts everything from an existing archive); needs - s and - d
    - i or--info(shows information about the archive); needs - s; do not use -rle or - d
    - l or--list(shows the file names of the archives content); needs - s, do not use -rle or - d");
        }

        private static void WriteSecondaryCommands()
        {
            Console.WriteLine(
$@"     -rle or --rleCompress (compresses an archive with the rle compression)
    -r or --retry and a number between 1 and 10 (used to specify the numbers of retries after a failed attempted)
    -w or --wait and a number between 1 and 10 (used to specify the time between retries in seconds)
    -s or --source and a path
    -d or --destination and a path");
        }
    }
}