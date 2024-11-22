namespace Gvas.Property
{
	public class GvasUInt32Property : GvasProperty
	{
		public override object Value
		{
			get => SaveData.Instance().ReadNumber(Address, 4);
			set
			{
				uint num;
				if (!uint.TryParse(value.ToString(), out num)) return;
				SaveData.Instance().WriteNumber(Address, 4, num);
			}
		}

		public override uint Read(uint address)
		{
			Address = address + 1;
			return 5;
		}
	}
}
