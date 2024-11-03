namespace GvasViewer.Gvas
{
	class GvasArrayProperty : GvasProperty
	{
		public override object Value
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}
		public override uint Read(uint address)
		{
			uint length = 0;

			// type
			var propType = Gvas.GetString(address + length);
			length += propType.length;

			// ???
			length++;

			// array's count
			uint count = SaveData.Instance().ReadNumber(address + length, 4);
			length += 4;

			switch (propType.name)
			{
				case "BoolProperty":
				case "ByteProperty":
					length += count;
					break;

				case "IntProperty":
					length += count * 4;
					break;

				case "NameProperty":
				case "EnumProperty":
					for (uint i = 0; i < count; i++)
					{
						var propValue = Gvas.GetString(address + length);
						length += propValue.length;
					}
					break;

				case "StructProperty":
					// name
					var propName = Gvas.GetString(address + length);
					length += propName.length;

					// type
					// StructProperty
					propType = Gvas.GetString(address + length);
					length += propType.length;

					// size
					length += 8;

					// name
					propName = Gvas.GetString(address + length);
					length += propName.length;

					// ???
					length += 17;

					for (uint i = 0; i < count; i++)
					{
						var property = new GvasStructProperty();
						length += property.Create(address + length, propName.name);
						Children.Add(property);
					}
					break;

				default:
					throw new NotImplementedException();
			}

			return length;
		}
	}
}
