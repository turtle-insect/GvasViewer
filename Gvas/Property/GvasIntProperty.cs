namespace Gvas.Property
{
	public class GvasIntProperty : GvasProperty
	{
		private int mValue;
		public override object Value
		{
			get => mValue;
			set
			{
				int tmp;
				if (int.TryParse(value.ToString(), out tmp) == false) return;
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
	}
}
