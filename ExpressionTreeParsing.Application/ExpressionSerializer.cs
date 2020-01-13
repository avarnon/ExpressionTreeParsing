using System;
using System.Linq.Expressions;
using ExpressionTreeParsing.Domain;

namespace ExpressionTreeParsing.Application
{
    public partial class ExpressionSerializer<TModel> : IExpressionSerializer<TModel>
    {
        Expression IExpressionSerializer<TModel>.Deserialize(ParsedExpression parsedExpression)
        {
            if (parsedExpression == null) throw new ArgumentNullException(nameof(parsedExpression));

            return Deserialize(parsedExpression);
        }

        public Expression<Func<TModel, TResult>> Deserialize<TResult>(ParsedExpression parsedExpression)
        {
            return (Expression<Func<TModel, TResult>>)Deserialize(parsedExpression);
        }

        public ParsedExpression Serialize<TResult>(Expression<Func<TModel, TResult>> expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return Serialize(expression as Expression);
        }
    }
}
