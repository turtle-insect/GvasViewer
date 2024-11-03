using GvasViewer.Gvas;

namespace GvasViewer.FileFormat
{
	internal class PlainGvas : IFileFormat
	{
		public Byte[] Load(String filename)
		{
			return System.IO.File.ReadAllBytes(filename);
		}

		public void Save(String filename, Byte[] buffer)
		{
			System.IO.File.WriteAllBytes(filename, buffer);
		}

		public uint Create(GvasStructProperty property, uint address, String name)
		{
			return 0;
		}
	}
}
