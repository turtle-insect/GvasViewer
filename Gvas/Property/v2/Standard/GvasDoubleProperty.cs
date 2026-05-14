namespace Gvas.Property.v2.Standard
{
	public class GvasDoubleProperty : GvasProperty
	{
		private double _value;

		public GvasDoubleProperty()
			: base()
		{ }

		public GvasDoubleProperty(GvasDoubleProperty property)
			: base(property)
		{
			_value = property._value;
		}

		public override GvasProperty Clone()
		{
			return new GvasDoubleProperty(this);
		}

		public override object Value
		{
			get => _value;
			set
			{
				double tmp;
				if (double.TryParse(value.ToString(), out tmp) == false) return;
				_value = tmp;
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
			writer.Write(_value);
		}

		public override void ReadValue(BinaryReader reader)
		{
			_value = reader.ReadDouble();
		}

		public override void WriteValue(BinaryWriter writer)
		{
			writer.Write(_value);
		}
	}
}
