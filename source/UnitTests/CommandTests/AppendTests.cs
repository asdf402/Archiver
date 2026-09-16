using System.Text;

namespace Archiver.Tests
{
    public class AppendTests
    {
        private const uint NoCompression = 0;

        [Fact]
        public void Execute_With_One_File_Returns_No_Error()
        {
            string testDirectory = this.CreateTestDirectory();
            string sourceDirectory = Path.Combine(testDirectory, "source");
            string archivePath = Path.Combine(testDirectory, "archive.dat");

            Directory.CreateDirectory(sourceDirectory);

            File.WriteAllText(
                Path.Combine(sourceDirectory, "new.txt"),
                "Hello");

            this.CreateEmptyArchive(archivePath);

            try
            {
                ParsedArguments arguments =
                    this.CreateAppendArguments(sourceDirectory, archivePath);

                Append append = new Append();

                Result result = append.Execute(arguments);

                Assert.False(result.ErrorOccured);
            }
            finally
            {
                Directory.Delete(testDirectory, true);
            }
        }

        [Fact]
        public void Execute_With_One_File_Increases_File_Amount()
        {
            string testDirectory = this.CreateTestDirectory();
            string sourceDirectory = Path.Combine(testDirectory, "source");
            string archivePath = Path.Combine(testDirectory, "archive.dat");

            Directory.CreateDirectory(sourceDirectory);

            File.WriteAllText(
                Path.Combine(sourceDirectory, "new.txt"),
                "Hello");

            this.CreateEmptyArchive(archivePath);

            try
            {
                ParsedArguments arguments =
                    this.CreateAppendArguments(sourceDirectory, archivePath);

                Append append = new Append();

                Result result = append.Execute(arguments);

                Assert.False(result.ErrorOccured);

                uint fileAmount = this.ReadFileAmount(archivePath);

                Assert.Equal((uint)1, fileAmount);
            }
            finally
            {
                Directory.Delete(testDirectory, true);
            }
        }

        [Fact]
        public void Execute_With_Multiple_Files_Increases_File_Amount()
        {
            string testDirectory = this.CreateTestDirectory();
            string sourceDirectory = Path.Combine(testDirectory, "source");
            string archivePath = Path.Combine(testDirectory, "archive.dat");

            Directory.CreateDirectory(sourceDirectory);

            File.WriteAllText(
                Path.Combine(sourceDirectory, "first.txt"),
                "First");

            File.WriteAllText(
                Path.Combine(sourceDirectory, "second.txt"),
                "Second");

            File.WriteAllText(
                Path.Combine(sourceDirectory, "third.txt"),
                "Third");

            this.CreateEmptyArchive(archivePath);

            try
            {
                ParsedArguments arguments =
                    this.CreateAppendArguments(sourceDirectory, archivePath);

                Append append = new Append();

                Result result = append.Execute(arguments);

                Assert.False(result.ErrorOccured);

                uint fileAmount = this.ReadFileAmount(archivePath);

                Assert.Equal((uint)3, fileAmount);
            }
            finally
            {
                Directory.Delete(testDirectory, true);
            }
        }

        [Fact]
        public void Execute_Keeps_Existing_File_Amount()
        {
            string testDirectory = this.CreateTestDirectory();
            string sourceDirectory = Path.Combine(testDirectory, "source");
            string archivePath = Path.Combine(testDirectory, "archive.dat");

            Directory.CreateDirectory(sourceDirectory);

            File.WriteAllText(
                Path.Combine(sourceDirectory, "new.txt"),
                "New");

            this.CreateArchiveWithOneExistingFile(
                archivePath,
                "existing.txt",
                Encoding.UTF8.GetBytes("Existing"));

            try
            {
                ParsedArguments arguments =
                    this.CreateAppendArguments(sourceDirectory, archivePath);

                Append append = new Append();

                Result result = append.Execute(arguments);

                Assert.False(result.ErrorOccured);

                uint fileAmount = this.ReadFileAmount(archivePath);

                Assert.Equal((uint)2, fileAmount);
            }
            finally
            {
                Directory.Delete(testDirectory, true);
            }
        }

