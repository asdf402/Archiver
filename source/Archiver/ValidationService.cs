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
                return new Result(true, ConsoleErrorOutput.WriteWrongArgumentCombination, parsedArguments.MainCommand.ToString());
            }
            else if (!this.retryRangeValidator.IsValid(parsedArguments))
            {
                return new Result(true, ConsoleErrorOutput.WriteWrongRetryAmount, parsedArguments.RetryAmount.ToString());
            }
            else if (!this.waitRangeValidator.IsValid(parsedArguments))
            {
                return new Result(true, ConsoleErrorOutput.WriteWrongWaitTime, parsedArguments.RetryAmount.ToString());
            }

            return new Result(false, ConsoleErrorOutput.WriteNoErrorOccurred, string.Empty);
        }
    }
}