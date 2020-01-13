using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using ExpressionTreeParsing.Domain;

namespace ExpressionTreeParsing.Application
{
    public partial class ExpressionSerializer<TModel>
    {
        private static ParsedExpression Serialize(Expression expression)
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

        private static ParsedBinaryExpression Serialize(BinaryExpression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return new ParsedBinaryExpression(
                expression.Conversion == null ? null : Serialize(expression.Conversion),
                expression.Left == null ? null : Serialize(expression.Left),
                expression.IsLiftedToNull,
                expression.Method == null ? null : Serialize(expression.Method),
                expression.NodeType,
                expression.Right == null ? null : Serialize(expression.Right));
        }

        private static ParsedBlockExpression Serialize(BlockExpression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return new ParsedBlockExpression(
                expression.Expressions.Take(expression.Expressions.Count - 1).Select(Serialize).ToArray(),
                expression.Result == null ? null : Serialize(expression.Result),
                expression.Type == null ? null : Serialize(expression.Type),
                expression.Variables.Select(Serialize).ToArray());
        }

        private static ParsedConditionalExpression Serialize(ConditionalExpression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return new ParsedConditionalExpression(
                expression.IfFalse == null ? null : Serialize(expression.IfFalse),
                expression.IfTrue == null ? null : Serialize(expression.IfTrue),
                expression.Test == null ? null : Serialize(expression.Test),
                expression.Type == null ? null : Serialize(expression.Type));
        }

        private static ParsedConstantExpression Serialize(ConstantExpression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return new ParsedConstantExpression(
                expression.Type == null ? null : Serialize(expression.Type),
                expression.Value);
        }

        private static ParsedDebugInfoExpression Serialize(DebugInfoExpression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return new ParsedDebugInfoExpression(
                expression.EndColumn,
                expression.EndLine,
                expression.Document == null ? null : Serialize(expression.Document),
                expression.IsClear,
                expression.StartColumn,
                expression.StartLine);
        }

        private static ParsedDefaultExpression Serialize(DefaultExpression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return new ParsedDefaultExpression(expression.Type == null ? null : Serialize(expression.Type));
        }

        private static ParsedDynamicExpression Serialize(DynamicExpression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return new ParsedDynamicExpression(
                expression.Arguments.Select(Serialize).ToArray(),
                expression.DelegateType == null ? null : Serialize(expression.DelegateType));
        }

        private static ParsedGotoExpression Serialize(GotoExpression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return new ParsedGotoExpression(
                expression.Kind,
                expression.Target == null ? null : Serialize(expression.Target),
                expression.Type == null ? null : Serialize(expression.Type),
                expression.Value == null ? null : Serialize(expression.Value));
        }

        private static ParsedFieldInfo Serialize(FieldInfo fieldInfo)
        {
            if (fieldInfo == null) throw new ArgumentNullException(nameof(fieldInfo));

            return new ParsedFieldInfo(
                fieldInfo.DeclaringType == null ? null : Serialize(fieldInfo.DeclaringType),
                fieldInfo.FieldType == null ? null : Serialize(fieldInfo.FieldType),
                fieldInfo.Name,
                fieldInfo.IsPublic,
                fieldInfo.IsStatic,
                fieldInfo.ReflectedType == null ? null : Serialize(fieldInfo.ReflectedType));
        }

        private static ParsedLabelTarget Serialize(LabelTarget labelTarget)
        {
            if (labelTarget == null) throw new ArgumentNullException(nameof(labelTarget));

            return new ParsedLabelTarget(
                labelTarget.Name,
                labelTarget.Type == null ? null : Serialize(labelTarget.Type));
        }

        private static ParsedLambdaExpression Serialize(LambdaExpression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return new ParsedLambdaExpression(
                expression.Body == null ? null : Serialize(expression.Body),
                expression.Name,
                expression.Parameters.Select(Serialize).ToArray(),
                expression.ReturnType == null ? null : Serialize(expression.ReturnType),
                expression.TailCall);
        }

