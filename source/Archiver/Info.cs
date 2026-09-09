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
                        reader.BaseStream.Position += fileInformation[fileCounter].FileNameSize + fileInformation[fileCounter].FileSizeCompressed;
                    }
                }
            }
            catch (IOException)
            {
                return new Result(true, ConsoleErrorOutput.WriteCouldNotOpenSource, "info execution");
            }

            ConsoleNormalOutput.WriteMetaInformation(metaInformation);
            ConsoleNormalOutput.WriteFileInformation(fileInformation);

            return new Result(false, ConsoleErrorOutput.WriteNoErrorOccurred, "info execution");
        }
    }
}