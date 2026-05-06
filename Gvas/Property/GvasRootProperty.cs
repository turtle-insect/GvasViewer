namespace Gvas.Property
{
	internal class GvasRootProperty : GvasProperty
	{
		public GvasRootProperty()
			: base()
		{ }

		public GvasRootProperty(GvasRootProperty property)
			: base(property)
		{ }

		public override GvasProperty Clone()
		{
			return new GvasRootProperty(this);
		}

		public override object Value
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}

		public override void Read(BinaryReader reader)
		{
			for (; ; )
			{
				var property = Util.ReadProperty(reader);
				property.Read(reader);
				AppendChildren(property);
				if (property is GvasNoneProperty) break;
			}
			reader.ReadBytes(4);
		}

		public override void Write(BinaryWriter writer)
		{
			foreach (var property in Children)
			{
				property.Write(writer);
			}
			writer.Write(0);
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
