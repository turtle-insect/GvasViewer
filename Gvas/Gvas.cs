using Gvas.Property;

namespace Gvas
{
	public class Gvas
	{
		private GvasEngine _engine = new();
		private List<GvasProperty> mProperties = new();

		public IReadOnlyList<GvasProperty> Properties
		{
			get => mProperties;
		}

		public void Read(BinaryReader reader)
		{
			mProperties.Clear();

			_engine.Read(reader);
			if (_engine.PropertyTag())
			{
				Util.GvasVersion = 2;
			}

			for (; reader.BaseStream.Position < reader.BaseStream.Length;)
			{
				if (_engine.PropertyTag())
				{
					reader.ReadByte();
				}

				GvasRootProperty property = new();
				property.Name = _engine.Name;
				property.Read(reader);
				mProperties.Add(property);
			}
		}

		public void Write(BinaryWriter writer)
		{
			_engine.Write(writer);
			foreach (var property in mProperties)
			{
				if (_engine.PropertyTag())
				{
					writer.Write('\0');
				}

				property.Write(writer);
			}
		}
	}
}
