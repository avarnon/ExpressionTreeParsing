using System.Linq.Expressions;

namespace ExpressionTreeParsing.Domain
{
    public class ParsedMemberExpression : ParsedExpression
    {
        public ParsedMemberExpression(
            ParsedExpression expression,
            ParsedMemberInfo member,
            ParsedType type)
            : base()
        {
            this.Expression = expression;
            this.Member = member;
            this.Type = type;
        }

        public ParsedExpression Expression { get; }

        public ParsedMemberInfo Member { get; }

        public override ExpressionType NodeType => ExpressionType.MemberAccess;

        public ParsedType Type { get; }
    }
}
