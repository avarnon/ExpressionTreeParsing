using System.Linq.Expressions;

namespace ExpressionTreeParsing.Domain
{
    public class ParsedConditionalExpression : ParsedExpression
    {
        public ParsedConditionalExpression(
            ParsedExpression ifFalse,
            ParsedExpression ifTrue,
            ParsedExpression test,
            ParsedType type)
            : base()
        {
            this.IfFalse = ifFalse;
            this.IfTrue = ifTrue;
            this.Test = test;
            this.Type = type;
        }

        public ParsedExpression IfFalse { get; }

        public ParsedExpression IfTrue { get; }

        public override ExpressionType NodeType => ExpressionType.Conditional;

        public ParsedExpression Test { get; }

        public ParsedType Type { get; }
    }
}
