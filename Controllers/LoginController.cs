using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalFinanceMVC.Models;
using PersonalFinanceMVC.Views.Login;

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

        [HttpPost("")]
        public async Task<IActionResult> LoginAsync(LoginVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var errorMessage = await accountService.TryLoginAsync(vm);
            if (errorMessage != null)
            {
                // Show error
                ModelState.AddModelError(string.Empty, errorMessage);
                return View(vm);
            }

            return RedirectToAction(nameof(HomeController.Member), nameof(HomeController).Replace("Controller", string.Empty));
        }

        [HttpGet("Register")]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction(nameof(HomeController.Member), nameof(HomeController).Replace("Controller", string.Empty));

            return View();
        }

        [HttpPost("Register")]
        public IActionResult Register(string hej)
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction(nameof(HomeController.Member), nameof(HomeController).Replace("Controller", string.Empty));

            return View();
        }
    }
}
