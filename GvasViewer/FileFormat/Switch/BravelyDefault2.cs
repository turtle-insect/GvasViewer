using Gvas.Property;

namespace GvasViwer.FileFormat.Switch
{
	internal class BravelyDefault2 : Gvas.FileFormat.IFileFormat
	{
		public byte[] Load(string filename)
		{
			Byte[] buffer = System.IO.File.ReadAllBytes(filename);
			buffer = buffer[12..];
			buffer = Util.Zlib.Decompress(buffer);
			return buffer;
		}

		public void Save(string filename, byte[] buffer)
		{
			Byte[] header = System.Text.Encoding.UTF8.GetBytes("SAVE");
			header = header.Concat(BitConverter.GetBytes(1)).ToArray();
			header = header.Concat(BitConverter.GetBytes(buffer.Length)).ToArray();

			buffer = Util.Zlib.Compression(buffer);

			buffer = header.Concat(buffer).ToArray();
			System.IO.File.WriteAllBytes(filename, buffer);
		}

		public uint Create(GvasStructProperty property, uint address, String name)
		{
			return 0;
		}
	}
}
