namespace Gvas.Property
{
	public class GvasNameProperty : GvasProperty
	{
		public override object Value
		{
			get => Gvas.GetString(Address).name;
			set => throw new NotImplementedException();
		}

		public override uint Read(uint address)
		{
			uint length = 1;

			Address = address + length;
			var info = Gvas.GetString(address + length);
			length += info.length;

			return length;
		}
	}
}
