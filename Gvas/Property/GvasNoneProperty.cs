namespace Gvas.Property
{
	internal class GvasNoneProperty : GvasProperty
	{
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
