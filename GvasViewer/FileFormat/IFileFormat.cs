using GvasViewer.Gvas.Property;

namespace GvasViewer.FileFormat
{
    internal interface IFileFormat
	{
		Byte[] Load(String filename);
		void Save(String filename, Byte[] buffer);
		uint Create(GvasStructProperty property, uint address, String name);
	}
}
