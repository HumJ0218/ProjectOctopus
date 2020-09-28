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
        public IActionResult Index()
        {
            Program.killSelf = false;

            return NoContent();
        }
    }
}