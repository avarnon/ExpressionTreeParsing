using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionTreeParsing.Domain
{
    public class ParsedMemberListBinding : ParsedMemberBinding
    {
        public ParsedMemberListBinding(
            ParsedMemberInfo member,
            IEnumerable<ParsedElementInit> initializers)
            : base(member)
        {
            this.Initializers = initializers.ToArray();
        }

        public override MemberBindingType BindingType => MemberBindingType.ListBinding;

        public IEnumerable<ParsedElementInit> Initializers { get; }
    }
}
