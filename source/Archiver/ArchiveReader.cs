namespace Archiver
{
    using System.Text;

    public class ArchiveReader
    {
        private MetaInformationPositions metaInformationPositions;

        public ArchiveReader()
        {
            this.metaInformationPositions = new MetaInformationPositions();
        }

        public FileInformation ReadFileInformation(BinaryReader reader)
        {
            long fileSizeCompressed = reader.ReadInt64();
            long fileSizeUncompressed = reader.ReadInt64();
            int fileNameLength = reader.ReadInt32();

            byte[] fileNameByte = reader.ReadBytes(fileNameLength);
            string fileName = Encoding.UTF8.GetString(fileNameByte);

            return new FileInformation(
                fileName,
                fileSizeUncompressed,
                fileSizeCompressed);
        }

        public FileInformation ReadFileInformationWithoutUncompressed(BinaryReader reader)
        {
            FileInformationTypeSize fileInformationTypeSize = new FileInformationTypeSize();

            long fileSizeCompressed = reader.ReadInt64();
            reader.BaseStream.Position += fileInformationTypeSize.FileSizeUncompressedTypeSize;
            int fileNameLength = reader.ReadInt32();

            byte[] fileNameByte = reader.ReadBytes(fileNameLength);
            string fileName = Encoding.UTF8.GetString(fileNameByte);

            return new FileInformation(
                fileName,
                0,
                fileSizeCompressed);
        }

        public MetaInformation ReadAllMetaInformation(BinaryReader reader)
        {
            return new MetaInformation(
                new DateTime(this.ReadCreationDateTicks(reader)),
                this.ReadCompressType(reader),
                this.ReadFileAmount(reader),
                this.ReadFileSizeUncompressed(reader));
        }

        public string ReadArchiveHeader(BinaryReader reader)
        {
            ArchiveInformation archiveInformation = new ArchiveInformation();

            long oldPosition = reader.BaseStream.Position;

            reader.BaseStream.Position = this.metaInformationPositions.ArchiveHeaderPosition;

            byte[] archiveHeaderBytes = reader.ReadBytes(archiveInformation.ArchiveIdentifierLength);
            string archiveHeader = Encoding.UTF8.GetString(archiveHeaderBytes);

            reader.BaseStream.Position = oldPosition;

            return archiveHeader;
        }

        public uint ReadCompressType(BinaryReader reader)
        {
            long oldPosition = reader.BaseStream.Position;

            reader.BaseStream.Position = this.metaInformationPositions.CompressTypeFilePosition;
            uint compressType = reader.ReadUInt32();

            reader.BaseStream.Position = oldPosition;

            return compressType;
        }

        public long ReadCreationDateTicks(BinaryReader reader)
        {
            long oldPosition = reader.BaseStream.Position;

            reader.BaseStream.Position = this.metaInformationPositions.CreationDateFilePosition;
            long creationDateTicks = reader.ReadInt64();

            reader.BaseStream.Position = oldPosition;

            return creationDateTicks;
        }

        public uint ReadFileAmount(BinaryReader reader)
        {
            long oldPosition = reader.BaseStream.Position;

            reader.BaseStream.Position = this.metaInformationPositions.FileAmountFilePosition;
            uint fileAmount = reader.ReadUInt32();

            reader.BaseStream.Position = oldPosition;

            return fileAmount;
        }

        public long ReadFileSizeUncompressed(BinaryReader reader)
        {
            long oldPosition = reader.BaseStream.Position;

            reader.BaseStream.Position = this.metaInformationPositions.FilesSizeUncompressedFilePosition;
            long fileSizeUncompressed = reader.ReadInt64();

            reader.BaseStream.Position = oldPosition;

            return fileSizeUncompressed;
        }
    }
}