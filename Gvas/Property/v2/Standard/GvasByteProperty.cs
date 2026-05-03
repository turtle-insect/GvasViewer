namespace Gvas.Property.v2.Standard
{
	public class GvasByteProperty : GvasProperty
	{
		private Byte[] mValue = [];

		public GvasByteProperty()
			: base()
		{ }

		public GvasByteProperty(GvasByteProperty property)
			: base(property)
		{
			mPropertyName = property.mPropertyName;
			mValue = property.mValue.ToArray();
		}

		public override GvasProperty Clone()
		{
			return new GvasByteProperty(this);
		}

		public override object Value
		{
			get => mValue;
			set
			{
				Byte[]? tmp = value as Byte[];
				if (tmp == null) return;

				mValue = tmp;
			}
		}

		private GvasString mPropertyName = new();


		public override void Read(BinaryReader reader)
		{
			var size = reader.ReadUInt64();

			mPropertyName.Read(reader);

			// ???
			reader.ReadByte();

			mValue = reader.ReadBytes((int)size);
		}

		public override void Write(BinaryWriter writer)
		{
			Name.Write(writer);
			Util.WriteString(writer, "ByteProperty");
			writer.Write(mValue.LongLength);
			mPropertyName.Write(writer);
			writer.Write('\0');
			writer.Write(mValue);
		}

		public override void ReadValue(BinaryReader reader)
		{
			throw new NotImplementedException();
		}

		public override void WriteValue(BinaryWriter writer)
		{
			writer.Write(mValue);
		}
	}
}
