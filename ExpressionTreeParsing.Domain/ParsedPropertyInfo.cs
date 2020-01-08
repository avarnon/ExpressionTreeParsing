using System.Reflection;

namespace ExpressionTreeParsing.Domain
{
    public class ParsedPropertyInfo : ParsedMemberInfo
    {
        public ParsedPropertyInfo(ParsedType declaringType, string name, ParsedType propertyType, ParsedType reflectedType)
            : base(declaringType, name, reflectedType)
        {
            this.PropertyType = propertyType;
        }

        public override MemberTypes MemberTypes => MemberTypes.Property;

        public ParsedType PropertyType { get; }
    }
}
