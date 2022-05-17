using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataVisualizer.ViewModels {
	internal class ViewModel {
		public ISeries[] Series { get; set; } = new ISeries[] {
			new LineSeries<double> {
				Values = new double[] { 2, 1, 3, 5, 3, 4, 6 },
				Fill = null
			}
		};

		public ISeries[] Series2 { get; set; } = new ISeries[] {
			new LineSeries<double> {
				Values = new double[] { 2, 1, 3, 5, 3, 4, 6, 5, 9, 41, 21, 20, 19 },
				Fill = null
			}
		};
	}
}
