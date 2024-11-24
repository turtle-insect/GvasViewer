namespace GvasViewer.FileFormat
{
	internal interface IFileFormat
	{
		Byte[] Load(String filename);
		void Save(String filename, Byte[] buffer);
	}
}
