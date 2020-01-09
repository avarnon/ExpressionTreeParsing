using System.Linq.Expressions;

namespace ExpressionTreeParsing.Domain
{
    public class ParsedDebugInfoExpression : ParsedExpression
    {
        public ParsedDebugInfoExpression(
            int endColumn,
            int endLine,
            ParsedSymbolDocumentInfo document,
            bool isClear,
            int startColumn,
            int startLine)
            : base()
        {
            this.EndColumn = endColumn;
            this.EndLine = endLine;
            this.IsClear = isClear;
            this.StartColumn = startColumn;
            this.StartLine = startLine;
            this.Document = document;
        }

        public int EndColumn { get; }

        public int EndLine { get; }

        public ParsedSymbolDocumentInfo Document { get; }

        public bool IsClear { get; }

        public override ExpressionType NodeType => ExpressionType.DebugInfo;

        public int StartColumn { get; }

        public int StartLine { get; }
    }
}
