namespace Archiver
{
    public class MetaInformation
    {
        public MetaInformation(DateTime creationDate, ICompress compress, uint fileAmount, long filesSizeUncompressed)
        {
            this.CreationDate = creationDate;
            this.Compress = compress;
            this.FileAmount = fileAmount;
            this.FilesSizeUncompressed = filesSizeUncompressed;

            switch (compress)
            {
                case NoCompress:
                    this.CompressType = 0;
                    break;

                case RleCompress:
                    this.CompressType = 1;
                    break;

                default:
                    throw new ArgumentException("compress type does not exist here");
            }
        }

        public MetaInformation(DateTime creationDate, uint compressType, uint fileAmount, long filesSizeUncompressed)
        {
            this.CreationDate = creationDate;
            this.CompressType = compressType;
            this.FileAmount = fileAmount;
            this.FilesSizeUncompressed = filesSizeUncompressed;

            switch (compressType)
            {
                case 0:
                    this.Compress = new NoCompress();
                    break;

                case 1:
                    this.Compress = new RleCompress();
                    break;

                default:
                    throw new ArgumentException("compress type does not exist here");
            }
        }

        public DateTime CreationDate
        {
            get;
            private set;
        }

        public ICompress Compress
        {
            get;
            private set;
        }

        public uint CompressType
        {
            get;
            private set;
        }

        public uint FileAmount
        {
            get;
            private set;
        }

        public long FilesSizeUncompressed
        {
            get;
            private set;
        }

        public void IncreaseFileAmount()
        {
            this.FileAmount++;
        }

        public void AddFileAmount(uint addedFileAmount)
        {
            this.FileAmount += addedFileAmount;
        }

        public void AddFileSizeUncompressed(long addedFileSizeUncompressed)
        {
            this.FilesSizeUncompressed += addedFileSizeUncompressed;
        }
    }
}