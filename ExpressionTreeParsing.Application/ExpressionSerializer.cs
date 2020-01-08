using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ExpressionTreeParsing.Domain;

namespace ExpressionTreeParsing.Application
{
    public class ExpressionSerializer<TModel> : IExpressionSerializer<TModel>
    {
        public Expression Deserialize(ParsedExpression parsedExpression)
        {
            throw new NotImplementedException();
        }

        public Expression<Func<TModel, TResult>> Deserialize<TResult>(ParsedExpression parsedExpression)
        {
            return (Expression<Func<TModel, TResult>>)this.Deserialize(parsedExpression);
        }

        public ParsedExpression Serialize<TResult>(Expression<Func<TModel, TResult>> expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return Serialize(expression as Expression);
        }

        private ParsedExpression Serialize(Expression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            switch (expression.NodeType)
            {
                case ExpressionType.Block:
                    return Serialize(expression as BlockExpression);

                case ExpressionType.Call:
                    return Serialize(expression as MethodCallExpression);

                case ExpressionType.Constant:
                    return Serialize(expression as ConstantExpression);

                case ExpressionType.Conditional:
                    return Serialize(expression as ConditionalExpression);

                case ExpressionType.DebugInfo:
                    return Serialize(expression as DebugInfoExpression);

                case ExpressionType.Default:
                    return Serialize(expression as DefaultExpression);

                case ExpressionType.Dynamic:
                    return Serialize(expression as DynamicExpression);

                case ExpressionType.Goto:
                    return Serialize(expression as GotoExpression);

                case ExpressionType.Lambda:
                    return Serialize(expression as LambdaExpression);

                case ExpressionType.MemberAccess:
                    return Serialize(expression as MemberExpression);

                case ExpressionType.Parameter:
                    return Serialize(expression as ParameterExpression);

                case ExpressionType.Add:
                case ExpressionType.AddAssign:
                case ExpressionType.AddChecked:
                case ExpressionType.AddAssignChecked:
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                case ExpressionType.AndAssign:
                case ExpressionType.ArrayIndex:
                case ExpressionType.Assign:
                case ExpressionType.Coalesce:
                case ExpressionType.Divide:
                case ExpressionType.DivideAssign:
                case ExpressionType.Equal:
                case ExpressionType.ExclusiveOr:
                case ExpressionType.ExclusiveOrAssign:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.LeftShift:
                case ExpressionType.LeftShiftAssign:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.Modulo:
                case ExpressionType.ModuloAssign:
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyAssign:
                case ExpressionType.MultiplyChecked:
                case ExpressionType.MultiplyAssignChecked:
                case ExpressionType.NotEqual:
                case ExpressionType.Or:
                case ExpressionType.OrAssign:
                case ExpressionType.OrElse:
                case ExpressionType.Power:
                case ExpressionType.PowerAssign:
                case ExpressionType.RightShift:
                case ExpressionType.RightShiftAssign:
                case ExpressionType.Subtract:
                case ExpressionType.SubtractAssign:
                case ExpressionType.SubtractChecked:
                case ExpressionType.SubtractAssignChecked:
                    return Serialize(expression as BinaryExpression);

                case ExpressionType.ArrayLength:
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                case ExpressionType.Decrement:
                case ExpressionType.Increment:
                case ExpressionType.IsFalse:
                case ExpressionType.IsTrue:
                case ExpressionType.OnesComplement:
                case ExpressionType.Negate:
                case ExpressionType.NegateChecked:
                case ExpressionType.Not:
                case ExpressionType.PreDecrementAssign:
                case ExpressionType.PreIncrementAssign:
                case ExpressionType.PostIncrementAssign:
                case ExpressionType.PostDecrementAssign:
                case ExpressionType.Quote:
                case ExpressionType.Throw:
                case ExpressionType.TypeAs:
                case ExpressionType.UnaryPlus:
                case ExpressionType.Unbox:
                    return Serialize(expression as UnaryExpression);

                case ExpressionType.Extension:
                case ExpressionType.Index:
                case ExpressionType.Invoke:
                case ExpressionType.Label:
                case ExpressionType.ListInit:
                case ExpressionType.Loop:
                case ExpressionType.MemberInit:
                case ExpressionType.New:
                case ExpressionType.NewArrayBounds:
                case ExpressionType.NewArrayInit:
                case ExpressionType.RuntimeVariables:
                case ExpressionType.Switch:
                case ExpressionType.Try:
                case ExpressionType.TypeEqual:
                case ExpressionType.TypeIs:
                default:
                    throw new Exception($"Unknown expression type {expression.NodeType} for expression {expression}");
            }
        }

