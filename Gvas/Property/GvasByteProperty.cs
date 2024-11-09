namespace Gvas.Property
{
	internal class GvasByteProperty : GvasProperty
	{
		public override object Value
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}

		public override uint Read(uint address)
		{
			uint length = 0;

			// name
			length += Gvas.GetString(address).length;

			// ???
			length++;

			// values
			length += Size;

			return length;
		}
	}
}
