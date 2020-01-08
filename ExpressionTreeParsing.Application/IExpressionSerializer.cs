using System;
using System.Linq.Expressions;
using ExpressionTreeParsing.Domain;

namespace ExpressionTreeParsing.Application
{
    public interface IExpressionSerializer<TModel>
    {
        Expression Deserialize(ParsedExpression parsedExpression);

        Expression<Func<TModel, TResult>> Deserialize<TResult>(ParsedExpression parsedExpression);

        ParsedExpression Serialize<TResult>(Expression<Func<TModel, TResult>> expression);
    }
}
