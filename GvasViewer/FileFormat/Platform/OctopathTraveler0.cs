using GvasViewer.FileFormat.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace GvasViewer.FileFormat.Platform
{
	internal class OctopathTraveler0 : IFileFormat
	{
		public Byte[] Load(String filename)
		{
			Byte[] buffer = System.IO.File.ReadAllBytes(filename);
			var length = BitConverter.ToInt32(buffer);
			buffer = buffer[8..];

			Oodle oodle = new();
			return oodle.Decompress(buffer, length);
		}

		public void Save(String filename, Byte[] buffer)
		{
			Oodle oodle = new();
			buffer = [
				.. BitConverter.GetBytes(buffer.Length),
				.. BitConverter.GetBytes(0),
				.. oodle.Compress(buffer),
			];

			System.IO.File.WriteAllBytes(filename, buffer);
		}
	}
}
