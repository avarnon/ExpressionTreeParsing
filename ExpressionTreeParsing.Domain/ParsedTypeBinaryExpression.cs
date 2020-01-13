using System.Linq.Expressions;

namespace ExpressionTreeParsing.Domain
{
    public class ParsedTypeBinaryExpression : ParsedExpression
    {
        public ParsedTypeBinaryExpression(
            ParsedExpression expression,
            ParsedType type)
            : base()
        {
            this.Expression = expression;
            this.Type = type;
        }

        public ParsedExpression Expression { get; }

        public override ExpressionType NodeType => ExpressionType.TypeEqual;

        public ParsedType Type { get; }
    }
}
