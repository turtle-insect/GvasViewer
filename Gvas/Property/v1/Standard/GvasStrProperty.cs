namespace Gvas.Property.v1.Standard
{
	public class GvasStrProperty : GvasProperty
	{
		private GvasString mValue = new();
		private Byte[] mBuffer = [];

		public GvasStrProperty()
			: base()
		{ }

		public GvasStrProperty(GvasStrProperty property)
			: base(property)
		{
			mValue = new(property.mValue);
			mBuffer = property.mBuffer.ToArray();
		}

		public override GvasProperty Clone()
		{
			return new GvasStrProperty(this);
		}

		public override object Value
		{
			get => mValue.Value;
			set => mValue.Value = value.ToString() ?? "";
		}

		public override void Read(BinaryReader reader)
		{
			var size = reader.ReadUInt64();

			// ???
			reader.ReadByte();

			try
			{
				mValue.Read(reader);
			}
			catch
			{
				mBuffer = reader.ReadBytes((int)size);
			}
		}

		public override void Write(BinaryWriter writer)
		{
			Name.Write(writer);
			Util.WriteString(writer, "StrProperty");

			if (mBuffer.Length == 0)
			{
				writer.Write((UInt64)mValue.Size() + 4);
				writer.Write('\0');
				mValue.Write(writer);
			}
			else
			{
				writer.Write(mBuffer.LongLength);
				writer.Write('\0');
				writer.Write(mBuffer);
			}
		}

		public override void ReadValue(BinaryReader reader)
		{
			throw new NotImplementedException();
		}

		public override void WriteValue(BinaryWriter writer)
		{
			throw new NotImplementedException();
		}
	}
}
