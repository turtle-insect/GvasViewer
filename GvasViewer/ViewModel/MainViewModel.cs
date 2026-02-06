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
		public ICommand OpenFileCommand { get; init; }
		public ICommand SaveFileCommand { get; init; }
		public ICommand SaveAsFileCommand { get; init; }
		public ICommand ExportFileCommand { get; init; }
		public ICommand ImportFileCommand { get; init; }
		public ICommand CopyPropertyPathCommand { get; init; }
		public ICommand SearchPropertyCommand { get; init; }
		public ICommand ExportBytePropertyCommand { get; init; }
		public ICommand ImportBytePropertyCommand { get; init; }
		public ICommand CreateArrayPropertyCommand { get; init; }
		public ICommand ImportArrayPropertyCommand { get; init; }
		public ICommand CreateMapPropertyCommand { get; init; }
		public ICommand ImportMapPropertyCommand { get; init; }

		public ObservableCollection<GvasPropertyViewModel> GvasProperties { get; init; } = new();

		private readonly SaveData mSaveData = new();

		public String Keyword { get; set; } = String.Empty;

		public MainViewModel()
		{
			OpenFileCommand = new ActionCommand(OpenFile);
			SaveFileCommand = new ActionCommand(SaveFile);
			SaveAsFileCommand = new ActionCommand(SaveAsFile);
			ExportFileCommand = new ActionCommand(ExportFile);
			ImportFileCommand = new ActionCommand(ImportFile);
			CopyPropertyPathCommand = new ActionCommand(CopyPropertyPath);
			SearchPropertyCommand = new ActionCommand(SearchProperty);
			ExportBytePropertyCommand = new ActionCommand(ExportByteProperty);
			ImportBytePropertyCommand = new ActionCommand(ImportByteProperty);
			CreateArrayPropertyCommand = new ActionCommand(CreateArrayProperty);
			ImportArrayPropertyCommand = new ActionCommand(ImportArrayProperty);
			CreateMapPropertyCommand = new ActionCommand(CreateMapProperty);
			ImportMapPropertyCommand = new ActionCommand(ImportMapProperty);
		}

		public void LoadFile(String filename)
		{
			mSaveData.Load(filename);
			LoadProperty();
		}

		private void OpenFile(Object? parameter)
		{
			var dlg = new OpenFileDialog();
			if (dlg.ShowDialog() == false) return;

			LoadFile(dlg.FileName);
		}

		private void SaveFile(Object? parameter)
		{
			mSaveData.Save();
		}

		private void SaveAsFile(Object? parameter)
		{
			if (!mSaveData.IsAction()) return;

			var dlg = new SaveFileDialog();
			if (dlg.ShowDialog() == false) return;

			mSaveData.SaveAs(dlg.FileName);
		}

		private void ExportFile(Object? parameter)
		{
			if (!mSaveData.IsAction()) return;

			var dlg = new SaveFileDialog();
			if (dlg.ShowDialog() == false) return;

			mSaveData.Export(dlg.FileName);
		}

		private void ImportFile(Object? parameter)
		{
			if (!mSaveData.IsAction()) return;

			var dlg = new OpenFileDialog();
			if (dlg.ShowDialog() == false) return;

			mSaveData.Import(dlg.FileName);
			LoadProperty();
		}

		private void CopyPropertyPath(Object? parameter)
		{
			var vm = parameter as GvasPropertyViewModel;
			if (vm == null) return;

			System.Windows.Clipboard.SetText(vm.Property.Path());
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

		private void ImportArrayProperty(Object? parameter)
		{
			var vm = parameter as GvasPropertyViewModel;
			if (vm == null) return;

			var property = vm.Property as GvasArrayProperty;
			if (property == null) return;

			var count = property.Children.Count;
			if (count == 0) return;

			var dlg = new OpenFileDialog();
			if (dlg.ShowDialog() == false) return;

			var clone = property.Children[0].Clone();
			vm.ClearChildren();

			foreach (var line in File.ReadLines(dlg.FileName))
			{
				if (String.IsNullOrEmpty(line)) continue;
				if (line.StartsWith('#')) continue;

				var child = clone.Clone();
				child.Name = $"[{property.Children.Count}]";
				child.Value = line;
				vm.AppendChildren(child);
			}
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

		private void ImportMapProperty(Object? parameter)
		{
			var vm = parameter as GvasPropertyViewModel;
			if (vm == null) return;

			var property = vm.Property as GvasMapProperty;
			if (property == null) return;

			var count = property.Children.Count;
			if (count == 0) return;

			var dlg = new OpenFileDialog();
			if (dlg.ShowDialog() == false) return;

			var clone = property.Children[0].Clone();
			vm.ClearChildren();

			foreach (var line in File.ReadLines(dlg.FileName))
			{
				if (String.IsNullOrEmpty(line)) continue;
				if (line.StartsWith('#')) continue;

				var elements = line.Split('\t');
				if (elements.Length != 2) continue;

				var key = elements[0];
				var value = elements[1];
				if (String.IsNullOrEmpty(key) || String.IsNullOrEmpty(value)) continue;

				var child = clone.Clone();
				child.Name = key;
				child.Value = value;
				vm.AppendChildren(child);
			}
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
