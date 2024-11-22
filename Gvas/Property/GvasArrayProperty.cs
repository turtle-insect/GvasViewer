namespace Gvas.Property
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
			uint count = (uint)SaveData.Instance().ReadNumber(address + length, 4);
			length += 4;

			switch (propType.name)
			{
				case "BoolProperty":
					length += count;
					break;

				case "ByteProperty":
					// The contents of ByteProperty can be freely defined.
					// Some games may include the GVAS format in the ByteProperty
					length += Size - 4;
					break;

				case "IntProperty":
					for (uint i = 0; i < count; i++)
					{
						var property = new GvasIntProperty() { Name = $"[{i}]", Address = address + length };
						Children.Add(property);
						length += 4;
					}
					break;

				case "Int64Property":
					for (uint i = 0; i < count; i++)
					{
						var property = new GvasInt64Property() { Name = $"[{i}]", Address = address + length };
						Children.Add(property);
						length += 8;
					}
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
						length += property.ReadEntity(address + length, propName.name);
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
