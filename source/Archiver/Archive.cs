namespace Archiver
{
    public class Archive
    {
        public Archive(MetaInformation metaInformation, FileInformation fileInformation)
        {
            this.MetaInformation = metaInformation;
            this.FileInformation = fileInformation;
        }

        public MetaInformation MetaInformation
        {
            get;
            private set;
        }

        public FileInformation FileInformation
        {
            get;
            private set;
        }
    }
}