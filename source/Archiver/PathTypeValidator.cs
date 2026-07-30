namespace Archiver
{
    public class PathTypeValidator : IValidator
    {
        public bool IsValid(ParsedArguments parsedArguments)
        {
            switch (parsedArguments.MainCommand)
            {
                case MainCommands.Create:
                    return this.IsCreatePathValid(parsedArguments.Source, parsedArguments.Destination);

                case MainCommands.Append:
                    return this.IsAppendPathValid(parsedArguments.Source, parsedArguments.Destination);

                case MainCommands.Extract:
                    return this.IsExtractPathValid(parsedArguments.Source, parsedArguments.Destination);

                case MainCommands.Info:
                    return this.IsInfoPathValid(parsedArguments.Source);

                case MainCommands.List:
                    return this.IsListPathValid(parsedArguments.Source);

                case MainCommands.None:
                    return false;

                default:
                    return true;
            }
        }

        private bool IsCreatePathValid(string source, string destination)
        {
            if (Path.EndsInDirectorySeparator(source) &&
                this.IsRightExtensionType(destination, ".dat"))
            {
                return true;
            }

            return false;
        }

        private bool IsAppendPathValid(string source, string destination)
        {
            if (Path.EndsInDirectorySeparator(source) &&
                this.IsRightExtensionType(destination, ".dat"))
            {
                return true;
            }

            return false;
        }

        private bool IsExtractPathValid(string source, string destination)
        {
            if (Path.EndsInDirectorySeparator(destination) &&
                this.IsRightExtensionType(source, ".dat"))
            {
                return true;
            }

            return false;
        }

        private bool IsInfoPathValid(string source)
        {
            return this.IsRightExtensionType(source, ".dat");
        }

        private bool IsListPathValid(string source)
        {
            return this.IsRightExtensionType(source, ".dat");
        }

        private bool IsRightExtensionType(string path, string extension)
        {
            string pathExtension = Path.GetExtension(path);

            return pathExtension == extension;
        }
    }
}