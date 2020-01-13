using System.Linq.Expressions;

namespace ExpressionTreeParsing.Domain
{
    public class ParsedNewExpression : ParsedExpression
    {
        public ParsedNewExpression(
            ParsedConstructorInfo constructor)
            : base()
        {
            this.Constructor = constructor;
        }

        public ParsedConstructorInfo Constructor { get; }

        public override ExpressionType NodeType => ExpressionType.New;
    }
}
