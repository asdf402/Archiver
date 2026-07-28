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
            // create local varaibles to have private setters in the ParsedArguments class
            MainCommands mainCommand = this.commandDefaults.MainCommand;
            bool rleCompress = this.commandDefaults.RleCompress;
            byte retryAmount = this.commandDefaults.RetryAmount;
            TimeSpan waitAmount = this.commandDefaults.WaitTime;
            string source = this.commandDefaults.Path;
            string destination = this.commandDefaults.Path;

            if (commandLineArguments.Length <= 0)
            {
                return new ParsedResult(
                new ParsedArguments(rleCompress, retryAmount, waitAmount, source, destination, mainCommand),
                new Result(true, ConsoleOutput.WriteNoArgumentsToParse, string.Empty));
            }

            int currentArgumentNumber = 0;
            string currentArgument = commandLineArguments[currentArgumentNumber];

            // check if the first argument is a valid primary command
            if (!this.ParsePrimaryArgument(currentArgument, out mainCommand))
            {
                return new ParsedResult(
                    new ParsedArguments(rleCompress, retryAmount, waitAmount, source, destination, mainCommand),
                    new Result(true, ConsoleOutput.WriteWrongPrimaryArgumentError, currentArgument));
            }

            currentArgumentNumber++;

            // same for these local varaibles
            bool errorOccured = false;
            ErrorMessage errorMessage = ConsoleOutput.WriteNoErrorOccured;
            string wrongArgument = currentArgument;

            while (currentArgumentNumber < commandLineArguments.Length)
            {
                currentArgument = commandLineArguments[currentArgumentNumber];

                // look for arguments whitch do not require a parameter
                switch (currentArgument)
                {
                    case "-rle" or "--rleCompress":
                        rleCompress = true;
                        currentArgumentNumber++;
                        continue;
                }

                // check if the array is big enough for a follow-up parameter
                if (currentArgumentNumber + 1 >= commandLineArguments.Length)
                {
                    errorOccured = true;
                    errorMessage = ConsoleOutput.WriteMissingParameterForArgumentError;
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
                            errorOccured = true;
                        }

                        break;

                    case "-w" or "--wait":
                        currentArgument = commandLineArguments[++currentArgumentNumber];
                        int temp;
                        if (!int.TryParse(currentArgument, out temp))
                        {
                            errorOccured = true;
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
                        errorOccured = true;
                        break;
                }

                if (errorOccured)
                {
                    errorMessage = ConsoleOutput.WriteWrongSecondaryArgumentsError;
                    wrongArgument = currentArgument;
                    break;
                }

                currentArgumentNumber++;
            }

            return new ParsedResult(
                new ParsedArguments(rleCompress, retryAmount, waitAmount, source, destination, mainCommand),
                new Result(errorOccured, errorMessage, wrongArgument));
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