namespace Archiver
{
    public interface ICompression
    {
        long Compress(BinaryReader binaryReader, BinaryWriter binaryWriter);

        void Decompress(BinaryReader binaryReader, BinaryWriter binaryWriter, long bytesToRead);
    }
}