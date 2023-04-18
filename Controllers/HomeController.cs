using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalFinanceMVC.Models;

namespace PersonalFinanceMVC.Controllers
{

    // TODO: Fix proper attributes for all view models
    // TODO: Add a todo function to the site ( a todo list is a part of a structured and bright future)
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
