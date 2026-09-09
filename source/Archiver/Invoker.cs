namespace Archiver
{
    public class Invoker
    {
        private ICommand command;

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
            }
        }

        public void ExecuteCommand()
        {
            this.command.Execute();
        }
    }
}