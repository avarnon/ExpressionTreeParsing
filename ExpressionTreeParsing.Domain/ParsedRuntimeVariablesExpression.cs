using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionTreeParsing.Domain
{
    public class ParsedRuntimeVariablesExpression : ParsedExpression
    {
        public ParsedRuntimeVariablesExpression(
            IEnumerable<ParsedParameterExpression> variables)
            : base()
        {
            this.Variables = variables.ToArray();
        }

        public override ExpressionType NodeType => ExpressionType.RuntimeVariables;

        public IEnumerable<ParsedParameterExpression> Variables { get; }
    }
}