        [Fact]
        public void Execute_Updates_Total_Uncompressed_Size()
        {
            string testDirectory = this.CreateTestDirectory();
            string sourceDirectory = Path.Combine(testDirectory, "source");
            string archivePath = Path.Combine(testDirectory, "archive.dat");

            Directory.CreateDirectory(sourceDirectory);

            byte[] firstContent = Encoding.UTF8.GetBytes("12345");
            byte[] secondContent = Encoding.UTF8.GetBytes("1234567890");

            File.WriteAllBytes(
                Path.Combine(sourceDirectory, "first.txt"),
                firstContent);

            File.WriteAllBytes(
                Path.Combine(sourceDirectory, "second.txt"),
                secondContent);

            this.CreateEmptyArchive(archivePath);

            try
            {
                ParsedArguments arguments =
                    this.CreateAppendArguments(sourceDirectory, archivePath);

                Append append = new Append();

                Result result = append.Execute(arguments);

                Assert.False(result.ErrorOccured);

                long totalSize =
                    this.ReadFilesSizeUncompressed(archivePath);

                Assert.Equal(
                    firstContent.Length + secondContent.Length,
                    totalSize);
            }
            finally
            {
                Directory.Delete(testDirectory, true);
            }
        }

        [Fact]
        public void Execute_Adds_New_Size_To_Existing_Uncompressed_Size()
        {
            string testDirectory = this.CreateTestDirectory();
            string sourceDirectory = Path.Combine(testDirectory, "source");
            string archivePath = Path.Combine(testDirectory, "archive.dat");

            Directory.CreateDirectory(sourceDirectory);

            byte[] existingContent =
                Encoding.UTF8.GetBytes("Existing");

            byte[] newContent =
                Encoding.UTF8.GetBytes("New content");

            this.CreateArchiveWithOneExistingFile(
                archivePath,
                "existing.txt",
                existingContent);

            File.WriteAllBytes(
                Path.Combine(sourceDirectory, "new.txt"),
                newContent);

            try
            {
                ParsedArguments arguments =
                    this.CreateAppendArguments(sourceDirectory, archivePath);

                Append append = new Append();

                Result result = append.Execute(arguments);

                Assert.False(result.ErrorOccured);

                long totalSize =
                    this.ReadFilesSizeUncompressed(archivePath);

                Assert.Equal(
                    existingContent.Length + newContent.Length,
                    totalSize);
            }
            finally
            {
                Directory.Delete(testDirectory, true);
            }
        }

        [Fact]
        public void Execute_Does_Not_Overwrite_Existing_File_Data()
        {
            string testDirectory = this.CreateTestDirectory();
            string sourceDirectory = Path.Combine(testDirectory, "source");
            string archivePath = Path.Combine(testDirectory, "archive.dat");

            Directory.CreateDirectory(sourceDirectory);

            byte[] existingContent =
                Encoding.UTF8.GetBytes("Existing content");

            this.CreateArchiveWithOneExistingFile(
                archivePath,
                "existing.txt",
                existingContent);

            File.WriteAllText(
                Path.Combine(sourceDirectory, "new.txt"),
                "New content");

            try
            {
                byte[] archiveBefore = File.ReadAllBytes(archivePath);

                ParsedArguments arguments =
                    this.CreateAppendArguments(sourceDirectory, archivePath);

                Append append = new Append();

                Result result = append.Execute(arguments);

                Assert.False(result.ErrorOccured);

                byte[] archiveAfter = File.ReadAllBytes(archivePath);

                /*
                 * The existing file block must still exist.
                 *
                 * Do not compare the entire archives because FileAmount
                 * and FilesSizeUncompressed are expected to change.
                 */
                byte[] existingFileName =
                    Encoding.UTF8.GetBytes("existing.txt");

                Assert.True(
                    this.ContainsBytes(
                        archiveAfter,
                        existingFileName));

                Assert.True(
                    this.ContainsBytes(
                        archiveAfter,
                        existingContent));
            }
            finally
            {
                Directory.Delete(testDirectory, true);
            }
        }

        [Fact]
        public void Execute_Stores_Appended_File_Name()
        {
            string testDirectory = this.CreateTestDirectory();
            string sourceDirectory = Path.Combine(testDirectory, "source");
            string archivePath = Path.Combine(testDirectory, "archive.dat");

            Directory.CreateDirectory(sourceDirectory);

            File.WriteAllText(
                Path.Combine(sourceDirectory, "important.txt"),
                "Content");

            this.CreateEmptyArchive(archivePath);

            try
            {
                ParsedArguments arguments =
                    this.CreateAppendArguments(sourceDirectory, archivePath);

                Append append = new Append();

                Result result = append.Execute(arguments);

                Assert.False(result.ErrorOccured);

                byte[] archiveBytes = File.ReadAllBytes(archivePath);

                Assert.True(
                    this.ContainsBytes(
                        archiveBytes,
                        Encoding.UTF8.GetBytes("important.txt")));
            }
            finally
            {
                Directory.Delete(testDirectory, true);
            }
        }

