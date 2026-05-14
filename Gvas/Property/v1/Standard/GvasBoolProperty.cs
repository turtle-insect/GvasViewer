namespace Gvas.Property.v1.Standard
{
	public class GvasBoolProperty : GvasProperty
	{
		private Boolean _value;

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
				Boolean tmp;
				Boolean.TryParse(value.ToString(), out tmp);
				_value = tmp;
			}
		}

		public override void Read(BinaryReader reader)
		{
			reader.ReadUInt64();

			ReadValue(reader);

			// ???
			reader.ReadByte();
		}

		public override void Write(BinaryWriter writer)
		{
			Name.Write(writer);
			Util.WriteString(writer, "BoolProperty");
			writer.Write((Int64)0);
			writer.Write(_value);
			writer.Write('\0');
		}

		public override void ReadValue(BinaryReader reader)
		{
			_value = reader.ReadBoolean();
		}

		public override void WriteValue(BinaryWriter writer)
		{
			writer.Write(_value);
		}
	}
}
