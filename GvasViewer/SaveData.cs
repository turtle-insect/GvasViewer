using Gvas;
using Gvas.Property;
using Gvas.Property.Standard;
using GvasViewer.FileFormat;
using GvasViewer.FileFormat.Platform;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GvasViewer
{
	internal class SaveData
	{
		private String mFileName;
		private Gvas.Gvas? mGvas;
		private IFileFormat? mFileFormat;

		public SaveData(String filename)
		{
			mFileName = filename;
		}

		public void Load()
		{
			if (!System.IO.File.Exists(mFileName)) return;

			IFileFormat[] fileFormats =
			[
				new PlainGvas(),
				new DivisionGvas(),
				new FileFormat.Platform.Switch.BravelyDefault2(),
				new FileFormat.Platform.Switch.RomancingSaga2(),
				new DragonQuest7(Platform.Steam),
				new DragonQuest7(Platform.Switch),
			];

			foreach (var fileFormat in fileFormats)
			{
				try
				{
					var buffer = fileFormat.Load(mFileName);
					if (buffer.Length < 4) continue;
					if (System.Text.Encoding.UTF8.GetString(buffer, 0, 4) != "GVAS") continue;

					mFileFormat = fileFormat;
					mGvas = ReadGvas(buffer);
					return;
				}
				catch { }
			}
		}

		public void Save()
		{
			if (mFileFormat == null) return;
			if (mGvas == null) return;

			using var ms = new MemoryStream();
			using var writer = new BinaryWriter(ms);
			mGvas.Write(writer);
			writer.Flush();

			File.WriteAllBytes(mFileName, ms.ToArray());
		}

		public void SaveAs(String filename)
		{
			mFileName = filename;
			Save();
		}

		public void Import(String filename)
		{
			var buffer = File.ReadAllBytes(filename);
			if (buffer.Length < 4) return;
			if (System.Text.Encoding.UTF8.GetString(buffer, 0, 4) != "GVAS") return;
		}

		public void Export(String filename)
		{

		}

		public bool IsAction()
		{
			if (mFileFormat == null) return false;
			if (mGvas == null) return false;

			return true;
		}

		private Gvas.Gvas ReadGvas(Byte[] buffer)
		{
			using var ms = new MemoryStream(buffer);
			using var reader = new BinaryReader(ms);
			var gvas = new Gvas.Gvas();
			gvas.Read(reader);
			return gvas;
		}
	}
}
