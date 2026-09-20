namespace Archiver.Tests
{
    using System.Text;
    using Xunit;

    public class RleCompressTests
    {
        [Fact]
        public void Compress_CompressesSingleByte()
        {
            byte[] input =
            {
                0x41,
            };

            byte[] result = this.Compress(input);

            byte[] expected =
            {
                0x01,
                0x41,
            };

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Compress_CompressesRepeatedBytes()
        {
            byte[] input =
            {
                0x41,
                0x41,
                0x41,
                0x41,
                0x41,
            };

            byte[] result = this.Compress(input);

            byte[] expected =
            {
                0x05,
                0x41,
            };

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Compress_CompressesDifferentBytesSeparately()
        {
            byte[] input =
            {
                0x41,
                0x42,
                0x43,
            };

            byte[] result = this.Compress(input);

            byte[] expected =
            {
                0x01,
                0x41,
                0x01,
                0x42,
                0x01,
                0x43,
            };

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Compress_CompressesMixedByteSequence()
        {
            byte[] input =
            {
                0x41,
                0x41,
                0x42,
                0x43,
                0x43,
                0x43,
            };

            byte[] result = this.Compress(input);

            byte[] expected =
            {
                0x02,
                0x41,
                0x01,
                0x42,
                0x03,
                0x43,
            };

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Compress_SplitsSequence_WhenMoreThan255BytesRepeat()
        {
            byte[] input = Enumerable
                .Repeat((byte)0x20, 260)
                .ToArray();

            byte[] result = this.Compress(input);

            byte[] expected =
            {
                0xFF,
                0x20,
                0x05,
                0x20,
            };

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Compress_DoesNotSplitSequence_WhenExactly255BytesRepeat()
        {
            byte[] input = Enumerable
                .Repeat((byte)0x20, 255)
                .ToArray();

            byte[] result = this.Compress(input);

            byte[] expected =
            {
                0xFF,
                0x20,
            };

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Compress_ReturnsAmountOfWrittenBytes()
        {
            byte[] input =
            {
                0x41,
                0x41,
                0x41,
                0x42,
            };

            long writtenBytes;

            using MemoryStream inputStream = new MemoryStream(input);
            using BinaryReader reader = new BinaryReader(inputStream);

            using MemoryStream outputStream = new MemoryStream();
            using BinaryWriter writer = new BinaryWriter(outputStream);

            RleCompress compression = new RleCompress();

            writtenBytes = compression.Compress(reader, writer);

            writer.Flush();

            Assert.Equal(outputStream.Length, writtenBytes);
        }

        [Fact]
        public void Decompress_DecompressesSingleSequence()
        {
            byte[] compressed =
            {
                0x05,
                0x41,
            };

            byte[] result = this.Decompress(
                compressed,
                compressed.Length);

            byte[] expected =
            {
                0x41,
                0x41,
                0x41,
                0x41,
                0x41,
            };

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Decompress_DecompressesMultipleSequences()
        {
            byte[] compressed =
            {
                0x02,
                0x41,
                0x01,
                0x42,
                0x03,
                0x43,
            };

            byte[] result = this.Decompress(
                compressed,
                compressed.Length);

            byte[] expected =
            {
                0x41,
                0x41,
                0x42,
                0x43,
                0x43,
                0x43,
            };

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Decompress_DecompressesSplitSequence()
        {
            byte[] compressed =
            {
                0xFF,
                0x20,
                0x05,
                0x20,
            };

            byte[] result = this.Decompress(
                compressed,
                compressed.Length);

            byte[] expected = Enumerable
                .Repeat((byte)0x20, 260)
                .ToArray();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void CompressAndDecompress_ReturnsOriginalData()
        {
            byte[] original =
            {
                0x41,
                0x41,
                0x41,
                0x42,
                0x43,
                0x43,
                0x44,
                0x44,
                0x44,
                0x44,
            };

            byte[] compressed = this.Compress(original);

            byte[] decompressed = this.Decompress(
                compressed,
                compressed.Length);

            Assert.Equal(original, decompressed);
        }

        private byte[] Compress(byte[] input)
        {
            using MemoryStream inputStream = new MemoryStream(input);
            using BinaryReader reader = new BinaryReader(inputStream);

            using MemoryStream outputStream = new MemoryStream();
            using BinaryWriter writer = new BinaryWriter(outputStream);

            RleCompress compression = new RleCompress();

            compression.Compress(reader, writer);
            writer.Flush();

            return outputStream.ToArray();
        }

        private byte[] Decompress(
            byte[] input,
            long bytesToRead)
        {
            using MemoryStream inputStream = new MemoryStream(input);
            using BinaryReader reader = new BinaryReader(inputStream);

            using MemoryStream outputStream = new MemoryStream();
            using BinaryWriter writer = new BinaryWriter(outputStream);

            RleCompress compression = new RleCompress();

            compression.Decompress(
                reader,
                writer,
                bytesToRead);

            writer.Flush();

            return outputStream.ToArray();
        }
    }
}