namespace Gvas.Property.v2.Standard
{
	public class GvasMapProperty : GvasProperty
	{
		private Byte[] mValue = [];
		private GvasNode _node = new();


		public GvasMapProperty()
			: base()
		{ }

		public GvasMapProperty(GvasMapProperty property)
			: base(property)
		{
			_node = new(property._node);
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
			_node.Read(reader);

			var size = reader.ReadUInt32();

			reader.ReadByte();
			var position = reader.BaseStream.Position;
			reader.ReadBytes(4);
			var count = reader.ReadUInt32();
			for (uint index = 0; index < count; index++)
			{
				GvasString name = new();

				switch(_node.Names[0].Value)
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

				switch(_node.Names[1].Value)
				{
					case "BoolProperty":
					case "IntProperty":
					case "NameProperty":
					case "StructProperty":
						{
							var property = Util.CreateProperty(_node.Names[1]);
							property.Name = name;
							property.ReadValue(reader);
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

			_node.Write(writer);
			if (mValue.Length > 0)
			{
				writer.Write(mValue.Length);
				writer.Write('\0');
				writer.Write(mValue);
			}
			else
			{
				using var ms = new MemoryStream();
				using var bw = new BinaryWriter(ms);
				foreach (var child in Children)
				{
					switch(_node.Names[0].Value)
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

				writer.Write((int)(ms.Length + 8));
				writer.Write('\0');
				writer.Write(0);
				writer.Write(Children.Count);
				writer.Write(ms.ToArray());
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
