using System.Linq.Expressions;

namespace ExpressionTreeParsing.Domain
{
    public class ParsedParameterExpression : ParsedExpression
    {
        public ParsedParameterExpression(
            bool isByRef,
            string name,
            ParsedType type)
            : base()
        {
            this.IsByRef = isByRef;
            this.Name = name;
            this.Type = type;
        }

        public bool IsByRef { get; }

        public string Name { get; }

        public override ExpressionType NodeType => ExpressionType.Parameter;

        public ParsedType Type { get; }
    }
}
