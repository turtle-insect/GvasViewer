using GvasViewer.Gvas;
using GvasViewer.Util;

namespace GvasViewer.FileFormat.Switch
{
	internal class RomancingSaga2 : IFileFormat
	{
		public Byte[] Load(String filename)
		{
			Byte[] buffer = System.IO.File.ReadAllBytes(filename);
			buffer = buffer.Skip(4).ToArray();
			buffer = Zlib.Decompress(buffer);

			return buffer;
		}

		public void Save(String filename, Byte[] buffer)
		{
			int fileSize = buffer.Length;
			buffer = Zlib.Compression(buffer);
			buffer = BitConverter.GetBytes(fileSize).Concat(buffer).ToArray();
			System.IO.File.WriteAllBytes(filename, buffer);
		}

		public uint Create(GvasStructProperty property, uint address, String name)
		{
			return 0;
		}
	}
}
