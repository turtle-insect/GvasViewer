namespace Gvas.Property.v2.Standard
{
	public class GvasByteProperty : GvasProperty
	{
		private Byte[] _value = [];

		public GvasByteProperty()
			: base()
		{ }

		public GvasByteProperty(GvasByteProperty property)
			: base(property)
		{
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
			reader.ReadUInt32();
			var size = reader.ReadUInt32();

			// ???
			reader.ReadByte();

			_value = reader.ReadBytes((int)size);
		}

		public override void Write(BinaryWriter writer)
		{
			Name.Write(writer);
			Util.WriteString(writer, "ByteProperty");
			writer.Write(0);
			writer.Write(_value.Length);
			writer.Write('\0');
			writer.Write(_value);
		}

		public override void ReadValue(BinaryReader reader)
		{
			throw new NotImplementedException();
		}

		public override void WriteValue(BinaryWriter writer)
		{
			writer.Write(_value);
		}
	}
}
