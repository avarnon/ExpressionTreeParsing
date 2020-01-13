using System.Linq.Expressions;

namespace ExpressionTreeParsing.Domain
{
    public abstract class ParsedMemberBinding
    {
        protected ParsedMemberBinding(ParsedMemberInfo member)
        {
            this.Member = member;
        }

        public abstract MemberBindingType BindingType { get; }

        public ParsedMemberInfo Member { get; }
    }
}
