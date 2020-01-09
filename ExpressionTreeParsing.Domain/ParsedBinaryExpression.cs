using System.Linq.Expressions;

namespace ExpressionTreeParsing.Domain
{
    public class ParsedBinaryExpression : ParsedExpression
    {
        public ParsedBinaryExpression(
            ParsedLambdaExpression conversion,
            ParsedExpression left,
            ParsedMethodInfo method,
            ExpressionType nodeType,
            ParsedExpression right)
            : base()
        {
            this.Conversion = conversion;
            this.Left = left;
            this.Method = method;
            this.NodeType = nodeType;
            this.Right = right;
        }

        public ParsedLambdaExpression Conversion { get; }

        public ParsedExpression Left { get; }

        public ParsedMethodInfo Method { get; }

        public override ExpressionType NodeType { get; }

        public ParsedExpression Right { get; }
    }
}
