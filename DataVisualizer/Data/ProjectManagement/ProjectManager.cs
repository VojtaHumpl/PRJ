using Microsoft.Win32;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Linq;

namespace DataVisualizer.Data.ProjectManagement {
	internal class ProjectManager {
		// load save

		internal Canvas MainCanvas { get; init; }
		internal List<Grid> InteractiveGrids { get; set; } = new();
		internal string FloorplanPath { get; set; } = "";
		internal string DatasetPath { get; set; } = "";

		public ProjectManager(Canvas canvas) {
			MainCanvas = canvas;
		}

		internal void SaveProject() {
			var saveFileDialog = new SaveFileDialog {
				Filter = "Data Visualizer Project|*.xml",
				Title = "Save Project",
				FileName = "project1",
				DefaultExt = "xml",
				OverwritePrompt = true,
				ValidateNames = true
			};

			var doc = new XDocument(
					new XElement("Project")
			);
			

			if (saveFileDialog.ShowDialog() == true) {

				var dirPath = Path.ChangeExtension(saveFileDialog.FileName, null);
				var dirName = Path.GetFileName(Path.ChangeExtension(saveFileDialog.FileName, null));
				var floorplanName = Path.GetFileName(FloorplanPath);
				var datasetName = Path.GetFileName(DatasetPath);
				Directory.CreateDirectory(dirPath);

				// save floorplans
				if (File.Exists(FloorplanPath))
					File.Copy(FloorplanPath, $"{dirPath}/{floorplanName}");
				FloorplanPath = floorplanName;
				var floorplan = new XElement("floorplan", FloorplanPath);
				doc.Root!.Add(floorplan);

				// save datasets
				if (File.Exists(DatasetPath))
					File.Copy(DatasetPath, $"{dirPath}/{datasetName}");
				DatasetPath = datasetName;
				var dataset = new XElement("dataset", DatasetPath);
				doc.Root!.Add(dataset);

				// save grids
				foreach (var child in MainCanvas.Children) {
					if (child is Grid grid) {
						var gelement = new XElement("grid");
						var text = new XElement("text", ((TextBox)grid.Children[0]).Text);
						var image = new XElement("image", ((Image)grid.Children[1]).Source);
						//var x = new XElement("X", grid.RenderTransform.Value.OffsetX + MainCanvas.RenderTransform.Value.OffsetX);
						//var y = new XElement("Y", grid.RenderTransform.Value.OffsetY + MainCanvas.RenderTransform.Value.OffsetY);
						var location = grid.TranslatePoint(new Point(0, 0), MainCanvas);
						var x = new XElement("X", location.X);
						var y = new XElement("Y", location.Y);

						gelement.Add(text);
						gelement.Add(image);
						gelement.Add(x);
						gelement.Add(y);
						doc.Root!.Add(gelement);
					}
				}

				doc.Save($"{dirPath}/{saveFileDialog.SafeFileName}");
			}

		}

		internal void LoadProject() {
			InteractiveGrids.Clear();

			OpenFileDialog openFileDialog = new() {
				Filter = "Data Visualizer Project|*.xml",
				CheckFileExists = true
			};

			if (openFileDialog.ShowDialog() == true) {
				var doc = XDocument.Load(openFileDialog.FileName);

				// load floorplan
				FloorplanPath = $"{Directory.GetParent(openFileDialog.FileName)}/{doc.Root?.Element("floorplan")?.Value}";

				// load grids
				foreach(var item in doc.Root.Descendants("grid")) {
					var grid = GridFactory.CreateGrid(item.Elements().ElementAt(0).Value, item.Elements().ElementAt(1).Value);
					Canvas.SetLeft(grid, double.Parse(item.Elements().ElementAt(2).Value, CultureInfo.InvariantCulture));
					Canvas.SetTop(grid, double.Parse(item.Elements().ElementAt(3).Value, CultureInfo.InvariantCulture));			
					InteractiveGrids.Add(grid);
				}
			}
		}

		internal void LoadLastProject() {
			throw new NotImplementedException();
		}
	}
}
