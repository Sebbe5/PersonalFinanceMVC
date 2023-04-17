using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalFinanceMVC.Models;

namespace PersonalFinanceMVC.Controllers
{

    // TODO: Add the ability to store settings to a user. Such as currency...
    
    public class HomeController : Controller
    {
        [Authorize]
        [HttpGet("home")]
        public IActionResult Home()
        {
            return View();
        }

        [HttpGet("colors")]
        public IActionResult Colors()
        {
            return View();
        }
    }
}
