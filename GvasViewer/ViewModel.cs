using GvasViewer.Gvas.Property;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace GvasViewer
{
    internal class ViewModel
	{
		public SaveData SaveData { get; init; } = SaveData.Instance();

		public ICommand CommandFileOpen { get; init; }
		public ICommand CommandFileSave { get; init; }
		public ICommand CommandFileImport { get; init; }
		public ICommand CommandFileExport { get; init; }

		public IList<GvasProperty> GvasProperties { get; set; } = new ObservableCollection<GvasProperty>();

		public ViewModel()
		{
			CommandFileOpen = new ActionCommand(FileOpen);
			CommandFileSave = new ActionCommand(FileSave);
			CommandFileImport = new ActionCommand(FileImport);
			CommandFileExport = new ActionCommand(FileExport);
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
