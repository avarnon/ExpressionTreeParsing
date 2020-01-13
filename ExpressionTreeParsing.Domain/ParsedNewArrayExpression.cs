using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionTreeParsing.Domain
{
    public class ParsedNewArrayExpression : ParsedExpression
    {
        public ParsedNewArrayExpression(
            ParsedType type,
            IEnumerable<ParsedExpression> expressions,
            ExpressionType nodeType)
            : base()
        {
            this.Type = type;
            this.Expressions = expressions.ToArray();
            this.NodeType = nodeType;
        }

        public IEnumerable<ParsedExpression> Expressions { get; }

        public override ExpressionType NodeType { get; }

        public ParsedType Type { get; }
    }
}
