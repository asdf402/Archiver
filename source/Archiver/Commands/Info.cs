namespace Archiver
{
    public class Info : ICommand
    {
        public Result Execute(ParsedArguments parsedArguments)
        {
            MetaInformationPositions metaInformationPositions = new MetaInformationPositions();
            ArchiveReader archiveReader = new ArchiveReader();
            MetaInformation metaInformation;
            FileInformation fileInformation;

            // read from the archive
            try
            {
                FileStream source = new FileStream(parsedArguments.Source, FileMode.Open);
                using (BinaryReader binaryReader = new BinaryReader(source))
                {
                    metaInformation = archiveReader.ReadAllMetaInformation(binaryReader);

                    ConsoleNormalOutput.WriteEmptyLine();
                    ConsoleNormalOutput.WriteMetaInformation(metaInformation);
                    ConsoleNormalOutput.WriteEmptyLine();

                    binaryReader.BaseStream.Position = metaInformationPositions.EndOfMetaInformation;

                    for (uint fileCounter = 0; fileCounter < metaInformation.FileAmount; fileCounter++)
                    {
                        fileInformation = archiveReader.ReadFileInformation(binaryReader);
                        binaryReader.BaseStream.Position += fileInformation.FileSizeCompressed;

                        ConsoleNormalOutput.WriteFileInformation(fileInformation);
                        ConsoleNormalOutput.WriteEmptyLine();
                    }
                }
            }
            catch (IOException)
            {
                return new Result(true, ConsoleErrorOutput.WriteCouldNotOpenSource, "info execution");
            }

            return new Result(false, ConsoleErrorOutput.WriteNoErrorOccurred, "info execution");
        }
    }
}