namespace UnitTests
{
    using Archiver;

    public class ParserTests
    {
        private readonly Parser parser;
        private readonly CommandDefaults commandDefaults;

        public ParserTests()
        {
            parser = new Parser();
            commandDefaults = new CommandDefaults();
        }

        [Theory]
        [InlineData("-c")]
        [InlineData("--create")]
        public void Valid_Create_Command_Is_Correctly_Parsed(string fristArgument)
        {
            string[] commandLineArguments =
            {
                fristArgument
            };

            ParsedResult parsedResult;
            parsedResult = this.parser.Parse(commandLineArguments);

            Assert.False(parsedResult.Result.ErrorOccured);
            Assert.Equal(MainCommands.Create, parsedResult.ParsedArguments.MainCommand);
            AssertDefaultArguments(parsedResult);
        }

        [Theory]
        [InlineData("-a")]
        [InlineData("--append")]
        public void Valid_Append_Command_Is_Correctly_Parsed(string firstArgument)
        {
            string[] commandLineArguments =
            {
                firstArgument
            };

            ParsedResult parsedResult;
            parsedResult = this.parser.Parse(commandLineArguments);

            Assert.False(parsedResult.Result.ErrorOccured);
            Assert.Equal(MainCommands.Append, parsedResult.ParsedArguments.MainCommand);
            AssertDefaultArguments(parsedResult);
        }

        [Theory]
        [InlineData("-x")]
        [InlineData("--extract")]
        public void Valid_Extract_Command_Is_Correctly_Parsed(string firstArgument)
        {
            string[] commandLineArguments =
            {
                firstArgument
            };

            ParsedResult parsedResult;
            parsedResult = this.parser.Parse(commandLineArguments);

            Assert.False(parsedResult.Result.ErrorOccured);
            Assert.Equal(MainCommands.Extract, parsedResult.ParsedArguments.MainCommand);
            AssertDefaultArguments(parsedResult);
        }

        [Theory]
        [InlineData("-i")]
        [InlineData("--info")]
        public void Valid_Info_Command_Is_Correctly_Parsed(string firstArgument)
        {
            string[] commandLineArguments =
            {
                firstArgument
            };

            ParsedResult parsedResult;
            parsedResult = this.parser.Parse(commandLineArguments);

            Assert.False(parsedResult.Result.ErrorOccured);
            Assert.Equal(MainCommands.Info, parsedResult.ParsedArguments.MainCommand);
            AssertDefaultArguments(parsedResult);
        }

        [Theory]
        [InlineData("-l")]
        [InlineData("--list")]
        public void Valid_List_Command_Is_Correctly_Parsed(string firstArgument)
        {
            string[] commandLineArguments =
            {
                firstArgument
            };

            ParsedResult parsedResult;
            parsedResult = this.parser.Parse(commandLineArguments);

            Assert.False(parsedResult.Result.ErrorOccured);
            Assert.Equal(MainCommands.List, parsedResult.ParsedArguments.MainCommand);
            AssertDefaultArguments(parsedResult);
        }

        [Theory]
        [InlineData("-rle")]
        [InlineData("--rleCompress")]
        public void Valid_Rle_Command_Is_Correctly_Parsed(string rleArgument)
        {
            string[] commandLineArguments =
            {
                "-c",
                rleArgument
            };

            ParsedResult parsedResult;
            parsedResult = this.parser.Parse(commandLineArguments);

            Assert.False(parsedResult.Result.ErrorOccured);
            Assert.Equal(MainCommands.Create, parsedResult.ParsedArguments.MainCommand);
            Assert.IsType<RleCompress>(parsedResult.ParsedArguments.Compress);
            Assert.Equal(string.Empty, parsedResult.ParsedArguments.Destination);
            Assert.Equal(string.Empty, parsedResult.ParsedArguments.Source);
            Assert.Equal(this.commandDefaults.RetryAmount, parsedResult.ParsedArguments.RetryAmount);
            Assert.Equal(this.commandDefaults.WaitTime, parsedResult.ParsedArguments.WaitTime);
        }

