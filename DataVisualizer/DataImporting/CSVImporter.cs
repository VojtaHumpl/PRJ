using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataVisualizer.DataImporting {
	internal static class CSVImporter {
		internal static List<string[]>? ImportCSV(string filename) {
			try {
				var lines = File.ReadAllLines(Path.ChangeExtension(filename, ".csv"));
				var csv = new List<string[]>();

				foreach (var line in lines) {
					csv.Add(line.Split(";"));
				}

				return csv;
			} catch (Exception e) {
				Debug.WriteLine(e);
			}
			return null;
		}
	}
}
