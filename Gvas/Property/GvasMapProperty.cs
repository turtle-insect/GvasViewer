namespace Gvas.Property
{
	public class GvasMapProperty : GvasProperty
	{
		public String KeyType { get; private set; } = String.Empty;
		public String ValueType { get; private set; } = String.Empty;
		private Byte[] mValue = [];
		public override object Value
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}

		public override void Read(BinaryReader reader)
		{
			var size = reader.ReadUInt64();

			KeyType = Util.ReadString(reader);
			ValueType = Util.ReadString(reader);

			// ???
			reader.ReadByte();

			var position = reader.BaseStream.Position;
			reader.ReadBytes(4);
			var count = reader.ReadUInt32();
			for (uint index = 0; index < count; index++)
			{
				var name = "";
				if (KeyType == "ByteProperty" ||
					KeyType == "NameProperty")
				{
					name = Util.ReadString(reader);
				}
				else
				{
					reader.BaseStream.Position = position;
					mValue = reader.ReadBytes((int)size);
					break;
				}

				if (ValueType == "BoolProperty")
				{
					var property = new GvasBoolProperty();
					property.Name = name;
					property.Value = reader.ReadBoolean();
					Childrens.Add(property);
				}
				else if (ValueType == "IntProperty")
				{
					var property = new GvasIntProperty();
					property.Name = name;
					property.Value = reader.ReadInt32();
					Childrens.Add(property);
				}
				else if (ValueType == "NameProperty")
				{
					var property = new GvasNameProperty();
					property.Name = name;
					property.Value = Util.ReadString(reader);
					Childrens.Add(property);
				}
				else if (ValueType == "StructProperty")
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
				Util.WriteString(writer, KeyType);
				Util.WriteString(writer, ValueType);
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
				Util.WriteString(writer, KeyType);
				Util.WriteString(writer, ValueType);
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
