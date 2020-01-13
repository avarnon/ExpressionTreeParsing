using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionTreeParsing.Domain
{
    public class ParsedInvocationExpression : ParsedExpression
    {
        public ParsedInvocationExpression(
            ParsedExpression expression,
            IEnumerable<ParsedExpression> arguments)
            : base()
        {
            this.Expression = expression;
            this.Arguments = arguments.ToArray();
        }

        public ParsedExpression Expression { get; }

        public IEnumerable<ParsedExpression> Arguments { get; }

        public override ExpressionType NodeType => ExpressionType.Invoke;
    }
}
