//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.9.2
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from f:\Google Drive\Skola\VS\TUL\Projekt\DataVisualizer\DataVisualizer\ExpressionEvaluation\Lang.g4 by ANTLR 4.9.2

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete generic visitor for a parse tree produced
/// by <see cref="LangParser"/>.
/// </summary>
/// <typeparam name="Result">The return type of the visit operation.</typeparam>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.9.2")]
[System.CLSCompliant(false)]
public interface ILangVisitor<Result> : IParseTreeVisitor<Result> {
	/// <summary>
	/// Visit a parse tree produced by <see cref="LangParser.start"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitStart([NotNull] LangParser.StartContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>Number</c>
	/// labeled alternative in <see cref="LangParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitNumber([NotNull] LangParser.NumberContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>AdditionOrSubtraction</c>
	/// labeled alternative in <see cref="LangParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAdditionOrSubtraction([NotNull] LangParser.AdditionOrSubtractionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>MultiplicationOrDivision</c>
	/// labeled alternative in <see cref="LangParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMultiplicationOrDivision([NotNull] LangParser.MultiplicationOrDivisionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>AverageOfRange</c>
	/// labeled alternative in <see cref="LangParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAverageOfRange([NotNull] LangParser.AverageOfRangeContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>Cell</c>
	/// labeled alternative in <see cref="LangParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCell([NotNull] LangParser.CellContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>Parentheses</c>
	/// labeled alternative in <see cref="LangParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitParentheses([NotNull] LangParser.ParenthesesContext context);
}