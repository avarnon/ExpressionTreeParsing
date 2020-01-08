using System.Linq.Expressions;

namespace ExpressionTreeParsing.Domain
{
    public class ParsedConstantExpression : ParsedExpression
    {
        public ParsedConstantExpression(
            ParsedType type,
            object value)
            : base()
        {
            this.Type = type;
            this.Value = value;
        }

        public ParsedType Type { get; }

        public object Value { get; }

        public override ExpressionType NodeType => ExpressionType.Constant;
    }
}
