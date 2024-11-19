namespace Gvas.Property
{
	internal class GvasSoftObjectProperty : GvasProperty
	{
		public override object Value
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}

		public override uint Read(uint address)
		{
			uint length = 1;
			var info = Gvas.GetString(address + length);
			length += info.length;
			length += 4;

			return length;
		}
	}
}
