using DataVisualizer.Data;
using DataVisualizer.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataVisualizer.DataImporting {
	internal static class CSVImporter {
		internal static CSVData ImportCSV(string filename, bool errorBadLines = true) {
			
			var lines = File.ReadAllLines(Path.ChangeExtension(filename, ".csv"));
			var delimiter = "";

			// detect delimiter
			Dictionary<string, int> delimiters = new Dictionary<string, int>();
			delimiters.Add(";", lines[0].Count(c => c == ';') + lines[^1].Count(c => c == ';'));
			delimiters.Add("\t", lines[0].Count(c => c == '\t') + lines[^1].Count(c => c == '\t'));
			delimiters.Add(",", lines[0].Count(c => c == ',') + lines[^1].Count(c => c == ','));
			delimiters.Add("|", lines[0].Count(c => c == '|') + lines[^1].Count(c => c == '|'));
			delimiters.Add(":", lines[0].Count(c => c == ':') + lines[^1].Count(c => c == ':'));

			var max = delimiters.Values.Max();
			var unique = delimiters.Values.Count(n => n == max) == 1;

			if (!unique) {
				throw new FileFormatException("Cannot determine delimiter");
			}
			delimiter = delimiters.FirstOrDefault(d => d.Value == max).Key;

			// get words per line
			var totalColumns = lines[^1].Split(delimiter).Length;

			// get header
			string[] header = { };
			var headerIndex = 0;
			foreach (var line in lines) {
				var tmpHeader = line.Split(delimiter);
				if (tmpHeader.Length == totalColumns) {
					header = tmpHeader;
					break;
				}
				headerIndex++;
			}

			// get rows
			var rows = new List<string[]>();
			var badRows = new List<string[]>();
			foreach (var line in lines.Skip(headerIndex + 1)) {
				var tmpLine = line.Split(delimiter);
				if (tmpLine.Length == totalColumns) {
					rows.Add(tmpLine);
				} else {
					badRows.Add(tmpLine);
				}
			}

			return new CSVData(Path.GetFileName(filename), header, headerIndex, rows, badRows);
			
		}
	}
}
