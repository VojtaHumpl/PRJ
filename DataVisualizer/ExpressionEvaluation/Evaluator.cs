using Antlr4.Runtime;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataVisualizer.ExpressionEvaluation {
	internal class Evaluator {
		internal static double Calculate(string input) {
			var chars = CharStreams.fromString(input);

			var lexer = new LangLexer(chars);
			var tokens = new CommonTokenStream(lexer);

			var parser = new LangParser(tokens);
			var tree = parser.start();

			ExpressionVisitor calculator = new();
			return calculator.Visit(tree);
		}
	}
}
