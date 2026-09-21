using Archiver;
using Archiver.Commands;

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
                new Create());

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
                new Create());

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
                new Append());

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
                new Append());

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
                new Extract());

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
                new Extract());

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
                new Info());

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
                new Info());

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
                new Info());

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
                new List());

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
                new List());

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
                new List());

            Assert.False(this.requiredArgumentsValidator.IsValid(parsed));
        }
    }
}
