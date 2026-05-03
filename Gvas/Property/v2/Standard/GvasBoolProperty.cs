namespace Gvas.Property.v2.Standard
{
	public class GvasBoolProperty : GvasProperty
	{
		private Byte mValue;

		public GvasBoolProperty()
			: base()
		{ }

		public GvasBoolProperty(GvasBoolProperty property)
			: base(property)
		{
			mValue = property.mValue;
		}

		public override GvasProperty Clone()
		{
			return new GvasBoolProperty(this);
		}

		public override object Value
		{
			get => mValue;
			set
			{
				Byte tmp;
				Byte.TryParse(value.ToString(), out tmp);
				mValue = tmp;
			}
		}

		public override void Read(BinaryReader reader)
		{
			reader.ReadUInt64();

			ReadValue(reader);
		}

		public override void Write(BinaryWriter writer)
		{
			Name.Write(writer);
			Util.WriteString(writer, "BoolProperty");
			writer.Write((Int64)0);
			writer.Write(mValue);
		}

		public override void ReadValue(BinaryReader reader)
		{
			mValue = reader.ReadByte();
		}

		public override void WriteValue(BinaryWriter writer)
		{
			writer.Write(mValue);
		}
	}
}
