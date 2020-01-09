namespace ExpressionTreeParsing.Domain
{
    public class ParsedLabelTarget
    {
        public ParsedLabelTarget(
            string name,
            ParsedType type)
        {
            this.Name = name;
            this.Type = type;
        }

        public string Name { get; }

        public ParsedType Type { get; }
    }
}
