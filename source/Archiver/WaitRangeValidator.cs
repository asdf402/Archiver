namespace Archiver
{
    public class WaitRangeValidator : IValidator
    {
        public bool IsValid(ParsedArguments parsedArguments)
        {
            if (TimeSpan.Compare(parsedArguments.WaitTime, new TimeSpan(0, 0, 01)) < 0 ||
                TimeSpan.Compare(parsedArguments.WaitTime, new TimeSpan(0, 0, 10)) > 0)
            {
                return false;
            }

            return true;
        }
    }
}