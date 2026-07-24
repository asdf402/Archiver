namespace Archiver
{
    public static class ConsoleOutput
    {
        public static void WriteWrongPrimaryArgumentError(string parsedArgument)
        {
            Console.WriteLine(
                $@"{parsedArgument} is not a valid primary argument.
                please use a main command as the primary command, a list of main commands:");
            WritePrimaryArgumentsList();
        }

        public static void WriteWrongSecondaryArgumentsError(string parsedArgument)
        {
            Console.WriteLine(
                $@"{parsedArgument} is not a valid secundary argument.
                please use a valid secondary command after the main command:");
            WriteSecundaryArgumentsList();
        }

        private static void WritePrimaryArgumentsList()
        {
            Console.WriteLine(
                $@"-c or --create (creates an archive)
                -a or --append (appends things to an existing archive)
                -x or --extract (extraxts everything from an existing archive)
                -i or --info (shows information about the archive)
                -l or --list (shows the file names of the archives content)");
        }

        private static void WriteSecundaryArgumentsList()
        {
            Console.WriteLine(
                $@"-rle or --rleCompress (compresses an archive with the rle compression)
                -r or --retry and a number between 1 and 10 (used to specify the numbers of retries after a failed attempted)
                -w or --wait and a number between 1 and 10 (used to specify the time between retries in seconds)
                -s or --source and a path
                -d or --destination and a path");
        }
    }
}