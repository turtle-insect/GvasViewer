namespace GvasViewer.FileFormat.Platform.Switch
{
	internal class BravelyDefault2 : IFileFormat
	{
		public Byte[] Load(String filename)
		{
			Byte[] buffer = System.IO.File.ReadAllBytes(filename);
			buffer = buffer[12..];
			buffer = Util.Zlib.Decompress(buffer);
			return buffer;
		}

		public void Save(String filename, Byte[] buffer)
		{
			var length = buffer.Length;

			buffer = Util.Zlib.Compression(buffer);

			buffer = [
				.. System.Text.Encoding.UTF8.GetBytes("SAVE"),
				.. BitConverter.GetBytes(1),
				.. BitConverter.GetBytes(length),
				.. buffer
			];

			System.IO.File.WriteAllBytes(filename, buffer);
		}
	}
}
