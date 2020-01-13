using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionTreeParsing.Domain
{
    public class ParsedTryExpression : ParsedExpression
    {
        public ParsedTryExpression(
            ParsedType type,
            ParsedExpression body,
            ParsedExpression @finally,
            ParsedExpression fault,
            IEnumerable<ParsedCatchBlock> handlers)
            : base()
        {
            this.Type = type;
            this.Body = body;
            this.Finally = @finally;
            this.Fault = fault;
            this.Handlers = handlers.ToArray();
        }

        public ParsedExpression Body { get; }

        public ParsedExpression Fault { get; }

        public ParsedExpression Finally { get; }

        public override ExpressionType NodeType => ExpressionType.Try;

        public IEnumerable<ParsedCatchBlock> Handlers { get; }

        public ParsedType Type { get; }
    }
}
