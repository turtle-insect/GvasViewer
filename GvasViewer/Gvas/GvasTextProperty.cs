namespace GvasViewer.Gvas
{
	internal class GvasTextProperty : GvasProperty
	{
		public override object Value
		{
			get
			{
				uint address = mAddress;
				uint length = 6;

				// key
				var propKey = Gvas.GetString(address + length);

				// 1Byte length ???
				if (propKey.length == 1) return "";
				length += propKey.length;

				// value
				var propValue = Gvas.GetString(address + length);
				return propValue.name;
			}
			set => throw new NotImplementedException();
		}

		public override uint Read(uint address)
		{
			uint length = 1;
			length += Size;

			return length;
		}
	}
}
