namespace Archiver
{
    public class List : ICommand
    {
        public Result Execute(ParsedArguments parsedArguments)
        {
            ArchiveReader archiveReader = new ArchiveReader();
            ArchiveInformation archiveInformation = new ArchiveInformation();
            MetaInformation metaInformation = new MetaInformation(new DateTime(), new NoCompress(), 0, 0);
            FileInformation[] fileInformation;

            // read from the archive
            try
            {
                FileStream source = new FileStream(parsedArguments.Source, FileMode.Open);
                using (BinaryReader reader = new BinaryReader(source))
                {
                    reader.BaseStream.Position = metaInformation.FileAmountFilePosition;
                    metaInformation.AddFileAmount(reader.ReadUInt32());
                    reader.BaseStream.Position = metaInformation.EndOfMetaInformationPosition;

                    fileInformation = new FileInformation[metaInformation.FileAmount];
                    for (uint fileCounter = 0; fileCounter < metaInformation.FileAmount; fileCounter++)
                    {
                        fileInformation[fileCounter] = archiveReader.ReadFileInformationWithoutUncompressed(reader);
                        reader.BaseStream.Position += fileInformation[fileCounter].FileSizeCompressed;
                    }
                }
            }
            catch (IOException)
            {
                return new Result(true, ConsoleErrorOutput.WriteCouldNotOpenSource, "list execution");
            }

            // write the infos to the console
            // (is not done with the reading part to have as little instructions in the file stream as possible)
            ConsoleNormalOutput.WriteEmptyLine();
            for (int fileCounter = 0; fileCounter < metaInformation.FileAmount; fileCounter++)
            {
                ConsoleNormalOutput.WriteFileInformation(fileInformation[fileCounter]);
            }

            ConsoleNormalOutput.WriteEmptyLine();

            return new Result(false, ConsoleErrorOutput.WriteNoErrorOccurred, "list execution");
        }
    }
}