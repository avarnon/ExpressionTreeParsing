using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ExpressionTreeParsing.Domain;

namespace ExpressionTreeParsing.Application
{
    public partial class ExpressionSerializer<TModel> : IExpressionSerializer<TModel>
    {
        public static Expression Deserialize(ParsedExpression parsedExpression)
        {
            if (parsedExpression == null) throw new ArgumentNullException(nameof(parsedExpression));

            switch (parsedExpression.NodeType)
            {
                case ExpressionType.Block:
                    return Deserialize(parsedExpression as ParsedBlockExpression);

                case ExpressionType.Call:
                    return Deserialize(parsedExpression as ParsedMethodCallExpression);

                case ExpressionType.Constant:
                    return Deserialize(parsedExpression as ParsedConstantExpression);

                case ExpressionType.Conditional:
                    return Deserialize(parsedExpression as ParsedConditionalExpression);

                case ExpressionType.DebugInfo:
                    return Deserialize(parsedExpression as ParsedDebugInfoExpression);

                case ExpressionType.Default:
                    return Deserialize(parsedExpression as ParsedDefaultExpression);

                case ExpressionType.Dynamic:
                    return Deserialize(parsedExpression as ParsedDynamicExpression);

                case ExpressionType.Goto:
                    return Deserialize(parsedExpression as ParsedGotoExpression);

                case ExpressionType.Lambda:
                    return Deserialize(parsedExpression as ParsedLambdaExpression);

                case ExpressionType.MemberAccess:
                    return Deserialize(parsedExpression as ParsedMemberExpression);

                case ExpressionType.Parameter:
                    return Deserialize(parsedExpression as ParsedParameterExpression);

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
                    return Deserialize(parsedExpression as ParsedBinaryExpression);

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
                    return Deserialize(parsedExpression as ParsedUnaryExpression);

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
                    throw new Exception($"Unknown expression type {parsedExpression.NodeType}");
            }
        }

        private static BinaryExpression Deserialize(ParsedBinaryExpression parsedExpression)
        {
            if (parsedExpression == null) throw new ArgumentNullException(nameof(parsedExpression));

            return Expression.MakeBinary(
                parsedExpression.NodeType,
                parsedExpression.Left == null ? null : Deserialize(parsedExpression.Left),
                parsedExpression.Right == null ? null : Deserialize(parsedExpression.Right),
                parsedExpression.LiftToNull,
                parsedExpression.Method == null ? null : (MethodInfo)Deserialize(parsedExpression.Method as ParsedMemberInfo),
                parsedExpression.Conversion == null ? null : Deserialize(parsedExpression.Conversion));
        }

        private static BlockExpression Deserialize(ParsedBlockExpression parsedExpression)
        {
            if (parsedExpression == null) throw new ArgumentNullException(nameof(parsedExpression));

            IEnumerable<Expression> expressions = parsedExpression.Expressions.Select(Deserialize).Concat(new[] { Deserialize(parsedExpression.Result), }).ToArray();
            IEnumerable<ParameterExpression> variables = parsedExpression.Variables.Select(Deserialize).ToArray();
            List<Expression> finalExpressions = expressions.ToList();

            foreach (ParameterExpression variable in variables)
            {
                for (int i = 0; i < finalExpressions.Count; i++)
                {
                    ParameterReplacementVisitor visitorExpression = new ParameterReplacementVisitor(variable);
                    finalExpressions[i] = visitorExpression.Visit(finalExpressions[i]);
                }
            }

            return Expression.Block(
                parsedExpression.Type == null ? null : Deserialize(parsedExpression.Type),
                variables,
                finalExpressions);
        }

        private static ConditionalExpression Deserialize(ParsedConditionalExpression parsedExpression)
        {
            if (parsedExpression == null) throw new ArgumentNullException(nameof(parsedExpression));

            return Expression.Condition(
                parsedExpression.Test == null ? null : Deserialize(parsedExpression.Test),
                parsedExpression.IfTrue == null ? null : Deserialize(parsedExpression.IfTrue),
                parsedExpression.IfFalse == null ? null : Deserialize(parsedExpression.IfFalse),
                parsedExpression.Type == null ? null : Deserialize(parsedExpression.Type));
        }

