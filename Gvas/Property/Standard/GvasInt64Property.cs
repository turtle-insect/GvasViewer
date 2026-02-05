namespace Gvas.Property.Standard
{
	public class GvasInt64Property : GvasProperty
	{
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

		private Int64 mValue;
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
			var size = reader.ReadUInt64();

			// ???
			reader.ReadByte();

			mValue = reader.ReadInt64();
		}

		public override void Write(BinaryWriter writer)
		{
			Util.WriteString(writer, Name);
			Util.WriteString(writer, "Int64Property");
			writer.Write((Int64)8);
			writer.Write('\0');
			writer.Write(mValue);
		}

		public override void WriteValue(BinaryWriter writer)
		{
			writer.Write(mValue);
		}
	}
}
