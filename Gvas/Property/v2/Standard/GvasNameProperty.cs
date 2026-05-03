namespace Gvas.Property.v2.Standard
{
	public class GvasNameProperty : GvasProperty
	{
		private GvasString mValue = new();
		private Byte[] mBuffer = [];

		public GvasNameProperty()
			: base()
		{ }

		public GvasNameProperty(GvasNameProperty property)
			: base(property)
		{
			mValue = new(property.mValue);
			mBuffer = property.mBuffer.ToArray();
		}

		public override GvasProperty Clone()
		{
			return new GvasNameProperty(this);
		}

		public override object Value
		{
			get => mValue.Value;
			set => mValue.Value = value.ToString() ?? "";
		}

		public override void Read(BinaryReader reader)
		{
			reader.ReadBytes(4);
			var size = reader.ReadUInt32();
			reader.ReadByte();
			var position = reader.BaseStream.Position;

			try
			{
				ReadValue(reader);
			}
			catch
			{
				reader.BaseStream.Position = position;
				mBuffer = reader.ReadBytes((int)size);
			}
		}

		public override void Write(BinaryWriter writer)
		{
			Name.Write(writer);
			Util.WriteString(writer, "NameProperty");
			writer.Write(0);
			if (mBuffer.Length == 0)
			{
				writer.Write(mValue.Size() + 4);
				writer.Write('\0');
				mValue.Write(writer);
			}
			else
			{
				writer.Write(mBuffer.Length);
				writer.Write('\0');
				writer.Write(mBuffer);
			}
		}

		public override void ReadValue(BinaryReader reader)
		{
			mValue.Read(reader);
		}

		public override void WriteValue(BinaryWriter writer)
		{
			mValue.Write(writer);
		}
	}
}
