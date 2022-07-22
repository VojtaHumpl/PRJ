using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataVisualizer.Data {
	internal interface IData {
		public string Name { get; init; }

		public string GetValue(int row, int column);
	}
}
