using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionTreeParsing.Domain
{
    public class ParsedLambdaExpression : ParsedExpression
    {
        public ParsedLambdaExpression(
            ParsedExpression body,
            string name,
            IEnumerable<ParsedParameterExpression> parameters,
            ParsedType returnType,
            bool tailCall)
            : base()
        {
            this.Body = body;
            this.Name = name;
            this.Parameters = parameters.ToArray();
            this.ReturnType = returnType;
            this.TailCall = tailCall;
        }

        public ParsedExpression Body { get; }

        public override ExpressionType NodeType => ExpressionType.Lambda;

        public string Name { get; }

        public IEnumerable<ParsedParameterExpression> Parameters { get; }

        public ParsedType ReturnType { get; }

        public bool TailCall { get; }
    }
}
