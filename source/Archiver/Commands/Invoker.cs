namespace Archiver
{
    public class Invoker
    {
        private ICommand? command;

        public void SetCommand(ICommand command)
        {
            this.command = command;
        }

        public Result ExecuteCommand(ParsedArguments parsedArguments)
        {
            ArgumentNullException.ThrowIfNull(this.command);

            Result result = new Result(
                true,
                ConsoleErrorOutput.WriteNoValidDatFile,
                "command");

            for (int counter = 0; counter < parsedArguments.RetryAmount; counter++)
            {
                try
                {
                    result = this.command.Execute(parsedArguments);
                    break;
                }
                catch (Exception)
                {
                    // do not wait when it is the last retry
                    if (counter < parsedArguments.RetryAmount - 1)
                    {
                        Thread.Sleep(parsedArguments.WaitTime);
                    }
                }
            }

            return result;
        }
    }
}