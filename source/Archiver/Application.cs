namespace Archiver
{
    public class Application
    {
        private Parser parser;

        public Application()
        {
            this.parser = new Parser();
        }

        public void Run(string[] commandLineArguments)
        {
            ParsedResult parsedResult;

            parsedResult = this.parser.Parse(commandLineArguments);
            if (parsedResult.Result.ErrorOccured)
            {
                parsedResult.Result.ErrorMessage?.Invoke(parsedResult.Result.WrongArgument);
            }
        }
    }
}