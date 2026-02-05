namespace Gvas.Property.Standard
{
	public class GvasArrayProperty : GvasProperty
	{
		public String PropertyType { get; private set; } = String.Empty;
		private GvasStructProperty? mBaseProperty;
		private Byte[] mValue = [];

		public GvasArrayProperty()
			: base()
		{ }

		public GvasArrayProperty(GvasArrayProperty property)
			: base(property)
		{
			PropertyType = property.PropertyType;
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
							AppendChildren(property);
						}
					}
					break;

				case "ByteProperty":
					{
						var property = new GvasByteProperty();
						property.Value = reader.ReadBytes((int)size);
						AppendChildren(property);
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
							AppendChildren(property);
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
							AppendChildren(property);
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
							AppendChildren(property);
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
							AppendChildren(property);
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
								property.Name = $"[{index}]";
								property.Value = Util.ReadString(reader);
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
			Util.WriteString(writer, Name);
			Util.WriteString(writer, "ArrayProperty");

			switch(PropertyType)
			{
				case "BoolProperty":
					WritePropertyValue(writer, 1);
					break;

				case "ByteProperty":
					{
						var buffer = Children[0].Value as Byte[];
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
						if(mValue.Length > 0)
						{
							writer.Write(mValue.LongLength);
							Util.WriteString(writer, PropertyType);
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
							Util.WriteString(writer, PropertyType);
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
						writer.Write((Int64)4 + (mBaseProperty.Name.Length + 5) + 19 + 8 + (mBaseProperty.Detail.Length + 5) + 17 + ms.Length);
						Util.WriteString(writer, PropertyType);
						writer.Write('\0');
						writer.Write(Children.Count);
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
			// Count + Children
			writer.Write((Int64)Children.Count * size + 4);
			Util.WriteString(writer, PropertyType);
			writer.Write('\0');
			writer.Write(Children.Count);
			foreach (var child in Children)
			{
				child.WriteValue(writer);
			}
		}
	}
}
