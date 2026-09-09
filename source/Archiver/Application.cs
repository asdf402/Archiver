namespace Archiver
{
    public class Application
    {
        private Parser parser;
        private ValidationService validationService;
        private Invoker invoker;

        public Application()
        {
            this.parser = new Parser();
            this.validationService = new ValidationService();
            this.invoker = new Invoker();
        }

        public void Run(string[] commandLineArguments)
        {
            ParsedResult parsedResult;
            parsedResult = this.parser.Parse(commandLineArguments);

            if (parsedResult.Result.ErrorOccured)
            {
                parsedResult.Result.ErrorMessage?.Invoke(parsedResult.Result.WrongArgument);
            }

            ParsedArguments parsedArguments = parsedResult.ParsedArguments;

            Result validationResult;
            validationResult = this.validationService.Validate(parsedArguments);
            if (validationResult.ErrorOccured)
            {
                validationResult.ErrorMessage?.Invoke(validationResult.WrongArgument);
            }

            Result invokerResult;
            this.invoker.SetCommand(parsedArguments.MainCommand);
            invokerResult = this.invoker.ExecuteCommand();
            if (invokerResult.ErrorOccured)
            {
                invokerResult.ErrorMessage?.Invoke(invokerResult.WrongArgument);
            }
        }
    }
}