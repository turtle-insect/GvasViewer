using Gvas.Property.Standard;
using System.Windows;
using System.Windows.Controls;

namespace GvasViewer
{
	class GvasTemplateSelector : DataTemplateSelector
	{
#pragma warning disable CS8618
		// use xaml
		public DataTemplate GvasTitleTemplate { get; set; }
		public DataTemplate GvasBytePropertyTemplate { get; set; }
		public DataTemplate GvasArrayPropertyTemplate { get; set; }
		public DataTemplate GvasMapPropertyTemplate { get; set; }
		public DataTemplate GvasStructPropertyTemplate { get; set; }
		public DataTemplate GvasCheckBoxTemplate { get; set; }
		public DataTemplate GvasTextBoxTemplate { get; set; }
#pragma warning restore CS8618

		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			var vm = item as ViewModel.GvasPropertyViewModel;
			if (vm == null) return GvasTitleTemplate;

			switch (vm.Property)
			{
				case GvasBoolProperty:
					return GvasCheckBoxTemplate;

				case GvasByteProperty:
					return GvasBytePropertyTemplate;

				case GvasIntProperty:
				case GvasUInt32Property:
				case GvasInt64Property:
				case GvasUInt64Property:
				case GvasFloatProperty:
				case GvasDoubleProperty:
				case GvasTextProperty:
				case GvasStrProperty:
				case GvasNameProperty:
					return GvasTextBoxTemplate;

				case GvasArrayProperty:
					return GvasArrayPropertyTemplate;

				case GvasMapProperty:
					return GvasMapPropertyTemplate;

				case GvasStructProperty:
					return GvasStructPropertyTemplate;

				default:
					return GvasTitleTemplate;
			}

			//return base.SelectTemplate(item, container);
		}
	}
}