        private static ConstantExpression Deserialize(ParsedConstantExpression parsedExpression)
        {
            if (parsedExpression == null) throw new ArgumentNullException(nameof(parsedExpression));

            object value = parsedExpression.Value;
            Type type = parsedExpression.Type == null ? null : Deserialize(parsedExpression.Type);

            if (value != null &&
                type != null &&
                value.GetType() != type)
            {
                if (type.IsEnum && value is string)
                {
                    value = Enum.Parse(type, value as string);
                }
                else
                {
                    value = Convert.ChangeType(value, type, null);
                }
            }

            return Expression.Constant(
                value,
                type);
        }

        private static DebugInfoExpression Deserialize(ParsedDebugInfoExpression parsedExpression)
        {
            if (parsedExpression == null) throw new ArgumentNullException(nameof(parsedExpression));

            return Expression.DebugInfo(
                parsedExpression.Document == null ? null : Deserialize(parsedExpression.Document),
                parsedExpression.StartLine,
                parsedExpression.StartColumn,
                parsedExpression.EndLine,
                parsedExpression.EndColumn);
        }

        private static DefaultExpression Deserialize(ParsedDefaultExpression parsedExpression)
        {
            if (parsedExpression == null) throw new ArgumentNullException(nameof(parsedExpression));

            return Expression.Default(parsedExpression.Type == null ? null : Deserialize(parsedExpression.Type));
        }

        private static DynamicExpression Deserialize(ParsedDynamicExpression parsedExpression)
        {
            if (parsedExpression == null) throw new ArgumentNullException(nameof(parsedExpression));

            return Expression.Dynamic(
                null,
                parsedExpression.DelegateType == null ? null : Deserialize(parsedExpression.DelegateType),
                parsedExpression.Arguments.Select(Deserialize).ToArray());
        }

        private static FieldInfo Deserialize(ParsedFieldInfo parsedFieldInfo, Type reflectedType)
        {
            if (parsedFieldInfo == null) throw new ArgumentNullException(nameof(parsedFieldInfo));
            if (reflectedType == null) throw new ArgumentNullException(nameof(reflectedType));

            BindingFlags bindingFlags = (parsedFieldInfo.IsPublic ? BindingFlags.Public : BindingFlags.NonPublic) |
                (parsedFieldInfo.IsStatic ? BindingFlags.Static : BindingFlags.Instance);

            return reflectedType.GetField(parsedFieldInfo.Name, bindingFlags) ?? throw new Exception($"Field {parsedFieldInfo.Name} not found on type {reflectedType}");
        }

        private static GotoExpression Deserialize(ParsedGotoExpression parsedExpression)
        {
            if (parsedExpression == null) throw new ArgumentNullException(nameof(parsedExpression));

            return Expression.Goto(
                parsedExpression.Target == null ? null : Deserialize(parsedExpression.Target),
                parsedExpression.Value == null ? null : Deserialize(parsedExpression.Value),
                parsedExpression.Type == null ? null : Deserialize(parsedExpression.Type));
        }

        private static LabelTarget Deserialize(ParsedLabelTarget parsedLabelTarget)
        {
            if (parsedLabelTarget == null) throw new ArgumentNullException(nameof(parsedLabelTarget));

            return Expression.Label(
                parsedLabelTarget.Type == null ? null : Deserialize(parsedLabelTarget.Type),
                parsedLabelTarget.Name);
        }

