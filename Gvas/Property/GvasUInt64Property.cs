namespace Gvas.Property
{
	public class GvasUInt64Property : GvasProperty
	{
		private UInt64 mValue;
		public override object Value
		{
			get => mValue;
			set => throw new NotImplementedException();
		}

		public override void Read(BinaryReader reader)
		{
			var size = reader.ReadUInt64();

			// ???
			reader.ReadByte();

			mValue = reader.ReadUInt64();
		}

		public override void Write(BinaryWriter writer)
		{
			Util.WriteString(writer, Name);
			Util.WriteString(writer, "UInt64Property");
			writer.Write((Int64)8);
			writer.Write('\0');
			writer.Write(mValue);
		}
	}
}
