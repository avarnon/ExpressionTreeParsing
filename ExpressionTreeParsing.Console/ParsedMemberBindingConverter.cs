using System;
using ExpressionTreeParsing.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MemberBindingType = System.Linq.Expressions.MemberBindingType;

namespace ExpressionTreeParsing.Console
{
    public class ParsedMemberBindingConverter : JsonConverter
    {
        public override bool CanWrite => false;

        public override bool CanConvert(Type objectType) => objectType == typeof(ParsedMemberBinding);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);
            string bindingTypeString = jObject["bindingType"].Value<string>();

            if (string.IsNullOrWhiteSpace(bindingTypeString) ||
                !Enum.TryParse(bindingTypeString, out MemberBindingType bindingType))
            {
                throw new NotImplementedException();
            }

            switch (bindingType)
            {
                case MemberBindingType.Assignment:
                    return jObject.ToObject<ParsedMemberAssignment>(serializer);

                case MemberBindingType.ListBinding:
                    return jObject.ToObject<ParsedMemberListBinding>(serializer);

                case MemberBindingType.MemberBinding:
                    return jObject.ToObject<ParsedMemberMemberBinding>(serializer);

                default:
                    throw new NotImplementedException();
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) => throw new NotImplementedException();
    }
}
