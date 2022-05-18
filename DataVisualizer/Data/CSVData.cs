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


		public CSVData(string[] header, int headerIndex, List<string[]> rows, List<string[]> badRows) {
			Header = header;
			HeaderIndex = headerIndex;
			Rows = rows.ToArray();
			BadRows = badRows.ToArray();
		}

		internal List<string[]> GetColumns() {
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
	}
}
