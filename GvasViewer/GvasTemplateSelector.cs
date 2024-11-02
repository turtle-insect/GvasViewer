using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
				case Gvas.GvasTextProperty:
				case Gvas.GvasStrProperty:
				case Gvas.GvasNameProperty:
					return GvasLabelTemplate;

				case Gvas.GvasIntProperty:
					return GvasTextBoxTemplate;

				default:
					return GvasTitleTemplate;
			}

			//return base.SelectTemplate(item, container);
		}
	}
}
