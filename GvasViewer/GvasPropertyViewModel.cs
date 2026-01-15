using Gvas.Property;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace GvasViewer
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
