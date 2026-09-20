namespace Archiver
{
    public class RleCompress : ICompression
    {
        public long Compress(BinaryReader binaryReader, BinaryWriter binaryWriter)
        {
            int compressedSize = 0;

            byte currentByte;
            byte nextByte;
            int sameByteCounter = 1;
            bool endOfFile = false;

            currentByte = binaryReader.ReadByte();

            while (!endOfFile)
            {
                if (!this.ReadNextByte(binaryReader, out nextByte))
                {
                    compressedSize += 2;
                    binaryWriter.Write(Convert.ToByte(sameByteCounter));
                    binaryWriter.Write(currentByte);

                    endOfFile = true;
                    break;
                }

                if (currentByte == nextByte &&
                    sameByteCounter < 255)
                {
                    sameByteCounter++;
                }
                else
                {
                    compressedSize += 2;
                    binaryWriter.Write(Convert.ToByte(sameByteCounter));
                    binaryWriter.Write(currentByte);

                    sameByteCounter = 1;
                    currentByte = nextByte;
                }
            }

            return compressedSize;
        }

        public void Decompress(BinaryReader binaryReader, BinaryWriter binaryWriter, long bytesToRead)
        {
            byte compressedByteCounter;
            byte dataByte;
            long readBytes = 0;

            while (readBytes < bytesToRead)
            {
                compressedByteCounter = binaryReader.ReadByte();
                dataByte = binaryReader.ReadByte();

                for (int counter = 0; counter < compressedByteCounter; counter++)
                {
                    binaryWriter.Write(dataByte);
                }

                readBytes += 2;
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