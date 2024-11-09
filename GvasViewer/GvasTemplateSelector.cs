using Gvas.Property;
using System.Windows;
using System.Windows.Controls;

namespace GvasViewer
{
	internal class GvasTemplateSelector : DataTemplateSelector
	{
#pragma warning disable CS8618
		// use xaml
		public DataTemplate GvasTitleTemplate { get; set; }
		public DataTemplate GvasLabelTemplate { get; set; }
		public DataTemplate GvasTextBoxTemplate { get; set; }
#pragma warning restore CS8618

		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			switch(item)
			{
				case GvasIntProperty:
				case GvasInt64Property:
					return GvasTextBoxTemplate;

				case GvasTextProperty:
				case GvasStrProperty:
				case GvasNameProperty:
					return GvasLabelTemplate;

				default:
					return GvasTitleTemplate;
			}

			//return base.SelectTemplate(item, container);
		}
	}
}
