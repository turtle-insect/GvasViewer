using GvasViewer.Gvas.Property;

namespace GvasViewer.Gvas
{
    internal class Gvas
	{
		public static List<GvasProperty> Read()
		{
			var engine = new GvasEngine();
			var address = engine.Read();
			var properties = new List<GvasProperty>();

			for (; ; )
			{
				var info = Read(address);
				address += info.length;
				properties.Add(info.property);
				if (info.property is GvasNoneProperty) break;
			}

			return properties;
		}

		public static (GvasProperty property, uint length) Read(uint address)
		{
			uint length = 0;

			// name
			var propName = GetString(address);
			length += propName.length;

			GvasProperty property = new GvasNoneProperty();

			if (propName.name == "None")
			{
				return (property, length);
			}

			// type
			var propType = GetString(address + length);
			length += propType.length;

			uint size = SaveData.Instance().ReadNumber(address + length, 4);
			length += 8;

			switch (propType.name)
			{
				case "BoolProperty":
					property = new GvasBoolProperty();
					break;

				case "ByteProperty":
					property = new GvasByteProperty();
					break;

				case "IntProperty":
					property = new GvasIntProperty();
					break;

				case "UInt32Property":
					property = new GvasUInt32Property();
					break;

				case "Int64Property":
					property = new GvasInt64Property();
					break;

				case "UInt64Property":
					property = new GvasUInt64Property();
					break;

				case "FloatProperty":
					property = new GvasFloatProperty();
					break;

				case "TextProperty":
					property = new GvasTextProperty();
					break;

				case "StrProperty":
					property = new GvasStrProperty();
					break;

				case "NameProperty":
					property = new GvasNameProperty();
					break;

				case "EnumProperty":
					property = new GvasEnumProperty();
					break;

				case "ArrayProperty":
					property = new GvasArrayProperty();
					break;

				case "SetProperty":
					property = new GvasSetProperty();
					break;

				case "MapProperty":
					property = new GvasMapProperty();
					break;

				case "StructProperty":
					property = new GvasStructProperty();
					break;

				case "SoftObjectProperty":
					property = new GvasSoftObjectProperty();
					break;

				default:
					throw new NotImplementedException();
			}

			property.Address = address + length;
			property.Name = propName.name;
			property.Size = size;
			length += property.Read(address + length);
			return (property, length);
		}

		public static (String name, uint length) GetString(uint address)
		{
			uint length = SaveData.Instance().ReadNumber(address, 4);
			if ((length & 0x80000000) == 0x80000000)
			{
				length = uint.MaxValue - length + 1;
				length *= 2;
			}
			String name = SaveData.Instance().ReadText(address + 4, length);
			return (name, length + 4);
		}
	}
}
