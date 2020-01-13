using System.Collections.Generic;
using System.Linq.Expressions;

namespace ExpressionTreeParsing.Domain
{
    public class ParsedMemberInitExpression : ParsedExpression
    {
        public ParsedMemberInitExpression(
            ParsedNewExpression newExpression,
            IEnumerable<ParsedMemberBinding> bindings)
            : base()
        {
            this.NewExpression = newExpression;
            this.Bindings = bindings;
        }

        public IEnumerable<ParsedMemberBinding> Bindings { get; }

        public override ExpressionType NodeType => ExpressionType.MemberInit;

        public ParsedNewExpression NewExpression { get; }
    }
}
