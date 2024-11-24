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

			if (mPropertyType == "StructProperty")
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
					Children.Add(property);
				}
			}
			else
			{
				mValue = reader.ReadBytes((int)size);
			}
		}

		public override void Write(BinaryWriter writer)
		{
			Util.WriteString(writer, Name);
			Util.WriteString(writer, "ArrayProperty");
			if (mProperty is GvasStructProperty targetProperty)
			{
				using var ms = new MemoryStream();
				using var bw = new BinaryWriter(ms);
				foreach (var property in Children)
				{
					var tmp = property as GvasStructProperty;
					if (tmp == null) continue;
					tmp.WriteChild(bw);
				}

				// size
				// (Children.Count ~ ms.ToArray()).size
				writer.Write((Int64)4 + (targetProperty.Name.Length + 5) + 19 + 8 + (targetProperty.Detail.Length + 5) + 17 + ms.Length);
				Util.WriteString(writer, mPropertyType);
				writer.Write('\0');
				writer.Write(Children.Count);
				Util.WriteString(writer, targetProperty.Name);
				Util.WriteString(writer, "StructProperty");
				writer.Write(ms.Length);
				Util.WriteString(writer, targetProperty.Detail);
				writer.Write(targetProperty.GUID);
				writer.Write('\0');
				writer.Write(ms.ToArray());
			}
			else
			{
				writer.Write(mValue.LongLength);
				Util.WriteString(writer, mPropertyType);
				writer.Write('\0');
				writer.Write(mValue);
			}
		}
	}
}
