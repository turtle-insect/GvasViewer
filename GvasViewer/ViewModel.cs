using Gvas.Property;
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
		public ICommand CommandFileSaveAs { get; init; }
		public ICommand CommandFileExport { get; init; }
		public ICommand CommandPropertyFilter { get; init; }

		public ObservableCollection<Gvas.Property.GvasProperty> GvasProperties { get; set; } = new();

		private String mFileName = String.Empty;
		private IFileFormat? mFileFormat;
		private Gvas.Gvas mGvas = new();

		public String Filter { get; set; } = String.Empty;

		public ViewModel()
		{
			CommandFileOpen = new ActionCommand(FileOpen);
			CommandFileSave = new ActionCommand(FileSave);
			CommandFileSaveAs = new ActionCommand(FileSaveAs);
			CommandFileExport = new ActionCommand(FileExport);
			CommandPropertyFilter = new ActionCommand(PropertyFilter);
		}

		private void FileOpen()
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

			if(mFileFormat == null)
			{
				MessageBox.Show("not gvas format");
				return;
			}

			try
			{
				using var ms = new MemoryStream(buffer);
				using var reader = new BinaryReader(ms);
				mGvas.Read(reader);
				mFileName = filename;
				PropertyFilter();
			}
			catch
			{
				mFileFormat = null;
				MessageBox.Show("not support game");
			}
		}

		private void FileSave()
		{
			if (mFileFormat == null) return;

			mFileFormat.Save(mFileName, ReadGvas());
		}

		private void FileSaveAs()
		{
			if (mFileFormat == null) return;

			var dlg = new SaveFileDialog();
			if (dlg.ShowDialog() == false) return;

			mFileName = dlg.FileName;
			FileSave();
		}

		private void FileExport()
		{
			if (mFileFormat == null) return;

			var dlg = new SaveFileDialog();
			if (dlg.ShowDialog() == false) return;

			File.WriteAllBytes(dlg.FileName, ReadGvas());
		}

		private Byte[] ReadGvas()
		{
			using var ms = new MemoryStream();
			using var writer = new BinaryWriter(ms);
			mGvas.Write(writer);
			writer.Flush();

			return ms.ToArray();
		}

		private void PropertyFilter()
		{
			GvasProperties.Clear();
			foreach (var property in mGvas.Properties)
			{
				PropertyFilter(property);
			}
		}

		private void PropertyFilter(GvasProperty property)
		{
			if(String.IsNullOrEmpty(Filter))
			{
				GvasProperties.Add(property);
				return;
			}

			if(property.Name.IndexOf(Filter) >= 0)
			{
				GvasProperties.Add(property);
			}

			foreach(var children  in property.Childrens)
			{
				PropertyFilter(children);
			}
		}
	}
}
