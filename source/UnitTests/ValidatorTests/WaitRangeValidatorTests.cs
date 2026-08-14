using Archiver;

namespace UnitTests.ValidatorTests
{
    public class WaitRangeValidatorTests
    {
        private WaitRangeValidator waitRangeValidator;
        private CommandDefaults commandDefaults;

        public WaitRangeValidatorTests()
        {
            this.waitRangeValidator = new WaitRangeValidator();
            this.commandDefaults = new CommandDefaults();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(4)]
        [InlineData(6)]
        [InlineData(9)]
        public void Valid_Numbers_Are_Inputed_And_Result_In_True(int waitTime)
        {
            ParsedArguments parsed = new ParsedArguments(
                commandDefaults.Compress,
                commandDefaults.RetryAmount,
                new TimeSpan(0, 0, waitTime),
                commandDefaults.Path,
                commandDefaults.Path,
                commandDefaults.MainCommand);

            Assert.True(waitRangeValidator.IsValid(parsed));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(11)]
        [InlineData(-3)]
        [InlineData(60)]
        [InlineData(1054)]
        public void Invalid_Numbers_Are_Inputed_And_Result_In_False(int waitTime)
        {
            ParsedArguments parsed = new ParsedArguments(
                commandDefaults.Compress,
                commandDefaults.RetryAmount,
                new TimeSpan(0, 0, waitTime),
                commandDefaults.Path,
                commandDefaults.Path,
                commandDefaults.MainCommand);

            Assert.False(waitRangeValidator.IsValid(parsed));
        }
    }
}
