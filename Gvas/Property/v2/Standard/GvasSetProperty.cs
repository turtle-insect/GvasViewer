namespace Gvas.Property.v2.Standard
{
	internal class GvasSetProperty : GvasProperty
	{
		private GvasString mPropertyType = new();
		private Byte[] mValue = [];

		public GvasSetProperty()
			: base()
		{ }

		public GvasSetProperty(GvasSetProperty property)
			: base(property)
		{
			mPropertyType = new(property.mPropertyType);
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

			mPropertyType.Read(reader);

			// ???
			reader.ReadByte();

			mValue = reader.ReadBytes((int)size);
		}

		public override void Write(BinaryWriter writer)
		{
			Name.Write(writer);
			Util.WriteString(writer, "SetProperty");
			writer.Write(mValue.LongLength);
			mPropertyType.Write(writer);
			writer.Write('\0');
			writer.Write(mValue);
		}

		public override void ReadValue(BinaryReader reader)
		{
			throw new NotImplementedException();
		}

		public override void WriteValue(BinaryWriter writer)
		{
			throw new NotImplementedException();
		}
	}
}
