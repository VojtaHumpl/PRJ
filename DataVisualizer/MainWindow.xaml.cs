using CodingSeb.ExpressionEvaluator;
using SharpVectors.Converters;
using SharpVectors.Renderers.Wpf;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Point = System.Windows.Point;

using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using Microsoft.Win32;
using System;
using DataVisualizer.Data.ProjectManagement;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Data;
using DataVisualizer.Data;
using DataVisualizer.Tabs;
using LiveChartsCore.SkiaSharpView.WPF;
using System.Globalization;
using DataVisualizer.ViewModels;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using System.Xml;
using Microsoft.Xaml.Behaviors.Layout;
using Microsoft.Xaml.Behaviors;
using Brushes = System.Windows.Media.Brushes;

namespace DataVisualizer {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {

		private Point FloorplanOrigin { get; set; }
		private Point FloorplanStart { get; set; }
		private Slider ZoomSlider { get; set; }

		private bool FormLoaded { get; set; } = false;


		private DataManager MainDataManager { get; set; } = new DataManager();

		internal MainViewModel ViewModel { get; set; }
		

		public MainWindow() {
			InitializeComponent();

			MainDataManager.PM = new(canvas);

			// TODO: move this
			//Setup a transform group that we'll use to manage panning of the image area
			TransformGroup group = new();
			ScaleTransform st = new();
			group.Children.Add(st);
			TranslateTransform tt = new ();
			group.Children.Add(tt);

			//Wire up the slider to the image for zooming
			ZoomSlider = zoomSlider;
			st.ScaleX = ZoomSlider.Value;
			st.ScaleY = ZoomSlider.Value;
			image.RenderTransformOrigin = new Point(0.5, 0.5);
			image.RenderTransform = group;

			canvas.RenderTransformOrigin = new Point(0.5, 0.5);
			canvas.RenderTransform = group;
		}

		private void Window_Loaded(object sender, RoutedEventArgs e) {

			//string expression = "1+1";
			string expression = "((x, y) => x * y)(4, 2)";		// can handle C# code
			//string expression = "5+5+8";		

			ExpressionEvaluator evaluator = new ();
			Debug.WriteLine(expression);
			Debug.WriteLine(evaluator.Evaluate(expression));

			FormLoaded = true;
		}

		private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
			image.ReleaseMouseCapture();
		}

		private void Image_MouseMove(object sender, MouseEventArgs e) {
			if (!image.IsMouseCaptured) 
				return;

			var tt = (TranslateTransform)((TransformGroup)image.RenderTransform).Children.First(tr => tr is TranslateTransform);

			Vector v = FloorplanStart - e.GetPosition(border);
			tt.X = FloorplanOrigin.X - v.X;
			tt.Y = FloorplanOrigin.Y - v.Y;
		}