        [Theory]
        [InlineData("-r")]
        [InlineData("--retry")]
        public void Valid_Retry_Command_Is_Correctly_Parsed(string retryArgument)
        {
            string[] commandLineArguments =
            {
                "-c",
                retryArgument,
                "3"
            };

            ParsedResult parsedResult;
            parsedResult = this.parser.Parse(commandLineArguments);

            Assert.False(parsedResult.Result.ErrorOccured);
            Assert.Equal(MainCommands.Create, parsedResult.ParsedArguments.MainCommand);
            Assert.Equal(3, parsedResult.ParsedArguments.RetryAmount);
            Assert.Equal(string.Empty, parsedResult.ParsedArguments.Destination);
            Assert.Equal(string.Empty, parsedResult.ParsedArguments.Source);
            Assert.Equal(this.commandDefaults.WaitTime, parsedResult.ParsedArguments.WaitTime);
            Assert.IsType<NoCompress>(parsedResult.ParsedArguments.Compress);
        }

        [Theory]
        [InlineData("-w")]
        [InlineData("--wait")]
        public void Valid_Wait_Command_Is_Correctly_Parsed(string waitArgument)
        {
            string[] commandLineArguments =
            {
                "-c",
                waitArgument,
                "5"
            };

            ParsedResult parsedResult;
            parsedResult = this.parser.Parse(commandLineArguments);

            Assert.False(parsedResult.Result.ErrorOccured);
            Assert.Equal(MainCommands.Create, parsedResult.ParsedArguments.MainCommand);
            Assert.Equal(TimeSpan.FromSeconds(5), parsedResult.ParsedArguments.WaitTime);
            Assert.Equal(string.Empty, parsedResult.ParsedArguments.Destination);
            Assert.Equal(string.Empty, parsedResult.ParsedArguments.Source);
            Assert.Equal(this.commandDefaults.RetryAmount, parsedResult.ParsedArguments.RetryAmount);
            Assert.IsType<NoCompress>(parsedResult.ParsedArguments.Compress);
        }

        [Theory]
        [InlineData("-s")]
        [InlineData("--source")]
        public void Valid_Source_Command_Is_Correctly_Parsed(string sourceArgument)
        {
            string expectedSource = "C:\\Temp\\Input";
            string[] commandLineArguments =
            {
                "-c",
                sourceArgument,
                expectedSource
            };

            ParsedResult parsedResult;
            parsedResult = this.parser.Parse(commandLineArguments);

            Assert.False(parsedResult.Result.ErrorOccured);
            Assert.Equal(MainCommands.Create, parsedResult.ParsedArguments.MainCommand);
            Assert.Equal(expectedSource, parsedResult.ParsedArguments.Source);
            Assert.Equal(string.Empty, parsedResult.ParsedArguments.Destination);
            Assert.Equal(this.commandDefaults.RetryAmount, parsedResult.ParsedArguments.RetryAmount);
            Assert.Equal(this.commandDefaults.WaitTime, parsedResult.ParsedArguments.WaitTime);
            Assert.IsType<NoCompress>(parsedResult.ParsedArguments.Compress);
        }

        [Theory]
        [InlineData("-d")]
        [InlineData("--destination")]
        public void Valid_Destination_Command_Is_Correctly_Parsed(string destinationArgument)
        {
            string expectedDestination = "archive.dat";
            string[] commandLineArguments =
            {
                "-c",
                destinationArgument,
                expectedDestination
            };

            ParsedResult parsedResult;
            parsedResult = this.parser.Parse(commandLineArguments);

            Assert.False(parsedResult.Result.ErrorOccured);
            Assert.Equal(MainCommands.Create, parsedResult.ParsedArguments.MainCommand);
            Assert.Equal(expectedDestination, parsedResult.ParsedArguments.Destination);
            Assert.Equal(string.Empty, parsedResult.ParsedArguments.Source);
            Assert.Equal(this.commandDefaults.RetryAmount, parsedResult.ParsedArguments.RetryAmount);
            Assert.Equal(this.commandDefaults.WaitTime, parsedResult.ParsedArguments.WaitTime);
            Assert.IsType<NoCompress>(parsedResult.ParsedArguments.Compress);
        }

