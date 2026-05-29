namespace Gvas.Property.v1.Standard
{
	public class GvasStructProperty : GvasProperty
	{
		public GvasString Detail { get; set; } = new();
		public Byte[] GUID { get; set; } = [];

		private UInt64 _size;

		public GvasStructProperty()
			: base()
		{ }

		public GvasStructProperty(GvasStructProperty property)
			: base(property)
		{
			Detail = new(property.Detail);
			// not GUID = property.GUID.ToArray();
			GUID = System.Guid.NewGuid().ToByteArray();
			_size = property._size;
		}

		public override GvasProperty Clone()
		{
			return new GvasStructProperty(this);
		}

		public override object Value
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}

		public override void Read(BinaryReader reader)
		{
			_size = reader.ReadUInt64();

			// name
			Detail = Util.ReadString(reader);

			GUID = reader.ReadBytes(16);
			// ???
			reader.ReadByte();

			ReadChild(reader, Detail);
		}

		public void ReadChild(BinaryReader reader, GvasString propertyName)
		{
			switch (propertyName.Value)
			{
				// Date & Time
				case "Timespan":
				case "DateTime":

				// Vector
				case "Vector2D":
				case "Vector":
				case "Rotator":

				// Quaternion
				case "Quat":

				// Color
				case "Color":
				case "LinearColor":

				// Guid
				case "Guid":
					{
						var property = new GvasLiteralProperty();
						property.Read(reader, (int)_size);
						AppendChildren(property);
					}
					break;

				// Dragon Quest VII Reimagined
				case "BP_GameInstance_C#SaveLoadAdventureLogStruct":
					{
						var property = new Custom.GvasDQ7MemoryProperty();
						property.Read(reader);
						AppendChildren(property);
						var child = Util.ReadProperty(reader);
						child.Read(reader);
						AppendChildren(child);
					}
					break;

				default:
					ReadValue(reader);
					break;
			}
		}

		public override void Write(BinaryWriter writer)
		{
			Name.Write(writer);
			Util.WriteString(writer, "StructProperty");

			using var ms = new MemoryStream();
			using var bw = new BinaryWriter(ms);
			WriteValue(bw);
			bw.Flush();

			writer.Write(ms.Length);
			Detail.Write(writer);
			writer.Write(GUID);
			writer.Write('\0');
			writer.Write(ms.ToArray());
		}

		public override void ReadValue(BinaryReader reader)
		{
			for (; ; )
			{
				var property = Util.ReadProperty(reader);
				property.Read(reader);
				AppendChildren(property);
				if (property is GvasNoneProperty) break;
			}
		}

		public override void WriteValue(BinaryWriter writer)
		{
			foreach (var child in Children)
			{
				child.Write(writer);
			}
		}
	}
}
