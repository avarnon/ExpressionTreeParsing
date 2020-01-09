using System.Linq.Expressions;

namespace ExpressionTreeParsing.Domain
{
    public class ParsedGotoExpression : ParsedExpression
    {
        public ParsedGotoExpression(
            GotoExpressionKind kind,
            ParsedLabelTarget target,
            ParsedType type,
            ParsedExpression value)
            : base()
        {
            this.Kind = kind;
            this.Target = target;
            this.Type = type;
            this.Value = value;
        }

        public GotoExpressionKind Kind { get; }

        public sealed override ExpressionType NodeType => ExpressionType.Goto;

		public ParsedLabelTarget Target { get; }

        public ParsedType Type { get; }

        public ParsedExpression Value { get; }
    }
}
