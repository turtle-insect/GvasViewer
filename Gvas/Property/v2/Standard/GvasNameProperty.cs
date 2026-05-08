namespace Gvas.Property.v2.Standard
{
	public class GvasNameProperty : GvasProperty
	{
		private GvasString mValue = new();

		public GvasNameProperty()
			: base()
		{ }

		public GvasNameProperty(GvasNameProperty property)
			: base(property)
		{
			mValue = new(property.mValue);
		}

		public override GvasProperty Clone()
		{
			return new GvasNameProperty(this);
		}

		public override object Value
		{
			get => mValue.Value;
			set => mValue.Value = value.ToString() ?? "";
		}

		public override void Read(BinaryReader reader)
		{
			reader.ReadBytes(9);

			ReadValue(reader);
		}

		public override void Write(BinaryWriter writer)
		{
			Name.Write(writer);
			Util.WriteString(writer, "NameProperty");
			writer.Write(0);
			writer.Write(mValue.Size() + 4);
			writer.Write('\0');
			mValue.Write(writer);
		}

		public override void ReadValue(BinaryReader reader)
		{
			mValue.Read(reader);
		}

		public override void WriteValue(BinaryWriter writer)
		{
			mValue.Write(writer);
		}
	}
}
