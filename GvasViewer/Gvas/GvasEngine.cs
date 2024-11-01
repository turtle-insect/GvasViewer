namespace GvasViewer.Gvas
{
	internal class GvasEngine
	{
		public uint Read()
		{
			uint address = 0;

			// header [0] -> 4Byte

			// save version [4] -> 4Byte
			uint version = SaveData.Instance().ReadNumber(4, 4);

			// engine name's length [*]-> length
			if(version == 2) address = 22;
			else if (version == 3) address = 26;

			// name
			var propName = Gvas.GetString(address);
			address += propName.length;

			// ???
			address += 4;

			var count = SaveData.Instance().ReadNumber(address, 4);
			address += 4;

			// ??? block info
			// one block's size => 20Byte
			address += count * 20;

			var propDate = Gvas.GetString(address);
			address += propDate.length;

			return address;
		}
	}
}
