﻿namespace GvasViewer.Gvas
{
	internal class GvasUInt32Property : GvasProperty
	{
		public override Object Value
		{
			get => SaveData.Instance().ReadNumber(mAddress + 1, 4);
			set
			{
				uint num;
				if (!uint.TryParse(value.ToString(), out num)) return;
				SaveData.Instance().WriteNumber(mAddress + 1, 4, num);
			}
		}

		public override uint Read(uint address)
		{
			return 5;
		}
	}
}
