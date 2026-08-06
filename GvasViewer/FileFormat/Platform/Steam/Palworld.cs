using GvasViewer.FileFormat.Util;

namespace GvasViewer.FileFormat.Platform.Steam
{
	internal class Palworld : IFileFormat
	{
		public byte[] Load(string filename)
		{
			Byte[] buffer = System.IO.File.ReadAllBytes(filename);
			var magic = System.Text.Encoding.UTF8.GetString(buffer, 8, 4);
			if (magic != "PlM1") return [];

			var length = BitConverter.ToInt32(buffer);
			buffer = buffer[12..];

			Oodle oodle = new();
			return oodle.Decompress(buffer, length);
		}

		public void Save(string filename, byte[] buffer)
		{
			var length = buffer.Length;

			Oodle oodle = new();
			buffer = oodle.Compress(buffer);
			
			buffer = [
				.. BitConverter.GetBytes(length),
				.. BitConverter.GetBytes(buffer.Length),
				.. System.Text.Encoding.UTF8.GetBytes("PlM1"),
				.. buffer,
			];

			System.IO.File.WriteAllBytes(filename, buffer);
		}
	}
}
