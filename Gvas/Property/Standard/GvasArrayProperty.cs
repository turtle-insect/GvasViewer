namespace Gvas.Property.Standard
{
	public class GvasArrayProperty : GvasProperty
	{
		public GvasString PropertyType { get; private set; } = new();
		private GvasStructProperty? mBaseProperty;
		private Byte[] mValue = [];

		public GvasArrayProperty()
			: base()
		{ }

		public GvasArrayProperty(GvasArrayProperty property)
			: base(property)
		{
			PropertyType = new(property.PropertyType);
			mBaseProperty = property.mBaseProperty;
			mValue = property.mValue.ToArray();
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
			var size = reader.ReadUInt64();

			// type
			PropertyType.Read(reader);

			// ???
			reader.ReadByte();

			switch (PropertyType.Value)
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
					{
						uint count = reader.ReadUInt32();
						for (uint index = 0; index < count; index++)
						{
							var property = Util.CreateProperty(PropertyType);
							property.Name = new($"[{index}]", System.Text.Encoding.UTF8);
							property.ReadValue(reader);
							AppendChildren(property);
						}
					}
					break;

				case "NameProperty":
					{
						var position = reader.BaseStream.Position;

						try
						{
							uint count = reader.ReadUInt32();
							for (uint index = 0; index < count; index++)
							{
								var property = new GvasNameProperty();
								property.Name = new($"[{index}]", System.Text.Encoding.UTF8);
								property.ReadValue(reader);
								AppendChildren(property);
							}
						}
						catch
						{
							reader.BaseStream.Position = position;
							mValue = reader.ReadBytes((int)size);
						}
					}
					break;

				case "StructProperty":
					{
						mBaseProperty = new GvasStructProperty();
						uint count = reader.ReadUInt32();

						// name
						mBaseProperty.Name.Read(reader);

						// type
						// StructProperty
						Util.ReadString(reader);

						// size
						reader.ReadUInt64();

						mBaseProperty.Detail.Read(reader);
						mBaseProperty.GUID = reader.ReadBytes(16);

						// ???
						reader.ReadByte();

						for (uint index = 0; index < count; index++)
						{
							var property = new GvasStructProperty();
							property.Name = mBaseProperty.Name;
							property.Detail = mBaseProperty.Detail;
							property.ReadChild(reader, mBaseProperty.Detail);
							AppendChildren(property);
						}
					}
					break;

				default:
					mValue = reader.ReadBytes((int)size);
					break;
			}
		}

		public override void Write(BinaryWriter writer)
		{
			Name.Write(writer);
			Util.WriteString(writer, "ArrayProperty");

			switch(PropertyType.Value)
			{
				case "BoolProperty":
					WritePropertyValue(writer, 1);
					break;

				case "ByteProperty":
					{
						var buffer = Children[0].Value as Byte[];
						if(buffer == null) throw new NotImplementedException();

						writer.Write((Int64)buffer.Length);
						PropertyType.Write(writer);
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

				case "NameProperty":
					{
						if(mValue.Length > 0)
						{
							writer.Write(mValue.LongLength);
							PropertyType.Write(writer);
							writer.Write('\0');
							writer.Write(mValue);
						}
						else
						{
							using var ms = new MemoryStream();
							using var bw = new BinaryWriter(ms);
							foreach (var child in Children)
							{
								child.WriteValue(bw);
							}
							bw.Flush();

							writer.Write(ms.Length + 4);
							PropertyType.Write(writer);
							writer.Write('\0');
							writer.Write(Children.Count);
							writer.Write(ms.ToArray());
						}
					}
					break;

				case "StructProperty":
					{
						using var ms = new MemoryStream();
						using var bw = new BinaryWriter(ms);
						foreach (var child in Children)
						{
							child.WriteValue(bw);
						}
						bw.Flush();

						// Implementation error
						if (mBaseProperty == null) throw new NullReferenceException();

						// size
						// (Children.Count ~ ms.ToArray()).size
						writer.Write((Int64)4 + mBaseProperty.Name.Size() + 4 + 19 + 8 + mBaseProperty.Detail.Size() + 4 + 17 + ms.Length);
						PropertyType.Write(writer);
						writer.Write('\0');
						writer.Write(Children.Count);
						mBaseProperty.Name.Write(writer);
						Util.WriteString(writer, "StructProperty");
						writer.Write(ms.Length);
						mBaseProperty.Detail.Write(writer);
						writer.Write(mBaseProperty.GUID);
						writer.Write('\0');
						writer.Write(ms.ToArray());
					}
					break;

				default:
					writer.Write(mValue.LongLength);
					PropertyType.Write(writer);
					writer.Write('\0');
					writer.Write(mValue);
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
			writer.Write((Int64)Children.Count * size + 4);
			PropertyType.Write(writer);
			writer.Write('\0');
			writer.Write(Children.Count);
			foreach (var child in Children)
			{
				child.WriteValue(writer);
			}
		}
	}
}
