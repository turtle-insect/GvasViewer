namespace Gvas.Property.v2.Standard
{
	internal class GvasSoftObjectProperty : GvasProperty
	{
		private Byte[] mValue = [];

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

		public override object Value
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}

		public override void Read(BinaryReader reader)
		{
			reader.ReadBytes(4);
			var size = reader.ReadUInt64();

			// ???
			reader.ReadByte();

			mValue = reader.ReadBytes((int)size);
		}

		public override void Write(BinaryWriter writer)
		{
			Name.Write(writer);
			Util.WriteString(writer, "SoftObjectProperty");
			writer.Write(0);
			writer.Write(mValue.Length);
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
