namespace Gvas.Property.Standard
{
	public class GvasDoubleProperty : GvasProperty
	{
		private double mValue;
		public override object Value
		{
			get => mValue;
			set
			{
				double tmp;
				if (double.TryParse(value.ToString(), out tmp) == false) return;
				mValue = tmp;
			}
		}

		public GvasDoubleProperty()
			: base()
		{ }

		public GvasDoubleProperty(GvasDoubleProperty property)
			: base(property)
		{
			mValue = property.mValue;
		}

		public override GvasProperty Clone()
		{
			return new GvasDoubleProperty(this);
		}

		public override void Read(BinaryReader reader)
		{
			var size = reader.ReadUInt64();

			// ???
			reader.ReadByte();

			mValue = reader.ReadDouble();
		}

		public override void Write(BinaryWriter writer)
		{
			Util.WriteString(writer, Name);
			Util.WriteString(writer, "DoubleProperty");
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
