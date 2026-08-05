namespace Archiver
{
    public interface ICommand
    {
        void Execute(ParsedArguments parsedArguments);
    }
}