        private static LambdaExpression Deserialize(ParsedLambdaExpression parsedExpression)
        {
            if (parsedExpression == null) throw new ArgumentNullException(nameof(parsedExpression));

            Expression body = parsedExpression.Body == null ? null : Deserialize(parsedExpression.Body);
            IEnumerable<ParameterExpression> parameters = parsedExpression.Parameters.Select(Deserialize).ToArray();
            Expression finalBody = body;

            foreach (ParameterExpression parameter in parameters)
            {
                ParameterReplacementVisitor visitorBody = new ParameterReplacementVisitor(parameter);
                finalBody = visitorBody.Visit(finalBody);
            }

            return Expression.Lambda(
                finalBody,
                parsedExpression.Name,
                parsedExpression.TailCall,
                parameters);
        }

        private static MemberExpression Deserialize(ParsedMemberExpression parsedExpression)
        {
            if (parsedExpression == null) throw new ArgumentNullException(nameof(parsedExpression));

            return Expression.MakeMemberAccess(
                parsedExpression.Expression == null ? null : Deserialize(parsedExpression.Expression),
                parsedExpression.Member == null ? null : Deserialize(parsedExpression.Member));
        }

        private static MemberInfo Deserialize(ParsedMemberInfo parsedMemberInfo)
        {
            if (parsedMemberInfo == null) throw new ArgumentNullException(nameof(parsedMemberInfo));

            Type reflectedType = parsedMemberInfo.ReflectedType == null
                ? throw new ArgumentNullException($"{nameof(parsedMemberInfo)}.{nameof(ParsedMemberInfo.ReflectedType)}")
                : Deserialize(parsedMemberInfo.ReflectedType);

            switch (parsedMemberInfo.MemberType)
            {
                case MemberTypes.Field:
                    return Deserialize(parsedMemberInfo as ParsedFieldInfo, reflectedType);

                case MemberTypes.Property:
                    return Deserialize(parsedMemberInfo as ParsedPropertyInfo, reflectedType);

                case MemberTypes.Method:
                    return Deserialize(parsedMemberInfo as ParsedMethodInfo, reflectedType);

                case MemberTypes.Constructor:
                case MemberTypes.Custom:
                case MemberTypes.Event:
                case MemberTypes.NestedType:
                case MemberTypes.TypeInfo:
                default:
                    throw new NotImplementedException();
            }
        }

        private static MethodCallExpression Deserialize(ParsedMethodCallExpression parsedExpression)
        {
            if (parsedExpression == null) throw new ArgumentNullException(nameof(parsedExpression));

            return Expression.Call(
                parsedExpression.Instance == null ? null : Deserialize(parsedExpression.Instance),
                parsedExpression.Method == null ? null : (MethodInfo)Deserialize(parsedExpression.Method as ParsedMemberInfo) ?? throw new Exception($"Could not find method {parsedExpression.Method.Name} on {parsedExpression.Method.ReflectedType}"),
                parsedExpression.Arguments.Select(Deserialize).ToArray());
        }

