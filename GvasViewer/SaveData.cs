using Gvas.Property;
using GvasViewer.FileFormat;
using GvasViewer.FileFormat.Platform;
using System.IO;

namespace GvasViewer
{
	internal class SaveData
	{
		private String mFileName = String.Empty;
		private Gvas.Gvas? mGvas;
		private IFileFormat? mFileFormat;

		public IReadOnlyList<GvasProperty>? Properties
		{
			get => mGvas?.Properties;
		}

		public void Load(String filename)
		{
			if (!File.Exists(filename)) return;

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
					var buffer = fileFormat.Load(filename);
					if (buffer.Length < 4) continue;
					if (System.Text.Encoding.UTF8.GetString(buffer, 0, 4) != "GVAS") continue;

					mGvas = CreateGvas(buffer);
					mFileName = filename;
					mFileFormat = fileFormat;
					Backup();
					return;
				}
				catch { }
			}
		}

		public void Save()
		{
			if (mFileFormat == null) return;
			if (mGvas == null) return;

			var buffer = CreateGvasBuffer();
			if (buffer == null) return;

			mFileFormat.Save(mFileName, buffer);
		}

		public void SaveAs(String filename)
		{
			if (!IsAction()) return;

			mFileName = filename;
			Save();
		}

		public void Import(String filename)
		{
			if (File.Exists(mFileName)) return;
			if (!IsAction()) return;

			var buffer = File.ReadAllBytes(filename);
			if (buffer.Length < 4) return;
			if (System.Text.Encoding.UTF8.GetString(buffer, 0, 4) != "GVAS") return;

			mGvas = CreateGvas(buffer);
		}

		public void Export(String filename)
		{
			if (!IsAction()) return;

			var buffer = CreateGvasBuffer();
			if (buffer == null) return;

			File.WriteAllBytes(filename, buffer);
		}

		public bool IsAction()
		{
			if (mFileFormat == null) return false;
			if (mGvas == null) return false;

			return true;
		}

		private Gvas.Gvas CreateGvas(Byte[] buffer)
		{
			using var ms = new MemoryStream(buffer);
			using var reader = new BinaryReader(ms);
			var gvas = new Gvas.Gvas();
			gvas.Read(reader);
			return gvas;
		}

		private Byte[]?  CreateGvasBuffer()
		{
			if (mGvas == null) return null;

			using var ms = new MemoryStream();
			using var writer = new BinaryWriter(ms);
			mGvas.Write(writer);
			writer.Flush();

			return ms.ToArray();
		}

		private void Backup()
		{
			String path = Directory.GetCurrentDirectory();
			path = Path.Combine(path, "backup");
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			path = Path.Combine(path, $"{DateTime.Now:yyyy-MM-dd HH-mm-ss} {Path.GetFileName(mFileName)}");
			File.Copy(mFileName, path, true);
		}
	}
}
