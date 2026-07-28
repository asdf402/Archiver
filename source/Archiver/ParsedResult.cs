namespace Archiver
{
    public class ParsedResult
    {
        public ParsedResult(ParsedArguments parsedArguments, Result result)
        {
            this.Result = result;
            this.ParsedArguments = parsedArguments;
        }

        public Result Result
        {
            get;
            private set;
        }

        public ParsedArguments ParsedArguments
        {
            get;
            private set;
        }
    }
}