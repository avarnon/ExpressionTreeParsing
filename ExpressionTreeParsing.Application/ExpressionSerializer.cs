using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using ExpressionTreeParsing.Domain;

namespace ExpressionTreeParsing.Application
{
    public class ExpressionSerializer<TModel> : IExpressionSerializer<TModel>
    {
        public Expression Deserialize(ParsedExpression parsedExpression)
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

        public Expression<Func<TModel, TResult>> Deserialize<TResult>(ParsedExpression parsedExpression)
        {
            return (Expression<Func<TModel, TResult>>)this.Deserialize(parsedExpression);
        }

        public ParsedExpression Serialize<TResult>(Expression<Func<TModel, TResult>> expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return Serialize(expression as Expression);
        }

        private BinaryExpression Deserialize(ParsedBinaryExpression parsedExpression)
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

        private BlockExpression Deserialize(ParsedBlockExpression parsedExpression)
        {
            if (parsedExpression == null) throw new ArgumentNullException(nameof(parsedExpression));

            return Expression.Block(
                parsedExpression.Type == null ? null : Deserialize(parsedExpression.Type),
                parsedExpression.Variables.Select(Deserialize).ToArray(),
                parsedExpression.Expressions.Select(Deserialize).Concat(new[] { Deserialize(parsedExpression.Result), }).ToArray());
        }

        private ConditionalExpression Deserialize(ParsedConditionalExpression parsedExpression)
        {
            if (parsedExpression == null) throw new ArgumentNullException(nameof(parsedExpression));

            return Expression.Condition(
                parsedExpression.Test == null ? null : Deserialize(parsedExpression.Test),
                parsedExpression.IfTrue == null ? null : Deserialize(parsedExpression.IfTrue),
                parsedExpression.IfFalse == null ? null : Deserialize(parsedExpression.IfFalse),
                parsedExpression.Type == null ? null : Deserialize(parsedExpression.Type));
        }

        private ConstantExpression Deserialize(ParsedConstantExpression parsedExpression)
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

        private DebugInfoExpression Deserialize(ParsedDebugInfoExpression parsedExpression)
        {
            if (parsedExpression == null) throw new ArgumentNullException(nameof(parsedExpression));

            return Expression.DebugInfo(
                parsedExpression.Document == null ? null : Deserialize(parsedExpression.Document),
                parsedExpression.StartLine,
                parsedExpression.StartColumn,
                parsedExpression.EndLine,
                parsedExpression.EndColumn);
        }

        private DefaultExpression Deserialize(ParsedDefaultExpression parsedExpression)
        {
            if (parsedExpression == null) throw new ArgumentNullException(nameof(parsedExpression));

            return Expression.Default(parsedExpression.Type == null ? null : Deserialize(parsedExpression.Type));
        }

        private DynamicExpression Deserialize(ParsedDynamicExpression parsedExpression)
        {
            if (parsedExpression == null) throw new ArgumentNullException(nameof(parsedExpression));

            return Expression.Dynamic(
                null,
                parsedExpression.DelegateType == null ? null : Deserialize(parsedExpression.DelegateType),
                parsedExpression.Arguments.Select(Deserialize).ToArray());
        }

        private FieldInfo Deserialize(ParsedFieldInfo parsedFieldInfo, Type reflectedType)
        {
            if (parsedFieldInfo == null) throw new ArgumentNullException(nameof(parsedFieldInfo));
            if (reflectedType == null) throw new ArgumentNullException(nameof(reflectedType));

            BindingFlags bindingFlags = (parsedFieldInfo.IsPublic ? BindingFlags.Public : BindingFlags.NonPublic) |
                (parsedFieldInfo.IsStatic ? BindingFlags.Static : BindingFlags.Instance);

            return reflectedType.GetField(parsedFieldInfo.Name, bindingFlags) ?? throw new Exception($"Field {parsedFieldInfo.Name} not found on type {reflectedType}");
        }

        private GotoExpression Deserialize(ParsedGotoExpression parsedExpression)
        {
            if (parsedExpression == null) throw new ArgumentNullException(nameof(parsedExpression));

            return Expression.Goto(
                parsedExpression.Target == null ? null : Deserialize(parsedExpression.Target),
                parsedExpression.Value == null ? null : Deserialize(parsedExpression.Value),
                parsedExpression.Type == null ? null : Deserialize(parsedExpression.Type));
        }

