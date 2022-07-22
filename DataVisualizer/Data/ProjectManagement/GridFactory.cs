using Microsoft.Xaml.Behaviors.Layout;
using SkiaSharp;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Microsoft.Xaml.Behaviors;

namespace DataVisualizer.Data.ProjectManagement {
	internal static class GridFactory {

		internal static Grid CreateGrid(string text, string resourcePath) {

			var drag = new MouseDragElementBehavior() {
				ConstrainToParentBounds = false
			};

			Grid g = new() {
				Width = 100,
				RowDefinitions = {
					new() { Height = new GridLength(30)},
					new() { Height = new GridLength(40)}
				},
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Top,
				Background = Brushes.Transparent
			};

			TextBox t = new() {
				Background = Brushes.LightPink,
				BorderThickness = new(0),
				Text = text,
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Center
			};

			Image i = new() {
				IsHitTestVisible = false,
				Stretch = Stretch.Uniform,
				Source = new BitmapImage(new Uri(resourcePath))
			};

			t.SetValue(Grid.RowProperty, 0);
			i.SetValue(Grid.RowProperty, 1);

			g.Children.Add(t);
			g.Children.Add(i);

			Interaction.GetBehaviors(g).Add(drag);

			return g;
		}

	}
}
