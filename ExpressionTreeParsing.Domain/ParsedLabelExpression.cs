using System.Linq.Expressions;

namespace ExpressionTreeParsing.Domain
{
    public class ParsedLabelExpression : ParsedExpression
    {
        public ParsedLabelExpression(
            ParsedLabelTarget target,
            ParsedExpression defaultValue)
            : base()
        {
            this.Target = target;
            this.DefaultValue = defaultValue;
        }

        public ParsedExpression DefaultValue { get; }

        public override ExpressionType NodeType => ExpressionType.Label;

        public ParsedLabelTarget Target { get; }
    }
}
