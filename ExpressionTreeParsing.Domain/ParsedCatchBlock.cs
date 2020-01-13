namespace ExpressionTreeParsing.Domain
{
    public class ParsedCatchBlock
    {
        public ParsedCatchBlock(
            ParsedType type,
            ParsedParameterExpression variable,
            ParsedExpression filter,
            ParsedExpression body)
        {
            this.Type = type;
            this.Variable = variable;
            this.Filter = filter;
            this.Body = body;
        }

        public ParsedExpression Body { get; }

        public ParsedExpression Filter { get; }

        public ParsedType Type { get; }

        public ParsedParameterExpression Variable { get; }
    }
}
