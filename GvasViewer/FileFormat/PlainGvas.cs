using Gvas;

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
			// LEGO Horizon Adventures
			if (name == "GameplayTagContainer")
			{
				uint count = (uint)SaveData.Instance().ReadNumber(address, 4);
				uint length = 4;
				for (uint i = 0; i < count; i++)
				{
					var tmp = Gvas.Gvas.GetString(address + length);
					length += tmp.length;
				}
				return length;
			}

			return 0;
		}
	}
}
