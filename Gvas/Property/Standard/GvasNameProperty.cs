namespace Gvas.Property.Standard
{
	public class GvasNameProperty : GvasProperty
	{
		public GvasNameProperty()
			: base()
		{ }

		public GvasNameProperty(GvasNameProperty property)
			: base(property)
		{
			mBuffer = property.mBuffer.ToArray();
			mValue = property.mValue;
		}

		public override GvasProperty Clone()
		{
			return new GvasNameProperty(this);
		}

		private String mValue = String.Empty;
		public override object Value
		{
			get => mValue;
			set => mValue = value.ToString() ?? "";
		}
		private Byte[] mBuffer = [];

		public override void Read(BinaryReader reader)
		{
			var size = reader.ReadUInt64();

			// ???
			reader.ReadByte();

			try
			{
				mValue = Util.ReadString(reader);
			}
			catch
			{
				mBuffer = reader.ReadBytes((int)size);
			}
		}

		public override void Write(BinaryWriter writer)
		{
			Util.WriteString(writer, Name);
			Util.WriteString(writer, "NameProperty");
			if (mBuffer.Length == 0)
			{
				writer.Write((UInt64)mValue.Length + 5);
				writer.Write('\0');
				Util.WriteString(writer, mValue);
			}
			else
			{
				writer.Write(mBuffer.LongLength);
				writer.Write('\0');
				writer.Write(mBuffer);
			}
		}

		public override void WriteValue(BinaryWriter writer)
		{
			Util.WriteString(writer, mValue);
		}
	}
}
