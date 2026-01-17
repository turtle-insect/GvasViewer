using Gvas.Property;
using Gvas.Property.Standard;
using GvasViewer.FileFormat;
using GvasViewer.FileFormat.Platform;
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

		private String mFileName = String.Empty;
		private IFileFormat? mFileFormat;
		private Gvas.Gvas? mGvas;

		public String Search { get; set; } = String.Empty;

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
		}

		public void LoadFile(String fileName)
		{
			IFileFormat[] fileFormats =
			[
				new PlainGvas(),
				new DivisionGvas(),
				new FileFormat.Platform.Switch.BravelyDefault2(),
				new FileFormat.Platform.Switch.RomancingSaga2(),
				new DragonQuest7(Platform.Steam),
				new DragonQuest7(Platform.Switch),
			];

			Byte[] buffer = [];
			foreach (var fileFormat in fileFormats)
			{
				try
				{
					var tmp = fileFormat.Load(fileName);
					if (tmp.Length < 4) continue;
					if (System.Text.Encoding.UTF8.GetString(tmp, 0, 4) != "GVAS") continue;

					mFileFormat = fileFormat;
					buffer = tmp;
					break;
				}
				catch { }
			}

			if (mFileFormat == null)
			{
				MessageBox.Show("not gvas format");
				return;
			}

			try
			{
				mGvas = LoadGvas(buffer);
				mFileName = fileName;
				LoadProperty();
			}
			catch
			{
				mFileFormat = null;
				MessageBox.Show("not support game");
			}
		}

		private void FileOpen(Object? parameter)
		{
			var dlg = new OpenFileDialog();
			if (dlg.ShowDialog() == false) return;

			LoadFile(dlg.FileName);
		}

		private void FileSave(Object? parameter)
		{
			if (mFileFormat == null) return;

			var buffer = CreateGvasBuffer();
			if (buffer == null) return;

			mFileFormat.Save(mFileName, buffer);
		}

		private void FileSaveAs(Object? parameter)
		{
			if (mFileFormat == null) return;

			var dlg = new SaveFileDialog();
			if (dlg.ShowDialog() == false) return;

			mFileName = dlg.FileName;
			FileSave(null);
		}

		private void FileExport(Object? parameter)
		{
			if (mFileFormat == null) return;

			var dlg = new SaveFileDialog();
			if (dlg.ShowDialog() == false) return;

			var buffer = CreateGvasBuffer();
			if (buffer == null) return;

			File.WriteAllBytes(dlg.FileName, buffer);
		}

		private void FileImport(Object? parameter)
		{
			if (mFileFormat == null) return;

			var dlg = new OpenFileDialog();
			if (dlg.ShowDialog() == false) return;

			try
			{
				var buffer = File.ReadAllBytes(dlg.FileName);
				if (buffer.Length < 4) return;
				if (System.Text.Encoding.UTF8.GetString(buffer, 0, 4) != "GVAS") return;

				mGvas = LoadGvas(buffer);
				LoadProperty();
			}
			catch
			{
				MessageBox.Show("not gvas format");
				return;
			}
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

			var count = property.Childrens.Count;
			GvasProperty? children = null;
			if (count == 0)
			{
				switch(property.PropertyType)
				{
					case "NameProperty":
						children = new GvasNameProperty();
						children.Value = "dummy";
						break;

					default:
						MessageBox.Show("not support property");
						break;
				}
			}
			else
			{
				children = property.Childrens[0].Clone();
			}

			if (children == null) return;

			children.Name = $"[{count}]";
			vm.AppendChildren(children);
		}

		private void CreateMapProperty(Object? parameter)
		{
			var vm = parameter as GvasPropertyViewModel;
			if (vm == null) return;

			var property = vm.Property as GvasMapProperty;
			if (property == null) return;

			var count = property.Childrens.Count;
			if (count == 0) return;

			vm.AppendChildren(property.Childrens[0].Clone());
		}

		private Byte[]? CreateGvasBuffer()
		{
			if (mGvas == null) return null;

			using var ms = new MemoryStream();
			using var writer = new BinaryWriter(ms);
			mGvas.Write(writer);
			writer.Flush();

			return ms.ToArray();
		}

		private Gvas.Gvas LoadGvas(Byte[] buffer)
		{
			using var ms = new MemoryStream(buffer);
			using var reader = new BinaryReader(ms);
			var gvas = new Gvas.Gvas();
			gvas.Read(reader);

			return gvas;
		}

		private void LoadProperty()
		{
			if (mGvas == null) return;

			GvasProperties.Clear();
			foreach (var property in mGvas.Properties)
			{
				LoadPropertyChildren(property);
			}
		}

		private void LoadPropertyChildren(GvasProperty property)
		{
			if(String.IsNullOrEmpty(Search))
			{
				GvasProperties.Add(new GvasPropertyViewModel(property));
				return;
			}

			if(property.Name.ToLower().Contains(Search.ToLower()))
			{
				GvasProperties.Add(new GvasPropertyViewModel(property));
			}

			foreach(var children  in property.Childrens)
			{
				LoadPropertyChildren(children);
			}
		}
	}
}