        private static ParsedMemberExpression Serialize(MemberExpression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return new ParsedMemberExpression(
                expression.Expression == null ? null : Serialize(expression.Expression),
                expression.Member == null ? null : Serialize(expression.Member),
                expression.Type == null ? null : Serialize(expression.Type));
        }

        private static ParsedMemberInfo Serialize(MemberInfo memberInfo)
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

                case MemberTypes.Constructor:
                case MemberTypes.Custom:
                case MemberTypes.Event:
                case MemberTypes.NestedType:
                case MemberTypes.TypeInfo:
                default:
                    throw new Exception($"Unknown member type {memberInfo.MemberType} for member {memberInfo}");
            }
        }

        private static ParsedMethodInfo Serialize(MethodInfo methodInfo)
        {
            if (methodInfo == null) throw new ArgumentNullException(nameof(methodInfo));

            return new ParsedMethodInfo(
                methodInfo.DeclaringType == null ? null : Serialize(methodInfo.DeclaringType),
                methodInfo.GetGenericArguments().Select(Serialize).ToArray(),
                methodInfo.IsPublic,
                methodInfo.IsStatic,
                methodInfo.Name,
                methodInfo.GetParameters().Select(Serialize).ToArray(),
                methodInfo.ReflectedType == null ? null : Serialize(methodInfo.ReflectedType),
                methodInfo.ReturnType == null ? null : Serialize(methodInfo.ReturnType));
        }

        private static ParsedMethodCallExpression Serialize(MethodCallExpression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return new ParsedMethodCallExpression(
                expression.Arguments.Select(Serialize).ToArray(),
                expression.Object == null ? null : Serialize(expression.Object),
                expression.Method == null ? null : Serialize(expression.Method),
                expression.Type == null ? null : Serialize(expression.Type));
        }

        private static ParsedParameterExpression Serialize(ParameterExpression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return new ParsedParameterExpression(
                expression.IsByRef,
                expression.Name,
                expression.Type == null ? null : Serialize(expression.Type));
        }

        private static ParsedParameterInfo Serialize(ParameterInfo parameterInfo)
        {
            if (parameterInfo == null) throw new ArgumentNullException(nameof(parameterInfo));

            return new ParsedParameterInfo(
                parameterInfo.Name,
                parameterInfo.ParameterType == null ? null : Serialize(parameterInfo.ParameterType),
                parameterInfo.Position);
        }

        private static ParsedPropertyInfo Serialize(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null) throw new ArgumentNullException(nameof(propertyInfo));

            return new ParsedPropertyInfo(
                propertyInfo.DeclaringType == null ? null : Serialize(propertyInfo.DeclaringType),
                propertyInfo.Name,
                propertyInfo.PropertyType == null ? null : Serialize(propertyInfo.PropertyType),
                propertyInfo.ReflectedType == null ? null : Serialize(propertyInfo.ReflectedType));
        }

        private static ParsedSymbolDocumentInfo Serialize(SymbolDocumentInfo symbolDocumentInfo)
        {
            if (symbolDocumentInfo == null) throw new ArgumentNullException(nameof(symbolDocumentInfo));

            return new ParsedSymbolDocumentInfo(symbolDocumentInfo.FileName);
        }

        private static ParsedType Serialize(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            StringBuilder assemblyName = new StringBuilder($"{type.FullName}, {type.Assembly.GetName().Name}");

            foreach (Type genericTypeArgument in type.GenericTypeArguments)
            {
                assemblyName.Replace(genericTypeArgument.AssemblyQualifiedName, $"{genericTypeArgument.FullName}, {genericTypeArgument.Assembly.GetName().Name}");
            }

            return new ParsedType(assemblyName.ToString());
        }

        private static ParsedUnaryExpression Serialize(UnaryExpression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return new ParsedUnaryExpression(
                expression.Operand == null ? null : Serialize(expression.Operand),
                expression.Method == null ? null : Serialize(expression.Method),
                expression.NodeType,
                expression.Type == null ? null : Serialize(expression.Type));
        }
    }
}
