namespace Archiver
{
    public class ValidationService
    {
        private RequiredArgumentsValidator requiredArgumentsValidator;
        private RetryRangeValidator retryRangeValidator;
        private WaitRangeValidator waitRangeValidator;

        public ValidationService()
        {
            this.requiredArgumentsValidator = new RequiredArgumentsValidator();
            this.retryRangeValidator = new RetryRangeValidator();
            this.waitRangeValidator = new WaitRangeValidator();
        }

        public Result Validate(ParsedArguments parsedArguments)
        {
            if (!this.requiredArgumentsValidator.IsValid(parsedArguments))
            {
                return new Result(true, ConsoleOutput.WriteWrongArgumentCombination, parsedArguments.MainCommand.ToString());
            }
            else if (!this.retryRangeValidator.IsValid(parsedArguments))
            {
                return new Result(true, ConsoleOutput.WriteWrongRetryAmount, parsedArguments.RetryAmount.ToString());
            }
            else if (!this.waitRangeValidator.IsValid(parsedArguments))
            {
                return new Result(true, ConsoleOutput.WriteWrongWaitTime, parsedArguments.RetryAmount.ToString());
            }

            return new Result(false, ConsoleOutput.WriteNoErrorOccured, string.Empty);
        }
    }
}