namespace Gvas.Property.Standard
{
	public class GvasFloatProperty : GvasProperty
	{
		private float mValue;

		public GvasFloatProperty()
			: base()
		{ }

		public GvasFloatProperty(GvasFloatProperty property)
			: base(property)
		{
			mValue = property.mValue;
		}

		public override GvasProperty Clone()
		{
			return new GvasFloatProperty(this);
		}

		public override object Value
		{
			get => mValue;
			set
			{
				float tmp;
				if (float.TryParse(value.ToString(), out tmp) == false) return;
				mValue = tmp;
			}
		}

		public override void Read(BinaryReader reader)
		{
			var size = reader.ReadUInt64();

			// ???
			reader.ReadByte();

			ReadValue(reader);
		}

		public override void Write(BinaryWriter writer)
		{
			Name.Write(writer);
			Util.WriteString(writer, "FloatProperty");
			writer.Write((Int64)4);
			writer.Write('\0');
			writer.Write(mValue);
		}

		public override void ReadValue(BinaryReader reader)
		{
			mValue = reader.ReadSingle();
		}

		public override void WriteValue(BinaryWriter writer)
		{
			writer.Write(mValue);
		}
	}
}
