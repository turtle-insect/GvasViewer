using Gvas.Property;
using System.Collections.ObjectModel;

namespace GvasViewer.ViewModel
{
	internal class GvasPropertyViewModel
	{
		public ObservableCollection<GvasPropertyViewModel> Childrens { get; init; } = new();
		public GvasProperty Property { get; init; }
		public String Name
		{
			get => Property.Name;
			set => Property.Name = value;
		}

		public GvasPropertyViewModel(GvasProperty property)
		{
			Property = property;
			foreach (var children in property.Childrens)
			{
				Childrens.Add(new GvasPropertyViewModel(children));
			}
		}

		public void AppendChildren(GvasProperty children)
		{
			Property.Childrens.Add(children);
			Childrens.Add(new GvasPropertyViewModel(children));
		}
	}
}
