using GvasViewer.Gvas;

namespace GvasViewer.FileFormat
{
	internal interface IFileFormat
	{
		Byte[] Load(String filename);
		void Save(String filename, Byte[] buffer);
		(GvasStructProperty property, uint length) Create(uint address, String name);
	}
}
