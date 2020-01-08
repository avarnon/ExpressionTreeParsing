using System.Linq.Expressions;

namespace ExpressionTreeParsing.Domain
{
    public abstract class ParsedExpression
    {
        public abstract ExpressionType NodeType { get; }
    }
}
