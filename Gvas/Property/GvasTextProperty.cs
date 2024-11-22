namespace Gvas.Property
{
	public class GvasTextProperty : GvasProperty
	{
		public override object Value
		{
			get => Gvas.GetString(Address).name;
			set => throw new NotImplementedException();
		}

		public override uint Read(uint address)
		{
			uint length = 6;

			var propName = Gvas.GetString(address + length);
			length += propName.length;
			Address = address + length;

			return Size + 1;
		}
	}
}
