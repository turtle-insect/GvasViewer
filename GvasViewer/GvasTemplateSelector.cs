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
				case Gvas.Property.v1.Standard.GvasBoolProperty:
				case Gvas.Property.v2.Standard.GvasBoolProperty:
					return GvasCheckBoxTemplate;

				case Gvas.Property.v1.Standard.GvasByteProperty:
				case Gvas.Property.v2.Standard.GvasByteProperty:
					return GvasBytePropertyTemplate;

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
					return GvasTextBoxTemplate;

				case Gvas.Property.v1.Standard.GvasArrayProperty:
				case Gvas.Property.v2.Standard.GvasArrayProperty:
					return GvasArrayPropertyTemplate;

				case Gvas.Property.v1.Standard.GvasMapProperty:
				case Gvas.Property.v2.Standard.GvasMapProperty:
					return GvasMapPropertyTemplate;

				case Gvas.Property.v1.Standard.GvasStructProperty:
				case Gvas.Property.v2.Standard.GvasStructProperty:
					return GvasStructPropertyTemplate;

				default:
					return GvasTitleTemplate;
			}

			//return base.SelectTemplate(item, container);
		}
	}
}
