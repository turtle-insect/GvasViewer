namespace Gvas.Property
{
	internal class GvasObjectProperty : GvasProperty
	{
		private Byte[] mValue = [];
		public override object Value
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}

		public GvasObjectProperty()
			: base()
		{ }

		public GvasObjectProperty(GvasObjectProperty property)
			: base(property)
		{
			mValue = property.mValue.ToArray();
		}

		public override GvasProperty Clone()
		{
			return new GvasObjectProperty(this);
		}

		public override void Read(BinaryReader reader)
		{
			var size = reader.ReadUInt64();

			// ???
			reader.ReadByte();

			mValue = reader.ReadBytes((int)size);
		}

		public override void Write(BinaryWriter writer)
		{
			Util.WriteString(writer, Name);
			Util.WriteString(writer, "ObjectProperty");
			writer.Write(mValue.LongLength);
			writer.Write('\0');
			writer.Write(mValue);
		}

		public override void WriteValue(BinaryWriter writer)
		{
			throw new NotImplementedException();
		}
	}
}
