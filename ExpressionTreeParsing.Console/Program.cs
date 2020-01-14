using System;
using System.Linq;
using ExpressionTreeParsing.Application;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ExpressionTreeParsing.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                using ServiceProvider serviceProvider = new ServiceCollection()
                    .AddLogging(opt =>
                    {
                        opt.SetMinimumLevel(LogLevel.Debug);
                        opt.AddConsole(_ => _.TimestampFormat = "yyyy-MM-ddTHH-mm-ss.fffffffZ ");
                    })
                    .AddSingleton<IExpressionSerializer<Model>, ExpressionSerializer<Model>>()
                    .AddSingleton<Runner>()
                    .BuildServiceProvider();

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

                Runner runner = serviceProvider.GetService<Runner>();

                //runner.Run(
                //    models,
                //    _ => _.SubModels != null && _.SubModels.Any(s => s.Int32Property > 50 || !s.StringProperty.Contains(".1", StringComparison.InvariantCultureIgnoreCase) == false) ? _.StringProperty : _.Int32Property.ToString("X"));

                //runner.Run(
                //    models,
                //    _ => _.SubModels.Sum(s => s.Int32Property));

                runner.Run(
                    models,
                    _ => _.SubModels.Sum(s => ((s.Int32Property | 6) >> 2) << 2) & 6);
            }
            catch (Exception ex)
            {
                System.Console.Error.WriteLine(ex.ToString());
            }
        }
    }
}
