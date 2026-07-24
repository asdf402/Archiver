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
            MainCommands mainCommand = MainCommands.None;
            bool rleCompress = this.commandDefaults.RleCompress;
            byte retryAmount = this.commandDefaults.RetryAmount;
            TimeSpan waitAmount = this.commandDefaults.WaitTime;
            string source = string.Empty;
            string destination = string.Empty;

            int currentArgumentNumber = 0;
            string currentArgument = commandLineArguments[currentArgumentNumber];

            if (this.ParsePrimaryArgument(currentArgument, out mainCommand))
            {
                return new ParsedResult(
                    new ParsedArguments(rleCompress, retryAmount, waitAmount, source, destination, mainCommand),
                    new Result(true, ConsoleOutput.WriteWrongPrimaryArgumentError, currentArgument));
            }

            currentArgumentNumber++;

            bool errorOccured = false;

            while (currentArgumentNumber < commandLineArguments.Length)
            {
                switch (currentArgument)
                {
                    case "-rle" or "--rleCompress":
                        rleCompress = true;
                        break;

                    case "-r" or "--retry":
                        currentArgumentNumber++;
                        if (!byte.TryParse(currentArgument, out retryAmount))
                        {
                            errorOccured = true;
                        }

                        break;

                    case "-w" or "--wait":
                        currentArgumentNumber++;
                        if (!TimeSpan.TryParse(currentArgument, out waitAmount))
                        {
                            errorOccured = true;
                        }

                        break;

                    case "-s" or "--source":
                        currentArgumentNumber++;
                        source = currentArgument;
                        break;

                    case "-d" or "--destination":
                        currentArgumentNumber++;
                        destination = currentArgument;
                        break;

                    default:
                        errorOccured = true;
                        break;
                }

                if (errorOccured)
                {
                    return new ParsedResult(
                        new ParsedArguments(rleCompress, retryAmount, waitAmount, source, destination, mainCommand),
                        new Result(true, ConsoleOutput.WriteWrongSecondaryArgumentsError, currentArgument));
                }

                currentArgumentNumber++;
            }

            return new ParsedResult(
                new ParsedArguments(rleCompress, retryAmount, waitAmount, source, destination, mainCommand),
                new Result(false));
        }

        // returns true if no error occured
        // returns false if an error occured
        private bool ParsePrimaryArgument(string firstArgument, out MainCommands mainCommand)
        {
            mainCommand = MainCommands.None;

            switch (firstArgument)
            {
                case "-c" or "--create":
                    mainCommand = MainCommands.Append;
                    break;

                case "-a" or "--append":
                    mainCommand = MainCommands.Append;
                    break;

                case "-x" or "--extraxt":
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