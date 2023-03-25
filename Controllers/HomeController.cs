using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalFinance.Models;
using PersonalFinance.Views.Home;

namespace PersonalFinance.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        AccountService accountService;
        public HomeController(AccountService accountService)
        {
            this.accountService = accountService;
        }

        [HttpGet("member")]
        public IActionResult Member()
        {
            return View();
        }

        [HttpGet("budget")]
        public IActionResult Budget()
        {
            return View(accountService.GetUserBudgets());
        }

        [HttpGet("createBudget")]
        public IActionResult CreateBudget()
        {
            return View();
        }


    }
}
