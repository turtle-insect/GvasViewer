namespace Gvas.Property
{
	internal class GvasLiteralProperty : GvasProperty
	{
		private Byte[] _value = [];

		public GvasLiteralProperty()
			: base()
		{ }

		public GvasLiteralProperty(GvasLiteralProperty property)
			: base(property)
		{
			_value = property._value.ToArray();
		}

		public override GvasProperty Clone()
		{
			return new GvasLiteralProperty(this);
		}

		public override object Value
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}

		public override void Read(BinaryReader reader)
		{
			throw new NotImplementedException();
		}

		public void Read(BinaryReader reader, int size)
		{
			_value = reader.ReadBytes(size);
		}

		public override void Write(BinaryWriter writer)
		{
			writer.Write(_value);
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
