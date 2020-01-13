using System;
using System.Linq;
using System.Linq.Expressions;
using ExpressionTreeParsing.Application;
using ExpressionTreeParsing.Domain;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
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
                IServiceProvider serviceProvider = new ServiceCollection()
                    .AddLogging(opt =>
                    {
                        opt.SetMinimumLevel(LogLevel.Debug);
                        opt.AddConsole(_ => _.TimestampFormat = "yyyy-MM-ddTHH-mm-ss.fffffffZ");
                    })
                    .AddSingleton<IExpressionSerializer<Model>, ExpressionSerializer<Model>>()
                    .AddSingleton<ProgramRunner>()
                    .BuildServiceProvider();

                serviceProvider.GetService<ProgramRunner>().Run();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.ToString());
            }
        }

        private class ProgramRunner
        {
            private readonly IExpressionSerializer<Model> _expressionSerializer;
            private readonly ILogger _logger;

            public ProgramRunner(IExpressionSerializer<Model> expressionSerializer, ILogger<ProgramRunner> logger)
            {
                this._expressionSerializer = expressionSerializer ?? throw new ArgumentNullException(nameof(expressionSerializer));
                this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
            }

            public void Run()
            {
                try
                {
                    Expression<Func<Model, string>> source = _ => _.SubModels != null && _.SubModels.Any(s => s.Int32Property > 50 || !s.StringProperty.Contains(".1", StringComparison.InvariantCultureIgnoreCase) == false) ? _.StringProperty : _.Int32Property.ToString("X");

                    this._logger.LogInformation($"{nameof(source)}: {source}");

                    ParsedExpression serialized = this._expressionSerializer.Serialize(source);
                    string json = JsonConvert.SerializeObject(serialized, __jsonSerializerSettings);

                    this._logger.LogInformation($"{nameof(json)}: {json}");

                    ParsedExpression deserialized = JsonConvert.DeserializeObject<ParsedExpression>(json, __jsonSerializerSettings);
                    Expression<Func<Model, string>> destination = (Expression<Func<Model, string>>)this._expressionSerializer.Deserialize(deserialized);

                    this._logger.LogInformation($"{nameof(destination)}: {destination}");

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

                    this._logger.LogInformation($"{nameof(source)}: {JsonConvert.SerializeObject(models.Select(source).ToArray())}");
                    this._logger.LogInformation($"{nameof(destination)}: {JsonConvert.SerializeObject(models.Select(destination).ToArray())}");
                }
                catch (Exception ex)
                {
                    this._logger.LogError(ex.ToString());
                }
            }
        }
    }
}
