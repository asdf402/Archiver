using System.Text;
using Xunit;

namespace Archiver.Tests
{
    public class ListTests
    {
        [Fact]
        public void Execute_With_Valid_Empty_Archive_Returns_No_Error()
        {
            string archivePath = this.CreateArchive(
                Array.Empty<TestFileInformation>());

            try
            {
                ParsedArguments parsedArguments = this.CreateListArguments(archivePath);
                List list = new List();

                Result result = list.Execute(parsedArguments);

                Assert.False(result.ErrorOccured);
            }
            finally
            {
                File.Delete(archivePath);
            }
        }

        [Fact]
        public void Execute_With_Valid_Archive_With_One_File_Returns_No_Error()
        {
            TestFileInformation[] files =
            {
                new TestFileInformation(
                    "important.txt",
                    100,
                    new byte[] { 1, 2, 3 }),
            };

            string archivePath = this.CreateArchive(files);

            try
            {
                ParsedArguments parsedArguments = this.CreateListArguments(archivePath);
                List list = new List();

                Result result = list.Execute(parsedArguments);

                Assert.False(result.ErrorOccured);
            }
            finally
            {
                File.Delete(archivePath);
            }
        }

        [Fact]
        public void Execute_With_Multiple_Files_Returns_No_Error()
        {
            TestFileInformation[] files =
            {
                new TestFileInformation(
                    "first.txt",
                    100,
                    new byte[] { 1, 2, 3 }),

                new TestFileInformation(
                    "second.txt",
                    200,
                    new byte[] { 4, 5 }),

                new TestFileInformation(
                    "directory\\third.txt",
                    300,
                    new byte[] { 6, 7 }),
            };

            string archivePath = this.CreateArchive(files);

            try
            {
                ParsedArguments parsedArguments = this.CreateListArguments(archivePath);
                List list = new List();

                Result result = list.Execute(parsedArguments);

                Assert.False(result.ErrorOccured);
            }
            finally
            {
                File.Delete(archivePath);
            }
        }

        [Fact]
        public void Execute_With_One_File_Prints_File_Name()
        {
            TestFileInformation[] files =
            {
                new TestFileInformation(
                    "important.txt",
                    100,
                    new byte[] { 1, 2, 3 }),
            };

            string archivePath = this.CreateArchive(files);

            TextWriter originalOutput = Console.Out;
            StringWriter consoleOutput = new StringWriter();

            try
            {
                Console.SetOut(consoleOutput);

                ParsedArguments parsedArguments = this.CreateListArguments(archivePath);
                List list = new List();

                Result result = list.Execute(parsedArguments);

                Assert.False(result.ErrorOccured);
                Assert.Contains(
                    "important.txt",
                    consoleOutput.ToString());
            }
            finally
            {
                Console.SetOut(originalOutput);
                consoleOutput.Dispose();
                File.Delete(archivePath);
            }
        }

        [Fact]
        public void Execute_With_Multiple_Files_Prints_All_File_Names()
        {
            TestFileInformation[] files =
            {
                new TestFileInformation(
                    "first.txt",
                    100,
                    new byte[] { 1, 2, 3 }),

                new TestFileInformation(
                    "second.txt",
                    200,
                    new byte[] { 4, 5 }),

                new TestFileInformation(
                    "directory\\third.txt",
                    300,
                    new byte[] { 6 }),
            };

            string archivePath = this.CreateArchive(files);

            TextWriter originalOutput = Console.Out;
            StringWriter consoleOutput = new StringWriter();

            try
            {
                Console.SetOut(consoleOutput);

                ParsedArguments parsedArguments = this.CreateListArguments(archivePath);
                List list = new List();

                Result result = list.Execute(parsedArguments);

                Assert.False(result.ErrorOccured);

                string output = consoleOutput.ToString();

                Assert.Contains("first.txt", output);
                Assert.Contains("second.txt", output);
                Assert.Contains("directory\\third.txt", output);
            }
            finally
            {
                Console.SetOut(originalOutput);
                consoleOutput.Dispose();
                File.Delete(archivePath);
            }
        }

