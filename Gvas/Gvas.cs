using Gvas.Property;

namespace Gvas
{
	public class Gvas
	{
		private GvasEngine _engine = new();
		private List<GvasProperty> _properties = new();

		public IReadOnlyList<GvasProperty> Properties
		{
			get => _properties;
		}

		public void Read(BinaryReader reader)
		{
			_properties.Clear();

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
				_properties.Add(property);
			}
		}

		public void Write(BinaryWriter writer)
		{
			_engine.Write(writer);
			foreach (var property in _properties)
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
