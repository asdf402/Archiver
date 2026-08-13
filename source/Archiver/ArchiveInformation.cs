namespace Archiver
{
    using System.Text;

    public class ArchiveInformation
    {
        private readonly string archiveIdentifier = "ARCHIVE";

        public byte[] ArchiveIdentifier
        {
            get
            {
                return Encoding.UTF8.GetBytes(this.archiveIdentifier);
            }
        }

        public long ArchiveIdentifierLength
        {
            get
            {
                return Encoding.UTF8.GetByteCount(this.archiveIdentifier);
            }
        }
    }
}