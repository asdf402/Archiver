namespace Archiver
{
    public class Extract : ICommand
    {
        public Result Execute(ParsedArguments parsedArguments)
        {
            MetaInformation metaInformation;
            FileInformation fileInformation;

            MetaInformationPositions metaInformationPositions = new MetaInformationPositions();
            ArchiveReader archiveReader = new ArchiveReader();

            try
            {
                FileStream source = new FileStream(parsedArguments.Source, FileMode.Open);
                using (BinaryReader binaryReader = new BinaryReader(source))
                {
                    try
                    {
                        metaInformation = archiveReader.ReadAllMetaInformation(binaryReader);
                    }
                    catch (Exception)
                    {
                        return new Result(
                            true,
                            ConsoleErrorOutput.WriteNoValidDatFile,
                            parsedArguments.Source);
                    }

                    this.CreateDirectories(parsedArguments.Destination);
                    binaryReader.BaseStream.Position = metaInformationPositions.EndOfMetaInformation;

                    for (int count = 0; count < metaInformation.FileAmount; count++)
                    {
                        try
                        {
                            fileInformation = archiveReader.ReadFileInformation(binaryReader);
                        }
                        catch (Exception)
                        {
                            return new Result(
                                true,
                                ConsoleErrorOutput.WriteNoValidDatFile,
                                parsedArguments.Source);
                        }

                        string fullPath = Path.Combine(
                            parsedArguments.Destination, fileInformation.FileName);
                        this.CreateDirectories(fullPath);

                        if (File.Exists(fullPath))
                        {
                            binaryReader.BaseStream.Position += fileInformation.FileSizeCompressed;
                            continue;
                        }

                        try
                        {
                            this.WriteFileData(
                                metaInformation.Compress,
                                fileInformation.FileSizeCompressed,
                                binaryReader,
                                fullPath);
                        }
                        catch (Exception)
                        {
                            throw new ArgumentException(
                                "can not open or write to the created file");
                        }
                    }
                }
            }
            catch (Exception)
            {
                return new Result(
                    true,
                    ConsoleErrorOutput.WriteCouldNotOpenSource,
                    parsedArguments.Source);
            }

            return new Result(
                false,
                ConsoleErrorOutput.WriteNoErrorOccurred,
                "extract command");
        }

        private void WriteFileData(
            ICompression compression, long fileSizeCompressed, BinaryReader binaryReader, string fullPath)
        {
            FileStream decompressDestination = File.Create(fullPath);
            using (BinaryWriter binaryWriter = new BinaryWriter(decompressDestination))
            {
                compression.Decompress(
                    binaryReader, binaryWriter, fileSizeCompressed);
            }
        }

        private void CreateDirectories(string fullPath)
        {
            string? directoryPath = Path.GetDirectoryName(fullPath);
            if (directoryPath != null &&
                directoryPath != string.Empty)
            {
                Directory.CreateDirectory(directoryPath);
            }
        }
    }
}