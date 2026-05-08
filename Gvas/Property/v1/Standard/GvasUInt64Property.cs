namespace Gvas.Property.v1.Standard
{
	public class GvasUInt64Property : GvasProperty
	{
		private UInt64 mValue;

		public GvasUInt64Property()
			: base()
		{ }

		public GvasUInt64Property(GvasUInt64Property property)
			: base(property)
		{
			mValue = property.mValue;
		}

		public override GvasProperty Clone()
		{
			return new GvasUInt64Property(this);
		}

		public override object Value
		{
			get => mValue;
			set
			{
				UInt64 tmp;
				if (UInt64.TryParse(value.ToString(), out tmp) == false) return;
				mValue = tmp;
			}
		}

		public override void Read(BinaryReader reader)
		{
			reader.ReadBytes(9);

			ReadValue(reader);
		}

		public override void Write(BinaryWriter writer)
		{
			Name.Write(writer);
			Util.WriteString(writer, "UInt64Property");
			writer.Write((Int64)8);
			writer.Write('\0');
			writer.Write(mValue);
		}

		public override void ReadValue(BinaryReader reader)
		{
			mValue = reader.ReadUInt64();
		}

		public override void WriteValue(BinaryWriter writer)
		{
			writer.Write(mValue);
		}
	}
}
