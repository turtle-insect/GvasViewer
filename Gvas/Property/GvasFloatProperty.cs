namespace Gvas.Property
{
	public class GvasFloatProperty : GvasProperty
	{
		public override object Value
		{
			get
			{
				var buffer = SaveData.Instance().ReadValue(Address, 4);
				return BitConverter.ToSingle(buffer);
			}
			set
			{
				float num;
				if (float.TryParse(value.ToString(), out num)) return;
				var buffer = BitConverter.GetBytes(num);
				SaveData.Instance().WriteValue(Address, buffer);
			}
		}

		public override uint Read(uint address)
		{
			uint length = 1;

			Address = address + length;
			length += 4;

			return length;
		}
	}
}
