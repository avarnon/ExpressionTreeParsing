using System.Reflection;

namespace ExpressionTreeParsing.Domain
{
    public class ParsedFieldInfo : ParsedMemberInfo
    {
        public ParsedFieldInfo(ParsedType declaringType, ParsedType fieldType, string name, ParsedType reflectedType)
            : base(declaringType, name, reflectedType)
        {
            this.FieldType = fieldType;
        }

        public ParsedType FieldType { get; }

        public override MemberTypes MemberTypes => MemberTypes.Field;
    }
}
