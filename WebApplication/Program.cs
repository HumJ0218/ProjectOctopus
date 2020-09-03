using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WebApplication
{
    public class Program
    {
        internal static int HttpPort;

        public static void Main(string[] args)
        {
            HttpPort = int.TryParse(args?.FirstOrDefault(m => m.StartsWith("httpPort="))?.Split('=')?[1], out var __) ? __ : 5000;

            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                File.AppendAllLines($"./crashReport.{DateTime.Now:yyyyMMdd}.log", new string[]
                {
                            DateTime.Now.ToString(),
                            ex.ToString(),
                            ""
                });

                Environment.Exit(-1);
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseUrls($"http://localhost:{HttpPort}").UseStartup<Startup>();
            });
    }
}
