namespace Archiver
{
    public class RequiredArgumentsValidator : IValidator
    {
        private readonly CommandDefaults commandDefaults;

        public RequiredArgumentsValidator()
        {
            this.commandDefaults = new CommandDefaults();
        }

        public bool IsValid(ParsedArguments parsedArguments)
        {
            switch (parsedArguments.MainCommand)
            {
                case MainCommands.Create:
                    return this.CreateValidation(parsedArguments);

                case MainCommands.Append:
                    return this.AppendValidation(parsedArguments);

                case MainCommands.Extract:
                    return this.ExtractValidation(parsedArguments);

                case MainCommands.Info:
                    return this.InfoValidation(parsedArguments);

                case MainCommands.List:
                    return this.ListValidation(parsedArguments);

                default:
                    return false;
            }
        }

        // allows: -rle, -r, -w
        private bool CreateValidation(ParsedArguments parsedArguments)
        {
            // needs: -s, -d
            if (this.HasSource(parsedArguments.Source) &&
                this.HasDestination(parsedArguments.Destination))
            {
                return true;
            }

            return false;
        }

        // allows: -rle, -r, -w
        private bool AppendValidation(ParsedArguments parsedArguments)
        {
            // needs: -s, -d
            if (this.HasSource(parsedArguments.Source) &&
                this.HasDestination(parsedArguments.Destination))
            {
                return true;
            }

            return false;
        }

        // allows: -rle, -r, -w
        private bool ExtractValidation(ParsedArguments parsedArguments)
        {
            // needs: -s, -d
            if (this.HasSource(parsedArguments.Source) &&
                this.HasDestination(parsedArguments.Destination))
            {
                return true;
            }

            return false;
        }

        // allows: -r, -w
        private bool InfoValidation(ParsedArguments parsedArguments)
        {
            // needs: -s
            if (this.HasSource(parsedArguments.Source))
            {
                // does not need: -d, rle
                if (!this.HasDestination(parsedArguments.Destination) &&
                    !this.HasCompress(parsedArguments.Compress))
                {
                    return true;
                }
            }

            return false;
        }

        // allows: -r, -w
        private bool ListValidation(ParsedArguments parsedArguments)
        {
            // needs: -s
            if (this.HasSource(parsedArguments.Source))
            {
                // not needs: -d, rle
                if (!this.HasDestination(parsedArguments.Destination) &&
                    !this.HasCompress(parsedArguments.Compress))
                {
                    return true;
                }
            }

            return false;
        }

        private bool HasSource(string source)
        {
            if (source == this.commandDefaults.Path)
            {
                return false;
            }

            return true;
        }

        private bool HasDestination(string destination)
        {
            if (destination == this.commandDefaults.Path)
            {
                return false;
            }

            return true;
        }

        private bool HasCompress(ICompress rle)
        {
            if (rle is NoCompress)
            {
                return false;
            }

            return true;
        }
    }
}