using GvasViewer.FileFormat.Switch;
using GvasViewer.FileFormat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GvasViewer
{
	internal class Setting
	{
		public List<FileFormatInfo> FileFormatInfos { get; private set; } = new List<FileFormatInfo>
		{
			new FileFormatInfo() { Name = "Plain GVAS" },
			new FileFormatInfo() { Name = "Romancing Saga 2", Format = new RomancingSaga2() },
			new FileFormatInfo() { Name = "Pikmin 4", Format = new DivisionGvas() },
			new FileFormatInfo() { Name = "Dragon Quest Treasures", Format = new DivisionGvas() },
		};

		public int FileFormatIndex { get; set; } = 0;

		public void Initialize()
		{
			FileFormatIndex = 0;
		}

		public FileFormatInfo FileFormat
		{
			get => FileFormatInfos[FileFormatIndex];
		}
	}

	class FileFormatInfo
	{
		public String Name { get; init; } = String.Empty;
		public IFileFormat Format { get; init; } = new PlainGvas();
	}
}