        private LabelTarget Deserialize(ParsedLabelTarget parsedLabelTarget)
        {
            if (parsedLabelTarget == null) throw new ArgumentNullException(nameof(parsedLabelTarget));

            return Expression.Label(
                parsedLabelTarget.Type == null ? null : Deserialize(parsedLabelTarget.Type),
                parsedLabelTarget.Name);
        }

        private LambdaExpression Deserialize(ParsedLambdaExpression parsedExpression)
        {
            if (parsedExpression == null) throw new ArgumentNullException(nameof(parsedExpression));

            return Expression.Lambda(
                parsedExpression.Body == null ? null : Deserialize(parsedExpression.Body),
                parsedExpression.Name,
                parsedExpression.TailCall,
                parsedExpression.Parameters.Select(Deserialize).ToArray());
        }

        private MemberExpression Deserialize(ParsedMemberExpression parsedExpression)
        {
            if (parsedExpression == null) throw new ArgumentNullException(nameof(parsedExpression));

            return Expression.MakeMemberAccess(
                parsedExpression.Expression == null ? null : Deserialize(parsedExpression.Expression),
                parsedExpression.Member == null ? null : Deserialize(parsedExpression.Member));
        }

        private MemberInfo Deserialize(ParsedMemberInfo parsedMemberInfo)
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

        private MethodCallExpression Deserialize(ParsedMethodCallExpression parsedExpression)
        {
            if (parsedExpression == null) throw new ArgumentNullException(nameof(parsedExpression));

            return Expression.Call(
                parsedExpression.Instance == null ? null : Deserialize(parsedExpression.Instance),
                parsedExpression.Method == null ? null : (MethodInfo)Deserialize(parsedExpression.Method as ParsedMemberInfo) ?? throw new Exception($"Could not find method {parsedExpression.Method.Name} on {parsedExpression.Method.ReflectedType}"),
                parsedExpression.Arguments.Select(Deserialize).ToArray());
        }

        private MethodInfo Deserialize(ParsedMethodInfo parsedMethodInfo, Type reflectedType)
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

        private ParameterExpression Deserialize(ParsedParameterExpression parsedExpression)
        {
            if (parsedExpression == null) throw new ArgumentNullException(nameof(parsedExpression));

            return Expression.Parameter(
                parsedExpression.Type == null ? null : Deserialize(parsedExpression.Type),
                parsedExpression.Name);
        }

        private PropertyInfo Deserialize(ParsedPropertyInfo parsedPropertyInfo, Type reflectedType)
        {
            if (parsedPropertyInfo == null) throw new ArgumentNullException(nameof(parsedPropertyInfo));
            if (reflectedType == null) throw new ArgumentNullException(nameof(reflectedType));

            BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance;

            return reflectedType.GetProperty(parsedPropertyInfo.Name, bindingFlags) ?? throw new Exception($"Property {parsedPropertyInfo.Name} not found on type {reflectedType}");
        }

        private SymbolDocumentInfo Deserialize(ParsedSymbolDocumentInfo parsedSymbolDocumentInfo)
        {
            if (parsedSymbolDocumentInfo == null) throw new ArgumentNullException(nameof(parsedSymbolDocumentInfo));

            return Expression.SymbolDocument(parsedSymbolDocumentInfo.FileName);
        }

        private Type Deserialize(ParsedType parsedType)
        {
            if (parsedType == null) throw new ArgumentNullException(nameof(parsedType));

            return Type.GetType(parsedType.AssemblyQualifiedName);
        }

        private UnaryExpression Deserialize(ParsedUnaryExpression parsedExpression)
        {
            if (parsedExpression == null) throw new ArgumentNullException(nameof(parsedExpression));

            return Expression.MakeUnary(
                parsedExpression.NodeType,
                parsedExpression.Operand == null ? null : Deserialize(parsedExpression.Operand),
                parsedExpression.Type == null ? null : Deserialize(parsedExpression.Type),
                parsedExpression.Method == null ? null : (MethodInfo)Deserialize(parsedExpression.Method as ParsedMemberInfo));
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
                expression.IsLiftedToNull,
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
                expression.Test == null ? null : Serialize(expression.Test),
                expression.Type == null ? null : Serialize(expression.Type));
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

