namespace Archiver
{
    public class MetaInformationPositions
    {
        public int ArchiveHeaderPosition
        {
            get
            {
                return 0;
            }
        }

        public int StartOfMetaInformation
        {
            get
            {
                return 7;
            }
        }

        public int CompressTypeFilePosition
        {
            get
            {
                return 7;
            }
        }

        public int CreationDateFilePosition
        {
            get
            {
                return 11;
            }
        }

        public int FileAmountFilePosition
        {
            get
            {
                return 19;
            }
        }

        public int FilesSizeUncompressedFilePosition
        {
            get
            {
                return 23;
            }
        }

        public int EndOfMetaInformation
        {
            get
            {
                return 31;
            }
        }
    }
}