namespace Gvas.Property
{
	internal class GvasMapProperty : GvasProperty
	{
		private String mKeyType = String.Empty;
		private String mValueType = String.Empty;
		private Byte[] mValue = [];
		public override object Value
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}

		public override void Read(BinaryReader reader)
		{
			var size = reader.ReadUInt64();

			mKeyType = Util.ReadString(reader);
			mValueType = Util.ReadString(reader);

			// ???
			reader.ReadByte();

			var position = reader.BaseStream.Position;
			reader.ReadBytes(4);
			var count = reader.ReadUInt32();
			for (uint index = 0; index < count; index++)
			{
				var name = "";
				if (mKeyType == "NameProperty")
				{
					name = Util.ReadString(reader);
				}
				else
				{
					break;
				}

				switch (mValueType)
				{
					case "StructProperty":
						var property = new GvasStructProperty();
						property.Name = name;
						property.ReadChild(reader, name);
						Childrens.Add(property);
						break;
				}
			}
			reader.BaseStream.Position = position;

			mValue = reader.ReadBytes((int)size);
		}

		public override void Write(BinaryWriter writer)
		{
			Util.WriteString(writer, Name);
			Util.WriteString(writer, "MapProperty");
			writer.Write(mValue.LongLength);
			Util.WriteString(writer, mKeyType);
			Util.WriteString(writer, mValueType);
			writer.Write('\0');
			writer.Write(mValue);
		}

		public override void WriteValue(BinaryWriter writer)
		{
			throw new NotImplementedException();
		}
	}
}
