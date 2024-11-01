using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GvasViewer.Gvas
{
	internal class GvasNameProperty : GvasProperty
	{
		public override object Value
		{
			get => Gvas.GetString(mAddress + 1).name;
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
