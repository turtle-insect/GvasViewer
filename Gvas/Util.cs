using Gvas.Property;
using Gvas.Property.Standard;
using System.Text;

namespace Gvas
{
	internal class Util
	{
		public static GvasString ReadString(BinaryReader reader)
		{
			GvasString str = new();
			str.Read(reader);
			return str;
		}

		public static void WriteString(BinaryWriter writer, String value)
		{
			writer.Write(value.Length + 1);
			writer.Write(Encoding.UTF8.GetBytes(value));
			writer.Write('\0');
		}

		public static GvasProperty ReadProperty(BinaryReader reader)
		{
			// name
			var propertyName = ReadString(reader);
			if (propertyName.Value == "None")
			{
				return new GvasNoneProperty();
			}

			var propertyType = ReadString(reader);
			var property = CreateProperty(propertyType);

			property.Name = propertyName;
			return property;
		}

		public static GvasProperty CreateProperty(GvasString propertyType)
		{
			return propertyType.Value switch
			{
				"BoolProperty" => new GvasBoolProperty(),
				"ByteProperty" => new GvasByteProperty(),
				"IntProperty" => new GvasIntProperty(),
				"UInt32Property" => new GvasUInt32Property(),
				"Int64Property" => new GvasInt64Property(),
				"UInt64Property" => new GvasUInt64Property(),
				"FloatProperty" => new GvasFloatProperty(),
				"DoubleProperty" => new GvasDoubleProperty(),
				"TextProperty" => new GvasTextProperty(),
				"StrProperty" => new GvasStrProperty(),
				"NameProperty" => new GvasNameProperty(),
				"EnumProperty" => new GvasEnumProperty(),
				"ArrayProperty" => new GvasArrayProperty(),
				"SetProperty" => new GvasSetProperty(),
				"MapProperty" => new GvasMapProperty(),
				"StructProperty" => new GvasStructProperty(),
				"ObjectProperty" => new GvasObjectProperty(),
				"SoftObjectProperty" => new GvasSoftObjectProperty(),
				_ => throw new NotImplementedException(),
			};
		}
	}
}
