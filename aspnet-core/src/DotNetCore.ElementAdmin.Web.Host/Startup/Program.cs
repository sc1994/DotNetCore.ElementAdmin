using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace DotNetCore.ElementAdmin.Web.Host.Startup
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            var logger = new LoggerConfiguration()
                               .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9222/"))
                               {
                                   AutoRegisterTemplate = true
                               })
                               .CreateLogger();

            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog(logger, true)
                .Build();
        }
    }
}
