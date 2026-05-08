namespace Gvas.Property.v1.Standard
{
	public class GvasInt8Property : GvasProperty
	{
		private Byte mValue;

		public GvasInt8Property()
			: base()
		{ }

		public GvasInt8Property(GvasInt8Property property)
			: base(property)
		{
			mValue = property.mValue;
		}

		public override GvasProperty Clone()
		{
			return new GvasInt8Property(this);
		}

		public override object Value
		{
			get => mValue;
			set
			{
				Byte tmp;
				if (Byte.TryParse(value.ToString(), out tmp) == false) return;
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
			Util.WriteString(writer, "Int8Property");
			writer.Write((Int64)4);
			writer.Write('\0');
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
