using Gvas.Property;
using System.Text;

namespace Gvas
{
	internal class Util
	{
		public static uint GvasVersion { get; set; } = 1;

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
			return CreateProperty(propertyType, GvasVersion);
		}

		private static GvasProperty CreateProperty(GvasString propertyType, uint version)
		{
			switch(version)
			{
				case 1:
					return propertyType.Value switch
					{
						"BoolProperty" => new Property.v1.Standard.GvasBoolProperty(),
						"ByteProperty" => new Property.v1.Standard.GvasByteProperty(),
						"Int8Property" => new Property.v1.Standard.GvasInt8Property(),
						"IntProperty" => new Property.v1.Standard.GvasIntProperty(),
						"UInt32Property" => new Property.v1.Standard.GvasUInt32Property(),
						"Int64Property" => new Property.v1.Standard.GvasInt64Property(),
						"UInt64Property" => new Property.v1.Standard.GvasUInt64Property(),
						"FloatProperty" => new Property.v1.Standard.GvasFloatProperty(),
						"DoubleProperty" => new Property.v1.Standard.GvasDoubleProperty(),
						"TextProperty" => new Property.v1.Standard.GvasTextProperty(),
						"StrProperty" => new Property.v1.Standard.GvasStrProperty(),
						"NameProperty" => new Property.v1.Standard.GvasNameProperty(),
						"EnumProperty" => new Property.v1.Standard.GvasEnumProperty(),
						"ArrayProperty" => new Property.v1.Standard.GvasArrayProperty(),
						"SetProperty" => new Property.v1.Standard.GvasSetProperty(),
						"MapProperty" => new Property.v1.Standard.GvasMapProperty(),
						"StructProperty" => new Property.v1.Standard.GvasStructProperty(),
						"ObjectProperty" => new Property.v1.Standard.GvasObjectProperty(),
						"SoftObjectProperty" => new Property.v1.Standard.GvasSoftObjectProperty(),
						_ => throw new NotImplementedException(),
					};

				case 2:
					return propertyType.Value switch
					{
						"BoolProperty" => new Property.v2.Standard.GvasBoolProperty(),
						"ByteProperty" => new Property.v2.Standard.GvasByteProperty(),
						"Int8Property" => new Property.v2.Standard.GvasInt8Property(),
						"IntProperty" => new Property.v2.Standard.GvasIntProperty(),
						"UInt32Property" => new Property.v2.Standard.GvasUInt32Property(),
						"Int64Property" => new Property.v2.Standard.GvasInt64Property(),
						"UInt64Property" => new Property.v2.Standard.GvasUInt64Property(),
						"FloatProperty" => new Property.v2.Standard.GvasFloatProperty(),
						"DoubleProperty" => new Property.v2.Standard.GvasDoubleProperty(),
						"TextProperty" => new Property.v2.Standard.GvasTextProperty(),
						"StrProperty" => new Property.v2.Standard.GvasStrProperty(),
						"NameProperty" => new Property.v2.Standard.GvasNameProperty(),
						"EnumProperty" => new Property.v2.Standard.GvasEnumProperty(),
						"ArrayProperty" => new Property.v2.Standard.GvasArrayProperty(),
						"SetProperty" => new Property.v2.Standard.GvasSetProperty(),
						"MapProperty" => new Property.v2.Standard.GvasMapProperty(),
						"StructProperty" => new Property.v2.Standard.GvasStructProperty(),
						"ObjectProperty" => new Property.v2.Standard.GvasObjectProperty(),
						"SoftObjectProperty" => new Property.v2.Standard.GvasSoftObjectProperty(),
						_ => throw new NotImplementedException(),
					};

				default:
					throw new NotImplementedException();
			}
		}
	}
}
