using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ECommerce.Gateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context,builder)=> {

                var orchestrator = context.Configuration["ORCHESTRATOR"];
                builder.SetBasePath(Directory.GetCurrentDirectory());
                builder.AddJsonFile($"ocelot.json", optional: false);
                builder.AddJsonFile($"ocelot.{context.HostingEnvironment.EnvironmentName}.json", optional: false);
                builder.AddJsonFile($"ocelot.{context.HostingEnvironment.EnvironmentName}.{orchestrator}.json", optional: true);
                builder.AddEnvironmentVariables();   
            })
                .UseStartup<Startup>();
    }
}
