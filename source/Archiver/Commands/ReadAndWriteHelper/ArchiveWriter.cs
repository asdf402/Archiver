namespace Archiver
{
    public class ArchiveWriter
    {
        public void WriteChangedMetaInformation(BinaryWriter writer, long filesSizeUncompressedSum, uint fileAmount)
        {
            long currentPosition = writer.BaseStream.Position;

            writer.BaseStream.Position = 19;
            writer.Write(fileAmount);
            writer.Write(filesSizeUncompressedSum);

            writer.BaseStream.Position = currentPosition;
        }

        public void WriteAddedFileSizeCompressed(BinaryWriter writer, long fileInformationPosition, FileInformation fileInformation)
        {
            long currentPosition = writer.BaseStream.Position;

            writer.BaseStream.Position = fileInformationPosition;
            writer.Write(fileInformation.FileSizeCompressed);

            writer.BaseStream.Position = currentPosition;
        }

        public void WriteFileInformation(BinaryWriter writer, FileInformation fileInformation)
        {
            writer.Write(fileInformation.FileSizeCompressed);
            writer.Write(fileInformation.FileSizeUncompressed);
            writer.Write(fileInformation.FileNameSize);
            writer.Write(fileInformation.FileName);
        }

        public void WriteArchiveHeader(BinaryWriter writer, byte[] archiveIdentifier)
        {
            long currentPosition = writer.BaseStream.Position;

            writer.BaseStream.Position = 0;
            writer.Write(archiveIdentifier);

            writer.BaseStream.Position = currentPosition;
        }

        public void WriteMetaInformation(BinaryWriter writer, MetaInformation metaInformation)
        {
            long currentPosition = writer.BaseStream.Position;

            writer.BaseStream.Position = 7;
            writer.Write(metaInformation.CompressType);
            writer.Write(metaInformation.CreationDate.Ticks);
            writer.Write(metaInformation.FileAmount);
            writer.Write(metaInformation.FilesSizeUncompressed);

            writer.BaseStream.Position = currentPosition;
        }
    }
}