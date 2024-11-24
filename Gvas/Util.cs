using Gvas.Property;
using System.Diagnostics;
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
			switch (propertyType)
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

				case "ObjectProperty":
					property = new GvasObjectProperty();
					break;

				case "SoftObjectProperty":
					property = new GvasSoftObjectProperty();
					break;

				default:
					throw new NotImplementedException();
			}

			property.Name = propertyName;
			property.Read(reader);
			return property;
		}
	}
}
