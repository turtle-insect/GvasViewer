﻿namespace GvasViewer.Gvas
{
	internal class GvasNoneProperty : GvasProperty
	{
		public override object Value
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}

		public override uint Read(uint address)
		{
			return 0;
		}
	}
}
