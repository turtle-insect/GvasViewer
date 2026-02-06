using Gvas.Property;
using System.Collections.ObjectModel;

namespace GvasViewer.ViewModel
{
	internal class GvasPropertyViewModel
	{
		public ObservableCollection<GvasPropertyViewModel> Children { get; init; } = new();
		public GvasProperty Property { get; init; }

		public GvasPropertyViewModel(GvasProperty property)
		{
			Property = property;
			foreach (var child in property.Children)
			{
				Children.Add(new GvasPropertyViewModel(child));
			}
		}

		public void AppendChildren(GvasProperty child)
		{
			Property.AppendChildren(child);
			Children.Add(new GvasPropertyViewModel(child));
		}

		public void ClearChildren()
		{
			Property.ClearChildren();
			Children.Clear();
		}
	}
}
