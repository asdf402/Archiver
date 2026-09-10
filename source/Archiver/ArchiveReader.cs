namespace Archiver
{
    using System.Text;

    public class ArchiveReader
    {
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

        public MetaInformation ReadMetaInformation(BinaryReader reader)
        {
            reader.BaseStream.Position = 7;

            uint compressType = reader.ReadUInt32();
            long creationDateTicks = reader.ReadInt64();
            uint fileAmount = reader.ReadUInt32();
            long fileSizeUncompressed = reader.ReadInt64();

            return new MetaInformation(
                new DateTime(creationDateTicks),
                compressType,
                fileAmount,
                fileSizeUncompressed);
        }
    }
}