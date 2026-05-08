namespace Gvas.Property.v1.Standard
{
	public class GvasInt64Property : GvasProperty
	{
		private Int64 mValue;

		public GvasInt64Property()
			: base()
		{ }

		public GvasInt64Property(GvasInt64Property property)
			: base(property)
		{
			mValue = property.mValue;
		}

		public override GvasProperty Clone()
		{
			return new GvasInt64Property(this);
		}

		public override object Value
		{
			get => mValue;
			set
			{
				Int64 tmp;
				if (Int64.TryParse(value.ToString(), out tmp) == false) return;
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
			Util.WriteString(writer, "Int64Property");
			writer.Write((Int64)8);
			writer.Write('\0');
			writer.Write(mValue);
		}

		public override void ReadValue(BinaryReader reader)
		{
			mValue = reader.ReadInt64();
		}

		public override void WriteValue(BinaryWriter writer)
		{
			writer.Write(mValue);
		}
	}
}
