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

        public override ExpressionType NodeType => ExpressionType.Constant;

        public ParsedType Type { get; }

        public object Value { get; }
    }
}
