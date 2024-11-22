namespace Gvas.Property
{
	public class GvasStructProperty : GvasProperty
	{
		public override object Value
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}

		public override uint Read(uint address)
		{
			uint length = 0;

			var propName = Gvas.GetString(address + length);
			length += propName.length;

			// ???
			length += 17;

			length += ReadEntity(address + length, propName.name);
			return length;
		}

		public uint ReadEntity(uint address, string name)
		{
			Address = address;
			Name = name;
			uint length = 0;

			// if you make unique struct
			// extends IFileFormat implementation
			//   Create(uint address, String name)
			// function
			var fileFormat = SaveData.Instance().FileFormat;
			if (fileFormat != null)
			{
				length = fileFormat.Create(this, address + length, name);
				if (length != 0) return length;
			}

			switch (name)
			{
				// Date & Time
				case "Timespan":
				case "DateTime":
					{
						var property = new GvasUInt64Property() { Name = "Time", Address = address + length };
						Children.Add(property);
						length += 8;
					}
					break;

				// Vector
				case "Vector2D":
					{
						String[] names = { "X", "Y" };
						foreach(var n in names)
						{
							var property = new GvasIntProperty() { Name = n, Address = address + length };
							Children.Add(property);
							length += 4;
						}
					}
					break;
				case "Vector":
				case "Rotator":
					{
						String[] names = { "X", "Y", "Z" };
						foreach (var n in names)
						{
							var property = new GvasIntProperty() { Name = n, Address = address + length };
							Children.Add(property);
							length += 4;
						}
					}
					break;
				// Quaternion
				case "Quat":
					{
						String[] names = { "X", "Y", "Z", "W" };
						foreach (var n in names)
						{
							var property = new GvasIntProperty() { Name = n, Address = address + length };
							Children.Add(property);
							length += 4;
						}
					}
					break;

				// Color
				case "Color":
					{
						var property = new GvasIntProperty() { Name = name, Address = address + length };
						Children.Add(property);
						length += 4;
					}
					break;

				case "LinearColor":
					{
						String[] names = { "A", "R", "G", "B" };
						foreach (var n in names)
						{
							var property = new GvasIntProperty() { Name = n, Address = address + length };
							Children.Add(property);
							length += 4;
						}
					}
					break;

				default:
					for (; ; )
					{
						var info = Gvas.Read(address + length);
						length += info.length;
						Children.Add(info.property);
						if (info.property is GvasNoneProperty) break;
					}
					break;
			}

			return length;
		}
	}
}
