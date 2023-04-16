using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalFinanceMVC.Models;

namespace PersonalFinanceMVC.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        [HttpGet("home")]
        public IActionResult Home()
        {
            return View();
        }
    }
}
