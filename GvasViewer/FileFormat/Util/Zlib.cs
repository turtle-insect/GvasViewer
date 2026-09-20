using System.IO;

namespace GvasViewer.FileFormat.Util
{
	internal class Zlib
	{
		public static Byte[] Compression(Byte[] buffer)
		{
			using MemoryStream input = new(buffer);
			using MemoryStream output = new();
			using System.IO.Compression.ZLibStream zlib = new(output, System.IO.Compression.CompressionLevel.Optimal);

			input.CopyTo(zlib);
			zlib.Flush();
			return output.ToArray();
		}

		public static Byte[] Decompress(Byte[] buffer)
		{
			using MemoryStream input = new(buffer);
			using System.IO.Compression.ZLibStream zlib = new(input, System.IO.Compression.CompressionMode.Decompress);
			using MemoryStream output = new();

			zlib.CopyTo(output);
			output.Flush();
			return output.ToArray();
		}
	}
}
