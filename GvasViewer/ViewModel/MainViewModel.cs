using Gvas.Property;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace GvasViewer.ViewModel
{
	class MainViewModel
    {
		private readonly SaveData mSaveData = new();

		public ICommand OpenFileCommand { get; init; }
		public ICommand SaveFileCommand { get; init; }
		public ICommand SaveAsFileCommand { get; init; }
		public ICommand ExportFileCommand { get; init; }
		public ICommand ImportFileCommand { get; init; }
		public ICommand CopyPropertyNameCommand { get; init; }
		public ICommand CopyPropertyPathCommand { get; init; }
		public ICommand SearchPropertyCommand { get; init; }
		public ICommand ExportBytePropertyCommand { get; init; }
		public ICommand ImportBytePropertyCommand { get; init; }
		public ICommand CreateArrayPropertyCommand { get; init; }
		public ICommand ImportArrayPropertyCommand { get; init; }
		public ICommand CreateMapPropertyCommand { get; init; }
		public ICommand ImportMapPropertyCommand { get; init; }

		public ObservableCollection<GvasPropertyViewModel> GvasProperties { get; init; } = new();

		public String Keyword { get; set; } = String.Empty;

		public enum eSearchType
		{
			eKey,
			eValue,
		}
		public Array SearchTypes => Enum.GetValues(typeof(eSearchType));
		public eSearchType SearchType { get; set; } = eSearchType.eKey;

		public MainViewModel()
		{
			OpenFileCommand = new ActionCommand(OpenFile);
			SaveFileCommand = new ActionCommand(SaveFile);
			SaveAsFileCommand = new ActionCommand(SaveAsFile);
			ExportFileCommand = new ActionCommand(ExportFile);
			ImportFileCommand = new ActionCommand(ImportFile);
			CopyPropertyNameCommand = new ActionCommand(CopyPropertyName);
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
			try
			{
				if(mSaveData.Load(filename))
				{
					LoadProperty();
				}
				else
				{
					MessageBox.Show("not support");
				}
			}
			catch(DllNotFoundException ex)
			{
				MessageBox.Show(ex.Message);
			}
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

		private void CopyPropertyName(Object? parameter)
		{
			var vm = parameter as GvasPropertyViewModel;
			if (vm == null) return;

			System.Windows.Clipboard.SetText(vm.Property.Name.Value);
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

			var property = vm.Property as Gvas.Property.v1.Standard.GvasByteProperty;
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

			var property = vm.Property;
			if (IsGvasByteProperty(property) == false) return;

			var dlg = new OpenFileDialog();
			if (dlg.ShowDialog() == false) return;

			property.Value = File.ReadAllBytes(dlg.FileName);
		}

		private void CreateArrayProperty(Object? parameter)
		{
			var vm = parameter as GvasPropertyViewModel;
			if (vm == null) return;

			var property = vm.Property;
			if (IsGvasArrayProperty(property) == false) return;

			var count = property.Children.Count;
			GvasProperty? child = null;
			if (count == 0)
			{
				switch(property.Children[0])
				{
					case Gvas.Property.v1.Standard.GvasNameProperty:
						child = new Gvas.Property.v1.Standard.GvasNameProperty();
						child.Value = "dummy";
						break;

					case Gvas.Property.v2.Standard.GvasNameProperty:
						child = new Gvas.Property.v2.Standard.GvasNameProperty();
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

			child.Name = new($"[{count}]", System.Text.Encoding.UTF8);
			vm.AppendChildren(child);
		}

		private void ImportArrayProperty(Object? parameter)
		{
			var vm = parameter as GvasPropertyViewModel;
			if (vm == null) return;

			var property = vm.Property;
			if (IsGvasArrayProperty(property) == false) return;

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
				child.Name = new($"[{property.Children.Count}]", System.Text.Encoding.UTF8);
				child.Value = line;
				vm.AppendChildren(child);
			}
		}

		private void CreateMapProperty(Object? parameter)
		{
			var vm = parameter as GvasPropertyViewModel;
			if (vm == null) return;

			var property = vm.Property;
			if (IsGvasMapProperty(property) == false) return;

			var count = property.Children.Count;
			if (count == 0) return;

			vm.AppendChildren(property.Children[0].Clone());
		}

		private void ImportMapProperty(Object? parameter)
		{
			var vm = parameter as GvasPropertyViewModel;
			if (vm == null) return;

			var property = vm.Property;
			if (IsGvasMapProperty(property) == false) return;

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
				child.Name = new(key, System.Text.Encoding.UTF8);
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

			var value = String.Empty;
			switch(SearchType)
			{
				case eSearchType.eKey:
					value = property.Name.Value.ToLower();
					break;

				case eSearchType.eValue:
					// Since using try-catch slows things down, limit the search objects
					switch (property)
					{
						case Gvas.Property.v1.Standard.GvasInt8Property:
						case Gvas.Property.v1.Standard.GvasIntProperty:
						case Gvas.Property.v1.Standard.GvasUInt32Property:
						case Gvas.Property.v1.Standard.GvasInt64Property:
						case Gvas.Property.v1.Standard.GvasUInt64Property:
						case Gvas.Property.v1.Standard.GvasFloatProperty:
						case Gvas.Property.v1.Standard.GvasDoubleProperty:
						case Gvas.Property.v1.Standard.GvasTextProperty:
						case Gvas.Property.v1.Standard.GvasStrProperty:
						case Gvas.Property.v1.Standard.GvasNameProperty:
						case Gvas.Property.v2.Standard.GvasInt8Property:
						case Gvas.Property.v2.Standard.GvasIntProperty:
						case Gvas.Property.v2.Standard.GvasUInt32Property:
						case Gvas.Property.v2.Standard.GvasInt64Property:
						case Gvas.Property.v2.Standard.GvasUInt64Property:
						case Gvas.Property.v2.Standard.GvasFloatProperty:
						case Gvas.Property.v2.Standard.GvasDoubleProperty:
						case Gvas.Property.v2.Standard.GvasTextProperty:
						case Gvas.Property.v2.Standard.GvasStrProperty:
						case Gvas.Property.v2.Standard.GvasNameProperty:
							value = property.Value.ToString()?.ToLower();
							break;
					}
					break;
			}

			if(String.IsNullOrEmpty(value) == false)
			{
				if (value.Contains(Keyword.ToLower()))
				{
					GvasProperties.Add(new GvasPropertyViewModel(property));
				}
			}

			foreach (var child in property.Children)
			{
				LoadProperty(child);
			}
		}

		private bool IsGvasByteProperty(GvasProperty property)
		{
			if (property is Gvas.Property.v1.Standard.GvasByteProperty) return true;
			if (property is Gvas.Property.v2.Standard.GvasByteProperty) return true;
			return false;
		}

		private bool IsGvasArrayProperty(GvasProperty property)
		{
			if (property is Gvas.Property.v1.Standard.GvasArrayProperty) return true;
			if (property is Gvas.Property.v2.Standard.GvasArrayProperty) return true;
			return false;
		}

		private bool IsGvasMapProperty(GvasProperty property)
		{
			if (property is Gvas.Property.v1.Standard.GvasMapProperty) return true;
			if (property is Gvas.Property.v2.Standard.GvasMapProperty) return true;
			return false;
		}
	}
}
