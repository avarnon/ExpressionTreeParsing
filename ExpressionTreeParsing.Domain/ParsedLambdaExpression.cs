using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionTreeParsing.Domain
{
    public class ParsedLambdaExpression : ParsedExpression
    {
        public ParsedLambdaExpression(
            ParsedExpression body,
            IEnumerable<ParsedParameterExpression> parameters,
            ParsedType returnType)
            : base()
        {
            this.Body = body;
            this.Parameters = parameters.ToArray();
            this.ReturnType = returnType;
        }

        public ParsedExpression Body { get; }

        public override ExpressionType NodeType => ExpressionType.Lambda;

        public IEnumerable<ParsedParameterExpression> Parameters { get; }

        public ParsedType ReturnType { get; }
    }
}
