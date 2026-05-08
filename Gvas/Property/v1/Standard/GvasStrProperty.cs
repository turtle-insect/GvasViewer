namespace Gvas.Property.v1.Standard
{
	public class GvasStrProperty : GvasProperty
	{
		private GvasString mValue = new();

		public GvasStrProperty()
			: base()
		{ }

		public GvasStrProperty(GvasStrProperty property)
			: base(property)
		{
			mValue = new(property.mValue);
		}

		public override GvasProperty Clone()
		{
			return new GvasStrProperty(this);
		}

		public override object Value
		{
			get => mValue.Value;
			set => mValue.Value = value.ToString() ?? "";
		}

		public override void Read(BinaryReader reader)
		{
			var size = reader.ReadUInt64();

			// ???
			reader.ReadByte();

			ReadValue(reader);
		}

		public override void Write(BinaryWriter writer)
		{
			Name.Write(writer);
			Util.WriteString(writer, "StrProperty");

			writer.Write((UInt64)mValue.Size() + 4);
			writer.Write('\0');
			mValue.Write(writer);
		}

		public override void ReadValue(BinaryReader reader)
		{
			mValue.Read(reader);
		}

		public override void WriteValue(BinaryWriter writer)
		{
			mValue.Write(writer);
		}
	}
}
