namespace Archiver
{
    public class Invoker
    {
        private ICommand? command;

        public void SetCommand(MainCommands command)
        {
            switch (command)
            {
                case MainCommands.Append:
                    this.command = new Append();
                    break;
                case MainCommands.Create:
                    this.command = new Create();
                    break;
                case MainCommands.Extract:
                    this.command = new Extract();
                    break;
                case MainCommands.Info:
                    this.command = new Info();
                    break;
                case MainCommands.List:
                    this.command = new List();
                    break;
                default:
                    throw new InvalidOperationException($"the given command ({command}) is not jet in the switch statment");
            }
        }

        public Result ExecuteCommand(ParsedArguments parsedArguments)
        {
            if (this.command == null)
            {
                throw new InvalidOperationException("Call Invoker.SetCommand first before calling Invoker.ExecuteCommand. The current command to be executed is null.");
            }

            Result result = new Result(
                true,
                ConsoleErrorOutput.WriteNoValidDatFile,
                parsedArguments.MainCommand.ToString());

            for (int counter = 0; counter < parsedArguments.RetryAmount; counter++)
            {
                try
                {
                    result = this.command.Execute(parsedArguments);
                    break;
                }
                catch (Exception)
                {
                    Thread.Sleep(parsedArguments.WaitTime);
                }
            }

            if (result.ErrorOccured)
            {
                return result;
            }
        }
    }
}