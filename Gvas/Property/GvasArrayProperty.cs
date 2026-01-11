namespace Gvas.Property
{
	public class GvasArrayProperty : GvasProperty
	{
		public String PropertyType { get; private set; } = String.Empty;
		private GvasStructProperty? mBaseProperty;

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
						var property = new GvasByteProperty();
						property.Value = reader.ReadBytes((int)size);
						Childrens.Add(property);
					}
					break;

				case "IntProperty":
					{
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
						mBaseProperty = new GvasStructProperty();
						uint count = reader.ReadUInt32();

						// name
						mBaseProperty.Name = Util.ReadString(reader);

						// type
						// StructProperty
						Util.ReadString(reader);

						// size
						reader.ReadUInt64();

						mBaseProperty.Detail = Util.ReadString(reader);
						mBaseProperty.GUID = reader.ReadBytes(16);

						// ???
						reader.ReadByte();

						for (uint index = 0; index < count; index++)
						{
							var property = new GvasStructProperty();
							property.Name = mBaseProperty.Name;
							property.Detail = mBaseProperty.Detail;
							property.ReadChild(reader, mBaseProperty.Detail);
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

			switch(PropertyType)
			{
				case "BoolProperty":
					WritePropertyValue(writer, 1);
					break;

				case "ByteProperty":
					{
						var buffer = Childrens[0].Value as Byte[];
						if(buffer == null) throw new NotImplementedException();

						writer.Write((Int64)buffer.Length);
						Util.WriteString(writer, PropertyType);
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

				case "StructProperty":
					{
						using var ms = new MemoryStream();
						using var bw = new BinaryWriter(ms);
						foreach (var children in Childrens)
						{
							children.WriteValue(bw);
						}
						bw.Flush();

						// Implementation error
						if (mBaseProperty == null) throw new NullReferenceException();

						// size
						// (Children.Count ~ ms.ToArray()).size
						writer.Write((Int64)4 + (mBaseProperty.Name.Length + 5) + 19 + 8 + (mBaseProperty.Detail.Length + 5) + 17 + ms.Length);
						Util.WriteString(writer, PropertyType);
						writer.Write('\0');
						writer.Write(Childrens.Count);
						Util.WriteString(writer, mBaseProperty.Name);
						Util.WriteString(writer, "StructProperty");
						writer.Write(ms.Length);
						Util.WriteString(writer, mBaseProperty.Detail);
						writer.Write(mBaseProperty.GUID);
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
