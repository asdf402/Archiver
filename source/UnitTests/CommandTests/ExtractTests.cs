namespace Archiver.Tests
{
    using System.Text;
    using Xunit;

    public class ExtractTests : IDisposable
    {
        private readonly string testDirectory;

        public ExtractTests()
        {
            this.testDirectory = Path.Combine(
                Path.GetTempPath(),
                "ArchiverTests",
                Guid.NewGuid().ToString());

            Directory.CreateDirectory(this.testDirectory);
        }

        [Fact]
        public void Execute_ExtractsSingleFile_WhenArchiveIsNotCompressed()
        {
            string archivePath = Path.Combine(this.testDirectory, "archive.dat");
            string destinationPath = Path.Combine(this.testDirectory, "destination");

            byte[] content = Encoding.UTF8.GetBytes("Hello World!");

            this.CreateArchive(
                archivePath,
                0,
                new ArchiveTestFile("hello.txt", content, content));

            Extract extract = new Extract();
            ParsedArguments arguments = this.CreateArguments(
                archivePath,
                destinationPath);

            Result result = extract.Execute(arguments);

            string extractedFilePath = Path.Combine(
                destinationPath,
                "hello.txt");

            Assert.False(result.ErrorOccured);
            Assert.True(File.Exists(extractedFilePath));
            Assert.Equal(content, File.ReadAllBytes(extractedFilePath));
        }

        [Fact]
        public void Execute_ExtractsMultipleFiles()
        {
            string archivePath = Path.Combine(this.testDirectory, "archive.dat");
            string destinationPath = Path.Combine(this.testDirectory, "destination");

            byte[] firstContent = Encoding.UTF8.GetBytes("First file");
            byte[] secondContent = Encoding.UTF8.GetBytes("Second file");

            this.CreateArchive(
                archivePath,
                0,
                new ArchiveTestFile(
                    "first.txt",
                    firstContent,
                    firstContent),
                new ArchiveTestFile(
                    "second.txt",
                    secondContent,
                    secondContent));

            Extract extract = new Extract();
            ParsedArguments arguments = this.CreateArguments(
                archivePath,
                destinationPath);

            Result result = extract.Execute(arguments);

            Assert.False(result.ErrorOccured);

            Assert.Equal(
                firstContent,
                File.ReadAllBytes(
                    Path.Combine(destinationPath, "first.txt")));

            Assert.Equal(
                secondContent,
                File.ReadAllBytes(
                    Path.Combine(destinationPath, "second.txt")));
        }

        [Fact]
        public void Execute_CreatesDestinationDirectory_WhenDirectoryDoesNotExist()
        {
            string archivePath = Path.Combine(this.testDirectory, "archive.dat");
            string destinationPath = Path.Combine(
                this.testDirectory,
                "notExistingDestination");

            byte[] content = Encoding.UTF8.GetBytes("content");

            this.CreateArchive(
                archivePath,
                0,
                new ArchiveTestFile("file.txt", content, content));

            Assert.False(Directory.Exists(destinationPath));

            Extract extract = new Extract();
            ParsedArguments arguments = this.CreateArguments(
                archivePath,
                destinationPath);

            Result result = extract.Execute(arguments);

            Assert.False(result.ErrorOccured);
            Assert.True(Directory.Exists(destinationPath));
            Assert.True(
                File.Exists(
                    Path.Combine(destinationPath, "file.txt")));
        }

        [Fact]
        public void Execute_CreatesSubdirectories_WhenFileContainsRelativePath()
        {
            string archivePath = Path.Combine(this.testDirectory, "archive.dat");
            string destinationPath = Path.Combine(this.testDirectory, "destination");

            byte[] content = Encoding.UTF8.GetBytes("nested file");

            string relativeFileName = Path.Combine(
                "first",
                "second",
                "file.txt");

            this.CreateArchive(
                archivePath,
                0,
                new ArchiveTestFile(
                    relativeFileName,
                    content,
                    content));

            Extract extract = new Extract();
            ParsedArguments arguments = this.CreateArguments(
                archivePath,
                destinationPath);

            Result result = extract.Execute(arguments);

            string expectedPath = Path.Combine(
                destinationPath,
                relativeFileName);

            Assert.False(result.ErrorOccured);
            Assert.True(File.Exists(expectedPath));
            Assert.Equal(content, File.ReadAllBytes(expectedPath));
        }

        [Fact]
        public void Execute_DoesNotOverwriteExistingFile()
        {
            string archivePath = Path.Combine(this.testDirectory, "archive.dat");
            string destinationPath = Path.Combine(this.testDirectory, "destination");

            Directory.CreateDirectory(destinationPath);

            string existingFilePath = Path.Combine(
                destinationPath,
                "file.txt");

            byte[] existingContent = Encoding.UTF8.GetBytes(
                "This content must remain.");

            byte[] archiveContent = Encoding.UTF8.GetBytes(
                "This content comes from the archive.");

            File.WriteAllBytes(existingFilePath, existingContent);

            this.CreateArchive(
                archivePath,
                0,
                new ArchiveTestFile(
                    "file.txt",
                    archiveContent,
                    archiveContent));

            Extract extract = new Extract();
            ParsedArguments arguments = this.CreateArguments(
                archivePath,
                destinationPath);

            Result result = extract.Execute(arguments);

            Assert.False(result.ErrorOccured);
            Assert.Equal(
                existingContent,
                File.ReadAllBytes(existingFilePath));
        }

        [Fact]
        public void Execute_ContinuesWithNextFile_WhenFileAlreadyExists()
        {
            string archivePath = Path.Combine(this.testDirectory, "archive.dat");
            string destinationPath = Path.Combine(this.testDirectory, "destination");

            Directory.CreateDirectory(destinationPath);

            string existingFilePath = Path.Combine(
                destinationPath,
                "existing.txt");

            byte[] existingContent = Encoding.UTF8.GetBytes(
                "Do not overwrite");

            byte[] archiveExistingContent = Encoding.UTF8.GetBytes(
                "Archive content");

            byte[] newFileContent = Encoding.UTF8.GetBytes(
                "New file");

            File.WriteAllBytes(existingFilePath, existingContent);

            this.CreateArchive(
                archivePath,
                0,
                new ArchiveTestFile(
                    "existing.txt",
                    archiveExistingContent,
                    archiveExistingContent),
                new ArchiveTestFile(
                    "new.txt",
                    newFileContent,
                    newFileContent));

            Extract extract = new Extract();
            ParsedArguments arguments = this.CreateArguments(
                archivePath,
                destinationPath);

            Result result = extract.Execute(arguments);

            Assert.False(result.ErrorOccured);

            Assert.Equal(
                existingContent,
                File.ReadAllBytes(existingFilePath));

            Assert.Equal(
                newFileContent,
                File.ReadAllBytes(
                    Path.Combine(destinationPath, "new.txt")));
        }

        [Fact]
        public void Execute_ExtractsRleCompressedFile()
        {
            string archivePath = Path.Combine(this.testDirectory, "archive.dat");
            string destinationPath = Path.Combine(this.testDirectory, "destination");

            byte[] uncompressedContent =
                Enumerable.Repeat((byte)0x20, 260).ToArray();

            byte[] compressedContent =
            {
                0xFF,
                0x20,
                0x05,
                0x20,
            };

            this.CreateArchive(
                archivePath,
                1,
                new ArchiveTestFile(
                    "rle.bin",
                    uncompressedContent,
                    compressedContent));

            Extract extract = new Extract();
            ParsedArguments arguments = this.CreateArguments(
                archivePath,
                destinationPath);

            Result result = extract.Execute(arguments);

            Assert.False(result.ErrorOccured);

            Assert.Equal(
                uncompressedContent,
                File.ReadAllBytes(
                    Path.Combine(destinationPath, "rle.bin")));
        }

        [Fact]
        public void Execute_ReturnsError_WhenSourceDoesNotExist()
        {
            string archivePath = Path.Combine(
                this.testDirectory,
                "does-not-exist.dat");

            string destinationPath = Path.Combine(
                this.testDirectory,
                "destination");

            Extract extract = new Extract();
            ParsedArguments arguments = this.CreateArguments(
                archivePath,
                destinationPath);

            Result result = extract.Execute(arguments);

            Assert.True(result.ErrorOccured);
            Assert.Equal(archivePath, result.WrongArgument);
        }

        [Fact]
        public void Execute_DoesNotCrash_WhenArchiveIsInvalid()
        {
            string archivePath = Path.Combine(this.testDirectory, "invalid.dat");
            string destinationPath = Path.Combine(this.testDirectory, "destination");

            File.WriteAllText(archivePath, "This is not an archive.");

            Extract extract = new Extract();
            ParsedArguments arguments = this.CreateArguments(
                archivePath,
                destinationPath);

            Result? result = null;

            Exception? exception = Record.Exception(
                () => result = extract.Execute(arguments));

            Assert.Null(exception);
            Assert.NotNull(result);
            Assert.True(result.ErrorOccured);
        }

        public void Dispose()
        {
            if (Directory.Exists(this.testDirectory))
            {
                Directory.Delete(this.testDirectory, true);
            }
        }

        private ParsedArguments CreateArguments(
            string source,
            string destination)
        {
            return new ParsedArguments(
                new NoCompress(),
                1,
                TimeSpan.FromSeconds(1),
                source,
                destination,
                new Extract());
        }

        private void CreateArchive(
            string archivePath,
            uint compressionType,
            params ArchiveTestFile[] files)
        {
            long totalUncompressedSize = files.Sum(
                file => (long)file.UncompressedContent.Length);

            using FileStream stream = new FileStream(
                archivePath,
                FileMode.Create,
                FileAccess.Write);

            using BinaryWriter writer = new BinaryWriter(stream);

            writer.Write(Encoding.UTF8.GetBytes("ARCHIVE"));
            writer.Write(compressionType);
            writer.Write(DateTime.Now.Ticks);
            writer.Write((uint)files.Length);
            writer.Write(totalUncompressedSize);

            foreach (ArchiveTestFile file in files)
            {
                byte[] fileNameBytes = Encoding.UTF8.GetBytes(
                    file.FileName);

                writer.Write((long)file.CompressedContent.Length);
                writer.Write((long)file.UncompressedContent.Length);
                writer.Write(fileNameBytes.Length);
                writer.Write(fileNameBytes);
                writer.Write(file.CompressedContent);
            }
        }

        private class ArchiveTestFile
        {
            public ArchiveTestFile(
                string fileName,
                byte[] uncompressedContent,
                byte[] compressedContent)
            {
                this.FileName = fileName;
                this.UncompressedContent = uncompressedContent;
                this.CompressedContent = compressedContent;
            }

            public string FileName
            {
                get;
            }

            public byte[] UncompressedContent
            {
                get;
            }

            public byte[] CompressedContent
            {
                get;
            }
        }
    }
}