            return new ParsedDebugInfoExpression(
                expression.EndColumn,
                expression.EndLine,
                expression.Document == null ? null : Serialize(expression.Document),
                expression.IsClear,
                expression.StartColumn,
                expression.StartLine);
        }

        private ParsedDefaultExpression Serialize(DefaultExpression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return new ParsedDefaultExpression(expression.Type == null ? null : Serialize(expression.Type));
        }

        private ParsedDynamicExpression Serialize(DynamicExpression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return new ParsedDynamicExpression(
                expression.Arguments.Select(Serialize).ToArray(),
                expression.DelegateType == null ? null : Serialize(expression.DelegateType));
        }

        private ParsedGotoExpression Serialize(GotoExpression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return new ParsedGotoExpression(
                expression.Kind,
                expression.Target == null ? null : Serialize(expression.Target),
                expression.Type == null ? null : Serialize(expression.Type),
                expression.Value == null ? null : Serialize(expression.Value));
        }

        private ParsedFieldInfo Serialize(FieldInfo fieldInfo)
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

        private ParsedLabelTarget Serialize(LabelTarget labelTarget)
        {
            if (labelTarget == null) throw new ArgumentNullException(nameof(labelTarget));

            return new ParsedLabelTarget(
                labelTarget.Name,
                labelTarget.Type == null ? null : Serialize(labelTarget.Type));
        }

        private ParsedLambdaExpression Serialize(LambdaExpression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return new ParsedLambdaExpression(
                expression.Body == null ? null : Serialize(expression.Body),
                expression.Name,
                expression.Parameters.Select(Serialize).ToArray(),
                expression.ReturnType == null ? null : Serialize(expression.ReturnType),
                expression.TailCall);
        }

        private ParsedMemberExpression Serialize(MemberExpression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return new ParsedMemberExpression(
                expression.Expression == null ? null : Serialize(expression.Expression),
                expression.Member == null ? null : Serialize(expression.Member),
                expression.Type == null ? null : Serialize(expression.Type));
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

                case MemberTypes.Constructor:
                case MemberTypes.Custom:
                case MemberTypes.Event:
                case MemberTypes.NestedType:
                case MemberTypes.TypeInfo:
                default:
                    throw new Exception($"Unknown member type {memberInfo.MemberType} for member {memberInfo}");
            }
        }

        private ParsedMethodInfo Serialize(MethodInfo methodInfo)
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

        private ParsedMethodCallExpression Serialize(MethodCallExpression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return new ParsedMethodCallExpression(
                expression.Arguments.Select(Serialize).ToArray(),
                expression.Object == null ? null : Serialize(expression.Object),
                expression.Method == null ? null : Serialize(expression.Method),
                expression.Type == null ? null : Serialize(expression.Type));
        }

        private ParsedParameterExpression Serialize(ParameterExpression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return new ParsedParameterExpression(
                expression.IsByRef,
                expression.Name,
                expression.Type == null ? null : Serialize(expression.Type));
        }

        private ParsedParameterInfo Serialize(ParameterInfo parameterInfo)
        {
            if (parameterInfo == null) throw new ArgumentNullException(nameof(parameterInfo));

            return new ParsedParameterInfo(
                parameterInfo.Name,
                parameterInfo.ParameterType == null ? null : Serialize(parameterInfo.ParameterType),
                parameterInfo.Position);
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

        private ParsedSymbolDocumentInfo Serialize(SymbolDocumentInfo symbolDocumentInfo)
        {
            if (symbolDocumentInfo == null) throw new ArgumentNullException(nameof(symbolDocumentInfo));

            return new ParsedSymbolDocumentInfo(symbolDocumentInfo.FileName);
        }

        private ParsedType Serialize(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            StringBuilder assemblyName = new StringBuilder($"{type.FullName}, {type.Assembly.GetName().Name}");

            foreach (Type genericTypeArgument in type.GenericTypeArguments)
            {
                assemblyName.Replace(genericTypeArgument.AssemblyQualifiedName, $"{genericTypeArgument.FullName}, {genericTypeArgument.Assembly.GetName().Name}");
            }

            return new ParsedType(assemblyName.ToString());
        }

        private ParsedUnaryExpression Serialize(UnaryExpression expression)
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