        private ParsedBinaryExpression Serialize(BinaryExpression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return new ParsedBinaryExpression(
                expression.Conversion == null ? null : Serialize(expression.Conversion),
                expression.Left == null ? null : Serialize(expression.Left),
                expression.Method == null ? null : Serialize(expression.Method),
                expression.NodeType,
                expression.Right == null ? null : Serialize(expression.Right));
        }

        private ParsedBlockExpression Serialize(BlockExpression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return new ParsedBlockExpression(
                expression.Expressions.Take(expression.Expressions.Count - 1).Select(Serialize).ToArray(),
                expression.Result == null ? null : Serialize(expression.Result),
                expression.Type == null ? null : Serialize(expression.Type),
                expression.Variables.Select(Serialize).ToArray());
        }

        private ParsedConditionalExpression Serialize(ConditionalExpression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return new ParsedConditionalExpression(
                expression.IfFalse == null ? null : Serialize(expression.IfFalse),
                expression.IfTrue == null ? null : Serialize(expression.IfTrue),
                expression.Test == null ? null : Serialize(expression.Test));
        }

        private ParsedConstantExpression Serialize(ConstantExpression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return new ParsedConstantExpression(
                expression.Type == null ? null : Serialize(expression.Type),
                expression.Value);
        }

        private ParsedDebugInfoExpression Serialize(DebugInfoExpression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            List<JProperty> properties = new List<JProperty>()
        {
            new JProperty("endColumn", expression.EndColumn),
            new JProperty("endLine", expression.EndLine),
            new JProperty("startColumn", expression.StartColumn),
            new JProperty("startLine", expression.StartLine),
            new JProperty("nodeType", expression.NodeType.ToString()),
        };

            return new JObject(properties.Where(p => p.Value != null).ToArray());
        }

        private ParsedDefaultExpression Serialize(DefaultExpression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            List<JProperty> properties = new List<JProperty>()
        {
            new JProperty("nodeType", expression.NodeType.ToString()),
            new JProperty("type", expression.Type == null ? null : Serialize(expression.Type)),
        };

            return new JObject(properties.Where(p => p.Value != null).ToArray());
        }

        private ParsedDynamicExpression Serialize(DynamicExpression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            List<JProperty> properties = new List<JProperty>()
        {
            new JProperty("arguments", expression.Arguments.Count > 0 ? new JArray(expression.Arguments.Select(Serialize).ToArray()) : null),
            new JProperty("delegateType", expression.DelegateType == null ? null : Serialize(expression.DelegateType)),
            new JProperty("nodeType", expression.NodeType.ToString()),
        };

            return new JObject(properties.Where(p => p.Value != null).ToArray());
        }

        private ParsedGotoExpression Serialize(GotoExpression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            List<JProperty> properties = new List<JProperty>()
        {
            new JProperty("kind", expression.Kind),
            new JProperty("nodeType", expression.NodeType.ToString()),
            new JProperty("target", expression.Target == null ? null : Serialize(expression.Target)),
            new JProperty("type", expression.Type == null ? null : Serialize(expression.Type)),
            new JProperty("value", expression.Value == null ? null : Serialize(expression.Value)),
        };

            return new JObject(properties.Where(p => p.Value != null).ToArray());
        }

        private ParsedFieldInfo Serialize(FieldInfo fieldInfo)
        {
            if (fieldInfo == null) throw new ArgumentNullException(nameof(fieldInfo));

            return new ParsedFieldInfo(
                fieldInfo.DeclaringType == null ? null : Serialize(fieldInfo.DeclaringType),
                fieldInfo.FieldType == null ? null : Serialize(fieldInfo.FieldType),
                fieldInfo.Name,
                fieldInfo.ReflectedType == null ? null : Serialize(fieldInfo.ReflectedType));
        }

        private ParsedLabelTarget Serialize(LabelTarget labelTarget)
        {
            if (labelTarget == null) throw new ArgumentNullException(nameof(labelTarget));

            List<JProperty> properties = new List<JProperty>()
        {
            new JProperty("name", labelTarget.Name),
            new JProperty("type", labelTarget.Type == null ? null : Serialize(labelTarget.Type)),
        };

            return new JObject(properties.Where(p => p.Value != null).ToArray());
        }

        private ParsedLambdaExpression Serialize(LambdaExpression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            List<JProperty> properties = new List<JProperty>()
        {
            new JProperty("body", expression.Body == null ? null : Serialize(expression.Body)),
            new JProperty("nodeType", expression.NodeType.ToString()),
            new JProperty("parameters", expression.Parameters.Count > 0 ? new JArray(expression.Parameters.Select(Serialize).ToArray()) : null),
            new JProperty("returnType", expression.ReturnType == null ? null : Serialize(expression.ReturnType)),
        };

            return new JObject(properties.Where(p => p.Value != null).ToArray());
        }

