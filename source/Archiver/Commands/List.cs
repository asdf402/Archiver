namespace Archiver
{
    public class List : ICommand
    {
        public Result Execute(ParsedArguments parsedArguments)
        {
            ArchiveReader archiveReader = new ArchiveReader();
            MetaInformationPositions metaInformationPositions = new MetaInformationPositions();
            FileInformation fileInformation;

            uint fileAmount;

            // read from the archive
            try
            {
                FileStream source = new FileStream(parsedArguments.Source, FileMode.Open);
                using (BinaryReader reader = new BinaryReader(source))
                {
                    reader.BaseStream.Position = metaInformationPositions.FileAmountFilePosition;
                    fileAmount = reader.ReadUInt32();
                    reader.BaseStream.Position = metaInformationPositions.EndOfMetaInformation;

                    ConsoleNormalOutput.WriteEmptyLine();
                    for (uint fileCounter = 0; fileCounter < fileAmount; fileCounter++)
                    {
                        fileInformation = archiveReader.ReadFileInformationWithoutUncompressed(reader);
                        reader.BaseStream.Position += fileInformation.FileSizeCompressed;

                        ConsoleNormalOutput.WriteFileNames(fileInformation.FileName);
                    }

                    ConsoleNormalOutput.WriteEmptyLine();
                }
            }
            catch (IOException)
            {
                return new Result(true, ConsoleErrorOutput.WriteCouldNotOpenSource, "list execution");
            }

            return new Result(false, ConsoleErrorOutput.WriteNoErrorOccurred, "list execution");
        }
    }
}