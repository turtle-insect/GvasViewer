namespace Gvas.Property.Standard
{
	internal class GvasSetProperty : GvasProperty
	{
		private String mPropertyType = String.Empty;
		private Byte[] mValue = [];

		public GvasSetProperty()
			: base()
		{ }

		public GvasSetProperty(GvasSetProperty property)
			: base(property)
		{
			mPropertyType = property.mPropertyType;
			mValue = property.mValue.ToArray();
		}

		public override GvasProperty Clone()
		{
			return new GvasSetProperty(this);
		}

		public override object Value
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}

		public override void Read(BinaryReader reader)
		{
			var size = reader.ReadUInt64();

			mPropertyType = Util.ReadString(reader);

			// ???
			reader.ReadByte();

			mValue = reader.ReadBytes((int)size);
		}

		public override void Write(BinaryWriter writer)
		{
			Util.WriteString(writer, Name);
			Util.WriteString(writer, "SetProperty");
			writer.Write(mValue.LongLength);
			Util.WriteString(writer, mPropertyType);
			writer.Write('\0');
			writer.Write(mValue);
		}

		public override void WriteValue(BinaryWriter writer)
		{
			throw new NotImplementedException();
		}
	}
}
