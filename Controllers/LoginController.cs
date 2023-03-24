using Microsoft.AspNetCore.Mvc;

namespace PersonalFinanceMVC.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet("")]
        public IActionResult Login()
        {
            return View();
        }
    }
}
