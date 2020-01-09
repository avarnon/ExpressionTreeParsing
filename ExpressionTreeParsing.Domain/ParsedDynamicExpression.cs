using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionTreeParsing.Domain
{
    public class ParsedDynamicExpression : ParsedExpression
    {
        public ParsedDynamicExpression(
            IEnumerable<ParsedExpression> arguments,
            ParsedType delegateType)
            : base()
        {
            this.Arguments = arguments.ToArray();
            this.DelegateType = delegateType;
        }

        public IEnumerable<ParsedExpression> Arguments { get; }

        public ParsedType DelegateType { get; }

        public override ExpressionType NodeType => ExpressionType.Dynamic;
    }
}
