using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

using static LangParser;

namespace DataVisualizer.ExpressionEvaluation {
	internal class ExpressionVisitor : LangBaseVisitor<double> {

		public override double VisitNumber([NotNull] NumberContext context) {
			return double.Parse(context.GetText());
		}

		public override double VisitCell([NotNull] CellContext context) {
			var cell = context.GetText();

			return 1d;
		}

		public override double VisitParentheses([NotNull] ParenthesesContext context) {
			return this.Visit(context.inner);
		}

		public override double VisitAdditionOrSubtraction([NotNull] AdditionOrSubtractionContext context) {
			if (context.@operator.Text == "+") {
				return this.Visit(context.left) + this.Visit(context.right);
			} else {
				return this.Visit(context.left) - this.Visit(context.right);
			}
		}

		public override double VisitMultiplicationOrDivision([NotNull] MultiplicationOrDivisionContext context) {
			if (context.@operator.Text == "*") {
				return this.Visit(context.left) * this.Visit(context.right);
			} else {
				return this.Visit(context.left) / this.Visit(context.right);
			}
		}

		public override double VisitAverageOfRange([NotNull] AverageOfRangeContext context) {
			return base.VisitAverageOfRange(context);
		}


	}
}
