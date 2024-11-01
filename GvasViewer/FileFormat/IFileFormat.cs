using GvasViewer.Gvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GvasViewer.FileFormat
{
	internal interface IFileFormat
	{
		Byte[] Load(String filename);
		void Save(String filename, Byte[] buffer);
		(GvasStructProperty property, uint length) Create(uint address, String name);
	}
}
