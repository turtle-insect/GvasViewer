namespace GvasViwer.FileFormat
{
	internal class PlainGvas : Gvas.FileFormat.IFileFormat
	{
		public Byte[] Load(String filename)
		{
			return System.IO.File.ReadAllBytes(filename);
		}

		public void Save(String filename, Byte[] buffer)
		{
			System.IO.File.WriteAllBytes(filename, buffer);
		}

		public uint Create(Gvas.Property.GvasStructProperty property, uint address, String name)
		{
			return 0;
		}
	}
}
