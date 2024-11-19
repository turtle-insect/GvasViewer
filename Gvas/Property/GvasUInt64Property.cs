namespace Gvas.Property
{
	public class GvasUInt64Property : GvasProperty
	{
		public override object Value
		{
			get => SaveData.Instance().ReadNumber(Address + 1, 8);
			set
			{
				UInt128 num;
				if (!UInt128.TryParse(value.ToString(), out num)) return;
				SaveData.Instance().WriteNumber(Address + 1, 8, num);
			}
		}

		public override uint Read(uint address)
		{
			uint length = 1;

			// value [1] -> 8Byte
			length += 8;

			return length;
		}
	}
}
