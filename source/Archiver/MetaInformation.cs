namespace Archiver
{
    public class MetaInformation
    {
        public MetaInformation(DateTime creationDate, bool rleCompressed, uint fileAmount, long filesSizeUncompressed)
        {
            this.CreationDate = creationDate;
            this.RleCompressed = rleCompressed;
            this.FileAmount = fileAmount;
            this.FilesSizeUncompressed = filesSizeUncompressed;
        }

        public DateTime CreationDate
        {
            get;
            private set;
        }

        public bool RleCompressed
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
    }
}