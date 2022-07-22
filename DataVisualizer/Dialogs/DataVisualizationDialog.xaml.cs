using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DataVisualizer.Dialogs {
	/// <summary>
	/// Interaction logic for DataVisualizationDialog.xaml
	/// </summary>
	public partial class DataVisualizationDialog : Window {

		public DataVisualizationDialog() {
			InitializeComponent();

			foreach (var item in App.Current.MainDataManager.Data) {
				comboBoxDataset.Items.Add(item);
			}
		}

		private void buttonOK_Click(object sender, RoutedEventArgs e) {
			DialogResult = true;
		}

		private void buttonCancel_Click(object sender, RoutedEventArgs e) {
			DialogResult = false;
		}
		
	}
}
