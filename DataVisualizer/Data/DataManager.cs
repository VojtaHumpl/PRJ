using DataVisualizer.Data.ProjectManagement;
using DataVisualizer.DataImporting;


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataVisualizer.Data {
	internal class DataManager {
		internal List<IData> Data { get; private set; } = new List<IData>();
		internal ProjectManager PM { get; set; }

		internal DataManager() {}

		internal bool ImportData(string path) {
			if (!File.Exists(path)) {
				throw new FileNotFoundException($"File {path} does not exist");
			}

			var extension = Path.GetExtension(path).ToLower();

			switch (extension) {
				case ".csv":
					Data.Add(CSVImporter.ImportCSV(path));
					return true;
				default:
					throw new NotSupportedException($"File extension {extension} is not supported");
			}
		}
	}
}
