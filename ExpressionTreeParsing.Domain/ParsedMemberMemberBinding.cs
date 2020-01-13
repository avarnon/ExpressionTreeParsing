using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionTreeParsing.Domain
{
    public class ParsedMemberMemberBinding : ParsedMemberBinding
    {
        public ParsedMemberMemberBinding(
            ParsedMemberInfo member,
            IEnumerable<ParsedMemberBinding> bindings)
            : base(member)
        {
            this.Bindings = bindings.ToArray();
        }

        public IEnumerable<ParsedMemberBinding> Bindings { get; }

        public override MemberBindingType BindingType => MemberBindingType.MemberBinding;
    }
}
