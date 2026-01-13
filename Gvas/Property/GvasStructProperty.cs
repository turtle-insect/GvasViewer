namespace Gvas.Property
{
	public class GvasStructProperty : GvasProperty
	{
		public String Detail { get; set; } = String.Empty;
		public Byte[] GUID { get; set; } = [];
		public override object Value
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}

		public GvasStructProperty()
			: base()
		{ }

		public GvasStructProperty(GvasStructProperty property)
			: base(property)
		{
			Detail = property.Detail;
			// not GUID = property.GUID.ToArray();
			GUID = System.Guid.NewGuid().ToByteArray();
		}

		public override GvasProperty Clone()
		{
			return new GvasStructProperty(this);
		}

		public override void Read(BinaryReader reader)
		{
			var size = reader.ReadUInt64();

			// name
			Detail = Util.ReadString(reader);

			GUID = reader.ReadBytes(16);
			// ???
			reader.ReadByte();

			ReadChild(reader, Detail);
		}

		public void ReadChild(BinaryReader reader, String propertyName)
		{
			switch (propertyName)
			{
				// Date & Time
				case "Timespan":
				case "DateTime":
					{
						var property = new Custom.GvasLiteralProperty();
						property.Read(reader, 8);
						Childrens.Add(property);
					}
					break;

				// Vector
				case "Vector2D":
					{
						var property = new Custom.GvasLiteralProperty();
						property.Read(reader, 8);
						Childrens.Add(property);
					}
					break;
				case "Vector":
				case "Rotator":
					{
						var property = new Custom.GvasLiteralProperty();
						property.Read(reader, 12);
						Childrens.Add(property);
					}
					break;
				// Quaternion
				case "Quat":
					{
						var property = new Custom.GvasLiteralProperty();
						property.Read(reader, 16);
						Childrens.Add(property);
					}
					break;

				// Color
				case "Color":
					{
						var property = new Custom.GvasLiteralProperty();
						property.Read(reader, 4);
						Childrens.Add(property);
					}
					break;
				case "LinearColor":
					{
						var property = new Custom.GvasLiteralProperty();
						property.Read(reader, 16);
						Childrens.Add(property);
					}
					break;

				// Dragon Quest VII Reimagined
				case "BP_GameInstance_C#SaveLoadAdventureLogStruct":
					{
						var property = new Custom.GvasDQ7MemoryProperty();
						property.Read(reader);
						Childrens.Add(property);
						Childrens.Add(Util.Read(reader));
					}
					break;

				default:
					for (; ; )
					{
						var property = Util.Read(reader);
						Childrens.Add(property);
						if (property is GvasNoneProperty) break;
					}
					break;
			}
		}

		public override void Write(BinaryWriter writer)
		{
			Util.WriteString(writer, Name);
			Util.WriteString(writer, "StructProperty");

			using var ms = new MemoryStream();
			using var bw = new BinaryWriter(ms);
			WriteValue(bw);
			bw.Flush();

			writer.Write(ms.Length);
			Util.WriteString(writer, Detail);
			writer.Write(GUID);
			writer.Write('\0');
			writer.Write(ms.ToArray());
		}

		public override void WriteValue(BinaryWriter writer)
		{
			foreach (var children in Childrens)
			{
				children.Write(writer);
			}
		}
	}
}
