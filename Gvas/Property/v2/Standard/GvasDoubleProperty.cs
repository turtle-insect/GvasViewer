namespace Gvas.Property.v2.Standard
{
	public class GvasDoubleProperty : GvasProperty
	{
		private double mValue;

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

		public override void Read(BinaryReader reader)
		{
			reader.ReadBytes(9);

			ReadValue(reader);
		}

		public override void Write(BinaryWriter writer)
		{
			Name.Write(writer);
			Util.WriteString(writer, "DoubleProperty");
			writer.Write(0);
			writer.Write(8);
			writer.Write('\0');
			writer.Write(mValue);
		}

		public override void ReadValue(BinaryReader reader)
		{
			mValue = reader.ReadDouble();
		}

		public override void WriteValue(BinaryWriter writer)
		{
			writer.Write(mValue);
		}
	}
}
