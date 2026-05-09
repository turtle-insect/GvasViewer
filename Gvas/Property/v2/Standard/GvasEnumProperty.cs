namespace Gvas.Property.v2.Standard
{
	public class GvasEnumProperty : GvasProperty
	{
		private GvasString mValue = new();
		private GvasTree _tree = new();

		public GvasEnumProperty()
			: base()
		{ }

		public GvasEnumProperty(GvasEnumProperty property)
			: base(property)
		{
			mValue = new(property.mValue);
			_tree = new(property._tree);
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
			_tree.Read(reader);

			reader.ReadBytes(5);

			ReadValue(reader);
		}

		public override void Write(BinaryWriter writer)
		{
			Name.Write(writer);
			Util.WriteString(writer, "EnumProperty");
			_tree.Write(writer);
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
