using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionTreeParsing.Domain
{
    public class ParsedMethodCallExpression : ParsedExpression
    {
        public ParsedMethodCallExpression(
            IEnumerable<ParsedExpression> arguments,
            ParsedExpression instance,
            ParsedMethodInfo method,
            ParsedType type)
            : base()
        {
            this.Arguments = arguments.ToArray();
            this.Instance = instance;
            this.Method = method;
            this.Type = type;
        }

        public IEnumerable<ParsedExpression> Arguments { get; }

        public ParsedExpression Instance { get; }

        public ParsedMethodInfo Method { get; }

        public override ExpressionType NodeType => ExpressionType.Call;

        public ParsedType Type { get; }
    }
}
