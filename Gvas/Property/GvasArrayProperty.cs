namespace Gvas.Property
{
	public class GvasArrayProperty : GvasProperty
	{
		public String PropertyType { get; private set; } = String.Empty;
		private GvasProperty? mProperty;

		private Byte[] mValue = [];
		public override object Value
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}

		public override void Read(BinaryReader reader)
		{
			var size = reader.ReadUInt64();

			// type
			PropertyType = Util.ReadString(reader);

			// ???
			reader.ReadByte();

			switch (PropertyType)
			{
				case "BoolProperty":
					{
						mProperty = new GvasBoolProperty();
						uint count = reader.ReadUInt32();
						for (uint index = 0; index < count; index++)
						{
							var property = new GvasBoolProperty();
							property.Name = $"[{index}]";
							property.Value = reader.ReadBoolean();
							Childrens.Add(property);
						}
					}
					break;

				case "ByteProperty":
					{
						mProperty = new GvasByteProperty();
						mProperty.Value = reader.ReadBytes((int)size);
						Childrens.Add(mProperty);
					}
					break;

				case "IntProperty":
					{
						mProperty = new GvasIntProperty();
						uint count = reader.ReadUInt32();
						for (uint index = 0; index < count; index++)
						{
							var property = new GvasIntProperty();
							property.Name = $"[{index}]";
							property.Value = reader.ReadInt32();
							Childrens.Add(property);
						}
					}
					break;

				case "UInt32Property":
					{
						mProperty = new GvasUInt32Property();
						uint count = reader.ReadUInt32();
						for (uint index = 0; index < count; index++)
						{
							var property = new GvasUInt32Property();
							property.Name = $"[{index}]";
							property.Value = reader.ReadUInt32();
							Childrens.Add(property);
						}
					}
					break;

				case "Int64Property":
					{
						mProperty = new GvasInt64Property();
						uint count = reader.ReadUInt32();
						for (uint index = 0; index < count; index++)
						{
							var property = new GvasInt64Property();
							property.Name = $"[{index}]";
							property.Value = reader.ReadInt64();
							Childrens.Add(property);
						}
					}
					break;

				case "UInt64Property":
					{
						mProperty = new GvasUInt64Property();
						uint count = reader.ReadUInt32();
						for (uint index = 0; index < count; index++)
						{
							var property = new GvasUInt64Property();
							property.Name = $"[{index}]";
							property.Value = reader.ReadUInt64();
							Childrens.Add(property);
						}
					}
					break;

				case "FloatProperty":
					{
						mProperty = new GvasFloatProperty();
						uint count = reader.ReadUInt32();
						for (uint index = 0; index < count; index++)
						{
							var property = new GvasFloatProperty();
							property.Name = $"[{index}]";
							property.Value = reader.ReadSingle();
							Childrens.Add(property);
						}
					}
					break;

				case "NameProperty":
					{
						mProperty = new GvasNameProperty();
						uint count = reader.ReadUInt32();
						for (uint index = 0; index < count; index++)
						{
							var property = new GvasNameProperty();
							property.Name = $"[{index}]";
							property.Value = Util.ReadString(reader);
							Childrens.Add(property);
						}
					}
					break;

				case "StructProperty":
					{
						var targetProperty = new GvasStructProperty();
						mProperty = targetProperty;
						uint count = reader.ReadUInt32();

						// name
						targetProperty.Name = Util.ReadString(reader);

						// type
						// StructProperty
						Util.ReadString(reader);

						// size
						reader.ReadUInt64();

						targetProperty.Detail = Util.ReadString(reader);
						targetProperty.GUID = reader.ReadBytes(16);

						// ???
						reader.ReadByte();

						for (uint index = 0; index < count; index++)
						{
							var property = new GvasStructProperty();
							property.Name = targetProperty.Name;
							property.Detail = targetProperty.Detail;
							property.ReadChild(reader, targetProperty.Detail);
							Childrens.Add(property);
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
			Util.WriteString(writer, Name);
			Util.WriteString(writer, "ArrayProperty");

			switch(mProperty)
			{
				case GvasBoolProperty:
					WritePropertyValue(writer, 1);
					break;

				case GvasByteProperty:
					{
						var buffer = mProperty.Value as Byte[];
						if(buffer == null) throw new NotImplementedException();

						writer.Write((Int64)buffer.Length);
						Util.WriteString(writer, PropertyType);
						writer.Write('\0');
						writer.Write(buffer);
					}
					break;

				case GvasIntProperty:
				case GvasUInt32Property:
				case GvasFloatProperty:
					WritePropertyValue(writer, 4);
					break;

				case GvasInt64Property:
				case GvasUInt64Property:
					WritePropertyValue(writer, 8);
					break;

				case GvasNameProperty nameProperty:
					{
						using var ms = new MemoryStream();
						using var bw = new BinaryWriter(ms);
						foreach (var children in Childrens)
						{
							children.WriteValue(bw);
						}
						bw.Flush();

						writer.Write(ms.Length + 4);
						Util.WriteString(writer, PropertyType);
						writer.Write('\0');
						writer.Write(Childrens.Count);
						writer.Write(ms.ToArray());
					}
					break;

				case GvasStructProperty structProperty:
					{
						using var ms = new MemoryStream();
						using var bw = new BinaryWriter(ms);
						foreach (var children in Childrens)
						{
							children.WriteValue(bw);
						}
						bw.Flush();

						// size
						// (Children.Count ~ ms.ToArray()).size
						writer.Write((Int64)4 + (structProperty.Name.Length + 5) + 19 + 8 + (structProperty.Detail.Length + 5) + 17 + ms.Length);
						Util.WriteString(writer, PropertyType);
						writer.Write('\0');
						writer.Write(Childrens.Count);
						Util.WriteString(writer, structProperty.Name);
						Util.WriteString(writer, "StructProperty");
						writer.Write(ms.Length);
						Util.WriteString(writer, structProperty.Detail);
						writer.Write(structProperty.GUID);
						writer.Write('\0');
						writer.Write(ms.ToArray());
					}
					break;

				default:
					writer.Write(mValue.LongLength);
					Util.WriteString(writer, PropertyType);
					writer.Write('\0');
					writer.Write(mValue);
					break;
			}
		}

		public override void WriteValue(BinaryWriter writer)
		{
			throw new NotImplementedException();
		}

		private void WritePropertyValue(BinaryWriter writer, uint size)
		{
			// Count + Childrens
			writer.Write((Int64)Childrens.Count * size + 4);
			Util.WriteString(writer, PropertyType);
			writer.Write('\0');
			writer.Write(Childrens.Count);
			foreach (var children in Childrens)
			{
				children.WriteValue(writer);
			}
		}
	}
}
