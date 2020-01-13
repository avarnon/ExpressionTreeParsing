using System.Collections.Generic;
using System.Linq;

namespace ExpressionTreeParsing.Domain
{
    public class ParsedElementInit
    {
        public ParsedElementInit(
            ParsedMethodInfo addMethod,
            IEnumerable<ParsedExpression> arguments)
        {
            this.AddMethod = addMethod;
            this.Arguments = arguments.ToArray();
        }

        public ParsedMethodInfo AddMethod { get; }

        public IEnumerable<ParsedExpression> Arguments { get; }
    }
}
