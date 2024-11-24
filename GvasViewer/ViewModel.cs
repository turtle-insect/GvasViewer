using GvasViewer.FileFormat;
using GvasViewer.FileFormat.Switch;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace GvasViewer
{
	class ViewModel
    {
		public ICommand CommandFileOpen { get; init; }
		public ICommand CommandFileSave { get; init; }

		public ObservableCollection<Gvas.Property.GvasProperty> GvasProperties { get; set; } = new();

		private IFileFormat? mFileFormat;
		private Gvas.Gvas mGvas = new();

		public ViewModel()
		{
			CommandFileOpen = new ActionCommand(FileOpen);
			CommandFileSave = new ActionCommand(FileSave);
		}

		private void FileOpen(Object? parameter)
		{
			var dlg = new OpenFileDialog();
			if (dlg.ShowDialog() == false) return;

			IFileFormat[] fileFormats = 
			[
				new PlainGvas(),
				new DivisionGvas(),
				new BravelyDefault2(),
				new RomancingSaga2(),
			];

			GvasProperties.Clear();
			mFileFormat = null;

			String filename = dlg.FileName;
			Byte[] buffer = [];

			foreach (var fileFormat in fileFormats)
			{
				try
				{
					var tmp = fileFormat.Load(filename);
					if (tmp.Length > 4 && Encoding.UTF8.GetString(tmp[..4].ToArray()) == "GVAS")
					{
						buffer = tmp;
						mFileFormat = fileFormat;
						break;
					}
				}
				catch { }
			}

			if(buffer.Length < 4 || mFileFormat == null)
			{
				MessageBox.Show("not gvas format");
				return;
			}

			using var ms = new MemoryStream(buffer);
			using var reader = new BinaryReader(ms);
			mGvas.Read(reader);

			try
			{
				foreach (var property in mGvas.Properties)
				{
					GvasProperties.Add(property);
				}
			}
			catch
			{
				mFileFormat = null;
				MessageBox.Show("not support game");
			}
		}

		private void FileSave(Object? parameter)
		{
			if (mFileFormat == null) return;

			var dlg = new SaveFileDialog();
			if (dlg.ShowDialog() == false) return;

			using var ms = new MemoryStream();
			using var writer = new BinaryWriter(ms);
			mGvas.Write(writer);

			mFileFormat.Save(dlg.FileName, ms.ToArray());
		}
	}
}
