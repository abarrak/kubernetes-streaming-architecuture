using System;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace WebUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                       .ConfigureWebHostDefaults(webBuilder =>
                       {
                           webBuilder.ConfigureKestrel((context, options) =>
                           {
                               options.Listen(IPAddress.Loopback, 9200);
                               options.Limits.MaxRequestBodySize = 52428800;
                           }).UseStartup<Startup>();
                       });
        }
    }
}
