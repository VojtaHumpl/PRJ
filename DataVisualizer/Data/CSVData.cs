using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataVisualizer.Data {
	internal class CSVData : IData {
		internal string[] Header { get; init; }
		internal int HeaderIndex { get; init; }
		internal string[][] Rows { get; init; }
		internal string[][] BadRows { get; init; }
		public string Name { get; init; }

		public CSVData(string name, string[] header, int headerIndex, List<string[]> rows, List<string[]> badRows) {
			Name = name;
			Header = header;
			HeaderIndex = headerIndex;
			Rows = rows.ToArray();
			BadRows = badRows.ToArray();
		}

		internal List<string[]> GetAllColumns() {
			var columns = new List<string[]>();
			var totalColumns = Rows[0].Length;
			for (int i = 0; i < totalColumns; i++) {
				columns.Add(new string[Rows.Length - HeaderIndex - 1]);
			}

			for (int i = HeaderIndex + 1; i < Rows.Length; i++) {
				var words = Rows[i];
				for (int j = 0; j < totalColumns; j++) {
					if (j >= words.Length) {
						columns[j][i - HeaderIndex - 1] = "";
					} else {
						columns[j][i - HeaderIndex - 1] = words[j];
					}
				}
			}
			return columns;
		}

		internal string[] GetColumn(int index) {
			var totalColumns = Rows[0].Length;

			if (index >= totalColumns)
				throw new IndexOutOfRangeException();

			var res = new List<string>();
			for (int i = HeaderIndex + 1; i < Rows.Length; i++) {
				var words = Rows[i];
				res.Add(words[index]);
			}

			return res.ToArray();
		}

		public string GetValue(int row, int column) {
			var values = Rows[row];
			if (column >= values.Length)
				throw new IndexOutOfRangeException();
			return values[column];
		}

	}
}
