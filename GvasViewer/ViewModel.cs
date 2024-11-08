using GvasViwer.FileFormat;
using GvasViwer.FileFormat.Switch;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace GvasViewer
{
	internal class ViewModel
	{
		public Gvas.SaveData SaveData { get; init; } = Gvas.SaveData.Instance();

		public ICommand CommandFileOpen { get; init; }
		public ICommand CommandFileSave { get; init; }
		public ICommand CommandFileImport { get; init; }
		public ICommand CommandFileExport { get; init; }

		public IList<Gvas.Property.GvasProperty> GvasProperties { get; set; } = new ObservableCollection<Gvas.Property.GvasProperty>();

		public ViewModel()
		{
			CommandFileOpen = new ActionCommand(FileOpen);
			CommandFileSave = new ActionCommand(FileSave);
			CommandFileImport = new ActionCommand(FileImport);
			CommandFileExport = new ActionCommand(FileExport);

			var fileFormat = Gvas.SaveData.Instance().mFormats;
			fileFormat.Clear();
			fileFormat.Add(new PlainGvas());
			fileFormat.Add(new DivisionGvas());
			fileFormat.Add(new BravelyDefault2());
			fileFormat.Add(new RomancingSaga2());
		}

		private void FileOpen(Object? parameter)
		{
			var dlg = new OpenFileDialog();
			if (dlg.ShowDialog() == false) return;

			if (SaveData.Open(dlg.FileName) == false)
			{
				MessageBox.Show("not gvas format");
				return;
			}

			try
			{
				GvasProperties.Clear();
				foreach (var property in Gvas.Gvas.Read())
				{
					GvasProperties.Add(property);
				} 
			}
			catch
			{
				MessageBox.Show("not support game");
			}
		}

		private void FileSave(Object? parameter)
		{
			SaveData.Save();
		}

		private void FileImport(Object? parameter)
		{
			var dlg = new OpenFileDialog();
			if (dlg.ShowDialog() == false) return;

			SaveData.Import(dlg.FileName);
		}

		private void FileExport(Object? parameter)
		{
			var dlg = new SaveFileDialog();
			if (dlg.ShowDialog() == false) return;

			SaveData.Export(dlg.FileName);
		}
	}
}
