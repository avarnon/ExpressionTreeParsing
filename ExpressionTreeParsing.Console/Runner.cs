using System;
using System.Linq;
using System.Linq.Expressions;
using ExpressionTreeParsing.Application;
using ExpressionTreeParsing.Domain;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace ExpressionTreeParsing.Console
{
    public class Runner
    {
        private static readonly JsonSerializerSettings __jsonSerializerSettings = new JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Converters =
            {
                new StringEnumConverter(),
                new ParsedExpressionConverter(),
                new ParsedMemberBindingConverter(),
                new ParsedMemberInfoConverter(),
            },
            DefaultValueHandling = DefaultValueHandling.Include,
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        };

        private readonly IExpressionSerializer<Model> _expressionSerializer;
        private readonly ILogger _logger;

        public Runner(IExpressionSerializer<Model> expressionSerializer, ILogger<Runner> logger)
        {
            this._expressionSerializer = expressionSerializer ?? throw new ArgumentNullException(nameof(expressionSerializer));
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Run<TResult>(IQueryable<Model> models, Expression<Func<Model, TResult>> source)
        {
            if (models == null) throw new ArgumentNullException(nameof(models));
            if (source == null) throw new ArgumentNullException(nameof(source));

            try
            {
                string json = null;
                Expression<Func<Model, TResult>> destination = null;

                try
                {
                    ParsedExpression serialized = this._expressionSerializer.Serialize(source);
                    json = JsonConvert.SerializeObject(serialized, __jsonSerializerSettings);

                    ParsedExpression deserialized = JsonConvert.DeserializeObject<ParsedExpression>(json, __jsonSerializerSettings);
                    destination = (Expression<Func<Model, TResult>>)this._expressionSerializer.Deserialize(deserialized);
                }
                finally
                {
                    this._logger.LogInformation($"{nameof(source)}:\t\t{source}");
                    if (destination != null) this._logger.LogInformation($"{nameof(destination)}:\t{destination}");
                    this._logger.LogInformation($"{nameof(json)}:\r\n{json ?? string.Empty}");
                }

                this._logger.LogInformation($"{nameof(source)}:\t\t{JsonConvert.SerializeObject(models.Select(source).ToArray())}");
                this._logger.LogInformation($"{nameof(destination)}:\t{JsonConvert.SerializeObject(models.Select(destination).ToArray())}");
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.ToString());
            }
        }
    }
}
