using System.Linq.Expressions;

namespace ExpressionTreeParsing.Domain
{
    public class ParsedLoopExpression : ParsedExpression
    {
        public ParsedLoopExpression(
            ParsedExpression body,
            ParsedLabelTarget @break,
            ParsedLabelTarget @continue)
            : base()
        {
            this.Body = body;
            this.Break = @break;
            this.Continue = @continue;
        }

        public ParsedExpression Body { get; }

        public ParsedLabelTarget Break { get; }

        public ParsedLabelTarget Continue { get; }

        public override ExpressionType NodeType => ExpressionType.Loop;
    }
}
