namespace Archiver.Commands
{
    public class None : ICommand
    {
        public Result Execute(ParsedArguments parsedArguments)
        {
            throw new InvalidOperationException(
                "This is not a valid command and only serves as a default when no command is selected jet");
        }
    }
}
