namespace Archiver
{
    public class NoCompress : ICompress
    {
        /// <summary>
        /// Does not do any compression. Writes the data from reader to the writer FileStream.
        /// </summary>
        /// <param name="reader">The FileStream from where the data will be read.</param>
        /// <param name="writer">The FileStream to where the data will be written.</param>
        /// <returns>Returns the not compressed size of the file.</returns>
        public long Execute(BinaryReader reader, BinaryWriter writer)
        {
            int readBytes = 0;
            long readBytesSum = readBytes;
            int byteArraySize = 81920;
            byte[] fileData = new byte[byteArraySize];

            do
            {
                readBytes = reader.Read(fileData, 0, byteArraySize);
                writer.Write(fileData, 0, readBytes);

                readBytesSum += readBytes;
            } while (readBytes > 0);

            return readBytesSum;
        }
    }
}