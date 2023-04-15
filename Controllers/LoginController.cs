using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        [HttpGet("Register")]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction(nameof(HomeController.Member), nameof(HomeController).Replace("Controller", string.Empty));

            return View();
        }

        [HttpPost("")]
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync(RegisterVM vm)
        {
            // Try to register user
            var errorMessage = await accountService.TryRegisterAsync(vm);
            if (errorMessage != null)
            {
                // Show error
                ModelState.AddModelError(string.Empty, errorMessage);
                return View();
            }
            if (!ModelState.IsValid)
                return View();


            return RedirectToAction(nameof(LoginAsync).Replace("Async", string.Empty));
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction(nameof(HomeController.Member), nameof(HomeController).Replace("Controller", string.Empty));

            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginVM vm)
        {
            if (!ModelState.IsValid)
                return View();

            // Try to login user
            var errorMessage = await accountService.TryLoginAsync(vm);
            if (errorMessage != null)
            {
                // Show error
                ModelState.AddModelError(string.Empty, errorMessage);
                return View();
            }

            return RedirectToAction(nameof(HomeController.Member), nameof(HomeController).Replace("Controller", string.Empty));

        }

        [HttpPost("logout")]
        public IActionResult LogOut(LoginVM vm)
        {
            accountService.SignOut();

            return RedirectToAction(nameof(LoginAsync).Replace("Async", string.Empty));
        }

    }
}
