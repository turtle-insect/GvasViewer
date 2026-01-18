using Gvas.Property;
using Gvas.Property.Standard;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace GvasViewer.ViewModel
{
	class MainViewModel
    {
		public ICommand CommandFileOpen { get; init; }
		public ICommand CommandFileSave { get; init; }
		public ICommand CommandFileSaveAs { get; init; }
		public ICommand CommandFileExport { get; init; }
		public ICommand CommandFileImport { get; init; }
		public ICommand CommandSearchProperty { get; init; }
		public ICommand CommandExportByteProperty { get; init; }
		public ICommand CommandImportByteProperty { get; init; }
		public ICommand CommandCreateArrayProperty { get; init; }
		public ICommand CommandCreateMapProperty { get; init; }

		public ObservableCollection<GvasPropertyViewModel> GvasProperties { get; init; } = new();

		private readonly SaveData mSaveData = new();

		public String Keyword { get; set; } = String.Empty;

		public MainViewModel()
		{
			CommandFileOpen = new ActionCommand(FileOpen);
			CommandFileSave = new ActionCommand(FileSave);
			CommandFileSaveAs = new ActionCommand(FileSaveAs);
			CommandFileExport = new ActionCommand(FileExport);
			CommandFileImport = new ActionCommand(FileImport);
			CommandSearchProperty = new ActionCommand(SearchProperty);
			CommandExportByteProperty = new ActionCommand(ExportByteProperty);
			CommandImportByteProperty = new ActionCommand(ImportByteProperty);
			CommandCreateArrayProperty = new ActionCommand(CreateArrayProperty);
			CommandCreateMapProperty = new ActionCommand(CreateMapProperty);

			CommandManager.InvalidateRequerySuggested();
		}

		public void LoadFile(String filename)
		{
			mSaveData.Load(filename);
			LoadProperty();
		}

		private void FileOpen(Object? parameter)
		{
			var dlg = new OpenFileDialog();
			if (dlg.ShowDialog() == false) return;

			LoadFile(dlg.FileName);
		}

		private void FileSave(Object? parameter)
		{
			mSaveData.Save();
		}

		private void FileSaveAs(Object? parameter)
		{
			if (!mSaveData.IsAction()) return;

			var dlg = new SaveFileDialog();
			if (dlg.ShowDialog() == false) return;

			mSaveData.SaveAs(dlg.FileName);
		}

		private void FileExport(Object? parameter)
		{
			if (!mSaveData.IsAction()) return;

			var dlg = new SaveFileDialog();
			if (dlg.ShowDialog() == false) return;

			mSaveData.Export(dlg.FileName);
		}

		private void FileImport(Object? parameter)
		{
			if (!mSaveData.IsAction()) return;

			var dlg = new OpenFileDialog();
			if (dlg.ShowDialog() == false) return;

			mSaveData.Import(dlg.FileName);
			LoadProperty();
		}

		private void SearchProperty(Object? parameter)
		{
			LoadProperty();
		}

		private void ExportByteProperty(Object? parameter)
		{
			var vm = parameter as GvasPropertyViewModel;
			if (vm == null) return;

			var property = vm.Property as GvasByteProperty;
			if (property == null) return;

			Byte[]? buffer = property.Value as Byte[];
			if (buffer == null) return;

			var dlg = new SaveFileDialog();
			if (dlg.ShowDialog() == false) return;

			File.WriteAllBytes(dlg.FileName, buffer);
		}

		private void ImportByteProperty(Object? parameter)
		{
			var vm = parameter as GvasPropertyViewModel;
			if (vm == null) return;

			var property = vm.Property as GvasByteProperty;
			if (property == null) return;

			var dlg = new OpenFileDialog();
			if (dlg.ShowDialog() == false) return;

			property.Value = File.ReadAllBytes(dlg.FileName);
		}

		private void CreateArrayProperty(Object? parameter)
		{
			var vm = parameter as GvasPropertyViewModel;
			if (vm == null) return;

			var property = vm.Property as GvasArrayProperty;
			if (property == null) return;

			var count = property.Children.Count;
			GvasProperty? child = null;
			if (count == 0)
			{
				switch(property.PropertyType)
				{
					case "NameProperty":
						child = new GvasNameProperty();
						child.Value = "dummy";
						break;

					default:
						MessageBox.Show("not support property");
						break;
				}
			}
			else
			{
				child = property.Children[0].Clone();
			}

			if (child == null) return;

			child.Name = $"[{count}]";
			vm.AppendChildren(child);
		}

		private void CreateMapProperty(Object? parameter)
		{
			var vm = parameter as GvasPropertyViewModel;
			if (vm == null) return;

			var property = vm.Property as GvasMapProperty;
			if (property == null) return;

			var count = property.Children.Count;
			if (count == 0) return;

			vm.AppendChildren(property.Children[0].Clone());
		}

		private void LoadProperty()
		{
			GvasProperties.Clear();
			if (mSaveData == null) return;

			var properties = mSaveData.Properties;
			if (properties == null) return;

			foreach (var property in properties)
			{
				LoadProperty(property);
			}
		}

		private void LoadProperty(GvasProperty property)
		{
			if(String.IsNullOrEmpty(Keyword))
			{
				GvasProperties.Add(new GvasPropertyViewModel(property));
				return;
			}

			if(property.Name.ToLower().Contains(Keyword.ToLower()))
			{
				GvasProperties.Add(new GvasPropertyViewModel(property));
			}

			foreach(var child in property.Children)
			{
				LoadProperty(child);
			}
		}
	}
}
