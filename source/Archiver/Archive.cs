namespace Archiver
{
    public class Archive
    {
        public Archive(MetaInformation metaInformation, List<FileInformation> fileInformation)
        {
            this.MetaInformation = metaInformation;
            this.FileInformation = fileInformation;
        }

        public MetaInformation MetaInformation
        {
            get;
            private set;
        }

        public List<FileInformation> FileInformation
        {
            get;
            private set;
        }
    }
}