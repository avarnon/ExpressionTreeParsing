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

                case ExpressionType.Index:
                    return Serialize(expression as IndexExpression);

                case ExpressionType.Invoke:
                    return Serialize(expression as InvocationExpression);

                case ExpressionType.Label:
                    return Serialize(expression as LabelExpression);

                case ExpressionType.Lambda:
                    return Serialize(expression as LambdaExpression);

                case ExpressionType.ListInit:
                    return Serialize(expression as ListInitExpression);

                case ExpressionType.Loop:
                    return Serialize(expression as LoopExpression);

                case ExpressionType.MemberAccess:
                    return Serialize(expression as MemberExpression);

                case ExpressionType.MemberInit:
                    return Serialize(expression as MemberInitExpression);

                case ExpressionType.New:
                    return Serialize(expression as NewExpression);

                case ExpressionType.Parameter:
                    return Serialize(expression as ParameterExpression);

                case ExpressionType.RuntimeVariables:
                    return Serialize(expression as RuntimeVariablesExpression);

                case ExpressionType.Switch:
                    return Serialize(expression as SwitchExpression);

                case ExpressionType.Try:
                    return Serialize(expression as TryExpression);

                case ExpressionType.TypeEqual:
                    return Serialize(expression as TypeBinaryExpression);

                case ExpressionType.NewArrayBounds:
                case ExpressionType.NewArrayInit:
                    return Serialize(expression as NewArrayExpression);

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
                    Expression currentExpression = expression;
                    do
                    {
                        currentExpression = currentExpression.ReduceAndCheck();
                    }
                    while (currentExpression.NodeType == ExpressionType.Extension);

                    return Serialize(currentExpression);

                default:
                    throw new NotImplementedException($"Unknown expression type {expression.NodeType} for expression {expression}");
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

        private ParsedCatchBlock Serialize(CatchBlock catchBlock)
        {
            if (catchBlock == null) throw new ArgumentNullException(nameof(catchBlock));


            return new ParsedCatchBlock(
                catchBlock.Variable?.Type == null ? null : Serialize(catchBlock.Variable.Type),
                catchBlock.Variable == null ? null : Serialize(catchBlock.Variable),
                catchBlock.Filter == null ? null : Serialize(catchBlock.Filter),
                catchBlock.Body == null ? null : Serialize(catchBlock.Body));
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

        private ParsedConstructorInfo Serialize(ConstructorInfo constructorInfo)
        {
            if (constructorInfo == null) throw new ArgumentNullException(nameof(constructorInfo));

            return new ParsedConstructorInfo(
                constructorInfo.DeclaringType == null ? null : Serialize(constructorInfo.DeclaringType),
                constructorInfo.GetGenericArguments().Select(Serialize).ToArray(),
                constructorInfo.IsPublic,
                constructorInfo.IsStatic,
                constructorInfo.Name,
                constructorInfo.GetParameters().Select(Serialize).ToArray(),
                constructorInfo.ReflectedType == null ? null : Serialize(constructorInfo.ReflectedType),
                constructorInfo.DeclaringType == null ? null : Serialize(constructorInfo.DeclaringType));
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

        private ParsedElementInit Serialize(ElementInit elementInit)
        {
            if (elementInit == null) throw new ArgumentNullException(nameof(elementInit));

            return new ParsedElementInit(
                elementInit.AddMethod == null ? null : Serialize(elementInit.AddMethod),
                elementInit.Arguments.Select(Serialize).ToArray());
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

        private ParsedGotoExpression Serialize(GotoExpression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return new ParsedGotoExpression(
                expression.Kind,
                expression.Target == null ? null : Serialize(expression.Target),
                expression.Type == null ? null : Serialize(expression.Type),
                expression.Value == null ? null : Serialize(expression.Value));
        }

        private ParsedIndexExpression Serialize(IndexExpression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return new ParsedIndexExpression(
                expression.Object == null ? null : Serialize(expression.Object),
                expression.Arguments.Select(Serialize).ToArray());
        }

        private ParsedInvocationExpression Serialize(InvocationExpression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return new ParsedInvocationExpression(
                expression.Expression == null ? null : Serialize(expression.Expression),
                expression.Arguments.Select(Serialize).ToArray());
        }

        private ParsedLabelExpression Serialize(LabelExpression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return new ParsedLabelExpression(
                expression.Target == null ? null : Serialize(expression.Target),
                expression.DefaultValue == null ? null : Serialize(expression.DefaultValue));
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

        private ParsedListInitExpression Serialize(ListInitExpression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return new ParsedListInitExpression(
                expression.NewExpression == null ? null : Serialize(expression.NewExpression),
                expression.Initializers.Select(Serialize).ToArray());
        }

        private ParsedLoopExpression Serialize(LoopExpression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return new ParsedLoopExpression(
                expression.Body == null ? null : Serialize(expression.Body),
                expression.BreakLabel == null ? null : Serialize(expression.BreakLabel),
                expression.ContinueLabel == null ? null : Serialize(expression.ContinueLabel));
        }

        private ParsedMemberAssignment Serialize(MemberAssignment memberBinding)
        {
            if (memberBinding == null) throw new ArgumentNullException(nameof(memberBinding));

            return new ParsedMemberAssignment(
                memberBinding.Member == null ? null : Serialize(memberBinding.Member),
                memberBinding.Expression == null ? null : Serialize(memberBinding.Expression));
        }

        private ParsedMemberExpression Serialize(MemberExpression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return new ParsedMemberExpression(
                expression.Expression == null ? null : Serialize(expression.Expression),
                expression.Member == null ? null : Serialize(expression.Member),
                expression.Type == null ? null : Serialize(expression.Type));
        }

        private ParsedMemberBinding Serialize(MemberBinding memberBinding)
        {
            if (memberBinding == null) throw new ArgumentNullException(nameof(memberBinding));

            switch (memberBinding.BindingType)
            {
                case MemberBindingType.Assignment:
                    return Serialize(memberBinding as MemberAssignment);

                case MemberBindingType.ListBinding:
                    return Serialize(memberBinding as MemberListBinding);

                case MemberBindingType.MemberBinding:
                    return Serialize(memberBinding as MemberMemberBinding);

                default:
                    throw new NotImplementedException($"Unknown binding type {memberBinding.BindingType} for member binding {memberBinding}");
            }
        }

        private ParsedMemberInfo Serialize(MemberInfo memberInfo)
        {
            if (memberInfo == null) throw new ArgumentNullException(nameof(memberInfo));

            switch (memberInfo.MemberType)
            {
                case MemberTypes.Constructor:
                    return Serialize(memberInfo as ConstructorInfo);

                case MemberTypes.Field:
                    return Serialize(memberInfo as FieldInfo);

                case MemberTypes.Property:
                    return Serialize(memberInfo as PropertyInfo);

                case MemberTypes.Method:
                    return Serialize(memberInfo as MethodInfo);

                case MemberTypes.Custom:
                case MemberTypes.Event:
                case MemberTypes.NestedType:
                case MemberTypes.TypeInfo:
                default:
                    throw new NotImplementedException($"Unknown member type {memberInfo.MemberType} for member {memberInfo}");
            }
        }

        private ParsedMemberInitExpression Serialize(MemberInitExpression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return new ParsedMemberInitExpression(
                expression.NewExpression == null ? null : Serialize(expression.NewExpression),
                expression.Bindings.Select(Serialize).ToArray());
        }

        private ParsedMemberListBinding Serialize(MemberListBinding memberBinding)
        {
            if (memberBinding == null) throw new ArgumentNullException(nameof(memberBinding));

            return new ParsedMemberListBinding(
                memberBinding.Member == null ? null : Serialize(memberBinding.Member),
                memberBinding.Initializers.Select(Serialize).ToArray());
        }

        private ParsedMemberMemberBinding Serialize(MemberMemberBinding memberBinding)
        {
            if (memberBinding == null) throw new ArgumentNullException(nameof(memberBinding));

            return new ParsedMemberMemberBinding(
                memberBinding.Member == null ? null : Serialize(memberBinding.Member),
                memberBinding.Bindings.Select(Serialize).ToArray());
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

        private ParsedNewArrayExpression Serialize(NewArrayExpression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return new ParsedNewArrayExpression(
                expression.Type == null ? null : Serialize(expression.Type),
                expression.Expressions.Select(Serialize).ToArray(),
                expression.NodeType);
        }

        private ParsedNewExpression Serialize(NewExpression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return new ParsedNewExpression(expression.Constructor == null ? null : Serialize(expression.Constructor));
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

        private ParsedRuntimeVariablesExpression Serialize(RuntimeVariablesExpression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return new ParsedRuntimeVariablesExpression(expression.Variables.Select(Serialize).ToArray());
        }

        private ParsedSwitchCase Serialize(SwitchCase switchCase)
        {
            if (switchCase == null) throw new ArgumentNullException(nameof(switchCase));

            return new ParsedSwitchCase(
                switchCase.Body == null ? null : Serialize(switchCase.Body),
                switchCase.TestValues.Select(Serialize).ToArray());
        }

        private ParsedSwitchExpression Serialize(SwitchExpression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return new ParsedSwitchExpression(
                expression.Type == null ? null : Serialize(expression.Type),
                expression.SwitchValue == null ? null : Serialize(expression.SwitchValue),
                expression.DefaultBody == null ? null : Serialize(expression.DefaultBody),
                expression.Comparison == null ? null : Serialize(expression.Comparison),
                expression.Cases.Select(Serialize).ToArray());
        }

        private ParsedSymbolDocumentInfo Serialize(SymbolDocumentInfo symbolDocumentInfo)
        {
            if (symbolDocumentInfo == null) throw new ArgumentNullException(nameof(symbolDocumentInfo));

            return new ParsedSymbolDocumentInfo(symbolDocumentInfo.FileName);
        }

        private ParsedTryExpression Serialize(TryExpression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return new ParsedTryExpression(
                expression.Type == null ? null : Serialize(expression.Type),
                expression.Body == null ? null : Serialize(expression.Body),
                expression.Finally == null ? null : Serialize(expression.Fault),
                expression.Fault == null ? null : Serialize(expression.Finally),
                expression.Handlers.Select(Serialize).ToArray());
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

        private ParsedTypeBinaryExpression Serialize(TypeBinaryExpression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return new ParsedTypeBinaryExpression(
                expression.Expression == null ? null : Serialize(expression.Expression),
                expression.Type == null ? null : Serialize(expression.Type));
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
