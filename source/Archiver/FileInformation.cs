namespace Archiver
{
    using System.Text;

    public class FileInformation
    {
        public FileInformation(string fileName, long fileSizeUncompressed, long fileSizeCompressed)
        {
            this.FileName = fileName;
            this.FileSizeUncompressed = fileSizeUncompressed;
            this.FileSizeCompressed = fileSizeCompressed;
        }

        public string FileName
        {
            get;
            private set;
        }

        public long FileSizeUncompressed
        {
            get;
            private set;
        }

        public long FileSizeCompressed
        {
            get;
            private set;
        }

        public int FileNameSize
        {
            get
            {
                return Encoding.UTF8.GetByteCount(this.FileName);
            }
        }

        public void AddFileSizeCompressed(long fileSizeCompressedToAdd)
        {
            this.FileSizeCompressed = fileSizeCompressedToAdd;
        }
    }
}