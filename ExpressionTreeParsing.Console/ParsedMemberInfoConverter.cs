using System;
using ExpressionTreeParsing.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MemberTypes = System.Reflection.MemberTypes;

namespace ExpressionTreeParsing.Console
{
    public class ParsedMemberInfoConverter : JsonConverter
    {
        public override bool CanWrite => false;

        public override bool CanConvert(Type objectType) => objectType == typeof(ParsedMemberInfo);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);
            string memberTypeString = jObject["memberType"].Value<string>();

            if (string.IsNullOrWhiteSpace(memberTypeString) ||
                !Enum.TryParse(memberTypeString, out MemberTypes memberType))
            {
                throw new NotImplementedException();
            }

            switch (memberType)
            {
                case MemberTypes.Field:
                    return jObject.ToObject<ParsedFieldInfo>(serializer);

                case MemberTypes.Property:
                    return jObject.ToObject<ParsedPropertyInfo>(serializer);

                case MemberTypes.Method:
                    return jObject.ToObject<ParsedMethodInfo>(serializer);

                case MemberTypes.Constructor:
                case MemberTypes.Custom:
                case MemberTypes.Event:
                case MemberTypes.NestedType:
                case MemberTypes.TypeInfo:
                default:
                    throw new NotImplementedException();
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) => throw new NotImplementedException();
    }
}
