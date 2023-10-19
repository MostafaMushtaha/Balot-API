using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Context;
using Serilog.Sinks.Loki;

namespace Stack.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()

                .Enrich.WithProperty("app", "myapp")
                .WriteTo.Console()
                .WriteTo.LokiHttp("http://172.20.10.3:3100")
                .Enrich.FromLogContext()
                .CreateLogger();

            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }

            //Configuration for running API using Kestrel  as an excutable program . 
            //  var configuration = new ConfigurationBuilder()
            //.AddCommandLine(args)
            //.Build();
            //  var hostUrl = configuration["hosturl"];
            //  if (string.IsNullOrEmpty(hostUrl))
            //      hostUrl = "http://0.0.0.0:8045";
            //  var host = new WebHostBuilder()
            //      .UseKestrel()
            //      .UseUrls(hostUrl)   // <!-- this 
            //      .UseContentRoot(Directory.GetCurrentDirectory())
            //      .UseIISIntegration()
            //      .UseStartup<Startup>()
            //      .UseConfiguration(configuration)
            //      .Build();
            //  host.Run();

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
          Host.CreateDefaultBuilder(args)
          .UseSerilog()
          .ConfigureWebHostDefaults(webBuilder =>
          {
              webBuilder.UseStartup<Startup>();
              // .UseUrls("http://*:5000");
          });

    }
}
