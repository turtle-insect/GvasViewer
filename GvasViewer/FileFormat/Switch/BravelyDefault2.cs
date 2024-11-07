using GvasViewer.Gvas.Property;
using GvasViewer.Util;
using System.Text;

namespace GvasViewer.FileFormat.Switch
{
    internal class BravelyDefault2 : IFileFormat
	{
		public byte[] Load(string filename)
		{
			Byte[] buffer = System.IO.File.ReadAllBytes(filename);
			buffer = buffer[12..^0];
			buffer = Zlib.Decompress(buffer);
			return buffer;
		}

		public void Save(string filename, byte[] buffer)
		{
			Byte[] header = Encoding.UTF8.GetBytes("SAVE");
			header = header.Concat(BitConverter.GetBytes(1)).ToArray();
			header = header.Concat(BitConverter.GetBytes(buffer.Length)).ToArray();

			buffer = Zlib.Compression(buffer);

			buffer = header.Concat(buffer).ToArray();
			System.IO.File.WriteAllBytes(filename, buffer);
		}

		public uint Create(GvasStructProperty property, uint address, String name)
		{
			switch (name)
			{
				case "Rotator":
					return 12;
			}
			
			return 0;
		}
	}
}
