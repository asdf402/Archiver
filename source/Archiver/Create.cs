namespace Archiver
{
    public class Create : ICommand
    {
        public void Execute(ParsedArguments parsedArguments)
        {
            long fileInformationPosition;

            FileInformation fileInformation;
            ArchiveInformation archiveInformation = new ArchiveInformation();
            MetaInformation metaInformation = new MetaInformation(
                DateTime.UtcNow,
                parsedArguments.Compress,
                0,
                0);
            ArchiveWriter archiveWriter = new ArchiveWriter();

            try
            {
                IEnumerable<string> files = Directory.EnumerateFiles(parsedArguments.Source);

                FileStream destinationStream = new FileStream(parsedArguments.Destination, FileMode.CreateNew);
                using (BinaryWriter writer = new BinaryWriter(destinationStream))
                {
                    archiveWriter.WriteArchiveHeader(writer, archiveInformation.ArchiveIdentifier);
                    archiveWriter.WriteMetaInformation(writer, metaInformation);

                    fileInformationPosition = destinationStream.Position;

                    foreach (var file in files)
                    {
                        fileInformation = new FileInformation(
                            Path.GetRelativePath(parsedArguments.Source, file),
                            new FileInfo(file).Length,
                            0);

                        archiveWriter.WriteFileInformation(writer, fileInformation);

                        FileStream sourceStream = new FileStream(file, FileMode.Open);
                        using (BinaryReader reader = new BinaryReader(sourceStream))
                        {
                            fileInformation.AddFileSizeCompressed(
                                parsedArguments.Compress.Execute(reader, writer));
                        }

                        metaInformation.AddFileSizeUncompressed(fileInformation.FileSizeUncompressed);
                        metaInformation.IncreaseFileAmount();

                        archiveWriter.WriteAddedFileSizeCompressed(writer, fileInformationPosition, fileInformation);
                    }

                    archiveWriter.WriteChangedMetaInformation(writer, metaInformation.FilesSizeUncompressed, metaInformation.FileAmount);
                }
            }
            catch (IOException)
            {

            }
        }
    }
}