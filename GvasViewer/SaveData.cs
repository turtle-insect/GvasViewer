using Gvas.Property;
using GvasViewer.FileFormat;
using GvasViewer.FileFormat.Platform;
using System.Diagnostics;
using System.IO;

namespace GvasViewer
{
	internal class SaveData
	{
		private String _fileName = String.Empty;
		private Gvas.Gvas _gvas = new();
		private IFileFormat? _fileFormat;

		public IReadOnlyList<GvasProperty>? Properties
		{
			get => _gvas.Properties;
		}

		public bool Load(String filename)
		{
			if (!File.Exists(filename)) return false;

			IFileFormat[] fileFormats =
			[
				new PlainGvas(),
				new DivisionGvas(),
				new FileFormat.Platform.Switch.BravelyDefault2(),
				new FileFormat.Platform.Switch.RomancingSaga2(),
				new DragonQuest7(Platform.Steam),
				new DragonQuest7(Platform.Switch),
				new OctopathTraveler0(),
			];

			foreach (var fileFormat in fileFormats)
			{
				try
				{
					var buffer = fileFormat.Load(filename);
					if (buffer.Length < 4) continue;
					if (System.Text.Encoding.UTF8.GetString(buffer, 0, 4) != "GVAS") continue;

					ReadGvas(buffer);
					_fileName = filename;
					_fileFormat = fileFormat;
					Backup();
					return true;
				}
				catch (DllNotFoundException)
				{
					throw;
				}
				catch { }
			}

			return false;
		}

		public void Save()
		{
			if (_fileFormat == null) return;

			var buffer = WriteGvasBuffer();
			if (buffer == null) return;

			_fileFormat.Save(_fileName, buffer);
		}

		public void SaveAs(String filename)
		{
			if (!IsAction()) return;

			_fileName = filename;
			Save();
		}

		public void Import(String filename)
		{
			if (!IsAction()) return;

			var buffer = File.ReadAllBytes(filename);
			if (buffer.Length < 4) return;
			if (System.Text.Encoding.UTF8.GetString(buffer, 0, 4) != "GVAS") return;

			ReadGvas(buffer);
		}

		public void Export(String filename)
		{
			if (!IsAction()) return;

			var buffer = WriteGvasBuffer();
			if (buffer == null) return;

			File.WriteAllBytes(filename, buffer);
		}

		public bool IsAction()
		{
			if (_fileFormat == null) return false;

			return true;
		}

		private void ReadGvas(Byte[] buffer)
		{
			using var ms = new MemoryStream(buffer);
			using var reader = new BinaryReader(ms);
			_gvas.Read(reader);
		}

		private Byte[]? WriteGvasBuffer()
		{
			if (!IsAction()) return null;

			using var ms = new MemoryStream();
			using var writer = new BinaryWriter(ms);
			_gvas.Write(writer);
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
			path = Path.Combine(path, $"{DateTime.Now:yyyy-MM-dd HH-mm-ss} {Path.GetFileName(_fileName)}");
			File.Copy(_fileName, path, true);
		}
	}
}