        [Fact]
        public void Execute_Stores_Uncompressed_File_Content()
        {
            string testDirectory = this.CreateTestDirectory();
            string sourceDirectory = Path.Combine(testDirectory, "source");
            string archivePath = Path.Combine(testDirectory, "archive.dat");

            Directory.CreateDirectory(sourceDirectory);

            byte[] content =
                Encoding.UTF8.GetBytes("Unique append content 12345");

            File.WriteAllBytes(
                Path.Combine(sourceDirectory, "content.txt"),
                content);

            this.CreateEmptyArchive(archivePath);

            try
            {
                ParsedArguments arguments =
                    this.CreateAppendArguments(sourceDirectory, archivePath);

                Append append = new Append();

                Result result = append.Execute(arguments);

                Assert.False(result.ErrorOccured);

                byte[] archiveBytes = File.ReadAllBytes(archivePath);

                Assert.True(
                    this.ContainsBytes(
                        archiveBytes,
                        content));
            }
            finally
            {
                Directory.Delete(testDirectory, true);
            }
        }

        [Fact]
        public void Execute_With_File_In_Subdirectory_Appends_File()
        {
            string testDirectory = this.CreateTestDirectory();
            string sourceDirectory = Path.Combine(testDirectory, "source");
            string subDirectory = Path.Combine(sourceDirectory, "sub");
            string archivePath = Path.Combine(testDirectory, "archive.dat");

            Directory.CreateDirectory(subDirectory);

            File.WriteAllText(
                Path.Combine(subDirectory, "nested.txt"),
                "Nested");

            this.CreateEmptyArchive(archivePath);

            try
            {
                ParsedArguments arguments =
                    this.CreateAppendArguments(sourceDirectory, archivePath);

                Append append = new Append();

                Result result = append.Execute(arguments);

                Assert.False(result.ErrorOccured);
                Assert.Equal((uint)1, this.ReadFileAmount(archivePath));
            }
            finally
            {
                Directory.Delete(testDirectory, true);
            }
        }

        [Fact]
        public void Execute_With_File_In_Subdirectory_Stores_Relative_Path()
        {
            string testDirectory = this.CreateTestDirectory();
            string sourceDirectory = Path.Combine(testDirectory, "source");
            string subDirectory = Path.Combine(sourceDirectory, "sub");
            string archivePath = Path.Combine(testDirectory, "archive.dat");

            Directory.CreateDirectory(subDirectory);

            File.WriteAllText(
                Path.Combine(subDirectory, "nested.txt"),
                "Nested");

            this.CreateEmptyArchive(archivePath);

            try
            {
                ParsedArguments arguments =
                    this.CreateAppendArguments(sourceDirectory, archivePath);

                Append append = new Append();

                Result result = append.Execute(arguments);

                Assert.False(result.ErrorOccured);

                string expectedRelativePath =
                    Path.Combine("sub", "nested.txt");

                byte[] archiveBytes = File.ReadAllBytes(archivePath);

                Assert.True(
                    this.ContainsBytes(
                        archiveBytes,
                        Encoding.UTF8.GetBytes(expectedRelativePath)));
            }
            finally
            {
                Directory.Delete(testDirectory, true);
            }
        }

        [Fact]
        public void Execute_With_Empty_Source_Does_Not_Change_File_Amount()
        {
            string testDirectory = this.CreateTestDirectory();
            string sourceDirectory = Path.Combine(testDirectory, "source");
            string archivePath = Path.Combine(testDirectory, "archive.dat");

            Directory.CreateDirectory(sourceDirectory);

            this.CreateEmptyArchive(archivePath);

            try
            {
                ParsedArguments arguments =
                    this.CreateAppendArguments(sourceDirectory, archivePath);

                Append append = new Append();

                Result result = append.Execute(arguments);

                Assert.False(result.ErrorOccured);
                Assert.Equal((uint)0, this.ReadFileAmount(archivePath));
                Assert.Equal(0, this.ReadFilesSizeUncompressed(archivePath));
            }
            finally
            {
                Directory.Delete(testDirectory, true);
            }
        }

