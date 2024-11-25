namespace Gvas.Property
{
	public class GvasIntProperty : GvasProperty
	{
		private Int32 mValue;
		public override object Value
		{
			get => mValue;
			set
			{
				Int32 tmp;
				if (Int32.TryParse(value.ToString(), out tmp) == false) return;
				mValue = tmp;
			}
		}

		public override void Read(BinaryReader reader)
		{
			var size = reader.ReadUInt64();

			// ???
			reader.ReadByte();

			mValue = reader.ReadInt32();
		}

		public override void Write(BinaryWriter writer)
		{
			Util.WriteString(writer, Name);
			Util.WriteString(writer, "IntProperty");
			writer.Write((Int64)4);
			writer.Write('\0');
			writer.Write(mValue);
		}

		public override void WriteValue(BinaryWriter writer)
		{
			writer.Write(mValue);
		}
	}
}
