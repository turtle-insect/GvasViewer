namespace Gvas.Property
{
	internal class GvasLiteralProperty : GvasProperty
	{
		private Byte[] mValue = [];
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
			mValue = reader.ReadBytes(size);
		}

		public override void Write(BinaryWriter writer)
		{
			writer.Write(mValue);
		}

		public override void WriteValue(BinaryWriter writer)
		{
			throw new NotImplementedException();
		}
	}
}
