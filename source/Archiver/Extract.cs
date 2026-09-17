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
                    metaInformation = archiveReader.ReadAllMetaInformation(binaryReader);

                    binaryReader.BaseStream.Position = metaInformationPositions.EndOfMetaInformation;

                    try
                    {
                        FileStream destination = new FileStream(parsedArguments.Destination, FileMode.OpenOrCreate);
                        using (BinaryWriter binaryWriter = new BinaryWriter(destination))
                        {
                            for (int count = 0; count < metaInformation.FileAmount; count++)
                            {
                                fileInformation = archiveReader.ReadFileInformation(binaryReader);

                                Directory.CreateDirectory(fileInformation.FileName);
                                if (File.Exists(fileInformation.FileName))
                                {
                                    binaryReader.BaseStream.Position += fileInformation.FileSizeCompressed;
                                    continue;
                                }

                                File.Create(fileInformation.FileName);

                                try
                                {
                                    FileStream decompressDestination = new FileStream(fileInformation.FileName, FileMode.Open);
                                    using (BinaryWriter binaryDecompressWriter = new BinaryWriter(decompressDestination))
                                    {
                                        metaInformation.Compress.Decompress(binaryReader, binaryWriter, fileInformation.FileSizeCompressed);
                                    }
                                }
                                catch (IOException)
                                {
                                    throw new ArgumentException("can not open or write to the created file");
                                }
                            }
                        }
                    }
                    catch (IOException)
                    {
                        return new Result(true, ConsoleErrorOutput.WriteCouldNotOpenDestination, parsedArguments.Destination);
                    }
                }
            }
            catch (IOException)
            {
                return new Result(true, ConsoleErrorOutput.WriteCouldNotOpenSource, parsedArguments.Source);
            }

            return new Result(false, ConsoleErrorOutput.WriteNoErrorOccurred, "extract command");
        }
    }
}