namespace Archiver
{
    public class NoCompress : ICompression
    {
        /// <summary>
        /// Does not do any compression. Writes the data from reader to the writer FileStream.
        /// </summary>
        /// <param name="binaryReader">The FileStream from where the data will be read.</param>
        /// <param name="binaryWriter">The FileStream to where the data will be written.</param>
        /// <returns>Returns the not compressed size of the file.</returns>
        public long Compress(BinaryReader binaryReader, BinaryWriter binaryWriter)
        {
            int readBytes = 0;
            long readBytesSum = readBytes;
            int byteArraySize = 81920;
            byte[] fileData = new byte[byteArraySize];

            do
            {
                readBytes = binaryReader.Read(fileData, 0, byteArraySize);
                binaryWriter.Write(fileData, 0, readBytes);

                readBytesSum += readBytes;
            } while (readBytes > 0);

            return readBytesSum;
        }

        public void Decompress(BinaryReader binaryReader, BinaryWriter binaryWriter, long bytesToRead)
        {
            int readBytes = 0;
            int byteArraySize = 81920;
            byte[] fileData = new byte[byteArraySize];

            while (bytesToRead != 0)
            {
                if (bytesToRead < byteArraySize)
                {
                    byteArraySize = Convert.ToInt32(bytesToRead);
                }

                readBytes = binaryReader.Read(fileData, 0, byteArraySize);
                binaryWriter.Write(fileData, 0, readBytes);

                bytesToRead -= byteArraySize;
            }
        }
    }
}
