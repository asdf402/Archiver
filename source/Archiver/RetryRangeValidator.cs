namespace Archiver
{
    public class RetryRangeValidator : IValidator
    {
        public bool IsValid(ParsedArguments parsedArguments)
        {
            if (parsedArguments.RetryAmount < 1 ||
                parsedArguments.RetryAmount > 10)
            {
                return false;
            }

            return true;
        }
    }
}