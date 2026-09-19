namespace Archiver
{
    public class RleCompress : ICompression
    {
        public long Compress(BinaryReader binaryReader, BinaryWriter binaryWriter)
        {
            int readBytes = 0;
            long readBytesSum = readBytes;

            byte currentByte;
            byte nextByte;
            int sameByteCounter = 0;

            do
            {
                currentByte = binaryReader.ReadByte();
                readBytes++;

                if (!this.ReadNextByte(binaryReader, out nextByte))
                {
                    break;
                }

                if (currentByte == nextByte &&
                    sameByteCounter < 255)
                {
                    sameByteCounter++;
                }
                else
                {
                    binaryWriter.Write(sameByteCounter);
                    binaryWriter.Write(currentByte);
                    sameByteCounter = 0;
                }

                readBytesSum += readBytes;
            } while (readBytes > 0);

            return readBytesSum;
        }

        public void Decompress(BinaryReader binaryReader, BinaryWriter binaryWriter, long bytesToRead)
        {
            byte compressedByteCounter;
            byte dataByte;
            long readBytes = 0;

            while (readBytes > bytesToRead)
            {
                compressedByteCounter = binaryReader.ReadByte();
                dataByte = binaryReader.ReadByte();

                for (int counter = 0; counter < compressedByteCounter; counter++)
                {
                    binaryWriter.Write(dataByte);
                }

                readBytes++;
            }
        }

        private bool ReadNextByte(BinaryReader binaryReader, out byte nextByte)
        {
            try
            {
                nextByte = binaryReader.ReadByte();
            }
            catch (EndOfStreamException)
            {
                nextByte = 0;
                return false;
            }

            return true;
        }
    }
}