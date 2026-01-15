namespace GvasViewer.FileFormat.Util
{
	internal class Crc32
	{
		private readonly UInt32[] mTable;

		public Crc32(UInt32 seed)
		{
			mTable = new UInt32[256];
			for (UInt32 i = 0; i < mTable.Length; i++)
			{
				var x = i;
				for (var j = 0; j < 8; j++)
				{
					x = (x & 1) == 0 ? x >> 1 : seed ^ x >> 1;
				}
				mTable[i] = x;
			}
		}

		public UInt32 Calc(Byte[] buf, Int32 offset, Int32 length)
		{
			var num = UInt32.MaxValue;
			for (var i = 0; i < length; i++)
			{
				num = mTable[(num ^ buf[offset + i]) & 255] ^ num >> 8;
			}

			return num ^ UInt32.MaxValue;
		}
	}
}
