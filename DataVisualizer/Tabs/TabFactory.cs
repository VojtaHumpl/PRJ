using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DataVisualizer.Tabs {
	internal static class TabFactory {

		internal static TabItem CreatePlanTab(DrawingImage drawing) {
			var tab = new TabItem() {
				Header = "Plan"
			};

			var border = new Border() { 
				BorderThickness = new Thickness(1.5),
				CornerRadius = new CornerRadius(4),
				Padding = new Thickness(6),
				BorderBrush = Brushes.LightGray,
				Background = Brushes.White,
				ClipToBounds = true
			};

			var image = new Image() {
				VerticalAlignment = VerticalAlignment.Stretch,
				HorizontalAlignment = HorizontalAlignment.Stretch,
				Stretch = Stretch.Uniform,
				Source = drawing
			};

			border.Child = image;
			tab.Content = border;

			return tab;
		}

	}
}
