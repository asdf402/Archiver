namespace Archiver
{
    public interface ICommand
    {
        Result Execute(ParsedArguments parsedArguments);
    }
}