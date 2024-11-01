using GvasViewer.Gvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GvasViewer.Control
{
	internal class GvasViewItem : TreeViewItem
	{
		public GvasViewItem(Gvas.GvasProperty gvasProperty)
		{
			FontSize = 18;

			switch (gvasProperty)
			{
				case Gvas.GvasIntProperty intProperty:
					Header = Create(intProperty);
					break;

				case Gvas.GvasTextProperty textProperty:
					Header = Create(textProperty);
					break;

				case Gvas.GvasStrProperty strProperty:
					Header = Create(strProperty);
					break;

				case Gvas.GvasNameProperty nameProperty:
					Header = Create(nameProperty);
					break;

				case Gvas.GvasArrayProperty arrayProperty:
					Header = gvasProperty.Name;
					foreach (var property in arrayProperty.Properties)
					{
						if (property is GvasNoneProperty) continue;
						Items.Add(new GvasViewItem(property));
					}
					break;

				case Gvas.GvasStructProperty structProperty:
					Header = gvasProperty.Name;
					foreach (var property in structProperty.Properties)
					{
						if (property is GvasNoneProperty) continue;
						Items.Add(new GvasViewItem(property));
					}
					break;

				default:
					Header = gvasProperty.Name;
					break;
			}
		}

		private Panel Create(GvasProperty property)
		{
			var panel = new StackPanel();
			panel.Orientation = Orientation.Horizontal;
			panel.Children.Add(new Label() { Content = property.Name });
			panel.Children.Add(new Label()
			{
				Content = property.Value.ToString(),
				FontWeight = FontWeights.Bold,
				Foreground = Brushes.Blue
			});
			//panel.Children.Add(new TextBox() { Text = property.Value.ToString() });

			return panel;
		}
	}
}
