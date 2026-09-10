namespace Archiver
{
    public class MetaInformationPositions
    {
        public uint CompressTypeFilePosition
        {
            get
            {
                return 7;
            }
        }

        public long CreationDateFilePosition
        {
            get
            {
                return 11;
            }
        }

        public long EndOfMetaInformationPosition
        {
            get
            {
                return 31;
            }
        }

        public uint FileAmountFilePosition
        {
            get
            {
                return 19;
            }
        }

        public long FilesSizeUncompressedFilePosition
        {
            get
            {
                return 23;
            }
        }
    }
}