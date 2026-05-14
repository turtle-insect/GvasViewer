namespace Gvas.Property.v2.Standard
{
	public class GvasBoolProperty : GvasProperty
	{
		private Byte _value;

		public GvasBoolProperty()
			: base()
		{ }

		public GvasBoolProperty(GvasBoolProperty property)
			: base(property)
		{
			_value = property._value;
		}

		public override GvasProperty Clone()
		{
			return new GvasBoolProperty(this);
		}

		public override object Value
		{
			get => _value;
			set
			{
				Byte tmp;
				Byte.TryParse(value.ToString(), out tmp);
				_value = tmp;
			}
		}

		public override void Read(BinaryReader reader)
		{
			reader.ReadUInt64();

			ReadValue(reader);
		}

		public override void Write(BinaryWriter writer)
		{
			Name.Write(writer);
			Util.WriteString(writer, "BoolProperty");
			writer.Write((Int64)0);
			writer.Write(_value);
		}

		public override void ReadValue(BinaryReader reader)
		{
			_value = reader.ReadByte();
		}

		public override void WriteValue(BinaryWriter writer)
		{
			writer.Write(_value);
		}
	}
}
