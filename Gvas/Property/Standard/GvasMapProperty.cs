using System.Xml.Linq;

namespace Gvas.Property.Standard
{
	public class GvasMapProperty : GvasProperty
	{
		private Byte[] mValue = [];
		public GvasString KeyType { get; private set; } = new();
		public GvasString ValueType { get; private set; } = new();


		public GvasMapProperty()
			: base()
		{ }

		public GvasMapProperty(GvasMapProperty property)
			: base(property)
		{
			KeyType = new(property.KeyType);
			ValueType = new(property.ValueType);
			mValue = property.mValue.ToArray();
		}

		public override GvasProperty Clone()
		{
			return new GvasMapProperty(this);
		}

		public override object Value
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}

		public override void Read(BinaryReader reader)
		{
			var size = reader.ReadUInt64();

			KeyType.Read(reader);
			ValueType.Read(reader);

			// ???
			reader.ReadByte();

			var position = reader.BaseStream.Position;
			reader.ReadBytes(4);
			var count = reader.ReadUInt32();
			for (uint index = 0; index < count; index++)
			{
				GvasString name = new();

				switch(KeyType.Value)
				{
					case "IntProperty":
						name = new(reader.ReadInt32().ToString(), System.Text.Encoding.UTF8);
						break;

					case "ByteProperty":
					case "NameProperty":
						name.Read(reader);
						break;

					default:
						reader.BaseStream.Position = position;
						mValue = reader.ReadBytes((int)size);
						return;
				}

				switch(ValueType.Value)
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
							property.ReadValue(reader);
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
			Name.Write(writer);
			Util.WriteString(writer, "MapProperty");

			if (mValue.Length > 0)
			{
				writer.Write(mValue.LongLength);
				KeyType.Write(writer);
				ValueType.Write(writer);
				writer.Write('\0');
				writer.Write(mValue);
			}
			else
			{
				using var ms = new MemoryStream();
				using var bw = new BinaryWriter(ms);
				foreach (var child in Children)
				{
					switch(KeyType.Value)
					{
						case "IntProperty":
							bw.Write(Int32.Parse(child.Name.Value));
							child.WriteValue(bw);
							break;

						default:
							child.Name.Write(bw);
							child.WriteValue(bw);
							break;
					}
				}
				bw.Flush();

				writer.Write(ms.Length + 8);
				KeyType.Write(writer);
				ValueType.Write(writer);
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
