using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalFinanceMVC.Models;

namespace PersonalFinanceMVC.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        AccountService accountService;
        DataService dataService;
        public HomeController(AccountService accountService, DataService dataService)
        {
            this.accountService = accountService;
            this.dataService = dataService;
        }

        [HttpGet("member")]
        public IActionResult Member()
        {
            return View();
        }
    }
}
