namespace GvasViewer.FileFormat.Switch
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
			Byte[] header = System.Text.Encoding.UTF8.GetBytes("SAVE");
			header = header.Concat(BitConverter.GetBytes(1)).ToArray();
			header = header.Concat(BitConverter.GetBytes(buffer.Length)).ToArray();

			buffer = Util.Zlib.Compression(buffer);

			buffer = header.Concat(buffer).ToArray();
			System.IO.File.WriteAllBytes(filename, buffer);
		}
	}
}
