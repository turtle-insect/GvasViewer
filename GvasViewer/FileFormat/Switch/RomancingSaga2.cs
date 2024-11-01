using GvasViewer.Gvas;
using GvasViewer.Util;

namespace GvasViewer.FileFormat.Switch
{
	internal class RomancingSaga2 : IFileFormat
	{
		public Byte[] Load(String filename)
		{
			Byte[] tmp = System.IO.File.ReadAllBytes(filename);
			Byte[] comp = new Byte[tmp.Length - 4];
			Array.Copy(tmp, 4, comp, 0, tmp.Length - 4);
			Byte[] result = Zlib.Decompress(comp);

			return result;
		}

		public void Save(String filename, Byte[] buffer)
		{
			Byte[] comp = Zlib.Compression(buffer);
			Byte[] tmp = new Byte[comp.Length + 4];
			Byte[] size = BitConverter.GetBytes(buffer.Length);
			Array.Copy(size, 0, tmp, 0, size.Length);
			Array.Copy(comp, 0, tmp, 4, comp.Length);
			System.IO.File.WriteAllBytes(filename, tmp);
		}

		public (GvasStructProperty property, uint length) Create(uint address, string name)
		{
			return (new GvasStructProperty(), 0);
		}
	}
}
