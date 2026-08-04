using Archiver;

namespace UnitTests.ValidatorTests
{
    public class PathTypeValidatorTests
    {
        private CommandDefaults commandDefaults;
        private PathTypeValidator pathTypeValidator;

        public PathTypeValidatorTests()
        {
            commandDefaults = new CommandDefaults();
            pathTypeValidator = new PathTypeValidator();
        }

        [Theory]
        [InlineData("C:\\_Temp_\\a", "C:\\_Temp_\\a.dat")]
        [InlineData("C:\\abcd\\hallo", "C:\\someDirectory\\archive.dat")]
        public void Valid_Create_Command_With_Source_And_Destination_Results_In_True(string source, string destination)
        {
            ParsedArguments parsed = new ParsedArguments(
                this.commandDefaults.RleCompress,
                this.commandDefaults.RetryAmount,
                this.commandDefaults.WaitTime,
                source,
                destination,
                MainCommands.Create);

            Assert.True(this.pathTypeValidator.IsValid(parsed));
        }

        [Theory]
        [InlineData("C:\\_Temp_\\a", "C:\\_Temp_\\a.dat")]
        [InlineData("C:\\abcd\\hallo", "C:\\someDirectory\\archive.dat")]
        public void Valid_Append_Command_With_Source_And_Destination_Results_In_True(string source, string destination)
        {
            ParsedArguments parsed = new ParsedArguments(
                this.commandDefaults.RleCompress,
                this.commandDefaults.RetryAmount,
                this.commandDefaults.WaitTime,
                source,
                destination,
                MainCommands.Append);

            Assert.True(this.pathTypeValidator.IsValid(parsed));
        }

        [Theory]
        [InlineData("C:\\_Temp_\\a.dat", "C:\\_Temp_\\a")]
        [InlineData("C:\\someDirectory\\archive.dat", "C:\\abcd\\hallo")]
        public void Valid_Extract_Command_With_Source_And_Destination_Results_In_True(string source, string destination)
        {
            ParsedArguments parsed = new ParsedArguments(
                this.commandDefaults.RleCompress,
                this.commandDefaults.RetryAmount,
                this.commandDefaults.WaitTime,
                source,
                destination,
                MainCommands.Extract);

            Assert.True(this.pathTypeValidator.IsValid(parsed));
        }

        [Theory]
        [InlineData("C:\\_Temp_\\a.dat")]
        [InlineData("C:\\abcd\\hallo.dat")]
        public void Valid_Info_Command_With_Source_And_Destination_Results_In_True(string source)
        {
            ParsedArguments parsed = new ParsedArguments(
                this.commandDefaults.RleCompress,
                this.commandDefaults.RetryAmount,
                this.commandDefaults.WaitTime,
                source,
                this.commandDefaults.Path,
                MainCommands.Info);

            Assert.True(this.pathTypeValidator.IsValid(parsed));
        }

        [Theory]
        [InlineData("C:\\_Temp_\\a.dat")]
        [InlineData("C:\\abcd\\hallo.dat")]
        public void Valid_List_Command_With_Source_And_Destination_Results_In_True(string source)
        {
            ParsedArguments parsed = new ParsedArguments(
                this.commandDefaults.RleCompress,
                this.commandDefaults.RetryAmount,
                this.commandDefaults.WaitTime,
                source,
                this.commandDefaults.Path,
                MainCommands.List);

            Assert.True(this.pathTypeValidator.IsValid(parsed));
        }

        [Theory]
        [InlineData("C:\\_Temp_\\a", "C:\\_Temp_\\a.dat")]
        [InlineData("C:\\abcd\\hallo", "C:\\someDirectory\\archive.dat")]
        public void Invalid_None_Command_With_Source_And_Destination_Results_In_False(string source, string destination)
        {
            ParsedArguments parsed = new ParsedArguments(
                this.commandDefaults.RleCompress,
                this.commandDefaults.RetryAmount,
                this.commandDefaults.WaitTime,
                source,
                destination,
                MainCommands.None);

            Assert.False(this.pathTypeValidator.IsValid(parsed));
        }

