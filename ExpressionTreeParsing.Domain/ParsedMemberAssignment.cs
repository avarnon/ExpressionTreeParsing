using System.Linq.Expressions;

namespace ExpressionTreeParsing.Domain
{
    public class ParsedMemberAssignment : ParsedMemberBinding
    {
        public ParsedMemberAssignment(
            ParsedMemberInfo member,
            ParsedExpression expression)
            : base(member)
        {
            this.Expression = expression;
        }

        public override MemberBindingType BindingType => MemberBindingType.Assignment;

        public ParsedExpression Expression { get; }
    }
}
