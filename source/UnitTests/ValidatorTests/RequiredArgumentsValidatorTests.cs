using Archiver;

namespace UnitTests.ValidatorTests
{
    public class RequiredArgumentsValidatorTests
    {
        private readonly CommandDefaults commandDefaults;
        private readonly RequiredArgumentsValidator requiredArgumentsValidator;

        public RequiredArgumentsValidatorTests()
        {
            this.commandDefaults = new CommandDefaults();
            this.requiredArgumentsValidator = new RequiredArgumentsValidator();
        }

        [Fact]
        public void Valid_Create_Returns_True()
        {
            ParsedArguments parsed = new ParsedArguments(
                this.commandDefaults.Compress,
                this.commandDefaults.RetryAmount,
                this.commandDefaults.WaitTime,
                "source",
                "destination.dat",
                MainCommands.Create);

            Assert.True(this.requiredArgumentsValidator.IsValid(parsed));
        }

        [Fact]
        public void Valid_Create_With_Rle_Returns_True()
        {
            ParsedArguments parsed = new ParsedArguments(
                new RleCompress(),
                this.commandDefaults.RetryAmount,
                this.commandDefaults.WaitTime,
                "source",
                "destination.dat",
                MainCommands.Create);

            Assert.True(this.requiredArgumentsValidator.IsValid(parsed));
        }

        [Fact]
        public void Valid_Append_Returns_True()
        {
            ParsedArguments parsed = new ParsedArguments(
                this.commandDefaults.Compress,
                this.commandDefaults.RetryAmount,
                this.commandDefaults.WaitTime,
                "source",
                "destination.dat",
                MainCommands.Append);

            Assert.True(this.requiredArgumentsValidator.IsValid(parsed));
        }

        [Fact]
        public void Valid_Append_With_Rle_Returns_False()
        {
            ParsedArguments parsed = new ParsedArguments(
                new RleCompress(),
                this.commandDefaults.RetryAmount,
                this.commandDefaults.WaitTime,
                "source",
                "destination.dat",
                MainCommands.Append);

            Assert.False(this.requiredArgumentsValidator.IsValid(parsed));
        }

        [Fact]
        public void Valid_Extract_Returns_True()
        {
            ParsedArguments parsed = new ParsedArguments(
                this.commandDefaults.Compress,
                this.commandDefaults.RetryAmount,
                this.commandDefaults.WaitTime,
                "source.dat",
                "destination",
                MainCommands.Extract);

            Assert.True(this.requiredArgumentsValidator.IsValid(parsed));
        }

        [Fact]
        public void Valid_Extract_With_Rle_Returns_False()
        {
            ParsedArguments parsed = new ParsedArguments(
                new RleCompress(),
                this.commandDefaults.RetryAmount,
                this.commandDefaults.WaitTime,
                "source.dat",
                "destination",
                MainCommands.Extract);

            Assert.False(this.requiredArgumentsValidator.IsValid(parsed));
        }

        [Fact]
        public void Valid_Info_Returns_True()
        {
            ParsedArguments parsed = new ParsedArguments(
                this.commandDefaults.Compress,
                this.commandDefaults.RetryAmount,
                this.commandDefaults.WaitTime,
                "source",
                this.commandDefaults.Path,
                MainCommands.Info);

            Assert.True(this.requiredArgumentsValidator.IsValid(parsed));
        }

        [Fact]
        public void Invalid_Info_With_Rle_Returns_False()
        {
            ParsedArguments parsed = new ParsedArguments(
                new RleCompress(),
                this.commandDefaults.RetryAmount,
                this.commandDefaults.WaitTime,
                "source",
                this.commandDefaults.Path,
                MainCommands.Info);

            Assert.False(this.requiredArgumentsValidator.IsValid(parsed));
        }

        [Fact]
        public void Invalid_Info_With_Destination_Returns_False()
        {
            ParsedArguments parsed = new ParsedArguments(
                new RleCompress(),
                this.commandDefaults.RetryAmount,
                this.commandDefaults.WaitTime,
                "source",
                "destination.dat",
                MainCommands.Info);

            Assert.False(this.requiredArgumentsValidator.IsValid(parsed));
        }

        [Fact]
        public void Valid_List_Returns_True()
        {
            ParsedArguments parsed = new ParsedArguments(
                this.commandDefaults.Compress,
                this.commandDefaults.RetryAmount,
                this.commandDefaults.WaitTime,
                "source",
                this.commandDefaults.Path,
                MainCommands.List);

            Assert.True(this.requiredArgumentsValidator.IsValid(parsed));
        }

        [Fact]
        public void Invalid_List_With_Rle_Returns_False()
        {
            ParsedArguments parsed = new ParsedArguments(
                new RleCompress(),
                this.commandDefaults.RetryAmount,
                this.commandDefaults.WaitTime,
                "source",
                this.commandDefaults.Path,
                MainCommands.List);

            Assert.False(this.requiredArgumentsValidator.IsValid(parsed));
        }

        [Fact]
        public void Invalid_List_With_Destination_Returns_False()
        {
            ParsedArguments parsed = new ParsedArguments(
                new RleCompress(),
                this.commandDefaults.RetryAmount,
                this.commandDefaults.WaitTime,
                "source",
                "destination.dat",
                MainCommands.List);

            Assert.False(this.requiredArgumentsValidator.IsValid(parsed));
        }
    }
}
