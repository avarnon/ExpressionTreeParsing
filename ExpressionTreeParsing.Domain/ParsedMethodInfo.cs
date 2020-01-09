using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ExpressionTreeParsing.Domain
{
    public class ParsedMethodInfo : ParsedMemberInfo
    {
        public ParsedMethodInfo(ParsedType declaringType, IEnumerable<ParsedType> genericArguments, bool isPublic, bool isStatic, string name, IEnumerable<ParsedParameterInfo> parameters, ParsedType reflectedType, ParsedType returnType)
            : base(declaringType, name, reflectedType)
        {
            this.GenericArguments = genericArguments.ToArray();
            this.IsPublic = isPublic;
            this.IsStatic = isStatic;
            this.Parameters = parameters.ToArray();
            this.ReturnType = returnType;
        }

        public IEnumerable<ParsedType> GenericArguments { get; }

        public bool IsPublic { get; }

        public bool IsStatic { get; }

        public override MemberTypes MemberType => MemberTypes.Method;

        public IEnumerable<ParsedParameterInfo> Parameters { get; }

        public ParsedType ReturnType { get; }
    }
}
