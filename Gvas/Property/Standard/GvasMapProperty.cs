namespace Gvas.Property.Standard
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

		public GvasMapProperty()
			: base()
		{ }

		public GvasMapProperty(GvasMapProperty property)
			: base(property)
		{
			KeyType = property.KeyType;
			ValueType = property.ValueType;
			mValue = property.mValue.ToArray();
		}

		public override GvasProperty Clone()
		{
			return new GvasMapProperty(this);
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

				switch(KeyType)
				{
					case "ByteProperty":
					case "NameProperty":
						name = Util.ReadString(reader);
						break;

					default:
						reader.BaseStream.Position = position;
						mValue = reader.ReadBytes((int)size);
						return;
				}

				switch(ValueType)
				{
					case "BoolProperty":
						{
							var property = new GvasBoolProperty();
							property.Name = name;
							property.Value = reader.ReadBoolean();
							AppendChildren(property);
						}
						break;

					case "IntProperty":
						{
							var property = new GvasIntProperty();
							property.Name = name;
							property.Value = reader.ReadInt32();
							AppendChildren(property);
						}
						break;

					case "NameProperty":
						{
							var property = new GvasNameProperty();
							property.Name = name;
							property.Value = Util.ReadString(reader);
							AppendChildren(property);
						}
						break;

					case "StructProperty":
						{
							var property = new GvasStructProperty();
							property.Name = name;
							property.ReadChild(reader, name);
							AppendChildren(property);
						}
						break;

					default:
						reader.BaseStream.Position = position;
						mValue = reader.ReadBytes((int)size);
						return;
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
				foreach (var child in Children)
				{
					Util.WriteString(bw, child.Name);
					child.WriteValue(bw);
				}
				bw.Flush();

				writer.Write(ms.Length + 8);
				Util.WriteString(writer, KeyType);
				Util.WriteString(writer, ValueType);
				writer.Write('\0');
				writer.Write(0);
				writer.Write(Children.Count);
				writer.Write(ms.ToArray());
			}
		}

		public override void WriteValue(BinaryWriter writer)
		{
			throw new NotImplementedException();
		}
	}
}
