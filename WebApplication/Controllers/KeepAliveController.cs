using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    public class KeepAliveController : Controller
    {
        private static bool killSelf = false;
        private static Task killSelfTask = null;

        public IActionResult Index()
        {
            killSelf = false;
            Console.WriteLine("keep alive");

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

            return NoContent();
        }
    }
}