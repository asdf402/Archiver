namespace Archiver
{
    public interface ICompress
    {
        long Execute(BinaryReader reader, BinaryWriter writer);
    }
}