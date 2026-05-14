namespace Gvas.Property.v1.Standard
{
	public class GvasByteProperty : GvasProperty
	{
		private Byte[] _value = [];
		private GvasString _propertyName = new();

		public GvasByteProperty()
			: base()
		{ }

		public GvasByteProperty(GvasByteProperty property)
			: base(property)
		{
			_propertyName = property._propertyName;
			_value = property._value.ToArray();
		}

		public override GvasProperty Clone()
		{
			return new GvasByteProperty(this);
		}

		public override object Value
		{
			get => _value;
			set
			{
				Byte[]? tmp = value as Byte[];
				if (tmp == null) return;

				_value = tmp;
			}
		}

		public override void Read(BinaryReader reader)
		{
			var size = reader.ReadUInt64();

			_propertyName.Read(reader);

			// ???
			reader.ReadByte();

			_value = reader.ReadBytes((int)size);
		}

		public override void Write(BinaryWriter writer)
		{
			Name.Write(writer);
			Util.WriteString(writer, "ByteProperty");
			writer.Write(_value.LongLength);
			_propertyName.Write(writer);
			writer.Write('\0');
			writer.Write(_value);
		}

		public override void ReadValue(BinaryReader reader)
		{
			_value = reader.ReadBytes(1);
		}

		public override void WriteValue(BinaryWriter writer)
		{
			writer.Write(_value);
		}
	}
}
