namespace Gvas.Property.Standard
{
	public class GvasNameProperty : GvasProperty
	{
		private String mValue = String.Empty;
		private Byte[] mBuffer = [];

		public GvasNameProperty()
			: base()
		{ }

		public GvasNameProperty(GvasNameProperty property)
			: base(property)
		{
			mValue = property.mValue;
			mBuffer = property.mBuffer.ToArray();
		}

		public override GvasProperty Clone()
		{
			return new GvasNameProperty(this);
		}

		public override object Value
		{
			get => mValue;
			set => mValue = value.ToString() ?? "";
		}

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
