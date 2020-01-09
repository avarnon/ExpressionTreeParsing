using System;
using ExpressionTreeParsing.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ExpressionType = System.Linq.Expressions.ExpressionType;

namespace ExpressionTreeParsing.Console
{
    public class ParsedExpressionConverter : JsonConverter
    {
        public override bool CanWrite => false;

        public override bool CanConvert(Type objectType) => objectType == typeof(ParsedExpression);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);
            string nodeTypeString = jObject["nodeType"].Value<string>();

            if (string.IsNullOrWhiteSpace(nodeTypeString) ||
                !Enum.TryParse(nodeTypeString, out ExpressionType nodeType))
            {
                throw new NotImplementedException();
            }

            switch (nodeType)
            {
                case ExpressionType.Block:
                    return jObject.ToObject<ParsedBlockExpression>(serializer);

                case ExpressionType.Call:
                    return jObject.ToObject<ParsedMethodCallExpression>(serializer);

                case ExpressionType.Constant:
                    return jObject.ToObject<ParsedConstantExpression>(serializer);

                case ExpressionType.Conditional:
                    return jObject.ToObject<ParsedConditionalExpression>(serializer);

                case ExpressionType.DebugInfo:
                    return jObject.ToObject<ParsedDebugInfoExpression>(serializer);

                case ExpressionType.Default:
                    return jObject.ToObject<ParsedDefaultExpression>(serializer);

                case ExpressionType.Dynamic:
                    return jObject.ToObject<ParsedDynamicExpression>(serializer);

                case ExpressionType.Goto:
                    return jObject.ToObject<ParsedGotoExpression>(serializer);

                case ExpressionType.Lambda:
                    return jObject.ToObject<ParsedLambdaExpression>(serializer);

                case ExpressionType.MemberAccess:
                    return jObject.ToObject<ParsedMemberExpression>(serializer);

                case ExpressionType.Parameter:
                    return jObject.ToObject<ParsedParameterExpression>(serializer);

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
                    return jObject.ToObject<ParsedBinaryExpression>(serializer);

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
                    return jObject.ToObject<ParsedUnaryExpression>(serializer);

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
                    throw new NotImplementedException();
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) => throw new NotImplementedException();
    }
}
