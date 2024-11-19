using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gvas.Property
{
	internal class GvasObjectProperty : GvasProperty
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

			return length;
		}
	}
}
