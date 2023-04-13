using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalFinanceMVC.Models;

namespace PersonalFinanceMVC.Controllers
{
    public class LoginController : Controller
    {
        AccountService accountService;
        public LoginController(AccountService accountService)
        {
            this.accountService = accountService;
        }

        [HttpGet("")]
        [HttpGet("login")]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction(nameof(HomeController.Member), nameof(HomeController).Replace("Controller", string.Empty));

            return View();
        }

        [HttpGet("Register")]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction(nameof(HomeController.Member), nameof(HomeController).Replace("Controller", string.Empty));

            return View();
        }
    }
}
