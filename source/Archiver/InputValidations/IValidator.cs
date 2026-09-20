namespace Archiver
{
    public interface IValidator
    {
        // returns true if the parsedArgument is valid or false if it is not valid
        bool IsValid(ParsedArguments parsedArguments);
    }
}