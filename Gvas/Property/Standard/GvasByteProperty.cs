namespace Gvas.Property.Standard
{
	public class GvasByteProperty : GvasProperty
	{
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

		private Byte[] mValue = [];
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

		private String mPropertyName = String.Empty;


		public override void Read(BinaryReader reader)
		{
			var size = reader.ReadUInt64();

			mPropertyName = Util.ReadString(reader);

			// ???
			reader.ReadByte();

			mValue = reader.ReadBytes((int)size);
		}

		public override void Write(BinaryWriter writer)
		{
			Util.WriteString(writer, Name);
			Util.WriteString(writer, "ByteProperty");
			writer.Write(mValue.LongLength);
			Util.WriteString(writer, mPropertyName);
			writer.Write('\0');
			writer.Write(mValue);
		}

		public override void WriteValue(BinaryWriter writer)
		{
			writer.Write(mValue);
		}
	}
}
