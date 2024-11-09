namespace Gvas.Property
{
	internal class GvasMapProperty : GvasProperty
	{
		public override object Value
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}

		public override uint Read(uint address)
		{
			uint length = 0;
			var propKey = Gvas.GetString(address);
			length += propKey.length;

			var propType = Gvas.GetString(address + length);
			length += propType.length;

			// ???
			length += 5;

			uint count = SaveData.Instance().ReadNumber(address + length, 4);
			length += 4;

			for (uint i = 0; i < count; i++)
			{
				// key
				length += CreateProperty(propKey.name, address + length);

				// value
				length += CreateProperty(propType.name, address + length);
			}

			return length;
		}

		private uint CreateProperty(string name, uint address)
		{
			uint length = 0;
			switch (name)
			{
				case "BoolProperty":
				case "ByteProperty":
				case "Int8Property":
					length += 1;
					break;

				case "IntProperty":
					length += 4;
					break;

				case "TextProperty":
					length += 9;
					break;

				case "StrProperty":
				case "NameProperty":
				case "EnumProperty":
					var key = Gvas.GetString(address + length);
					length += key.length;
					break;

				case "StructProperty":
					var property = new GvasStructProperty();
					length += property.ReadEntity(address + length, "");
					break;

				default:
					throw new NotImplementedException();
			}

			return length;
		}
	}
}
