using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

using DataVisualizer.Data;

using Microsoft.Win32;

namespace DataVisualizer.Commands {
	internal class ImportDataCommand : CommandBase {

		/*private readonly DataManager _dataManager;

		public ImportDataCommand(DataManager dataManager) {
			_dataManager = dataManager;
		}*/

		public override void Execute(object? parameter) {
			/*OpenFileDialog openFileDialog = new();
			openFileDialog.Multiselect = true;
			openFileDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
			openFileDialog.RestoreDirectory = false;
			openFileDialog.CheckFileExists = true;
			if (openFileDialog.ShowDialog() == true) {
				var source = openFileDialog.FileName;

				try {
					var res = _dataManager.ImportData(source);
					if (!res)
						return;

					if (_dataManager.Data.Last() is CSVData data) {
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
			}*/
		}
	}
}
