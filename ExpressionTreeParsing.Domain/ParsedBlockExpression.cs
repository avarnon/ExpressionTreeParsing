using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionTreeParsing.Domain
{
    public class ParsedBlockExpression : ParsedExpression
    {
        public ParsedBlockExpression(
            IEnumerable<ParsedExpression> expressions,
            ParsedExpression result,
            ParsedType type,
            IEnumerable<ParsedParameterExpression> variables)
            : base()
        {
            this.Expressions = expressions.ToArray();
            this.Result = result;
            this.Type = type;
            this.Variables = variables.ToArray();
        }

        public IEnumerable<ParsedExpression> Expressions { get; }

        public override ExpressionType NodeType => ExpressionType.Block;

        public ParsedExpression Result { get; }

        public ParsedType Type { get; }

        public IEnumerable<ParsedParameterExpression> Variables { get; }
    }
}
