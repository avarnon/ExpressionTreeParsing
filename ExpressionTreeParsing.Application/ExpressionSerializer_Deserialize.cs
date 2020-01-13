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

                case ExpressionType.Index:
                    return Deserialize(parsedExpression as ParsedIndexExpression);

                case ExpressionType.Invoke:
                    return Deserialize(parsedExpression as ParsedInvocationExpression);

                case ExpressionType.Label:
                    return Deserialize(parsedExpression as ParsedLabelExpression);

                case ExpressionType.Lambda:
                    return Deserialize(parsedExpression as ParsedLambdaExpression);

                case ExpressionType.ListInit:
                    return Deserialize(parsedExpression as ParsedListInitExpression);

                case ExpressionType.Loop:
                    return Deserialize(parsedExpression as ParsedLoopExpression);

                case ExpressionType.MemberAccess:
                    return Deserialize(parsedExpression as ParsedMemberExpression);

                case ExpressionType.MemberInit:
                    return Deserialize(parsedExpression as ParsedMemberInitExpression);

                case ExpressionType.New:
                    return Deserialize(parsedExpression as ParsedNewExpression);

                case ExpressionType.Parameter:
                    return Deserialize(parsedExpression as ParsedParameterExpression);

                case ExpressionType.RuntimeVariables:
                    return Deserialize(parsedExpression as ParsedRuntimeVariablesExpression);

                case ExpressionType.Switch:
                    return Deserialize(parsedExpression as ParsedSwitchExpression);

                case ExpressionType.Try:
                    return Deserialize(parsedExpression as ParsedTryExpression);

                case ExpressionType.TypeEqual:
                    return Deserialize(parsedExpression as ParsedTypeBinaryExpression);

                case ExpressionType.NewArrayBounds:
                case ExpressionType.NewArrayInit:
                    return Deserialize(parsedExpression as ParsedNewArrayExpression);

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
                case ExpressionType.TypeIs:
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
                default:
                    throw new NotImplementedException($"Unknown expression type {parsedExpression.NodeType}");
            }
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

        private CatchBlock Deserialize(ParsedCatchBlock parsedCatchBlock)
        {
            if (parsedCatchBlock == null) throw new ArgumentNullException(nameof(parsedCatchBlock));

            return Expression.MakeCatchBlock(
                parsedCatchBlock.Type == null ? null : Deserialize(parsedCatchBlock.Type),
                parsedCatchBlock.Variable == null ? null : Deserialize(parsedCatchBlock.Variable),
                parsedCatchBlock.Body == null ? null : Deserialize(parsedCatchBlock.Body),
                parsedCatchBlock.Filter == null ? null : Deserialize(parsedCatchBlock.Filter));
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

        private ConstructorInfo Deserialize(ParsedConstructorInfo parsedConstructorInfo, Type reflectedType)
        {
            if (parsedConstructorInfo == null) throw new ArgumentNullException(nameof(parsedConstructorInfo));
            if (reflectedType == null) throw new ArgumentNullException(nameof(reflectedType));

            BindingFlags bindingFlags = (parsedConstructorInfo.IsPublic ? BindingFlags.Public : BindingFlags.NonPublic);

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

            return reflectedType.GetConstructors(bindingFlags)
                .Where(_ => _.Name == parsedConstructorInfo.Name &&
                            parametersMatch(_.GetParameters(), parsedConstructorInfo.Parameters) &&
                            genericArgumentsMatch(_.GetGenericArguments(), parsedConstructorInfo.GenericArguments))
                .SingleOrDefault();
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

        private ElementInit Deserialize(ParsedElementInit parsedElementInit)
        {
            if (parsedElementInit == null) throw new ArgumentNullException(nameof(parsedElementInit));

            return Expression.ElementInit(
                parsedElementInit.AddMethod == null ? null : (MethodInfo)Deserialize(parsedElementInit.AddMethod as ParsedMethodInfo),
                parsedElementInit.Arguments.Select(Deserialize).ToArray());
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

        private IndexExpression Deserialize(ParsedIndexExpression parsedExpression)
        {
            if (parsedExpression == null) throw new ArgumentNullException(nameof(parsedExpression));

            return Expression.ArrayAccess(
                parsedExpression.Array == null ? null : Deserialize(parsedExpression.Array),
                parsedExpression.Indexes.Select(Deserialize).ToArray());
        }

        private InvocationExpression Deserialize(ParsedInvocationExpression parsedExpression)
        {
            if (parsedExpression == null) throw new ArgumentNullException(nameof(parsedExpression));

            return Expression.Invoke(
                parsedExpression.Expression == null ? null : Deserialize(parsedExpression.Expression),
                parsedExpression.Arguments.Select(Deserialize).ToArray());
        }

        private LabelExpression Deserialize(ParsedLabelExpression parsedExpression)
        {
            if (parsedExpression == null) throw new ArgumentNullException(nameof(parsedExpression));

            return Expression.Label(
                parsedExpression.Target == null ? null : Deserialize(parsedExpression.Target),
                parsedExpression.DefaultValue == null ? null : Deserialize(parsedExpression.DefaultValue));
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

        private ListInitExpression Deserialize(ParsedListInitExpression parsedExpression)
        {
            if (parsedExpression == null) throw new ArgumentNullException(nameof(parsedExpression));

            return Expression.ListInit(
                parsedExpression.NewExpression == null ? null : Deserialize(parsedExpression.NewExpression),
                parsedExpression.Initializers.Select(Deserialize).ToArray());
        }

        private LoopExpression Deserialize(ParsedLoopExpression parsedExpression)
        {
            if (parsedExpression == null) throw new ArgumentNullException(nameof(parsedExpression));

            return Expression.Loop(
                parsedExpression.Body == null ? null : Deserialize(parsedExpression.Body),
                parsedExpression.Break == null ? null : Deserialize(parsedExpression.Break),
                parsedExpression.Continue == null ? null : Deserialize(parsedExpression.Continue));
        }

        private MemberAssignment Deserialize(ParsedMemberAssignment parsedMemberBinding)
        {
            if (parsedMemberBinding == null) throw new ArgumentNullException(nameof(parsedMemberBinding));

            return Expression.Bind(
                parsedMemberBinding.Member == null ? null : Deserialize(parsedMemberBinding.Member),
                parsedMemberBinding.Expression == null ? null : Deserialize(parsedMemberBinding.Expression));
        }

        private MemberBinding Deserialize(ParsedMemberBinding parsedMemberBinding)
        {
            if (parsedMemberBinding == null) throw new ArgumentNullException(nameof(parsedMemberBinding));

            switch (parsedMemberBinding.BindingType)
            {
                case MemberBindingType.Assignment:
                    return Deserialize(parsedMemberBinding as ParsedMemberAssignment);

                case MemberBindingType.ListBinding:
                    return Deserialize(parsedMemberBinding as ParsedMemberListBinding);

                case MemberBindingType.MemberBinding:
                    return Deserialize(parsedMemberBinding as ParsedMemberMemberBinding);

                default:
                    throw new NotImplementedException($"Unknown binding type {parsedMemberBinding.BindingType}");
            }
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
                case MemberTypes.Constructor:
                    return Deserialize(parsedMemberInfo as ParsedConstructorInfo, reflectedType);

                case MemberTypes.Field:
                    return Deserialize(parsedMemberInfo as ParsedFieldInfo, reflectedType);

                case MemberTypes.Property:
                    return Deserialize(parsedMemberInfo as ParsedPropertyInfo, reflectedType);

                case MemberTypes.Method:
                    return Deserialize(parsedMemberInfo as ParsedMethodInfo, reflectedType);

                case MemberTypes.Custom:
                case MemberTypes.Event:
                case MemberTypes.NestedType:
                case MemberTypes.TypeInfo:
                default:
                    throw new NotImplementedException($"Unknown member type {parsedMemberInfo.MemberType}");
            }
        }

        private MemberListBinding Deserialize(ParsedMemberListBinding parsedMemberBinding)
        {
            if (parsedMemberBinding == null) throw new ArgumentNullException(nameof(parsedMemberBinding));

            return Expression.ListBind(
                parsedMemberBinding.Member == null ? null : Deserialize(parsedMemberBinding.Member),
                parsedMemberBinding.Initializers.Select(Deserialize).ToArray());
        }

        private MemberInitExpression Deserialize(ParsedMemberInitExpression parsedExpression)
        {
            if (parsedExpression == null) throw new ArgumentNullException(nameof(parsedExpression));

            return Expression.MemberInit(
                parsedExpression.NewExpression == null ? null : Deserialize(parsedExpression.NewExpression),
                parsedExpression.Bindings.Select(Deserialize).ToArray());
        }

        private MemberMemberBinding Deserialize(ParsedMemberMemberBinding parsedMemberBinding)
        {
            if (parsedMemberBinding == null) throw new ArgumentNullException(nameof(parsedMemberBinding));

            return Expression.MemberBind(
                parsedMemberBinding.Member == null ? null : Deserialize(parsedMemberBinding.Member),
                parsedMemberBinding.Bindings.Select(Deserialize).ToArray());
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

        private NewArrayExpression Deserialize(ParsedNewArrayExpression parsedExpression)
        {
            if (parsedExpression == null) throw new ArgumentNullException(nameof(parsedExpression));

            switch (parsedExpression.NodeType)
            {
                case ExpressionType.NewArrayBounds:
                    return Expression.NewArrayBounds(
                        parsedExpression.Type == null ? null : Deserialize(parsedExpression.Type),
                        parsedExpression.Expressions.Select(Deserialize).ToArray());

                case ExpressionType.NewArrayInit:
                    return Expression.NewArrayInit(
                        parsedExpression.Type == null ? null : Deserialize(parsedExpression.Type),
                        parsedExpression.Expressions.Select(Deserialize).ToArray());

                default:
                    throw new NotImplementedException($"Unknown expression type {parsedExpression.NodeType}");
            }
        }

        private NewExpression Deserialize(ParsedNewExpression parsedExpression)
        {
            if (parsedExpression == null) throw new ArgumentNullException(nameof(parsedExpression));

            return Expression.New(parsedExpression.Constructor == null ? null : (ConstructorInfo)Deserialize(parsedExpression.Constructor as ParsedMemberInfo));
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

        private RuntimeVariablesExpression Deserialize(ParsedRuntimeVariablesExpression parsedExpression)
        {
            if (parsedExpression == null) throw new ArgumentNullException(nameof(parsedExpression));

            return Expression.RuntimeVariables(parsedExpression.Variables.Select(Deserialize).ToArray());
        }

        private SwitchCase Deserialize(ParsedSwitchCase parsedSwitchCase)
        {
            if (parsedSwitchCase == null) throw new ArgumentNullException(nameof(parsedSwitchCase));

            return Expression.SwitchCase(
                parsedSwitchCase.Body == null ? null : Deserialize(parsedSwitchCase.Body),
                parsedSwitchCase.TestValues.Select(Deserialize).ToArray());
        }

        private SwitchExpression Deserialize(ParsedSwitchExpression parsedExpression)
        {
            if (parsedExpression == null) throw new ArgumentNullException(nameof(parsedExpression));

            return Expression.Switch(
                parsedExpression.Type == null ? null : Deserialize(parsedExpression.Type),
                parsedExpression.SwitchValue == null ? null : Deserialize(parsedExpression.SwitchValue),
                parsedExpression.DefaultBody == null ? null : Deserialize(parsedExpression.DefaultBody),
                parsedExpression.Comparison == null ? null : (MethodInfo)Deserialize(parsedExpression.Comparison as ParsedMemberInfo),
                parsedExpression.Cases.Select(Deserialize).ToArray());
        }

        private SymbolDocumentInfo Deserialize(ParsedSymbolDocumentInfo parsedSymbolDocumentInfo)
        {
            if (parsedSymbolDocumentInfo == null) throw new ArgumentNullException(nameof(parsedSymbolDocumentInfo));

            return Expression.SymbolDocument(parsedSymbolDocumentInfo.FileName);
        }

        private TryExpression Deserialize(ParsedTryExpression parsedExpression)
        {
            if (parsedExpression == null) throw new ArgumentNullException(nameof(parsedExpression));

            return Expression.MakeTry(
                parsedExpression.Type == null ? null : Deserialize(parsedExpression.Type),
                parsedExpression.Body == null ? null : Deserialize(parsedExpression.Body),
                parsedExpression.Finally == null ? null : Deserialize(parsedExpression.Fault),
                parsedExpression.Fault == null ? null : Deserialize(parsedExpression.Finally),
                parsedExpression.Handlers.Select(Deserialize).ToArray());
        }

        private Type Deserialize(ParsedType parsedType)
        {
            if (parsedType == null) throw new ArgumentNullException(nameof(parsedType));

            return Type.GetType(parsedType.AssemblyQualifiedName);
        }

        private TypeBinaryExpression Deserialize(ParsedTypeBinaryExpression parsedExpression)
        {
            if (parsedExpression == null) throw new ArgumentNullException(nameof(parsedExpression));

            return Expression.TypeEqual(
                parsedExpression.Expression == null ? null : Deserialize(parsedExpression.Expression),
                parsedExpression.Type == null ? null : Deserialize(parsedExpression.Type));
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
    }
}
