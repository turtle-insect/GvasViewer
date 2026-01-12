namespace Gvas.Property
{
	public class GvasUInt64Property : GvasProperty
	{
		private UInt64 mValue;
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

		public override void Read(BinaryReader reader)
		{
			var size = reader.ReadUInt64();

			// ???
			reader.ReadByte();

			mValue = reader.ReadUInt64();
		}

		public override void Write(BinaryWriter writer)
		{
			Util.WriteString(writer, Name);
			Util.WriteString(writer, "UInt64Property");
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
