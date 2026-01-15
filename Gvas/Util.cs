using Gvas.Property;
using Gvas.Property.Standard;
using System.Text;

namespace Gvas
{
	internal class Util
	{
		public static String ReadString(BinaryReader reader)
		{
			int length = reader.ReadInt32();
			if (length <= 0)
			{
				reader.BaseStream.Position -= 4;
				throw new ArgumentException();
			}

			var buffer = reader.ReadBytes(length - 1);
			reader.ReadByte();
			return Encoding.UTF8.GetString(buffer);
		}

		public static void WriteString(BinaryWriter writer, String value)
		{
			writer.Write(value.Length + 1);
			writer.Write(Encoding.UTF8.GetBytes(value));
			writer.Write('\0');
		}

		public static GvasProperty Read(BinaryReader reader)
		{
			// name
			var propertyName = ReadString(reader);

			GvasProperty property = new GvasNoneProperty();

			if (propertyName == "None")
			{
				property.Name = propertyName;
				return property;
			}

			var propertyType = ReadString(reader);
			property = propertyType switch
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

			property.Name = propertyName;
			property.Read(reader);
			return property;
		}
	}
}
