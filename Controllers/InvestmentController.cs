using Microsoft.AspNetCore.Mvc;
using PersonalFinanceMVC.Models;

namespace PersonalFinanceMVC.Controllers
{
    public class InvestmentController : Controller
    {
        InvestmentService investmentService;
        public InvestmentController(InvestmentService dataService)
        {
            this.investmentService = dataService;
        }
        [HttpGet("/investments")]
        public IActionResult Investments()
        {

            return View();
        }
    }
}
