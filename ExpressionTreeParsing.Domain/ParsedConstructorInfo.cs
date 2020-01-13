using System.Collections.Generic;
using System.Reflection;

namespace ExpressionTreeParsing.Domain
{
    public class ParsedConstructorInfo : ParsedMethodInfo
    {
        public ParsedConstructorInfo(ParsedType declaringType, IEnumerable<ParsedType> genericArguments, bool isPublic, bool isStatic, string name, IEnumerable<ParsedParameterInfo> parameters, ParsedType reflectedType, ParsedType returnType)
            : base(declaringType, genericArguments, isPublic, isStatic, name, parameters, reflectedType, returnType)
        {
        }

        public override MemberTypes MemberType => MemberTypes.Constructor;
    }
}
