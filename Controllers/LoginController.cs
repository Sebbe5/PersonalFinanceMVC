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

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginVM vm)
        {
            if (!ModelState.IsValid)
                return View();

            var success = await accountService.TryLogin(vm);
            if (!success)
            {
                // Show error
                ModelState.AddModelError(string.Empty, "Login Failed");
                return View();
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

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterVM vm)
        {
            if (!ModelState.IsValid)
                return View();

            // Try to register user
            var errorMessage = await accountService.TryRegister(vm);
            if (errorMessage != null)
            {
                // Show error
                ModelState.AddModelError(string.Empty, errorMessage);
                return View();
            }

            LoginVM loginVM = new LoginVM();
            loginVM.Username = vm.Username;
            loginVM.Password = vm.Password;

            var success = await accountService.TryLogin(loginVM);
            if (!success)
            {
                // Show error
                ModelState.AddModelError(string.Empty, "Login Failed");
                return View();
            }

            return RedirectToAction(nameof(HomeController.Member), nameof(HomeController).Replace("Controller", string.Empty));
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            accountService.LogOut();

            return RedirectToAction(nameof(Login));
        }
    }
}
