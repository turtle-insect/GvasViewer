namespace Gvas.Property
{
	public class GvasStrProperty : GvasProperty
	{
		public override object Value
		{
			get => Gvas.GetString(Address + 1).name;
			set => throw new NotImplementedException();
		}

		public override uint Read(uint address)
		{
			uint length = 1;
			var info = Gvas.GetString(address + length);
			length += info.length;

			return length;
		}
	}
}