		private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
			image.CaptureMouse();
			var tt = (TranslateTransform)((TransformGroup)image.RenderTransform).Children.First(tr => tr is TranslateTransform);
			FloorplanStart = e.GetPosition(border);
			FloorplanOrigin = new Point(tt.X, tt.Y);
		}

		private void ZoomSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
			if (!FormLoaded)
				return;

			//Panel panel = _ImageScrollArea;
			var panel = image;

			//Set the scale coordinates on the ScaleTransform from the slider
			ScaleTransform transform = (ScaleTransform)((TransformGroup)panel.RenderTransform).Children.First(tr => tr is ScaleTransform);
			transform.ScaleX = ZoomSlider.Value;
			transform.ScaleY = ZoomSlider.Value;

			//Set the zoom (this will affect rotate too) origin to the center of the panel
			panel.RenderTransformOrigin = new Point(0.5, 0.5);
		}

		private void Window_MouseWheel(object sender, MouseWheelEventArgs e) {
			ZoomSlider.Value += ZoomSlider.SmallChange * e.Delta / 60;
		}

		private void buttonImportSVG_Click(object sender, RoutedEventArgs e) {
			OpenFileDialog openFileDialog = new();
			openFileDialog.Filter = "SVG files (*.svg)|*.svg|All files (*.*)|*.*";
			openFileDialog.CheckFileExists = true;
			if (openFileDialog.ShowDialog() == true) {
				//var source = @"..\..\..\res\floorplan.svg";
				var source = openFileDialog.FileName;

				// Conversion options
				WpfDrawingSettings settings = new();
				settings.IncludeRuntime = false;
				settings.TextAsGeometry = true;

				FileSvgReader converter = new(settings);

				DrawingGroup drawing = converter.Read(source);

				image.Source = new DrawingImage(drawing);

				// reset position
				var tt = (TranslateTransform)((TransformGroup)image.RenderTransform).Children.First(tr => tr is TranslateTransform);
				tt.X = 0;
				tt.Y = 0;

				MainDataManager.PM.FloorplanPath = openFileDialog.FileName;
			}
		}

		private void buttonImportData_Click(object sender, RoutedEventArgs e) {
			OpenFileDialog openFileDialog = new() {
				Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
				CheckFileExists = true
			};
			//openFileDialog.Multiselect = true; do later
			if (openFileDialog.ShowDialog() == true) {
				var source = openFileDialog.FileName;

				try {
					var res = MainDataManager.ImportData(source);
					if (!res)
						return;

					if (MainDataManager.Data.Last() is CSVData data) {
						for (int i = 0; i < data.Header.Length; i++) {
							var col = new DataGridTextColumn();
							col.Header = data.Header[i];
							col.Binding = new Binding(string.Format("[{0}]", i));
							dataGrid1.Columns.Add(col);
						}
						dataGrid1.ItemsSource = data.Rows;
					}

					
				} catch (Exception ex) {
					Debug.WriteLine(ex);
				}
			}
		}

		#region TabControl

		private void tabItemPlus_MouseUp(object sender, MouseButtonEventArgs e) {
			if (e.ChangedButton == MouseButton.Left || e.ChangedButton == MouseButton.Right) {
				var contextMenu = tabItemPlus.ContextMenu;
				contextMenu.StaysOpen = true;
				contextMenu.IsOpen = true;
				e.Handled = true;
			}
		}

		private void menuItemPlan_Click(object sender, RoutedEventArgs e) {
			OpenFileDialog openFileDialog = new();
			openFileDialog.Filter = "SVG files (*.svg)|*.svg|All files (*.*)|*.*";
			openFileDialog.RestoreDirectory = false;
			openFileDialog.CheckFileExists = true;
			if (openFileDialog.ShowDialog() == true) {
				var source = openFileDialog.FileName;

				

				// Conversion options
				WpfDrawingSettings settings = new();
				settings.IncludeRuntime = false;
				settings.TextAsGeometry = true;

				FileSvgReader converter = new(settings);

				DrawingGroup drawing = converter.Read(source);

				var newTab = TabFactory.CreatePlanTab(new DrawingImage(drawing));
				newTab.Header = openFileDialog.SafeFileName;
				tabControl.Items.Add(newTab);

				// reset position
				var tt = (TranslateTransform)((TransformGroup)image.RenderTransform).Children.First(tr => tr is TranslateTransform);
				tt.X = 0;
				tt.Y = 0;
			}

			
			RefreshPlusButton();
		}

		private void RefreshPlusButton() {
			var plusTab = tabControl.Items.OfType<TabItem>().SingleOrDefault(n => (string)n.Header == "+");
			tabControl.Items.Remove(plusTab);
			tabControl.Items.Add(plusTab);
		}

		#endregion

		#region Graphs

		private void buttonNewChart_Click(object sender, RoutedEventArgs e) {
			var contextMenu = gridCharts.ContextMenu;
			contextMenu.StaysOpen = true;
			contextMenu.IsOpen = true;
			e.Handled = true;
		}

		private void menuItemFull_Click(object sender, RoutedEventArgs e) {
			/*CartesianChart chart = new CartesianChart();
			var data = ((CSVData)MainDataManager.Data.Last()).GetColumn(1);
			var dates = ((CSVData)MainDataManager.Data.Last()).GetColumn(0);
			var doubles = new List<double>();
			var strings = new List<string>();
			for (int i = 0; i < data.Length; i++) {
				// TODO: more culture styles https://stackoverflow.com/questions/1354924/how-do-i-parse-a-string-with-a-decimal-point-to-a-double
				if (i % 10 == 0) {
					doubles.Add(double.Parse(data[i], NumberStyles.Any, CultureInfo.InvariantCulture));
					var time = DateTime.ParseExact(dates[i], "yyMMdd HH:mm:ss.fff", CultureInfo.InvariantCulture);
					strings.Add(time.ToString("dd/MM/yy"));
				}
			}

			var series = new ISeries[] { new LineSeries<double> { Values = doubles, Fill = null, GeometrySize = 0, EnableNullSplitting = false, DataPadding = new LiveChartsCore.Drawing.LvcPoint(0, 0) } };
			var axis = new Axis[] { new Axis { Labels = strings, MinStep = 5 } };

			chart.XAxes = axis;
			chart.Series = series;
			chart.ZoomMode = LiveChartsCore.Measure.ZoomAndPanMode.X;

			((DockPanel)((Button)sender).Parent).Children.Add(chart);
			((DockPanel)((Button)sender).Parent).Children.Remove((Button)sender);*/

		}

		// TODO: generalize, grid span
		private void AddChart(object sender) {
			CartesianChart chart = new CartesianChart();
			var data = ((CSVData)MainDataManager.Data.Last()).GetColumn(1);
			var dates = ((CSVData)MainDataManager.Data.Last()).GetColumn(0);
			var doubles = new List<double>();
			var strings = new List<string>();
			for (int i = 0; i < data.Length; i++) {
				// TODO: more culture styles https://stackoverflow.com/questions/1354924/how-do-i-parse-a-string-with-a-decimal-point-to-a-double
				if (i % 10 == 0) {
					doubles.Add(double.Parse(data[i], NumberStyles.Any, CultureInfo.InvariantCulture));
					var time = DateTime.ParseExact(dates[i], "yyMMdd HH:mm:ss.fff", CultureInfo.InvariantCulture);
					strings.Add(time.ToString("dd/MM/yy"));
				}
			}

			var series = new ISeries[] { new LineSeries<double> { Values = doubles, Fill = null, GeometrySize = 0, EnableNullSplitting = false, DataPadding = new LiveChartsCore.Drawing.LvcPoint(0, 0) } };
			var axis = new Axis[] { new Axis { Labels = strings, MinStep = 5 } };

			chart.XAxes = axis;
			chart.Series = series;
			chart.ZoomMode = LiveChartsCore.Measure.ZoomAndPanMode.X;

			((DockPanel)((Button)sender).Parent).Children.Add(chart);
			((DockPanel)((Button)sender).Parent).Children.Remove((Button)sender);
		}

		private void Button_Click(object sender, RoutedEventArgs e) {
			AddChart(sender);
		}

		private void Button_Click_1(object sender, RoutedEventArgs e) {
			AddChart(sender);
		}

		private void Button_Click_2(object sender, RoutedEventArgs e) {
			AddChart(sender);
		}

		private void Button_Click_3(object sender, RoutedEventArgs e) {
			AddChart(sender);
		}


		#endregion

		/*private bool isDragging;
		private Point mousePosition;
		private Double prevX, prevY;
		private void gridTest_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
			isDragging = true; 
			mousePosition = e.GetPosition(this);
			gridTest.CaptureMouse();
		}

		private void gridTest_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
			isDragging = false;
			var draggable = sender as Grid;
			var transform = draggable.RenderTransform as TranslateTransform;
			if (transform != null) {
				prevX = transform.X;
				prevY = transform.Y;
			}
			gridTest.ReleaseMouseCapture();
		}*/

		private void Button_Click_4(object sender, RoutedEventArgs e) {

			var drag = new MouseDragElementBehavior() {
				ConstrainToParentBounds = false
			};

			/*var x = testGrid;

			var str = XamlWriter.Save(x);
			StringReader stringReader = new StringReader(str);
			XmlReader xmlReader = XmlReader.Create(stringReader);
			var test = (Grid)XamlReader.Load(xmlReader);
			canvas.Children.Add(test);
			Interaction.GetBehaviors(test).Add(drag);*/

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
				Background = System.Windows.Media.Brushes.LightPink,
				BorderThickness = new(0),
				Text = "Example text",
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Center
			};

			System.Windows.Controls.Image i = new() {
				IsHitTestVisible = false,
				Stretch = Stretch.Uniform,
				Source = new BitmapImage(new Uri("pack://application:,,,/DataVisualizer;component/Resources/thermometer.png"))
			};

			t.SetValue(Grid.RowProperty, 0);
			i.SetValue(Grid.RowProperty, 1);

			g.Children.Add(t);
			g.Children.Add(i);

			//canvas.Children.Add(g);
			Interaction.GetBehaviors(g).Add(drag);



			var newGrid = GridFactory.CreateGrid("example text", "pack://application:,,,/DataVisualizer;component/Resources/thermometer.png");
			canvas.Children.Add(newGrid);
			Canvas.SetLeft(newGrid, (canvas.ActualWidth - newGrid.Width) / 2);
			Canvas.SetTop(newGrid, canvas.ActualHeight / 2);
			MainDataManager.PM.InteractiveGrids.Add(g);
		}

		private void Button_Click_5(object sender, RoutedEventArgs e) {
			MainDataManager.PM.SaveProject();
		}

		private void Button_Click_6(object sender, RoutedEventArgs e) {
			MainDataManager.PM.LoadProject();
			canvas.Children.Clear();
			foreach(var grid in MainDataManager.PM.InteractiveGrids) {
				canvas.Children.Add(grid);
			}

			// Conversion options
			WpfDrawingSettings settings = new();
			settings.IncludeRuntime = false;
			settings.TextAsGeometry = true;

			FileSvgReader converter = new(settings);

			DrawingGroup drawing = converter.Read(MainDataManager.PM.FloorplanPath);

			image.Source = new DrawingImage(drawing);

			// reset position
			var tt = (TranslateTransform)((TransformGroup)image.RenderTransform).Children.First(tr => tr is TranslateTransform);
			tt.X = 0;
			tt.Y = 0;
		}

		/*private void gridTest_MouseMove(object sender, MouseEventArgs e) {
			var draggableControl = sender as Grid;
			if (isDragging && draggableControl != null) {
				var currentPosition = e.GetPosition(Parent as UIElement);
				var transform = draggableControl.RenderTransform as TranslateTransform;
				if (transform == null) {
					transform = new TranslateTransform();
					draggableControl.RenderTransform = transform;
				}

				var deltaX = mousePosition.X - currentPosition.X;
				var deltaY = mousePosition.Y - currentPosition.Y;

				transform.X = prevX - deltaX / ZoomSlider.Value;
				transform.Y = prevY - deltaY / ZoomSlider.Value;
			}
		}*/
	}
}