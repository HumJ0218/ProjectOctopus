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