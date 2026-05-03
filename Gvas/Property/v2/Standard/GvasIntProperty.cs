namespace Gvas.Property.v2.Standard
{
	public class GvasIntProperty : GvasProperty
	{
		private Int32 mValue;

		public GvasIntProperty()
			: base()
		{ }

		public GvasIntProperty(GvasIntProperty property)
			: base(property)
		{
			mValue = property.mValue;
		}

		public override GvasProperty Clone()
		{
			return new GvasIntProperty(this);
		}

		public override object Value
		{
			get => mValue;
			set
			{
				Int32 tmp;
				if (Int32.TryParse(value.ToString(), out tmp) == false) return;
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
			Util.WriteString(writer, "IntProperty");
			writer.Write(0);
			writer.Write(4);
			writer.Write('\0');
			writer.Write(mValue);
		}

		public override void ReadValue(BinaryReader reader)
		{
			mValue = reader.ReadInt32();
		}

		public override void WriteValue(BinaryWriter writer)
		{
			writer.Write(mValue);
		}
	}
}