        private static MethodInfo Deserialize(ParsedMethodInfo parsedMethodInfo, Type reflectedType)
        {
            if (parsedMethodInfo == null) throw new ArgumentNullException(nameof(parsedMethodInfo));
            if (reflectedType == null) throw new ArgumentNullException(nameof(reflectedType));

            BindingFlags bindingFlags = (parsedMethodInfo.IsPublic ? BindingFlags.Public : BindingFlags.NonPublic) |
                (parsedMethodInfo.IsStatic ? BindingFlags.Static : BindingFlags.Instance);

            Func<IEnumerable<ParameterInfo>, IEnumerable<ParsedParameterInfo>, bool> parametersMatch = (paramterInfos, parsedParameterInfos) =>
            {
                if (paramterInfos == null) throw new ArgumentNullException(nameof(paramterInfos));
                if (parsedParameterInfos == null) throw new ArgumentNullException(nameof(parsedParameterInfos));

                if (paramterInfos.Count() != parsedParameterInfos.Count()) return false;

                for (int i = 0; i < paramterInfos.Count(); i++)
                {
                    ParameterInfo parameterInfo = paramterInfos.ElementAt(i);
                    ParsedParameterInfo parsedParameterInfo = parsedParameterInfos.ElementAt(i);

                    if (parameterInfo.Name != parsedParameterInfo.Name) return false;

                    Type source = parsedParameterInfo.ParameterType == null ? null : Deserialize(parsedParameterInfo.ParameterType);
                    if (parameterInfo.ParameterType != source)
                    {
                        Type parameterInfo2 = parameterInfo.ParameterType.IsGenericType ? parameterInfo.ParameterType.GetGenericTypeDefinition() : parameterInfo.ParameterType;
                        Type source2 = source.IsGenericType ? source.GetGenericTypeDefinition() : source;

                        if (parameterInfo2 != source2) return false;
                    }
                }

                return true;
            };
            Func<IEnumerable<Type>, IEnumerable<ParsedType>, bool> genericArgumentsMatch = (types, parsedTypes) =>
            {
                if (types == null) throw new ArgumentNullException(nameof(types));
                if (parsedTypes == null) throw new ArgumentNullException(nameof(parsedTypes));

                if (types.Count() != parsedTypes.Count()) return false;

                return true;
            };

            MethodInfo methodInfo = reflectedType.GetMethods(bindingFlags)
                .Where(_ => _.Name == parsedMethodInfo.Name &&
                            _.ReturnType == (parsedMethodInfo.ReturnType == null ? null : Deserialize(parsedMethodInfo.ReturnType)) &&
                            parametersMatch(_.GetParameters(), parsedMethodInfo.Parameters) &&
                            genericArgumentsMatch(_.GetGenericArguments(), parsedMethodInfo.GenericArguments))
                .SingleOrDefault();

            if (methodInfo != null && methodInfo.GetGenericArguments().Length > 0)
            {
                methodInfo = methodInfo.MakeGenericMethod(parsedMethodInfo.GenericArguments.Select(Deserialize).ToArray());
            }

            return methodInfo;
        }

        private static ParameterExpression Deserialize(ParsedParameterExpression parsedExpression)
        {
            if (parsedExpression == null) throw new ArgumentNullException(nameof(parsedExpression));

            return Expression.Parameter(
                parsedExpression.Type == null ? null : Deserialize(parsedExpression.Type),
                parsedExpression.Name);
        }

        private static PropertyInfo Deserialize(ParsedPropertyInfo parsedPropertyInfo, Type reflectedType)
        {
            if (parsedPropertyInfo == null) throw new ArgumentNullException(nameof(parsedPropertyInfo));
            if (reflectedType == null) throw new ArgumentNullException(nameof(reflectedType));

            BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance;

            return reflectedType.GetProperty(parsedPropertyInfo.Name, bindingFlags) ?? throw new Exception($"Property {parsedPropertyInfo.Name} not found on type {reflectedType}");
        }

        private static SymbolDocumentInfo Deserialize(ParsedSymbolDocumentInfo parsedSymbolDocumentInfo)
        {
            if (parsedSymbolDocumentInfo == null) throw new ArgumentNullException(nameof(parsedSymbolDocumentInfo));

            return Expression.SymbolDocument(parsedSymbolDocumentInfo.FileName);
        }

        private static Type Deserialize(ParsedType parsedType)
        {
            if (parsedType == null) throw new ArgumentNullException(nameof(parsedType));

            return Type.GetType(parsedType.AssemblyQualifiedName);
        }

        private static UnaryExpression Deserialize(ParsedUnaryExpression parsedExpression)
        {
            if (parsedExpression == null) throw new ArgumentNullException(nameof(parsedExpression));

            return Expression.MakeUnary(
                parsedExpression.NodeType,
                parsedExpression.Operand == null ? null : Deserialize(parsedExpression.Operand),
                parsedExpression.Type == null ? null : Deserialize(parsedExpression.Type),
                parsedExpression.Method == null ? null : (MethodInfo)Deserialize(parsedExpression.Method as ParsedMemberInfo));
        }
    }
}
