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

namespace DataVisualizer {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {

		private Point origin;
		private Point start;
		private Slider slider;

		private bool FormLoaded { get; set; } = false;


		public MainWindow() {
			InitializeComponent();


			var source = @"..\..\..\res\floorplan.svg";

			// Conversion options
			WpfDrawingSettings settings = new ();
			settings.IncludeRuntime = false;
			settings.TextAsGeometry = true;

			FileSvgReader converter = new(settings);

			DrawingGroup drawing = converter.Read(source);

			image.Source = new DrawingImage(drawing);
			/*var x = new Bitmap(10, 10);
			using (Graphics gfx = Graphics.FromImage(x))
			using (SolidBrush brush = new SolidBrush(Color.FromArgb(255, 0, 0))) {
				gfx.FillRectangle(brush, 0, 0, 10, 10);
			}
			image.Source = BitmapToImageSource(x);*/

			//Setup a transform group that we'll use to manage panning of the image area
			TransformGroup group = new();
			ScaleTransform st = new();
			group.Children.Add(st);
			TranslateTransform tt = new ();
			group.Children.Add(tt);
			//Wire up the slider to the image for zooming
			slider = zoomSlider;
			st.ScaleX = slider.Value;
			st.ScaleY = slider.Value;
			//_ImageScrollArea.RenderTransformOrigin = new Point(0.5, 0.5);
			//_ImageScrollArea.LayoutTransform = group;
			image.RenderTransformOrigin = new Point(0.5, 0.5);
			image.RenderTransform = group;
			//_ROICollectionCanvas.RenderTransformOrigin = new Point(0.5, 0.5);
			//_ROICollectionCanvas.RenderTransform = group;

			
		}

		private BitmapImage BitmapToImageSource(Bitmap bitmap) {
			using (MemoryStream memory = new MemoryStream()) {
				bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
				memory.Position = 0;
				BitmapImage bitmapimage = new BitmapImage();
				bitmapimage.BeginInit();
				bitmapimage.StreamSource = memory;
				bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
				bitmapimage.EndInit();
				return bitmapimage;
			}
		}

		private void Window_Loaded(object sender, RoutedEventArgs e) {
			//string expression = "1+1";
			string expression = "((x, y) => x * y)(4, 2)";		// can handle C# code
			//string expression = "5+5+8";		// can handle C# code

			ExpressionEvaluator evaluator = new ();
			Debug.WriteLine(expression);
			Debug.WriteLine(evaluator.Evaluate(expression));

			FormLoaded = true;
		}

		private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
			image.ReleaseMouseCapture();
		}

		private void Image_MouseMove(object sender, MouseEventArgs e) {
			if (!image.IsMouseCaptured) return;

			var tt = (TranslateTransform)((TransformGroup)image.RenderTransform).Children.First(tr => tr is TranslateTransform);

			Vector v = start - e.GetPosition(border);
			tt.X = origin.X - v.X;
			tt.Y = origin.Y - v.Y;
		}

		private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
			image.CaptureMouse();
			var tt = (TranslateTransform)((TransformGroup)image.RenderTransform).Children.First(tr => tr is TranslateTransform);
			start = e.GetPosition(border);
			origin = new Point(tt.X, tt.Y);
		}

		private void ZoomSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
			if (!FormLoaded)
				return;

			//Panel panel = _ImageScrollArea;
			System.Windows.Controls.Image panel = image;

			//Set the scale coordinates on the ScaleTransform from the slider
			ScaleTransform transform = (ScaleTransform)((TransformGroup)panel.RenderTransform).Children.First(tr => tr is ScaleTransform);
			transform.ScaleX = slider.Value;
			transform.ScaleY = slider.Value;


			//Set the zoom (this will affect rotate too) origin to the center of the panel
			panel.RenderTransformOrigin = new Point(0.5, 0.5);

			/*foreach (UIElement child in _ROICollectionCanvas.Children) {
				//Assume all shapes are contained in a panel
				Panel childPanel = child as Panel;

				var x = childPanel.Children;

				//Shape width and heigh should scale, but not StrokeThickness
				foreach (var shape in childPanel.Children.OfType<Shape>()) {
					if (shape.Tag == null) {
						//Hack: This is be a property on a usercontrol in my solution
						shape.Tag = shape.StrokeThickness;
					}
					double orignalStrokeThickness = (double)shape.Tag;

					//Attempt to keep the underlying shape border/stroke from thickening as well
					double newThickness = shape.StrokeThickness - (orignalStrokeThickness / transform.ScaleX);

					shape.StrokeThickness -= newThickness;
				}
			}*/
		}

		private void Window_MouseWheel(object sender, MouseWheelEventArgs e) {
			slider.Value += slider.SmallChange * e.Delta / 60;
		}
	}
}