namespace ExpressionTreeParsing.Domain
{
    public class ParsedType
    {
        public ParsedType(string assemblyQualifiedName)
        {
            this.AssemblyQualifiedName = assemblyQualifiedName;
        }

        public string AssemblyQualifiedName { get; }

        public override string ToString()
        {
            return this.AssemblyQualifiedName;
        }
    }
}
