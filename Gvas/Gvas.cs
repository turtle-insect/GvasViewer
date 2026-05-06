using Gvas.Property;

namespace Gvas
{
	public class Gvas
	{
		private GvasEngine mEngine = new();
		private List<GvasProperty> mProperties = new();

		public IReadOnlyList<GvasProperty> Properties
		{
			get => mProperties;
		}

		public void Read(BinaryReader reader)
		{
			mProperties.Clear();

			mEngine.Read(reader);
			if (mEngine.PropertyTag())
			{
				Util.GvasVersion = 2;
			}

			for (; reader.BaseStream.Position < reader.BaseStream.Length;)
			{
				if (mEngine.PropertyTag())
				{
					reader.ReadByte();
				}

				GvasRootProperty property = new();
				property.Name = mEngine.Name;
				property.Read(reader);
				mProperties.Add(property);
			}
		}

		public void Write(BinaryWriter writer)
		{
			mEngine.Write(writer);
			foreach (var property in mProperties)
			{
				if (mEngine.PropertyTag())
				{
					writer.Write('\0');
				}

				property.Write(writer);
			}
		}
	}
}
