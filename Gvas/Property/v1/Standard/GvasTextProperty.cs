namespace Gvas.Property.v1.Standard
{
	public class GvasTextProperty : GvasProperty
	{
		private List<String> mValue = new();

		private Byte mFlag;
		private int mPattern;
		private Byte[] mBuffer = [];

		public GvasTextProperty()
			: base()
		{ }

		public GvasTextProperty(GvasTextProperty property)
			: base(property)
		{
			foreach (var value in property.mValue)
			{
				mValue.Add(value);
			}
			mFlag = property.mFlag;
			mPattern = property.mPattern;
			mBuffer = property.mBuffer.ToArray();
		}

		public override GvasProperty Clone()
		{
			return new GvasTextProperty(this);
		}

		public override object Value
		{
			get
			{
				if (mValue.Count < 2) return "";
				return mValue[1];
			}
			set
			{
				if (mValue.Count < 2) return;
				mValue[1] = value.ToString() ?? "";
			}
		}

		public override void Read(BinaryReader reader)
		{
			var size = reader.ReadUInt64();

			// ???
			reader.ReadByte();

			mFlag = reader.ReadByte();
			if(mFlag != 0)
			{
				reader.BaseStream.Position--;
				mBuffer = reader.ReadBytes((int)size);
			}
			else
			{
				var position = reader.BaseStream.Position;
				mPattern = reader.ReadInt32();
				uint length = 0;
				try
				{
					for (; length < size - 5;)
					{
						var str = Util.ReadString(reader);
						mValue.Add(str.Value);
						length += 4;
						length += (uint)str.Value.Length + 1;
					}
				}
				catch
				{
					mValue.Clear();
					reader.BaseStream.Position = position - 1;
					mBuffer = reader.ReadBytes((int)size);
				}
			}
		}

		public override void Write(BinaryWriter writer)
		{
			Name.Write(writer);
			Util.WriteString(writer, "TextProperty");
			if (mBuffer.Length > 0)
			{
				writer.Write(mBuffer.LongLength);
				writer.Write('\0');
				writer.Write(mBuffer);
			}
			else
			{
				UInt64 size = 0;
				foreach (var value in mValue)
				{
					size += 4;
					size += (uint)value.Length + 1;
				}
				writer.Write(size + 5);
				writer.Write('\0');
				writer.Write(mFlag);
				writer.Write(mPattern);
				foreach (var value in mValue)
				{
					Util.WriteString(writer, value);
				}
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
