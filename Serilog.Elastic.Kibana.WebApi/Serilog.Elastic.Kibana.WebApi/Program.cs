using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using System;
using System.Reflection;

namespace Serilog.Elastic.Kibana.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ConfigureSerilogWithElasticSearch();
            CreateHostBuilder(args).Build().Run();
        }

        private static void ConfigureSerilogWithElasticSearch()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            Log.Logger = new LoggerConfiguration()
                            .Enrich.FromLogContext()
                            .Enrich.WithExceptionDetails()
                            .Enrich.WithMachineName()
                            .WriteTo.Debug()
                            .WriteTo.Console()
                            .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(configuration["ElasticSearchUrl"]))
                            {
                                AutoRegisterTemplate = true,
                                IndexFormat = $"Service_{ Assembly.GetExecutingAssembly().GetName().Name }"
                            })
                            .ReadFrom.Configuration(configuration)
                            .CreateLogger();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseSerilog();
    }
}
