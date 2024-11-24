namespace Gvas.Property
{
	public class GvasBoolProperty : GvasProperty
	{
		private Byte mValue;

		public override object Value
		{
			get => mValue;
			set => throw new NotImplementedException();
		}

		public override void Read(BinaryReader reader)
		{
			var size = reader.ReadUInt64();

			mValue = reader.ReadByte();

			// ???
			reader.ReadByte();
		}

		public override void Write(BinaryWriter writer)
		{
			Util.WriteString(writer, Name);
			Util.WriteString(writer, "BoolProperty");
			writer.Write((Int64)0);
			writer.Write(mValue);
			writer.Write('\0');
		}
	}
}
