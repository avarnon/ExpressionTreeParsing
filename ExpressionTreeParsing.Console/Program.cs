using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ExpressionTreeParsing.Application;
using ExpressionTreeParsing.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace ExpressionTreeParsing.Console
{
    class Program
    {
        private static readonly JsonSerializerSettings __jsonSerializerSettings = new JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Converters =
            {
                new StringEnumConverter(),
                new ParsedExpressionConverter(),
                new ParsedMemberInfoConverter(),
            },
            DefaultValueHandling = DefaultValueHandling.Include,
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        };

        static void Main(string[] args)
        {
            try
            {
                IExpressionSerializer<Model> expressionSerializer = new ExpressionSerializer<Model>();
                Expression<Func<Model, string>> source = _ => _.SubModels.Any(s => s.Int32Property > 50 || !s.StringProperty.Contains(".1", StringComparison.InvariantCultureIgnoreCase)) ? _.StringProperty : _.Int32Property.ToString("X");

                System.Console.WriteLine(nameof(source));
                System.Console.WriteLine(source.ToString());
                System.Console.WriteLine();

                ParsedExpression serialized = expressionSerializer.Serialize(source);
                string json = JsonConvert.SerializeObject(serialized, __jsonSerializerSettings);

                System.Console.WriteLine(nameof(json));
                System.Console.WriteLine(json);
                System.Console.WriteLine();

                ParsedExpression deserialized = JsonConvert.DeserializeObject<ParsedExpression>(json, __jsonSerializerSettings);
                Expression destination = expressionSerializer.Deserialize(deserialized);

                System.Console.WriteLine(nameof(destination));
                System.Console.WriteLine(destination.ToString());
                System.Console.WriteLine();

                IQueryable<Model> models = Enumerable.Range(0, 10)
                    .Select(i => new Model()
                    {
                        Int32Property = i * 10,
                        StringProperty = i.ToString("X"),
                        SubModels = Enumerable.Range(0, i == 0 ? 0 : i - 1)
                        .Select(j => new Model()
                        {
                            Int32Property = i * 10 + j,
                            StringProperty = $"{i:X}.{j:X}",
                            SubModels = Enumerable.Empty<Model>(),
                        })
                        .ToArray(),
                    })
                    .ToArray()
                    .AsQueryable();

                System.Console.WriteLine(nameof(source));
                System.Console.WriteLine(JsonConvert.SerializeObject(models.Select(source).ToArray()));
                System.Console.WriteLine();

                System.Console.WriteLine(nameof(destination));
                System.Console.WriteLine(JsonConvert.SerializeObject(models.Select(destination as Expression<Func<Model, string>>).ToArray()));
                System.Console.WriteLine();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.ToString());
            }
        }
    }
}
