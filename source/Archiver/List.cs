namespace Archiver
{
    public class List : ICommand
    {
        public Result Execute(ParsedArguments parsedArguments)
        {
            ArchiveReader archiveReader = new ArchiveReader();
            ArchiveInformation archiveInformation = new ArchiveInformation();
            MetaInformationPositions metaInformationPositions = new MetaInformationPositions();
            FileInformation[] fileInformation;

            uint fileAmount;

            // read from the archive
            try
            {
                FileStream source = new FileStream(parsedArguments.Source, FileMode.Open);
                using (BinaryReader reader = new BinaryReader(source))
                {
                    reader.BaseStream.Position = metaInformationPositions.FileAmountFilePosition;
                    fileAmount = reader.ReadUInt32();
                    reader.BaseStream.Position = metaInformationPositions.EndOfMetaInformationPosition;

                    fileInformation = new FileInformation[fileAmount];
                    for (uint fileCounter = 0; fileCounter < fileAmount; fileCounter++)
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
            ConsoleNormalOutput.WriteFileNames(fileInformation);
            ConsoleNormalOutput.WriteEmptyLine();

            return new Result(false, ConsoleErrorOutput.WriteNoErrorOccurred, "list execution");
        }
    }
}