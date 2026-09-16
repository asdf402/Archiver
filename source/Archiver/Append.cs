namespace Archiver
{
    public class Append : ICommand
    {
        public Result Execute(ParsedArguments parsedArguments)
        {
            ArchiveReader archiveReader = new ArchiveReader();
            MetaInformation metaInformation;

            try
            {
                FileStream destination = new FileStream(parsedArguments.Destination, FileMode.Open);
                using (BinaryReader binaryReader = new BinaryReader(destination))
                {
                    metaInformation = new MetaInformation(
                        new DateTime(0, 0, 0, 0, 0, 0),
                        archiveReader.ReadCompressType(binaryReader),
                        archiveReader.ReadFileAmount(binaryReader),
                        archiveReader.ReadFileSizeUncompressed(binaryReader));
                }
            }
            catch (IOException)
            {
                return new Result(true, ConsoleErrorOutput.WriteCouldNotOpenDestination, parsedArguments.Destination);
                throw;
            }

            return this.AppendFiles(parsedArguments, metaInformation, FileMode.Open);
        }

        public Result AppendFiles(ParsedArguments parsedArguments, MetaInformation metaInformation, FileMode destinationFileMode)
        {
            FileInformation fileInformation;
            ArchiveWriter archiveWriter = new ArchiveWriter();

            long fileSizeCompressedPosition;

            try
            {
                IEnumerable<string> files = Directory.EnumerateFiles(parsedArguments.Source);

                FileStream destination = new FileStream(parsedArguments.Destination, destinationFileMode);
                using (BinaryWriter binaryWriter = new BinaryWriter(destination))
                {
                    foreach (var file in files)
                    {
                        fileSizeCompressedPosition = binaryWriter.BaseStream.Position;

                        fileInformation = new FileInformation(
                            Path.GetRelativePath(parsedArguments.Source, file),
                            new FileInfo(file).Length,
                            0);

                        archiveWriter.WriteFileInformation(binaryWriter, fileInformation);

                        try
                        {
                            FileStream source = new FileStream(parsedArguments.Source, FileMode.Open);
                            using (BinaryReader binaryReader = new BinaryReader(source))
                            {
                                fileInformation.AddFileSizeCompressed(
                                    metaInformation.Compress.Execute(binaryReader, binaryWriter));
                            }
                        }
                        catch (IOException)
                        {
                            return new Result(true, ConsoleErrorOutput.WriteCouldNotOpenSource, file);
                        }

                        metaInformation.AddFileSizeUncompressed(fileInformation.FileSizeUncompressed);
                        metaInformation.IncreaseFileAmount();

                        archiveWriter.WriteAddedFileSizeCompressed(binaryWriter, fileSizeCompressedPosition, fileInformation);
                    }

                    archiveWriter.WriteChangedMetaInformation(binaryWriter, metaInformation.FilesSizeUncompressed, metaInformation.FileAmount);
                }

                return new Result(false, ConsoleErrorOutput.WriteNoErrorOccurred, "AppendFiles");
            }
            catch (IOException)
            {
                return new Result(true, ConsoleErrorOutput.WriteCouldNotOpenDestination, parsedArguments.Destination);
            }
        }
    }
}
