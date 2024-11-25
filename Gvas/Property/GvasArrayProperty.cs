namespace Gvas.Property
{
	public class GvasArrayProperty : GvasProperty
	{
		private String mPropertyType = String.Empty;
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
			mPropertyType = Util.ReadString(reader);

			// ???
			reader.ReadByte();

			switch(mPropertyType)
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

			var WriteProperty = (uint size) =>
			{
				// Count + Childrens
				writer.Write((Int64)Childrens.Count * size + 4);
				Util.WriteString(writer, mPropertyType);
				writer.Write('\0');
				writer.Write(Childrens.Count);
				foreach (var children in Childrens)
				{
					children.WriteValue(writer);
				}
			};

			switch(mProperty)
			{
				case GvasBoolProperty:
					WriteProperty(1);
					break;

				case GvasIntProperty:
				case GvasUInt32Property:
				case GvasFloatProperty:
					WriteProperty(4);
					break;

				case GvasInt64Property:
				case GvasUInt64Property:
					WriteProperty(8);
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
						Util.WriteString(writer, mPropertyType);
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
					Util.WriteString(writer, mPropertyType);
					writer.Write('\0');
					writer.Write(mValue);
					break;
			}
		}

		public override void WriteValue(BinaryWriter writer)
		{
			throw new NotImplementedException();
		}
	}
}
