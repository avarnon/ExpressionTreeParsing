using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionTreeParsing.Domain
{
    public class ParsedListInitExpression : ParsedExpression
    {
        public ParsedListInitExpression(
            ParsedNewExpression newExpression,
            IEnumerable<ParsedElementInit> initializers)
            : base()
        {
            this.NewExpression = newExpression;
            this.Initializers = initializers.ToArray();
        }

        public IEnumerable<ParsedElementInit> Initializers { get; }

        public ParsedNewExpression NewExpression { get; }

        public override ExpressionType NodeType => ExpressionType.ListInit;
    }
}
