namespace Archiver
{
    public interface IValidator
    {
        bool Validate(ParsedArguments parsedArguments);
    }
}