using System.Reflection;

namespace ExpressionTreeParsing.Domain
{
    public class ParsedFieldInfo : ParsedMemberInfo
    {
        public ParsedFieldInfo(
            ParsedType declaringType,
            ParsedType fieldType,
            string name,
            bool isPublic,
            bool isStatic,
            ParsedType reflectedType)
            : base(declaringType, name, reflectedType)
        {
            this.FieldType = fieldType;
            this.IsPublic = isPublic;
            this.IsStatic = isStatic;
        }

        public ParsedType FieldType { get; }

        public bool IsPublic { get; }

        public bool IsStatic { get; }

        public override MemberTypes MemberType => MemberTypes.Field;
    }
}
