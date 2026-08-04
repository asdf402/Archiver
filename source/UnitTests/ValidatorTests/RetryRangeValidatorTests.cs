using Archiver;

namespace UnitTests.ValidatorTests
{
    public class RetryRangeValidatorTests
    {
        private RetryRangeValidator retryRangeValidator;
        private CommandDefaults commandDefaults;

        public RetryRangeValidatorTests()
        {
            this.retryRangeValidator = new RetryRangeValidator();
            this.commandDefaults = new CommandDefaults();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(4)]
        [InlineData(6)]
        [InlineData(9)]
        public void Valid_Numbers_Are_Inputed_And_Result_In_True(byte retryAmount)
        {
            ParsedArguments parsed = new ParsedArguments(
                commandDefaults.RleCompress,
                retryAmount,
                commandDefaults.WaitTime,
                commandDefaults.Path,
                commandDefaults.Path,
                commandDefaults.MainCommand);

            Assert.True(retryRangeValidator.IsValid(parsed));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(11)]
        [InlineData(100)]
        [InlineData(60)]
        [InlineData(200)]
        public void Invalid_Numbers_Are_Inputed_And_Result_In_False(byte retryAmount)
        {
            ParsedArguments parsed = new ParsedArguments(
                commandDefaults.RleCompress,
                retryAmount,
                commandDefaults.WaitTime,
                commandDefaults.Path,
                commandDefaults.Path,
                commandDefaults.MainCommand);

            Assert.False(retryRangeValidator.IsValid(parsed));
        }
    }
}
