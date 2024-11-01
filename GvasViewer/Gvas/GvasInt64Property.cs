using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GvasViewer.Gvas
{
	internal class GvasInt64Property : GvasProperty
	{
		public override object Value
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}

		public override uint Read(uint address)
		{
			uint length = 1;

			// value [1] -> 8Byte
			length += 8;

			return length;
		}
	}
}
