using System.Linq.Expressions;

namespace ExpressionTreeParsing.Domain
{
    public class ParsedUnaryExpression : ParsedExpression
    {
        public ParsedUnaryExpression(
            ParsedExpression operand,
            ParsedMethodInfo method,
            ExpressionType nodeType,
            ParsedType type)
            : base()
        {
            this.Operand = operand;
            this.Method = method;
            this.NodeType = nodeType;
            this.Type = type;
        }

        public ParsedExpression Operand { get; }

        public ParsedMethodInfo Method { get; }

        public override ExpressionType NodeType { get; }

        public ParsedType Type { get; }
    }
}
