namespace Gvas.Property
{
	public class GvasBoolProperty : GvasProperty
	{
		private Boolean mValue;

		public override object Value
		{
			get => mValue;
			set
			{
				Boolean tmp;
				Boolean.TryParse(value.ToString(), out tmp);
				mValue = tmp;
			}
		}

		public override void Read(BinaryReader reader)
		{
			var size = reader.ReadUInt64();

			mValue = reader.ReadBoolean();

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