        [Fact]
        public void Multiple_Valid_Secondary_Commands_Are_Correctly_Parsed()
        {
            string[] commandLineArguments =
            {
                "-c",
                "-s",
                "C:\\Temp\\Input",
                "-d",
                "archive.dat",
                "-rle",
                "-r",
                "3",
                "-w",
                "5"
            };

            ParsedResult parsedResult;
            parsedResult = this.parser.Parse(commandLineArguments);

            Assert.False(parsedResult.Result.ErrorOccured);
            Assert.Equal(MainCommands.Create, parsedResult.ParsedArguments.MainCommand);
            Assert.Equal("C:\\Temp\\Input", parsedResult.ParsedArguments.Source);
            Assert.Equal("archive.dat", parsedResult.ParsedArguments.Destination);
            Assert.IsType<RleCompress>(parsedResult.ParsedArguments.Compress);
            Assert.Equal(3, parsedResult.ParsedArguments.RetryAmount);
            Assert.Equal(TimeSpan.FromSeconds(5), parsedResult.ParsedArguments.WaitTime);
        }

        [Fact]
        public void Invalid_Primary_Command_Results_In_Error()
        {
            string[] commandLineArguments =
            {
                "--unknown"
            };

            ParsedResult parsedResult;
            parsedResult = this.parser.Parse(commandLineArguments);

            Assert.True(parsedResult.Result.ErrorOccured);
            Assert.Equal(MainCommands.None, parsedResult.ParsedArguments.MainCommand);
            Assert.Equal("--unknown", parsedResult.Result.WrongArgument);
        }

        [Fact]
        public void Invalid_Secondary_Command_Results_In_Error()
        {
            string[] commandLineArguments =
            {
                "-c",
                "--unknown"
            };

            ParsedResult parsedResult;
            parsedResult = this.parser.Parse(commandLineArguments);

            Assert.True(parsedResult.Result.ErrorOccured);
            Assert.Equal(MainCommands.Create, parsedResult.ParsedArguments.MainCommand);
            Assert.Equal("--unknown", parsedResult.Result.WrongArgument);
        }

        [Fact]
        public void String_Instead_Of_A_Number_Results_In_Error()
        {
            string[] commandLineArguments =
            {
                "-c",
                "-r",
                "abc"
            };

            ParsedResult parsedResult;
            parsedResult = this.parser.Parse(commandLineArguments);

            Assert.True(parsedResult.Result.ErrorOccured);
            Assert.Equal(MainCommands.Create, parsedResult.ParsedArguments.MainCommand);
            Assert.Equal("abc", parsedResult.Result.WrongArgument);
        }

        [Fact]
        public void Secondary_Command_As_First_Argument_Results_In_Error()
        {
            string[] commandLineArguments =
            {
                "-s",
                "C:\\Temp\\Input"
            };

            ParsedResult parsedResult;
            parsedResult = this.parser.Parse(commandLineArguments);

            Assert.True(parsedResult.Result.ErrorOccured);
            Assert.Equal(MainCommands.None, parsedResult.ParsedArguments.MainCommand);
            Assert.Equal("-s", parsedResult.Result.WrongArgument);
        }

        [Fact]
        public void Missing_Retry_Value_Results_In_Error()
        {
            string[] commandLineArguments =
            {
                "-c",
                "-r"
            };

            ParsedResult parsedResult;
            parsedResult = this.parser.Parse(commandLineArguments);

            Assert.True(parsedResult.Result.ErrorOccured);
            Assert.Equal(MainCommands.Create, parsedResult.ParsedArguments.MainCommand);
            Assert.Equal("-r", parsedResult.Result.WrongArgument);
        }

        [Fact]
        public void Missing_Source_Value_Results_In_Error()
        {
            string[] commandLineArguments =
            {
                "-c",
                "-s"
            };

            ParsedResult parsedResult;
            parsedResult = this.parser.Parse(commandLineArguments);

            Assert.True(parsedResult.Result.ErrorOccured);
            Assert.Equal(MainCommands.Create, parsedResult.ParsedArguments.MainCommand);
            Assert.Equal("-s", parsedResult.Result.WrongArgument);
        }

        private void AssertDefaultArguments(ParsedResult parsedResult)
        {
            Assert.Equal(string.Empty, parsedResult.ParsedArguments.Destination);
            Assert.Equal(string.Empty, parsedResult.ParsedArguments.Source);
            Assert.Equal(this.commandDefaults.RetryAmount, parsedResult.ParsedArguments.RetryAmount);
            Assert.Equal(this.commandDefaults.WaitTime, parsedResult.ParsedArguments.WaitTime);
            Assert.IsType<NoCompress>(parsedResult.ParsedArguments.Compress);
        }
    }
}
