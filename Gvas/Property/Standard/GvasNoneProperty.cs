using System;

namespace Gvas.Property.Standard
{
	internal class GvasNoneProperty : GvasProperty
	{
		public GvasNoneProperty()
			: base()
		{ }

		public GvasNoneProperty(GvasNoneProperty property)
			: base(property)
		{ }

		public override GvasProperty Clone()
		{
			return new GvasNoneProperty(this);
		}

		public override object Value
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}

		public override void Read(BinaryReader reader)
		{
		}

		public override void Write(BinaryWriter writer)
		{
			Util.WriteString(writer, Name);
		}

		public override void WriteValue(BinaryWriter writer)
		{
			throw new NotImplementedException();
		}
	}
}
