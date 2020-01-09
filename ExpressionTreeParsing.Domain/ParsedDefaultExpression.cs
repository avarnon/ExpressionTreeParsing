using System.Linq.Expressions;

namespace ExpressionTreeParsing.Domain
{
    public class ParsedDefaultExpression : ParsedExpression
    {
        public ParsedDefaultExpression(
            ParsedType type)
            : base()
        {
            this.Type = type;
        }

        public override ExpressionType NodeType => ExpressionType.Default;

        public ParsedType Type { get; }
    }
}
