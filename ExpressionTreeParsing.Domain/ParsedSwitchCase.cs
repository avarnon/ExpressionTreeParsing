using System.Collections.Generic;
using System.Linq;

namespace ExpressionTreeParsing.Domain
{
    public class ParsedSwitchCase
    {
        public ParsedSwitchCase(
            ParsedExpression body,
            IEnumerable<ParsedExpression> testValues)
        {
            this.Body = body;
            this.TestValues = testValues.ToArray();
        }

        public ParsedExpression Body { get; }

        public IEnumerable<ParsedExpression> TestValues { get; }
    }
}
