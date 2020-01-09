namespace ExpressionTreeParsing.Domain
{
    public class ParsedSymbolDocumentInfo
    {
        public ParsedSymbolDocumentInfo(string fileName)
        {
            this.FileName = fileName;
        }

        public string FileName { get; }

        public override string ToString() => this.FileName;
    }
}