        [Theory]
        [InlineData(" C:\\_Temp_\\a", " C:\\_Temp_\\a.dat")]
        [InlineData("     C:\\abcd\\hallo", "     C:\\abcd\\hallo.dat")]
        [InlineData("   C:\\abcd\\hallo", "   C:\\abcd\\hallo.dat")]
        [InlineData("C:\\abcd\\hallo ", "C:\\abcd\\hallo.dat ")]
        [InlineData("C:\\abcd\\hallo      ", "C:\\abcd\\hallo.dat      ")]
        [InlineData("C:\\abcd\\hallo    ", "C:\\abcd\\hallo.dat    ")]
        [InlineData(" C:\\abcd\\hallo ", " C:\\abcd\\hallo.dat ")]
        [InlineData("   C:\\abcd\\hallo ", "   C:\\abcd\\hallo.dat ")]
        [InlineData("      C:\\abcd\\hallo      ", "      C:\\abcd\\hallo.dat      ")]
        public void Valid_Command_With_Source_And_Destination_With_White_Spaces_Results_In_True(string source, string destination)
        {
            ParsedArguments parsed = new ParsedArguments(
                this.commandDefaults.RleCompress,
                this.commandDefaults.RetryAmount,
                this.commandDefaults.WaitTime,
                source,
                destination,
                MainCommands.Create);

            Assert.True(this.pathTypeValidator.IsValid(parsed));
        }


        [Theory]
        [InlineData("C:\\_Temp_\\a.dat", "C:\\_Temp_\\a.dat")]
        [InlineData("C:\\abcd\\hallo.cs", "C:\\someDirectory\\archive.dat")]
        [InlineData("C:\\abcd\\hallo.pdf", "C:\\someDirectory\\archive.dat")]
        [InlineData("C:\\abcd\\hallo.", "C:\\someDirectory\\archive.dat")]
        public void Invalid_Source_Results_In_False(string source, string destination)
        {
            ParsedArguments parsed = new ParsedArguments(
                this.commandDefaults.RleCompress,
                this.commandDefaults.RetryAmount,
                this.commandDefaults.WaitTime,
                source,
                destination,
                MainCommands.Create);

            Assert.False(this.pathTypeValidator.IsValid(parsed));
        }

        [Theory]
        [InlineData("C:\\_Temp_\\a", "C:\\_Temp_\\a")]
        [InlineData("C:\\abcd\\hallo", "C:\\someDirectory\\archive.cs")]
        [InlineData("C:\\abcd\\hallo", "C:\\someDirectory\\archive.pdf")]
        [InlineData("C:\\abcd\\hallo", "C:\\someDirectory\\archive.")]
        public void Invalid_Destination_Results_In_False(string source, string destination)
        {
            ParsedArguments parsed = new ParsedArguments(
                this.commandDefaults.RleCompress,
                this.commandDefaults.RetryAmount,
                this.commandDefaults.WaitTime,
                source,
                destination,
                MainCommands.Create);

            Assert.False(this.pathTypeValidator.IsValid(parsed));
        }

        [Theory]
        [InlineData("/home/user/input/", "/home/user/archive.dat")]
        [InlineData("/tmp/source/", "/tmp/output/archive.dat")]
        [InlineData("/var/data/project/", "/var/backups/project.dat")]
        public void Valid_Source_And_Destination_On_Linux_Results_In_True(string source, string destination)
        {
            ParsedArguments parsed = new ParsedArguments(
                this.commandDefaults.RleCompress,
                this.commandDefaults.RetryAmount,
                this.commandDefaults.WaitTime,
                source,
                destination,
                MainCommands.Create);

            Assert.True(this.pathTypeValidator.IsValid(parsed));
        }

        [Theory]
        [InlineData("C:\\_Temp_\\a\\", "C:\\_Temp_\\a.DAT")]
        [InlineData("C:\\_Temp_\\a\\", "C:\\_Temp_\\a.Dat")]
        public void Destination_Extension_With_Different_Casing_Results_In_False(string source, string destination)
        {
            ParsedArguments parsed = new ParsedArguments(
                this.commandDefaults.RleCompress,
                this.commandDefaults.RetryAmount,
                this.commandDefaults.WaitTime,
                source,
                destination,
                MainCommands.Create);

            Assert.False(this.pathTypeValidator.IsValid(parsed));
        }
    }
}
