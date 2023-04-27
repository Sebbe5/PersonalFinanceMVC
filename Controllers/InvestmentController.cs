using Microsoft.AspNetCore.Mvc;

namespace PersonalFinanceMVC.Controllers
{
    public class InvestmentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
