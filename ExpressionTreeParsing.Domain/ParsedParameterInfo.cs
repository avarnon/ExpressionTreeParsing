namespace ExpressionTreeParsing.Domain
{
    public class ParsedParameterInfo
    {
        public ParsedParameterInfo(
            string name,
            ParsedType parameterType,
            int position)
        {
            this.Name = name;
            this.ParameterType = parameterType;
            this.Position = position;
        }

        public string Name { get; }

        public ParsedType ParameterType { get; }

        public int Position { get; }

        public override string ToString()
        {
            return $"{this.ParameterType} {this.Name}";
        }
    }
}
