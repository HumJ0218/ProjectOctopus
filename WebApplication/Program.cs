using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WebApplication
{
    public class Program
    {
        internal static bool killSelf = false;
        internal static Task killSelfTask = null;

        public static void Main(string[] args)
        {
            var webTask= Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            }).Build().RunAsync();

            KillSelf();

            webTask.Wait();
        }

        internal static void KillSelf() {

            killSelf = false;
            Console.WriteLine("keep alive");
            Thread.Sleep(10000);

            if (killSelfTask is null)
            {
                killSelfTask = Task.Run(delegate ()
                {
                    while (true)
                    {
                        killSelf = true;
                        Console.WriteLine("wait to kill");

                        Thread.Sleep(1000);
                        if (killSelf)
                        {
                            Console.WriteLine("kill?");
                            Thread.Sleep(1000);

                            if (killSelf)
                            {
                                Console.WriteLine("kill confirmed");
                                Environment.Exit(0);
                            }
                            else
                            {
                                Console.WriteLine("alive");
                            }
                        }
                        else
                        {
                            Console.WriteLine("alive");
                        }
                    }
                });
            }

        }
    }
}