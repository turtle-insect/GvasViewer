namespace Gvas.Property.v2.Standard
{
	public class GvasArrayProperty : GvasProperty
	{
		public GvasString PropertyType
		{
			get => _tree.Children[0].Name;
		}
		private Byte[] _value = [];
		private GvasTree _tree = new();

		public GvasArrayProperty()
			: base()
		{ }

		public GvasArrayProperty(GvasArrayProperty property)
			: base(property)
		{
			_value = property._value.ToArray();
			_tree = new(property._tree);
		}

		public override GvasProperty Clone()
		{
			return new GvasArrayProperty(this);
		}

		public override object Value
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}

		public override void Read(BinaryReader reader)
		{
			_tree.Read(reader);

			var size = reader.ReadUInt32();
			reader.ReadByte();

			var name = _tree.Children[0].Name;
			switch (name.Value)
			{
				case "ByteProperty":
					{
						var property = new GvasByteProperty();
						property.Value = reader.ReadBytes((int)size);
						AppendChildren(property);
					}
					break;

				case "BoolProperty":
				case "IntProperty":
				case "UInt32Property":
				case "Int64Property":
				case "UInt64Property":
				case "FloatProperty":
				case "StrProperty":
				case "NameProperty":
				case "StructProperty":
					{
						uint count = reader.ReadUInt32();
						for (uint index = 0; index < count; index++)
						{
							var property = Util.CreateProperty(name);
							property.Name = new($"[{index}]", System.Text.Encoding.UTF8);
							property.ReadValue(reader);
							AppendChildren(property);
						}
					}
					break;

				default:
					_value = reader.ReadBytes((int)size);
					break;
			}
		}

		public override void Write(BinaryWriter writer)
		{
			Name.Write(writer);
			Util.WriteString(writer, "ArrayProperty");
			_tree.Write(writer);

			var name = _tree.Children[0].Name;
			switch (name.Value)
			{
				case "BoolProperty":
					WritePropertyValue(writer, 1);
					break;

				case "ByteProperty":
					{
						var buffer = Children[0].Value as Byte[];
						if(buffer == null) throw new NotImplementedException();

						writer.Write(buffer.Length);
						writer.Write('\0');
						writer.Write(buffer);
					}
					break;

				case "IntProperty":
				case "UInt32Property":
				case "FloatProperty":
					WritePropertyValue(writer, 4);
					break;

				case "Int64Property":
				case "UInt64Property":
					WritePropertyValue(writer, 8);
					break;

				case "StrProperty":
				case "NameProperty":
				case "StructProperty":
					{
						using var ms = new MemoryStream();
						using var bw = new BinaryWriter(ms);
						foreach (var child in Children)
						{
							child.WriteValue(bw);
						}
						bw.Flush();

						writer.Write((uint)ms.Length + 4);
						writer.Write('\0');
						writer.Write(Children.Count);
						writer.Write(ms.ToArray());
					}
					break;

				default:
					writer.Write(_value.Length);
					writer.Write('\0');
					writer.Write(_value);
					break;
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

		private void WritePropertyValue(BinaryWriter writer, uint size)
		{
			// Count + Children
			writer.Write((int)(Children.Count * size + 4));
			writer.Write('\0');
			writer.Write(Children.Count);
			foreach (var child in Children)
			{
				child.WriteValue(writer);
			}
		}
	}
}
