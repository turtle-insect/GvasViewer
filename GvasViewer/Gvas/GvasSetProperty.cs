namespace GvasViewer.Gvas
{
	internal class GvasSetProperty : GvasProperty
	{
		public override object Value
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}

		public override uint Read(uint address)
		{
			uint length = 0;

			var propType = Gvas.GetString(address + length);
			length += propType.length;

			// value
			length += Size + 1;

			return length;
		}
	}
}