        [Fact]
        public void Execute_With_Missing_Archive_Returns_Error()
        {
            string archivePath =
                Path.Combine(
                    Path.GetTempPath(),
                    Guid.NewGuid() + ".dat");

            ParsedArguments parsedArguments = this.CreateListArguments(archivePath);
            List list = new List();

            Result result = list.Execute(parsedArguments);

            Assert.True(result.ErrorOccured);
        }

        [Fact]
        public void Execute_Prints_Only_File_Names()
        {
            TestFileInformation[] files =
            {
                new TestFileInformation(
                    "first.txt",
                    123456789,
                    new byte[] { 1, 2, 3 }),

                new TestFileInformation(
                    "second.txt",
                    987654321,
                    new byte[] { 4, 5 }),
            };

            string archivePath = this.CreateArchive(files);

            TextWriter originalOutput = Console.Out;
            StringWriter consoleOutput = new StringWriter();

            try
            {
                Console.SetOut(consoleOutput);

                ParsedArguments parsedArguments = this.CreateListArguments(archivePath);
                List list = new List();

                Result result = list.Execute(parsedArguments);

                Assert.False(result.ErrorOccured);

                string output = consoleOutput.ToString();

                Assert.Contains("first.txt", output);
                Assert.Contains("second.txt", output);

                Assert.DoesNotContain("123456789", output);
                Assert.DoesNotContain("987654321", output);
                Assert.DoesNotContain("Compressed file size", output);
                Assert.DoesNotContain("Uncompressed file size", output);
            }
            finally
            {
                Console.SetOut(originalOutput);
                consoleOutput.Dispose();
                File.Delete(archivePath);
            }
        }

        private string CreateArchive(TestFileInformation[] files)
        {
            string archivePath =
                Path.Combine(
                    Path.GetTempPath(),
                    Guid.NewGuid() + ".dat");

            using FileStream stream = new FileStream(
                archivePath,
                FileMode.Create,
                FileAccess.Write);

            using BinaryWriter writer = new BinaryWriter(stream);

            writer.Write(Encoding.UTF8.GetBytes("ARCHIVE"));

            // Compression type.
            writer.Write((uint)0);

            DateTime creationDate =
                new DateTime(2026, 9, 9, 12, 0, 0);

            writer.Write(creationDate.Ticks);

            writer.Write((uint)files.Length);

            long totalUncompressedSize = 0;

            foreach (TestFileInformation file in files)
            {
                totalUncompressedSize += file.UncompressedSize;
            }

            writer.Write(totalUncompressedSize);

            foreach (TestFileInformation file in files)
            {
                byte[] fileNameBytes =
                    Encoding.UTF8.GetBytes(file.FileName);

                writer.Write((long)file.Content.Length);
                writer.Write(file.UncompressedSize);
                writer.Write(fileNameBytes.Length);

                writer.Write(fileNameBytes);
                writer.Write(file.Content);
            }

            return archivePath;
        }

        private ParsedArguments CreateListArguments(string archivePath)
        {
            string[] arguments =
            {
                "-l",
                "-s",
                archivePath,
            };

            Parser parser = new Parser();
            ParsedResult parsedResult = parser.Parse(arguments);

            Assert.False(parsedResult.Result.ErrorOccured);

            return parsedResult.ParsedArguments;
        }

        private sealed class TestFileInformation
        {
            public TestFileInformation(
                string fileName,
                long uncompressedSize,
                byte[] content)
            {
                this.FileName = fileName;
                this.UncompressedSize = uncompressedSize;
                this.Content = content;
            }

            public string FileName { get; }

            public long UncompressedSize { get; }

            public byte[] Content { get; }
        }
    }
}