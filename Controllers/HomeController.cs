using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalFinance.Models;

namespace PersonalFinance.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        
        [HttpGet("member")]
        public IActionResult Member()
        {
            return View();
        }

        [HttpGet("budget")]
        public IActionResult Budget()
        {
            return View();
        }
    }
}
