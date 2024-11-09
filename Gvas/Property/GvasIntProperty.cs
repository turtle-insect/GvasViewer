﻿namespace Gvas.Property
{
	public class GvasIntProperty : GvasProperty
    {
        public override object Value
        {
            get => SaveData.Instance().ReadNumber(Address + 1, 4);
            set
            {
                uint num;
                if (!uint.TryParse(value.ToString(), out num)) return;
                SaveData.Instance().WriteNumber(Address + 1, 4, num);
            }
        }

        public override uint Read(uint address)
        {
            return 5;
        }
    }
}