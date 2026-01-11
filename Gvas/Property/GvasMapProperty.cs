namespace Gvas.Property
{
	public class GvasMapProperty : GvasProperty
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
				if (mKeyType == "ByteProperty" ||
					mKeyType == "NameProperty")
				{
					name = Util.ReadString(reader);
				}
				else
				{
					reader.BaseStream.Position = position;
					mValue = reader.ReadBytes((int)size);
					break;
				}

				if (mValueType == "BoolProperty")
				{
					var property = new GvasBoolProperty();
					property.Name = name;
					property.Value = reader.ReadBoolean();
					Childrens.Add(property);
				}
				else if (mValueType == "IntProperty")
				{
					var property = new GvasIntProperty();
					property.Name = name;
					property.Value = reader.ReadInt32();
					Childrens.Add(property);
				}
				else if (mValueType == "NameProperty")
				{
					var property = new GvasNameProperty();
					property.Name = name;
					property.Value = Util.ReadString(reader);
					Childrens.Add(property);
				}
				else if (mValueType == "StructProperty")
				{
					var property = new GvasStructProperty();
					property.Name = name;
					property.ReadChild(reader, name);
					Childrens.Add(property);
				}
				else
				{
					reader.BaseStream.Position = position;
					mValue = reader.ReadBytes((int)size);
					break;
				}
			}
		}

		public override void Write(BinaryWriter writer)
		{
			Util.WriteString(writer, Name);
			Util.WriteString(writer, "MapProperty");

			if (mValue.Length > 0)
			{
				writer.Write(mValue.LongLength);
				Util.WriteString(writer, mKeyType);
				Util.WriteString(writer, mValueType);
				writer.Write('\0');
				writer.Write(mValue);
			}
			else
			{
				using var ms = new MemoryStream();
				using var bw = new BinaryWriter(ms);
				foreach (var children in Childrens)
				{
					Util.WriteString(bw, children.Name);
					children.WriteValue(bw);
				}
				bw.Flush();

				writer.Write(ms.Length + 8);
				Util.WriteString(writer, mKeyType);
				Util.WriteString(writer, mValueType);
				writer.Write('\0');
				writer.Write(0);
				writer.Write(Childrens.Count);
				writer.Write(ms.ToArray());
			}
		}

		public override void WriteValue(BinaryWriter writer)
		{
			throw new NotImplementedException();
		}
	}
}
