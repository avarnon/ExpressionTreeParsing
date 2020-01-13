using System.Collections.Generic;
using System.Linq.Expressions;

namespace ExpressionTreeParsing.Domain
{
    public class ParsedSwitchExpression : ParsedExpression
    {
        public ParsedSwitchExpression(
            ParsedType type,
            ParsedExpression switchValue,
            ParsedExpression defaultBody,
            ParsedMethodInfo comparison,
            IEnumerable<ParsedSwitchCase> cases)
            : base()
        {
            this.Type = type;
            this.SwitchValue = switchValue;
            this.DefaultBody = defaultBody;
            this.Comparison = comparison;
            this.Cases = cases;
        }

        public IEnumerable<ParsedSwitchCase> Cases { get; }

        public ParsedMethodInfo Comparison { get; }

        public ParsedExpression DefaultBody { get; }

        public override ExpressionType NodeType => ExpressionType.Switch;

        public ParsedExpression SwitchValue { get; }

        public ParsedType Type { get; }
    }
}
