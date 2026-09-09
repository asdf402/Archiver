namespace Archiver
{
    public class Info : ICommand
    {
        public Result Execute(ParsedArguments parsedArguments)
        {
            ArchiveReader archiveReader = new ArchiveReader();
            ArchiveInformation archiveInformation = new ArchiveInformation();
            MetaInformation metaInformation;
            FileInformation[] fileInformation;

            // read from the archive
            try
            {
                FileStream source = new FileStream(parsedArguments.Source, FileMode.Open);
                using (BinaryReader reader = new BinaryReader(source))
                {
                    metaInformation = archiveReader.ReadMetaInformation(reader);

                    fileInformation = new FileInformation[metaInformation.FileAmount];
                    for (uint fileCounter = 0; fileCounter < metaInformation.FileAmount; fileCounter++)
                    {
                        fileInformation[fileCounter] = archiveReader.ReadFileInformation(reader);
                        reader.BaseStream.Position += fileInformation[fileCounter].FileSizeCompressed;
                    }
                }
            }
            catch (IOException)
            {
                return new Result(true, ConsoleErrorOutput.WriteCouldNotOpenSource, "info execution");
            }

            // write the infos to the console
            // (is not done with the reading part to have as little instructions in the file stream as possible)
            ConsoleNormalOutput.WriteEmptyLine();
            ConsoleNormalOutput.WriteMetaInformation(metaInformation);
            ConsoleNormalOutput.WriteEmptyLine();

            for (int fileCounter = 0; fileCounter < metaInformation.FileAmount; fileCounter++)
            {
                ConsoleNormalOutput.WriteFileInformation(fileInformation[fileCounter]);
                ConsoleNormalOutput.WriteEmptyLine();
            }

            ConsoleNormalOutput.WriteEmptyLine();

            return new Result(false, ConsoleErrorOutput.WriteNoErrorOccurred, "info execution");
        }
    }
}