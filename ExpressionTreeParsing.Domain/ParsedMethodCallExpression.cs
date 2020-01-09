using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionTreeParsing.Domain
{
    public class ParsedMethodCallExpression : ParsedExpression
    {
        public ParsedMethodCallExpression(
            IEnumerable<ParsedExpression> arguments,
            ParsedMethodInfo method,
            ParsedExpression @object,
            ParsedType type)
            : base()
        {
            this.Arguments = arguments.ToArray();
            this.Method = method;
            this.Object = @object;
            this.Type = type;
        }

        public IEnumerable<ParsedExpression> Arguments { get; }

        public ParsedMethodInfo Method { get; }

        public override ExpressionType NodeType => ExpressionType.Call;

        public ParsedExpression Object { get; }

        public ParsedType Type { get; }
    }
}