        private ParsedMemberExpression Serialize(MemberExpression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            List<JProperty> properties = new List<JProperty>()
        {
            new JProperty("expression", expression.Expression == null ? null : Serialize(expression.Expression)),
            new JProperty("member", expression.Member == null ? null : Serialize(expression.Member)),
            new JProperty("nodeType", expression.NodeType.ToString()),
            new JProperty("type", expression.Type == null ? null : Serialize(expression.Type)),
        };

            return new JObject(properties.Where(p => p.Value != null).ToArray());
        }

        private ParsedMemberInfo Serialize(MemberInfo memberInfo)
        {
            if (memberInfo == null) throw new ArgumentNullException(nameof(memberInfo));

            switch (memberInfo.MemberType)
            {
                case MemberTypes.Field:
                    return Serialize(memberInfo as FieldInfo);

                case MemberTypes.Property:
                    return Serialize(memberInfo as PropertyInfo);

                case MemberTypes.Method:
                    return Serialize(memberInfo as MethodInfo);

                default:
                    throw new Exception($"Unknown member type {memberInfo.MemberType} for member {memberInfo}");
            }
        }

        private ParsedMethodInfo Serialize(MethodInfo methodInfo)
        {
            if (methodInfo == null) throw new ArgumentNullException(nameof(methodInfo));

            return new ParsedMethodInfo(
                methodInfo.DeclaringType == null ? null : Serialize(methodInfo.DeclaringType),
                methodInfo.IsPublic,
                methodInfo.IsStatic,
                methodInfo.Name,
                methodInfo.GetParameters().Select(Serialize).ToArray(),
                methodInfo.ReflectedType == null ? null : Serialize(methodInfo.ReflectedType),
                methodInfo.ReturnType == null ? null : Serialize(methodInfo.ReturnType));
        }

        private ParsedMethodCallExpression Serialize(MethodCallExpression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            List<JProperty> properties = new List<JProperty>()
        {
            new JProperty("arguments", expression.Arguments.Count > 0 ? new JArray(expression.Arguments.Select(Serialize).ToArray()) : null),
            new JProperty("method", expression.Method == null ? null : Serialize(expression.Method)),
            new JProperty("nodeType", expression.NodeType.ToString()),
            new JProperty("object", expression.Object == null ? null : Serialize(expression.Object)),
            new JProperty("type", Serialize(expression.Type)),
        };

            return new JObject(properties.Where(p => p.Value != null).ToArray());
        }

        private ParsedParameterExpression Serialize(ParameterExpression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            List<JProperty> properties = new List<JProperty>()
        {
            new JProperty("name", expression.Name),
            new JProperty("nodeType", expression.NodeType.ToString()),
            new JProperty("type", expression.Type == null ? null : Serialize(expression.Type)),
        };

            return new JObject(properties.Where(p => p.Value != null).ToArray());
        }

        private ParsedParameterInfo Serialize(ParameterInfo parameterInfo)
        {
            if (parameterInfo == null) throw new ArgumentNullException(nameof(parameterInfo));

            List<JProperty> properties = new List<JProperty>()
        {
            new JProperty("name", parameterInfo.Name),
            new JProperty("parameterType", parameterInfo.ParameterType == null ? null : Serialize(parameterInfo.ParameterType)),
        };

            return new JObject(properties.Where(p => p.Value != null).ToArray());
        }

        private ParsedPropertyInfo Serialize(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null) throw new ArgumentNullException(nameof(propertyInfo));

            return new ParsedPropertyInfo(
                propertyInfo.DeclaringType == null ? null : Serialize(propertyInfo.DeclaringType),
                propertyInfo.Name,
                propertyInfo.PropertyType == null ? null : Serialize(propertyInfo.PropertyType),
                propertyInfo.ReflectedType == null ? null : Serialize(propertyInfo.ReflectedType));
        }

        private ParsedType Serialize(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return new ParsedType(type.AssemblyQualifiedName);
        }

        private ParsedUnaryExpression Serialize(UnaryExpression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            List<JProperty> properties = new List<JProperty>()
        {
            new JProperty("operand", expression.Operand == null ? null : Serialize(expression.Operand)),
            new JProperty("method", expression.Method == null ? null : Serialize(expression.Method)),
            new JProperty("nodeType", expression.NodeType.ToString()),
        };

            return new JObject(properties.Where(p => p.Value != null).ToArray());
        }
    }
}
