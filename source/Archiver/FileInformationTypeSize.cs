namespace Archiver
{
    public class FileInformationTypeSize
    {
        public int FileSizeCompressedTypeSize
        {
            get
            {
                return sizeof(long);
            }
        }

        public int FileSizeUncompressedTypeSize
        {
            get
            {
                return sizeof(long);
            }
        }
    }
}