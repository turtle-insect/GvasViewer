namespace Gvas.Property
{
	public class GvasFloatProperty : GvasProperty
	{
		private float mValue;
		public override object Value
		{
			get => mValue;
			set
			{
				float tmp;
				if (float.TryParse(value.ToString(), out tmp) == false) return;
				mValue = tmp;
			}
		}

		public override void Read(BinaryReader reader)
		{
			var size = reader.ReadUInt64();

			// ???
			reader.ReadByte();

			mValue = reader.ReadSingle();
		}

		public override void Write(BinaryWriter writer)
		{
			Util.WriteString(writer, Name);
			Util.WriteString(writer, "FloatProperty");
			writer.Write((Int64)4);
			writer.Write('\0');
			writer.Write(mValue);
		}
	}
}
