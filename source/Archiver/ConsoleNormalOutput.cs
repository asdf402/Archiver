namespace Archiver
{
    public static class ConsoleNormalOutput
    {
        public static void WriteEmptyLine()
        {
            Console.WriteLine();
        }

        public static void WriteFileInformation(FileInformation fileInformation)
        {
            Console.WriteLine($"File name: {fileInformation.FileName}");
            Console.WriteLine($"Uncompressed file size: {fileInformation.FileSizeUncompressed}");
            Console.WriteLine($"Compressed file size: {fileInformation.FileSizeCompressed}");
        }

        public static void WriteMetaInformation(MetaInformation metaInformation)
        {
            Console.WriteLine($"Archive creation date: {metaInformation.CreationDate}");
            Console.WriteLine($"Archive compression type: {metaInformation.CompressType}");
            Console.WriteLine($"Archive file amount: {metaInformation.FileAmount}");
            Console.WriteLine($"Archive uncompressed filee sizes: {metaInformation.FilesSizeUncompressed}");
        }

        public static void WriteFileNames(FileInformation[] fileInformation)
        {
            for (uint i = 0; i < fileInformation.Count(); i++)
            {
                Console.WriteLine(fileInformation[i].FileName);
            }
        }
    }
}