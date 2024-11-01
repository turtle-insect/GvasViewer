namespace GvasViewer.Gvas
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
			uint length = 4;
			length += SaveData.Instance().ReadNumber(address, 4);
			//return length;

			return 11;
		}
	}
}
