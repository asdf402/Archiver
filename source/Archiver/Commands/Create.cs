namespace Archiver
{
    public class Create : ICommand
    {
        public Result Execute(ParsedArguments parsedArguments)
        {
            Append append = new Append();
            ArchiveWriter archiveWriter = new ArchiveWriter();
            MetaInformation metaInformation = new MetaInformation(
                DateTime.UtcNow,
                parsedArguments.Compress,
                0,
                0);

            try
            {
                FileStream destination = new FileStream(parsedArguments.Destination, FileMode.CreateNew);
                using (BinaryWriter binaryWriter = new BinaryWriter(destination))
                {
                    archiveWriter.WriteMetaInformation(binaryWriter, metaInformation);
                }
            }
            catch (IOException)
            {
                return new Result(true, ConsoleErrorOutput.WriteCouldNotOpenDestination, parsedArguments.Destination);
            }

            return append.AppendFiles(parsedArguments, metaInformation, new MetaInformationPositions().EndOfMetaInformation);
        }
    }
}