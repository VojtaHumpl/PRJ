using DataVisualizer.Data;
using DataVisualizer.ViewModels;

using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DataVisualizer {
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application {

		public static new App Current => Application.Current as App;


		internal DataManager MainDataManager { get; }

		public App() {
			MainDataManager = new();
		}

		protected override void OnStartup(StartupEventArgs e) {
			base.OnStartup(e);

			MainWindow = new MainWindow() {
				DataContext = new MainViewModel(MainDataManager)
			};

			LiveCharts.Configure(config =>
				config.AddSkiaSharp().AddDefaultMappers()
					.AddLightTheme()
					//.AddDarkTheme()
			);

			MainWindow.Show();
		}


	}
}
