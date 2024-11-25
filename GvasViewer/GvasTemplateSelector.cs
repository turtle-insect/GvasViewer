using Gvas.Property;
using System.Windows.Controls;
using System.Windows;

namespace GvasViewer
{
    class GvasTemplateSelector : DataTemplateSelector
	{
#pragma warning disable CS8618
		// use xaml
		public DataTemplate GvasTitleTemplate { get; set; }
		public DataTemplate GvasArrayPropertyTemplate { get; set; }
		public DataTemplate GvasStructPropertyTemplate { get; set; }
		public DataTemplate GvasCheckBoxTemplate { get; set; }
		public DataTemplate GvasTextBoxTemplate { get; set; }
#pragma warning restore CS8618

		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			switch (item)
			{
				case GvasBoolProperty:
					return GvasCheckBoxTemplate;

				case GvasIntProperty:
				case GvasUInt32Property:
				case GvasInt64Property:
				case GvasUInt64Property:
				case GvasFloatProperty:
				case GvasTextProperty:
				case GvasStrProperty:
				case GvasNameProperty:
					return GvasTextBoxTemplate;

				case GvasArrayProperty:
					return GvasArrayPropertyTemplate;

				case GvasStructProperty:
					return GvasStructPropertyTemplate;

				default:
					return GvasTitleTemplate;
			}

			//return base.SelectTemplate(item, container);
		}
	}
}