        [Fact]
        public void Execute_With_Missing_Destination_Returns_Error()
        {
            string testDirectory = this.CreateTestDirectory();
            string sourceDirectory = Path.Combine(testDirectory, "source");
            string archivePath = Path.Combine(testDirectory, "missing.dat");

            Directory.CreateDirectory(sourceDirectory);

            File.WriteAllText(
                Path.Combine(sourceDirectory, "test.txt"),
                "Test");

            try
            {
                ParsedArguments arguments =
                    this.CreateAppendArguments(sourceDirectory, archivePath);

                Append append = new Append();

                Result result = append.Execute(arguments);

                Assert.True(result.ErrorOccured);
            }
            finally
            {
                Directory.Delete(testDirectory, true);
            }
        }

        private string CreateTestDirectory()
        {
            string path = Path.Combine(
                Path.GetTempPath(),
                "ArchiverTests",
                Guid.NewGuid().ToString());

            Directory.CreateDirectory(path);

            return path;
        }

        private ParsedArguments CreateAppendArguments(
            string sourceDirectory,
            string archivePath)
        {
            string[] arguments =
            {
                "-a",
                "-s",
                sourceDirectory,
                "-d",
                archivePath,
            };

            Parser parser = new Parser();
            ParsedResult parsedResult = parser.Parse(arguments);

            Assert.False(parsedResult.Result.ErrorOccured);

            return parsedResult.ParsedArguments;
        }

        private void CreateEmptyArchive(string archivePath)
        {
            using FileStream stream = new FileStream(
                archivePath,
                FileMode.Create,
                FileAccess.Write);

            using BinaryWriter writer = new BinaryWriter(stream);

            writer.Write(Encoding.UTF8.GetBytes("ARCHIVE"));

            writer.Write(NoCompression);

            writer.Write(DateTime.Now.Ticks);

            writer.Write((uint)0);

            writer.Write((long)0);
        }

        private void CreateArchiveWithOneExistingFile(
            string archivePath,
            string fileName,
            byte[] content)
        {
            using FileStream stream = new FileStream(
                archivePath,
                FileMode.Create,
                FileAccess.Write);

            using BinaryWriter writer = new BinaryWriter(stream);

            writer.Write(Encoding.UTF8.GetBytes("ARCHIVE"));

            writer.Write(NoCompression);
            writer.Write(DateTime.Now.Ticks);
            writer.Write((uint)1);
            writer.Write((long)content.Length);

            byte[] fileNameBytes =
                Encoding.UTF8.GetBytes(fileName);

            writer.Write((long)content.Length);
            writer.Write((long)content.Length);
            writer.Write(fileNameBytes.Length);

            writer.Write(fileNameBytes);
            writer.Write(content);
        }

        private uint ReadFileAmount(string archivePath)
        {
            using FileStream stream =
                new FileStream(archivePath, FileMode.Open, FileAccess.Read);

            using BinaryReader reader = new BinaryReader(stream);

            reader.BaseStream.Position = 19;

            return reader.ReadUInt32();
        }

        private long ReadFilesSizeUncompressed(string archivePath)
        {
            using FileStream stream =
                new FileStream(archivePath, FileMode.Open, FileAccess.Read);

            using BinaryReader reader = new BinaryReader(stream);

            reader.BaseStream.Position = 23;

            return reader.ReadInt64();
        }

        private bool ContainsBytes(
            byte[] source,
            byte[] searchedBytes)
        {
            if (searchedBytes.Length == 0)
            {
                return true;
            }

            if (searchedBytes.Length > source.Length)
            {
                return false;
            }

            for (int sourceIndex = 0;
                 sourceIndex <= source.Length - searchedBytes.Length;
                 sourceIndex++)
            {
                bool equal = true;

                for (int searchIndex = 0;
                     searchIndex < searchedBytes.Length;
                     searchIndex++)
                {
                    if (source[sourceIndex + searchIndex]
                        != searchedBytes[searchIndex])
                    {
                        equal = false;
                        break;
                    }
                }

                if (equal)
                {
                    return true;
                }
            }

            return false;
        }
    }
}