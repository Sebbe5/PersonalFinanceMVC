using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalFinanceMVC.Models;

namespace PersonalFinanceMVC.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        DataService dataService;
        public HomeController(DataService dataService)
        {
            this.dataService = dataService;
        }

        [HttpGet("member")]
        public IActionResult Member()
        {
            return View();
        }
    }
}
