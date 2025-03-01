﻿namespace GvasViewer.FileFormat.Switch
{
	internal class RomancingSaga2 : IFileFormat
	{
		public Byte[] Load(String filename)
		{
			Byte[] buffer = System.IO.File.ReadAllBytes(filename);
			buffer = buffer[4..];
			buffer = Util.Zlib.Decompress(buffer);

			return buffer;
		}

		public void Save(String filename, Byte[] buffer)
		{
			int fileSize = buffer.Length;
			buffer = Util.Zlib.Compression(buffer);
			buffer = BitConverter.GetBytes(fileSize).Concat(buffer).ToArray();
			System.IO.File.WriteAllBytes(filename, buffer);
		}
	}
}
