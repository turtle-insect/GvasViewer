namespace GvasViewer.Gvas
{
	class GvasStructProperty : GvasProperty
	{
		public List<GvasProperty> Properties { get; private set; } = new List<GvasProperty>();
		public override object Value
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}

		public override uint Read(uint address)
		{
			var info = Create(address, 1);

			Properties = info.properties[0].Properties;
			return info.length;
		}

		public static (List<GvasStructProperty> properties, uint length) Create(uint address, uint count)
		{
			uint length = 0;
			var properties = new List<GvasStructProperty>();

			var propName = Gvas.GetString(address + length);
			length += propName.length;

			// ???
			length += 17;

			for (uint i = 0; i < count; i++)
			{
				var info = Create(address + length, propName.name);
				properties.Add(info.property);
				length += info.length;
			}

			return (properties, length);
		}

		public static (GvasStructProperty property, uint length) Create(uint address, String name)
		{
			var property = new GvasStructProperty();
			property.Address = address;
			property.Name = name;
			uint length = 0;

			// if you make unique struct
			// extends IFileFormat implementation
			//   Create(uint address, String name)
			// function
			var fileFormat = SaveData.Instance().FileFormat;
			if(fileFormat != null)
			{
				var uniqueStruct = fileFormat.Create(address + length, name);
				if (uniqueStruct.length != 0) return uniqueStruct;
			}

			switch (name)
			{
				case "DateTime":
				case "Vector2D":
					length += 8;
					break;

				case "Color":
					length += 4;
					break;

				case "Vector":
					length += 3 * 4;
					break;

				default:
					for (; ; )
					{
						var info = Gvas.Read(address + length);
						property.Properties.Add(info.property);
						length += info.length;
						if (info.property is GvasNoneProperty) break;
					}
					break;
			}

			return (property, length);
		}
	}
}
