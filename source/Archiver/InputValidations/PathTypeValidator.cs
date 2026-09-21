namespace Archiver
{
    public class PathTypeValidator : IValidator
    {
        public bool IsValid(ParsedArguments parsedArguments)
        {
            switch (parsedArguments.MainCommand)
            {
                case Create:
                    return this.IsCreatePathValid(parsedArguments.Source, parsedArguments.Destination);

                case Append:
                    return this.IsAppendPathValid(parsedArguments.Source, parsedArguments.Destination);

                case Extract:
                    return this.IsExtractPathValid(parsedArguments.Source, parsedArguments.Destination);

                case Info:
                    return this.IsInfoPathValid(parsedArguments.Source);

                case List:
                    return this.IsListPathValid(parsedArguments.Source);

                default:
                    return false;
            }
        }

        private bool IsCreatePathValid(string source, string destination)
        {
            if (this.IsRightExtensionType(destination, ".dat"))
            {
                return true;
            }

            return false;
        }

        private bool IsAppendPathValid(string source, string destination)
        {
            if (this.IsRightExtensionType(destination, ".dat"))
            {
                return true;
            }

            return false;
        }

        private bool IsExtractPathValid(string source, string destination)
        {
            if (this.IsRightExtensionType(source, ".dat"))
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