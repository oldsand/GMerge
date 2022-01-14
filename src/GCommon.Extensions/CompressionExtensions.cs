using System.IO;
using System.IO.Compression;

namespace GCommon.Extensions
{
    public static class CompressionExtensions
    {
        public static byte[] Compress(this byte[] input)
        {
            var inputStream = new MemoryStream(input);
            using var compressedStream = new MemoryStream();
            using var compressor = new DeflateStream(compressedStream, CompressionMode.Compress);
            inputStream.CopyTo(compressor);
            compressor.Close();
            return compressedStream.ToArray();
        }

        public static byte[] Decompress(this byte[] input)
        {
            using var outputStream = new MemoryStream();
            using var compressedStream = new MemoryStream(input);
            using var compressor = new DeflateStream(compressedStream, CompressionMode.Decompress);
            compressor.CopyTo(outputStream);
            compressor.Close();
            return outputStream.ToArray();
        }
    }
}