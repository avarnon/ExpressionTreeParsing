using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionTreeParsing.Domain
{
    public class ParsedIndexExpression : ParsedExpression
    {
        public ParsedIndexExpression(
            ParsedExpression array,
            IEnumerable<ParsedExpression> indexes)
            : base()
        {
            this.Array = array;
            this.Indexes = indexes.ToArray();
        }

        public ParsedExpression Array { get; }

        public IEnumerable<ParsedExpression> Indexes { get; }

        public override ExpressionType NodeType => ExpressionType.ArrayIndex;
    }
}
