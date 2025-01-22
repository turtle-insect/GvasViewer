﻿using Gvas.Property;
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
		public ICommand CommandFilterProperty { get; init; }
		public ICommand CommandExportByteProperty { get; init; }
		public ICommand CommandImportByteProperty { get; init; }

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
			CommandFilterProperty = new ActionCommand(FilterProperty);
			CommandExportByteProperty = new ActionCommand(ExportByteProperty);
			CommandImportByteProperty = new ActionCommand(ImportByteProperty);
		}

		public void LoadFile(String fileName)
		{
			IFileFormat[] fileFormats =
			[
				new PlainGvas(),
				new DivisionGvas(),
				new BravelyDefault2(),
				new RomancingSaga2(),
			];
			mFileFormat = null;

			Byte[] buffer = [];

			foreach (var fileFormat in fileFormats)
			{
				try
				{
					var tmp = fileFormat.Load(fileName);
					if (tmp.Length > 4 && Encoding.UTF8.GetString(tmp[..4].ToArray()) == "GVAS")
					{
						buffer = tmp;
						mFileFormat = fileFormat;
						break;
					}
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
				using var ms = new MemoryStream(buffer);
				using var reader = new BinaryReader(ms);
				mGvas.Read(reader);
				mFileName = fileName;
				FilterProperty();
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

			mFileFormat.Save(mFileName, ReadGvas());
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

			File.WriteAllBytes(dlg.FileName, ReadGvas());
		}

		private void FilterProperty(Object? parameter)
		{
			FilterProperty();
		}

		private void ExportByteProperty(Object? parameter)
		{
			GvasByteProperty? property = parameter as GvasByteProperty;
			if (property == null) return;
			Byte[]? buffer = property.Value as Byte[];
			if (buffer == null) return;

			var dlg = new SaveFileDialog();
			if (dlg.ShowDialog() == false) return;

			File.WriteAllBytes(dlg.FileName, buffer);
		}

		private void ImportByteProperty(Object? parameter)
		{
			GvasByteProperty? property = parameter as GvasByteProperty;
			if (property == null) return;

			var dlg = new OpenFileDialog();
			if (dlg.ShowDialog() == false) return;

			property.Value = File.ReadAllBytes(dlg.FileName);
		}

		private Byte[] ReadGvas()
		{
			using var ms = new MemoryStream();
			using var writer = new BinaryWriter(ms);
			mGvas.Write(writer);
			writer.Flush();

			return ms.ToArray();
		}

		private void FilterProperty()
		{
			GvasProperties.Clear();
			foreach (var property in mGvas.Properties)
			{
				FilterPropertyChildren(property);
			}
		}

		private void FilterPropertyChildren(GvasProperty property)
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
				FilterPropertyChildren(children);
			}
		}
	}
}
