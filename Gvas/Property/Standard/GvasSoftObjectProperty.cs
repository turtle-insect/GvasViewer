namespace Gvas.Property.Standard
{
	internal class GvasSoftObjectProperty : GvasProperty
	{
		public GvasSoftObjectProperty()
			: base()
		{ }

		public GvasSoftObjectProperty(GvasSoftObjectProperty property)
			: base(property)
		{
			mValue = property.mValue.ToArray();
		}

		public override GvasProperty Clone()
		{
			return new GvasSoftObjectProperty(this);
		}

		private Byte[] mValue = [];
		public override object Value
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
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
			Util.WriteString(writer, "SoftObjectProperty");
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
