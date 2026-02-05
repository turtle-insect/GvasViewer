namespace Gvas.Property.Standard
{
	public class GvasEnumProperty : GvasProperty
	{
		public GvasEnumProperty()
			: base()
		{ }

		public GvasEnumProperty(GvasEnumProperty property)
			: base(property)
		{
			mKey = property.mKey;
			mValue = property.mValue;
		}

		public override GvasProperty Clone()
		{
			return new GvasEnumProperty(this);
		}

		private String mValue = String.Empty;

		public override object Value
		{
			get => mValue;
			set => throw new NotImplementedException();
		}

		private String mKey = String.Empty;

		public override void Read(BinaryReader reader)
		{
			var size = reader.ReadUInt64();

			mKey = Util.ReadString(reader);
			// ???
			reader.ReadByte();

			mValue = Util.ReadString(reader);
		}

		public override void Write(BinaryWriter writer)
		{
			Util.WriteString(writer, Name);
			Util.WriteString(writer, "EnumProperty");
			writer.Write((UInt64)mValue.Length + 5);
			Util.WriteString(writer, mKey);
			writer.Write('\0');
			Util.WriteString(writer, mValue);
		}

		public override void WriteValue(BinaryWriter writer)
		{
			throw new NotImplementedException();
		}
	}
}
