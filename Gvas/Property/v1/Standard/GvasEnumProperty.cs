namespace Gvas.Property.v1.Standard
{
	public class GvasEnumProperty : GvasProperty
	{
		private GvasString mKey = new();
		private GvasString mValue = new();

		public GvasEnumProperty()
			: base()
		{ }

		public GvasEnumProperty(GvasEnumProperty property)
			: base(property)
		{
			mKey = new(property.mKey);
			mValue = new(property.mValue);
		}

		public override GvasProperty Clone()
		{
			return new GvasEnumProperty(this);
		}

		public override object Value
		{
			get => mValue;
			set => throw new NotImplementedException();
		}

		public override void Read(BinaryReader reader)
		{
			var size = reader.ReadUInt64();

			mKey.Read(reader);
			// ???
			reader.ReadByte();

			mValue.Read(reader);
		}

		public override void Write(BinaryWriter writer)
		{
			Name.Write(writer);
			Util.WriteString(writer, "EnumProperty");
			writer.Write((UInt64)mValue.Size() + 4);
			mKey.Write(writer);
			writer.Write('\0');
			mValue.Write(writer);
		}

		public override void ReadValue(BinaryReader reader)
		{
			throw new NotImplementedException();
		}

		public override void WriteValue(BinaryWriter writer)
		{
			throw new NotImplementedException();
		}
	}
}
