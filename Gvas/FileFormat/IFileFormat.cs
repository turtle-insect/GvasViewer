using Gvas.Property;

namespace Gvas.FileFormat
{
	public interface IFileFormat
	{
		Byte[] Load(String filename);
		void Save(String filename, Byte[] buffer);
		uint Create(GvasStructProperty property, uint address, String name);
	}
}
