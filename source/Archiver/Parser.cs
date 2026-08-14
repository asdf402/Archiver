namespace Archiver
{
    public class Parser
    {
        private CommandDefaults commandDefaults;

        public Parser()
        {
            this.commandDefaults = new CommandDefaults();
        }

        public ParsedResult Parse(string[] commandLineArguments)
        {
            // create local variables to have private setters in the ParsedArguments class
            MainCommands mainCommand = this.commandDefaults.MainCommand;
            ICompress compress = this.commandDefaults.Compress;
            byte retryAmount = this.commandDefaults.RetryAmount;
            TimeSpan waitAmount = this.commandDefaults.WaitTime;
            string source = this.commandDefaults.Path;
            string destination = this.commandDefaults.Path;

            if (commandLineArguments.Length <= 0)
            {
                return new ParsedResult(
                new ParsedArguments(compress, retryAmount, waitAmount, source, destination, mainCommand),
                new Result(true, ConsoleErrorOutput.WriteNoArgumentsToParse, string.Empty));
            }

            int currentArgumentNumber = 0;
            string currentArgument = commandLineArguments[currentArgumentNumber];

            // check if the first argument is a valid primary command
            if (!this.ParsePrimaryArgument(currentArgument, out mainCommand))
            {
                return new ParsedResult(
                    new ParsedArguments(compress, retryAmount, waitAmount, source, destination, mainCommand),
                    new Result(true, ConsoleErrorOutput.WriteWrongPrimaryArgumentError, currentArgument));
            }

            currentArgumentNumber++;

            // same for these local variables
            bool errorOccurred = false;
            ErrorMessage errorMessage = ConsoleErrorOutput.WriteNoErrorOccurred;
            string wrongArgument = currentArgument;

            while (currentArgumentNumber < commandLineArguments.Length)
            {
                currentArgument = commandLineArguments[currentArgumentNumber];

                // look for arguments which do not require a parameter
                switch (currentArgument)
                {
                    case "-rle" or "--rleCompress":
                        compress = new RleCompress();
                        currentArgumentNumber++;
                        continue;
                }

                // check if the array is big enough for a follow-up parameter
                if (currentArgumentNumber + 1 >= commandLineArguments.Length)
                {
                    errorOccurred = true;
                    errorMessage = ConsoleErrorOutput.WriteMissingParameterForArgumentError;
                    wrongArgument = currentArgument;
                    break;
                }

                // look for arguments whitch require a parameter
                switch (currentArgument)
                {
                    case "-r" or "--retry":
                        currentArgument = commandLineArguments[++currentArgumentNumber];
                        if (!byte.TryParse(currentArgument, out retryAmount))
                        {
                            errorOccurred = true;
                        }

                        break;

                    case "-w" or "--wait":
                        currentArgument = commandLineArguments[++currentArgumentNumber];
                        int temp;
                        if (!int.TryParse(currentArgument, out temp))
                        {
                            errorOccurred = true;
                        }

                        waitAmount = new TimeSpan(hours: 0, minutes: 0, seconds: temp);
                        break;

                    case "-s" or "--source":
                        currentArgument = commandLineArguments[++currentArgumentNumber];
                        source = currentArgument;
                        break;

                    case "-d" or "--destination":
                        currentArgument = commandLineArguments[++currentArgumentNumber];
                        destination = currentArgument;
                        break;

                    default:
                        errorOccurred = true;
                        break;
                }

                if (errorOccurred)
                {
                    errorMessage = ConsoleErrorOutput.WriteWrongSecondaryArgumentsError;
                    wrongArgument = currentArgument;
                    break;
                }

                currentArgumentNumber++;
            }

            return new ParsedResult(
                new ParsedArguments(compress, retryAmount, waitAmount, source, destination, mainCommand),
                new Result(errorOccurred, errorMessage, wrongArgument));
        }

        // returns true if no error occured
        // returns false if an error occured
        private bool ParsePrimaryArgument(string firstArgument, out MainCommands mainCommand)
        {
            mainCommand = MainCommands.None;

            switch (firstArgument)
            {
                case "-c" or "--create":
                    mainCommand = MainCommands.Create;
                    break;

                case "-a" or "--append":
                    mainCommand = MainCommands.Append;
                    break;

                case "-x" or "--extract":
                    mainCommand = MainCommands.Extract;
                    break;

                case "-i" or "--info":
                    mainCommand = MainCommands.Info;
                    break;

                case "-l" or "--list":
                    mainCommand = MainCommands.List;
                    break;

                default:
                    return false;
            }

            return true;
        }
    }
}