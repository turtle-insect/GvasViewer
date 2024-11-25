namespace Gvas.Property
{
	public class GvasUInt32Property : GvasProperty
	{
		private UInt32 mValue;
		public override object Value
		{
			get => mValue;
			set
			{
				UInt32 tmp;
				if (UInt32.TryParse(value.ToString(), out tmp) == false) return;
				mValue = tmp;
			}
		}

		public override void Read(BinaryReader reader)
		{
			var size = reader.ReadUInt64();

			// ???
			reader.ReadByte();

			mValue = reader.ReadUInt32();
		}

		public override void Write(BinaryWriter writer)
		{
			Util.WriteString(writer, Name);
			Util.WriteString(writer, "UInt32Property");
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
