namespace Archiver
{
    public class Create : ICommand
    {
        public Result Execute(ParsedArguments parsedArguments)
        {
            Append append = new Append();
            MetaInformation metaInformation = new MetaInformation(
                DateTime.UtcNow,
                parsedArguments.Compress,
                0,
                0);

            return append.AppendFiles(parsedArguments, metaInformation, FileMode.CreateNew);
        }
    }
}