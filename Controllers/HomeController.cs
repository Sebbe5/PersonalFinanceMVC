using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalFinanceMVC.Models;

namespace PersonalFinanceMVC.Controllers
{
    // TODO: Make partial views of all the cards
    // TODO: Deploy to azure
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
