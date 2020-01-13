using System;
using System.Linq.Expressions;

namespace ExpressionTreeParsing.Application
{
    /// <summary>
    /// A class to rewrite the parameters in an Expression.
    /// </summary>
    /// <remarks>https://stackoverflow.com/a/6736589</remarks>
    public class ParameterReplacementVisitor : ExpressionVisitor
    {
        private readonly ParameterExpression _parameter;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="parameter">The parameter to use for replacement.</param>
        public ParameterReplacementVisitor(ParameterExpression parameter)
        {
            this._parameter = parameter ?? throw new ArgumentNullException(nameof(parameter));
        }

        /// <summary>
        /// Visits the <see cref="ParameterExpression"/>.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>the modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
        protected override Expression VisitParameter(ParameterExpression node)
        {
            return base.VisitParameter(_parameter);
        }
    }
}
