namespace Gvas.Property
{
	public class GvasInt64Property : GvasProperty
	{
		public override object Value
		{
			get => SaveData.Instance().ReadNumber(Address, 8);
			set
			{
				UInt128 num;
				if (!UInt128.TryParse(value.ToString(), out num)) return;
				SaveData.Instance().WriteNumber(Address, 8, num);
			}
		}

		public override uint Read(uint address)
		{
			uint length = 1;

			Address = address + length;
			length += 8;

			return length;
		}
	}
}
