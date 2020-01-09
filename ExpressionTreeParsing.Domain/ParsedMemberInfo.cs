using System.Reflection;

namespace ExpressionTreeParsing.Domain
{
    public abstract class ParsedMemberInfo
    {
        protected ParsedMemberInfo(ParsedType declaringType, string name, ParsedType reflectedType)
        {
            this.DeclaringType = declaringType;
            this.Name = name;
            this.ReflectedType = reflectedType;
        }

        public ParsedType DeclaringType { get; }

        public string Name { get; }

        public abstract MemberTypes MemberType { get; }

        public ParsedType ReflectedType { get; }
    }
}
